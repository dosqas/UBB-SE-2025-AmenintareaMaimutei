using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuoClassLibrary.Models;

namespace DuoClassLibrary.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<Comment>> GetCommentsByPostId(int postId);
        Task<(List<Comment> AllComments, List<Comment> TopLevelComments, Dictionary<int, List<Comment>> RepliesByParentId)> GetProcessedCommentsByPostId(int postId);
        Task<int> CreateComment(string content, int postId, int? parentCommentId = null);
        Task<(bool Success, string ReplySignature)> CreateReplyWithDuplicateCheck(
            string replyText, 
            int postId, 
            int parentCommentId, 
            IEnumerable<Comment> existingComments,
            string lastProcessedReplySignature = null);
        Task<bool> DeleteComment(int commentId, int userId);
        Task<bool> LikeComment(int commentId);
        public T FindCommentInHierarchy<T>(
            int commentId, 
            IEnumerable<T> topLevelComments,
            Func<T, IEnumerable<T>> getRepliesFunc,
            Func<T, int> getIdFunc) where T : class;
    }
}
