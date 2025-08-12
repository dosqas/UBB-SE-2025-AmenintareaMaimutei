using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Api.Persistence;
using Duo.Api.Models;
using Duo.Api.Repositories.Repos;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TestProject1.Repositories
{
    public class PostRepositoryTests : IDisposable
    {
        private readonly DataContext _mockContext;
        private readonly PostRepository _repository;

        public PostRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mockContext = new DataContext(options);
            _repository = new PostRepository(_mockContext);

            // Seed test data
            SeedTestData();
        }

        private void SeedTestData()
        {
            // Clear existing data
            _mockContext.Posts.RemoveRange(_mockContext.Posts);
            _mockContext.SaveChanges();

            // Add test posts
            var posts = new List<Post>
            {
                new Post 
                { 
                    Id = 1,
                    Title = "Test Post 1",
                    Description = "Description 1",
                    UserID = 1,
                    CategoryID = 1,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    UpdatedAt = DateTime.Now.AddDays(-1),
                    LikeCount = 5
                },
                new Post 
                { 
                    Id = 2,
                    Title = "Test Post 2",
                    Description = "Description 2",
                    UserID = 1,
                    CategoryID = 2,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    UpdatedAt = DateTime.Now,
                    LikeCount = 10
                },
                new Post 
                { 
                    Id = 3,
                    Title = "Test Post 3",
                    Description = "Description 3",
                    UserID = 2,
                    CategoryID = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    LikeCount = 0
                }
            };

            _mockContext.Posts.AddRange(posts);
            _mockContext.SaveChanges();
        }

        public void Dispose()
        {
            _mockContext.Dispose();
        }

        [Fact]
        public void Constructor_WithNullContext_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PostRepository(null));
        }

        [Fact]
        public async Task GetPosts_ReturnsAllPosts()
        {
            // Act
            var posts = await _repository.GetPosts();

            // Assert
            Assert.NotNull(posts);
            Assert.Equal(3, posts.Count);
        }

        [Fact]
        public async Task CreatePost_WithValidPost_ReturnsId()
        {
            // Arrange
            var post = new Post
            {
                Title = "New Post",
                Description = "New Description",
                UserID = 1,
                CategoryID = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                LikeCount = 0
            };

            // Act
            var result = await _repository.CreatePost(post);

            // Assert
            Assert.True(result > 0);
            var createdPost = await _mockContext.Posts.FindAsync(result);
            Assert.NotNull(createdPost);
            Assert.Equal("New Post", createdPost.Title);
        }

        [Fact]
        public async Task CreatePost_WithNullPost_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.CreatePost(null));
        }

        [Fact]
        public async Task DeletePost_WithValidId_RemovesPost()
        {
            // Act
            await _repository.DeletePost(1);

            // Assert
            var deletedPost = await _mockContext.Posts.FindAsync(1);
            Assert.Null(deletedPost);
        }

        [Fact]
        public async Task UpdatePost_WithValidPost_UpdatesPost()
        {
            // Arrange
            var post = await _mockContext.Posts.FindAsync(1);
            post.Title = "Updated Title";
            post.Description = "Updated Description";

            // Act
            await _repository.UpdatePost(post);

            // Assert
            var updatedPost = await _mockContext.Posts.FindAsync(1);
            Assert.NotNull(updatedPost);
            Assert.Equal("Updated Title", updatedPost.Title);
            Assert.Equal("Updated Description", updatedPost.Description);
        }

        [Fact]
        public async Task GetPosts_WithNoPosts_ReturnsEmptyList()
        {
            // Arrange
            _mockContext.Posts.RemoveRange(_mockContext.Posts);
            await _mockContext.SaveChangesAsync();

            // Act
            var posts = await _repository.GetPosts();

            // Assert
            Assert.NotNull(posts);
            Assert.Empty(posts);
        }
    }
}
