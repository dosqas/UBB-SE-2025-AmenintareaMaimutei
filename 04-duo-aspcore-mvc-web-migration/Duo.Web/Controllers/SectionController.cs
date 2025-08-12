using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DuoClassLibrary.Services;
using DuoClassLibrary.Models.Sections;
using DuoClassLibrary.Models.Quizzes;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Duo.Web.Controllers
{
    public class SectionController : Controller
    {
        private readonly ILogger<SectionController> _logger;
        private readonly ISectionService _sectionService;
        private readonly IQuizService _quizService;

        public SectionController(ILogger<SectionController> logger, ISectionService sectionService, IQuizService quizService)
        {
            _logger = logger;
            _sectionService = sectionService;
            _quizService = quizService;
        }

        public async Task<IActionResult> ManageSection()
        {
            List<Section> sections = await _sectionService.GetAllSections();
            return View("~/Views/Section/ManageSection.cshtml", sections);
        }

        [HttpGet]
        public async Task<IActionResult> GetSectionQuizzes(int id)
        {
            try
            {
                var section = await _sectionService.GetSectionById(id);
                if (section == null)
                {
                    return NotFound(new { message = "Section not found" });
                }

                var quizzes = await _quizService.GetAllQuizzesFromSection(id);
                return Json(quizzes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quizzes for section {SectionId}", id);
                return StatusCode(500, new { message = "Error fetching quizzes" });
            }
        }

        public async Task<IActionResult> AddSection()
        {
            ViewBag.Quizzes = await _quizService.GetAllAvailableQuizzes();
            ViewBag.Exams = await _quizService.GetAllAvailableExams();
            return View("~/Views/Section/AddSection.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddSection([FromBody] Section section)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid section data" });

            try
            {
                // Create a new section with the provided data
                var newSection = new Section
                {
                    Title = section.Title,
                    Description = section.Description,
                    RoadmapId = section.RoadmapId,
                    SubjectId = section.SubjectId,
                    OrderNumber = section.OrderNumber,
                    Quizzes = new List<Quiz>(),
                    Exam = null
                };

                // Add selected quizzes
                foreach (var quiz in section.Quizzes)
                {
                    var quizData = await _quizService.GetQuizById(quiz.Id);
                    if (quizData != null)
                    {
                        newSection.Quizzes.Add(quizData);
                    }
                }

                // Add selected exam
                if (section.Exam != null)
                {
                    var examData = await _quizService.GetExamById(section.Exam.Id);
                    if (examData != null)
                    {
                        newSection.Exam = examData;
                    }
                }

                // Save the section
                await _sectionService.AddSection(newSection);
                return Ok(new { message = "Section created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating section");
                return BadRequest(new { message = "Failed to create section: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSection(int id)
        {
            await _sectionService.DeleteSection(id);
            return RedirectToAction("ManageSection");
        }

        public async Task<IActionResult> EditSection(int id)
        {
            var section = await _sectionService.GetSectionById(id);
            if (section == null)
                return NotFound();
            return View("~/Views/Section/EditSection.cshtml", section);
        }

        [HttpPost]
        public async Task<IActionResult> EditSection(Section section)
        {
            if (!ModelState.IsValid)
                return View(section);
            await _sectionService.UpdateSection(section);
            return RedirectToAction("ManageSection");
        }
    }
}
