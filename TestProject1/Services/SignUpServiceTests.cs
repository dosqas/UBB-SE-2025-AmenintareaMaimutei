using System;
using System.Threading.Tasks;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Moq;
using Xunit;

namespace TestsDuo2.Services
{
    public class SignUpServiceTests
    {
        private readonly Mock<IUserHelperService> _mockUserHelperService;
        private readonly SignUpService _signUpService;
        
        public SignUpServiceTests()
        {
            _mockUserHelperService = new Mock<IUserHelperService>();
            _signUpService = new SignUpService(_mockUserHelperService.Object);
        }
        
        [Fact]
        public void Constructor_WithNullUserHelperService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SignUpService(null));
        }
        
        [Fact]
        public async Task IsUsernameTaken_WhenUserExists_ReturnsTrue()
        {
            // Arrange
            string username = "existinguser";
            var existingUser = new User { UserId = 1, UserName = username };
            _mockUserHelperService.Setup(s => s.GetUserByUsername(username)).ReturnsAsync(existingUser);
            
            // Act
            bool result = await _signUpService.IsUsernameTaken(username);
            
            // Assert
            Assert.True(result);
            _mockUserHelperService.Verify(s => s.GetUserByUsername(username), Times.Once);
        }
        
        [Fact]
        public async Task IsUsernameTaken_WhenUserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            string username = "newuser";
            _mockUserHelperService.Setup(s => s.GetUserByUsername(username)).ReturnsAsync((User)null);
            
            // Act
            bool result = await _signUpService.IsUsernameTaken(username);
            
            // Assert
            Assert.False(result);
            _mockUserHelperService.Verify(s => s.GetUserByUsername(username), Times.Once);
        }
        
        [Fact]
        public async Task IsUsernameTaken_WhenExceptionOccurs_ReturnsTrue()
        {
            // Arrange
            string username = "erroruser";
            _mockUserHelperService.Setup(s => s.GetUserByUsername(username)).ThrowsAsync(new Exception("Database error"));
            
            // Act
            bool result = await _signUpService.IsUsernameTaken(username);
            
            // Assert
            Assert.True(result);
            _mockUserHelperService.Verify(s => s.GetUserByUsername(username), Times.Once);
        }
        
        [Fact]
        public async Task RegisterUser_WhenEmailExists_ReturnsFalse()
        {
            // Arrange
            var user = new User { UserId = 0, UserName = "newuser", Email = "existing@example.com" };
            var existingUser = new User { UserId = 1, UserName = "existinguser", Email = user.Email };
            _mockUserHelperService.Setup(s => s.GetUserByEmail(user.Email)).ReturnsAsync(existingUser);
            
            // Act
            bool result = await _signUpService.RegisterUser(user);
            
            // Assert
            Assert.False(result);
            _mockUserHelperService.Verify(s => s.GetUserByEmail(user.Email), Times.Once);
            _mockUserHelperService.Verify(s => s.GetUserByUsername(It.IsAny<string>()), Times.Never);
            _mockUserHelperService.Verify(s => s.CreateUser(It.IsAny<User>()), Times.Never);
        }
        
        [Fact]
        public async Task RegisterUser_WhenUsernameExists_ReturnsFalse()
        {
            // Arrange
            var user = new User { UserId = 0, UserName = "existinguser", Email = "new@example.com" };
            var existingUser = new User { UserId = 1, UserName = user.UserName, Email = "existing@example.com" };
            
            _mockUserHelperService.Setup(s => s.GetUserByEmail(user.Email)).ReturnsAsync((User)null);
            _mockUserHelperService.Setup(s => s.GetUserByUsername(user.UserName)).ReturnsAsync(existingUser);
            
            // Act
            bool result = await _signUpService.RegisterUser(user);
            
            // Assert
            Assert.False(result);
            _mockUserHelperService.Verify(s => s.GetUserByEmail(user.Email), Times.Once);
            _mockUserHelperService.Verify(s => s.GetUserByUsername(user.UserName), Times.Once);
            _mockUserHelperService.Verify(s => s.CreateUser(It.IsAny<User>()), Times.Never);
        }
        
        [Fact]
        public async Task RegisterUser_WithValidUser_ReturnsTrue()
        {
            // Arrange
            var user = new User { UserId = 0, UserName = "newuser", Email = "new@example.com" };
            int newUserId = 1;
            
            _mockUserHelperService.Setup(s => s.GetUserByEmail(user.Email)).ReturnsAsync((User)null);
            _mockUserHelperService.Setup(s => s.GetUserByUsername(user.UserName)).ReturnsAsync((User)null);
            _mockUserHelperService.Setup(s => s.CreateUser(It.IsAny<User>())).ReturnsAsync(newUserId);
            
            // Act
            bool result = await _signUpService.RegisterUser(user);
            
            // Assert
            Assert.True(result);
            Assert.True(user.OnlineStatus);
            Assert.Equal(newUserId, user.UserId);
            _mockUserHelperService.Verify(s => s.GetUserByEmail(user.Email), Times.Once);
            _mockUserHelperService.Verify(s => s.GetUserByUsername(user.UserName), Times.Once);
            _mockUserHelperService.Verify(s => s.CreateUser(user), Times.Once);
        }
    }
} 