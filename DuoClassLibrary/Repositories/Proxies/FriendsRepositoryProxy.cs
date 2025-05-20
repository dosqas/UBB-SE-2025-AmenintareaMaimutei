using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace DuoClassLibrary.Repositories.Proxies
{
    public class FriendsRepositoryProxy : IFriendsRepository
    {
        private readonly IFriendsRepository _repository;
        private readonly ILogger<FriendsRepositoryProxy> _logger;

        public FriendsRepositoryProxy(
            IFriendsRepository repository,
            ILogger<FriendsRepositoryProxy> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<Friend>> GetFriends(int userId)
        {
            _logger.LogInformation($"Retrieving friends for user {userId}");
            return await _repository.GetFriends(userId);
        }

        public async Task<bool> AddFriend(int userId1, int userId2)
        {
            var result = await _repository.AddFriend(userId1, userId2);
            if (result)
            {
                _logger.LogInformation($"Added friend {userId2} for user {userId1}");
            }
            return result;
        }

        public async Task<bool> RemoveFriend(int userId1, int userId2)
        {
            var result = await _repository.RemoveFriend(userId1, userId2);
            if (result)
            {
                _logger.LogInformation($"Removed friend {userId2} for user {userId1}");
            }
            return result;
        }

        public async Task<bool> IsFriend(int userId1, int userId2)
        {
            _logger.LogInformation($"Checking friendship status for users {userId1} and {userId2}");
            return await _repository.IsFriend(userId1, userId2);
        }
    }
} 