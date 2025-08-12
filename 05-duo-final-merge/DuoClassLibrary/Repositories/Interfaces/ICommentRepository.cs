using DuoClassLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DuoClassLibrary.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> GetCommentById(int commentId);
        Task<List<Comment>> GetCommentsByPostId(int postId);
        Task<int> CreateComment(Comment comment);
        Task DeleteComment(int id);
        Task<List<Comment>> GetRepliesByCommentId(int parentCommentId);
        Task IncrementLikeCount(int commentId);
        Task<int> GetCommentsCountForPost(int postId);
    }
} 