using Duo.Api.DTO;
using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Roadmaps;
using Duo.Api.Models.Sections;
using Duo.Models.Quizzes.API;
using Microsoft.AspNetCore.Mvc;

namespace Duo.Api.Repositories
{
    public interface IRepository
    {
        #region Users

        /// <summary>
        /// Retrieves all users from the database asynchronously.
        /// </summary>
        /// <returns>A list of all users in the database.</returns>
        public Task<List<User>> GetUsersFromDbAsync();

        /// <summary>
        /// Retrieves a specific user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>The user with the specified ID, or null if not found.</returns>
        public Task<User?> GetUserByIdAsync(int id);

        /// <summary>
        /// Adds a new user to the database asynchronously.
        /// </summary>
        /// <param name="user">The user to be added to the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddUserAsync(User user);

        /// <summary>
        /// Updates an existing user in the database asynchronously.
        /// </summary>
        /// <param name="user">The user with updated information to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateUserAsync(User user);

        /// <summary>
        /// Deletes a user from the database asynchronously by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to be deleted.</param>
        /// <returns>A task representing the asynchronous operation. The task completes once the user is removed.</returns>
        public Task DeleteUserAsync(int id);

        #endregion

        #region Tags

        /// <summary>
        /// Retrieves all tags from the database asynchronously.
        /// </summary>
        /// <returns>A list of all tags in the database.</returns>
        public Task<List<Tag>> GetTagsFromDbAsync();

        /// <summary>
        /// Retrieves a specific tag by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the tag to retrieve.</param>
        /// <returns>The tag with the specified ID, or null if not found.</returns>
        public Task<Tag?> GetTagByIdAsync(int id);

        /// <summary>
        /// Adds a new tag to the database asynchronously.
        /// </summary>
        /// <param name="tag">The tag to be added to the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddTagAsync(Tag tag);

        /// <summary>
        /// Updates an existing tag in the database asynchronously.
        /// </summary>
        /// <param name="tag">The tag with updated information to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateTagAsync(Tag tag);

        /// <summary>
        /// Deletes a tag from the database asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tag to be deleted.</param>
        /// <returns>A task representing the asynchronous operation. The task completes once the tag is removed.</returns>
        public Task DeleteTagAsync(int id);

        Task<List<Tag>> GetTagsForCourseAsync(int courseId);

        Task AddTagToCourseAsync(int courseId, int tagId);

        Task<int> GetCompletedModulesCountAsync(int userId, int courseId);

        Task<int> GetRequiredModulesCountAsync(int courseId);

        #endregion

        #region Modules

        /// <summary>
        /// Retrieves all modules from the database asynchronously.
        /// </summary>
        /// <returns>A list of all modules in the database.</returns>
        public Task<List<Module>> GetModulesFromDbAsync();

        /// <summary>
        /// Retrieves a specific module by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the module to retrieve.</param>
        /// <returns>The module with the specified ID, or null if not found.</returns>
        public Task<Module?> GetModuleByIdAsync(int id);

        /// <summary>
        /// Adds a new module to the database asynchronously.
        /// </summary>
        /// <param name="module">The module to be added to the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddModuleAsync(Module module);

        /// <summary>
        /// Updates an existing module in the database asynchronously.
        /// </summary>
        /// <param name="module">The module with updated information to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateModuleAsync(Module module);

        /// <summary>
        /// Deletes a module from the database asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the module to be deleted.</param>
        /// <returns>A task representing the asynchronous operation. The task completes once the module is removed.</returns>
        public Task DeleteModuleAsync(int id);

        /// <summary>
        /// Marks a module as completed for a user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who is completing the module.</param>
        /// <param name="moduleId">The unique identifier of the module to be marked as completed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task CompleteModuleAsync(int userId, int moduleId);

        /// <summary>
        /// Checks if a module is open for a user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <returns>True if the module is open for the user, otherwise false.</returns>
        public Task<bool> IsModuleOpenAsync(int userId, int moduleId);

        /// <summary>
        /// Checks if a module is completed for a user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <returns>True if the module is completed by the user, otherwise false.</returns>
        public Task<bool> IsModuleCompletedAsync(int userId, int moduleId);

        /// <summary>
        /// Checks if a module is available for a user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <returns>True if the module is available for the user, otherwise false.</returns>
        public Task<bool> IsModuleAvailableAsync(int userId, int moduleId);

        /// <summary>
        /// Records a click event for a module image by a user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user clicking the image.</param>
        /// <param name="moduleId">The unique identifier of the module whose image is being clicked.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task ClickModuleImageAsync(int userId, int moduleId);

        /// <summary>
        /// Checks if a user has clicked the module image asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <returns>True if the user has clicked the module image, otherwise false.</returns>
        public Task<bool> IsModuleImageClickedAsync(int userId, int moduleId);

