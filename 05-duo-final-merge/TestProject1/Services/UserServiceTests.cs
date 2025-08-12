using System;
using System.Threading.Tasks;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using DuoClassLibrary.Models;
using Moq;
using Xunit;
using Duo.Services;

namespace TestProject1.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserHelperService> _mockUserHelperService;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserHelperService = new Mock<IUserHelperService>();
            _userService = new UserService(_mockUserHelperService.Object);
        }

        [Fact]
        public async Task SetUser_WithExistingUser_SetsCurrentUser()
        {
            // Arrange
            var username = "testuser";
            var existingUser = new User(1, username);
            _mockUserHelperService.Setup(x => x.GetUserByUsername(username))
                .ReturnsAsync(existingUser);

            // Act
            await _userService.SetUser(username);

            // Assert
            var currentUser = _userService.GetCurrentUser();
            Assert.Equal(existingUser.UserId, currentUser.UserId);
            Assert.Equal(existingUser.UserName, currentUser.UserName);
        }

        [Fact]
        public async Task SetUser_WithNewUser_CreatesAndSetsCurrentUser()
        {
            // Arrange
            var username = "newuser";
            var newUserId = 1;
            _mockUserHelperService.Setup(x => x.GetUserByUsername(username))
                .ReturnsAsync((User)null);
            _mockUserHelperService.Setup(x => x.CreateUser(It.IsAny<User>()))
                .ReturnsAsync(newUserId);

            // Act
            await _userService.SetUser(username);

            // Assert
            var currentUser = _userService.GetCurrentUser();
            Assert.Equal(newUserId, currentUser.UserId);
            Assert.Equal(username, currentUser.UserName);
        }

        [Fact]
        public async Task SetUser_WithEmptyUsername_ThrowsArgumentException()
        {
            // Arrange
            var username = "";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.SetUser(username));
        }

        [Fact]
        public async Task GetUserById_WithValidId_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User(userId, "testuser");
            _mockUserHelperService.Setup(x => x.GetUserById(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.UserName, result.UserName);
        }

        [Fact]
        public async Task GetUserById_WithInvalidId_ThrowsException()
        {
            // Arrange
            var userId = 1;
            _mockUserHelperService.Setup(x => x.GetUserById(userId))
                .ThrowsAsync(new Exception("User not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.GetUserById(userId));
        }

        [Fact]
        public async Task GetUserByUsername_WithValidUsername_ReturnsUser()
        {
            // Arrange
            var username = "testuser";
            var expectedUser = new User(1, username);
            _mockUserHelperService.Setup(x => x.GetUserByUsername(username))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByUsername(username);

            // Assert
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.UserName, result.UserName);
        }

        [Fact]
        public async Task GetUserByUsername_WithInvalidUsername_ReturnsNull()
        {
            // Arrange
            var username = "nonexistent";
            _mockUserHelperService.Setup(x => x.GetUserByUsername(username))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUserByUsername(username);

            // Assert
            Assert.Null(result);
        }
    }
}