using Duo.Api.Models;

namespace Duo.Api.Repositories.Interfaces
{
    public interface IFriendsRepository
    {
        Task<IEnumerable<Friend>> GetFriends(int userId);
        Task<bool> AddFriend(int userId1, int userId2);
        Task<bool> RemoveFriend(int userId1, int userId2);
        Task<bool> IsFriend(int userId1, int userId2);
    }
} 