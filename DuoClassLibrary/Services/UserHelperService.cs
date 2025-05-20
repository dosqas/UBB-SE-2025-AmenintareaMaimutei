using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using DuoClassLibrary.Services.Interfaces;

namespace DuoClassLibrary.Services
{
    public class UserHelperService : IUserHelperService
    {
        private IUserRepository _userRepository;

        public UserHelperService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUser(id);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var users = await _userRepository.GetUsers();
            return users.Find(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var users = await _userRepository.GetUsers();
            return users.Find(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<int> CreateUser(User newUserToCreate)
        {
            if (newUserToCreate == null)
                throw new ArgumentNullException(nameof(newUserToCreate));

            return await _userRepository.CreateUser(newUserToCreate);
        }

        public async Task UpdateUser(User userToUpdate)
        {
            if (userToUpdate == null)
                throw new ArgumentNullException(nameof(userToUpdate));

            await _userRepository.UpdateUser(userToUpdate);
        }

        public async Task<bool> ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            var user = await GetUserByUsername(username);
            return user != null && user.Password == password;
        }

        public async Task<User> GetUserByCredentials(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await GetUserByUsername(username);
            return user != null && user.Password == password ? user : null;
        }

        public async Task<List<LeaderboardEntry>> GetTopUsersByCompletedQuizzes()
        {
            return new List<LeaderboardEntry>();
        }

        public async Task<List<LeaderboardEntry>> GetTopUsersByAccuracy()
        {
            return new List<LeaderboardEntry>();
        }

        public async Task<User> GetUserStats(int userId)
        {
            return await _userRepository.GetUser(userId);
        }

        public async Task<List<Achievement>> GetAllAchievements()
        {
            // This would typically come from a separate achievement repository
            // For now, returning an empty list as a placeholder
            return new List<Achievement>();
        }

        public async Task<List<Achievement>> GetUserAchievements(int userId)
        {
            // This would typically come from a separate achievement repository
            // For now, returning an empty list as a placeholder
            return new List<Achievement>();
        }

        public async Task AwardAchievement(int userId, int achievementId)
        {
            // This would typically update a separate achievement repository
            // For now, this is a placeholder
            await Task.CompletedTask;
        }

        public async Task<List<User>> GetFriends(int userId)
        {
            // This would typically come from a separate friends repository
            // For now, returning an empty list as a placeholder
            return new List<User>();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task UpdateUserSectionProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesInSectionCompleted)
        {
            await _userRepository.UpdateUserSectionProgressAsync(userId,newNrOfSectionsCompleted,newNrOfQuizzesInSectionCompleted);
        }
    }
} 