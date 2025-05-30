using DuoClassLibrary.Services;
using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using Moq;
using Xunit;

namespace TestProject1.Services
{
    public class UserHelperServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserHelperService _userHelperService;

        public UserHelperServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userHelperService = new UserHelperService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task GetUserById_WithValidId_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User(userId, "testuser");
            _mockUserRepository.Setup(x => x.GetUser(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userHelperService.GetUserById(userId);

            // Assert
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.UserName, result.UserName);
        }

        [Fact]
        public async Task GetUserByUsername_WithValidUsername_ReturnsUser()
        {
            // Arrange
            var username = "testuser";
            var expectedUser = new User(1, username);
            var users = new List<User> { expectedUser };
            _mockUserRepository.Setup(x => x.GetUsers())
                .ReturnsAsync(users);

            // Act
            var result = await _userHelperService.GetUserByUsername(username);

            // Assert
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.UserName, result.UserName);
        }

        [Fact]
        public async Task GetUserByUsername_WithInvalidUsername_ReturnsNull()
        {
            // Arrange
            var username = "nonexistent";
            var users = new List<User> { new User(1, "testuser") };
            _mockUserRepository.Setup(x => x.GetUsers())
                .ReturnsAsync(users);

            // Act
            var result = await _userHelperService.GetUserByUsername(username);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByEmail_WithValidEmail_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUser = new User(1, "testuser") { Email = email };
            var users = new List<User> { expectedUser };
            _mockUserRepository.Setup(x => x.GetUsers())
                .ReturnsAsync(users);

            // Act
            var result = await _userHelperService.GetUserByEmail(email);

            // Assert
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.Email, result.Email);
        }

        [Fact]
        public async Task CreateUser_WithValidUser_ReturnsUserId()
        {
            // Arrange
            var newUser = new DuoClassLibrary.Models. User("testuser");
            var expectedUserId = 1;
            _mockUserRepository.Setup(x => x.CreateUser(newUser))
                .ReturnsAsync(expectedUserId);

            // Act
            var result = await _userHelperService.CreateUser(newUser);

            // Assert
            Assert.Equal(expectedUserId, result);
        }

        [Fact]
        public async Task CreateUser_WithNullUser_ThrowsArgumentNullException()
        {
            // Arrange
            User nullUser = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userHelperService.CreateUser(nullUser));
        }

        [Fact]
        public async Task UpdateUser_WithValidUser_UpdatesSuccessfully()
        {
            // Arrange
            var userToUpdate = new User(1, "testuser");
            _mockUserRepository.Setup(x => x.UpdateUser(userToUpdate))
                .Returns(Task.CompletedTask);

            // Act
            await _userHelperService.UpdateUser(userToUpdate);

            // Assert
            _mockUserRepository.Verify(x => x.UpdateUser(userToUpdate), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_WithNullUser_ThrowsArgumentNullException()
        {
            // Arrange
            User nullUser = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userHelperService.UpdateUser(nullUser));
        }

        [Fact]
        public async Task ValidateCredentials_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            var username = "testuser";
            var password = "password";
            var user = new User(1, username) { Password = password };
            var users = new List<User> { user };
            _mockUserRepository.Setup(x => x.GetUsers())
                .ReturnsAsync(users);

            // Act
            var result = await _userHelperService.ValidateCredentials(username, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateCredentials_WithInvalidCredentials_ReturnsFalse()
        {
            // Arrange
            var username = "testuser";
            var password = "wrongpassword";
            var user = new User(1, username) { Password = "correctpassword" };
            var users = new List<User> { user };
            _mockUserRepository.Setup(x => x.GetUsers())
                .ReturnsAsync(users);

            // Act
            var result = await _userHelperService.ValidateCredentials(username, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetUserByCredentials_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var username = "testuser";
            var password = "password";
            var expectedUser = new User(1, username) { Password = password };
            var users = new List<User> { expectedUser };
            _mockUserRepository.Setup(x => x.GetUsers())
                .ReturnsAsync(users);

            // Act
            var result = await _userHelperService.GetUserByCredentials(username, password);

            // Assert
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.UserName, result.UserName);
        }

        [Fact]
        public async Task GetUserByCredentials_WithInvalidCredentials_ReturnsNull()
        {
            // Arrange
            var username = "testuser";
            var password = "wrongpassword";
            var user = new User(1, username) { Password = "correctpassword" };
            var users = new List<User> { user };
            _mockUserRepository.Setup(x => x.GetUsers())
                .ReturnsAsync(users);

            // Act
            var result = await _userHelperService.GetUserByCredentials(username, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserStats_WithValidUserId_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User(userId, "testuser");
            _mockUserRepository.Setup(x => x.GetUser(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userHelperService.GetUserStats(userId);

            // Assert
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.UserName, result.UserName);
        }
    }
}