using Microsoft.AspNetCore.Mvc;
using Duo.Api.Repositories.Interfaces;
using Duo.Api.Models;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HashtagController : ControllerBase
    {
        private readonly IHashtagRepository _hashtagRepository;

        public HashtagController(IHashtagRepository hashtagRepository)
        {
            _hashtagRepository = hashtagRepository;
        }

        [HttpGet(Name = "GetAllHashtags")]
        public async Task<IEnumerable<Hashtag>> Get()
        {
            return await _hashtagRepository.GetHashtags();
        }
        
        [HttpPost(Name = "CreateHashtag")]
        public async Task<ActionResult<int>> Create([FromBody] Hashtag hashtag)
        {
            var hashtagId = await _hashtagRepository.CreateHashtag(hashtag);
            return Ok(hashtagId);
        }

        [HttpGet("posthashtags", Name = "GetAllPostHashtags")]
        public async Task<IEnumerable<PostHashtags>> GetAllPostHashtags()
        {
            return await _hashtagRepository.GetAllPostHashtags();
        }

        [HttpPost("posthashtag", Name = "CreatePostHashtag")]
        public async Task<ActionResult> CreatePostHashtag([FromBody] PostHashtags postHashtag)
        {
            await _hashtagRepository.AddHashtagToPost(postHashtag.PostId, postHashtag.HashtagId);
            return Ok();
        }

        [HttpDelete("posthashtag", Name = "DeletePostHashtag")]
        public async Task<ActionResult> DeletePostHashtag([FromBody] PostHashtags postHashtag)
        {
            await _hashtagRepository.RemoveHashtagFromPost(postHashtag.PostId, postHashtag.HashtagId);
            return NoContent();
        }
    }
}
