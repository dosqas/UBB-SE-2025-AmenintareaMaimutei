using Duo.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Duo.Api.Persistence;
using Duo.Api.Models;

namespace Duo.Api.Repositories.Repos
{
    public class FriendsRepository : IFriendsRepository
    {
        private readonly DataContext _context;

        public FriendsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Friend>> GetFriends(int userId)
        {
            return await _context.Friends
                .Where(f => f.UserId1 == userId || f.UserId2 == userId)
                .ToListAsync();
        }

        public async Task<bool> AddFriend(int userId1, int userId2)
        {
            if (userId1 == userId2)
                return false;

            var existingFriendship = await _context.Friends
                .FirstOrDefaultAsync(f => 
                    f.UserId1 == userId1 && f.UserId2 == userId2 ||
                    f.UserId1 == userId2 && f.UserId2 == userId1);

            if (existingFriendship != null)
                return false;

            var friendship = new Friend
            {
                UserId1 = userId1,
                UserId2 = userId2
            };

            await _context.Friends.AddAsync(friendship);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFriend(int userId1, int userId2)
        {
            var friendship = await _context.Friends
                .FirstOrDefaultAsync(f => 
                    f.UserId1 == userId1 && f.UserId2 == userId2 ||
                    f.UserId1 == userId2 && f.UserId2 == userId1);

            if (friendship == null)
                return false;

            _context.Friends.Remove(friendship);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsFriend(int userId1, int userId2)
        {
            return await _context.Friends
                .AnyAsync(f => 
                    f.UserId1 == userId1 && f.UserId2 == userId2 ||
                    f.UserId1 == userId2 && f.UserId2 == userId1);
        }
    }
} 