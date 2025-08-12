using Duo.Api.Persistence;
using Duo.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Duo.Api.Models;

namespace Duo.Api.Repositories.Repos
{
    public class PostRepository : IPostRepository
    {
        DataContext _context;

        public PostRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Post>> GetPosts()
        {
            try
            {
                var posts = await _context.Posts.ToListAsync();
                Console.WriteLine($"Retrieved {posts.Count} posts.");

                return await _context.Posts.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error getting categories: {ex.Message}");
                return new List<Post>();
            }
        }
        
        public async Task<int> CreatePost(Post post)
        {
            try
            {
                if (post == null)
                {
                    throw new ArgumentNullException(nameof(post), "Post cannot be null.");
                }
            
                post.Id = 0;
                
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                
                Console.WriteLine($"Created post with ID: {post.Id}");
                return post.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating post: {ex.Message}");
                throw;
            }
        }

        public async Task DeletePost(int id)
        {
            try
            {
                var post = await _context.Posts.FindAsync(id);
                if (post == null)
                {
                    Console.WriteLine($"Post with ID {id} not found for deletion.");
                    return;
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                
                Console.WriteLine($"Deleted post with ID: {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting post: {ex.Message}");
                throw;
            }
        }

        public async Task UpdatePost(Post post)
        {
            try
            {
                if (post == null)
                {
                    throw new ArgumentNullException(nameof(post), "Post cannot be null.");
                }

                var existingPost = await _context.Posts.FindAsync(post.Id);
                if (existingPost == null)
                {
                    Console.WriteLine($"Post with ID {post.Id} not found for update.");
                    return;
                }

                _context.Entry(existingPost).CurrentValues.SetValues(post);
                await _context.SaveChangesAsync();
                
                Console.WriteLine($"Updated post with ID: {post.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating post: {ex.Message}");
                throw;
            }
        }
    }
}
