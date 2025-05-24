using System;
using System.Collections.Generic;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using DuoClassLibrary.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace DuoClassLibrary.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserService _userService;
        private const int MINIMUM_ALLOWED_ID_NUMBER = 0;
        private const int MAXIMUM_COMMENT_COUNT = 1000;
        private const int MAXIMUM_COMMENT_LEVEL = 5;


        public CommentService(ICommentRepository commentRepository, IPostRepository postRepository, IUserService userService)
        {
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<List<Comment>> GetCommentsByPostId(int postId)
        {
            if (postId <= MINIMUM_ALLOWED_ID_NUMBER) throw new ArgumentException("Invalid post ID", nameof(postId));

            try
            {
                var comments = await _commentRepository.GetCommentsByPostId(postId);

                if (comments != null && comments.Count > 0)
                {
                    foreach (var comment in comments)
                    {
                        try
                        {
                            User user = await _userService.GetUserById(comment.UserId);
                            comment.Username = user?.UserName ?? "Unknown User";
                        }
                        catch (Exception)
                        {
                            comment.Username = "Unknown User";
                        }
                    }
                }
                else
                {
                    return new List<Comment>();
                }

                return comments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving comments for post ID {postId}: {ex.Message}", ex);
            }
        }

        public async Task<(List<Comment> AllComments, List<Comment> TopLevelComments, Dictionary<int, List<Comment>> RepliesByParentId)> GetProcessedCommentsByPostId(int postId)
        {
            // Get all comments for the post
            var allComments = await GetCommentsByPostId(postId);
            
            if (allComments == null || !allComments.Any())
            {
                return (new List<Comment>(), new List<Comment>(), new Dictionary<int, List<Comment>>());
            }
            
            // Organize comments into hierarchical structure
            var topLevelComments = allComments.Where(c => c.ParentCommentId == null).ToList();
            
            var repliesByParentId = allComments
                            .Where(c => c.ParentCommentId.HasValue)
                            .GroupBy(c => c.ParentCommentId.Value)
                            .ToDictionary(g => g.Key, g => g.ToList());
                            
            // Set proper level values for all comments
            foreach (var comment in topLevelComments)
            {
                comment.Level = 1;
            }

            foreach (var parentId in repliesByParentId.Keys)
            {
                var parentComment = allComments.FirstOrDefault(c => c.Id == parentId);
                if (parentComment != null)
                {
                    foreach (var reply in repliesByParentId[parentId])
                    {
                        reply.Level = parentComment.Level + 1;
                    }
                }
            }
            
            return (allComments, topLevelComments, repliesByParentId);
        }

        public async Task<int> CreateComment(string content, int postId, int? parentCommentId = null)
        {
            if (postId <= MINIMUM_ALLOWED_ID_NUMBER) throw new ArgumentException("Invalid post ID", nameof(postId));
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Content cannot be empty", nameof(content));

            try
            {
                await ValidateCommentCount(postId);

                int level = 1;
                if (parentCommentId.HasValue)
                {
                    var parentComment = await _commentRepository.GetCommentById(parentCommentId.Value);
                    if (parentComment == null) throw new Exception("Parent comment not found");
                    if (parentComment.Level >= MAXIMUM_COMMENT_LEVEL) throw new Exception("Comment nesting limit reached");
                    level = parentComment.Level + 1;
                }

                User user = _userService.GetCurrentUser();


                // Create the comment without relying on UserService
                var comment = new Comment
                {
                    Content = content,
                    PostId = postId,
                    UserId = user.UserId,
                    ParentCommentId = parentCommentId,
                    CreatedAt = DateTime.Now,
                    Level = level
                };

                return await _commentRepository.CreateComment(comment);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating comment: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteComment(int commentId, int userId)
        {
            if (commentId <= MINIMUM_ALLOWED_ID_NUMBER) throw new ArgumentException("Invalid comment ID", nameof(commentId));
            if (userId <= MINIMUM_ALLOWED_ID_NUMBER) throw new ArgumentException("Invalid user ID", nameof(userId));

            try
            {
                await _commentRepository.DeleteComment(commentId);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting comment with ID {commentId}: {ex.Message}", ex);
            }
        }

        public async Task<bool> LikeComment(int commentId)
        {
            if (commentId <= MINIMUM_ALLOWED_ID_NUMBER) throw new ArgumentException("Invalid comment ID", nameof(commentId));

            try
            {
                await _commentRepository.IncrementLikeCount(commentId);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error liking comment with ID {commentId}: {ex.Message}", ex);
            }
        }


        public T FindCommentInHierarchy<T>(
            int commentId, 
            IEnumerable<T> topLevelComments,
            Func<T, IEnumerable<T>> getRepliesFunc,
            Func<T, int> getIdFunc) where T : class
        {
            if (topLevelComments == null)
                return null;

            // First, check top-level comments
            var comment = topLevelComments.FirstOrDefault(c => getIdFunc(c) == commentId);
            if (comment != null)
            {
                return comment;
            }
            
            // If not found in top-level, recursively search through replies
            foreach (var topLevelComment in topLevelComments)
            {
                var replies = getRepliesFunc(topLevelComment);
                var foundInReplies = FindCommentInRepliesRecursive(commentId, replies, getRepliesFunc, getIdFunc);
                if (foundInReplies != null)
                {
                    return foundInReplies;
                }
            }
            
            return null;
        }


        private T FindCommentInRepliesRecursive<T>(
            int commentId, 
            IEnumerable<T> replies,
            Func<T, IEnumerable<T>> getRepliesFunc,
            Func<T, int> getIdFunc) where T : class
        {
            if (replies == null)
                return null;
                
            foreach (var reply in replies)
            {
                if (getIdFunc(reply) == commentId)
                    return reply;
                    
                var foundInNestedReplies = FindCommentInRepliesRecursive(commentId, getRepliesFunc(reply), getRepliesFunc, getIdFunc);
                if (foundInNestedReplies != null)
                    return foundInNestedReplies;
            }
            
            return null;
        }

        private async Task ValidateCommentCount(int postId)
        {
            try
            {
                var posts = await _postRepository.GetPosts();
                var post = posts.FirstOrDefault(p => p.Id == postId);
                
                if (post == null) throw new Exception("Post not found");

                var commentCount = await _commentRepository.GetCommentsCountForPost(postId);
                if (commentCount >= MAXIMUM_COMMENT_COUNT) throw new Exception("Comment limit reached");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error validating comment count: {ex.Message}", ex);
            }
        }

        public async Task<(bool Success, string ReplySignature)> CreateReplyWithDuplicateCheck(
            string replyText, 
            int postId, 
            int parentCommentId, 
            IEnumerable<Comment> existingComments,
            string lastProcessedReplySignature = null)
        {

            // Create a unique signature for this reply
            string replySignature = $"{parentCommentId}_{replyText}";

            // Check if this is a duplicate of the last processed reply
            if (lastProcessedReplySignature == replySignature)
            {
                return (false, replySignature);
            }

            // Check for duplicate comments in the existing comments
            bool isDuplicate = existingComments != null && existingComments.Any(comment => 
                comment.ParentCommentId == parentCommentId && 
                comment.Content.Equals(replyText, StringComparison.OrdinalIgnoreCase));

            if (isDuplicate)
            {
                return (false, replySignature);
            }

            // Create the comment/reply
            int commentId = await CreateComment(replyText, postId, parentCommentId);
            
            return (commentId > 0, replySignature);
        }

    }
}