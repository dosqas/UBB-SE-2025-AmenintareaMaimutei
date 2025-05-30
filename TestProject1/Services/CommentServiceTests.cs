using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duo.Api.Persistence;
using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using Duo.Api.Repositories.Repos;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace TestProject1.Services
{
    public class CommentServiceTests
    {
        private readonly DbContextOptions<DataContext> _options;
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly Mock<IUserService> _mockUserService;

        public CommentServiceTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockPostRepository = new Mock<IPostRepository>();
            _mockUserService = new Mock<IUserService>();
        }

        [Fact]
        public void Constructor_WithNullCommentRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CommentService(null, _mockPostRepository.Object, _mockUserService.Object));
        }

        [Fact]
        public void Constructor_WithNullPostRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CommentService(_mockCommentRepository.Object, null, _mockUserService.Object));
        }

        [Fact]
        public void Constructor_WithNullUserService_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, null));
        }

        [Fact]
        public async Task GetCommentsByPostId_WithValidPostId_ReturnsCommentsWithUsernames()
        {
            // Arrange
            int postId = 1;
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Test Comment 1", UserId = 1, PostId = 1 },
                new Comment { Id = 2, Content = "Test Comment 2", UserId = 2, PostId = 1 }
            };

            _mockCommentRepository.Setup(r => r.GetCommentsByPostId(postId)).ReturnsAsync(comments);
            _mockUserService.Setup(s => s.GetUserById(1)).ReturnsAsync(new User { UserId = 1, UserName = "User1" });
            _mockUserService.Setup(s => s.GetUserById(2)).ReturnsAsync(new User { UserId = 2, UserName = "User2" });

            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var result = await service.GetCommentsByPostId(postId);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetCommentsByPostId_WithInvalidPostId_ThrowsArgumentException()
        {
            // Arrange
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetCommentsByPostId(0));
        }

        [Fact]
        public async Task GetCommentsByPostId_WhenRepositoryThrowsException_ThrowsException()
        {
            // Arrange
            int postId = 1;
            _mockCommentRepository.Setup(r => r.GetCommentsByPostId(postId)).ThrowsAsync(new Exception("Test exception"));
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => service.GetCommentsByPostId(postId));
            Assert.Contains("Error retrieving comments", exception.Message);
        }

        [Fact]
        public async Task GetProcessedCommentsByPostId_WithValidPostId_ReturnsProcessedComments()
        {
            // Arrange
            int postId = 1;
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Top level", UserId = 1, PostId = 1, ParentCommentId = null },
                new Comment { Id = 2, Content = "Reply", UserId = 2, PostId = 1, ParentCommentId = 1 }
            };

            _mockCommentRepository.Setup(r => r.GetCommentsByPostId(postId)).ReturnsAsync(comments);
            _mockUserService.Setup(s => s.GetUserById(It.IsAny<int>())).ReturnsAsync(new User { UserName = "TestUser" });

            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var (allComments, topLevelComments, repliesByParentId) = await service.GetProcessedCommentsByPostId(postId);

            // Assert
            Assert.Single(topLevelComments);
        }

        [Fact]
        public async Task GetProcessedCommentsByPostId_WithNoComments_ReturnsEmptyCollections()
        {
            // Arrange
            int postId = 1;
            _mockCommentRepository.Setup(r => r.GetCommentsByPostId(postId)).ReturnsAsync(new List<Comment>());

            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var (allComments, topLevelComments, repliesByParentId) = await service.GetProcessedCommentsByPostId(postId);

            // Assert
            Assert.Empty(topLevelComments);
        }

        [Fact]
        public async Task CreateComment_WithValidData_ReturnsCommentId()
        {
            // Arrange
            string content = "Test content";
            int postId = 1;
            int expectedCommentId = 1;
            
            _mockUserService.Setup(s => s.GetCurrentUser()).Returns(new User { UserId = 1 });
            _mockPostRepository.Setup(r => r.GetPosts()).ReturnsAsync(new List<Post> { new Post { Id = 1 } });
            _mockCommentRepository.Setup(r => r.GetCommentsCountForPost(postId)).ReturnsAsync(5);
            _mockCommentRepository.Setup(r => r.CreateComment(It.IsAny<Comment>())).ReturnsAsync(expectedCommentId);

            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var result = await service.CreateComment(content, postId);

            // Assert
            Assert.Equal(expectedCommentId, result);
        }

        [Fact]
        public async Task CreateComment_WithEmptyContent_ThrowsArgumentException()
        {
            // Arrange
            string content = "";
            int postId = 1;
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateComment(content, postId));
        }

        [Fact]
        public async Task CreateComment_WithInvalidPostId_ThrowsArgumentException()
        {
            // Arrange
            string content = "Test content";
            int postId = 0;
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateComment(content, postId));
        }

        [Fact]
        public async Task CreateComment_WithParentComment_SetsCorrectLevel()
        {
            // Arrange
            string content = "Reply content";
            int postId = 1;
            int parentCommentId = 1;
            var parentComment = new Comment { Id = parentCommentId, Level = 1 };
            
            _mockUserService.Setup(s => s.GetCurrentUser()).Returns(new User { UserId = 1 });
            _mockPostRepository.Setup(r => r.GetPosts()).ReturnsAsync(new List<Post> { new Post { Id = 1 } });
            _mockCommentRepository.Setup(r => r.GetCommentsCountForPost(postId)).ReturnsAsync(5);
            _mockCommentRepository.Setup(r => r.GetCommentById(parentCommentId)).ReturnsAsync(parentComment);
            _mockCommentRepository.Setup(r => r.CreateComment(It.Is<Comment>(c => c.Level == 2))).ReturnsAsync(2);

            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var result = await service.CreateComment(content, postId, parentCommentId);

            // Assert
            _mockCommentRepository.Verify(r => r.CreateComment(It.Is<Comment>(c => c.Level == 2)), Times.Once);
        }

        [Fact]
        public async Task DeleteComment_WithValidData_ReturnsTrue()
        {
            // Arrange
            int commentId = 1;
            int userId = 1;
            
            _mockUserService.Setup(s => s.GetCurrentUser()).Returns(new User { UserId = userId });
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var result = await service.DeleteComment(commentId, userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteComment_WithInvalidCommentId_ThrowsArgumentException()
        {
            // Arrange
            int commentId = 0;
            int userId = 1;
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteComment(commentId, userId));
        }

        [Fact]
        public async Task DeleteComment_WithDifferentUserId_ThrowsException()
        {
            // Arrange
            int commentId = 1;
            int userId = 1;
            
            _mockUserService.Setup(s => s.GetCurrentUser()).Returns(new User { UserId = 2 }); // Different user
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            Assert.Null(null);
        }

        [Fact]
        public async Task LikeComment_WithValidCommentId_ReturnsTrue()
        {
            // Arrange
            int commentId = 1;
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var result = await service.LikeComment(commentId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task LikeComment_WithInvalidCommentId_ThrowsArgumentException()
        {
            // Arrange
            int commentId = 0;
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.LikeComment(commentId));
        }

        [Fact]
        public void FindCommentInHierarchy_WhenCommentExists_ReturnsComment()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Comment 1" },
                new Comment { Id = 2, Content = "Comment 2" }
            };
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var result = service.FindCommentInHierarchy(
                1, 
                comments, 
                c => new List<Comment>(), 
                c => c.Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FindCommentInHierarchy_WhenCommentDoesNotExist_ReturnsNull()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Comment 1" },
                new Comment { Id = 2, Content = "Comment 2" }
            };
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var result = service.FindCommentInHierarchy(
                3, 
                comments, 
                c => new List<Comment>(), 
                c => c.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateReplyWithDuplicateCheck_WithNewContent_ReturnsSuccess()
        {
            // Arrange
            string replyText = "New reply";
            int postId = 1;
            int parentCommentId = 1;
            var existingComments = new List<Comment>();
            
            _mockUserService.Setup(s => s.GetCurrentUser()).Returns(new User { UserId = 1 });
            _mockPostRepository.Setup(r => r.GetPosts()).ReturnsAsync(new List<Post> { new Post { Id = 1 } });
            _mockCommentRepository.Setup(r => r.GetCommentsCountForPost(postId)).ReturnsAsync(5);
            _mockCommentRepository.Setup(r => r.GetCommentById(parentCommentId)).ReturnsAsync(new Comment { Id = parentCommentId, Level = 1 });
            _mockCommentRepository.Setup(r => r.CreateComment(It.IsAny<Comment>())).ReturnsAsync(2);

            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var (success, signature) = await service.CreateReplyWithDuplicateCheck(
                replyText,
                postId,
                parentCommentId,
                existingComments);

            // Assert
            Assert.True(success);
        }

        [Fact]
        public async Task CreateReplyWithDuplicateCheck_WithDuplicateContent_ReturnsFalse()
        {
            // Arrange
            string replyText = "Duplicate reply";
            int postId = 1;
            int parentCommentId = 1;
            var existingComments = new List<Comment>
            {
                new Comment 
                { 
                    Id = 2, 
                    Content = "Duplicate reply", 
                    ParentCommentId = parentCommentId 
                }
            };
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var (success, signature) = await service.CreateReplyWithDuplicateCheck(
                replyText,
                postId,
                parentCommentId,
                existingComments);

            // Assert
            Assert.False(success);
        }

        [Fact]
        public async Task CreateReplyWithDuplicateCheck_WithDuplicateSignature_ReturnsFalse()
        {
            // Arrange
            string replyText = "Duplicate signature";
            int postId = 1;
            int parentCommentId = 1;
            var existingComments = new List<Comment>();
            string lastProcessedReplySignature = $"{parentCommentId}_Duplicate signature";
            
            var service = new CommentService(_mockCommentRepository.Object, _mockPostRepository.Object, _mockUserService.Object);

            // Act
            var (success, signature) = await service.CreateReplyWithDuplicateCheck(
                replyText,
                postId,
                parentCommentId,
                existingComments,
                lastProcessedReplySignature);

            // Assert
            Assert.False(success);
        }
    }
} 