using System;
using DuoClassLibrary.Models;
using Duo.Api.Repositories.Interfaces;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Moq;
using Xunit;

namespace TestProject1.Services
{
    public class LoginServiceTests
    {
        private readonly Mock<IUserHelperService> _userHelperServiceMock;
        private readonly LoginService _loginService;

        public LoginServiceTests()
        {
            _userHelperServiceMock = new Mock<IUserHelperService>();
            _loginService = new LoginService(_userHelperServiceMock.Object);
        }

        [Fact]
        public async Task AuthenticateUser_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            const string username = "testUser";
            const string password = "testPass";
            _userHelperServiceMock.Setup(x => x.ValidateCredentials(username, password))
                .ReturnsAsync(true);

            // Act
            var result = await _loginService.AuthenticateUser(username, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AuthenticateUser_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            const string username = "testUser";
            const string password = "wrongPass";
            _userHelperServiceMock.Setup(x => x.ValidateCredentials(username, password))
                .ReturnsAsync(false);

            // Act
            var result = await _loginService.AuthenticateUser(username, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetUserByCredentials_ValidCredentials_ReturnsUserAndUpdatesStatus()
        {
            // Arrange
            const string username = "testUser";
            const string password = "testPass";
            var user = new User { UserId = 1, UserName = username };
            _userHelperServiceMock.Setup(x => x.GetUserByCredentials(username, password))
                .ReturnsAsync(user);

            // Act
            var result = await _loginService.GetUserByCredentials(username, password);

            // Assert
            Assert.True(result.OnlineStatus);
        }

        [Fact]
        public async Task GetUserByCredentials_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            const string username = "testUser";
            const string password = "wrongPass";
            _userHelperServiceMock.Setup(x => x.GetUserByCredentials(username, password))
                .ReturnsAsync((User)null);

            // Act
            var result = await _loginService.GetUserByCredentials(username, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserStatusOnLogout_ValidUser_UpdatesStatusAndLastActivity()
        {
            // Arrange
            var user = new User { UserId = 1, UserName = "testUser", OnlineStatus = true };

            // Act
            await _loginService.UpdateUserStatusOnLogout(user);

            // Assert
            Assert.False(user.OnlineStatus);
        }

        [Fact]
        public async Task UpdateUserStatusOnLogout_NullUser_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _loginService.UpdateUserStatusOnLogout(null));
        }

        [Fact]
        public async Task UpdateUserStatusOnLogout_ValidUser_UpdatesLastActivityDate()
        {
            // Arrange
            var user = new User { UserId = 1, UserName = "testUser" };
            var beforeUpdate = DateTime.Now;

            // Act
            await _loginService.UpdateUserStatusOnLogout(user);

            // Assert
            Assert.NotNull(user.LastActivityDate);
            Assert.True(user.LastActivityDate >= beforeUpdate);
        }
    }
} 