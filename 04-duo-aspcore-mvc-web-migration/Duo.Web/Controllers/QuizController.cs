using Microsoft.AspNetCore.Mvc;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Exercises;
using Duo.Web.ViewModels;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using DuoClassLibrary.Services;
using Microsoft.Extensions.Logging;

namespace Duo.Web.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IExerciseService _exerciseService;
        private readonly ISectionService _sectionService;
        private readonly IUserService _userService;
        private readonly ILogger<QuizController> _logger;

        public QuizController(
            IQuizService quizService,
            IExerciseService exerciseService,
            ISectionService sectionService,
            IUserService userService,
            ILogger<QuizController> logger)
        {
            _quizService = quizService;
            _exerciseService = exerciseService;
            _sectionService = sectionService;
            _userService = userService;
            _logger = logger;
        }

        // GET /Quiz/ViewQuizzes?selectedQuizId=5
        [HttpGet]
        public async Task<IActionResult> ViewQuizzes(int? selectedQuizId)
        {
            var allQuizzes = await _quizService.GetAllQuizzes();
            var vm = new ManageQuizViewModel
            {
                Quizzes = allQuizzes,
                SelectedQuizId = selectedQuizId
            };

            if (selectedQuizId.HasValue && selectedQuizId > 0)
            {
                if (!allQuizzes.Any(q => q.Id == selectedQuizId.Value))
                {
                    TempData["Warning"] = $"Quiz with ID {selectedQuizId} no longer exists.";
                    vm.SelectedQuizId = null;
                    vm.AssignedExercises = new List<Exercise>();
                    vm.AvailableExercises = new List<Exercise>();
                    return View(vm);
                }

                try
                {
                    vm.AssignedExercises = await _exerciseService
                        .GetAllExercisesFromQuiz(selectedQuizId.Value);
                    var all = await _exerciseService.GetAllExercises();
                    vm.AvailableExercises = all
                        .Where(e => !vm.AssignedExercises.Any(a => a.ExerciseId == e.ExerciseId))
                        .ToList();
                }
                catch
                {
                    vm.AssignedExercises = new List<Exercise>();
                    vm.AvailableExercises = new List<Exercise>();
                    TempData["Error"] = "Could not load exercises for this quiz.";
                }
            }

            return View(vm);
        }

        // GET: Quiz/Preview/{id}
        [HttpGet]
        public async Task<IActionResult> Preview(int id)
        {
            try
            {
                var allQuizzes = await _quizService.GetAllQuizzes();
                var exam = allQuizzes.FirstOrDefault(e => e.Id == id);
                
                if (exam != null)
                {
                    var vm = new QuizPreviewViewModel
                    {
                        Quiz = exam,
                        ExerciseIds = exam.Exercises.Select(e => e.ExerciseId).ToList(),
                        SectionTitle = exam.SectionId.HasValue
                            ? (await _sectionService.GetSectionById(exam.SectionId.Value))?.Title
                              ?? "Section:"
                            : "Section:"
                    };
                    return View("QuizPreview", vm);
                }

                var quiz = await _quizService.GetQuizById(id);
                if (quiz == null) return NotFound();

                var viewModel = new QuizPreviewViewModel
                {
                    Quiz = quiz,
                    ExerciseIds = quiz.Exercises.Select(e => e.ExerciseId).ToList(),
                    SectionTitle = quiz.SectionId.HasValue
                        ? (await _sectionService.GetSectionById(quiz.SectionId.Value))?.Title
                          ?? "Section:"
                        : "Section:"
                };

                return View("QuizPreview", viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Preview: {ex.Message}");
                throw;
            }
        }

        // GET: Quiz/Solve/{id}
        [HttpGet]
        public async Task<IActionResult> Solve(int id, int? index)
        {
            try
            {
                BaseQuiz quiz;
                
                var allExams = await _quizService.GetAllQuizzes();
                quiz = allExams.FirstOrDefault(e => e.Id == id);
                
                if (quiz == null)
                {
                    quiz = await _quizService.GetQuizById(id);
                    if (quiz == null)
                    {
                        return NotFound($"Quiz or exam with ID {id} not found.");
                    }
                }

                if (quiz.Exercises == null || !quiz.Exercises.Any())
                {
                    return NotFound($"No exercises found for {(quiz is Exam ? "exam" : "quiz")} {id}.");
                }

                int idx = index.GetValueOrDefault(0);
                if (idx < 0 || idx >= quiz.Exercises.Count)
                {
                    return RedirectToAction(nameof(Solve), new { id, index = 0 });
                }

                var vm = new QuizSolverViewModel
                {
                    QuizId = quiz.Id,
                    QuizTitle = quiz is Exam ? "Final Exam" : $"Quiz {quiz.Id}",
                    AllExercises = quiz.Exercises.ToList(),
                    CurrentExerciseIndex = idx,
                    CurrentExercise = quiz.Exercises[idx],
                    CurrentExerciseType = quiz.Exercises[idx].GetType().Name,
                    IsLastExercise = (idx == quiz.Exercises.Count - 1)
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Solve action for ID {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: Quiz/Solve/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Solve(
            int id,
            int index,
            string? FlashcardAnswer,        
            string? AssociationPairsJson,
            string? BlankAnswersJson,
            string? mcChoice)
        {
            try
            {
                BaseQuiz quiz;
                
                var allExams = await _quizService.GetAllQuizzes();
                quiz = allExams.FirstOrDefault(e => e.Id == id);
                
                if (quiz == null)
                {
                    quiz = await _quizService.GetQuizById(id);
                    if (quiz == null)
                    {
                        return NotFound($"Quiz or exam with ID {id} not found.");
                    }
                }

                if (index < 0 || index >= quiz.Exercises.Count)
                {
                    return NotFound($"Invalid exercise index {index} for {(quiz is Exam ? "exam" : "quiz")} {id}.");
                }

                var current = quiz.Exercises[index];
                bool valid = false;

                if (current is FlashcardExercise fc && !string.IsNullOrEmpty(FlashcardAnswer))
                {
                    valid = fc.ValidateAnswer(FlashcardAnswer);
                }
                else if (!string.IsNullOrEmpty(BlankAnswersJson) && current is FillInTheBlankExercise fib)
                {
                    var blanks = JsonSerializer.Deserialize<List<string>>(BlankAnswersJson)!;
                    valid = fib.ValidateAnswer(blanks);
                }
                else if (!string.IsNullOrEmpty(AssociationPairsJson) && current is AssociationExercise assoc)
                {
                    var rawPairs = JsonSerializer.Deserialize<List<AssociationPair>>(AssociationPairsJson)!;
                    var mapped = rawPairs
                        .Select(p => (assoc.FirstAnswersList[p.left], assoc.SecondAnswersList[p.right]))
                        .ToList();
                    valid = assoc.ValidateAnswer(mapped);
                }
                else if (!string.IsNullOrEmpty(mcChoice) && current is MultipleChoiceExercise mc)
                {
                    valid = mc.ValidateAnswer(new List<string> { mcChoice });
                }

                if (!TempData.ContainsKey("TotalQuestions"))
                {
                    TempData["TotalQuestions"] = quiz.Exercises.Count;
                    TempData["QuizId"] = id;
                    TempData["CorrectAnswers"] = 0;
                }
                
                //if (valid)
                //{
                //    int currentCorrect = TempData.Peek("CorrectAnswers") as int? ?? 0;
                //    TempData["CorrectAnswers"] = currentCorrect + 1;
                //}

                int next = index + 1;
                if (next >= quiz.Exercises.Count)
                {
                    return RedirectToAction(nameof(EndQuiz));
                }

                return RedirectToAction(nameof(Solve), new { id, index = next });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Solve POST action for ID {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET /Quiz/EndQuiz
        [HttpGet]
        public async Task<IActionResult> EndQuiz()
        {
            try
            {
                int correctAnswers = TempData["CorrectAnswers"] as int? ?? 0;
                int totalQuestions = TempData["TotalQuestions"] as int? ?? 0;
                int quizId = TempData["QuizId"] as int? ?? 0;
                int passingPercent = 100;

                double percentage = totalQuestions > 0 ? (double)correctAnswers / totalQuestions * 100 : 0;
                bool isPassed = correctAnswers == totalQuestions;

                var viewModel = new QuizEndViewModel
                {
                    CorrectAnswersText = $"{correctAnswers} / {totalQuestions}",
                    PassingPercentText = "100% needed to pass",
                    IsPassedText = isPassed ? "Quiz passed!" : "You need to redo this one.",
                    IsPassed = isPassed
                };

                if (isPassed && quizId > 0)
                {
                    var quiz = await _quizService.GetQuizById(quizId);
                    if (quiz?.SectionId != null)
                    {
                        var section = await _sectionService.GetSectionById(quiz.SectionId.Value);
                        if (section != null)
                        {
                            var user = await _userService.GetByIdAsync(1);
                            var quizzesInSection = (await _quizService.GetAllQuizzesFromSection(section.Id)).ToList();

                            await _quizService.CompleteQuiz(1, quizId);
                        }
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in EndQuiz: {ex.Message}");
                throw;
            }
        }

        // POST /Quiz/AddExercise
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExercise(int quizId, int exerciseId)
        {
            await _quizService.AddExerciseToQuiz(quizId, exerciseId);
            return RedirectToAction(nameof(ViewQuizzes), new { selectedQuizId = quizId });
        }

        // POST /Quiz/RemoveExercise
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveExercise(int quizId, int exerciseId)
        {
            await _quizService.RemoveExerciseFromQuiz(quizId, exerciseId);
            return RedirectToAction(nameof(ViewQuizzes), new { selectedQuizId = quizId });
        }

        // POST /Quiz/DeleteQuiz/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            await _quizService.DeleteQuiz(id);
            return RedirectToAction(nameof(ViewQuizzes));
        }

        // GET /Quiz/CreateQuiz
        [HttpGet]
        public async Task<IActionResult> CreateQuiz()
        {
            TempData["SelectedExerciseIds"] = new List<int>();
            TempData.Keep("SelectedExerciseIds");
            await SetCreateQuizViewData();
            return View();
        }

        // POST /Quiz/AddSelectedExercise
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSelectedExercise(List<int> exerciseIds)
        {
            var selected = TempData["SelectedExerciseIds"] as List<int> ?? new();
            foreach (var id in exerciseIds)
                if (!selected.Contains(id))
                    selected.Add(id);

            TempData["SelectedExerciseIds"] = selected;
            TempData.Keep("SelectedExerciseIds");
            await SetCreateQuizViewData();
            return View("CreateQuiz");
        }

        // POST /Quiz/RemoveSelectedExercise
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RemoveSelectedExercise(int exerciseId)
        {
            var selected = TempData["SelectedExerciseIds"] as List<int> ?? new();
            selected.Remove(exerciseId);
            TempData["SelectedExerciseIds"] = selected;
            TempData.Keep("SelectedExerciseIds");
            return Json(new { success = true, remainingIds = selected });
        }

        // POST /Quiz/CreateQuizConfirmed
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuizConfirmed(List<int> SelectedExerciseIds)
        {
            if (SelectedExerciseIds == null || !SelectedExerciseIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one exercise.");
                await SetCreateQuizViewData();
                return View("CreateQuiz");
            }

            var quiz = new Quiz(0, null, null);
            await _quizService.CreateQuiz(quiz);
            var allQuizzes = await _quizService.GetAllQuizzes();
            int newQuizId = allQuizzes.Max(q => q.Id);
            foreach (var exId in SelectedExerciseIds)
                await _quizService.AddExerciseToQuiz(newQuizId, exId);

            return RedirectToAction(nameof(ViewQuizzes), new { selectedQuizId = newQuizId });
        }

        // GET /Quiz/GetAvailableExercisesModal
        [HttpGet]
        public async Task<IActionResult> GetAvailableExercisesModal()
        {
            var all = await _exerciseService.GetAllExercises();
            var selectedIds = TempData["SelectedExerciseIds"] as List<int> ?? new();
            TempData.Keep("SelectedExerciseIds");
            var available = all.Where(e => !selectedIds.Contains(e.ExerciseId)).ToList();
            return PartialView("_AddExerciseModal", available);
        }

        // Helper DTO for association JSON
        private class AssociationPair
        {
            public int left { get; set; }
            public int right { get; set; }
        }

        private async Task SetCreateQuizViewData()
        {
            ViewBag.AvailableExercises = await _exerciseService.GetAllExercises();
            TempData.Keep("SelectedExerciseIds");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Quiz/ValidateAnswer/{quizId}/{exerciseIndex}")]
        public async Task<IActionResult> ValidateAnswer(int quizId, int exerciseIndex, [FromBody] AnswerSubmissionModel submission)
        {
            try
            {
                var quiz = await _quizService.GetQuizById(quizId);
                if (quiz == null || exerciseIndex >= quiz.Exercises.Count)
                {
                    return BadRequest(new { error = "Quiz or exercise not found" });
                }

                var exercise = quiz.Exercises[exerciseIndex];
                bool isCorrect = false;
                string correctAnswer = "";

                switch (exercise)
                {
                    case FlashcardExercise flashcard:
                        isCorrect = flashcard.ValidateAnswer(submission.Answer);
                        correctAnswer = flashcard.GetCorrectAnswer();
                        break;

                    case AssociationExercise association:
                        var associationPairs = JsonSerializer.Deserialize<List<AssociationPair>>(submission.Answer)!;
                        var mappedPairs = associationPairs
                            .Select(p => (association.FirstAnswersList[p.left], association.SecondAnswersList[p.right]))
                            .ToList();
                        isCorrect = association.ValidateAnswer(mappedPairs);
                        correctAnswer = string.Join(" | ", association.FirstAnswersList.Zip(association.SecondAnswersList, (a, b) => $"{a} → {b}"));
                        break;

                    case MultipleChoiceExercise multipleChoice:
                        var mcAnswer = new List<string> { submission.Answer };
                        isCorrect = multipleChoice.ValidateAnswer(mcAnswer);
                        correctAnswer = string.Join(" | ", multipleChoice.Choices.Where(c => c.IsCorrect).Select(c => c.Answer));
                        break;

                    case FillInTheBlankExercise fillInBlank:
                        var blankAnswers = JsonSerializer.Deserialize<List<string>>(submission.Answer)!;
                        isCorrect = fillInBlank.ValidateAnswer(blankAnswers);
                        correctAnswer = string.Join(" | ", fillInBlank.PossibleCorrectAnswers);
                        break;
                }

                if (!TempData.ContainsKey("TotalQuestions"))
                {
                    TempData["TotalQuestions"] = quiz.Exercises.Count;
                    TempData["QuizId"] = quizId;
                    TempData["CorrectAnswers"] = 0;
                }

                int currentCorrect = TempData.Peek("CorrectAnswers") as int? ?? 0;
                if (isCorrect)
                {
                    TempData["CorrectAnswers"] = currentCorrect + 1;
                }

                TempData.Keep("TotalQuestions");
                TempData.Keep("QuizId");
                TempData.Keep("CorrectAnswers");

                return Json(new { isCorrect, correctAnswer });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class AnswerSubmissionModel
    {
        public string Answer { get; set; }
    }
}