        #endregion

        #region Exercises

        /// <summary>
        /// Retrieves all exercises from the database asynchronously.
        /// </summary>
        /// <returns>A list of all exercises in the database.</returns>
        public Task<List<Exercise>> GetExercisesFromDbAsync();

        /// <summary>
        /// Retrieves a specific exercise by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the exercise to retrieve.</param>
        /// <returns>The exercise with the specified ID, or null if not found.</returns>
        public Task<Exercise?> GetExerciseByIdAsync(int id);

        /// <summary>
        /// Adds a new exercise to the database asynchronously.
        /// </summary>
        /// <param name="exercise">The exercise to be added to the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddExerciseAsync(Exercise exercise);

        /// <summary>
        /// Updates an existing exercise in the database asynchronously.
        /// </summary>
        /// <param name="exercise">The exercise with updated information to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateExerciseAsync(Exercise exercise);

        /// <summary>
        /// Deletes an exercise from the database asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the exercise to be deleted.</param>
        /// <returns>A task representing the asynchronous operation. The task completes once the exercise is removed.</returns>
        public Task DeleteExerciseAsync(int id);

        #endregion

        #region Quizzes

        /// <summary>
        /// Retrieves all quizzes from the database asynchronously.
        /// </summary>
        /// <returns>A list of all quizzes in the database.</returns>
        public Task<List<Quiz>> GetQuizzesFromDbAsync();

        /// <summary>
        /// Retrieves a specific quiz by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the quiz to retrieve.</param>
        /// <returns>The quiz with the specified ID, or null if not found.</returns>
        public Task<Quiz?> GetQuizByIdAsync(int id);

        /// <summary>
        /// Adds a new quiz to the database asynchronously.
        /// </summary>
        /// <param name="quiz">The quiz to be added to the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddQuizAsync(Quiz quiz);

        /// <summary>
        /// Updates an existing quiz in the database asynchronously.
        /// </summary>
        /// <param name="quiz">The quiz with updated information to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateQuizAsync(Quiz quiz);

        /// <summary>
        /// Deletes a quiz from the database asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the quiz to be deleted.</param>
        /// <returns>A task representing the asynchronous operation. The task completes once the quiz is removed.</returns>
        public Task DeleteQuizAsync(int id);

        /// <summary>
        /// Retrieves all quizzes associated with a specific section asynchronously.
        /// </summary>
        /// <param name="sectionId">The unique identifier of the section whose quizzes are to be retrieved.</param>
        /// <returns>A list of quizzes associated with the specified section.</returns>
        public Task<List<Quiz>> GetAllQuizzesFromSectionAsync(int sectionId);

        /// <summary>
        /// Counts the number of quizzes in a specific section asynchronously.
        /// </summary>
        /// <param name="sectionId">The unique identifier of the section to count quizzes from.</param>
        /// <returns>The number of quizzes in the specified section.</returns>
        public Task<int> CountQuizzesFromSectionAsync(int sectionId);

        /// <summary>
        /// Retrieves the last order number from a specific section asynchronously.
        /// </summary>
        /// <param name="sectionId">The unique identifier of the section to retrieve the last order number from.</param>
        /// <returns>The last order number in the specified section.</returns>
        public Task<int> GetLastOrderNumberFromSectionAsync(int sectionId);

        /// <summary>
        /// Adds a list of exercises to a quiz asynchronously.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz to which exercises are being added.</param>
        /// <param name="exerciseIds">A list of unique identifiers of the exercises to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddExercisesToQuizAsync(int quizId, List<int> exerciseIds);

        /// <summary>
        /// Adds a single exercise to a quiz asynchronously.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz to which the exercise is being added.</param>
        /// <param name="exerciseId">The unique identifier of the exercise to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddExerciseToQuizAsync(int quizId, int exerciseId);

        /// <summary>
        /// Removes a specific exercise from a quiz asynchronously.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz from which the exercise is being removed.</param>
        /// <param name="exerciseId">The unique identifier of the exercise to be removed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task RemoveExerciseFromQuizAsync(int quizId, int exerciseId);

        /// <summary>
        /// Retrieves the result of a quiz asynchronously.
        /// </summary>
        /// <param name="quizId">The unique identifier of the quiz whose result is to be retrieved.</param>
        /// <returns>An object representing the quiz result, which can be customized as needed.</returns>
        public Task<QuizResult> GetQuizResultAsync(int quizId);

        /// <summary>
        /// Saves a quiz submitted by a user
        /// </summary>
        /// <param name="submission">Submitted quiz entity</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task SaveQuizSubmissionAsync(QuizSubmissionEntity submission);

