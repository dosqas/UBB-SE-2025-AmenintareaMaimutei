using Microsoft.AspNetCore.Mvc;

namespace Duo.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 