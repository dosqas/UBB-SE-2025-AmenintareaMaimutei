using Microsoft.AspNetCore.Mvc;

public class QuizAdminController : Controller
{
    public IActionResult Index()
    {
        return View(); 
    }

    public IActionResult ViewQuizzes()
    {
        return View("~/Views/Quiz/ViewQuizzes.cshtml");
    }

    public IActionResult CreateQuiz()
      => View("~/Views/Quiz/CreateQuiz.cshtml");

}