        /// <summary>
        /// Gets quiz submission by the given quiz id
        /// </summary>
        /// <param name="quizId">Id of submitted quiz you want to retrieve</param>
        /// <returns>An object representing the quiz submitted by the user</returns>
        Task<QuizSubmissionEntity?> GetSubmissionByQuizIdAsync(int quizId);

        /// <summary>
        /// Retrieves a list of all available quizzes asynchronously.
        /// </summary>
        /// <returns>A list of all available quizzes in the database.</returns>
        public Task<List<Quiz>> GetAvailableQuizzesAsync();

        #endregion

        #region Courses

        /// <summary>
        /// Retrieves all courses from the database asynchronously.
        /// </summary>
        /// <returns>A list of all courses in the database.</returns>
        public Task<List<Course>> GetCoursesFromDbAsync();

        /// <summary>
        /// Retrieves a specific course by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the course to retrieve.</param>
        /// <returns>The course with the specified ID, or null if not found.</returns>
        public Task<Course?> GetCourseByIdAsync(int id);

        /// <summary>
        /// Adds a new course to the database asynchronously.
        /// </summary>
        /// <param name="course">The course to be added to the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddCourseAsync(Course course);

        /// <summary>
        /// Updates an existing course in the database asynchronously.
        /// </summary>
        /// <param name="course">The course with updated information to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateCourseAsync(Course course);

        /// <summary>
        /// Deletes a course from the database asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the course to be deleted.</param>
        /// <returns>A task representing the asynchronous operation. The task completes once the course is removed.</returns>
        public Task DeleteCourseAsync(int id);

        /// <summary>
        /// Enrolls a user in a specific course asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to enroll.</param>
        /// <param name="courseId">The unique identifier of the course to enroll in.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task EnrollUserInCourseAsync(int userId, int courseId);

        /// <summary>
        /// Checks if a user is enrolled in a specific course asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="courseId">The unique identifier of the course to check enrollment in.</param>
        /// <returns>True if the user is enrolled in the course, otherwise false.</returns>
        public Task<bool> IsUserEnrolledInCourseAsync(int userId, int courseId);

        /// <summary>
        /// Checks if a user has completed a specific course asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="courseId">The unique identifier of the course to check completion for.</param>
        /// <returns>True if the user has completed the course, otherwise false.</returns>
        public Task<bool> IsCourseCompletedAsync(int userId, int courseId);

        /// <summary>
        /// Updates the amount of time a user has spent on a course asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="timeInSeconds">The time spent in seconds to be updated.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateTimeSpentAsync(int userId, int courseId, int timeInSeconds);

        /// <summary>
        /// Claims the completion reward for a user in a specific course asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="courseId">The unique identifier of the course to claim the reward for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task ClaimCompletionRewardAsync(int userId, int courseId);

        /// <summary>
        /// Claims the time reward for a user based on time spent in a specific course asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="courseId">The unique identifier of the course to claim the reward for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task ClaimTimeRewardAsync(int userId, int courseId);

        /// <summary>
        /// Retrieves the total time spent by a user in a specific course asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="courseId">The unique identifier of the course to get the time spent in.</param>
        /// <returns>The total time in seconds that the user has spent in the course.</returns>
        public Task<int> GetTimeSpentAsync(int userId, int courseId);

        /// <summary>
        /// Retrieves the time limit for a course asynchronously.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course to get the time limit for.</param>
        /// <returns>The time limit for the course in seconds.</returns>
        public Task<int> GetCourseTimeLimitAsync(int courseId);

        /// <summary>
        /// Retrieves a filtered list of courses based on various criteria asynchronously.
        /// </summary>
        /// <param name="searchText">Text to search for in course titles or descriptions.</param>
        /// <param name="filterPremium">Whether to filter for premium courses.</param>
        /// <param name="filterFree">Whether to filter for free courses.</param>
        /// <param name="filterEnrolled">Whether to filter for courses the user is already enrolled in.</param>
        /// <param name="filterNotEnrolled">Whether to filter for courses the user is not enrolled in.</param>
        /// <param name="userId">The unique identifier of the user for whom the courses are being filtered.</param>
        /// <returns>A list of courses that match the filter criteria.</returns>
        public Task<List<Course>> GetFilteredCoursesAsync(string searchText, bool filterPremium, bool filterFree, bool filterEnrolled, bool filterNotEnrolled, int userId);

        Task<List<Module>> GetModulesByCourseIdAsync(int courseId);

        #endregion

        #region Exams

        /// <summary>
        /// Retrieves all exams from the database asynchronously.
        /// </summary>
        /// <returns>A list of all exams in the database.</returns>
        public Task<List<Exam>> GetExamsFromDbAsync();

