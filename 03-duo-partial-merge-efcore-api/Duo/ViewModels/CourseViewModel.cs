using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Input;
using Duo.Models;
using Duo.Services;
using Duo.ViewModels.Helpers;
using Duo.Commands;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

namespace Duo.ViewModels
{
    /// <summary>
    /// ViewModel for handling course presentation, progress tracking, and user interactions
    /// </summary>
    public partial class CourseViewModel : BaseViewModel, ICourseViewModel
    {
        #region Constants

        /// <summary>Duration for which notifications are displayed (in seconds)</summary>
        internal const int NotificationDisplayDurationInSeconds = 3;

        /// <summary>Coin reward for completing all required modules</summary>
        private const int CourseCompletionRewardCoins = 50;

        /// <summary>Coin reward for completing the course within time limit</summary>
        private const int TimedCompletionRewardCoins = 300;

        /// <summary>Adjustment factor for time tracking to prevent double counting</summary>
        private const int TimeTrackingDatabaseAdjustmentDivisor = 2;

        /// <summary>Number of minutes in one hour</summary>
        private const int MinutesInAnHour = 60;
        #endregion

        #region Fields
        private IDispatcherTimerService? courseProgressTimer;
        private int totalSecondsSpentOnCourse;
        private int courseCompletionTimeLimitInSeconds;
        private string? formattedTimeRemaining;
        internal bool IsCourseTimerRunning;
        private int lastSavedTimeInSeconds = 0;

        private readonly ICourseService courseService;
        private readonly ICoinsService coinsService;
        private INotificationHelper? notificationHelper;

        private string notificationMessageText = string.Empty;
        private bool shouldShowNotification = false;
        #endregion

        #region Properties

        /// <summary>Gets the current course being viewed</summary>
        public Course CurrentCourse { get; }

        /// <summary>Gets the collection of modules with their progress status</summary>
        public ObservableCollection<ModuleProgressStatus> ModuleRoadmap { get; } = [];

        /// <summary>Gets the command for enrolling in the course</summary>
        public ICommand? EnrollCommand { get; private set; }

        /// <summary>Gets a value indicating whether the user is enrolled in this course</summary>
        public bool IsEnrolled { get; set; }

        /// <summary>Gets whether coin information should be visible</summary>
        public bool CoinVisibility => CurrentCourse.IsPremium && !IsEnrolled;

        /// <summary>Gets the current coin balance of the user</summary>
        public int CoinBalance => coinsService.GetCoinBalance(0);

        /// <summary>Gets the tags associated with this course</summary>
        public ObservableCollection<Tag> Tags => [.. courseService.GetCourseTags(CurrentCourse.CourseId)];

