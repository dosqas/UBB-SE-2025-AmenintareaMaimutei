using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using Duo.Api.DTO.Requests;
using Duo.Api.Models.Exercises;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Duo.Api.Helpers;
using Duo.Api.Models.Quizzes;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for managing exercises, including CRUD operations and retrieval by quiz or exam.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExerciseController"/> class with the specified repository.
    /// </remarks>
    /// <param name="repository">The repository instance for data access.</param>
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class ExerciseController(IRepository repository) : BaseController(repository)
    {
        #region Methods

        /// <summary>
        /// Retrieves all exercises from the database.
        /// </summary>
        /// <returns>A list of all exercises.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Exercise>>> GetAllExercisesAsync()
        {
            try
            {
                var exercises = await repository.GetExercisesFromDbAsync();
                var mergedExercises = MergeExercises(exercises);
                return Ok(mergedExercises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves an exercise by its ID.
        /// </summary>
        /// <param name="id">The ID of the exercise to retrieve.</param>
        /// <returns>The exercise if found; otherwise, NotFound.</returns>
        [HttpGet("{id}", Name = "GetExerciseById")]
        public async Task<ActionResult<Exercise>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Exercise ID must be greater than 0.");
            }

            try
            {
                var exercise = await repository.GetExerciseByIdAsync(id);
                if (exercise == null)
                {
                    return NotFound($"Exercise with ID {id} not found.");
                }
                var exercises = new List<Exercise> { exercise };
                var mergedExercises = MergeExercises(exercises);

                if (mergedExercises == null || mergedExercises.Count == 0)
                {
                    return NotFound($"Exercise with ID {id} not found.");
                }

                return Ok(mergedExercises[0]);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving exercise with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all exercises associated with a specific quiz.
        /// </summary>
        /// <param name="quizId">The ID of the quiz.</param>
        /// <returns>A list of exercises associated with the quiz.</returns>
        [HttpGet("quiz/{quizId}")]
        public async Task<ActionResult<List<Exercise>>> GetQuizExercises(int quizId)
        {
            if (quizId <= 0)
            {
                return BadRequest("Invalid quiz ID.");
            }

            try
            {
                var quiz = await repository.GetQuizByIdAsync(quizId);
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
                return Ok(mergedExercises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all exercises associated with a specific exam.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <returns>A list of exercises associated with the exam.</returns>
        [HttpGet("exam/{examId}")]
        public async Task<ActionResult<List<Exercise>>> GetExamExercises(int examId)
        {
            if (examId <= 0)
            {
                return BadRequest("Invalid exam ID.");
            }

            try
            {
                var exam = await repository.GetExamByIdAsync(examId);
                if (exam == null)
                {
                    return NotFound();
                }
                var exercises = exam.Exercises.ToList();
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
                return Ok(mergedExercises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Adds a new exercise to the database.
        /// </summary>
        /// <param name="exercise">The exercise to add.</param>
        /// <returns>The created exercise.</returns>
        [HttpPost]
        public async Task<ActionResult> AddExercise([FromBody] JsonElement rawJson)
        {

            string json = rawJson.GetRawText();

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

            try
            {
                switch (exercise.Type)
                {
                    case "Association":
                        AssociationExercise associationExercise = (AssociationExercise)exercise;
                        await this.repository.AddExerciseAsync(associationExercise);
                        break;
                    case "FillInTheBlank":
                        FillInTheBlankExercise fillInTheBlankExercise = (FillInTheBlankExercise)exercise;
                        await this.repository.AddExerciseAsync(fillInTheBlankExercise);
                        break;
                    case "Flashcard":
                        FlashcardExercise flashcardExercise = (FlashcardExercise)exercise;
                        await this.repository.AddExerciseAsync(flashcardExercise);
                        break;
                    case "MultipleChoice":
                        MultipleChoiceExercise multipleChoiceExercise = (MultipleChoiceExercise)exercise;
                        await this.repository.AddExerciseAsync(multipleChoiceExercise);
                        break;
                    default:
                        throw new InvalidOperationException("Invalid exercise type.");
                }

                return this.CreatedAtRoute(
                  routeName: "GetExerciseById",
                  routeValues: new { id = exercise.ExerciseId },
                  value: exercise.ExerciseId);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deletes an exercise by its ID.
        /// </summary>
        /// <param name="id">The ID of the exercise to delete.</param>
        /// <returns>NoContent if deleted; otherwise, NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExercise(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid exercise ID.");
            }

            try
            {
                var existingExercise = await repository.GetExerciseByIdAsync(id);
                if (existingExercise == null)
                {
                    return NotFound();
                }

                await repository.DeleteExerciseAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Merges duplicate exercises into a single instance.
        /// </summary>
        /// <param name="exercises">The list of exercises to merge.</param>
        /// <returns>A list of merged exercises.</returns>
        private static List<Exercise> MergeExercises(List<Exercise> exercises)
        {
            var mergedExercises = new List<Exercise>();
            var exerciseMap = new Dictionary<int, Exercise>();

            foreach (var exercise in exercises)
            {
                if (!exerciseMap.TryGetValue(exercise.ExerciseId, out var existingExercise))
                {
                    exerciseMap[exercise.ExerciseId] = exercise switch
                    {
                        MultipleChoiceExercise mc => new MultipleChoiceExercise { ExerciseId = mc.ExerciseId, Question = mc.Question!, Difficulty = mc.Difficulty, Choices = [.. mc.Choices!] },
                        FillInTheBlankExercise fb => new FillInTheBlankExercise { ExerciseId = fb.ExerciseId, Question = fb.Question!, Difficulty = fb.Difficulty, PossibleCorrectAnswers = [.. fb.PossibleCorrectAnswers!] },
                        AssociationExercise assoc => new AssociationExercise { ExerciseId = assoc.ExerciseId, Question = assoc.Question!, Difficulty = assoc.Difficulty, FirstAnswersList = [.. assoc.FirstAnswersList], SecondAnswersList = [.. assoc.SecondAnswersList] },
                        FlashcardExercise flash => new FlashcardExercise { ExerciseId = flash.ExerciseId, Question = flash.Question!, Answer = flash.Answer, Difficulty = flash.Difficulty },
                        _ => exercise
                    };
                }
                else
                {
                    // Merge the data
                    switch (existingExercise)
                    {
                        case MultipleChoiceExercise existingMC when exercise is MultipleChoiceExercise newMC:
                            newMC.Choices!.RemoveAll(c => c.IsCorrect);
                            existingMC.Choices!.AddRange(newMC.Choices);
                            break;

                        case FillInTheBlankExercise existingFB when exercise is FillInTheBlankExercise newFB:
                            existingFB.PossibleCorrectAnswers!.AddRange(newFB.PossibleCorrectAnswers!);
                            break;

                        case AssociationExercise existingAssoc when exercise is AssociationExercise newAssoc:
                            existingAssoc.FirstAnswersList.AddRange(newAssoc.FirstAnswersList);
                            existingAssoc.SecondAnswersList.AddRange(newAssoc.SecondAnswersList);
                            break;
                    }
                }
            }

            mergedExercises.AddRange(exerciseMap.Values);

            return mergedExercises;
        }

        #endregion
    }
}