        /// <summary>
        /// Retrieves a specific exam by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the exam to retrieve.</param>
        /// <returns>The exam with the specified ID, or null if not found.</returns>
        public Task<Exam?> GetExamByIdAsync(int id);

        /// <summary>
        /// Adds a new exam to the database asynchronously.
        /// </summary>
        /// <param name="exam">The exam to be added to the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddExamAsync(Exam exam);

        /// <summary>
        /// Updates an existing exam in the database asynchronously.
        /// </summary>
        /// <param name="exam">The exam with updated information to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateExamAsync(Exam exam);

        /// <summary>
        /// Deletes an exam from the database asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the exam to be deleted.</param>
        /// <returns>A task representing the asynchronous operation. The task completes once the exam is removed.</returns>
        public Task DeleteExamAsync(int id);

        /// <summary>
        /// Retrieves an exam associated with a specific section asynchronously.
        /// </summary>
        /// <param name="sectionId">The unique identifier of the section to retrieve the exam from.</param>
        /// <returns>The exam associated with the section, or null if no exam exists for the section.</returns>
        public Task<Exam?> GetExamFromSectionAsync(int sectionId);

        /// <summary>
        /// Adds a single exercise to an exam asynchronously.
        /// </summary>
        /// <param name="examId">The unique identifier of the exam to which the exercise is being added.</param>
        /// <param name="exerciseId">The unique identifier of the exercise to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddExerciseToExamAsync(int examId, int exerciseId);

        /// <summary>
        /// Removes a specific exercise from a quiz asynchronously.
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        public Task RemoveExerciseFromExamAsync(int examId, int exerciseId);

        /// <summary>
        /// Retrieves a list of all available exams asynchronously.
        /// </summary>
        /// <returns>A list of all available exams in the database.</returns>
        public Task<List<Exam>> GetAvailableExamsAsync();

        #endregion

        #region Sections

        /// <summary>
        /// Retrieves all sections from the database asynchronously.
        /// </summary>
        /// <returns>A list of all sections in the database.</returns>
        public Task<List<Section>> GetSectionsFromDbAsync();

        /// <summary>
        /// Retrieves a specific section by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the section to retrieve.</param>
        /// <returns>The section with the specified ID, or null if not found.</returns>
        public Task<Section?> GetSectionByIdAsync(int id);

        /// <summary>
        /// Adds a new section to the database asynchronously.
        /// </summary>
        /// <param name="section">The section to be added to the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddSectionAsync(Section section);

        /// <summary>
        /// Updates an existing section in the database asynchronously.
        /// </summary>
        /// <param name="section">The section with updated information to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateSectionAsync(Section section);

        /// <summary>
        /// Deletes a section from the database asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the section to be deleted.</param>
        /// <returns>A task representing the asynchronous operation. The task completes once the section is removed.</returns>
        public Task DeleteSectionAsync(int id);

        #endregion

        #region Coins

        /// <summary>
        /// Retrieves the current coin balance of a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose coin balance is to be retrieved.</param>
        /// <returns>The current coin balance of the specified user.</returns>
        public Task<int> GetUserCoinBalanceAsync(int userId);

        /// <summary>
        /// Attempts to deduct a specified number of coins from a user's wallet asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose coins are to be deducted.</param>
        /// <param name="cost">The number of coins to be deducted.</param>
        /// <returns>True if the coins were successfully deducted, otherwise false (e.g., if the user does not have enough coins).</returns>
        public Task<bool> TryDeductCoinsFromUserWalletAsync(int userId, int cost);

        /// <summary>
        /// Adds a specified number of coins to a user's wallet asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to whom the coins will be added.</param>
        /// <param name="amount">The number of coins to add to the user's wallet.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddCoinsToUserWalletAsync(int userId, int amount);

        /// <summary>
        /// Retrieves the last login time of a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose last login time is to be retrieved.</param>
        /// <returns>The last login time of the specified user.</returns>
        public Task<DateTime> GetUserLastLoginTimeAsync(int userId);

        /// <summary>
        /// Updates the last login time of a specific user to the current time asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose last login time is to be updated.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateUserLastLoginTimeToNowAsync(int userId);

        /// <summary>
        /// Opens a module for a user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who is accessing the module.</param>
        /// <param name="moduleId">The unique identifier of the module to open.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task OpenModuleAsync(int userId, int moduleId);

        #endregion

        #region Roadmaps

        /// <summary>
        /// Asynchronously retrieves a specific roadmap by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the roadmap to retrieve.</param>
        /// <returns>A <see cref="Roadmap"/> object representing the specified roadmap, or null if not found.</returns>
        Task<Roadmap> GetRoadmapByIdAsync(int id);

        #endregion
    }
}