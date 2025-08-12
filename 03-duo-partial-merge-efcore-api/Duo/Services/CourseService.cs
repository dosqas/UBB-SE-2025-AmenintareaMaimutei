using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duo.Models;
using Duo.Services.Interfaces;

namespace Duo.Services
{
    /// <summary>
    /// Provides core business logic for managing courses, modules, and user interactions via API calls.
    /// </summary>
    public class CourseService : ICourseService
    {
        private readonly ICourseServiceProxy courseServiceProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ICourseServiceProxy"/> class.
        /// </summary>
        public CourseService(ICourseServiceProxy courseServiceProxy)
        {
            this.courseServiceProxy = courseServiceProxy;
        }

        /// <summary>
        /// Retrieves all available courses.
        /// </summary>
        public async Task<List<Course>> GetCoursesAsync()
        {
            return await courseServiceProxy.GetAllCourses();
        }

        /// <summary>
        /// Retrieves all available tags.
        /// </summary>
        public async Task<List<Tag>> GetTagsAsync()
        {
            return await courseServiceProxy.GetAllTags();
        }

        /// <summary>
        /// Gets all tags associated with a specific course.
        /// </summary>
        public async Task<List<Tag>> GetCourseTagsAsync(int courseId)
        {
            return await courseServiceProxy.GetTagsForCourse(courseId);
        }

        /// <summary>
        /// Opens a module for the user if not already opened.
        /// </summary>
        public async Task OpenModuleAsync(int userId, int moduleId)
        {
            if (!await courseServiceProxy.IsModuleOpen(userId, moduleId))
            {
                await courseServiceProxy.OpenModule(userId, moduleId);
            }
        }

        /// <summary>
        /// Retrieves all modules for a specific course.
        /// </summary>
        public async Task<List<Module>> GetModulesAsync(int courseId)
        {
            try
            {
                return await courseServiceProxy.GetModulesByCourseId(courseId);
            }
            catch
            {
                return new List<Module>();
            }
        }

        /// <summary>
        /// Retrieves all non-bonus modules for a course.
        /// </summary>
        public async Task<List<Module>> GetNormalModulesAsync(int courseId)
        {
            var modules = await courseServiceProxy.GetModulesByCourseId(courseId);
            return modules.Where(m => !m.IsBonus).ToList();
        }

        /// <summary>
        /// Enrolls the user in a course if not already enrolled.
        /// </summary>
        public async Task<bool> EnrollInCourseAsync(int userId, int courseId)
        {
            if (await courseServiceProxy.IsUserEnrolled(userId, courseId))
            {
                return false;
            }

            await courseServiceProxy.EnrollUser(userId, courseId);
            return true;
        }

        /// <summary>
        /// Completes a module and marks course as completed if all modules are done.
        /// </summary>
        public async Task CompleteModuleAsync(int userId, int moduleId, int courseId)
        {
                await courseServiceProxy.CompleteModule(userId, moduleId);

                if (await courseServiceProxy.IsCourseCompleted(userId, courseId))
                {
                    await courseServiceProxy.MarkCourseAsCompleted(userId, courseId);
                }
        }

        /// <summary>
        /// Returns whether the user is enrolled in the specified course.
        /// </summary>
        public async Task<bool> IsUserEnrolledAsync(int userId, int courseId)
        {
                return await courseServiceProxy.IsUserEnrolled(userId, courseId);
        }

        /// <summary>
        /// Returns whether the user has completed the specified module.
        /// </summary>
        public async Task<bool> IsModuleCompletedAsync(int userId, int moduleId)
        {
                return await courseServiceProxy.IsModuleCompleted(userId, moduleId);
        }

