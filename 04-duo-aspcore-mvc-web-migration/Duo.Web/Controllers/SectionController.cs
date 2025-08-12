using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Duo.Web.Controllers
{
    public class SectionController : Controller
    {
        private readonly ILogger<SectionController> _logger;

        public SectionController(ILogger<SectionController> logger)
        {
            _logger = logger;
        }

        public IActionResult ManageSection()
        {
            return View("~/Views/Section/ManageSection.cshtml");
        }

        public IActionResult AddSection()
        {
            return View("~/Views/Section/AddSection.cshtml");
        }
    }
}