        /// <summary>
        /// Gets or sets the formatted string representing time remaining in course
        /// Format: "X min Y sec"
        /// </summary>
        public string FormattedTimeRemaining
        {
            get => formattedTimeRemaining!;
            private set
            {
                formattedTimeRemaining = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the notification message to display</summary>
        public virtual string NotificationMessage
        {
            get => notificationMessageText;
            set
            {
                notificationMessageText = value;
                OnPropertyChanged(nameof(NotificationMessage));
            }
        }

        /// <summary>Gets or sets whether notification should be visible</summary>
        public virtual bool ShowNotification
        {
            get => shouldShowNotification;
            set
            {
                shouldShowNotification = value;
                OnPropertyChanged(nameof(ShowNotification));
            }
        }

        /// <summary>Gets the number of completed modules</summary>
        public int CompletedModules { get; private set; }

        /// <summary>Gets the number of required modules</summary>
        public int RequiredModules { get; private set; }

        /// <summary>Gets whether all required modules are completed</summary>
        public bool IsCourseCompleted => CompletedModules >= RequiredModules;

        /// <summary>Gets the total time limit for course completion (in seconds)</summary>
        public int TimeLimit { get; private set; }

        /// <summary>Gets the remaining time to complete the course (in seconds)</summary>
        public int TimeRemaining => Math.Max(0, TimeLimit - totalSecondsSpentOnCourse);

        /// <summary>Gets whether completion reward was claimed</summary>
        public bool CompletionRewardClaimed { get; private set; }

        /// <summary>Gets whether timed completion reward was claimed</summary>
        public bool TimedRewardClaimed { get; private set; }
        #endregion

        #region Nested Classes

        /// <summary>
        /// Represents a module along with its progress status
        /// </summary>
        public class ModuleProgressStatus
        {
            /// <summary>Gets or sets the module</summary>
            public Module? Module { get; set; }

            /// <summary>Gets or sets whether the module is unlocked</summary>
            public bool IsUnlocked { get; set; }

            /// <summary>Gets or sets whether the module is completed</summary>
            public bool IsCompleted { get; set; }
        }
        #endregion

        #region Constructor and Initialization

        /// <summary>
        /// Initializes a new instance of the CourseViewModel class
        /// </summary>
        public CourseViewModel()
        {
            CurrentCourse = new Course
            {
                Title = string.Empty,
                Description = string.Empty,
                ImageUrl = string.Empty,
                Difficulty = string.Empty
            };
            courseService = new CourseService();
            coinsService = new CoinsService();
            notificationHelper = null;
        }

        /// <summary>
        /// Initializes a new instance of the CourseViewModel class
        /// </summary>
        /// <param name="course">The course to display and manage</param>
        /// <param name="courseService">The service for course-related operations (optional)</param>
        /// <param name="coinsService">The service for coin-related operations (optional)</param>
        /// <param name="timerService">The timer service for course progress tracking (optional)</param>
        /// <param name="notificationTimerService">The timer service for notifications (optional)</param>
        /// <exception cref="ArgumentNullException">Thrown when course is null</exception>
        public CourseViewModel(Course course, ICourseService? courseService = null,
            ICoinsService? coinsService = null, IDispatcherTimerService? timerService = null,
            IDispatcherTimerService? notificationTimerService = null, INotificationHelper? notificationHelper = null)
        {
            CurrentCourse = course ?? throw new ArgumentNullException(nameof(course));
            this.courseService = courseService ?? new CourseService();
            this.coinsService = coinsService ?? new CoinsService();

            InitializeTimersAndNotificationHelper(timerService, notificationTimerService, notificationHelper);

            InitializeProperties();
            LoadInitialData();
        }

        /// <summary>
        /// Initializes the timers and notification helper for the course progress and notifications.
        /// If any of the parameters are null, default implementations are used.
        /// </summary>
        /// <param name="timerService">Optional dispatcher timer service for course progress tracking.</param>
        /// <param name="notificationTimerService">Optional dispatcher timer service for notifications.</param>
        /// <param name="notificationHelper">Optional notification helper instance.</param>
        [ExcludeFromCodeCoverage]
        private void InitializeTimersAndNotificationHelper(IDispatcherTimerService? timerService,
            IDispatcherTimerService? notificationTimerService, INotificationHelper? notificationHelper)
        {
            // Use separate timers for course progress and notifications
            courseProgressTimer = timerService ?? new DispatcherTimerService();
            var notificationTimer = notificationTimerService ?? new DispatcherTimerService();

            this.notificationHelper = notificationHelper ?? new NotificationHelper(this, notificationTimer);

            courseProgressTimer.Tick += OnCourseTimerTick;
        }

        /// <summary>
        /// Initializes key ViewModel properties such as enrollment status and enrollment command.
        /// </summary>
        private void InitializeProperties()
        {
            IsEnrolled = courseService.IsUserEnrolled(CurrentCourse.CourseId);
            EnrollCommand = new RelayCommand(EnrollUserInCourse, CanUserEnrollInCourse);
        }

        /// <summary>
        /// Loads the initial course-related data such as time spent, modules completed,
        /// time remaining, and initializes the course module structure.
        /// </summary>
        private void LoadInitialData()
        {
            totalSecondsSpentOnCourse = courseService.GetTimeSpent(CurrentCourse.CourseId);
            lastSavedTimeInSeconds = totalSecondsSpentOnCourse;
            courseCompletionTimeLimitInSeconds = CurrentCourse.TimeToComplete - totalSecondsSpentOnCourse;
            FormattedTimeRemaining = FormatTimeRemainingDisplay(courseCompletionTimeLimitInSeconds - totalSecondsSpentOnCourse);

            CompletedModules = courseService.GetCompletedModulesCount(CurrentCourse.CourseId);
            RequiredModules = courseService.GetRequiredModulesCount(CurrentCourse.CourseId);
            TimeLimit = courseService.GetCourseTimeLimit(CurrentCourse.CourseId);

            LoadAndOrganizeCourseModules();
        }
        #endregion

        #region Timer Methods

        /// <summary>
        /// Handles the Tick event of the course progress timer, updating the total time spent
        /// and refreshing the time display.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains no event data.</param>
        private void OnCourseTimerTick(object? sender, EventArgs e)
        {
            totalSecondsSpentOnCourse++;
            UpdateTimeDisplay();
            OnPropertyChanged(nameof(TimeRemaining));
        }

        /// <summary>
        /// Updates the formatted time display
        /// </summary>
        private void UpdateTimeDisplay()
        {
            int remainingSeconds = courseCompletionTimeLimitInSeconds - totalSecondsSpentOnCourse;
            FormattedTimeRemaining = FormatTimeRemainingDisplay(Math.Max(0, remainingSeconds));
        }
        #endregion

        #region Module Management

        /// <summary>
        /// Loads and organizes all modules for the current course with their progress status
        /// </summary>
        public void LoadAndOrganizeCourseModules()
        {
            var modules = courseService.GetModules(CurrentCourse.CourseId)
                           .OrderBy(module => module.Position)
                           .ToList();

            ModuleRoadmap.Clear();

            for (int index = 0; index < modules.Count; index++)
            {
                var module = modules[index];
                bool isCompleted = courseService.IsModuleCompleted(module.ModuleId);
                bool isUnlocked = GetModuleUnlockStatus(module, index);

                ModuleRoadmap.Add(new ModuleProgressStatus
                {
                    Module = module,
                    IsUnlocked = isUnlocked,
                    IsCompleted = isCompleted
                });
            }

            OnPropertyChanged(nameof(ModuleRoadmap));
        }

        /// <summary>
        /// Determines if a module should be unlocked based on its position and progress
        /// </summary>
        /// <param name="module">The module to check</param>
        /// <param name="moduleIndex">The index of the module in the collection</param>
        /// <returns>True if the module should be unlocked, otherwise false</returns>
        private bool GetModuleUnlockStatus(Module module, int moduleIndex)
        {
            if (!module.IsBonus)
            {
                return IsEnrolled &&
                      (moduleIndex == 0 ||
                       courseService.IsModuleCompleted(ModuleRoadmap[moduleIndex - 1].Module!.ModuleId));
            }
            return courseService.IsModuleInProgress(module.ModuleId);
        }

        /// <summary>
        /// Determines if the user can enroll in the course
        /// </summary>
        private bool CanUserEnrollInCourse(object? parameter)
        {
            return !IsEnrolled && CoinBalance >= CurrentCourse.Cost;
        }

        /// <summary>
        /// Enrolls the user in the current course
        /// </summary>
        private void EnrollUserInCourse(object? parameter)
        {
            if (!coinsService.TrySpendingCoins(0, CurrentCourse.Cost))
            {
                return;
            }

            if (!courseService.EnrollInCourse(CurrentCourse.CourseId))
            {
                return;
            }

            IsEnrolled = true;
            ResetCourseProgressTracking();
            OnPropertyChanged(nameof(IsEnrolled));
            OnPropertyChanged(nameof(CoinBalance));
            StartCourseProgressTimer();
            LoadAndOrganizeCourseModules();
        }

        /// <summary>
        /// Resets all course progress tracking metrics
        /// </summary>
        private void ResetCourseProgressTracking()
        {
            totalSecondsSpentOnCourse = 0;
            FormattedTimeRemaining = FormatTimeRemainingDisplay(totalSecondsSpentOnCourse);
        }
        #endregion

        #region Timer Control Methods

        /// <summary>
        /// Starts the course progress timer if not already running
        /// </summary>
        public void StartCourseProgressTimer()
        {
            if (!IsCourseTimerRunning && IsEnrolled)
            {
                IsCourseTimerRunning = true;
                courseProgressTimer!.Start();
            }
        }

        /// <summary>
        /// Pauses the course progress timer and saves the current progress
        /// </summary>
        public void PauseCourseProgressTimer()
        {
            if (IsCourseTimerRunning)
            {
                courseProgressTimer!.Stop();
                SaveCourseProgressTime();
                IsCourseTimerRunning = false;
            }
        }

        /// <summary>
        /// Saves the current course progress time to the database
        /// </summary>
        private void SaveCourseProgressTime()
        {
            int secondsToSave = (totalSecondsSpentOnCourse - lastSavedTimeInSeconds) /
                               TimeTrackingDatabaseAdjustmentDivisor;

            Console.WriteLine($"Attempting to save: Current={totalSecondsSpentOnCourse}, " +
                             $"LastSaved={lastSavedTimeInSeconds}, ToSave={secondsToSave}");

            if (secondsToSave > 0)
            {
                Console.WriteLine($"Saving {secondsToSave} seconds");
                courseService.UpdateTimeSpent(CurrentCourse.CourseId, secondsToSave);
                lastSavedTimeInSeconds = totalSecondsSpentOnCourse;
            }
        }
        #endregion

        #region Utility Methods

        /// <summary>
        /// Formats time in seconds to a display string (X min Y sec)
        /// </summary>
        /// <param name="totalSeconds">Total seconds to format</param>
        /// <returns>Formatted time string</returns>
        internal static string FormatTimeRemainingDisplay(int totalSeconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
            int totalMinutes = timeSpan.Minutes + (timeSpan.Hours * MinutesInAnHour);
            return $"{totalMinutes} min {timeSpan.Seconds} sec";
        }

        /// <summary>
        /// Refreshes the course modules display
        /// </summary>
        public void RefreshCourseModulesDisplay()
        {
            LoadAndOrganizeCourseModules();
        }
        #endregion

        #region Reward Handling

        /// <summary>
        /// Marks a module as completed and checks for any earned rewards
        /// </summary>
        /// <param name="targetModuleId">ID of the module to mark as completed</param>
        public void MarkModuleAsCompletedAndCheckRewards(int targetModuleId)
        {
            courseService.CompleteModule(targetModuleId, CurrentCourse.CourseId);
            UpdateCompletionStatus();

            if (IsCourseCompleted)
            {
                CheckForCompletionReward();
                CheckForTimedReward();
            }
        }

        /// <summary>
        /// Updates the module completion status properties
        /// </summary>
        private void UpdateCompletionStatus()
        {
            CompletedModules = courseService.GetCompletedModulesCount(CurrentCourse.CourseId);
            OnPropertyChanged(nameof(CompletedModules));
            OnPropertyChanged(nameof(IsCourseCompleted));
        }

        /// <summary>
        /// Checks and claims the course completion reward if eligible
        /// </summary>
        private void CheckForCompletionReward()
        {
            bool rewardClaimed = courseService.ClaimCompletionReward(CurrentCourse.CourseId);
            if (rewardClaimed)
            {
                CompletionRewardClaimed = true;
                OnPropertyChanged(nameof(CompletionRewardClaimed));
                OnPropertyChanged(nameof(CoinBalance));
                ShowCourseCompletionRewardNotification();
            }
        }

        /// <summary>
        /// Checks and claims the timed completion reward if eligible
        /// </summary>
        private void CheckForTimedReward()
        {
            if (TimeRemaining > 0)
            {
                bool rewardClaimed = courseService.ClaimTimedReward(CurrentCourse.CourseId, totalSecondsSpentOnCourse);
                if (rewardClaimed)
                {
                    TimedRewardClaimed = true;
                    OnPropertyChanged(nameof(TimedRewardClaimed));
                    OnPropertyChanged(nameof(CoinBalance));
                    ShowTimedCompletionRewardNotification();
                }
            }
        }
        #endregion

        #region Notification Methods

        /// <summary>
        /// Shows notification for course completion reward
        /// </summary>
        private void ShowCourseCompletionRewardNotification()
        {
            string message = $"Congratulations! You have completed all required modules in this course. {CourseCompletionRewardCoins} coins have been added to your balance.";
            notificationHelper!.ShowTemporaryNotification(message);
        }

        /// <summary>
        /// Shows notification for timed completion reward
        /// </summary>
        private void ShowTimedCompletionRewardNotification()
        {
            string message = $"Congratulations! You completed the course within the time limit. {TimedCompletionRewardCoins} coins have been added to your balance.";
            notificationHelper!.ShowTemporaryNotification(message);
        }

        /// <summary>
        /// Shows notification for successful module purchase
        /// </summary>
        /// <param name="module">The module that was purchased</param>
        private void ShowModulePurchaseNotification(Module module)
        {
            string message = $"Congratulations! You have purchased bonus module {module.Title}, {module.Cost} coins have been deducted from your balance.";
            notificationHelper!.ShowTemporaryNotification(message);
            RefreshCourseModulesDisplay();
        }
        #endregion

        #region Module Purchase

        /// <summary>
        /// Attempts to purchase a bonus module
        /// </summary>
        /// <param name="module">The module to purchase</param>
        public void AttemptBonusModulePurchase(Module? module)
        {
            ArgumentNullException.ThrowIfNull(module);

            if (courseService.IsModuleCompleted(module.ModuleId))
            {
                return;
            }

            bool purchaseSuccessful = courseService.BuyBonusModule(module.ModuleId, CurrentCourse.CourseId);

            if (purchaseSuccessful)
            {
                UpdatePurchasedModuleStatus(module);
                ShowModulePurchaseNotification(module);
                OnPropertyChanged(nameof(ModuleRoadmap));
                OnPropertyChanged(nameof(CoinBalance));
            }
            else
            {
                ShowPurchaseFailedNotification();
            }
        }

        /// <summary>
        /// Updates the status of a purchased module
        /// </summary>
        /// <param name="module">The module that was purchased</param>
        private void UpdatePurchasedModuleStatus(Module module)
        {
            var moduleToUpdate = ModuleRoadmap.FirstOrDefault(m => m.Module!.ModuleId == module.ModuleId);
            if (moduleToUpdate != null)
            {
                moduleToUpdate.IsUnlocked = true;
                moduleToUpdate.IsCompleted = false;
                courseService.OpenModule(module.ModuleId);
            }
        }

        /// <summary>
        /// Shows notification for failed module purchase
        /// </summary>
        private void ShowPurchaseFailedNotification()
        {
            notificationHelper!.ShowTemporaryNotification("You do not have enough coins to buy this module.");
        }
        #endregion
    }
}
