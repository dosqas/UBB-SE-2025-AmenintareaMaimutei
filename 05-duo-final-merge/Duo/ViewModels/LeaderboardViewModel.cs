using DuoClassLibrary.Models;
using Duo.Services;
using DuoClassLibrary.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Duo.ViewModels
{
    /// <summary>
    /// ViewModel for the leaderboard functionality
    /// </summary>
    public class LeaderboardViewModel
    {
        private readonly LeaderboardService leaderboardService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderboardViewModel"/> class.
        /// </summary>
        /// <param name="leaderboardService">The leaderboard service.</param>
        public LeaderboardViewModel(LeaderboardService leaderboardService)
        {
            this.leaderboardService = leaderboardService ?? throw new ArgumentNullException(nameof(leaderboardService));
        }
        
        /// <summary>
        /// Gets or sets the user's rank in the leaderboard
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accuracy percentage
        /// </summary>
        public decimal Accuracy { get; set; }

        /// <summary>
        /// Gets the global leaderboard based on the specified criteria
        /// </summary>
        /// <param name="criteria">The sorting criteria</param>
        /// <returns>A list of leaderboard entries</returns>
        public async Task<List<LeaderboardEntry>> GetGlobalLeaderboard(string criteria)
        {
            return await leaderboardService.GetGlobalLeaderboard(criteria);
        }

        /// <summary>
        /// Gets the friends leaderboard for the specified user based on the criteria
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="criteria">The sorting criteria</param>
        /// <returns>A list of leaderboard entries</returns>
        public async Task<List<LeaderboardEntry>> GetFriendsLeaderboard(int userId, string criteria)
        {
            return await leaderboardService.GetFriendsLeaderboard(userId, criteria);
        }

        /// <summary>
        /// Gets the current user's rank in the global leaderboard
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="criteria">The sorting criteria</param>
        /// <returns>The user's rank, or -1 if not found</returns>
        public async Task<int> GetCurrentUserGlobalRank(int userId, string criteria)
        {
            var users = await leaderboardService.GetGlobalLeaderboard(criteria);
            var currentUser = users.FirstOrDefault(user => user.UserId == userId);
            if (currentUser == null)
            {
                return LeaderboardConstants.NoRankValue;
            }
            return users.IndexOf(currentUser) + LeaderboardConstants.RankIndexAdjustment;
        }

        /// <summary>
        /// Gets the current user's rank among friends
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="criteria">The sorting criteria</param>
        /// <returns>The user's rank among friends, or -1 if not found</returns>
        public async Task<int> GetCurrentUserFriendsRank(int userId, string criteria)
        {
            var users = await leaderboardService.GetFriendsLeaderboard(userId, criteria);
            var currentUser = users.FirstOrDefault(user => user.UserId == userId);
            if (currentUser == null)
            {
                return LeaderboardConstants.NoRankValue;
            }
            return users.IndexOf(currentUser) + LeaderboardConstants.RankIndexAdjustment;
        }
    }
}