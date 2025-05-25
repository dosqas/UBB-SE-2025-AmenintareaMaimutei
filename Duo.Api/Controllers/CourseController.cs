using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Duo.Api.Models;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Controller for managing courses in the system.
    /// Provides endpoints for adding, retrieving, updating, deleting, and managing user interactions with courses.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CourseController"/> class with the specified repository.
    /// </remarks>
    /// <param name="repository">The repository instance to be used for data access.</param>
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class CourseController(IRepository repository) : BaseController(repository)
    {
        #region Methods

        /// <summary>
        /// Adds a new course to the database.
        /// </summary>
        /// <param name="course">The course data to add.</param>
        /// <returns>The added course.</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddCourse([FromForm] Course course)
        {
            await repository.AddCourseAsync(course);
            return Ok(course);
        }

        /// <summary>
        /// Retrieves a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to retrieve.</param>
        /// <returns>The course if found; otherwise, NotFound.</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetCourse([FromQuery] int id)
        {
            var course = await repository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        /// <summary>
        /// Lists all courses in the database.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListCourses()
        {
            var courses = await repository.GetCoursesFromDbAsync();
            return Ok(courses);
        }

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="updatedCourse">The updated course data, including CourseId.</param>
        /// <returns>The updated course if found; otherwise, NotFound.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCourse([FromForm] Course updatedCourse)
        {
            var course = await repository.GetCourseByIdAsync(updatedCourse.CourseId);
            if (course == null)
            {
                return NotFound();
            }
            await repository.UpdateCourseAsync(updatedCourse);
            return Ok(updatedCourse);
        }

        /// <summary>
        /// Deletes a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to delete.</param>
        /// <returns>Ok if deleted; otherwise, NotFound.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCourse([FromQuery] int id)
        {
            var course = await repository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            await repository.DeleteCourseAsync(id);
            return Ok();
        }

        /// <summary>
        /// Enrolls a user in a course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>Ok if enrollment is successful.</returns>
        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] Dictionary<string, object> data)
        {
            if (!data.TryGetValue("userId", out var userIdObj) || !data.TryGetValue("courseId", out var courseIdObj))
            {
                return this.BadRequest("Missing userId or courseId");
            }

            var userId = ((JsonElement)data["userId"]).GetInt32();
            var courseId = ((JsonElement)data["courseId"]).GetInt32();

            await repository.EnrollUserInCourseAsync(userId, courseId);
            return Ok();
        }

        /// <summary>
        /// Checks if a user is enrolled in a course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if enrolled; otherwise, false.</returns>
        [HttpGet("is-enrolled")]
        public async Task<IActionResult> IsEnrolled([FromQuery] int userId, [FromQuery] int courseId)
        {
            var isEnrolled = await repository.IsUserEnrolledInCourseAsync(userId, courseId);
            return Ok(isEnrolled);
        }

        /// <summary>
        /// Checks if a course is completed by a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if completed; otherwise, false.</returns>
        [HttpGet("is-completed")]
        public async Task<IActionResult> IsCompleted([FromQuery] int userId, [FromQuery] int courseId)
        {
            var isCompleted = await repository.IsCourseCompletedAsync(userId, courseId);
            return Ok(isCompleted);
        }

        /// <summary>
        /// Filters courses based on search text, type, enrollment status, and tags.
        /// </summary>
        /// <param name="searchText">The text to search for in course titles.</param>
        /// <param name="filterPremium">Whether to filter premium courses.</param>
        /// <param name="filterFree">Whether to filter free courses.</param>
        /// <param name="filterEnrolled">Whether to filter enrolled courses.</param>
        /// <param name="filterNotEnrolled">Whether to filter not enrolled courses.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A filtered list of courses.</returns>
        [HttpGet("get-filtered")]
        public async Task<IActionResult> GetFilteredCourses(
            [FromQuery] string searchText,
            [FromQuery] bool filterPremium,
            [FromQuery] bool filterFree,
            [FromQuery] bool filterEnrolled,
            [FromQuery] bool filterNotEnrolled,
            [FromQuery] int userId)
        {
            var courses = await repository.GetFilteredCoursesAsync(searchText, filterPremium, filterFree, filterEnrolled, filterNotEnrolled, userId);
            return Ok(courses);
        }

        /// <summary>
        /// Updates the time a user spent on a course.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="courseId">The course ID.</param>
        /// <param name="timeInSeconds">The time in seconds to add.</param>
        /// <returns>Ok if updated.</returns>
        [HttpPut("update-time")]
        public async Task<IActionResult> UpdateTime([FromBody] Dictionary<string, object> data)
        {
            if (!data.TryGetValue("userId", out var userIdObj) || !data.TryGetValue("courseId", out var courseIdObj) || !data.TryGetValue("seconds", out var secondsObj))
            {
                return this.BadRequest("Missing one or more required parameters: userId, courseId, or seconds.");
            }

            var userId = ((JsonElement)data["userId"]).GetInt32();
            var courseId = ((JsonElement)data["courseId"]).GetInt32();
            var seconds = ((JsonElement)data["seconds"]).GetInt32();

            await this.repository.UpdateTimeSpentAsync(userId, courseId, seconds);
            return this.Ok();
        }

        /// <summary>
        /// Claims the completion reward for a course.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="courseId">The course ID.</param>
        /// <returns>Ok if reward claimed.</returns>
        [HttpPost("claim-completion")]
        public async Task<IActionResult> ClaimCompletionReward([FromBody] JsonElement body)
        {
            int userId = body.GetProperty("userId").GetInt32();
            int courseId = body.GetProperty("courseId").GetInt32();
            int coins = body.GetProperty("coins").GetInt32();
            await repository.ClaimCompletionRewardAsync(userId, courseId, coins);
            return Ok();
        }

        /// <summary>
        /// Claims the time reward for a course.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="courseId">The course ID.</param>
        /// <returns>Ok if reward claimed.</returns>
        [HttpPost("claim-time")]
        public async Task<IActionResult> ClaimTimeReward([FromBody] JsonElement body)
        {
            int userId = body.GetProperty("userId").GetInt32();
            int courseId = body.GetProperty("courseId").GetInt32();
            int timeSpent = body.GetProperty("timeSpent").GetInt32();
            int coins = body.GetProperty("coins").GetInt32();
            await repository.ClaimTimeRewardAsync(userId, courseId, coins);
            return Ok();
        }

        /// <summary>
        /// Gets the time a user has spent on a course.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="courseId">The course ID.</param>
        /// <returns>The time spent in seconds.</returns>
        [HttpGet("get-time")]
        public async Task<IActionResult> GetTimeSpent([FromQuery] int userId, [FromQuery] int courseId)
        {
            var timeSpent = await repository.GetTimeSpentAsync(userId, courseId);
            return Ok(timeSpent);
        }

        /// <summary>
        /// Gets the time limit of a course.
        /// </summary>
        /// <param name="courseId">The course ID.</param>
        /// <returns>The time limit in seconds.</returns>
        [HttpGet("get-time-limit")]
        public async Task<IActionResult> GetTimeLimit([FromForm] int courseId)
        {
            var course = await repository.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course.TimeToComplete);
        }

        [HttpGet("{courseId}/tags")]
        public async Task<IActionResult> GetTagsForCourse([FromRoute] int courseId)
        {
            var tags = await repository.GetTagsForCourseAsync(courseId);
            return Ok(tags);
        }

        [HttpPost("add-tag")]
        public async Task<IActionResult> AddTagToCourse([FromForm] int courseId, [FromForm] int tagId)
        {
            await repository.AddTagToCourseAsync(courseId, tagId);
            return Ok();
        }

        [HttpGet("completedModules")]
        public async Task<ActionResult<int>> GetCompletedModulesCount([FromQuery] int userId, [FromQuery] int courseId)
        {
            var count = await repository.GetCompletedModulesCountAsync(userId, courseId);
            return Ok(count);
        }

        [HttpGet("requiredModules")]
        public async Task<ActionResult<int>> GetRequiredModulesCount([FromQuery] int courseId)
        {
            int count = await repository.GetRequiredModulesCountAsync(courseId);
            return Ok(count);
        }

        [HttpGet("timeLimit")]
        public async Task<ActionResult<int>> GetCourseTimeLimit([FromQuery] int courseId)
        {
            int timeLimit = await repository.GetCourseTimeLimitAsync(courseId);
            return Ok(timeLimit);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteCourse([FromBody] JsonElement body)
        {
            int userId = body.GetProperty("userId").GetInt32();
            int courseId = body.GetProperty("courseId").GetInt32();
            await this.repository.CompleteCourseAsync(userId, courseId);
            return this.Ok();
        }

        [HttpPost("buyBonusModule")]
        public async Task<IActionResult> BuyBonusModule([FromBody] Dictionary<string, object> data)
        {
            try
            {
                if (!data.TryGetValue("UserId", out var userIdObj) || !data.TryGetValue("ModuleId", out var moduleIdObj))
                {
                    return this.BadRequest("Missing userId or courseId");
                }

                var userId = ((JsonElement)data["UserId"]).GetInt32();
                var moduleId = ((JsonElement)data["ModuleId"]).GetInt32();

                // Check if the user has enough coins
                var user = await this.repository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return this.NotFound("User not found");
                }

                var module = await this.repository.GetModuleByIdAsync(moduleId);
                if (module == null)
                {
                    return this.NotFound("Module not found");
                }

                if (user.CoinBalance < module.Cost)
                {
                    return this.BadRequest("Not enough coins");
                }

                var succes = await this.repository.BuyBonusModuleAsync(userId, moduleId);
                if (!succes)
                {
                    return this.BadRequest("Failed to buy bonus module");
                }

                // Deduct the cost from the user's coin balance
                user.CoinBalance -= module.Cost;
                await this.repository.UpdateUserAsync(user);

                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        #endregion
    }
}