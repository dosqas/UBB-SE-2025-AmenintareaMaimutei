using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuoClassLibrary.Models;

namespace DuoClassLibrary.Services.Interfaces
{
    public interface ICourseServiceProxy
    {
        Task<List<Course>> GetAllCourses();
        Task<Course> GetCourse(int courseId);
        Task<List<Tag>> GetAllTags();
        Task<List<Tag>> GetTagsForCourse(int courseId);
        Task OpenModule(int userId, int moduleId);
        Task<List<Module>> GetModulesByCourseId(int courseId);
        Task<Module> GetModule(int moduleId);
        Task<bool> IsModuleOpen(int userId, int moduleId);
        Task EnrollUser(int userId, int courseId);
        Task<bool> IsUserEnrolled(int userId, int courseId);
        Task CompleteModule(int userId, int moduleId);
        Task<bool> IsCourseCompleted(int userId, int courseId);
        Task MarkCourseAsCompleted(int userId, int courseId);
        Task UpdateTimeSpent(int userId, int courseId, int seconds);
        Task<int> GetTimeSpent(int userId, int courseId);
        Task ClickModuleImage(int userId, int moduleId);
        Task<bool> IsModuleImageClicked(int userId, int moduleId);
        Task<bool> IsModuleAvailable(int userId, int moduleId);
        Task<bool> IsModuleCompleted(int userId, int moduleId);
        Task<int> GetCompletedModulesCount(int userId, int courseId);
        Task<int> GetRequiredModulesCount(int courseId);
        Task<bool> ClaimCompletionReward(int userId, int courseId, int coins);
        Task<bool> ClaimTimedReward(int userId, int courseId, int timeSpent, int coins);
        Task<int> GetCourseTimeLimit(int courseId);
        Task<bool> BuyBonusModule(int userId, int moduleId);
    }
}