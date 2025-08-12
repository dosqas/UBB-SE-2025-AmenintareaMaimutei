using Duo;
using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


//TODO: comentat ca s a zis ca nu merge
namespace Duo.Services
{
    /// <summary>
    /// Service for managing friend relationships between users.
    /// </summary>
    public class FriendsService
    {
        /*
        private readonly IFriendsRepository _friendsRepository;
        //private readonly DataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendsService"/> class.
        /// </summary>
        /// <param name="friendsRepository">The friends repository.</param>
        /// <param name="context">The database context.</param>
        public FriendsService(IFriendsRepository friendsRepository, DataContext context)
        {
            _friendsRepository = friendsRepository ?? throw new ArgumentNullException(nameof(friendsRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the friends for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A list of user's friends.</returns>
        public async Task<List<User>> GetFriends(int userId)
        {
            var friends = await _friendsRepository.GetFriends(userId);
            var friendIds = friends.Select(f => f.UserId1 == userId ? f.UserId2 : f.UserId1).ToList();
            return await _context.Users.Where(u => friendIds.Contains(u.UserId)).ToListAsync();
        }

        /// <summary>
        /// Sorts friends alphabetically by name.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A sorted list of user's friends.</returns>
        public async Task<List<User>> SortFriendsByName(int userId)
        {
            var friends = await GetFriends(userId);
            return friends.OrderBy(f => f.UserName).ToList();
        }

        /// <summary>
        /// Sorts friends by the date they were added.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A sorted list of user's friends.</returns>
        public async Task<List<User>> SortFriendsByDateAdded(int userId)
        {
            var friends = await GetFriends(userId);
            return friends.OrderBy(f => f.DateJoined).ToList();
        }

        /// <summary>
        /// Sorts friends by their online status.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A sorted list of user's friends.</returns>
        public async Task<List<User>> SortFriendsByOnlineStatus(int userId)
        {
            var friends = await GetFriends(userId);
            return friends
                .OrderByDescending(f => f.OnlineStatus)
                .ThenByDescending(f => f.LastActivityDate ?? DateTime.MinValue)
                .ToList();
        }

        /// <summary>
        /// Gets the top friends by completed quizzes.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A list of leaderboard entries for friends sorted by completed quizzes.</returns>
        public async Task<List<LeaderboardEntry>> GetTopFriendsByCompletedQuizzes(int userId)
        {
            var friends = await GetFriends(userId);
            var leaderboardEntries = friends
                .Select(u => new LeaderboardEntry
                {
                    UserId = u.UserId,
                    Username = u.UserName,
                    ScoreValue = u.QuizzesCompleted,
                    Rank = 0
                })
                .OrderByDescending(e => e.ScoreValue)
                .Take(10)
                .ToList();

            // Set ranks
            for (int i = 0; i < leaderboardEntries.Count; i++)
            {
                leaderboardEntries[i].Rank = i + 1;
            }

            return leaderboardEntries;
        }

        /// <summary>
        /// Gets the top friends by accuracy.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A list of leaderboard entries for friends sorted by accuracy.</returns>
        public async Task<List<LeaderboardEntry>> GetTopFriendsByAccuracy(int userId)
        {
            var friends = await GetFriends(userId);
            var leaderboardEntries = friends
                .Select(u => new LeaderboardEntry
                {
                    UserId = u.UserId,
                    Username = u.UserName,
                    ScoreValue = u.Accuracy,
                    Rank = 0
                })
                .OrderByDescending(e => e.ScoreValue)
                .Take(10)
                .ToList();

            // Set ranks
            for (int i = 0; i < leaderboardEntries.Count; i++)
            {
                leaderboardEntries[i].Rank = i + 1;
            }

            return leaderboardEntries;
        }

        /// <summary>
        /// Gets all friends with their details.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A list of friends with their complete details.</returns>
        public async Task<List<User>> GetAllFriendsWithDetails(int userId)
        {
            return await GetFriends(userId);
        } */
    }
}
