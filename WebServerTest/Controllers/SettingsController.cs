using System.Threading.Tasks;
using DuoClassLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebServerTest.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly IUserHelperService _userHelperService;
        private readonly IProfileService _profileService;
        private readonly ICategoryService _categoryService;

        public SettingsController(
            ILogger<SettingsController> logger,
            IUserHelperService userHelperService,
            IProfileService profileService,
            ICategoryService categoryService)
        {
            _logger = logger;
            _userHelperService = userHelperService;
            _profileService = profileService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            // Check if user is authenticated by checking session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                // Redirect to login if not authenticated
                return RedirectToAction("Login", "Account");
            }

            // Get categories for the Community dropdown in navbar
            ViewBag.Categories = await _categoryService.GetAllCategories();

            // Get user details
            var user = await _userHelperService.GetUserById(userId.Value);
            if (user == null)
            {
                // If user not found, redirect to login
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveChanges(bool isPrivate)
        {
            // Check if user is authenticated
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Get current user
                var user = await _userHelperService.GetUserById(userId.Value);
                if (user == null)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "Account");
                }

                // Update user privacy setting
                user.PrivacyStatus = isPrivate;
                await _profileService.UpdateUser(user);

                // Set success message
                TempData["SuccessMessage"] = "Your settings have been saved successfully.";
                return RedirectToAction("Index");
            }
            catch
            {
                // Set error message
                TempData["ErrorMessage"] = "An error occurred while saving your settings.";
                return RedirectToAction("Index");
            }
        }
    }
} 