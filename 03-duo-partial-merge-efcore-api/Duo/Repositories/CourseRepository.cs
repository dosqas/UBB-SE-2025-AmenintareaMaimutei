using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Duo.Models;
using Duo.ModelViews;

namespace Duo.Repositories
{
    /// <summary>
    /// Repository class responsible for managing courses, modules, enrollments, progress, and rewards.
    /// Interacts with various ModelViews to perform operations related to course management.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CourseRepository : ICourseRepository
    {
        // Course operations

        /// <summary>
        /// Retrieves a course by its ID.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The course corresponding to the provided ID, or null if not found.</returns>
        public Course? GetCourse(int courseId)
        {
            return CourseModelView.GetCourse(courseId);
        }

        /// <summary>
        /// Retrieves all courses.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        public List<Course> GetAllCourses()
        {
            return CourseModelView.GetAllCourses();
        }

        // Module operations

        /// <summary>
        /// Retrieves a module by its ID.
        /// </summary>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>The module corresponding to the provided ID, or null if not found.</returns>
        public Module? GetModule(int moduleId)
        {
            return ModuleModelView.GetModule(moduleId);
        }

        /// <summary>
        /// Retrieves all modules for a given course ID.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>A list of modules for the specified course.</returns>
        public List<Module> GetModulesByCourseId(int courseId)
        {
            return ModuleModelView.GetModulesByCourseId(courseId);
        }

        /// <summary>
        /// Checks if a module is available for a given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>True if the module is available for the user; otherwise, false.</returns>
        public bool IsModuleAvailable(int userId, int moduleId)
        {
            return ModuleModelView.IsModuleAvailable(userId, moduleId);
        }

        /// <summary>
        /// Checks if a module is open for a given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>True if the module is open for the user; otherwise, false.</returns>
        public bool IsModuleOpen(int userId, int moduleId)
        {
            return ModuleModelView.IsModuleOpen(userId, moduleId);
        }

        /// <summary>
        /// Checks if a module is completed by a given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>True if the module is completed by the user; otherwise, false.</returns>
        public bool IsModuleCompleted(int userId, int moduleId)
        {
            return ModuleModelView.IsModuleCompleted(userId, moduleId);
        }

        /// <summary>
        /// Checks if a module is in progress for a given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>True if the module is in progress for the user; otherwise, false.</returns>
        public bool IsModuleInProgress(int userId, int moduleId)
        {
            return ModuleModelView.IsModuleInProgress(userId, moduleId);
        }

        /// <summary>
        /// Checks if the module image has been clicked by a given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>True if the module image is clicked by the user; otherwise, false.</returns>
        public bool IsModuleImageClicked(int userId, int moduleId)
        {
            return ModuleModelView.IsModuleImageClicked(userId, moduleId);
        }

        /// <summary>
        /// Marks the specified module as open for the given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module to open.</param>
        public void OpenModule(int userId, int moduleId)
        {
            ModuleModelView.OpenModule(userId, moduleId);
        }

        /// <summary>
        /// Marks the specified module as completed for the given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module to complete.</param>
        public void CompleteModule(int userId, int moduleId)
        {
            ModuleModelView.CompleteModule(userId, moduleId);
        }

        /// <summary>
        /// Marks the module image as clicked for the given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        public void ClickModuleImage(int userId, int moduleId)
        {
            ModuleModelView.ClickModuleImage(userId, moduleId);
        }

        // Enrollment operations

        /// <summary>
        /// Checks if a user is enrolled in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the user is enrolled in the course; otherwise, false.</returns>
        public bool IsUserEnrolled(int userId, int courseId)
        {
            return EnrollmentModelView.IsUserEnrolled(userId, courseId);
        }

        /// <summary>
        /// Enrolls a user in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        public void EnrollUser(int userId, int courseId)
        {
            EnrollmentModelView.EnrollUser(userId, courseId);
        }

        // Progress tracking

        /// <summary>
        /// Updates the time spent by a user in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="seconds">The number of seconds to update the time spent.</param>
        public void UpdateTimeSpent(int userId, int courseId, int seconds)
        {
            ProgressModelView.UpdateTimeSpent(userId, courseId, seconds);
        }

        /// <summary>
        /// Retrieves the total time spent by a user in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The total time spent by the user in the course.</returns>
        public int GetTimeSpent(int userId, int courseId)
        {
            return ProgressModelView.GetTimeSpent(userId, courseId);
        }

        /// <summary>
        /// Retrieves the required number of modules for a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The required number of modules for the course.</returns>
        public int GetRequiredModulesCount(int courseId)
        {
            return ProgressModelView.GetRequiredModulesCount(courseId);
        }

        /// <summary>
        /// Retrieves the number of completed modules by a user in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The number of completed modules.</returns>
        public int GetCompletedModulesCount(int userId, int courseId)
        {
            return ProgressModelView.GetCompletedModulesCount(userId, courseId);
        }

        /// <summary>
        /// Checks if a user has completed a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the course is completed; otherwise, false.</returns>
        public bool IsCourseCompleted(int userId, int courseId)
        {
            return ProgressModelView.IsCourseCompleted(userId, courseId);
        }

        /// <summary>
        /// Marks a course as completed for a given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        public void MarkCourseAsCompleted(int userId, int courseId)
        {
            ProgressModelView.MarkCourseAsCompleted(userId, courseId);
        }

        // Reward operations

        /// <summary>
        /// Claims the completion reward for a user upon course completion.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the reward was claimed successfully; otherwise, false.</returns>
        public bool ClaimCompletionReward(int userId, int courseId)
        {
            return RewardModelView.ClaimCompletionReward(userId, courseId);
        }

        /// <summary>
        /// Claims a timed reward for the user based on their time spent on the course and the time limit.
        /// </summary>
        /// <param name="userId">The ID of the user claiming the reward.</param>
        /// <param name="courseId">The ID of the course for which the reward is being claimed.</param>
        /// <param name="timeSpent">The time the user has spent on the course.</param>
        /// <param name="timeLimit">The time limit for the reward.</param>
        /// <returns>True if the reward is successfully claimed; otherwise, false.</returns>
        public bool ClaimTimedReward(int userId, int courseId, int timeSpent, int timeLimit)
        {
            return RewardModelView.ClaimTimedReward(userId, courseId, timeSpent, timeLimit);
        }

        /// <summary>
        /// Retrieves the time limit for a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course to retrieve the time limit for.</param>
        /// <returns>The time limit for the course in seconds.</returns>
        public int GetCourseTimeLimit(int courseId)
        {
            return RewardModelView.GetCourseTimeLimit(courseId);
        }

        /// <summary>
        /// Retrieves all available tags.
        /// </summary>
        /// <returns>A list of all tags.</returns>
        public List<Tag> GetAllTags()
        {
            return TagModelView.GetAllTags();
        }

        /// <summary>
        /// Retrieves all tags associated with a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course to retrieve tags for.</param>
        /// <returns>A list of tags associated with the course.</returns>
        public List<Tag> GetTagsForCourse(int courseId)
        {
            return TagModelView.GetTagsForCourse(courseId);
        }
    }
}