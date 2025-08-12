using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Duo.Api.DTO.Requests;
using Duo.Api.Helpers;
using Duo.Api.Models.Sections;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Controller for managing sections in the system.
    /// Provides endpoints for CRUD operations and additional section-related functionalities.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SectionController"/> class with the specified repository.
    /// </remarks>
    /// <param name="repository">The repository instance for data access.</param>
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class SectionController(IRepository repository) : BaseController(repository)
    {
        #region Private Methods

        /// <summary>
        /// Gets the next available order number for a section in a roadmap.
        /// </summary>
        /// <param name="roadmapId">The ID of the roadmap.</param>
        /// <returns>The next available order number.</returns>
        private async Task<int> GetLastOrderNumberAsync(int roadmapId)
        {
            var sections = await repository.GetSectionsFromDbAsync();
            var orderNumbers = sections
                .Where(section => section.RoadmapId == roadmapId)
                .OrderBy(section => section.OrderNumber)
                .Select(section => section.OrderNumber ?? 0)
                .ToList();

            if (orderNumbers.Count == 0)
            {
                return 1;
            }

            for (int i = 1; i <= orderNumbers.Count; i++)
            {
                if (orderNumbers[i - 1] != i)
                {
                    return i;
                }
            }

            return orderNumbers.Count + 1;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new section to the system.
        /// </summary>
        /// <param name="request">The section data to add.</param>
        /// <returns>ActionResult with operation result.</returns>
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSection([FromBody] JsonElement rawJson)
        {
            try
            {
                string json = rawJson.GetRawText();
                var section = await JsonSerializationUtil.DeserializeSection(json, this.repository);

                await this.repository.AddSectionAsync(section);
                return Ok(new { message = "Section added successfully!", id = section.Id });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Removes a section from the system.
        /// </summary>
        /// <param name="id">The ID of the section to remove.</param>
        /// <returns>ActionResult with operation result.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveSection(int id)
        {
            try
            {
                var section = await repository.GetSectionByIdAsync(id);
                if (section == null)
                {
                    return NotFound(new { message = "Section not found!" });
                }

                await repository.DeleteSectionAsync(id);
                return Ok(new { message = "Section removed successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Gets a section by its ID.
        /// </summary>
        /// <param name="id">The ID of the section to retrieve.</param>
        /// <returns>ActionResult with the section data or error message.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSectionById([FromQuery] int id)
        {
            try
            {
                var section = await repository.GetSectionByIdAsync(id);
                if (section == null)
                {
                    return NotFound(new { message = "Section not found!" });
                }

                return Ok(new { result = section, message = "Successfully retrieved section!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Gets a list of all sections in the system.
        /// </summary>
        /// <returns>ActionResult with list of sections or error message.</returns>
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListSections()
        {
            try
            {
                var sections = await repository.GetSectionsFromDbAsync();
                return Ok(sections);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Gets a list of sections belonging to a specific roadmap.
        /// </summary>
        /// <param name="roadmapId">The ID of the roadmap.</param>
        /// <returns>ActionResult with list of sections or error message.</returns>
        [HttpGet("list/roadmap/{roadmapId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListSectionsByRoadmapId(int roadmapId)
        {
            try
            {
                var sections = await repository.GetSectionsFromDbAsync();
                var filteredSections = sections.Where(section => section.RoadmapId == roadmapId).ToList();

                if (filteredSections.Count == 0)
                {
                    return NotFound(new { result = new List<Section>(), message = "No sections found for the specified roadmap!" });
                }

                return Ok(new { result = filteredSections, message = "Successfully retrieved list of sections." });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Updates an existing section.
        /// </summary>
        /// <param name="id">The ID of the section to update.</param>
        /// <param name="request">The updated section data.</param>
        /// <returns>ActionResult with operation result.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSection([FromForm] int id, [FromForm] UpdateSectionRequest request)
        {
            try
            {
                var section = await repository.GetSectionByIdAsync(id);
                if (section == null)
                {
                    return NotFound(new { message = "Section not found!" });
                }

                section.SubjectId = request.SubjectId ?? section.SubjectId;
                section.Title = request.Title ?? section.Title;
                section.Description = request.Description ?? section.Description;
                section.RoadmapId = request.RoadmapId ?? section.RoadmapId;
                section.OrderNumber = request.OrderNumber ?? section.OrderNumber;

                await repository.UpdateSectionAsync(section);
                return Ok(new { message = "Section updated successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Gets the count of sections in a specific roadmap.
        /// </summary>
        /// <param name="roadmapId">The ID of the roadmap.</param>
        /// <returns>The count of sections.</returns>
        [HttpGet("count-on-roadmap")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSectionCountOnRoadmap([FromQuery] int roadmapId)
        {
            try
            {
                var sections = await repository.GetSectionsFromDbAsync();
                var count = sections.Count(s => s.RoadmapId == roadmapId);
                return Ok(new { result = count, message = "Successfully counted sections." });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Gets the last order number of sections in a specific roadmap.
        /// </summary>
        /// <param name="roadmapId">The ID of the roadmap.</param>
        /// <returns>The last order number.</returns>
        [HttpGet("last-from-roadmap")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastOrderNumberFromRoadmap([FromQuery] int roadmapId)
        {
            try
            {
                var sections = await repository.GetSectionsFromDbAsync();
                var sectionsInRoadmap = sections.Where(s => s.RoadmapId == roadmapId);
                var lastOrderNumber = sectionsInRoadmap.Any()
                    ? sectionsInRoadmap.Max(s => s.OrderNumber)
                    : 0;
                return Ok(new { result = lastOrderNumber, message = "Successfully retrieved last order number." });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        #endregion
    }
}