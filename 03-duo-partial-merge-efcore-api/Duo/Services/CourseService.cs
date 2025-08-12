using System;
using System.Collections.Generic;
using System.Linq;
using Duo.Models;
using Duo.Repositories;
using Duo.ModelViews;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

namespace Duo.Services
{
    /// <summary>
    /// Provides core business logic for managing courses, modules, and user interactions.
    /// </summary>
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository repository;
        private readonly ICoinsRepository coinsRepository = new CoinsRepository(new UserWalletModelView());
        private const int UserId = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseService"/> class with optional repository injection.
        /// </summary>
        public CourseService(ICourseRepository? courseRepository = null)
        {
            repository = courseRepository ?? new CourseRepository();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseService"/> class with injected course and coins repositories.
        /// </summary>
        public CourseService(ICourseRepository repository, ICoinsRepository coinsRepository)
        {
            this.repository = repository;
            this.coinsRepository = coinsRepository;
        }

        /// <summary>
        /// Retrieves all available courses.
        /// </summary>
        public List<Course> GetCourses() => repository.GetAllCourses();

        /// <summary>
        /// Retrieves all available tags.
        /// </summary>
        public List<Tag> GetTags() => repository.GetAllTags();

        /// <summary>
        /// Gets all tags associated with a specific course.
        /// </summary>
        public List<Tag> GetCourseTags(int courseId) => repository.GetTagsForCourse(courseId);

        /// <summary>
        /// Opens a module for the user if not already opened.
        /// </summary>
        public void OpenModule(int moduleId)
        {
            if (!repository.IsModuleOpen(UserId, moduleId))
            {
                repository.OpenModule(UserId, moduleId);
            }
        }

        /// <summary>
        /// Retrieves all modules for a specific course.
        /// </summary>
        public List<Module> GetModules(int courseId) => repository.GetModulesByCourseId(courseId);

        /// <summary>
        /// Retrieves all non-bonus modules for a course.
        /// </summary>
        public List<Module> GetNormalModules(int courseId) =>
            [.. repository.GetModulesByCourseId(courseId).Where(m => !m.IsBonus)];

        /// <summary>
        /// Attempts to buy a bonus module if not yet opened and enough coins exist.
        /// </summary>
        public bool BuyBonusModule(int moduleId, int courseId)
        {
            var module = repository.GetModule(moduleId);
            if (module == null || !module.IsBonus || repository.IsModuleOpen(UserId, moduleId))
            {
                return false;
            }

            var course = repository.GetCourse(courseId);
            if (course == null)
            {
                return false;
            }

            if (!coinsRepository.TryDeductCoinsFromUserWallet(UserId, module.Cost))
            {
                return false;
            }

            repository.OpenModule(UserId, moduleId);
            return true;
        }

        /// <summary>
        /// Enrolls the user in a course if not already enrolled and deducts coins if premium.
        /// </summary>
        public bool EnrollInCourse(int courseId)
        {
            if (repository.IsUserEnrolled(UserId, courseId))
            {
                return false;
            }

            var course = repository.GetCourse(courseId);
            if (course == null)
            {
                return false;
            }

            if (course.IsPremium && !coinsRepository.TryDeductCoinsFromUserWallet(UserId, course.Cost))
            {
                return false;
            }

            repository.EnrollUser(UserId, courseId);
            return true;
        }

        /// <summary>
        /// Completes a module and checks for course completion status.
        /// </summary>
        public void CompleteModule(int moduleId, int courseId)
        {
            repository.CompleteModule(UserId, moduleId);

            if (repository.IsCourseCompleted(UserId, courseId))
            {
                repository.MarkCourseAsCompleted(UserId, courseId);
            }
        }

        /// <summary>
        /// Returns whether the user is enrolled in the specified course.
        /// </summary>
        public bool IsUserEnrolled(int courseId) => repository.IsUserEnrolled(UserId, courseId);

        /// <summary>
        /// Returns whether the user has completed the specified module.
        /// </summary>
        public bool IsModuleCompleted(int moduleId) => repository.IsModuleCompleted(UserId, moduleId);

        /// <summary>
        /// Filters courses based on search text, type, enrollment status, and tags.
        /// </summary>
        public List<Course> GetFilteredCourses(string searchText, bool filterPremium, bool filterFree, bool filterEnrolled, bool filterNotEnrolled, List<int> selectedTagIds)
        {
            var courses = repository.GetAllCourses();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                courses = [.. courses.Where(c => c.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))];
            }

