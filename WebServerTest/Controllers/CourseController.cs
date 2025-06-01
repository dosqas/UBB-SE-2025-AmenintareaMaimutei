using Microsoft.AspNetCore.Mvc;
using DuoClassLibrary.Services;
using System.Security.Claims;
using DuoClassLibrary.Models;
using WebServerTest.Models;
using DuoClassLibrary.Services.Interfaces;
using System.Text.Json;
using System.Numerics;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace WebServerTest.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICoinsService _coinsService;
        private readonly IUserService _userService;

        public CourseController(ICourseService courseService, ICoinsService coinsService)
        {
            _courseService = courseService;
            _coinsService = coinsService;
        }

        public async Task<IActionResult> ViewCourses()
        {
            var courses = await _courseService.GetCoursesAsync();
            foreach (var course in courses)
            {
                course.Tags = await _courseService.GetCourseTagsAsync(course.CourseId);
            }
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _courseService.GetTagsAsync();
            return Json(tags);
        }

        public async Task<IActionResult> CoursePreview(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            // Fetch course data
            var course = await _courseService.GetCourseAsync(id);
            var modules = await _courseService.GetModulesAsync(id);
            var isEnrolled = await _courseService.IsUserEnrolledAsync(userId, id);
            var completedModules = await _courseService.GetCompletedModulesCountAsync(userId, id);
            var requiredModules = await _courseService.GetRequiredModulesCountAsync(id);
            var coinBalance = await _coinsService.GetCoinBalanceAsync(userId);
            var tags = await _courseService.GetCourseTagsAsync(id);

            // Prepare module view models
            var moduleViewModels = new List<ModuleViewModel>();

            foreach (var module in modules)
            {
                bool isUnlocked = await GetModuleUnlockStatus(module, userId);
                bool isCompleted = await _courseService.IsModuleCompletedAsync(userId, module.ModuleId);

                // Add module details to the view model
                moduleViewModels.Add(new ModuleViewModel
                {
                    Module = module,
                    CourseId = id,  // Course ID is still relevant for context
                    IsCompleted = isCompleted,
                    IsUnlocked = isUnlocked,
                    IsBonus = module.IsBonus,
                    TimeSpent = await _courseService.GetTimeSpentAsync(userId, id).ContinueWith(t =>
                        TimeSpan.FromSeconds(t.Result).ToString(@"hh\:mm\:ss")), // Format time spent
                    CoinBalance = coinBalance.ToString()  // Assuming coin balance is an integer
                });
            }

            // Calculate the remaining time for the course (in seconds)
            int timeRemainingSeconds = await _courseService.GetCourseTimeLimitAsync(id) - await _courseService.GetTimeSpentAsync(userId, id);
            string formattedTimeRemaining = TimeSpan.FromSeconds(Math.Max(timeRemainingSeconds, 0)).ToString(@"hh\:mm\:ss");

            // Create the view model for the course preview
            var viewModel = new CoursePreviewViewModel
            {
                Course = course,
                Modules = moduleViewModels,
                IsEnrolled = isEnrolled,
                CompletedModules = completedModules,
                RequiredModules = requiredModules,
                CoinBalance = coinBalance,
                Tags = tags,
                FormattedTimeRemaining = formattedTimeRemaining
            };

            return View("ViewCoursePreview", viewModel);
        }

        private async Task<bool> GetModuleUnlockStatus(Module module, int currentUserId)
        {
            var modules = await _courseService.GetModulesAsync(module.CourseId);
            int moduleIndex = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].ModuleId == module.ModuleId)
                {
                    moduleIndex = i;
                    break;
                }
            }
            try
            {
                var IsEnrolled = await _courseService.IsUserEnrolledAsync(currentUserId, module.CourseId);
                if (!module.IsBonus)
                {
                    return IsEnrolled &&
                           (moduleIndex == 0 ||
                            await _courseService.IsModuleCompletedAsync(currentUserId, modules[moduleIndex - 1].ModuleId));
                }
                return await _courseService.IsModuleInProgressAsync(currentUserId, module.ModuleId);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0; ;
            var CurrentCourse = await _courseService.GetCourseAsync(id);
            var CoinBalance = await _coinsService.GetCoinBalanceAsync(userId);
            var IsEnrolled = await _courseService.IsUserEnrolledAsync(userId, id);

            if (!IsEnrolled && (CurrentCourse.Cost == 0 || CoinBalance >= CurrentCourse.Cost))
            {
                if (CurrentCourse.Cost > 0)
                {
                    bool coinDeductionSuccessful = await _coinsService.TrySpendingCoinsAsync(userId, CurrentCourse.Cost);
                }

                bool enrollmentSuccessful = await _courseService.EnrollInCourseAsync(userId, CurrentCourse.CourseId);

                CoinBalance = await _coinsService.GetCoinBalanceAsync(userId);
                IsEnrolled = true;

                // Start Course Progress Timer ...
            }

            return RedirectToAction("CoursePreview", new { id = CurrentCourse.CourseId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTimeLocal([FromBody] Dictionary<string, object> data)
        {
            // Extract values safely
            if (!data.TryGetValue("courseId", out var courseIdObj) ||
                !data.TryGetValue("seconds", out var secondsObj))
            {
                return BadRequest("Missing one or more required parameters: courseId, or seconds.");
            }

            // Extract from JsonElement
            int courseId, seconds;

            if (courseIdObj is JsonElement courseIdElement)
            {
                courseId = courseIdElement.ValueKind == JsonValueKind.Number
                    ? courseIdElement.GetInt32()
                    : int.Parse(courseIdElement.GetString());
            }
            else
            {
                courseId = Convert.ToInt32(courseIdObj);
            }

            if (secondsObj is JsonElement secondsElement)
            {
                seconds = secondsElement.ValueKind == JsonValueKind.Number
                    ? secondsElement.GetInt32()
                    : int.Parse(secondsElement.GetString());
            }
            else
            {
                seconds = Convert.ToInt32(secondsObj);
            }
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0; ;
            await _courseService.UpdateTimeSpentAsync(userId, courseId, seconds);
            return Ok();
        }
    }
}