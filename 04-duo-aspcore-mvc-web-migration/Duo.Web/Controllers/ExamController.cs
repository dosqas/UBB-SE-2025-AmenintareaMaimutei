using Duo.Web.ViewModels;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Quizzes.API;
using DuoClassLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Duo.Web.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExerciseServiceProxy _exerciseService;
        private readonly IQuizServiceProxy _quizService;
        private readonly ISectionService _sectionService;
        private readonly ILogger<ExamController> _logger;

        public ExamController(IExerciseServiceProxy exerciseService, IQuizServiceProxy quizService, ISectionService sectionService)
        {
            _exerciseService = exerciseService;
            _quizService = quizService;
            _sectionService = sectionService;
        }

        // GET: /Exam/AddExam
        public async Task<IActionResult> AddExam()
        {
            var allExercises = await _exerciseService.GetAllExercises();
            ViewBag.AllExercises = allExercises;
            return View();
        }

        // POST: /Exam/CreateExam
        [HttpPost]
        public async Task<IActionResult> CreateExam([FromBody] List<int> selectedExerciseIds)
        {
            var newExam = new Exam(0, null);
            Exam createdExam = await _quizService.CreateExamAsync(newExam);

            var allExams = await _quizService.GetAllExams();
            int newExamId = allExams.Max(q => q.Id);

            foreach (var id in selectedExerciseIds)
            {
                await _quizService.AddExerciseToExamAsync(newExamId, id);
            }

            return RedirectToAction("ManageExam");
        }


        // GET: /Exam/ManageExam
        public async Task<IActionResult> ManageExam()
        {
            var exams = await _quizService.GetAllExams();
            var allExercises = await _exerciseService.GetAllExercises();

            ViewBag.Exams = exams;
            ViewBag.AllExercises = allExercises;

            return View();
        }

        // GET: /Exam/GetExercisesForExam/{id}
        [HttpGet]
        public async Task<IActionResult> GetExercisesForExam(int id)
        {
            var exercises = await _exerciseService.GetAllExercisesFromExam(id);
            return PartialView("_SelectedExercisesPartial", exercises);
        }

        // POST: /Exam/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _quizService.DeleteExamAsync(id);
            return RedirectToAction("ManageExam");
        }

        // POST: /Exam/RemoveExerciseFromExam
        [HttpPost]
        public async Task<IActionResult> RemoveExerciseFromExam([FromBody] ExamExerciseDto dto)
        {
            await _quizService.RemoveExerciseFromExamAsync(dto.ExamId, dto.ExerciseId);
            return Ok();
        }

        // POST: /Exam/AddExerciseToExam
        [HttpPost]
        public async Task<IActionResult> AddExerciseToExam([FromBody] ExamExerciseDto dto)
        {
            await _quizService.AddExerciseToExamAsync(dto.ExamId, dto.ExerciseId);
            return Ok();
        }

        // POST: /Exam/RenderSelectedExercises
        [HttpPost]
        public IActionResult RenderSelectedExercises([FromBody] List<ExerciseDto> selectedExercises)
        {
            var proxyExercises = selectedExercises
                .Select(dto => new MultipleChoiceExercise
                {
                    ExerciseId = dto.ExerciseId,
                    Question = dto.Question,
                    Type = "MultipleChoice"
                })
                .ToList<Exercise>();

            return PartialView("_SelectedExercisesPartial", proxyExercises);
        }

        // GET: Exam/Preview/{id}
        [HttpGet]
        public async Task<IActionResult> Preview(int id)
        {
            try
            {
                var allExams = await _quizService.GetAllExams();
                var exam = allExams.FirstOrDefault(e => e.Id == id);

                if (exam != null)
                {
                    var vm = new ExamPreviewViewModel
                    {
                        Exam = exam,
                        ExerciseIds = exam.Exercises.Select(e => e.ExerciseId).ToList(),
                        SectionTitle = exam.SectionId.HasValue
                            ? (await _sectionService.GetSectionById(exam.SectionId.Value))?.Title
                              ?? "Section:"
                            : "Section:"
                    };
                    return View("ExamPreview", vm);
                }

                var currentExam = await _quizService.GetExamByIdAsync(id);
                if (currentExam == null) return NotFound();

                var viewModel = new ExamPreviewViewModel
                {
                    Exam = currentExam,
                    ExerciseIds = currentExam.Exercises.Select(e => e.ExerciseId).ToList(),
                    SectionTitle = currentExam.SectionId.HasValue
                        ? (await _sectionService.GetSectionById(currentExam.SectionId.Value))?.Title
                          ?? "Section:"
                        : "Section:"
                };

                return View("ExamPreview", viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Preview: {ex.Message}");
                throw;
            }
        }


        // GET: Exam/Solve/{id}
        [HttpGet]
        public async Task<IActionResult> Solve(int id, int? index)
        {
            try
            {
                BaseQuiz quiz;

                var allExams = await _quizService.GetAllExams();
                quiz = allExams.FirstOrDefault(e => e.Id == id);

                if (quiz == null)
                {
                    quiz = await _quizService.GetExamByIdAsync(id);
                    if (quiz == null)
                    {
                        return NotFound($"Exam with ID {id} not found.");
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

                var vm = new ExamSolverViewModel
                {
                    ExamId = quiz.Id,
                    ExamTitle = "Final Exam" ,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Exam/ValidateAnswer/{examId}/{exerciseIndex}")]
        public async Task<IActionResult> ValidateAnswer(int examId, int exerciseIndex, [FromBody] AnswerSubmissionModel submission)
        {
            try
            {
                var exam = await _quizService.GetExamByIdAsync(examId);
                if (exam == null || exerciseIndex >= exam.Exercises.Count)
                {
                    return BadRequest(new { error = "Exam or exercise not found" });
                }

                var exercise = exam.Exercises[exerciseIndex];
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
                    TempData["TotalQuestions"] = exam.Exercises.Count;
                    TempData["ExamId"] = examId;
                    TempData["CorrectAnswers"] = 0;
                }

                int currentCorrect = TempData.Peek("CorrectAnswers") as int? ?? 0;
                if (isCorrect)
                {
                    TempData["CorrectAnswers"] = currentCorrect + 1;
                }

                TempData.Keep("TotalQuestions");
                TempData.Keep("ExamId");
                TempData.Keep("CorrectAnswers");

                return Json(new { isCorrect, correctAnswer });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        // Helper DTO for association JSON
        private class AssociationPair
        {
            public int left { get; set; }
            public int right { get; set; }
        }

        // POST: Exam/Solve/{id}
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
                BaseQuiz exam;

                var allExams = await _quizService.GetAllExams();
                exam = allExams.FirstOrDefault(e => e.Id == id);

                if (exam == null)
                {
                    exam = await _quizService.GetExamByIdAsync(id);
                    if (exam == null)
                    {
                        return NotFound($"Exam with ID {id} not found.");
                    }
                }

                if (index < 0 || index >= exam.Exercises.Count)
                {
                    return NotFound($"Invalid exercise index {index} for {(exam is Exam ? "exam" : "quiz")} {id}.");
                }

                var current = exam.Exercises[index];
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
                    TempData["TotalQuestions"] = exam.Exercises.Count;
                    TempData["ExamId"] = id;
                    TempData["CorrectAnswers"] = 0;
                }

                //if (valid)
                //{
                //    int currentCorrect = TempData.Peek("CorrectAnswers") as int? ?? 0;
                //    TempData["CorrectAnswers"] = currentCorrect + 1;
                //}

                int next = index + 1;
                if (next >= exam.Exercises.Count)
                {
                    return RedirectToAction(nameof(EndExam));
                }

                return RedirectToAction(nameof(Solve), new { id, index = next });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Solve POST action for ID {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET /Quiz/EndExam
        [HttpGet]
        public async Task<IActionResult> EndExam()
        {
            try
            {
                int correctAnswers = TempData["CorrectAnswers"] as int? ?? 0;
                int totalQuestions = TempData["TotalQuestions"] as int? ?? 0;
                int examId = TempData["ExamId"] as int? ?? 0;
                int passingPercent = 100;

                double percentage = totalQuestions > 0 ? (double)correctAnswers / totalQuestions * 100 : 0;
                bool isPassed = correctAnswers == totalQuestions;

                var viewModel = new ExamEndViewModel
                {
                    CorrectAnswersText = $"{correctAnswers} / {totalQuestions}",
                    PassingPercentText = "100% needed to pass",
                    IsPassedText = isPassed ? "Final Exam passed!" : "You need to redo this one.",
                    IsPassed = isPassed
                };
                
                if (isPassed && examId > 0)
                {
                    var quiz = await _quizService.GetExamByIdAsync(examId);
                    if (quiz?.SectionId != null)
                    {
                        await _quizService.CompleteExamAsync(1, examId);
                        await _sectionService.CompleteSection(1, quiz.SectionId.Value);
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
    }

}
