using Duo.Api.DTO;
using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Roadmaps;
using Duo.Api.Models.Sections;
using Duo.Api.Persistence;
using Duo.Models.Quizzes.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Repositories
{
    /// <summary>
    /// Provides an implementation of the <see cref="IRepository"/> interface.
    /// Handles data access operations for various entities in the system.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Repository"/> class with the specified database context.
    /// </remarks>
    /// <param name="dataContext">The database context to use for data access.</param>
    public class Repository(DataContext dataContext) : IRepository
    {
        #region Fields

        /// <summary>
        /// The database context used for data access.
        /// </summary>
        private readonly DataContext context = dataContext;

        #endregion

        #region Users

        /// <summary>
        /// Asynchronously retrieves all users from the database.
        /// </summary>
        /// <returns>A list of <see cref="User"/> objects representing all users in the database.</returns>
        public async Task<List<User>> GetUsersFromDbAsync()
        {
            return await context.Users.ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a user from the database by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>A <see cref="User"/> object representing the user with the given ID, or null if not found.</returns>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        /// <summary>
        /// Asynchronously adds a new user to the database.
        /// </summary>
        /// <param name="user">The <see cref="User"/> object to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddUserAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates an existing user's information in the database.
        /// </summary>
        /// <param name="user">The <see cref="User"/> object with updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateUserAsync(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a user from the database by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteUserAsync(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        #endregion

        #region Tags

        /// <summary>
        /// Asynchronously retrieves all tags from the database.
        /// </summary>
        /// <returns>A list of <see cref="Tag"/> objects representing all tags in the database.</returns>
        public async Task<List<Tag>> GetTagsFromDbAsync()
        {
            return await context.Tags.ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a tag from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tag.</param>
        /// <returns>A <see cref="Tag"/> object representing the tag with the given ID, or null if not found.</returns>
        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await context.Tags.FindAsync(id);
        }

        /// <summary>
        /// Asynchronously adds a new tag to the database.
        /// </summary>
        /// <param name="tag">The <see cref="Tag"/> object to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddTagAsync(Tag tag)
        {
            context.Tags.Add(tag);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates an existing tag's information in the database.
        /// </summary>
        /// <param name="tag">The <see cref="Tag"/> object with updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateTagAsync(Tag tag)
        {
            context.Tags.Update(tag);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a tag from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tag to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteTagAsync(int id)
        {
            var tag = await context.Tags.FindAsync(id);
            if (tag != null)
            {
                context.Tags.Remove(tag);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Tag>> GetTagsForCourseAsync(int courseId)
        {
            return await context.CourseTags
                .Where(ct => ct.CourseId == courseId)
                .Select(ct => ct.Tag)
                .ToListAsync();
        }

        #endregion

        #region Modules

        /// <summary>
        /// Asynchronously retrieves all modules from the database.
        /// </summary>
        /// <returns>A list of <see cref="Module"/> objects representing all modules in the database.</returns>
        public async Task<List<Module>> GetModulesFromDbAsync()
        {
            return await context.Modules.ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a module from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the module.</param>
        /// <returns>A <see cref="Module"/> object representing the module with the given ID, or null if not found.</returns>
        public async Task<Module?> GetModuleByIdAsync(int id)
        {
            return await context.Modules.FindAsync(id);
        }

        /// <summary>
        /// Asynchronously adds a new module to the database.
        /// </summary>
        /// <param name="module">The <see cref="Module"/> object to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddModuleAsync(Module module)
        {
            context.Modules.Add(module);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates an existing module's information in the database.
        /// </summary>
        /// <param name="module">The <see cref="Module"/> object with updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateModuleAsync(Module module)
        {
            context.Modules.Update(module);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a module from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the module to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteModuleAsync(int id)
        {
            var module = await context.Modules.FindAsync(id);
            if (module != null)
            {
                context.Modules.Remove(module);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously opens a module for a user by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user opening the module.</param>
        /// <param name="moduleId">The unique identifier of the module to be opened.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown if the module is not found.</exception>
        public async Task OpenModuleAsync(int userId, int moduleId)
        {
            var module = await context.Modules.FindAsync(moduleId);
            if (module == null)
            {
                var message = $"[OpenModuleAsync] Module with ID {moduleId} not found for user {userId}.";
                Console.Error.WriteLine(message);
                throw new KeyNotFoundException(message);
            }

            context.Modules.Update(module);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously completes a module for a user by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user completing the module.</param>
        /// <param name="moduleId">The unique identifier of the module to be completed.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CompleteModuleAsync(int userId, int moduleId)
        {
            var progress = await context.UserProgresses
                .FirstOrDefaultAsync(up => up.UserId == userId && up.ModuleId == moduleId);

            if (progress != null)
            {
                progress.Status = "completed";
            }
            else
            {
                var newProgress = new UserProgress
                {
                    UserId = userId,
                    ModuleId = moduleId,
                    Status = "completed",
                    ImageClicked = false,
                };
                context.UserProgresses.Add(newProgress);
                await context.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Asynchronously checks if a module is open for a user by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="moduleId">The unique identifier of the module to check.</param>
        /// <returns>A boolean indicating whether the module is open for the user.</returns>
        public async Task<bool> IsModuleOpenAsync(int userId, int moduleId)
        {
            return await context.UserProgresses
                .AnyAsync(up => up.UserId == userId && up.ModuleId == moduleId);
        }

        /// <summary>
        /// Asynchronously checks if a module is completed by a user by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="moduleId">The unique identifier of the module to check.</param>
        /// <returns>A boolean indicating whether the module is completed by the user.</returns>
        public async Task<bool> IsModuleCompletedAsync(int userId, int moduleId)
        {
            return await context.UserProgresses
                .AnyAsync(up => up.UserId == userId && up.ModuleId == moduleId && up.Status == "completed");
        }

        /// <summary>
        /// Asynchronously checks if a module is available to a user by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="moduleId">The unique identifier of the module to check.</param>
        /// <returns>A boolean indicating whether the module is available to the user.</returns>
        public async Task<bool> IsModuleAvailableAsync(int userId, int moduleId)
        {
            var module = await context.Modules.FindAsync(moduleId);

            if (module == null)
            {
                return false;
            }

            if (module.Position == 1 || module.IsBonus)
            {
                return true;
            }

            var previousModule = await context.Modules
                .FirstOrDefaultAsync(m => m.CourseId == module.CourseId && m.Position == module.Position - 1);

            if (previousModule == null)
            {
                return false;
            }

            return await context.UserProgresses
                .AnyAsync(up => up.UserId == userId
                                && up.ModuleId == previousModule.ModuleId
                                && up.Status == "completed");
        }

        /// <summary>
        /// Asynchronously handles a click event for a module image by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user clicking the module image.</param>
        /// <param name="moduleId">The unique identifier of the module being clicked.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ClickModuleImageAsync(int userId, int moduleId)
        {
            var progress = await context.UserProgresses
                .FirstOrDefaultAsync(up => up.UserId == userId && up.ModuleId == moduleId);

            if (progress != null)
            {
                progress.ImageClicked = true;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously checks if a module image has been clicked by a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <returns>A boolean indicating whether the module image has been clicked by the user.</returns>
        public async Task<bool> IsModuleImageClickedAsync(int userId, int moduleId)
        {
            var progress = await context.UserProgresses
                .FirstOrDefaultAsync(up => up.UserId == userId && up.ModuleId == moduleId);

            return progress?.ImageClicked ?? false;
        }

        public async Task<bool> BuyBonusModuleAsync(int userId, int moduleId)
        {
            var result = await this.context.UserProgresses.AddAsync(new UserProgress
            {
                UserId = userId,
                ModuleId = moduleId,
            });
            await this.context.SaveChangesAsync();
            if (result.Entity != null)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Exercises

        /// <summary>
        /// Asynchronously retrieves all exercises from the database.
        /// </summary>
        /// <returns>A list of <see cref="Exercise"/> objects representing all exercises in the database.</returns>
        public async Task<List<Exercise>> GetExercisesFromDbAsync()
        {
            return await context.Exercises
                .Include("Choices")
                .Include(e => e.Quizzes)
                .Include(e => e.Exams)
                .ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves an exercise from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the exercise.</param>
        /// <returns>A <see cref="Exercise"/> object representing the exercise with the given ID, or null if not found.</returns>
        public async Task<Exercise?> GetExerciseByIdAsync(int id)
        {
            return await context.Exercises.
                Include("Choices").
                FirstOrDefaultAsync(e => e.ExerciseId == id);
        }

        /// <summary>
        /// Asynchronously adds a new exercise to the database.
        /// </summary>
        /// <param name="exercise">The <see cref="Exercise"/> object to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddExerciseAsync(Exercise exercise)
        {
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates an existing exercise's information in the database.
        /// </summary>
        /// <param name="exercise">The <see cref="Exercise"/> object with updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateExerciseAsync(Exercise exercise)
        {
            context.Exercises.Update(exercise);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes an exercise from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the exercise to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteExerciseAsync(int id)
        {
            var exercise = await context.Exercises.FindAsync(id);
            if (exercise != null)
            {
                context.Exercises.Remove(exercise);
                await context.SaveChangesAsync();
            }
        }

        #endregion

        #region Quizzes

        /// <summary>
        /// Asynchronously retrieves all quizzes from the database, including their associated exercises.
        /// </summary>
        /// <returns>A list of <see cref="Quiz"/> objects representing all quizzes in the database, along with their associated exercises.</returns>
        public async Task<List<Quiz>> GetQuizzesFromDbAsync()
        {
            // Retrieves all quizzes from the database, including their associated exercises.
            return await context.Quizzes.Include(q => q.Exercises).ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a quiz from the database by its unique identifier, including its associated exercises.
        /// </summary>
        /// <param name="id">The unique identifier of the quiz.</param>
        /// <returns>A <see cref="Quiz"/> object representing the quiz with the given ID, or null if not found.</returns>
        public async Task<Quiz?> GetQuizByIdAsync(int id)
        {
            // Retrieves a quiz by its unique identifier, including its associated exercises.
            return await context.Quizzes.Include(q => q.Exercises).FirstOrDefaultAsync(q => q.Id == id);
        }

        /// <summary>
        /// Asynchronously saves a quiz submitted by a user
        /// </summary>
        /// <param name="submission">Object representing submitted quiz</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SaveQuizSubmissionAsync(QuizSubmissionEntity submission)
        {
            context.QuizSubmissions.Add(submission);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously gets a quiz submitted by a user
        /// </summary>
        /// <param name="quizId">Id of quiz submitted you want to retrieve</param>
        /// <returns>An object representing quiz submission by a user</returns>
        public async Task<QuizSubmissionEntity?> GetSubmissionByQuizIdAsync(int quizId)
        {
            return await context.QuizSubmissions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuizId == quizId);
        }

        /// <summary>
        /// Asynchronously adds a new quiz to the database.
        /// </summary>
        /// <param name="quiz">The <see cref="Quiz"/> object to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddQuizAsync(Quiz quiz)
        {
            // Adds a new quiz to the database.
            context.Quizzes.Add(quiz);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates an existing quiz's information in the database.
        /// </summary>
        /// <param name="quiz">The <see cref="Quiz"/> object with updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateQuizAsync(Quiz quiz)
        {
            // Updates an existing quiz in the database.
            context.Quizzes.Update(quiz);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a quiz from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the quiz to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteQuizAsync(int id)
        {
            // Deletes a quiz from the database by its unique identifier.
            var quiz = await context.Quizzes.FindAsync(id);
            if (quiz != null)
            {
                context.Quizzes.Remove(quiz);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Retrieves all quizzes from a specific section.
        /// </summary>
        /// <param name="sectionId">The unique identifier of the section.</param>
        /// <returns>A list of quizzes belonging to the specified section.</returns>
        public async Task<List<Quiz>> GetAllQuizzesFromSectionAsync(int sectionId)
        {
            return await context.Quizzes
                .Where(q => q.SectionId == sectionId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves the number of quizzes in a specific section.
        /// </summary>
        /// <param name="sectionId">The unique identifier of the section.</param>
        /// <returns>The count of quizzes in the specified section.</returns>
        public async Task<int> CountQuizzesFromSectionAsync(int sectionId)
        {
            return await context.Quizzes
                .Where(q => q.SectionId == sectionId)
                .CountAsync();
        }

        /// <summary>
        /// Retrieves the last order number in a specific section.
        /// </summary>
        /// <param name="sectionId">The unique identifier of the section.</param>
        /// <returns>The last order number of quizzes in the specified section, or 0 if none exist.</returns>
        public async Task<int> GetLastOrderNumberFromSectionAsync(int sectionId)
        {
            var quiz = await context.Quizzes
                .Where(q => q.SectionId == sectionId && q.OrderNumber.HasValue)
                .OrderByDescending(q => q.OrderNumber)
                .FirstOrDefaultAsync();

            return quiz?.OrderNumber ?? 0;
        }

        /// <summary>
        /// Adds a list of exercises to a quiz.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz.</param>
        /// <param name="exerciseIds">A list of exercise IDs to add to the quiz.</param>
        public async Task AddExercisesToQuizAsync(int quizId, List<int> exerciseIds)
        {
            var quiz = await context.Quizzes
                .Include(q => q.Exercises)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz != null)
            {
                var exercises = await context.Exercises
                    .Where(e => exerciseIds.Contains(e.ExerciseId))
                    .ToListAsync();

                foreach (var exercise in exercises)
                {
                    if (!quiz.Exercises.Contains(exercise))
                    {
                        quiz.Exercises.Add(exercise);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Adds a single exercise to a quiz.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz.</param>
        /// <param name="exerciseId">The unique identifier of the exercise to add.</param>
        public async Task AddExerciseToQuizAsync(int quizId, int exerciseId)
        {
            var quiz = await context.Quizzes
                .Include(q => q.Exercises)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            var exercise = await context.Exercises.FindAsync(exerciseId);

            if (quiz != null && exercise != null && !quiz.Exercises.Contains(exercise))
            {
                quiz.Exercises.Add(exercise);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes an exercise from a quiz.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz.</param>
        /// <param name="exerciseId">The unique identifier of the exercise to remove.</param>
        public async Task RemoveExerciseFromQuizAsync(int quizId, int exerciseId)
        {
            var quiz = await context.Quizzes
                .Include(q => q.Exercises)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            var exercise = await context.Exercises.FindAsync(exerciseId);

            if (quiz != null && exercise != null && quiz.Exercises.Contains(exercise))
            {
                quiz.Exercises.Remove(exercise);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets the quiz result, including the quiz ID and the count of associated exercises.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz.</param>
        /// <returns>An object containing the quiz ID and the count of exercises, or <c>null</c> if the quiz does not exist.</returns>
        public async Task<QuizResult> GetQuizResultAsync(int quizId)
        {
            var quiz = await context.Quizzes
                .Include(q => q.Exercises)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
            {
                return null;
            }

            // Basic mock result
            return new QuizResult();
        }

        /// <summary>
        /// Asynchronously retrieves all available quizzes from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Quiz>> GetAvailableQuizzesAsync()
        {
            return await context.Quizzes
                .Include(e => e.Exercises) // Ensure exercises related to the quizzes are included
                .Where(e => e.SectionId == null) // Filter quizzes with no section assigned
                .ToListAsync();
        }

        /// <summary>
        /// Checks if a user has completed a specific quiz asynchronously.
        /// </summary>
        /// <param name="userId"> Id of user.</param>
        /// <param name="quizId"> Id of quiz.</param>
        /// <returns> A task returning a boolean result indicating whether the quiz is completed or not for the user.</returns>
        public async Task<bool> IsQuizCompleted(int userId, int quizId)
        {
            var completion = await this.context.QuizCompletions.FindAsync(userId, quizId);

            if (completion == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Marks a quiz as completed for a user asynchronously.
        /// </summary>
        /// <param name="completedQuiz"> Completed quiz for a user. </param>
        /// <returns> A task representing the async operation.</returns>
        public async Task CompleteQuizForUser(QuizCompletions completedQuiz)
        {
            this.context.QuizCompletions.Add(completedQuiz);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a quiz completion record for all users by quiz ID asynchronously.
        /// </summary>
        /// <param name="quizId"> Quiz id.</param>
        /// <returns> A task representing the async operation. </returns>
        public async Task DeleteQuizCompletions(int quizId)
        {
            var completions = await this.context.QuizCompletions
                .Where(q => q.QuizId == quizId)
                .ToListAsync();
            if (completions != null)
            {
                this.context.QuizCompletions.RemoveRange(completions);
                await this.context.SaveChangesAsync();
            }
        }

        #endregion

        #region Courses

        /// <summary>
        /// Asynchronously retrieves all courses from the database.
        /// </summary>
        /// <returns>A list of <see cref="Course"/> objects representing all courses in the database.</returns>
        public async Task<List<CourseDto>> GetCoursesFromDbAsync()
        {
            return await context.Courses
                .Include(c => c.CourseTags)
                .ThenInclude(ct => ct.Tag)
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    IsPremium = c.IsPremium,
                    Cost = c.Cost,
                    ImageUrl = c.ImageUrl,
                    TimeToComplete = c.TimeToComplete,
                    Difficulty = c.Difficulty,
                    CourseTags = c.CourseTags.Select(ct => new TagDto
                    {
                        TagId = ct.TagId,
                        Name = ct.Tag.Name,
                    }).ToList(),
                })
                .ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a course from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the course.</param>
        /// <returns>A <see cref="Course"/> object representing the course with the given ID, or null if not found.</returns>
        public async Task<CourseDto?> GetCourseByIdAsync(int id)
        {
            return await this.context.Courses
                .Include(c => c.CourseTags)
                .ThenInclude(ct => ct.Tag)
                .Where(c => c.CourseId == id)
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    IsPremium = c.IsPremium,
                    Cost = c.Cost,
                    ImageUrl = c.ImageUrl,
                    TimeToComplete = c.TimeToComplete,
                    Difficulty = c.Difficulty,
                    CourseTags = c.CourseTags.Select(ct => new TagDto
                    {
                        TagId = ct.TagId,
                        Name = ct.Tag.Name,
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Asynchronously adds a new course to the database.
        /// </summary>
        /// <param name="course">The <see cref="Course"/> object to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddCourseAsync(Course course)
        {
            context.Courses.Add(course);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates an existing course's information in the database.
        /// </summary>
        /// <param name="course">The <see cref="Course"/> object with updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateCourseAsync(Course course)
        {
            context.Courses.Update(course);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a course from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the course to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteCourseAsync(int id)
        {
            var course = await context.Courses.FindAsync(id);
            if (course != null)
            {
                context.Courses.Remove(course);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously enrolls a user in a course, creating a course completion record if it does not already exist.
        /// </summary>
        /// <param name="userId">The ID of the user to be enrolled.</param>
        /// <param name="courseId">The ID of the course to enroll in.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task EnrollUserInCourseAsync(int userId, int courseId)
        {
            var existing = await context.Enrollments
                .FirstOrDefaultAsync(cc => cc.UserId == userId && cc.CourseId == courseId);

            if (existing == null)
            {
                var enrollment = new Enrollment
                {
                    UserId = userId,
                    CourseId = courseId,
                    EnrolledAt = DateTime.UtcNow,
                    TimeSpent = 0,
                    IsCompleted = false,
                };
                context.Enrollments.Add(enrollment);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously checks if a user is enrolled in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the user is enrolled in the course, otherwise false.</returns>
        public async Task<bool> IsUserEnrolledInCourseAsync(int userId, int courseId)
        {
            return await context.Enrollments.FindAsync(userId, courseId) != null;
        }

        /// <summary>
        /// Asynchronously checks if a user has completed a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the course is completed, otherwise false.</returns>
        public async Task<bool> IsCourseCompletedAsync(int userId, int courseId)
        {
            var enrollment = await this.context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
            if (enrollment == null)
            {
                return false;
            }
            return enrollment.IsCompleted;
        }

        /// <summary>
        /// Asynchronously retrieves the number of modules completed by a user in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The number of completed modules.</returns>
        public async Task<int> GetCompletedModulesCountAsync(int userId, int courseId)
        {
            return await context.UserProgresses
                .Where(up => up.UserId == userId &&
                             up.Status == "completed" &&
                             context.Modules.Any(m => m.ModuleId == up.ModuleId && m.CourseId == courseId && !m.IsBonus))
                .CountAsync();
        }

        public async Task<int> GetRequiredModulesCountAsync(int courseId)
        {
            return await context.Modules
                .Where(m => m.CourseId == courseId && !m.IsBonus)
                .CountAsync();
        }

        /// <summary>
        /// Asynchronously updates the time spent by a user in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="timeInSeconds">The time spent by the user in seconds.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateTimeSpentAsync(int userId, int courseId, int timeInSeconds)
        {
            var enrollment = await context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
            if (enrollment != null)
            {
                enrollment.TimeSpent += timeInSeconds;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously claims the completion reward for a user in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ClaimCompletionRewardAsync(int userId, int courseId)
        {
            var completion = await context.CourseCompletions
                .FirstOrDefaultAsync(cc => cc.UserId == userId && cc.CourseId == courseId);

            if (completion != null && !completion.CompletionRewardClaimed)
            {
                completion.CompletionRewardClaimed = true;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously claims the timed reward for a user in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ClaimTimeRewardAsync(int userId, int courseId)
        {
            var completion = await context.CourseCompletions
                .FirstOrDefaultAsync(cc => cc.UserId == userId && cc.CourseId == courseId);

            if (completion != null && !completion.TimedRewardClaimed)
            {
                completion.TimedRewardClaimed = true;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously retrieves the time spent by a user on a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The time spent by the user in seconds, or 0 if no data exists.</returns>
        public async Task<int> GetTimeSpentAsync(int userId, int courseId)
        {
            var time = await context.Enrollments.FindAsync(userId, courseId);
            return time?.TimeSpent ?? 0;
        }

        /// <summary>
        /// Asynchronously retrieves the time limit for completing a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The time limit for completing the course, or 0 if not set.</returns>
        public async Task<int> GetCourseTimeLimitAsync(int courseId)
        {
            var course = await context.Courses.FindAsync(courseId);
            return course?.TimeToComplete ?? 0;
        }

        /// <summary>
        /// Asynchronously retrieves a filtered list of courses based on various parameters.
        /// </summary>
        /// <param name="searchText">Search text to filter courses by title.</param>
        /// <param name="filterPremium">Flag to filter premium courses.</param>
        /// <param name="filterFree">Flag to filter free courses.</param>
        /// <param name="filterEnrolled">Flag to filter courses the user is enrolled in.</param>
        /// <param name="filterNotEnrolled">Flag to filter courses the user is not enrolled in.</param>
        /// <param name="userId">The ID of the user (for enrollment filtering).</param>
        /// <returns>A list of <see cref="Course"/> objects matching the filters.</returns>
        public async Task<List<CourseDto>> GetFilteredCoursesAsync(string searchText, bool filterPremium, bool filterFree, bool filterEnrolled, bool filterNotEnrolled, int userId)
        {
            var query = context.Courses
                .Include(c => c.CourseTags)
                .ThenInclude(ct => ct.Tag)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(c => c.Title.ToLower().Contains(searchText) || c.Description.ToLower().Contains(searchText));
            }

            if (filterPremium && !filterFree)
            {
                query = query.Where(c => c.IsPremium);
            }
            else if (filterFree && !filterPremium)
            {
                query = query.Where(c => !c.IsPremium);
            }

            if (filterEnrolled && !filterNotEnrolled)
            {
                query = query.Where(c => c.Enrollments.Any(e => e.UserId == userId));
            }
            else if (filterNotEnrolled && !filterEnrolled)
            {
                query = query.Where(c => !c.Enrollments.Any(e => e.UserId == userId));
            }

            return await query
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    IsPremium = c.IsPremium,
                    Cost = c.Cost,
                    ImageUrl = c.ImageUrl,
                    TimeToComplete = c.TimeToComplete,
                    Difficulty = c.Difficulty,
                    CourseTags = c.CourseTags.Select(ct => new TagDto
                    {
                        TagId = ct.TagId,
                        Name = ct.Tag.Name
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task AddTagToCourseAsync(int courseId, int tagId)
        {
            var exists = await context.CourseTags.AnyAsync(ct => ct.CourseId == courseId && ct.TagId == tagId);
            if (!exists)
            {
                context.CourseTags.Add(new CourseTag { CourseId = courseId, TagId = tagId });
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Module>> GetModulesByCourseIdAsync(int courseId)
        {
            return await context.Modules
                .Where(m => m.CourseId == courseId)
                .OrderBy(m => m.Position)
                .ToListAsync();
        }

        #endregion

        #region Exams

        /// <summary>
        /// Retrieves all exams from the database asynchronously.
        /// </summary>
        /// <returns>A list of all exams available in the database.</returns>
        public async Task<List<Exam>> GetExamsFromDbAsync()
        {
            return await context.Exams.Include(exam => exam.Exercises).ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific exam by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the exam to retrieve.</param>
        /// <returns>The exam with the specified ID if found; otherwise, null.</returns>
        public async Task<Exam?> GetExamByIdAsync(int id)
        {
            return await context.Exams.Include(exam => exam.Exercises).FirstAsync(e => e.Id == id);
        }

        /// <summary>
        /// Adds a new exam to the database asynchronously.
        /// </summary>
        /// <param name="exam">The exam to be added to the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddExamAsync(Exam exam)
        {
            context.Exams.Add(exam);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing exam in the database asynchronously.
        /// </summary>
        /// <param name="exam">The exam with updated information to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateExamAsync(Exam exam)
        {
            context.Exams.Update(exam);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an exam from the database asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the exam to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task completes once the exam is removed.</returns>
        public async Task DeleteExamAsync(int id)
        {
            var exam = await context.Exams.FindAsync(id);
            if (exam != null)
            {
                context.Exams.Remove(exam);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously adds a list of exercises to an exam.
        /// </summary>
        /// <param name="examId"></param>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        public async Task AddExerciseToExamAsync(int examId, int exerciseId)
        {
            var exam = await context.Exams
                .Include(q => q.Exercises)
                .FirstOrDefaultAsync(q => q.Id == examId);

            var exercise = await context.Exercises.FindAsync(exerciseId);

            if (exam != null && exercise != null && !exam.Exercises.Contains(exercise))
            {
                exam.Exercises.Add(exercise);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes an exercise from an exam.
        /// </summary>
        /// <param name="examId">The unique identifier of the exam.</param>
        /// <param name="exerciseId">The unique identifier of the exercise to remove.</param>
        public async Task RemoveExerciseFromExamAsync(int examId, int exerciseId)
        {
            var exam = await context.Exams
                .Include(q => q.Exercises)
                .FirstOrDefaultAsync(q => q.Id == examId);

            var exercise = await context.Exercises.FindAsync(exerciseId);

            if (exam != null && exercise != null && exam.Exercises.Contains(exercise))
            {
                exam.Exercises.Remove(exercise);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Retrieves an exam associated with a specific section asynchronously.
        /// </summary>
        /// <param name="sectionId">The unique identifier of the section to retrieve the exam from.</param>
        /// <returns>The exam associated with the specified section, or null if no exam exists for the section.</returns>
        public async Task<Exam?> GetExamFromSectionAsync(int sectionId)
        {
            return await context.Exams
                .Include(e => e.Exercises) // Ensure exercises related to the exam are included
                .FirstOrDefaultAsync(e => e.SectionId == sectionId);
        }

        /// <summary>
        /// Retrieves a list of all available exams that are not yet assigned to any section asynchronously.
        /// </summary>
        /// <returns>A list of exams that have no section assignment, making them available for future use.</returns>
        public async Task<List<Exam>> GetAvailableExamsAsync()
        {
            return await context.Exams
                .Include(e => e.Exercises) // Ensure exercises related to the exams are included
                .Where(e => e.SectionId == null) // Filter exams with no section assigned
                .ToListAsync();
        }

        /// <summary>
        /// Asynchronously checks if a user has completed a specific exam.
        /// </summary>
        /// <param name="userId"> Id of the user.</param>
        /// <param name="examId"> Id of the exam. </param>
        /// <returns> A task returning a boolean value indicating if the exam was completed.</returns>
        public async Task<bool> IsExamCompleted(int userId, int examId)
        {
            var completion = await this.context.ExamCompletions.FindAsync(userId, examId);

            if (completion == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Asynchronously marks an exam as completed for a user.
        /// </summary>
        /// <param name="completedExam"> The completed exam. </param>
        /// <returns> A task representing the async operation. </returns>
        public async Task CompleteExamForUser(ExamCompletions completedExam)
        {
            this.context.ExamCompletions.Add(completedExam);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an exam completion record for all users by exam ID asynchronously.
        /// </summary>
        /// <param name="examId"> Exam id.</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        public async Task DeleteExamCompletions(int examId)
        {
            var examCompletions = await this.context.ExamCompletions
                .Where(ec => ec.ExamId == examId)
                .ToListAsync();
            if (examCompletions != null)
            {
                this.context.ExamCompletions.RemoveRange(examCompletions);
                await this.context.SaveChangesAsync();
            }
        }

        #endregion

        #region Sections

        /// <summary>
        /// Retrieves all sections from the database asynchronously.
        /// </summary>
        /// <returns>A list of all sections in the database.</returns>
        public async Task<List<Section>> GetSectionsFromDbAsync()
        {
            return await context.Sections.
                Include(section => section.Quizzes).
                Include(section => section.Exam).
                ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific section by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the section to retrieve.</param>
        /// <returns>The section with the specified ID, or null if not found.</returns>
        public async Task<Section?> GetSectionByIdAsync(int id)
        {
            return await context.Sections.
                Include(section => section.Quizzes).
                Include(section => section.Exam).
                FirstOrDefaultAsync(section => section.Id == id);
        }

        /// <summary>
        /// Adds a new section to the database asynchronously.
        /// </summary>
        /// <param name="section">The section to be added to the database.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddSectionAsync(Section section)
        {
            context.Sections.Add(section);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing section in the database asynchronously.
        /// </summary>
        /// <param name="section">The section with updated information to save.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateSectionAsync(Section section)
        {
            context.Sections.Update(section);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a section from the database asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the section to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. The task is completed once the section is removed.</returns>
        public async Task DeleteSectionAsync(int id)
        {
            var section = await context.Sections.FindAsync(id);

            if (section != null)
            {
                context.Sections.Remove(section);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Checks if a section is open for a user asynchronously.
        /// </summary>
        /// <param name="userId"> The unique id of the user.</param>
        /// <param name="sectionId"> The unique id of the section. </param>
        /// <returns> A task returning a bool result indicating if the section is completed. </returns>
        public async Task<bool> IsSectionCompletedAsync(int userId, int sectionId)
        {
            var completion = await this.context.SectionCompletions
                .FindAsync(userId, sectionId);

            if (completion == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Marks a section as completed for a user asynchronously.
        /// </summary>
        /// <param name="completedSection"> SectionCompletions object reprsenting a section completed by an user. </param>
        /// <returns> A task representing the asynchronous operation. The task completes once the sectionCompletion is added. </returns>
        public async Task CompleteSectionForUser(SectionCompletions completedSection)
        {
            this.context.SectionCompletions.Add(completedSection);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a section completion record for all users by section ID asynchronously.
        /// </summary>
        /// <param name="sectionId"> Id of the section. </param>
        /// <returns> A task representing the asynchronous operation.</returns>
        public async Task DeleteSectionCompletions(int sectionId)
        {
            var sectionCompletions = await this.context.SectionCompletions
                .Where(sc => sc.SectionId == sectionId)
                .ToListAsync();
            if (sectionCompletions != null)
            {
                this.context.SectionCompletions.RemoveRange(sectionCompletions);
                await this.context.SaveChangesAsync();
            }
        }

        #endregion

        #region Coins

        /// <summary>
        /// Retrieves the coin balance for a user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose coin balance is to be retrieved.</param>
        /// <returns>The coin balance of the specified user. If the user is not found, returns 0.</returns>
        public async Task<int> GetUserCoinBalanceAsync(int userId)
        {
            try
            {
                var user = await context.Users.FindAsync(userId);
                return user?.CoinBalance ?? 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving coin balance for user {userId}: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Attempts to deduct coins from the user's wallet asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose wallet will be deducted.</param>
        /// <param name="cost">The number of coins to deduct from the user's wallet.</param>
        /// <returns>True if the deduction was successful, false if the user does not have enough coins.</returns>
        public async Task<bool> TryDeductCoinsFromUserWalletAsync(int userId, int cost)
        {
            try
            {
                var user = await context.Users.FindAsync(userId);
                if (user != null && user.CoinBalance >= cost)
                {
                    user.CoinBalance -= cost;
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deducting coins for user {userId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Adds a specified amount of coins to a user's wallet asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose wallet will be credited.</param>
        /// <param name="amount">The number of coins to add to the user's wallet.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddCoinsToUserWalletAsync(int userId, int amount)
        {
            try
            {
                var user = await context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.CoinBalance += amount;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error adding coins for user {userId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the last login time of a user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose last login time is to be retrieved.</param>
        /// <returns>The last login time of the user.</returns>
        /// <exception cref="Exception">Thrown when the user with the specified ID is not found.</exception>
        public async Task<DateTime> GetUserLastLoginTimeAsync(int userId)
        {
            try
            {
                var user = await context.Users.FindAsync(userId);
                if (user == null)
                {
                    var message = $"[ERROR] User with ID {userId} was not found at {DateTime.Now}.";
                    Console.Error.WriteLine(message);
                    throw new KeyNotFoundException(message);
                }
                return user.LastLoginTime;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving last login time for user {userId}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates the last login time of a user to the current date and time asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose last login time will be updated.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateUserLastLoginTimeToNowAsync(int userId)
        {
            try
            {
                var user = await context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.LastLoginTime = DateTime.Now;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error updating last login time for user {userId}: {ex.Message}");
            }
        }

        #endregion

        #region Roadmaps

        /// <summary>
        /// Asynchronously retrieves a specific roadmap by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the roadmap to retrieve.</param>
        /// <returns>A <see cref="Roadmap"/> object representing the specified roadmap, or null if not found.</returns>
        public async Task<Roadmap> GetRoadmapByIdAsync(int id)
        {
            return await context.Roadmaps.FindAsync(id);
        }
        #endregion
    }
}