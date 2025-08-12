using DuoClassLibrary.Models;
using DuoClassLibrary.Services.Interfaces;
using DuoClassLibrary.Constants;

namespace Duo.Services;

public class LeaderboardService
{
    private readonly IUserHelperService _userHelperService;
    private readonly IFriendsService _friendsService;

    public LeaderboardService(IUserHelperService userHelperService, IFriendsService friendsService)
    {
        _userHelperService = userHelperService ?? throw new ArgumentNullException(nameof(userHelperService));
        _friendsService = friendsService ?? throw new ArgumentNullException(nameof(friendsService));
    }

    public async Task<List<LeaderboardEntry>> GetGlobalLeaderboard(string criteria)
    {
        // Return the top users in the repository sorted by the specified criteria
        if (criteria == LeaderboardConstants.CompletedQuizzesCriteria)
        {
            return await _userHelperService.GetTopUsersByCompletedQuizzes();
        }
        else if (criteria == LeaderboardConstants.AccuracyCriteria)
        {
            return await _userHelperService.GetTopUsersByAccuracy();
        }
        else
        {
            throw new ArgumentException($"Invalid criteria: {criteria}", nameof(criteria));
        }
    }

    public async Task<List<LeaderboardEntry>> GetFriendsLeaderboard(int userId, string criteria)
    {
        // Return the top friends of the user sorted by the specified criteria
        if (criteria == LeaderboardConstants.CompletedQuizzesCriteria)
        {
            return await _friendsService.GetTopFriendsByCompletedQuizzes(userId);
        }
        else if (criteria == LeaderboardConstants.AccuracyCriteria)
        {
            return await _friendsService.GetTopFriendsByAccuracy(userId);
        }
        else
        {
            throw new ArgumentException($"Invalid criteria: {criteria}", nameof(criteria));
        }
    }

    /// <summary>
    /// Updates the user's score with the specified points.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="points">The points to add.</param>
    public async Task UpdateUserScore(int userId, int points)
    {
        var user = await _userHelperService.GetUserById(userId);
        if (user != null)
        {
            //user.Score += points;
            await _userHelperService.UpdateUser(user);
        }
    }

    /// <summary>
    /// Calculates the rank change for a user within the specified time frame.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="timeFrame">The time frame to calculate rank change for.</param>
    public async Task CalculateRankChange(int userId, string timeFrame)
    {
        // TODO: Implement this method
        await Task.CompletedTask;
    }
}

