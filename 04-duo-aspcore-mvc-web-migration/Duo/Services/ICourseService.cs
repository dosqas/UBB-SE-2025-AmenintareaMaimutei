using System.Collections.Generic;
using System.Threading.Tasks;
using DuoClassLibrary.Models;

namespace Duo.Services
{
    /// <summary>
    /// Interface for course-related operations.
    /// </summary>
    public interface ICourseService
    {
        Task<List<Course>> GetCoursesAsync();
        Task<List<Course>> GetFilteredCoursesAsync(string searchText, bool filterPremium, bool filterFree,
                                      bool filterEnrolled, bool filterNotEnrolled, List<int> selectedTagIds, int userId);
        Task<List<Module>> GetModulesAsync(int courseId);
        Task<List<Module>> GetNormalModulesAsync(int courseId);
        Task OpenModuleAsync(int userId, int moduleId);
        Task CompleteModuleAsync(int userId, int moduleId, int courseId);
        Task<bool> IsModuleAvailableAsync(int userId, int moduleId);
        Task<bool> IsModuleCompletedAsync(int userId, int moduleId);
        Task<bool> IsModuleInProgressAsync(int userId, int moduleId);
        Task<bool> ClickModuleImageAsync(int userId, int moduleId);

        Task<bool> IsUserEnrolledAsync(int userId, int courseId);
        Task<bool> EnrollInCourseAsync(int userId, int courseId);

        Task UpdateTimeSpentAsync(int userId, int courseId, int seconds);
        Task<int> GetTimeSpentAsync(int userId, int courseId);

        Task<bool> IsCourseCompletedAsync(int userId, int courseId);
        Task<int> GetCompletedModulesCountAsync(int userId, int courseId);
        Task<int> GetRequiredModulesCountAsync(int courseId);

        Task<bool> ClaimCompletionRewardAsync(int userId, int courseId);
        Task<bool> ClaimTimedRewardAsync(int userId, int courseId, int timeSpent);
        Task<int> GetCourseTimeLimitAsync(int courseId);

        Task<List<Tag>> GetTagsAsync();
        Task<List<Tag>> GetCourseTagsAsync(int courseId);
        Task<bool> BuyBonusModuleAsync(int userId, int moduleId, int courseId);
    }
}
