﻿using System.Threading.Tasks;
using Duo.Models;

namespace Duo.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<int> CreateUserAsync(User user);
        Task UpdateUserProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesCompletedInSection);
        Task<User> GetByIdAsync(int userId);
        Task IncrementUserProgressAsync(int userId);
    }
}
