using Microsoft.AspNetCore.Mvc;
using DuoClassLibrary.Services.Interfaces;
using System.Threading.Tasks;

namespace WebServerTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikePost(int id)
        {
            try
            {
                var result = await _postService.LikePost(id);
                if (result)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (System.Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
} 