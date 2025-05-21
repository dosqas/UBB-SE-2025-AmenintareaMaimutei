using Microsoft.AspNetCore.Mvc;
using Duo.Api.Repositories.Interfaces;
using Duo.Api.Models;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        [HttpGet(Name = "GetAllPosts")]
        public async Task<IEnumerable<Post>> Get()
        {
            return await _postRepository.GetPosts();
        }
        [HttpPost(Name = "CreatePost")]
        public async Task<ActionResult<int>> Create([FromBody] Post post)
        {
            var postId = await _postRepository.CreatePost(post);
            return Ok(postId);
        }

        [HttpPut("{id}", Name = "UpdatePost")]
        public async Task<ActionResult> Update(int id, [FromBody] Post post)
        {
            if (id != post.Id)
            {
                return BadRequest("Post ID mismatch");
            }

            await _postRepository.UpdatePost(post);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeletePost")]
        public async Task<ActionResult> Delete(int id)
        {
            await _postRepository.DeletePost(id);
            return NoContent();
        }
    }
}

