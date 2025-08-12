using Microsoft.AspNetCore.Mvc;
using Duo.Api.Repositories.Interfaces;
using Duo.Api.Models;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("{id}")]
        public async Task<Comment?> Get(int id)
        {
            var comment = await _commentRepository.GetCommentById(id);
            return comment;
        }

        [HttpGet("ByPost/{postId}")]
        public async Task<List<Comment>> GetByPost(int postId)
        {
            var comments = await _commentRepository.GetCommentsByPostId(postId);
            return comments;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Comment comment)
        {
            var id = await _commentRepository.CreateComment(comment);
            return Ok(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _commentRepository.DeleteComment(id);
            return NoContent();
        }

        [HttpPost("Like/{id}")]
        public async Task<IActionResult> Like(int id)
        {
            await _commentRepository.IncrementLikeCount(id);
            return Ok();
        }

        [HttpGet("Replies/{parentCommentId}")]
        public async Task<List<Comment>> GetReplies(int parentCommentId)
        {
            var replies = await _commentRepository.GetRepliesByCommentId(parentCommentId);
            return replies;
        }

        [HttpGet("Count/{postId}")]
        public async Task<int> GetCount(int postId)
        {
            var count = await _commentRepository.GetCommentsCountForPost(postId);
            return count;
        }
    }
} 