            if (filterPremium && filterFree)
            {
                courses = [];
            }
            else if (filterPremium)
            {
                courses = [.. courses.Where(c => c.IsPremium)];
            }
            else if (filterFree)
            {
                courses = [.. courses.Where(c => !c.IsPremium)];
            }
            if (filterEnrolled && filterNotEnrolled)
            {
                courses = [];
            }
            else if (filterEnrolled)
            {
                courses = [.. courses.Where(c => repository.IsUserEnrolled(UserId, c.CourseId))];
            }
            else if (filterNotEnrolled)
            {
                courses = [.. courses.Where(c => !repository.IsUserEnrolled(UserId, c.CourseId))];
            }

                if (selectedTagIds.Count > 0)
            {
                courses = [.. courses.Where(c =>
                {
                    var courseTagIds = repository.GetTagsForCourse(c.CourseId).Select(t => t.TagId).ToList();
                    return selectedTagIds.All(id => courseTagIds.Contains(id));
                })];
            }

            return courses;
        }

        /// <summary>
        /// Updates the time the user has spent on a course.
        /// </summary>
        public void UpdateTimeSpent(int courseId, int seconds) => repository.UpdateTimeSpent(UserId, courseId, seconds);

        /// <summary>
        /// Retrieves the time the user has spent on a course.
        /// </summary>
        public int GetTimeSpent(int courseId) => repository.GetTimeSpent(UserId, courseId);

        /// <summary>
        /// Handles user interaction with module images and rewards coins if not previously clicked.
        /// </summary>
        public bool ClickModuleImage(int moduleId)
        {
            if (repository.IsModuleImageClicked(UserId, moduleId))
            {
                return false;
            }
                repository.ClickModuleImage(UserId, moduleId);
            coinsRepository.AddCoinsToUserWallet(UserId, 10);
            return true;
        }

        /// <summary>
        /// Checks if a module is in progress.
        /// </summary>
        public bool IsModuleInProgress(int moduleId) => repository.IsModuleInProgress(UserId, moduleId);

        /// <summary>
        /// Checks if a module is available for the user.
        /// </summary>
        public bool IsModuleAvailable(int moduleId) => repository.IsModuleAvailable(UserId, moduleId);

        /// <summary>
        /// Checks if a course has been completed by the user.
        /// </summary>
        public bool IsCourseCompleted(int courseId) => repository.IsCourseCompleted(UserId, courseId);

        /// <summary>
        /// Gets the number of completed modules in a course.
        /// </summary>
        public int GetCompletedModulesCount(int courseId) => repository.GetCompletedModulesCount(UserId, courseId);

        /// <summary>
        /// Gets the number of required modules for a course.
        /// </summary>
        public int GetRequiredModulesCount(int courseId) => repository.GetRequiredModulesCount(courseId);

        /// <summary>
        /// Claims the course completion reward if eligible.
        /// </summary>
        public bool ClaimCompletionReward(int courseId)
        {
            bool claimed = repository.ClaimCompletionReward(UserId, courseId);
            if (claimed)
            {
                coinsRepository.AddCoinsToUserWallet(UserId, 50);
            }
            return claimed;
        }

        /// <summary>
        /// Claims a reward if the course was completed within a time limit.
        /// </summary>
        public bool ClaimTimedReward(int courseId, int timeSpent)
        {
            int timeLimit = repository.GetCourseTimeLimit(courseId);
            bool claimed = repository.ClaimTimedReward(UserId, courseId, timeSpent, timeLimit);
            if (claimed)
            {
                coinsRepository.AddCoinsToUserWallet(UserId, 300); // Hardcoded reward
            }
            return claimed;
        }

        /// <summary>
        /// Retrieves the time limit for completing a course.
        /// </summary>
        public int GetCourseTimeLimit(int courseId) => repository.GetCourseTimeLimit(courseId);
    }
}
