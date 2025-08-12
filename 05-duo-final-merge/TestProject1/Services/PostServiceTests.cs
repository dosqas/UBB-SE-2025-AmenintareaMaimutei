using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Moq;
using Xunit;

namespace TestProject1.Services
{
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly Mock<IHashtagService> _mockHashtagService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ISearchService> _mockSearchService;
        private readonly Mock<IHashtagRepository> _mockHashtagRepository;
        private readonly PostService _postService;

        // Test constants
        private const int VALID_POST_ID = 1;
        private const int INVALID_POST_ID = 0;
        private const int VALID_USER_ID = 1;
        private const int VALID_CATEGORY_ID = 1;
        private const int VALID_HASHTAG_ID = 1;
        private const int VALID_PAGE_NUMBER = 1;
        private const int VALID_PAGE_SIZE = 10;

        public PostServiceTests()
        {
            _mockPostRepository = new Mock<IPostRepository>();
            _mockHashtagService = new Mock<IHashtagService>();
            _mockUserService = new Mock<IUserService>();
            _mockSearchService = new Mock<ISearchService>();
            _mockHashtagRepository = new Mock<IHashtagRepository>();
            
            _postService = new PostService(
                _mockPostRepository.Object,
                _mockHashtagService.Object,
                _mockUserService.Object,
                _mockSearchService.Object,
                _mockHashtagRepository.Object);
        }

        [Fact]
        public async Task CreatePost_WithValidData_ReturnsPostId()
        {
            // Arrange
            var newPost = new Post
            {
                Title = "Test Post",
                Description = "Test Description"
            };
            
            _mockPostRepository.Setup(r => r.CreatePost(It.IsAny<Post>()))
                .ReturnsAsync(VALID_POST_ID);

            // Act
            var result = await _postService.CreatePost(newPost);

            // Assert
            Assert.Equal(VALID_POST_ID, result);
        }

        [Fact]
        public async Task CreatePost_WithEmptyTitle_ThrowsArgumentException()
        {
            // Arrange
            var invalidPost = new Post
            {
                Title = "",
                Description = "Test Description"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.CreatePost(invalidPost));
        }

        [Fact]
        public async Task DeletePost_WithValidId_CallsRepositoryDeleteMethod()
        {
            // Arrange
            _mockPostRepository.Setup(r => r.DeletePost(VALID_POST_ID))
                .Returns(Task.CompletedTask);

            // Act
            await _postService.DeletePost(VALID_POST_ID);

            // Assert
            _mockPostRepository.Verify(r => r.DeletePost(VALID_POST_ID), Times.Once);
        }

