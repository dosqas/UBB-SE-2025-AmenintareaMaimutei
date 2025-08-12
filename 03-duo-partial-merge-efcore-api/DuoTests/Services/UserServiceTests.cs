using System;
using System.Threading.Tasks;
using Duo.Models;
using Duo.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DuoTests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserServiceProxy> userServiceProxyMock;
        private UserService userService;

        [TestInitialize]
        public void Setup()
        {
            this.userServiceProxyMock = new Mock<IUserServiceProxy>();
            this.userService = new UserService(this.userServiceProxyMock.Object);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserIdIsInvalid()
        {
            // Act
            var result = await this.userService.GetByIdAsync(0);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldCallProxy_WhenUserIdIsValid()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User(userId, "testuser");
            this.userServiceProxyMock.Setup(proxy => proxy.GetByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await this.userService.GetByIdAsync(userId);

            // Assert
            Assert.AreEqual(expectedUser, result);
            this.userServiceProxyMock.Verify(proxy => proxy.GetByIdAsync(userId), Times.Once);
        }

        [TestMethod]
        public async Task GetByUsernameAsync_ShouldReturnNull_WhenUsernameIsInvalid()
        {
            // Act
            var result = await this.userService.GetByUsernameAsync("");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByUsernameAsync_ShouldCallProxy_WhenUsernameIsValid()
        {
            // Arrange
            var username = "testuser";
            var expectedUser = new User(1, username);
            this.userServiceProxyMock.Setup(proxy => proxy.GetByUsernameAsync(username))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await this.userService.GetByUsernameAsync(username);

            // Assert
            Assert.AreEqual(expectedUser, result);
            this.userServiceProxyMock.Verify(proxy => proxy.GetByUsernameAsync(username), Times.Once);
        }

        [TestMethod]
        public async Task CreateUserAsync_ShouldReturnMinusOne_WhenUserIsInvalid()
        {
            // Act
            var result = await this.userService.CreateUserAsync(null);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public async Task CreateUserAsync_ShouldCallProxy_WhenUserIsValid()
        {
            // Arrange
            var user = new User("testuser");
            this.userServiceProxyMock.Setup(proxy => proxy.CreateUserAsync(user))
                .ReturnsAsync(1);

            // Act
            var result = await this.userService.CreateUserAsync(user);

            // Assert
            Assert.AreEqual(1, result);
            this.userServiceProxyMock.Verify(proxy => proxy.CreateUserAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task UpdateUserSectionProgressAsync_ShouldHandleException_WhenUserIdIsInvalid()
        {
            // Act
            await this.userService.UpdateUserSectionProgressAsync(0, 5, 10);

            // Assert
            this.userServiceProxyMock.Verify(proxy => proxy.UpdateUserSectionProgressAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
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
        public async Task IncrementUserProgressAsync_ShouldHandleException_WhenUserIdIsInvalid()
        {
            // Act
            await this.userService.IncrementUserProgressAsync(0);

            // Assert
            this.userServiceProxyMock.Verify(proxy => proxy.GetByIdAsync(It.IsAny<int>()), Times.Never);
            this.userServiceProxyMock.Verify(proxy => proxy.UpdateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public async Task IncrementUserProgressAsync_ShouldUpdateUserProgress_WhenUserIdIsValid()
        {
            // Arrange
            var userId = 1;
            var user = new User(userId, "testuser", 0, 0);
            this.userServiceProxyMock.Setup(proxy => proxy.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            await this.userService.IncrementUserProgressAsync(userId);

            // Assert
            Assert.AreEqual(1, user.NumberOfCompletedQuizzesInSection);
            this.userServiceProxyMock.Verify(proxy => proxy.UpdateUserAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task UpdateUserAsync_ShouldHandleException_WhenUserIsNull()
        {
            // Act
            await this.userService.UpdateUserAsync(null);

            // Assert
            this.userServiceProxyMock.Verify(proxy => proxy.UpdateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateUserAsync_ShouldCallProxy_WhenUserIsValid()
        {
            // Arrange
            var user = new User(1, "testuser");
            this.userServiceProxyMock.Setup(proxy => proxy.UpdateUserAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            await this.userService.UpdateUserAsync(user);

            // Assert
            this.userServiceProxyMock.Verify(proxy => proxy.UpdateUserAsync(user), Times.Once);
        }
    }
}
