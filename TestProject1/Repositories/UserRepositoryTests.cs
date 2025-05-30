using Duo.Api.Persistence;
using Duo.Api.Models;
using Duo.Api.Repositories.Interfaces;
using Duo.Api.Repositories.Repos;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1.Repositories
{
    public class UserRepositoryTests
    {
        private readonly DbContextOptions<DataContext> _options;

        public UserRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetUsers_WhenCalled_ReturnsAllUsers()
        {
            // Arrange
            using (var context = new DataContext(_options))
            {
                context.Users.Add(new User { UserId = 1, UserName = "User1" });
                context.Users.Add(new User { UserId = 2, UserName = "User2" });
                context.Users.Add(new User { UserId = 3, UserName = "User3" });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new DataContext(_options))
            {
                var repository = new UserRepository(context);
                var result = await repository.GetUsers();

                // Assert
                Assert.Equal(3, result.Count);
            }
        }

        [Fact]
        public async Task GetUser_WithValidId_ReturnsMatchingUser()
        {
            // Arrange
            var userId = 1;
            using (var context = new DataContext(_options))
            {
                context.Users.Add(new User { UserId = userId, UserName = "User1" });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new DataContext(_options))
            {
                var repository = new UserRepository(context);
                var result = await repository.GetUser(userId);

                // Assert
                Assert.Equal(userId, result.UserId);
            }
        }

        [Fact]
        public async Task CreateUser_WithValidUser_ReturnsUserId()
        {
            // Arrange
            var user = new User { UserName = "NewUser" };

            // Act
            using (var context = new DataContext(_options))
            {
                var repository = new UserRepository(context);
                var result = await repository.CreateUser(user);

                // Assert
                Assert.Equal(user.UserId, result);
            }
        }

        [Fact]
        public async Task CreateUser_WithNullUser_ThrowsArgumentNullException()
        {
            // Arrange
            User user = null;
            
            // Act & Assert
            using (var context = new DataContext(_options))
            {
                var repository = new UserRepository(context);
                await Assert.ThrowsAsync<ArgumentNullException>(() => repository.CreateUser(user));
            }
        }

        [Fact]
        public async Task DeleteUser_WithExistingId_RemovesUser()
        {
            // Arrange
            var userId = 1;
            using (var context = new DataContext(_options))
            {
                context.Users.Add(new User { UserId = userId, UserName = "User1" });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new DataContext(_options))
            {
                var repository = new UserRepository(context);
                await repository.DeleteUser(userId);
            }

            // Assert
            using (var context = new DataContext(_options))
            {
                var user = await context.Users.FindAsync(userId);
                Assert.Null(user);
            }
        }

        [Fact]
        public async Task DeleteUser_WithNonExistingId_DoesNotThrowException()
        {
            // Arrange
            var userId = 999;

            // Act & Assert (no exception should be thrown)
            using (var context = new DataContext(_options))
            {
                var repository = new UserRepository(context);
                await repository.DeleteUser(userId);
            }

            // Success if no exception
            Assert.True(true);
        }

        [Fact]
        public async Task UpdateUser_WithValidUser_UpdatesExistingUser()
        {
            // Arrange
            var userId = 1;
            var updatedName = "UpdatedUser";
            
            using (var context = new DataContext(_options))
            {
                context.Users.Add(new User { UserId = userId, UserName = "User1" });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new DataContext(_options))
            {
                var user = new User { UserId = userId, UserName = updatedName };
                var repository = new UserRepository(context);
                await repository.UpdateUser(user);
            }

            // Assert
            using (var context = new DataContext(_options))
            {
                var updatedUser = await context.Users.FindAsync(userId);
                Assert.Equal(updatedName, updatedUser.UserName);
            }
        }

        [Fact]
        public async Task UpdateUser_WithNullUser_ThrowsArgumentNullException()
        {
            // Arrange
            User user = null;

            // Act & Assert
            using (var context = new DataContext(_options))
            {
                var repository = new UserRepository(context);
                await Assert.ThrowsAsync<ArgumentNullException>(() => repository.UpdateUser(user));
            }
        }

        [Fact]
        public async Task UpdateUser_WithNonExistingUser_DoesNotThrowException()
        {
            // Arrange
            var user = new User { UserId = 999, UserName = "NonExistingUser" };

            // Act (no exception should be thrown)
            using (var context = new DataContext(_options))
            {
                var repository = new UserRepository(context);
                await repository.UpdateUser(user);
            }

            // Success if no exception
            Assert.True(true);
        }

        [Fact]
        public void Constructor_WithNullContext_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new UserRepository(null));
        }
    }
} 