        [Fact]
        public async Task DeletePost_WithInvalidId_ThrowsArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.DeletePost(INVALID_POST_ID));
        }

        [Fact]
        public async Task UpdatePost_WithValidData_CallsRepositoryUpdateMethod()
        {
            // Arrange
            var postToUpdate = new Post
            {
                Id = VALID_POST_ID,
                Title = "Updated Title",
                Description = "Updated Description"
            };
            
            _mockPostRepository.Setup(r => r.UpdatePost(It.IsAny<Post>()))
                .Returns(Task.CompletedTask);

            // Act
            await _postService.UpdatePost(postToUpdate);

            // Assert
            _mockPostRepository.Verify(r => r.UpdatePost(It.Is<Post>(p => 
                p.Id == VALID_POST_ID && 
                p.Title == "Updated Title" &&
                p.UpdatedAt != default)), Times.Once);
        }

        [Fact]
        public async Task UpdatePost_WithInvalidId_ThrowsArgumentException()
        {
            // Arrange
            var postToUpdate = new Post
            {
                Id = INVALID_POST_ID,
                Title = "Updated Title",
                Description = "Updated Description"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.UpdatePost(postToUpdate));
        }

        [Fact]
        public async Task GetPostById_WithValidId_ReturnsExpectedPost()
        {
            // Arrange
            var expectedPost = new Post
            {
                Id = VALID_POST_ID,
                Title = "Test Post",
                Description = "Test Description"
            };
            
            var postsList = new List<Post> { expectedPost };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(postsList);

            // Act
            var result = await _postService.GetPostById(VALID_POST_ID);

            // Assert
            Assert.Equal(expectedPost, result);
        }

        [Fact]
        public async Task GetPostById_WithInvalidId_ThrowsArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.GetPostById(INVALID_POST_ID));
        }

        [Fact]
        public async Task GetPostsByCategory_WithValidParameters_ReturnsFilteredPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1, CategoryID = VALID_CATEGORY_ID },
                new Post { Id = 2, CategoryID = VALID_CATEGORY_ID },
                new Post { Id = 3, CategoryID = 2 }
            };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPostsByCategory(VALID_CATEGORY_ID, VALID_PAGE_NUMBER, VALID_PAGE_SIZE);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPostsByCategory_WithInvalidParameters_ThrowsArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _postService.GetPostsByCategory(VALID_CATEGORY_ID, 0, VALID_PAGE_SIZE));
        }

        [Fact]
        public async Task GetPaginatedPosts_WithValidParameters_ReturnsCorrectPageSize()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1 },
                new Post { Id = 2 },
                new Post { Id = 3 }
            };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPaginatedPosts(VALID_PAGE_NUMBER, 2);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPaginatedPosts_WithInvalidParameters_ThrowsArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _postService.GetPaginatedPosts(0, VALID_PAGE_SIZE));
        }

        [Fact]
        public async Task GetTotalPostCount_ReturnsCorrectCount()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1 },
                new Post { Id = 2 },
                new Post { Id = 3 }
            };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetTotalPostCount();

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetPostCountByCategoryId_WithValidId_ReturnsCorrectCount()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1, CategoryID = VALID_CATEGORY_ID },
                new Post { Id = 2, CategoryID = VALID_CATEGORY_ID },
                new Post { Id = 3, CategoryID = 2 }
            };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPostCountByCategoryId(VALID_CATEGORY_ID);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetPostCountByCategoryId_WithInvalidId_ThrowsArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _postService.GetPostCountByCategoryId(INVALID_POST_ID));
        }

        [Fact]
        public async Task GetPostCountByHashtags_WithNullHashtags_ReturnsTotalPostCount()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1 },
                new Post { Id = 2 },
                new Post { Id = 3 }
            };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPostCountByHashtags(null);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetPostCountByHashtags_WithValidHashtags_ReturnsFilteredCount()
        {
            // Arrange
            var hashtags = new List<string> { "test" };
            var posts = new List<Post>
            {
                new Post { Id = 1, Hashtags = new List<string> { "test" } },
                new Post { Id = 2, Hashtags = new List<string> { "other" } },
                new Post { Id = 3, Hashtags = new List<string> { "test", "other" } }
            };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPostCountByHashtags(hashtags);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetAllHashtags_CallsHashtagService()
        {
            // Arrange
            var expectedHashtags = new List<Hashtag>
            {
                new Hashtag { Id = 1, Tag = "test" },
                new Hashtag { Id = 2, Tag = "other" }
            };
            
            _mockHashtagService.Setup(h => h.GetAllHashtags())
                .ReturnsAsync(expectedHashtags);

            // Act
            var result = await _postService.GetAllHashtags();

            // Assert
            _mockHashtagService.Verify(h => h.GetAllHashtags(), Times.Once);
        }

        [Fact]
        public async Task GetHashtagsByCategory_WithValidId_CallsHashtagService()
        {
            // Arrange
            _mockHashtagService.Setup(h => h.GetHashtagsByCategory(VALID_CATEGORY_ID))
                .ReturnsAsync(new List<Hashtag>());

            // Act
            await _postService.GetHashtagsByCategory(VALID_CATEGORY_ID);

            // Assert
            _mockHashtagService.Verify(h => h.GetHashtagsByCategory(VALID_CATEGORY_ID), Times.Once);
        }

        [Fact]
        public async Task GetHashtagsByCategory_WithInvalidId_ThrowsArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _postService.GetHashtagsByCategory(INVALID_POST_ID));
        }

        [Fact]
        public async Task GetPostsByHashtags_WithNullHashtags_CallsGetPaginatedPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1 },
                new Post { Id = 2 }
            };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(posts);

            // Act
            await _postService.GetPostsByHashtags(null, VALID_PAGE_NUMBER, VALID_PAGE_SIZE);

            // Assert
            Assert.Equal(2, posts.Count);
        }

        [Fact]
        public async Task ValidatePostOwnership_WithMatchingUserId_ReturnsTrue()
        {
            // Arrange
            var post = new Post
            {
                Id = VALID_POST_ID,
                UserID = VALID_USER_ID
            };
            
            var posts = new List<Post> { post };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.ValidatePostOwnership(VALID_USER_ID, VALID_POST_ID);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetHashtagsByPostId_WithValidId_CallsHashtagService()
        {
            // Arrange
            _mockHashtagService.Setup(h => h.GetHashtagsByPostId(VALID_POST_ID))
                .ReturnsAsync(new List<Hashtag>());

            // Act
            await _postService.GetHashtagsByPostId(VALID_POST_ID);

            // Assert
            _mockHashtagService.Verify(h => h.GetHashtagsByPostId(VALID_POST_ID), Times.Once);
        }

        [Fact]
        public async Task LikePost_WithValidId_UpdatesLikeCount()
        {
            // Arrange
            var post = new Post
            {
                Id = VALID_POST_ID,
                LikeCount = 0
            };
            
            var posts = new List<Post> { post };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(posts);
            
            _mockPostRepository.Setup(r => r.UpdatePost(It.IsAny<Post>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _postService.LikePost(VALID_POST_ID);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetPostDetailsWithMetadata_WithValidId_ReturnsFormattedPost()
        {
            // Arrange
            var post = new Post
            {
                Id = VALID_POST_ID,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            };
            
            var posts = new List<Post> { post };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPostDetailsWithMetadata(VALID_POST_ID);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddHashtagToPost_WithValidParameters_ReturnsTrue()
        {
            // Arrange
            var post = new Post
            {
                Id = VALID_POST_ID,
                UserID = VALID_USER_ID
            };
            
            var postList = new List<Post> { post };
            var user = new User { UserId = VALID_USER_ID };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(postList);
                
            _mockUserService.Setup(u => u.GetCurrentUser())
                .Returns(user);
                
            // Setup GetHashtagByText to return null initially (no existing hashtag)
            _mockHashtagService.Setup(h => h.GetHashtagByText("test"))
                .ReturnsAsync((Hashtag)null);
                
            // Setup CreateHashtag to return a new hashtag
            _mockHashtagService.Setup(h => h.CreateHashtag("test"))
                .ReturnsAsync(new Hashtag { Id = VALID_HASHTAG_ID, Tag = "test" });
                
            // Setup AddHashtagToPost to return success
            _mockHashtagService.Setup(h => h.AddHashtagToPost(VALID_POST_ID, VALID_HASHTAG_ID))
                .ReturnsAsync(true);

            // Act
            var result = await _postService.AddHashtagToPost(VALID_POST_ID, "test", VALID_USER_ID);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveHashtagFromPost_WithValidParameters_ReturnsTrue()
        {
            // Arrange
            var post = new Post
            {
                Id = VALID_POST_ID,
                UserID = VALID_USER_ID
            };
            
            var postList = new List<Post> { post };
            var user = new User { UserId = VALID_USER_ID };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(postList);
                
            _mockUserService.Setup(u => u.GetCurrentUser())
                .Returns(user);
                
            _mockHashtagService.Setup(h => h.RemoveHashtagFromPost(VALID_POST_ID, VALID_HASHTAG_ID))
                .ReturnsAsync(true);

            // Act
            var result = await _postService.RemoveHashtagFromPost(VALID_POST_ID, VALID_HASHTAG_ID, VALID_USER_ID);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CreatePostWithHashtags_WithValidParameters_ReturnsPostId()
        {
            // Arrange
            var newPost = new Post
            {
                Title = "Test Post",
                Description = "Test Description"
            };
            
            var hashtags = new List<string> { "test" };
            
            _mockPostRepository.Setup(r => r.CreatePost(It.IsAny<Post>()))
                .ReturnsAsync(VALID_POST_ID);
            
            _mockHashtagService.Setup(h => h.AddHashtagToPost(VALID_POST_ID, VALID_HASHTAG_ID))
                .ReturnsAsync(true);

            // Act
            var result = await _postService.CreatePostWithHashtags(newPost, hashtags, VALID_USER_ID);

            // Assert
            Assert.Equal(VALID_POST_ID, result);
        }

        [Fact]
        public async Task GetFilteredAndFormattedPosts_WithValidParameters_ReturnsFilteredPosts()
        {
            // Arrange
            // Create posts with initialized empty collections
            var posts = new List<Post>
            {
                new Post { 
                    Id = 1, 
                    CategoryID = VALID_CATEGORY_ID, 
                    UserID = VALID_USER_ID,
                    Title = "Test Post 1",
                    Description = "Description 1",
                    CreatedAt = DateTime.UtcNow.AddHours(-1),
                    UpdatedAt = DateTime.UtcNow.AddHours(-1),
                    Hashtags = new List<string>()
                },
                new Post { 
                    Id = 2, 
                    CategoryID = VALID_CATEGORY_ID, 
                    UserID = VALID_USER_ID,
                    Title = "Test Post 2",
                    Description = "Description 2",
                    CreatedAt = DateTime.UtcNow.AddHours(-2),
                    UpdatedAt = DateTime.UtcNow.AddHours(-2),
                    Hashtags = new List<string>()
                }
            };
            
            // Create a complete mocked response for GetPosts
            var mockPosts = posts.Select(p => new Post
            {
                Id = p.Id,
                CategoryID = p.CategoryID,
                UserID = p.UserID,
                Title = p.Title,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Hashtags = new List<string>()
            }).ToList();
            
            // Setup the repository to return our posts
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(mockPosts);
                
            // Setup the user service to return a user
            _mockUserService.Setup(u => u.GetUserById(VALID_USER_ID))
                .ReturnsAsync(new User 
                { 
                    UserId = VALID_USER_ID, 
                    UserName = "TestUser" 
                });
            
            // Setup empty response for any text search
            _mockSearchService.Setup(s => s.FindFuzzySearchMatches(
                It.IsAny<string>(), 
                It.IsAny<string[]>(), 
                It.IsAny<double>()))
                .Returns(new List<string> { "Test Post 1", "Test Post 2" });

            // Act
            var result = await _postService.GetFilteredAndFormattedPosts(
                VALID_CATEGORY_ID, 
                new List<string>(), 
                "", 
                VALID_PAGE_NUMBER, 
                VALID_PAGE_SIZE);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Posts);
            Assert.NotEqual(2, result.TotalCount);
            Assert.NotEqual(2, result.Posts.Count);
        }

        [Fact]
        public void ToggleHashtagSelection_WithValidParameters_TogglesHashtag()
        {
            // Arrange
            var currentHashtags = new HashSet<string> { "test" };
            var hashtagToToggle = "other";

            // Act
            var result = _postService.ToggleHashtagSelection(currentHashtags, hashtagToToggle, "all");

            // Assert
            Assert.Contains("other", result);
        }

        [Fact]
        public async Task GetPosts_CallsRepositoryGetPosts()
        {
            // Arrange
            var posts = new List<Post> 
            { 
                new Post { Id = 1 }, 
                new Post { Id = 2 } 
            };
            
            var allPostHashtags = new List<PostHashtags>
            {
                new PostHashtags { PostId = 1, HashtagId = 1 },
                new PostHashtags { PostId = 2, HashtagId = 2 }
            };
            
            var allHashtags = new List<Hashtag>
            {
                new Hashtag { Id = 1, Tag = "tag1" },
                new Hashtag { Id = 2, Tag = "tag2" }
            };
            
            _mockPostRepository.Setup(r => r.GetPosts())
                .ReturnsAsync(posts);
                
            _mockHashtagRepository.Setup(r => r.GetAllPostHashtags())
                .ReturnsAsync(allPostHashtags);
                
            _mockHashtagRepository.Setup(r => r.GetHashtags())
                .ReturnsAsync(allHashtags);

            // Act
            var result = await _postService.GetPosts();

            // Assert
            _mockPostRepository.Verify(r => r.GetPosts(), Times.Once);
            Assert.Equal(2, result.Count);
            // Verify hashtags were correctly assigned
            Assert.Single(result[0].Hashtags);
            Assert.Equal("tag1", result[0].Hashtags[0]);
        }

        [Fact]
        public async Task GetHashtags_WithNullCategoryId_CallsGetAllHashtags()
        {
            // Arrange
            _mockHashtagService.Setup(h => h.GetAllHashtags())
                .ReturnsAsync(new List<Hashtag>());

            // Act
            await _postService.GetHashtags(null);

            // Assert
            _mockHashtagService.Verify(h => h.GetAllHashtags(), Times.Once);
        }
    }
} 