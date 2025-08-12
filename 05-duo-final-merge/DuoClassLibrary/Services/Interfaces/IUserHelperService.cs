
using DuoClassLibrary.Models;

namespace DuoClassLibrary.Services.Interfaces
{
    public interface IUserHelperService
    {
        Task<User> GetUserById(int id);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
        Task<int> CreateUser(User newUserToCreate);
        Task UpdateUser(User userToUpdate);
        Task<bool> ValidateCredentials(string username, string password);
        Task<User> GetUserByCredentials(string username, string password);
        Task<List<LeaderboardEntry>> GetTopUsersByCompletedQuizzes();
        Task<List<LeaderboardEntry>> GetTopUsersByAccuracy();
        Task<User> GetUserStats(int userId);
        Task<List<Achievement>> GetAllAchievements();
        Task<List<Achievement>> GetUserAchievements(int userId);
        Task AwardAchievement(int userId, int achievementId);
        Task<List<User>> GetFriends(int userId);
        Task UpdateUserAsync(User user);
        Task UpdateUserSectionProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesInSectionCompleted);
    }
} 