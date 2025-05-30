using Duo.Api.Models;
using Duo.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendsRepository _friendsRepository;

        public FriendsController(IFriendsRepository friendsRepository)
        {
            _friendsRepository = friendsRepository;
        }

        [HttpGet("{userId}", Name = "GetFriends")]
        public async Task<IEnumerable<Friend>> GetFriends(int userId)
        {
            return await _friendsRepository.GetFriends(userId);
        }

        public class AddRemoveFriendRequest
        {
            public int UserId1 { get; set; }
            public int UserId2 { get; set; }
        }

        [HttpPost(Name = "AddFriend")]
        public async Task<ActionResult> AddFriend([FromBody] AddRemoveFriendRequest request)
        {
            var result = await _friendsRepository.AddFriend(request.UserId1, request.UserId2);
            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpDelete(Name = "RemoveFriend")]
        public async Task<ActionResult> RemoveFriend([FromBody] AddRemoveFriendRequest request)
        {
            var result = await _friendsRepository.RemoveFriend(request.UserId1, request.UserId2);
            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpGet("check/{userId1}/{userId2}", Name = "IsFriend")]
        public async Task<bool> IsFriend(int userId1, int userId2)
        {
            return await _friendsRepository.IsFriend(userId1, userId2);
        }
    }
} 