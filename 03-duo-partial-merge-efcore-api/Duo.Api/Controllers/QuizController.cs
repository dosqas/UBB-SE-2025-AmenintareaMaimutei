using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Duo.Api.DTO;
using Duo.Api.Helpers;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Duo.Models.Quizzes.API;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Controller for managing quizzes in the system.
    /// Provides endpoints for CRUD operations and additional quiz-related functionalities.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="QuizController"/> class with the specified repository.
    /// </remarks>
    /// <param name="repository">The repository instance for data access.</param>
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class QuizController(IRepository repository) : BaseController(repository)
    {
        #region Public Methods

        /// <summary>
        /// Adds a new quiz to the database.
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> AddQuiz([FromBody] JsonElement rawJson)
        {
            try
            {
                string json = rawJson.GetRawText();
                var quiz = await JsonSerializationUtil.DeserializeQuiz(json, this.repository);
                await repository.AddQuizAsync(quiz);
                return Ok(quiz);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a quiz by its ID.
        /// </summary>
        [HttpGet("get-quiz-by-id")]
        public async Task<IActionResult> GetQuiz([FromQuery] int id)
        {
            try
            {
                var quiz = await repository.GetQuizByIdAsync(id);
                if (quiz == null)
                {
                    return NotFound();
                }

                var exercises = quiz.Exercises.ToList();
                List<Exercise> exercisesWithType = new List<Exercise>(exercises.Count);
                foreach (var exercise in exercises)
                {
                    var ex = await this.repository.GetExerciseByIdAsync(exercise.ExerciseId);
                    if (ex == null)
                    {
                        return this.NotFound();
                    }

                    exercisesWithType.Add(ex);
                }

                var mergedExercises = ExerciseMerger.MergeExercises(exercisesWithType);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true, // Pretty print
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never, // Ensure null values are not ignored
                    ReferenceHandler = ReferenceHandler.Preserve,  // Handle object cycles
                };

                var quizDTO = new
                {
                    Id = quiz.Id,
                    SectionId = quiz.SectionId, // This will be included even if null
                    OrderNumber = quiz.OrderNumber,
                    Exercises = mergedExercises,
                };

                return Ok(quizDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Lists all quizzes in the database.
        /// </summary>
        [HttpGet("get-all")]
        public async Task<IActionResult> ListQuizzes()
        {
            try
            {
                var quizzes = await repository.GetQuizzesFromDbAsync();
                List<Quiz> quizList = new List<Quiz>(quizzes.Count);

                foreach (var quiz in quizzes)
                {
                    var exercises = quiz.Exercises.ToList();
                    List<Exercise> exercisesWithType = new List<Exercise>(exercises.Count);
                    foreach (var exercise in exercises)
                    {
                        var ex = await this.repository.GetExerciseByIdAsync(exercise.ExerciseId);
                        if (ex == null)
                        {
                            return this.NotFound();
                        }

                        exercisesWithType.Add(ex);
                    }

                    var mergedExercises = ExerciseMerger.MergeExercises(exercisesWithType);
                    quiz.Exercises = mergedExercises;
                    quizList.Add(quiz);
                }

                return Ok(quizList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing quiz.
        /// </summary>
        [HttpPut("modify")]
        public async Task<IActionResult> UpdateQuiz([FromForm] Quiz updatedQuiz)
        {
            try
            {
                var quiz = await repository.GetQuizByIdAsync(updatedQuiz.Id);
                if (quiz == null)
                {
                    return NotFound();
                }

                await repository.UpdateQuizAsync(updatedQuiz);
                return Ok(updatedQuiz);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deletes a quiz by its ID.
        /// </summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteQuiz([FromQuery] int id)
        {
            try
            {
                var quiz = await repository.GetQuizByIdAsync(id);
                if (quiz == null)
                {
                    return NotFound();
                }

                await repository.DeleteQuizAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all quizzes from a specific section.
        /// </summary>
        [HttpGet("get-all-by-section")]
        public async Task<IActionResult> GetAllQuizzesFromSection([FromQuery] int sectionId)
        {
            try
            {
                var quizzes = await repository.GetAllQuizzesFromSectionAsync(sectionId);
                return Ok(quizzes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the number of quizzes in a specific section.
        /// </summary>
        [HttpGet("count-quizzes-from-section")]
        public async Task<IActionResult> CountQuizzesFromSection([FromQuery] int sectionId)
        {
            try
            {
                var count = await repository.CountQuizzesFromSectionAsync(sectionId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the last order number from a specific section.
        /// </summary>
        [HttpGet("get-last-order-number-from-section")]
        public async Task<IActionResult> GetLastOrderNumberFromSection([FromQuery] int sectionId)
        {
            try
            {
                var lastOrder = await repository.GetLastOrderNumberFromSectionAsync(sectionId);
                return Ok(lastOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Adds a list of new exercises to a quiz.
        /// </summary>
        [HttpPost("add-exercises")]
        public async Task<IActionResult> AddExercisesToQuiz([FromForm] int quizId, [FromForm] List<int> exercises)
        {
            try
            {
                await repository.AddExercisesToQuizAsync(quizId, exercises);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Adds a single exercise to a quiz.
        /// </summary>
        [HttpPost("add-exercise")]
        public async Task<IActionResult> AddExerciseToQuiz([FromForm] int quizId, [FromForm] int exerciseId)
        {
            try
            {
                await repository.AddExerciseToQuizAsync(quizId, exerciseId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Removes an exercise from a quiz.
        /// </summary>
        [HttpDelete("delete-exercise")]
        public async Task<IActionResult> RemoveExerciseFromQuiz([FromQuery] int quizId, [FromQuery] int exerciseId)
        {
            try
            {
                await repository.RemoveExerciseFromQuizAsync(quizId, exerciseId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Submits a quiz with user's answers, checks correctness, and saves the submission.
        /// </summary>
        [HttpPost("add-submit")]
        public async Task<IActionResult> SubmitQuiz([FromBody] QuizSubmission submission)
        {
            try
            {
                var quiz = await repository.GetQuizByIdAsync(submission.QuizId);
                if (quiz == null)
                {
                    return NotFound();
                }

                var submissionEntity = new QuizSubmissionEntity
                {
                    QuizId = submission.QuizId,
                    StartTime = DateTime.Now.AddMinutes(-5),
                    EndTime = DateTime.Now,
                };

                foreach (var answer in submission.Answers)
                {
                    var question = quiz.Exercises.FirstOrDefault(q => q.ExerciseId == answer.QuestionId);
                    if (question != null)
                    {
                        submissionEntity.Answers.Add(new AnswerSubmissionEntity
                        {
                            QuestionId = answer.QuestionId,
                            IsCorrect = CheckIfCorrect(question, answer)
                        });
                    }
                }

                await repository.SaveQuizSubmissionAsync(submissionEntity);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Checks if a given answer is correct depending on the type of exercise.
        /// </summary>
        /// <param name="question">The exercise question to check against.</param>
        /// <param name="userAnswer">The user's submitted answer.</param>
        /// <returns>True if the answer is correct, otherwise false.</returns>
        private bool CheckIfCorrect(Exercise question, AnswerSubmission userAnswer)
        {
            if (question is MultipleChoiceExercise mcq)
            {
                if (userAnswer.SelectedOptionIndex.HasValue &&
                    mcq.Choices != null &&
                    userAnswer.SelectedOptionIndex.Value >= 0 &&
                    userAnswer.SelectedOptionIndex.Value < mcq.Choices.Count)
                {
                    return mcq.Choices[userAnswer.SelectedOptionIndex.Value].IsCorrect;
                }
            }
            else if (question is FillInTheBlankExercise fill)
            {
                return fill.PossibleCorrectAnswers
                    .Any(correct => string.Equals(correct, userAnswer.WrittenAnswer?.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            else if (question is FlashcardExercise flash)
            {
                return flash.ValidateAnswer(userAnswer.WrittenAnswer ?? string.Empty);
            }
            else if (question is AssociationExercise association)
            {
                if (userAnswer.SelectedOptionIndex.HasValue && userAnswer.AssociatedPairId.HasValue)
                {
                    var selectedFirstIndex = userAnswer.SelectedOptionIndex.Value;
                    var selectedSecondIndex = userAnswer.AssociatedPairId.Value;

                    if (selectedFirstIndex >= 0 && selectedFirstIndex < association.FirstAnswersList.Count &&
                        selectedSecondIndex >= 0 && selectedSecondIndex < association.SecondAnswersList.Count)
                    {
                        return selectedFirstIndex == selectedSecondIndex;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Retrieves the quiz result (total questions, correct answers, and time taken) after submission.
        /// </summary>
        [HttpGet("get-quiz-result")]
        public async Task<IActionResult> GetQuizResult([FromQuery] int quizId)
        {
            try
            {
                var submission = await repository.GetSubmissionByQuizIdAsync(quizId);
                if (submission == null)
                {
                    return NotFound();
                }

                var result = new QuizResult
                {
                    QuizId = quizId,
                    TotalQuestions = submission.Answers.Count,
                    CorrectAnswers = submission.Answers.Count(a => a.IsCorrect),
                    TimeTaken = submission.EndTime - submission.StartTime
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Retrieves all available quiz.
        /// </summary>
        /// <returns>A list of available quizzes.</returns>
        [HttpGet("get-all-available")]
        public async Task<IActionResult> GetAvailableQuizzes()
        {
            try
            {
                var quizzes = await this.repository.GetAvailableQuizzesAsync();
                List<Quiz> quizList = new List<Quiz>(quizzes.Count);

                foreach (var quiz in quizzes)
                {
                    var exercises = quiz.Exercises.ToList();
                    List<Exercise> exercisesWithType = new List<Exercise>(exercises.Count);
                    foreach (var exercise in exercises)
                    {
                        var ex = await this.repository.GetExerciseByIdAsync(exercise.ExerciseId);
                        if (ex == null)
                        {
                            return this.NotFound();
                        }

                        exercisesWithType.Add(ex);
                    }

                    var mergedExercises = ExerciseMerger.MergeExercises(exercisesWithType);
                    quiz.Exercises = mergedExercises;
                    quizList.Add(quiz);
                }

                return Ok(quizList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion
    }
}
