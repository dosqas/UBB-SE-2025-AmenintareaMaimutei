using Duo.Api.Models;

namespace Duo.Api.Repositories.Interfaces
{
    public interface IHashtagRepository
    {
        public Task<List<Hashtag>> GetHashtags();

        public Task<int> CreateHashtag(Hashtag hashtag);
        Task AddHashtagToPost(int postId, int hashtag);
        Task RemoveHashtagFromPost(int postId, int hashtagId);
        Task<List<PostHashtags>> GetAllPostHashtags();
    }
}