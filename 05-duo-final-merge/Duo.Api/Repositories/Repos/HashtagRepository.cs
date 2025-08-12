using Duo.Api.Persistence;
using Duo.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Duo.Api.Models;

namespace Duo.Api.Repositories.Repos
{
    public class HashtagRepository:IHashtagRepository
    {
        DataContext _context;

        public HashtagRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> CreateHashtag(Hashtag hashtag)
        {
            try
            {
                if (hashtag == null)
                {
                    throw new ArgumentNullException(nameof(hashtag), "Hashtag cannot be null.");
                }

                hashtag.Id = 0;

                _context.Hashtags.Add(hashtag);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Created hashtag with ID: {hashtag.Id}");
                return hashtag.Id;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error creating hashtag: {exception.Message}");
                throw;
            }
        }

        public async Task<List<Hashtag>> GetHashtags()
        {
            try
            {
                var hashtags = await _context.Hashtags.ToListAsync();
                Console.WriteLine($"Retrieved {hashtags.Count} posts.");

                return await _context.Hashtags.ToListAsync();
            }
            catch (Exception exception)
            {
                // Log the error
                Console.WriteLine($"Error getting hashtags: {exception.Message}");
                return new List<Hashtag>();
            }
        }

        public async Task AddHashtagToPost(int postId, int hashtagId)
        {
            try
            {
                // Check if post exists
                var post = await _context.Posts.FindAsync(postId);
                if (post == null)
                    throw new Exception($"Post with ID {postId} not found");

                // Check if hashtag already exists
                var existingHashtag = await _context.Hashtags.FirstOrDefaultAsync(current_hashtag => current_hashtag.Id == hashtagId);
                if (existingHashtag == null)
                    throw new Exception($"Hashtag with ID {hashtagId} not found");

                // Check if relation already exists
                var existingRelation = await _context.PostHashtags.FirstOrDefaultAsync(
                    post_hashtag => post_hashtag.PostId == postId && post_hashtag.HashtagId == hashtagId);

                if (existingRelation == null)
                {
                    // Add the relation
                    _context.PostHashtags.Add(new PostHashtags
                    {
                        PostId = postId,
                        HashtagId = hashtagId
                    });
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Added hashtag {hashtagId} to post {postId}");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error adding hashtag to post: {exception.Message}");
                throw;
            }
        }

        public async Task RemoveHashtagFromPost(int postId, int hashtagId)
        {
            try
            {
                // Find the relation
                var relation = await _context.PostHashtags.FirstOrDefaultAsync(
                    post_hashtag => post_hashtag.PostId == postId && post_hashtag.HashtagId == hashtagId);

                if (relation != null)
                {
                    // Remove the relation
                    _context.PostHashtags.Remove(relation);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Removed hashtag {hashtagId} from post {postId}");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error removing hashtag from post: {exception.Message}");
                throw;
            }
        }

        public async Task<List<PostHashtags>> GetAllPostHashtags()
        {
            try
            {
                var postHashtags = await _context.PostHashtags.ToListAsync();
                Console.WriteLine($"Retrieved {postHashtags.Count} post-hashtag associations.");

                return postHashtags;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error getting post hashtags: {exception.Message}");
                return new List<PostHashtags>();
            }
        }

    }
}
