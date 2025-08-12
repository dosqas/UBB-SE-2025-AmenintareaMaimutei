using Duo.Api.Persistence;
using Duo.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Duo.Api.Models;

namespace Duo.Api.Repositories.Repos
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Comment?> GetCommentById(int commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }

        public async Task<List<Comment>> GetCommentsByPostId(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> CreateComment(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            try
            {
                comment.Id = 0; // Ensure new comment
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return comment.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating comment: {ex.Message}", ex);
            }
        }

        public async Task DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Comment>> GetRepliesByCommentId(int parentCommentId)
        {
            return await _context.Comments
                .Where(c => c.ParentCommentId == parentCommentId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task IncrementLikeCount(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                comment.LikeCount++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCommentsCountForPost(int postId)
        {
            return await _context.Comments.CountAsync(c => c.PostId == postId);
        }
    }
} 