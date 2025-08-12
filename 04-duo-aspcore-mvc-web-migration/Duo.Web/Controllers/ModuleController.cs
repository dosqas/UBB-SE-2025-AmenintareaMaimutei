using Duo.Web.Models;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Duo.Web.Controllers
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

        public async Task<IActionResult> Details(int id)
        {
            // Fetch the module details
            var module = await _courseService.GetModuleAsync(id);
            var userId = 1;

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
            var userId = 1;
            var module = await _courseService.GetModuleAsync(id);
            var courseId = module.CourseId;
            var CourseCompletionRewardCoins = 50;
            var totalSecondsSpentOnCourse = await _courseService.GetTimeSpentAsync(userId, courseId);
            var TimedCompletionRewardCoins = 300;

            try
            {
                await _courseService.CompleteModuleAsync(userId, id, courseId);

                if (await IsCourseCompleted(userId, courseId))
                {
                    if (await _courseService.ClaimCompletionRewardAsync(userId, courseId))
                    {
                        string message = $"Congratulations! You have completed all required modules in this course. {CourseCompletionRewardCoins} coins have been added to your balance.";
                        // alert(message)
                    }

                    if (await _courseService.ClaimTimedRewardAsync(userId, courseId, totalSecondsSpentOnCourse))
                    {
                        string message = $"Congratulations! You completed the course within the time limit. {TimedCompletionRewardCoins} coins have been added to your balance.";
                        // alert(message)
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error completing module: {e.Message}");
            }
            return RedirectToRoute("Module", new { id });
        }

        private async Task<Boolean> IsCourseCompleted(int userId, int courseId)
        {
            var completedModulesCount = await _courseService.GetCompletedModulesCountAsync(userId, courseId);
            var requiredModulesCount = await _courseService.GetRequiredModulesCountAsync(courseId);
            return completedModulesCount >= requiredModulesCount;
        }
    }
}
