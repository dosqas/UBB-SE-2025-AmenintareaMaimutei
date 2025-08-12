using System.Collections.Generic;
using CourseApp.Models;

namespace CourseApp.Repository
{
    /// <summary>
    /// Interface for course-related data operations
    /// </summary>
    public interface ICourseRepository
    {
        // Course operations

        /// <summary>
        /// Gets the course by its ID.
        /// </summary>
        /// <param name="courseId">The ID of the course to retrieve.</param>
        /// <returns>The course corresponding to the given ID, or null if not found.</returns>
        Course? GetCourse(int courseId);

        /// <summary>
        /// Gets all available courses.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        List<Course> GetAllCourses();

        // Module operations

        /// <summary>
        /// Gets the module by its ID.
        /// </summary>
        /// <param name="moduleId">The ID of the module to retrieve.</param>
        /// <returns>The module corresponding to the given ID, or null if not found.</returns>
        Module? GetModule(int moduleId);

        /// <summary>
        /// Gets all modules for a given course.
        /// </summary>
        /// <param name="courseId">The ID of the course whose modules are to be retrieved.</param>
        /// <returns>A list of modules for the specified course.</returns>
        List<Module> GetModulesByCourseId(int courseId);

        /// <summary>
        /// Checks if a module is available for the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>True if the module is available for the user; otherwise, false.</returns>
        bool IsModuleAvailable(int userId, int moduleId);

        /// <summary>
        /// Checks if a module is open for the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>True if the module is open for the user; otherwise, false.</returns>
        bool IsModuleOpen(int userId, int moduleId);

        /// <summary>
        /// Checks if a module has been completed by the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>True if the module has been completed; otherwise, false.</returns>
        bool IsModuleCompleted(int userId, int moduleId);

        /// <summary>
        /// Checks if a module is in progress for the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>True if the module is in progress; otherwise, false.</returns>
        bool IsModuleInProgress(int userId, int moduleId);

        /// <summary>
        /// Checks if the user has clicked on the module's image.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>True if the module image has been clicked; otherwise, false.</returns>
        bool IsModuleImageClicked(int userId, int moduleId);

        /// <summary>
        /// Opens the module for the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module to open.</param>
        void OpenModule(int userId, int moduleId);

        /// <summary>
        /// Marks the module as completed for the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module to mark as completed.</param>
        void CompleteModule(int userId, int moduleId);

        /// <summary>
        /// Marks the module's image as clicked by the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module whose image is clicked.</param>
        void ClickModuleImage(int userId, int moduleId);

        // Enrollment operations

        /// <summary>
        /// Checks if the user is enrolled in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the user is enrolled in the course; otherwise, false.</returns>
        bool IsUserEnrolled(int userId, int courseId);

        /// <summary>
        /// Enrolls the user in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        void EnrollUser(int userId, int courseId);

        // Progress tracking

        /// <summary>
        /// Updates the time spent by the user on a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="seconds">The number of seconds the user has spent on the course.</param>
        void UpdateTimeSpent(int userId, int courseId, int seconds);

        /// <summary>
        /// Gets the total time spent by the user on a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The total time the user has spent on the course, in seconds.</returns>
        int GetTimeSpent(int userId, int courseId);

        /// <summary>
        /// Gets the number of required modules for a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The number of required modules for the course.</returns>
        int GetRequiredModulesCount(int courseId);

        /// <summary>
        /// Gets the number of modules completed by the user for a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The number of completed modules for the course by the user.</returns>
        int GetCompletedModulesCount(int userId, int courseId);

        /// <summary>
        /// Checks if the user has completed a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the user has completed the course; otherwise, false.</returns>
        bool IsCourseCompleted(int userId, int courseId);

        /// <summary>
        /// Marks a course as completed for the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course to mark as completed.</param>
        void MarkCourseAsCompleted(int userId, int courseId);

        // Reward operations

        /// <summary>
        /// Claims a reward for course completion.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the reward was successfully claimed; otherwise, false.</returns>
        bool ClaimCompletionReward(int userId, int courseId);

        /// <summary>
        /// Claims a reward based on the time spent on a course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="timeSpent">The time spent on the course, in seconds.</param>
        /// <param name="timeLimit">The time limit for claiming the reward, in seconds.</param>
        /// <returns>True if the reward was successfully claimed; otherwise, false.</returns>
        bool ClaimTimedReward(int userId, int courseId, int timeSpent, int timeLimit);

        /// <summary>
        /// Gets the time limit for completing a course and claiming the reward.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The time limit for completing the course, in seconds.</returns>
        int GetCourseTimeLimit(int courseId);

        // Tag operations

        /// <summary>
        /// Gets all available tags.
        /// </summary>
        /// <returns>A list of all tags.</returns>
        List<Tag> GetAllTags();

        /// <summary>
        /// Gets the tags for a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>A list of tags associated with the course.</returns>
        List<Tag> GetTagsForCourse(int courseId);
    }
}
