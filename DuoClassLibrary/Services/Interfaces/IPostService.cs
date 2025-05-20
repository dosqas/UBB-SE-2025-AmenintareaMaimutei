using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DuoClassLibrary.Models;
using System.Threading.Tasks;

namespace DuoClassLibrary.Services.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetPosts();
        Task<int> CreatePost(Post newPost);
        Task DeletePost(int postId);
        Task UpdatePost(Post postToUpdate);
        Task<Post?> GetPostById(int postId);
        Task<Collection<Post>> GetPostsByCategory(int categoryId, int pageNumber, int pageSize);
        Task<List<Post>> GetPaginatedPosts(int pageNumber, int pageSize);
        Task<int> GetTotalPostCount();
        Task<int> GetPostCountByCategoryId(int categoryId);
        Task<int> GetPostCountByHashtags(List<string> hashtagList);
        Task<List<Hashtag>> GetAllHashtags();
        Task<List<Hashtag>> GetHashtagsByCategory(int categoryId);
        Task<List<Hashtag>> GetHashtags(int? categoryId);
        Task<List<Post>> GetPostsByHashtags(List<string> hashtagList, int pageNumber, int pageSize);
        Task<bool> ValidatePostOwnership(int authorUserId, int targetPostId);
        Task<List<Hashtag>> GetHashtagsByPostId(int postId);
        Task<bool> LikePost(int postId);
        Task<Post?> GetPostDetailsWithMetadata(int postId);
        Task<bool> AddHashtagToPost(int postId, string hashtagName, int userId);
        Task<bool> RemoveHashtagFromPost(int postId, int hashtagId, int userId);
        Task<int> CreatePostWithHashtags(Post newPost, List<string> hashtagList, int authorId);
        Task<(List<Post> Posts, int TotalCount)> GetFilteredAndFormattedPosts(
            int? categoryId,
            List<string> selectedHashtags,
            string filterText,
            int currentPage,
            int itemsPerPage);
        HashSet<string> ToggleHashtagSelection(HashSet<string> currentHashtags, string hashtagToToggle, string allHashtagsFilter);
    }
} 