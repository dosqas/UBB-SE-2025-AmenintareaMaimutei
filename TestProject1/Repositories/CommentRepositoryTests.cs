using Duo.Api.Persistence;
using Duo.Api.Models;
using Duo.Api.Repositories.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1.Repositories
{
    public class CommentRepositoryTests : IDisposable
    {
        private readonly DataContext _mockContext;
        private readonly CommentRepository _repository;
        
        public CommentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _mockContext = new DataContext(options);
            _repository = new CommentRepository(_mockContext);
            
            // Seed test data
            SeedTestData();
        }

        private void SeedTestData()
        {
            // Clear existing data
            _mockContext.Comments.RemoveRange(_mockContext.Comments);
            _mockContext.SaveChanges();

            // Add test comments
            var comments = new List<Comment>
            {
                new Comment 
                { 
                    Id = 1,
                    Content = "Test comment 1",
                    UserId = 1,
                    PostId = 1,
                    ParentCommentId = null,
                    CreatedAt = DateTime.Now,
                    LikeCount = 0,
                    Level = 1
                },
                new Comment 
                { 
                    Id = 2,
                    Content = "Test comment 2",
                    UserId = 1,
                    PostId = 1,
                    ParentCommentId = 1,
                    CreatedAt = DateTime.Now,
                    LikeCount = 0,
                    Level = 2
                }
            };

            _mockContext.Comments.AddRange(comments);
            _mockContext.SaveChanges();
        }

        public void Dispose()
        {
            _mockContext.Dispose();
        }

        [Fact]
        public void Constructor_WithValidContext_CreatesInstance()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            var context = new DataContext(options);
            
            // Act
            var repository = new CommentRepository(context);
            
            // Assert
            Assert.NotNull(repository);
        }

        [Fact]
        public void Constructor_WithNullContext_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CommentRepository(null));
        }

        [Fact]
        public async Task GetCommentById_WithValidId_ReturnsComment()
        {
            // Act
            var comment = await _repository.GetCommentById(1);

            // Assert
            Assert.NotNull(comment);
            Assert.Equal(1, comment.Id);
            Assert.Equal("Test comment 1", comment.Content);
        }

        [Fact]
        public async Task GetCommentById_WithInvalidId_ReturnsNull()
        {
            // Act
            var comment = await _repository.GetCommentById(999);

            // Assert
            Assert.Null(comment);
        }

        [Fact]
        public async Task GetCommentsByPostId_WithValidId_ReturnsComments()
        {
            // Act
            var comments = await _repository.GetCommentsByPostId(1);

            // Assert
            Assert.NotNull(comments);
            Assert.Equal(2, comments.Count);
        }

        [Fact]
        public async Task GetCommentsByPostId_WithInvalidId_ReturnsEmptyList()
        {
            // Act
            var comments = await _repository.GetCommentsByPostId(999);

            // Assert
            Assert.NotNull(comments);
            Assert.Empty(comments);
        }

        [Fact]
        public async Task CreateComment_WithValidComment_ReturnsId()
        {
            // Arrange
            var comment = new Comment
            {
                Content = "New test comment",
                UserId = 1,
                PostId = 1,
                ParentCommentId = null,
                CreatedAt = DateTime.Now,
                LikeCount = 0,
                Level = 1
            };

            // Act
            var result = await _repository.CreateComment(comment);

            // Assert
            Assert.True(result > 0);
            var createdComment = await _mockContext.Comments.FindAsync(result);
            Assert.NotNull(createdComment);
            Assert.Equal(comment.Content, createdComment.Content);
        }

        [Fact]
        public async Task CreateComment_WithNullComment_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.CreateComment(null));
        }

        [Fact]
        public async Task DeleteComment_WithValidId_DeletesComment()
        {
            // Act
            await _repository.DeleteComment(1);

            // Assert
            var deletedComment = await _mockContext.Comments.FindAsync(1);
            Assert.Null(deletedComment);
        }

        [Fact]
        public async Task GetRepliesByCommentId_WithValidId_ReturnsReplies()
        {
            // Act
            var replies = await _repository.GetRepliesByCommentId(1);

            // Assert
            Assert.NotNull(replies);
            Assert.Single(replies);
            Assert.Equal(2, replies[0].Id);
        }

        [Fact]
        public async Task GetRepliesByCommentId_WithInvalidId_ReturnsEmptyList()
        {
            // Act
            var replies = await _repository.GetRepliesByCommentId(999);

            // Assert
            Assert.NotNull(replies);
            Assert.Empty(replies);
        }

        [Fact]
        public async Task IncrementLikeCount_WithValidId_IncrementsCount()
        {
            // Act
            await _repository.IncrementLikeCount(1);

            // Assert
            var updatedComment = await _mockContext.Comments.FindAsync(1);
            Assert.Equal(1, updatedComment.LikeCount);
        }

        [Fact]
        public async Task GetCommentsCountForPost_WithValidId_ReturnsCount()
        {
            // Act
            var count = await _repository.GetCommentsCountForPost(1);

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetCommentsCountForPost_WithInvalidId_ReturnsZero()
        {
            // Act
            var count = await _repository.GetCommentsCountForPost(999);

            // Assert
            Assert.Equal(0, count);
        }
    }
} 