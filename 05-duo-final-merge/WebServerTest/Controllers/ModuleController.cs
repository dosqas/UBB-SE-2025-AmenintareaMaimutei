using WebServerTest.Models;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace WebServerTest.Controllers
{
    public class ModuleController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        private readonly ICoinsService _coinsService;

        public ModuleController(ICourseService courseService, IUserService userService, ICoinsService coinsService)
        {
            _courseService = courseService;
            _userService = userService;
            _coinsService = coinsService;
        }

        [HttpGet("/Module/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the module details
            var module = await _courseService.GetModuleAsync(id);
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            // Get the necessary details
            var timeSpent = await _courseService.GetTimeSpentAsync(userId, module.CourseId);
            var coinBalance = await _coinsService.GetCoinBalanceAsync(userId);
            var isCompleted = await _courseService.IsModuleCompletedAsync(userId, id);
            var isUnlocked = await GetModuleUnlockStatus(module, userId);

            // Create the view model with the required properties
            var viewModel = new ModuleViewModel
            {
                Module = module,
                CourseId = module.CourseId, 
                TimeSpent = TimeSpan.FromSeconds(timeSpent).ToString(@"hh\:mm\:ss"),
                CoinBalance = coinBalance.ToString(),
                IsCompleted = isCompleted,
                IsUnlocked = isUnlocked
            };

            // Return the view with the populated view model
            return View("Index", viewModel);
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
        public async Task<IActionResult> Complete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var module = await _courseService.GetModuleAsync(id);
            var courseId = module.CourseId;
            var CourseCompletionRewardCoins = 50;
            var totalSecondsSpentOnCourse = await _courseService.GetTimeSpentAsync(userId, courseId);
            var TimedCompletionRewardCoins = 300;

            try
            {
                await _courseService.CompleteModuleAsync(userId, id, courseId);

                if (await IsCourseCompleted(userId, courseId) && !module.IsBonus)
                {
                    if (await _courseService.ClaimCompletionRewardAsync(userId, courseId, CourseCompletionRewardCoins))
                    {
                        TempData["CompletionReward"] = $"Congratulations! You have completed all required modules in this course. {CourseCompletionRewardCoins} coins have been added to your balance.";
                    }

                    if (await _courseService.ClaimTimedRewardAsync(userId, courseId, totalSecondsSpentOnCourse, TimedCompletionRewardCoins))
                    {
                        TempData["TimedReward"] = $"Congratulations! You completed the course within the time limit. {TimedCompletionRewardCoins} coins have been added to your balance.";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error completing module: {e.Message}");
                TempData["Error"] = "An error occurred while completing the module.";
            }
            return RedirectToAction("Details", "Module", new { id });
        }

        [HttpGet("/Module/PurchaseBonusModule")]
        public async Task<IActionResult> PurchaseBonusModule(int moduleId)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (userId == 0)
            {
                TempData["Error"] = "You must be logged in to purchase modules.";
                return RedirectToAction("Login", "User");
            }

            var module = await _courseService.GetModuleAsync(moduleId);

            if (!module.IsBonus)
            {
                TempData["Error"] = "This module is not a bonus module.";
                return RedirectToAction("Preview", "Course", new { id = module.CourseId });
            }

            // ✅ Check if the user is enrolled
            var isEnrolled = await _courseService.IsUserEnrolledAsync(userId, module.CourseId);
            if (!isEnrolled)
            {
                TempData["Error"] = "You must be enrolled in the course to purchase bonus modules.";
                return RedirectToAction("CoursePreview", "Course", new { id = module.CourseId });
            }

            // ✅ Check if all required modules are completed
            var isCourseCompleted = await IsCourseCompleted(userId, module.CourseId);
            if (!isCourseCompleted)
            {
                TempData["Error"] = "You must complete all required modules before purchasing bonus modules.";
                return RedirectToAction("CoursePreview", "Course", new { id = module.CourseId });
            }

            // Attempt purchase
            var success = await _courseService.BuyBonusModuleAsync(userId, moduleId);
            if (success)
            {
                TempData["Notification"] = "Bonus module purchased successfully!";
            }
            else
            {
                TempData["Error"] = "Not enough coins or module already unlocked.";
            }

            return RedirectToAction("CoursePreview", "Course", new { id = module.CourseId });
        }

        private async Task<Boolean> IsCourseCompleted(int userId, int courseId)
        {
            var completedModulesCount = await _courseService.GetCompletedModulesCountAsync(userId, courseId);
            var requiredModulesCount = await _courseService.GetRequiredModulesCountAsync(courseId);
            return completedModulesCount == requiredModulesCount;
        }

    }
}
