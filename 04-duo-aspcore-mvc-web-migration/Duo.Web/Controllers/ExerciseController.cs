using DuoClassLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using DuoClassLibrary.Models;
using DuoClassLibrary.Models.Exercises;
using System.Text.Json;


namespace Duo.Web.Controllers;
public class ExerciseController : Controller
{
    private readonly ILogger<ExerciseController> _logger;
    private readonly IExerciseService _exerciseService;

    public ExerciseController(ILogger<ExerciseController> logger, IExerciseService exerciseService)
    {
        _logger = logger;
        _exerciseService = exerciseService;
    }

    public IActionResult Index()
    {
        // Return the index view that lists all exercises
        return View("~/Views/Exercise/Index.cshtml");
    }

    public IActionResult AssociationExercise()
    {
        // Using mock data in the view for now
        return View("~/Views/Exercise/AssociationExercise.cshtml");
    }

    public IActionResult FillBlanksExercise()
    {
        // Using mock data in the view for now
        return View("~/Views/Exercise/FillBlanksExercise.cshtml");
    }

    public IActionResult MultipleChoiceExercise()
    {
        // Using mock data in the view for now
        return View("~/Views/Exercise/MultipleChoiceExercise.cshtml");
    }

    public IActionResult FlashcardExercise()
    {
        // Using mock data in the view for now
        return View("~/Views/Exercise/FlashcardExercise.cshtml");
    }

    public IActionResult CreateExercise()
    {
        ViewBag.Difficulties = Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>().ToArray();
        ViewBag.ExerciseTypes = ExerciseTypes.EXERCISE_TYPES;
        return View("~/Views/Exercise/CreateExercise.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JsonElement exerciseJson)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        string json = exerciseJson.GetRawText();

        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("Type", out var typeProp))
        {
            return BadRequest("Missing 'type' discriminator.");
        }

        var type = typeProp.GetString();

        Exercise? exercise = type switch
        {
            "Flashcard" => JsonSerializer.Deserialize<FlashcardExercise>(json),
            "MultipleChoice" => JsonSerializer.Deserialize<MultipleChoiceExercise>(json),
            "FillInTheBlank" => JsonSerializer.Deserialize<FillInTheBlankExercise>(json),
            "Association" => JsonSerializer.Deserialize<AssociationExercise>(json),
            _ => null
        };

        if (exercise == null)
        {
            return this.BadRequest("Invalid payload.");
        }

        await _exerciseService.CreateExercise(exercise);

        // Return success (could also return the created entity or a redirect URL)
        return Ok(new { message = "Exercise created successfully" });
    }


    public IActionResult ViewExercises()
    {
        return View("~/Views/Exercise/ManageExercise.cshtml");
    }

    [HttpGet]
    public IActionResult GetExerciseForm(string type)
    {
        switch (type)
        {
            case "Association":
                return PartialView("_AssociationExerciseCreateForm");
            case "Fill in the blank":
                return PartialView("_FillInTheBlankExerciseCreateForm");
            case "Multiple Choice":
                return PartialView("_MultipleChoiceExerciseCreateForm");
            case "Flashcard":
                return PartialView("_FlashcardExerciseCreateForm", new FlashcardExercise());
            default:
                return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Manage()
    {
        var exercises = await _exerciseService.GetAllExercises();
        return View("~/Views/Exercise/ManageExercise.cshtml", exercises);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _exerciseService.DeleteExercise(id);
        return RedirectToAction("Manage");
    }
}
