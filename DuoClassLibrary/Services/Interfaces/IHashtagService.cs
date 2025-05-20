using DuoClassLibrary.Models;


namespace DuoClassLibrary.Services.Interfaces
{
    public interface IHashtagService
    {
        Task<Hashtag> GetHashtagByText(string textToSearchBy);
        Task<Hashtag> CreateHashtag(string newHashtagTag);
        Task<List<Hashtag>> GetHashtagsByPostId(int postId);
        Task<bool> AddHashtagToPost(int postId, int hashtagId);
        Task<bool> RemoveHashtagFromPost(int postId, int hashtagId);
        Task<Hashtag> GetHashtagByName(string hashtagName);
        Task<List<Hashtag>> GetAllHashtags();
        Task<List<Hashtag>> GetHashtagsByCategory(int categoryId);
    }
}
