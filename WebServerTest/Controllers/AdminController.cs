using Microsoft.AspNetCore.Mvc;

namespace WebServerTest.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 