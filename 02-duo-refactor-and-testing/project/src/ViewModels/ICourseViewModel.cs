using System.Collections.ObjectModel;
using System.Windows.Input;
using CourseApp.Models;
using static CourseApp.ViewModels.CourseViewModel;

namespace CourseApp.ViewModels
{
    /// <summary>
    /// Interface defining the view model for course management in the application.
    /// This interface is responsible for exposing course-related data, user interaction commands, and course progress tracking.
    /// </summary>
    public interface ICourseViewModel : IBaseViewModel
    {
        /// <summary>
        /// Gets the current course being viewed.
        /// </summary>
        Course CurrentCourse { get; }

        /// <summary>
        /// Gets the collection of modules and their progress status for the current course.
        /// </summary>
        ObservableCollection<ModuleProgressStatus> ModuleRoadmap { get; }

        /// <summary>
        /// Gets the command for enrolling in the current course.
        /// </summary>
        ICommand? EnrollCommand { get; }

        /// <summary>
        /// Gets a value indicating whether the user is enrolled in the current course.
        /// </summary>
        bool IsEnrolled { get; }

        /// <summary>
        /// Gets whether the coin visibility is enabled (e.g., for premium courses).
        /// </summary>
        bool CoinVisibility { get; }

        /// <summary>
        /// Gets the current coin balance of the user.
        /// </summary>
        int CoinBalance { get; }

        /// <summary>
        /// Gets the tags associated with the current course.
        /// </summary>
        ObservableCollection<Tag> Tags { get; }

        /// <summary>
        /// Gets the formatted string representing the time remaining to complete the course.
        /// The format is typically "X min Y sec".
        /// </summary>
        string FormattedTimeRemaining { get; }

        /// <summary>
        /// Gets or sets the notification message that should be displayed to the user.
        /// </summary>
        string NotificationMessage { get; }

        /// <summary>
        /// Gets or sets whether the notification message should be shown.
        /// </summary>
        bool ShowNotification { get; }

        /// <summary>
        /// Gets the number of modules that have been completed for the current course.
        /// </summary>
        int CompletedModules { get; }

        /// <summary>
        /// Gets the number of modules that are required for completing the current course.
        /// </summary>
        int RequiredModules { get; }

        /// <summary>
        /// Gets a value indicating whether the course is completed based on module progress.
        /// </summary>
        bool IsCourseCompleted { get; }

        /// <summary>
        /// Gets the total time limit for completing the course, in seconds.
        /// </summary>
        int TimeLimit { get; }

        /// <summary>
        /// Gets the time remaining to complete the course, in seconds.
        /// </summary>
        int TimeRemaining { get; }

        /// <summary>
        /// Gets a value indicating whether the completion reward has been claimed.
        /// </summary>
        bool CompletionRewardClaimed { get; }

        /// <summary>
        /// Gets a value indicating whether the timed completion reward has been claimed.
        /// </summary>
        bool TimedRewardClaimed { get; }

        #region Methods

        /// <summary>
        /// Starts the timer for tracking course progress.
        /// </summary>
        void StartCourseProgressTimer();

        /// <summary>
        /// Pauses the timer for tracking course progress.
        /// </summary>
        void PauseCourseProgressTimer();

        /// <summary>
        /// Refreshes the display of course modules based on current progress and status.
        /// </summary>
        void RefreshCourseModulesDisplay();

        /// <summary>
        /// Marks the specified module as completed and checks if rewards can be claimed.
        /// </summary>
        /// <param name="targetModuleId">The ID of the module to mark as completed.</param>
        void MarkModuleAsCompletedAndCheckRewards(int targetModuleId);

        /// <summary>
        /// Attempts to purchase a bonus module for the course.
        /// </summary>
        /// <param name="module">The module to purchase.</param>
        void AttemptBonusModulePurchase(Module module);

        /// <summary>
        /// Loads and organizes all the modules for the current course, updating their progress status.
        /// </summary>
        void LoadAndOrganizeCourseModules();

        #endregion
    }
}
