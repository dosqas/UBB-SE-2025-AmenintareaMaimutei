using System;
using System.Threading.Tasks;
using DuoClassLibrary.Models;

namespace DuoClassLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly IUserServiceProxy userServiceProxy;

        public UserService(IUserServiceProxy userServiceProxy)
        {
            this.userServiceProxy = userServiceProxy ?? throw new ArgumentNullException(nameof(userServiceProxy));
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }
            return await userServiceProxy.GetByIdAsync(userId);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            }
            return await userServiceProxy.GetByUsernameAsync(username);
        }

        public async Task<int> CreateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(user));
            }
            return await userServiceProxy.CreateUserAsync(user);
        }

        public async Task UpdateUserSectionProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesInSectionCompleted)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }
            await userServiceProxy.UpdateUserSectionProgressAsync(
                userId,
                newNrOfSectionsCompleted,
                newNrOfQuizzesInSectionCompleted);
        }

        public async Task IncrementUserProgressAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }
            var user = await GetByIdAsync(userId);
            user.NumberOfCompletedQuizzesInSection++;

            await userServiceProxy.UpdateUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await userServiceProxy.UpdateUserAsync(user);
        }
    }
}