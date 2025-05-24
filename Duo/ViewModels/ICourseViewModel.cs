using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DuoClassLibrary.Models;
using static Duo.ViewModels.CourseViewModel;

namespace Duo.ViewModels
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
        /// Gets or sets a value indicating whether the user is enrolled in the current course.
        /// </summary>
        bool IsEnrolled { get; set; }

        /// <summary>
        /// Gets whether coin information should be visible (e.g., for premium courses).
        /// </summary>
        bool CoinVisibility { get; }

        /// <summary>
        /// Gets or sets the current coin balance of the user.
        /// </summary>
        int CoinBalance { get; set; }

        /// <summary>
        /// Gets the tags associated with the current course.
        /// </summary>
        ObservableCollection<Tag> Tags { get; }

        /// <summary>
        /// Gets the formatted string representing the time remaining to complete the course (e.g., "X min Y sec").
        /// </summary>
        string FormattedTimeRemaining { get; }

        /// <summary>
        /// Gets or sets the notification message to display to the user.
        /// </summary>
        string NotificationMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the notification should be visible.
        /// </summary>
        bool ShowNotification { get; set; }

        /// <summary>
        /// Gets the number of modules that have been completed for the current course.
        /// </summary>
        int CompletedModules { get; }

        /// <summary>
        /// Gets the number of modules required to complete the course.
        /// </summary>
        int RequiredModules { get; }

        /// <summary>
        /// Gets a value indicating whether the course has been completed.
        /// </summary>
        bool IsCourseCompleted { get; }

        /// <summary>
        /// Gets the total time limit for completing the course, in seconds.
        /// </summary>
        int TimeLimit { get; }

        /// <summary>
        /// Gets the remaining time to complete the course, in seconds.
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
        /// Retrieves and updates the current coin balance asynchronously.
        /// </summary>
        Task<int> GetCoinBalanceAsync(int currentUserId);

        /// <summary>
        /// Starts the timer that tracks course progress.
        /// </summary>
        void StartCourseProgressTimer();

        /// <summary>
        /// Pauses the course progress timer and saves the current progress asynchronously.
        /// </summary>
        Task PauseCourseProgressTimer(int currentUserId);

        /// <summary>
        /// Refreshes the module roadmap by reloading module statuses asynchronously.
        /// </summary>
        Task RefreshCourseModulesDisplay(int currentUserId);

        /// <summary
        /// >Marks a module as completed and checks for rewards asynchronously.
        /// </summary>
        /// <param name="targetModuleId">The ID of the module to mark as completed.</param>
        Task MarkModuleAsCompletedAndCheckRewards(int targetModuleId, int currentUserId);

        /// <summary>
        /// Attempts to purchase a bonus module asynchronously.
        /// </summary>
        /// <param name="module">The module to purchase.</param>
        Task AttemptBonusModulePurchaseAsync(Module? module, int currentUserId);

        /// <summary>
        /// Loads and organizes all modules for the course asynchronously.
        /// </summary>
        Task LoadAndOrganizeCourseModules(int currentUserId);

        #endregion
    }
}
