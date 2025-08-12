using System.Diagnostics.CodeAnalysis;
using Duo.Api.DTO.Requests;
using Duo.Api.Models;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Controller for managing modules in the system.
    /// Provides endpoints for CRUD operations and additional module-related functionalities.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ModuleController"/> class with the specified repository.
    /// </remarks>
    /// <param name="repository">The repository instance for data access.</param>
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class ModuleController(IRepository repository) : BaseController(repository)
    {
        #region Private Methods

        /// <summary>
        /// Gets the next available position for a module in a course.
        /// </summary>
        /// <param name="courseId">The ID of the course (nullable).</param>
        /// <returns>The next available position number.</returns>
        private async Task<int> GetLastPositionAsync(int? courseId)
        {
            if (courseId == null)
            {
                return 1;
            }

            var modules = await repository.GetModulesFromDbAsync();
            var positions = modules
                .Where(module => module.CourseId == courseId)
                .OrderBy(module => module.Position)
                .Select(module => module.Position)
                .ToList();

            if (positions.Count == 0)
            {
                return 1;
            }

            for (int i = 1; i <= positions.Count; i++)
            {
                if (positions[i - 1] != i)
                {
                    return i;
                }
            }

            return positions.Count + 1;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new module to the system.
        /// </summary>
        /// <param name="request">The module data to add.</param>
        /// <returns>ActionResult with operation result.</returns>
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddModule([FromForm] AddModuleRequest request)
        {
            try
            {
                var module = new Module
                {
                    Title = request.Title,
                    Description = request.Description,
                    IsBonus = request.IsBonus,
                    Cost = request.Cost,
                    ImageUrl = request.ImageUrl,
                    Position = await GetLastPositionAsync(request.CourseId),
                    CourseId = request.CourseId
                };

                await repository.AddModuleAsync(module);

                return Ok(new { message = "Module added successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Removes a module from the system.
        /// </summary>
        /// <param name="id">The ID of the module to remove.</param>
        /// <returns>ActionResult with operation result.</returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveModule(int id)
        {
            try
            {
                var module = await repository.GetModuleByIdAsync(id);
                if (module == null)
                {
                    return NotFound(new { message = "Module not found!" });
                }

                await repository.DeleteModuleAsync(id);
                return Ok(new { message = "Module removed successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Gets a module by its ID.
        /// </summary>
        /// <param name="id">The ID of the module to retrieve.</param>
        /// <returns>ActionResult with the module data or error message.</returns>
        [HttpGet("get-module-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var module = await repository.GetModuleByIdAsync(id);
                if (module == null)
                {
                    return NotFound(new { message = "Module not found!" });
                }

                return Ok(module);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Gets a list of all modules in the system.
        /// </summary>
        /// <returns>ActionResult with list of modules or error message.</returns>
        [HttpGet("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListModules()
        {
            try
            {
                var modules = await repository.GetModulesFromDbAsync();
                return Ok(modules);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Gets a list of modules belonging to a specific course.
        /// </summary>
        /// <param name="id">The ID of the course.</param>
        /// <returns>ActionResult with list of modules or error message.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("get-all/by-course/{courseId}")]
        public async Task<IActionResult> ListModulesByCourseId([FromRoute] int courseId)
        {
            try
            {
                var modules = await repository.GetModulesFromDbAsync();
                var filteredModules = modules.Where(m => m.CourseId == courseId).ToList();

                return Ok(filteredModules);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Updates an existing module.
        /// </summary>
        /// <param name="id">The ID of the module to update.</param>
        /// <param name="request">The updated module data.</param>
        /// <returns>ActionResult with operation result.</returns>
        [HttpPatch("modify/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateModule(int id, [FromForm] UpdateModuleRequest request)
        {
            try
            {
                var module = await repository.GetModuleByIdAsync(id);
                if (module == null)
                {
                    return NotFound(new { message = "Module not found!" });
                }

                module.Title = request.Title ?? module.Title;
                module.Description = request.Description ?? module.Description;
                module.IsBonus = request.IsBonus ?? module.IsBonus;
                module.Cost = request.Cost ?? module.Cost;
                module.ImageUrl = request.ImageUrl ?? module.ImageUrl;
                module.CourseId = request.CourseId ?? module.CourseId;

                await repository.UpdateModuleAsync(module);
                return Ok(new { message = "Module updated successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Checks if a module is completed by a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>True if completed, false otherwise.</returns>
        [HttpGet("get-completion-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsModuleCompleted([FromQuery] int userId, [FromQuery] int moduleId)
        {
            try
            {
                bool isCompleted = await repository.IsModuleCompletedAsync(userId, moduleId);
                return Ok(isCompleted);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("add-open-module")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OpenModule([FromBody] OpenModuleRequest request)
        {
            if (request == null || request.UserId <= 0 || request.ModuleId <= 0)
            {
                return BadRequest("Invalid user or module ID.");
            }

            try
            {
                Console.WriteLine($"User {request.UserId} opened module {request.ModuleId}");

                return Ok(new { message = "Module opened successfully!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("get-open-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsModuleOpen([FromQuery] int userId, [FromQuery] int moduleId)
        {
            if (userId <= 0 || moduleId <= 0)
            {
                return BadRequest("Invalid userId or moduleId");
            }

            try
            {
                bool isUnlocked = await repository.IsModuleOpenAsync(userId, moduleId);

                return Ok(isUnlocked);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion
    }
}
