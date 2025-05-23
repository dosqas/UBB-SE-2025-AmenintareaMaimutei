using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DuoClassLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using WebServerTest.Models;

namespace WebServerTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;

        public HomeController(
            ILogger<HomeController> logger,
            ICategoryService categoryService)
        {
            _logger = logger;
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

            // Get categories for the Community dropdown
            ViewBag.Categories = await _categoryService.GetAllCategories();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