        /// <summary>
        /// Filters courses based on search text, type, enrollment status, and tags.
        /// </summary>
        public async Task<List<Course>> GetFilteredCoursesAsync(string searchText, bool filterPremium, bool filterFree, bool filterEnrolled, bool filterNotEnrolled, List<int> selectedTagIds, int userId)
        {
                var courses = await courseServiceProxy.GetAllCourses();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    courses = courses.Where(c => c.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (filterPremium && filterFree)
                {
                    courses = new List<Course>();
                }
                else if (filterPremium)
                {
                    courses = courses.Where(c => c.IsPremium).ToList();
                }
                else if (filterFree)
                {
                    courses = courses.Where(c => !c.IsPremium).ToList();
                }

                if (filterEnrolled && filterNotEnrolled)
                {
                    courses = new List<Course>();
                }
                else if (filterEnrolled)
                {
                    courses = (await Task.WhenAll(courses.Select(async c => new { c, enrolled = await courseServiceProxy.IsUserEnrolled(userId, c.CourseId) })))
                        .Where(x => x.enrolled)
                        .Select(x => x.c)
                        .ToList();
                }
                else if (filterNotEnrolled)
                {
                    courses = (await Task.WhenAll(courses.Select(async c => new { c, enrolled = await courseServiceProxy.IsUserEnrolled(userId, c.CourseId) })))
                        .Where(x => !x.enrolled)
                        .Select(x => x.c)
                        .ToList();
                }

                if (selectedTagIds.Count > 0)
                {
                    courses = (await Task.WhenAll(courses.Select(async c =>
                    {
                        var tags = await courseServiceProxy.GetTagsForCourse(c.CourseId);
                        return new { c, tags };
                    })))
                    .Where(x => selectedTagIds.All(id => x.tags.Select(t => t.TagId).Contains(id)))
                    .Select(x => x.c)
                    .ToList();
                }

                return courses;
        }

        /// <summary>
        /// Updates the time the user has spent on a course.
        /// </summary>
        public async Task UpdateTimeSpentAsync(int userId, int courseId, int seconds)
        {
                await courseServiceProxy.UpdateTimeSpent(userId, courseId, seconds);
        }

        /// <summary>
        /// Retrieves the time the user has spent on a course.
        /// </summary>
        public async Task<int> GetTimeSpentAsync(int userId, int courseId)
        {
                return await courseServiceProxy.GetTimeSpent(userId, courseId);
        }

        /// <summary>
        /// Handles user interaction with module images.
        /// </summary>
        public async Task<bool> ClickModuleImageAsync(int userId, int moduleId)
        {
                if (await courseServiceProxy.IsModuleImageClicked(userId, moduleId))
                {
                    return false;
                }

                await courseServiceProxy.ClickModuleImage(userId, moduleId);
                return true;
        }

        /// <summary>
        /// Checks if a module is in progress.
        /// </summary>
        public async Task<bool> IsModuleInProgressAsync(int userId, int moduleId)
        {
                return await courseServiceProxy.IsModuleOpen(userId, moduleId);
        }

        /// <summary>
        /// Checks if a module is available for the user.
        /// </summary>
        public async Task<bool> IsModuleAvailableAsync(int userId, int moduleId)
        {
                return await courseServiceProxy.IsModuleAvailable(userId, moduleId);
        }

        /// <summary>
        /// Checks if a course has been completed by the user.
        /// </summary>
        public async Task<bool> IsCourseCompletedAsync(int userId, int courseId)
        {
                return await courseServiceProxy.IsCourseCompleted(userId, courseId);
        }

        /// <summary>
        /// Gets the number of completed modules in a course.
        /// </summary>
        public async Task<int> GetCompletedModulesCountAsync(int userId, int courseId)
        {
                return await courseServiceProxy.GetCompletedModulesCount(userId, courseId);
        }

        /// <summary>
        /// Gets the number of required modules for a course.
        /// </summary>
        public async Task<int> GetRequiredModulesCountAsync(int courseId)
        {
                return await courseServiceProxy.GetRequiredModulesCount(courseId);
        }

        /// <summary>
        /// Claims the course completion reward if eligible.
        /// </summary>
        public async Task<bool> ClaimCompletionRewardAsync(int userId, int courseId)
        {
                return await courseServiceProxy.ClaimCompletionReward(userId, courseId);
        }

        /// <summary>
        /// Claims a reward if the course was completed within a time limit.
        /// </summary>
        public async Task<bool> ClaimTimedRewardAsync(int userId, int courseId, int timeSpent)
        {
                return await courseServiceProxy.ClaimTimedReward(userId, courseId, timeSpent);
        }

        /// <summary>
        /// Retrieves the time limit for completing a course.
        /// </summary>
        public async Task<int> GetCourseTimeLimitAsync(int courseId)
        {
                return await courseServiceProxy.GetCourseTimeLimit(courseId);
        }

        public async Task<bool> BuyBonusModuleAsync(int userId, int moduleId, int courseId)
        {
                return await courseServiceProxy.BuyBonusModule(userId, moduleId, courseId);
        }
    }
}
