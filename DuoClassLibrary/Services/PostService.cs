using System.Collections.ObjectModel;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services.Interfaces;
using DuoClassLibrary.Repositories.Interfaces;

namespace DuoClassLibrary.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IHashtagService _hashtagService;
        private readonly IUserService _userService;
        private readonly ISearchService _searchService;
        private readonly IHashtagRepository _hashtagRepository;
        private const double FUZZY_SEARCH_SCORE_DEFAULT_THRESHOLD = 0.6;
        
        // Constants for validation
        private const int INVALID_ID = 0;
        private const int MIN_PAGE_NUMBER = 1;
        private const int MIN_PAGE_SIZE = 1;
        private const int DEFAULT_COUNT = 0;
        private const int DEFAULT_PAGE_NUMBER = 1;

        public PostService(IPostRepository postRepository, IHashtagService hashtagService, IUserService userService, ISearchService searchService, IHashtagRepository hashtagRepository)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _hashtagService = hashtagService ?? throw new ArgumentNullException(nameof(hashtagService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
            _hashtagRepository = hashtagRepository ?? throw new ArgumentNullException(nameof(hashtagRepository));
        }

        public async Task<int> CreatePost(Post newPost)
        {
            if (string.IsNullOrWhiteSpace(newPost.Title) || string.IsNullOrWhiteSpace(newPost.Description))
            {
                throw new ArgumentException("Title and Description cannot be empty.");
            }

            try
            {
                return await _postRepository.CreatePost(newPost);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating post: {ex.Message}");
            }
        }

        public async Task DeletePost(int postId)
        {
            if (postId <= INVALID_ID)
            {
                throw new ArgumentException("Invalid Post ID.");
            }

            try
            {
                await _postRepository.DeletePost(postId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting post with ID {postId}: {ex.Message}");
            }
        }

        public async Task UpdatePost(Post postToUpdate)
        {
            if (postToUpdate.Id <= INVALID_ID)
            {
                throw new ArgumentException("Invalid Post ID.");
            }

            try
            {
                postToUpdate.UpdatedAt = DateTime.UtcNow;
                await _postRepository.UpdatePost(postToUpdate);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating post with ID {postToUpdate.Id}: {ex.Message}");
            }
        }

        public async Task<Post?> GetPostById(int postId)
        {
            if (postId <= INVALID_ID)
            {
                throw new ArgumentException("Invalid Post ID.");
            }

            try
            {
                var allPosts = await _postRepository.GetPosts();
                return allPosts.FirstOrDefault(p => p.Id == postId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving post with ID {postId}: {ex.Message}");
            }
        }

        public async Task<Collection<Post>> GetPostsByCategory(int categoryId, int pageNumber, int pageSize)
        {
            if (categoryId <= INVALID_ID || pageNumber < MIN_PAGE_NUMBER || pageSize < MIN_PAGE_SIZE)
            {
                throw new ArgumentException("Invalid pagination parameters.");
            }

            try
            {
                var allPosts = await _postRepository.GetPosts();
                var filtered = allPosts
                    .Where(p => p.CategoryID == categoryId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                return new Collection<Post>(filtered);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving posts for category {categoryId}: {ex.Message}");
            }
        }

        public async Task<List<Post>> GetPaginatedPosts(int pageNumber, int pageSize)
        {
            if (pageNumber < MIN_PAGE_NUMBER || pageSize < MIN_PAGE_SIZE)
            {
                throw new ArgumentException("Invalid pagination parameters.");
            }

            try
            {
                var allPosts = await _postRepository.GetPosts();
                return allPosts
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving paginated posts: {ex.Message}");
            }
        }

        public async Task<int> GetTotalPostCount()
        {
            try
            {
                var allPosts = await _postRepository.GetPosts();
                return allPosts.Count;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving total post count: {ex.Message}");
            }
        }

        public async Task<int> GetPostCountByCategoryId(int categoryId)
        {
            if (categoryId <= INVALID_ID)
            {
                throw new ArgumentException("Invalid Category ID.");
            }

            try
            {
                var allPosts = await _postRepository.GetPosts();
                return allPosts.Count(p => p.CategoryID == categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving post count for category {categoryId}: {ex.Message}");
            }
        }

        public async Task<int> GetPostCountByHashtags(List<string> hashtagList)
        {
            if (hashtagList == null || hashtagList.Count == DEFAULT_COUNT)
            {
                return await GetTotalPostCount();
            }

            List<string> filteredHashtags = hashtagList.Where(h => !string.IsNullOrWhiteSpace(h)).ToList();
            if (filteredHashtags.Count == DEFAULT_COUNT)
            {
                return await GetTotalPostCount();
            }

            try
            {
                var allPosts = await _postRepository.GetPosts();
                return allPosts.Count(p => p.Hashtags != null && p.Hashtags.Any(h => filteredHashtags.Contains(h)));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving post count for hashtags: {ex.Message}");
            }
        }

        public async Task<List<Hashtag>> GetAllHashtags()
        {
            try
            {
                return await _hashtagService.GetAllHashtags();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all hashtags: {ex.Message}");
            }
        }

        public async Task<List<Hashtag>> GetHashtagsByCategory(int categoryId)
        {
            if (categoryId <= INVALID_ID)
            {
                throw new ArgumentException("Invalid Category ID.");
            }

            try
            {
                return await _hashtagService.GetHashtagsByCategory(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving hashtags for category {categoryId}: {ex.Message}");
            }
        }

        public async Task<List<Post>> GetPostsByHashtags(List<string> hashtagList, int pageNumber, int pageSize)
        {
            if (pageNumber < MIN_PAGE_NUMBER || pageSize < MIN_PAGE_SIZE)
            {
                throw new ArgumentException("Invalid pagination parameters.");
            }

            if (hashtagList == null || hashtagList.Count == DEFAULT_COUNT)
            {
                return await GetPaginatedPosts(pageNumber, pageSize);
            }

            List<string> filteredHashtags = hashtagList.Where(h => !string.IsNullOrWhiteSpace(h)).ToList();
            if (filteredHashtags.Count == DEFAULT_COUNT)
            {
                return await GetPaginatedPosts(pageNumber, pageSize);
            }

            try
            {
                var allPosts = await _postRepository.GetPosts();
                var filtered = allPosts
                    .Where(p => p.Hashtags != null && p.Hashtags.Any(h => filteredHashtags.Contains(h)))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                return filtered;
            }
            catch (Exception ex)
            {
                return await GetPaginatedPosts(pageNumber, pageSize);
            }
        }

        public async Task<bool> ValidatePostOwnership(int authorUserId, int targetPostId)
        {
            var post = await GetPostById(targetPostId);
            return post != null && authorUserId == post.UserID;
        }

        public async Task<List<Hashtag>> GetHashtagsByPostId(int postId)
        {
            if (postId <= INVALID_ID) throw new ArgumentException("Invalid Post ID.");

            try
            {
                return await _hashtagService.GetHashtagsByPostId(postId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving hashtags for post with ID {postId}: {ex.Message}");
            }
        }

        public async Task<bool> LikePost(int postId)
        {
            if (postId <= INVALID_ID) throw new ArgumentException("Invalid Post ID.");

            try
            {
                var targetPost = await GetPostById(postId);
                if (targetPost == null) throw new Exception("Post not found");

                targetPost.LikeCount++;

                await UpdatePost(targetPost);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error liking post with ID {postId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a post by ID with all associated metadata (user info, formatted date, hashtags)
        /// </summary>
        /// <param name="postId">The ID of the post to retrieve</param>
        /// <returns>Post with user, date, and hashtag information populated</returns>
        public async Task<Post?> GetPostDetailsWithMetadata(int postId)
        {
            if (postId <= INVALID_ID)
            {
                throw new ArgumentException("Invalid post ID", nameof(postId));
            }

            // Get the basic post
            var post = await GetPostById(postId);
            if (post == null)
            {
                return null;
            }

            // Ensure post ID is set correctly
            if (post.Id <= 0)
            {
                post.Id = postId;
            }

            // Ensure hashtags list exists
            if (post.Hashtags == null)
            {
                post.Hashtags = new List<string>();
            }

            // Get and set user information
            try 
            {
                var user = await _userService.GetUserById(post.UserID);
                post.Username = $"{user?.UserName ?? "Unknown User"}";
            }
            catch (Exception)
            {
                post.Username = "Unknown User";
            }

            // Format the created date
            if (string.IsNullOrEmpty(post.Date) && post.CreatedAt != default)
            {
                DateTime localCreatedAt = Helpers.DateTimeHelper.ConvertUtcToLocal(post.CreatedAt);
                post.Date = FormatDate(localCreatedAt);
            }

            // Get hashtags for the post
            var hashtags = await GetHashtagsByPostId(post.Id);
            if (hashtags != null && hashtags.Any())
            {
                post.Hashtags = hashtags.Select(h => h.Tag ?? h.Tag).ToList();
            }
            
            return post;
        }

        private string FormatDate(DateTime date)
        {
            return date.ToString("MMM dd, yyyy HH:mm");
        }

        public async Task<bool> AddHashtagToPost(int postId, string hashtagName, int userId)
        {
            if (postId <= INVALID_ID)
            {
                throw new ArgumentException("Invalid Post ID.");
            }
            
            if (string.IsNullOrWhiteSpace(hashtagName))
            {
                throw new ArgumentException("Tag name cannot be empty.");
            }
            
            if (userId <= INVALID_ID)
            {
                throw new ArgumentException("Invalid User ID.");
            }

            try
            {
                var targetPost = await GetPostById(postId);
                if (targetPost == null)
                {
                    throw new Exception($"Post with ID {postId} not found");
                }
                
                if (_userService.GetCurrentUser().UserId != userId)
                {
                    throw new Exception("User does not have permission to add hashtags to this post.");
                }

                Hashtag? existingHashtag = null;
                existingHashtag = await _hashtagService.GetHashtagByText(hashtagName);

                Hashtag hashtag = await _hashtagService.CreateHashtag(hashtagName);

                bool addResult = await _hashtagService.AddHashtagToPost(postId, hashtag.Id);
                return addResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding hashtag to post with ID {postId}: {ex.Message}");
            }
        }

        public async Task<bool> RemoveHashtagFromPost(int postId, int hashtagId, int userId)
        {
            if (postId <= INVALID_ID) throw new ArgumentException("Invalid Post ID.");
            if (hashtagId <= INVALID_ID) throw new ArgumentException("Invalid Hashtag ID.");
            if (userId <= INVALID_ID) throw new ArgumentException("Invalid User ID.");

            try
            {
                if (_userService.GetCurrentUser().UserId != userId)
                    throw new Exception("User does not have permission to remove hashtags from this post.");

                return await _hashtagService.RemoveHashtagFromPost(postId, hashtagId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing hashtag from post with ID {postId}: {ex.Message}");
            }
        }

        public async Task<int> CreatePostWithHashtags(Post newPost, List<string> hashtagList, int authorId)
        {
            if (string.IsNullOrWhiteSpace(newPost.Title) || string.IsNullOrWhiteSpace(newPost.Description))
            {
                throw new ArgumentException("Title and Description cannot be empty.");
            }

            try
            {
                int createdPostId = await CreatePost(newPost);
                
                if (createdPostId <= INVALID_ID)
                {
                    throw new Exception("Failed to create post: Invalid post ID returned from database");
                }
                
                try
                {
                    var createdPost = await GetPostById(createdPostId);
                }
                catch (Exception ex)
                {
                }
                
                if (hashtagList != null && hashtagList.Count > DEFAULT_COUNT)
                {    
                    foreach (var hashtagName in hashtagList)
                    {
                        try
                        {
                            Hashtag? existingHashtag = await _hashtagService.GetHashtagByText(hashtagName);
                            Hashtag hashtag = await _hashtagService.CreateHashtag(hashtagName);
                            
                            bool addSuccess = await _hashtagService.AddHashtagToPost(createdPostId, hashtag.Id);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                
                return createdPostId;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                }
                throw new Exception($"Error creating post with hashtags: {ex.Message}", ex);
            }
        }

        public async Task<List<Hashtag>> GetHashtags(int? categoryId)
        {
            if(categoryId == null || categoryId <= INVALID_ID)
                    return await GetAllHashtags();
            return await GetHashtagsByCategory(categoryId.Value);
        }

        public async Task<(List<Post> Posts, int TotalCount)> GetFilteredAndFormattedPosts(
            int? categoryId,
            List<string> selectedHashtags,
            string filterText,
            int currentPage,
            int itemsPerPage)
        {
            if (currentPage < MIN_PAGE_NUMBER || itemsPerPage < MIN_PAGE_SIZE)
            {
                throw new ArgumentException("Invalid pagination parameters.");
            }

            try
            {
                var allPosts = await this.GetPosts();
                IEnumerable<Post> filteredPosts = allPosts;
                int totalCount;

                // Filter by category first if needed
                if (categoryId.HasValue && categoryId.Value > INVALID_ID)
                {
                    filteredPosts = filteredPosts.Where(p => p.CategoryID == categoryId.Value);
                }

                // Filter by hashtags if needed and not "All"
                if (selectedHashtags.Count > DEFAULT_COUNT && !selectedHashtags.Contains("All"))
                {
                    filteredPosts = filteredPosts.Where(p => 
                        p.Hashtags != null && 
                        p.Hashtags.Any(h => selectedHashtags.Contains(h)));
                }

                // Apply text filtering if needed
                if (!string.IsNullOrEmpty(filterText))
                {
                    var searchResults = new List<Post>();
                    foreach (var post in filteredPosts)
                    {
                        if (_searchService.FindFuzzySearchMatches(filterText, new[] { post.Title }).Any())
                        {
                            searchResults.Add(post);
                        }
                    }
                    filteredPosts = searchResults;
                }

                totalCount = filteredPosts.Count();

                // Apply pagination
                filteredPosts = filteredPosts
                    .Skip((currentPage - 1) * itemsPerPage)
                    .Take(itemsPerPage);

                var resultPosts = new List<Post>();
                foreach (var post in filteredPosts)
                {
                    if (string.IsNullOrEmpty(post.Username))
                    {
                        var postAuthor = await _userService.GetUserById(post.UserID);
                        post.Username = postAuthor?.UserName ?? "Unknown User";
                    }

                    DateTime localCreatedAt = Helpers.DateTimeHelper.ConvertUtcToLocal(post.CreatedAt);
                    post.Date = Helpers.DateTimeHelper.GetRelativeTime(localCreatedAt);

                    resultPosts.Add(post);
                }

                return (resultPosts, totalCount);
            }
            catch (Exception ex)
            {
                return (new List<Post>(), 0);
            }
        }

        public HashSet<string> ToggleHashtagSelection(HashSet<string> currentHashtags, string hashtagToToggle, string allHashtagsFilter)
        {
            if (string.IsNullOrEmpty(hashtagToToggle)) return currentHashtags;

            var updatedHashtags = new HashSet<string>(currentHashtags);

            if (hashtagToToggle == allHashtagsFilter)
            {
                updatedHashtags.Clear();
                updatedHashtags.Add(allHashtagsFilter);
            }
            else
            {
                if (updatedHashtags.Contains(hashtagToToggle))
                {
                    updatedHashtags.Remove(hashtagToToggle);

                    if (updatedHashtags.Count == DEFAULT_COUNT)
                    {
                        updatedHashtags.Add(allHashtagsFilter);
                    }
                }
                else
                {
                    updatedHashtags.Add(hashtagToToggle);

                    if (updatedHashtags.Contains(allHashtagsFilter))
                    {
                        updatedHashtags.Remove(allHashtagsFilter);
                    }
                }
            }

            return updatedHashtags;
        }

        public async Task<List<Post>> GetPosts()
        {
            var posts = await _postRepository.GetPosts();
            var allPostHashtags = await _hashtagRepository.GetAllPostHashtags();
            var allHashtags = await _hashtagRepository.GetHashtags();

            // Create a dictionary for quick hashtag lookup
            var hashtagDict = allHashtags.ToDictionary(h => h.Id, h => h.Tag);

            // Group post-hashtag relationships by post ID
            var postHashtagsDict = allPostHashtags
                .GroupBy(ph => ph.PostId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(ph => hashtagDict[ph.HashtagId]).ToList()
                );

            // Assign hashtags to posts
            foreach (var post in posts)
            {
                if (postHashtagsDict.TryGetValue(post.Id, out var hashtags))
                {
                    post.Hashtags = hashtags;
                }
                else
                {
                    post.Hashtags = new List<string>();
                }
            }

            return posts;
        }
    }
}