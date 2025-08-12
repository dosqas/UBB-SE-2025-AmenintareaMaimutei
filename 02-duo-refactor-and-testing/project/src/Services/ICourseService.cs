using System;
using System.Collections.Generic;
using CourseApp.Models;

namespace CourseApp.Services
{
    /// <summary>
    /// Interface for course-related operations
    /// </summary>
    public interface ICourseService
    {
        // Course operations
        List<Course> GetCourses();
        List<Course> GetFilteredCourses(string searchText, bool filterPremium, bool filterFree,
                                      bool filterEnrolled, bool filterNotEnrolled, List<int> selectedTagIds);

        // Module operations
        List<Module> GetModules(int courseId);
        List<Module> GetNormalModules(int courseId);
        void OpenModule(int moduleId);
        void CompleteModule(int moduleId, int courseId);
        bool IsModuleAvailable(int moduleId);
        bool IsModuleCompleted(int moduleId);
        bool IsModuleInProgress(int moduleId);
        bool ClickModuleImage(int moduleId);
        bool BuyBonusModule(int moduleId, int courseId);

        // Enrollment operations
        bool IsUserEnrolled(int courseId);
        bool EnrollInCourse(int courseId);

        // Progress tracking
        void UpdateTimeSpent(int courseId, int seconds);
        int GetTimeSpent(int courseId);

        // Completion tracking
        bool IsCourseCompleted(int courseId);
        int GetCompletedModulesCount(int courseId);
        int GetRequiredModulesCount(int courseId);

        // Reward operations
        bool ClaimCompletionReward(int courseId);
        bool ClaimTimedReward(int courseId, int timeSpent);
        int GetCourseTimeLimit(int courseId);

        // Tag operations
        List<Tag> GetTags();
        List<Tag> GetCourseTags(int courseId);
    }
}