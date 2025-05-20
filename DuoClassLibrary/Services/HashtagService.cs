using DuoClassLibrary.Services.Interfaces;
using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;

namespace DuoClassLibrary.Services
{
    public class HashtagService : IHashtagService
    {
        private readonly IHashtagRepository _hashtagRepository;
        private readonly IPostRepository _postRepository;

        public HashtagService(IHashtagRepository hashtagRepository, IPostRepository postRepository)
        {
            _hashtagRepository = hashtagRepository;
            _postRepository = postRepository;
        }

        public async Task<Hashtag> CreateHashtag(string newHashtagTag)
        {
            if (string.IsNullOrWhiteSpace(newHashtagTag)) throw new Exception("Error - CreateHashtag: Text cannot be null or empty");

            try
            {
                Hashtag newHashtag = new Hashtag(newHashtagTag);
                newHashtag.Id = await _hashtagRepository.CreateHashtag(newHashtag);
                return newHashtag;
            }
            catch (Exception caughtException)
            {
                throw new Exception($"Error - CreateHashtag: {caughtException.Message}");
            }
        }

        public async Task<List<Hashtag>> GetAllHashtags()
        {
            try
            {
                return await _hashtagRepository.GetHashtags();
            }
            catch (Exception caughtException) {
                throw new Exception($"Error - GetAllHashtag: {caughtException.Message}");
            }
        }

        public async Task<Hashtag> GetHashtagByName(string hashtagName)
        {
            
            try
            {
                var hashtags = await _hashtagRepository.GetHashtags();
                return hashtags.Where(hashtag =>hashtag.Tag == hashtagName).ToList().First();
            }
            catch (Exception caughtException)
            {
                throw new Exception($"Error - GetHashtagByName: {caughtException.Message}");
            }
        }

        public async Task<Hashtag> GetHashtagByText(string textToSearchBy)
        {
            try
            {
                var hashtags = await _hashtagRepository.GetHashtags();
                return hashtags.Where(hashtag => hashtag.Tag.Contains(textToSearchBy)).ToList().First();
            }
            catch (Exception caughtException)
            {
                throw new Exception($"Error - GetHashtagByName: {caughtException.Message}");
            }
        }

        public async Task<List<Hashtag>> GetHashtagsByCategory(int categoryId)
        {
            try
            {
                var hashtags = await _hashtagRepository.GetHashtags();
                var postHashtags = await _hashtagRepository.GetAllPostHashtags();
                var posts = await _postRepository.GetPosts();
                var postsWithCategory = posts.Where(post => post.CategoryID == categoryId).ToList();
                var postHashtagsWithCategory = postHashtags.
                    Where(post_hashtag => postsWithCategory.Any
                    (post_with_category => post_hashtag.PostId == post_with_category.Id))
                    .ToList();
                return hashtags
                    .Where(hashtag => postHashtagsWithCategory.Any
                        (post_hashtag => post_hashtag.HashtagId == hashtag.Id))
                    .ToList();
            }
            catch (Exception caughtException)
            {
                throw new Exception($"Error - GetHashtagsByCategory: {caughtException.Message}");
            }
        }

        public async Task<List<Hashtag>> GetHashtagsByPostId(int postId)
        {
            try
            {
                var hashtags = await _hashtagRepository.GetHashtags();
                var postHashtags = await _hashtagRepository.GetAllPostHashtags();
                return hashtags
                    .Where(hashtag => postHashtags.Any
                        (post_hashtag => post_hashtag.PostId == postId && post_hashtag.HashtagId == hashtag.Id))
                    .ToList();
            }
            catch (Exception caughtException)
            {
                throw new Exception($"Error - GetHashtagsByPostId: {caughtException.Message}");
            }
        }

        public async Task<bool> RemoveHashtagFromPost(int postId, int hashtagId)
        {
            try
            {
                await _hashtagRepository.RemoveHashtagFromPost(postId, hashtagId);
                return true;
            }
            catch (Exception caughtException)
            {
                throw new Exception($"Error - RemoveHashtagFromPost: {caughtException.Message}");
            }

        }

        public async Task<bool> AddHashtagToPost(int postId, int hashtagId)
        {
            try
            {
                await _hashtagRepository.AddHashtagToPost(postId, hashtagId);
                return true;
            }
            catch (Exception caughtException)
            {
                throw new Exception($"Error - AddHashtagToPost: {caughtException.Message}");
            }

        }

    }
}
