using DuoClassLibrary.Models;


namespace DuoClassLibrary.Services.Interfaces
{
    public interface IFriendsService
    {
        Task<List<LeaderboardEntry>> GetTopFriendsByCompletedQuizzes(int userId);
        Task<List<LeaderboardEntry>> GetTopFriendsByAccuracy(int userId);
    }
} 