using DuoClassLibrary.Models;

namespace DuoClassLibrary.Repositories.Interfaces
{
    public interface IPostRepository
    {
        public Task<List<Post>> GetPosts();

        public Task<int> CreatePost(Post post);

        public Task DeletePost(int id);

        public Task UpdatePost(Post post);
    }
}
