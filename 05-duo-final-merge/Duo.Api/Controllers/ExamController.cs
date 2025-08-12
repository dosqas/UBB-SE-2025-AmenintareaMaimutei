using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Duo.Api.DTO;
using Duo.Api.Helpers;
using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for managing exams, including CRUD operations and additional functionalities.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExamController"/> class with the specified repository.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExamController"/> class.
    /// </remarks>
    /// <param name="repository">The repository instance for data access.</param>
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class ExamController(IRepository repository) : BaseController(repository)
    {
        #region Methods

        /// <summary>
        /// Adds a new exam to the database.
        /// </summary>
        /// <param name="exam">The exam data to add.</param>
        /// <returns>The added exam.</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddExam([FromBody] JsonElement rawJson)
        {
            try
            {
                string json = rawJson.GetRawText();
                var exam = await JsonSerializationUtil.DeserializeExamWithTypedExercises(json, this.repository);
                await repository.AddExamAsync(exam);
                return Ok(exam);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves an exam by its ID.
        /// </summary>
        /// <param name="id">The ID of the exam to retrieve.</param>
        /// <returns>The exam if found; otherwise, NotFound.</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetExam([FromQuery] int id)
        {
            try
            {
                var exam = await this.repository.GetExamByIdAsync(id);
                if (exam == null)
                {
                    return NotFound();
                }

                // Handle polymorphysm
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

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true, // Pretty print
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never, // Ensure null values are not ignored
                    ReferenceHandler = ReferenceHandler.Preserve,  // Handle object cycles
                };

                ExamDTO examDto = new ExamDTO
                {
                    Id = exam.Id,
                    SectionId = exam.SectionId, // This will be included even if null
                    Exercises = mergedExercises,
                };

                return Ok(examDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Lists all exams in the database.
        /// </summary>
        /// <returns>A list of all exams.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListExams()
        {
            try
            {
                var exams = await this.repository.GetExamsFromDbAsync();
                List<Exam> examList = new List<Exam>(exams.Count);

                foreach (var exam in exams)
                {
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
                    exam.Exercises = mergedExercises;
                    examList.Add(exam);
                }

                return Ok(examList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing exam.
        /// </summary>
        /// <param name="updatedExam">The updated exam data, including Id.</param>
        /// <returns>The updated exam if found; otherwise, NotFound.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateExam([FromForm] Exam updatedExam)
        {
            try
            {
                var exam = await repository.GetExamByIdAsync(updatedExam.Id);
                if (exam == null)
                {
                    return NotFound();
                }

                await repository.UpdateExamAsync(updatedExam);
                return Ok(updatedExam);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deletes an exam by its ID.
        /// </summary>
        /// <param name="id">The ID of the exam to delete.</param>
        /// <returns>Ok if deleted; otherwise, NotFound.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteExam([FromQuery] int id)
        {
            try
            {
                var exam = await repository.GetExamByIdAsync(id);
                if (exam == null)
                {
                    return NotFound();
                }

                // Delete section if it exists
                if (exam.SectionId != null)
                {
                    var section = await repository.GetSectionByIdAsync(exam.SectionId.Value);
                    if (section != null)
                    {
                        await repository.DeleteSectionAsync(section.Id);
                    }
                }

                await repository.DeleteExamAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Adds a single exercise to an exam.
        /// </summary>
        /// <param name="examId">The ID of the exam to which the exercise will be added.</param>
        /// <param name="exerciseId">The ID of the exercise to add to the exam.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("add-exercise")]
        public async Task<IActionResult> AddExerciseToExam([FromForm] int examId, [FromForm] int exerciseId)
        {
            try
            {
                await repository.AddExerciseToExamAsync(examId, exerciseId);
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
        [HttpDelete("remove-exercise")]
        public async Task<IActionResult> RemoveExerciseFromExam([FromQuery] int examId, [FromQuery] int exerciseId)
        {
            try
            {
                await repository.RemoveExerciseFromExamAsync(examId, exerciseId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the exam associated with a specific section.
        /// </summary>
        /// <param name="sectionId">The ID of the section.</param>
        /// <returns>The exam if found; otherwise, NotFound.</returns>
        [HttpGet("get-from-section")]
        public async Task<IActionResult> GetExamFromSection([FromQuery] int sectionId)
        {
            try
            {
                var exam = await repository.GetExamFromSectionAsync(sectionId);
                if (exam == null)
                {
                    return NotFound();
                }

                return Ok(exam);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all available exams.
        /// </summary>
        /// <returns>A list of available exams.</returns>
        [HttpGet("get-available")]
        public async Task<IActionResult> GetAvailableExams()
        {
            try
            {
                var exams = await this.repository.GetAvailableExamsAsync();
                List<Exam> examList = new List<Exam>(exams.Count);

                foreach (var exam in exams)
                {
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
                    exam.Exercises = mergedExercises;
                    examList.Add(exam);
                }

                return Ok(examList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Checks if an exam is completed by a specific user.
        /// </summary>
        /// <param name="examId"> Id of the exam. </param>
        /// <param name="userId"> Id of the user. </param>
        /// <returns> A boolean in an http response signaling the status of the exam for the user. </returns>
        [HttpGet("is-completed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsExamCompleted([FromQuery] int userId, [FromQuery] int examId)
        {
            try
            {
                bool isCompleted = await this.repository.IsExamCompleted(userId, examId);
                return this.Ok(new { IsCompleted = isCompleted });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { message = e.Message });
            }
        }


        /// <summary>
        /// Marks an exam as completed for a specific user.
        /// </summary>
        /// <param name="examId"> Id of the completed exam.</param>
        /// <param name="userId"> Id of the user.</param>
        /// <returns> Answer indicating the exam was added.</returns>
        [HttpPost("add-completed-exam")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCompletedExam([FromQuery] int examId, [FromQuery] int userId)
        {
            try
            {
                ExamCompletions examCompletion = new ExamCompletions
                {
                    ExamId = examId,
                    UserId = userId,
                };
                await this.repository.CompleteExamForUser(examCompletion);
                return this.Ok(new { message = "Quiz marked as completed." });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { message = e.Message });
            }
        }

        #endregion
    }
}