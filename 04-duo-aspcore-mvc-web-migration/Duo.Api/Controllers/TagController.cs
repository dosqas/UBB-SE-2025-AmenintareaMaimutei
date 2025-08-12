using System.Diagnostics.CodeAnalysis;
using Duo.Api.Models;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Controller for managing tags in the system.
    /// Provides endpoints for CRUD operations and additional tag-related functionalities.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TagController"/> class with the specified repository.
    /// </remarks>
    /// <param name="repository">The repository instance for data access.</param>
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class TagController(IRepository repository) : BaseController(repository)
    {
        #region Constructors

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new tag to the database.
        /// </summary>
        /// <param name="tag">The tag data to add.</param>
        /// <returns>The added tag.</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddTag([FromForm] Tag tag)
        {
            try
            {
                await repository.AddTagAsync(tag);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a tag by its ID.
        /// </summary>
        /// <param name="id">The ID of the tag to retrieve.</param>
        /// <returns>The tag if found; otherwise, NotFound.</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetTag([FromQuery] int id)
        {
            try
            {
                var tag = await repository.GetTagByIdAsync(id);
                if (tag == null)
                {
                    return NotFound();
                }

                return Ok(tag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Lists all tags in the database.
        /// </summary>
        /// <returns>A list of all tags.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListTags()
        {
            try
            {
                var tags = await repository.GetTagsFromDbAsync();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing tag.
        /// </summary>
        /// <param name="updatedTag">The updated tag data, including TagId.</param>
        /// <returns>The updated tag if found; otherwise, NotFound.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTag([FromForm] Tag updatedTag)
        {
            var tag = await repository.GetTagByIdAsync(updatedTag.TagId);
            if (tag == null)
            {
                return NotFound();
            }

            await repository.UpdateTagAsync(updatedTag);
            return Ok(updatedTag);
        }

        /// <summary>
        /// Deletes a tag by its ID.
        /// </summary>
        /// <param name="id">The ID of the tag to delete.</param>
        /// <returns>Ok if deleted; otherwise, NotFound.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTag([FromQuery] int id)
        {
            var tag = await repository.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            await repository.DeleteTagAsync(id);
            return Ok();
        }

        #endregion
    }
}
