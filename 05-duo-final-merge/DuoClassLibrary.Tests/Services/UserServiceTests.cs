using System;
using System.Threading.Tasks;
using Duo.Services;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DuoTests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserHelperService> userServiceProxyMock;
        private UserService userService;

        [TestInitialize]
        public void Setup()
        {
            this.userServiceProxyMock = new Mock<IUserHelperService>();
            this.userService = new UserService(this.userServiceProxyMock.Object);
        }

        [TestMethod]
        public async Task GetByIdAsync_ThrowsException_WhenUserIdIsInvalid()
        {
            // Act
            await Assert.ThrowsExceptionAsync<Exception>(() => this.userService.GetUserById(0));
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldCallProxy_WhenUserIdIsValid()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User(userId, "testuser");
            this.userServiceProxyMock.Setup(proxy => proxy.GetUserById(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await this.userService.GetUserById(userId);

            // Assert
            Assert.AreEqual(expectedUser, result);
            this.userServiceProxyMock.Verify(proxy => proxy.GetUserById(userId), Times.Once);
        }

        [TestMethod]
        public async Task GetByUsernameAsync_ThrowsException_WhenUsernameIsInvalid()
        {
            // Act
            var result = await this.userService.GetUserByUsername("");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByUsernameAsync_ShouldCallProxy_WhenUsernameIsValid()
        {
            // Arrange
            var username = "testuser";
            var expectedUser = new User(1, username);
            this.userServiceProxyMock.Setup(proxy => proxy.GetUserByUsername(username))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await this.userService.GetUserByUsername(username);

            // Assert
            Assert.AreEqual(expectedUser, result);
            this.userServiceProxyMock.Verify(proxy => proxy.GetUserByUsername(username), Times.Once);
        }


        [TestMethod]
        public async Task UpdateUserSectionProgressAsync_ThrowsException_WhenUserIdIsInvalid()
        {
            // Act
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => this.userService.UpdateUserSectionProgressAsync(0, 5, 10));

        }

        [TestMethod]
        public async Task UpdateUserSectionProgressAsync_ShouldCallProxy_WhenUserIdIsValid()
        {
            // Act
            await this.userService.UpdateUserSectionProgressAsync(1, 5, 10);

            // Assert
            this.userServiceProxyMock.Verify(proxy => proxy.UpdateUserSectionProgressAsync(1, 5, 10), Times.Once);
        }

        [TestMethod]
        public async Task IncrementUserProgressAsync_ThrowsException_WhenUserIdIsInvalid()
        {
            // Act
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => this.userService.IncrementUserProgressAsync(0));

        }

        [TestMethod]
        public async Task IncrementUserProgressAsync_ShouldUpdateUserProgress_WhenUserIdIsValid()
        {
            // Arrange
            var userId = 1;
            var user = new User(userId, "testuser");
            this.userServiceProxyMock.Setup(proxy => proxy.GetUserById(userId))
                .ReturnsAsync(user);

            // Act
            await this.userService.IncrementUserProgressAsync(userId);

            // Assert
            Assert.AreEqual(1, user.NumberOfCompletedQuizzesInSection);
            this.userServiceProxyMock.Verify(proxy => proxy.UpdateUserAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task UpdateUserAsync_ThrowsException_WhenUserIsNull()
        {
            // Act
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => this.userService.SetUser(null));

        }

    }
}