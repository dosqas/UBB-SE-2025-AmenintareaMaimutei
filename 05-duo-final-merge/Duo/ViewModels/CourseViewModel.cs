using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Helpers.Interfaces;
using Duo.Helpers.Timers;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services;
using Duo.ViewModels.Helpers;
using Windows.System;
using DuoClassLibrary.Services.Interfaces;

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
        private IDispatcherTimerHelper? courseProgressTimer;
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
        private int currentUserId;

        #endregion

        #region Properties
        public int CurrentUserId { get => currentUserId; }


        /// <summary>Gets the current course being viewed</summary>
        public Course CurrentCourse { get; }

        /// <summary>Gets the collection of modules with their progress status</summary>
        public ObservableCollection<ModuleProgressStatus> ModuleRoadmap { get; } = [];

        /// <summary>Gets the command for enrolling in the course</summary>
        public ICommand? EnrollCommand { get; private set; }

        /// <summary>Gets a value indicating whether the user is enrolled in this course</summary>
        public bool IsEnrolled
        {
            get => isEnrolled;
            set
            {
                if (isEnrolled != value)
                {
                    isEnrolled = value;
                    OnPropertyChanged(nameof(IsEnrolled));

                    if (EnrollCommand is RelayCommand cmd)
                    {
                        cmd.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        private bool isEnrolled;

        /// <summary>Gets whether coin information should be visible</summary>
        public bool CoinVisibility => CurrentCourse.IsPremium && !IsEnrolled;

        /// <summary>Gets the current coin balance of the user</summary>
        private int coinBalance;
        public int CoinBalance
        {
            get => coinBalance;
            set
            {
                if (coinBalance != value)
                {
                    coinBalance = value;
                    OnPropertyChanged(nameof(CoinBalance));

                    if (EnrollCommand is RelayCommand cmd)
                    {
                        cmd.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        public async Task<int> GetCoinBalanceAsync(int currentUserId)
        {
            try
            {
                CoinBalance = await coinsService.GetCoinBalanceAsync(currentUserId);
                return CoinBalance;
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Failed to load coin balance", e.Message);
                return 0;
            }
        }

        private ObservableCollection<Tag> tags = new ();

        public ObservableCollection<Tag> Tags
        {
            get => tags;
            private set
            {
                tags = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadTagsAsync()
        {
            try
            {
                var tagList = await courseService.GetCourseTagsAsync(CurrentCourse.CourseId);
                Tags = new ObservableCollection<Tag>(tagList);
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Failed to load tags", e.Message);
            }
        }

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
        public bool IsCourseCompleted => CompletedModules == RequiredModules;

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

            public bool IsLockedBonus => Module?.IsBonus == true && !IsUnlocked;
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

            var httpClient = new System.Net.Http.HttpClient();
            var courseServiceProxy = new CourseServiceProxy(httpClient);

            courseService = new CourseService(courseServiceProxy);
            coinsService = new CoinsService(new CoinsServiceProxy(httpClient));

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
        public CourseViewModel(Course course, int currentUserId, ICourseService? courseService = null,
            ICoinsService? coinsService = null, IDispatcherTimerHelper? timerService = null,
            IDispatcherTimerHelper? notificationTimerService = null, INotificationHelper? notificationHelper = null,
            CourseServiceProxy? serviceProxy = null)
        {
            CurrentCourse = course ?? throw new ArgumentNullException(nameof(course));

            this.currentUserId = currentUserId;

            var httpClient = new System.Net.Http.HttpClient();
            var defaultServiceProxy = serviceProxy ?? new CourseServiceProxy(httpClient);

            this.courseService = courseService ?? new CourseService(defaultServiceProxy);
            this.coinsService = coinsService ?? new CoinsService(new CoinsServiceProxy(httpClient));

            EnrollCommand = new RelayCommand(
            async (_) => await EnrollUserInCourseAsync(_, currentUserId),
            (_) => !IsEnrolled && (CurrentCourse.Cost == 0 || CoinBalance >= CurrentCourse.Cost));

            InitializeTimersAndNotificationHelper(timerService, notificationTimerService, notificationHelper);
        }

        public async Task InitializeAsync(int currentUserId)
        {
            try
            {
                await InitializeProperties(currentUserId);
                await LoadInitialData(currentUserId);
                await LoadTagsAsync();
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize course data.\nDetails: {ex.Message}");
            }
        }

        private bool CanExecuteEnroll(object? parameter)
        {
            return !IsEnrolled;
        }

        private async void ExecuteEnroll(object? parameter)
        {
            await courseService.EnrollInCourseAsync(currentUserId, CurrentCourse.CourseId);
            IsEnrolled = true;
            await LoadAndOrganizeCourseModules(currentUserId);
        }

        /// <summary>
        /// Initializes the timers and notification helper for the course progress and notifications.
        /// If any of the parameters are null, default implementations are used.
        /// </summary>
        /// <param name="timerService">Optional dispatcher timer service for course progress tracking.</param>
        /// <param name="notificationTimerService">Optional dispatcher timer service for notifications.</param>
        /// <param name="notificationHelper">Optional notification helper instance.</param>
        [ExcludeFromCodeCoverage]
        private void InitializeTimersAndNotificationHelper(IDispatcherTimerHelper? timerService,
            IDispatcherTimerHelper? notificationTimerService, INotificationHelper? notificationHelper)
        {
            courseProgressTimer = timerService ?? new DispatcherTimerHelper();
            var notificationTimer = notificationTimerService ?? new DispatcherTimerHelper();

            this.notificationHelper = notificationHelper ?? new NotificationHelper(this, notificationTimer);

            courseProgressTimer.Tick += OnCourseTimerTick;
        }

        /// <summary>
        /// Initializes key ViewModel properties such as enrollment status and enrollment command.
        /// </summary>
        private async Task InitializeProperties(int currentUserId)
        {
            try
            {
                IsEnrolled = await courseService.IsUserEnrolledAsync(currentUserId, CurrentCourse.CourseId);
                CoinBalance = await coinsService.GetCoinBalanceAsync(currentUserId);

                EnrollCommand = new RelayCommand(
            async (_) => await EnrollUserInCourseAsync(_, currentUserId),
            (_) => !IsEnrolled && (CurrentCourse.Cost == 0 || CoinBalance >= CurrentCourse.Cost));

                OnPropertyChanged(nameof(EnrollCommand));
                OnPropertyChanged(nameof(IsEnrolled));
                OnPropertyChanged(nameof(CoinBalance));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to check enrollment.\nDetails: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the initial course-related data such as time spent, modules completed,
        /// time remaining, and initializes the course module structure.
        /// </summary>
        private async Task LoadInitialData(int currentUserId)
        {
            try
            {
                totalSecondsSpentOnCourse = await courseService.GetTimeSpentAsync(currentUserId, CurrentCourse.CourseId);
                lastSavedTimeInSeconds = totalSecondsSpentOnCourse;
                courseCompletionTimeLimitInSeconds = CurrentCourse.TimeToComplete - totalSecondsSpentOnCourse;
                FormattedTimeRemaining = FormatTimeRemainingDisplay(courseCompletionTimeLimitInSeconds - totalSecondsSpentOnCourse);

                CompletedModules = await courseService.GetCompletedModulesCountAsync(currentUserId, CurrentCourse.CourseId);
                RequiredModules = await courseService.GetRequiredModulesCountAsync(CurrentCourse.CourseId);
                TimeLimit = await courseService.GetCourseTimeLimitAsync(CurrentCourse.CourseId);

                await LoadAndOrganizeCourseModules(currentUserId);
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Course Load Error", $"Unable to load course data.\nDetails: {ex.Message}");
            }
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
            try
            {
                totalSecondsSpentOnCourse++;
                UpdateTimeDisplay();
                OnPropertyChanged(nameof(TimeRemaining));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Timer Error", $"An error occurred while updating the timer.\nDetails: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the formatted time display
        /// </summary>
        private void UpdateTimeDisplay()
        {
            try
            {
                int remainingSeconds = courseCompletionTimeLimitInSeconds - totalSecondsSpentOnCourse;
                FormattedTimeRemaining = FormatTimeRemainingDisplay(Math.Max(0, remainingSeconds));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Display Update Error", $"Failed to update remaining time.\nDetails: {ex.Message}");
            }
        }

        #endregion

        #region Module Management

        /// <summary>
        /// Loads and organizes all modules for the current course with their progress status
        /// </summary>
        public async Task LoadAndOrganizeCourseModules(int currentUserId)
        {
            try
            {
                var modules = await courseService.GetModulesAsync(CurrentCourse.CourseId);
                if (modules == null || modules.Count == 0)
                {
                    Console.WriteLine("No modules found, skipping module display.");
                    return;
                }

                ModuleRoadmap.Clear();

                for (int index = 0; index < modules.Count; index++)
                {
                    var module = modules[index];

                    bool isCompleted = false;
                    bool isUnlocked = false;

                    try
                    {
                        isCompleted = await courseService.IsModuleCompletedAsync(currentUserId, module.ModuleId);
                        isUnlocked = await GetModuleUnlockStatus(module, index, currentUserId);
                    }
                    catch (KeyNotFoundException kex)
                    {
                        Console.WriteLine($"Module {module.ModuleId} failed: {kex.Message}");
                        continue;
                    }

                    ModuleRoadmap.Add(new ModuleProgressStatus
                    {
                        Module = module,
                        IsUnlocked = isUnlocked,
                        IsCompleted = isCompleted
                    });
                }

                OnPropertyChanged(nameof(ModuleRoadmap));
            }
            catch (Exception e)
            {
                Console.WriteLine($"LoadAndOrganizeCourseModules crashed: {e.Message}");
                RaiseErrorMessage("Loading Modules Failed", $"Could not load modules.\nDetails: {e.Message}");
            }
        }

        /// <summary>
        /// Determines if a module should be unlocked based on its position and progress
        /// </summary>
        /// <param name="module">The module to check</param>
        /// <param name="moduleIndex">The index of the module in the collection</param>
        /// <returns>True if the module should be unlocked, otherwise false</returns>
        private async Task<bool> GetModuleUnlockStatus(Module module, int moduleIndex, int currentUserId)
        {
            try
            {
                if (!module.IsBonus)
                {
                    return IsEnrolled &&
                           (moduleIndex == 0 ||
                            await courseService.IsModuleCompletedAsync(currentUserId, ModuleRoadmap[moduleIndex - 1].Module!.ModuleId));
                }
                return await courseService.IsModuleInProgressAsync(currentUserId, module.ModuleId);
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Module Unlock Error", $"Failed to check unlock status.\nDetails: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Determines if the user can enroll in the course
        /// </summary>
        private async Task<bool> CanUserEnrollInCourseAsync(object? parameter, int currentUserId)
        {
            try
            {
                int coinBalance = await GetCoinBalanceAsync(currentUserId);
                return !IsEnrolled && coinBalance >= CurrentCourse.Cost;
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Enrollment Check Failed", $"Unable to verify enrollment eligibility.\nDetails: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Enrolls the user in the current course
        /// </summary>
        private async Task EnrollUserInCourseAsync(object? parameter, int currentUserId)
        {
            try
            {
                if (CurrentCourse.Cost > 0)
                {
                    bool coinDeductionSuccessful = await coinsService.TrySpendingCoinsAsync(currentUserId, CurrentCourse.Cost);
                    if (!coinDeductionSuccessful)
                    {
                        RaiseErrorMessage("Insufficient Coins", "You do not have enough coins to enroll.");
                        return;
                    }
                }

                bool enrollmentSuccessful = await courseService.EnrollInCourseAsync(currentUserId, CurrentCourse.CourseId);
                if (!enrollmentSuccessful)
                {
                    RaiseErrorMessage("Enrollment Failed", "Unable to enroll in the course.");
                    return;
                }

                CoinBalance = await coinsService.GetCoinBalanceAsync(currentUserId);
                IsEnrolled = true;
                ResetCourseProgressTracking();
                OnPropertyChanged(nameof(IsEnrolled));
                OnPropertyChanged(nameof(CoinBalance));

                StartCourseProgressTimer();
                await LoadAndOrganizeCourseModules(currentUserId);
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Enrollment Error", $"Failed to enroll in the course.\nDetails: {e.Message}");
            }
        }

        /// <summary>
        /// Resets all course progress tracking metrics
        /// </summary>
        private void ResetCourseProgressTracking()
        {
            try
            {
                totalSecondsSpentOnCourse = 0;
                FormattedTimeRemaining = FormatTimeRemainingDisplay(totalSecondsSpentOnCourse);
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Reset Progress Error", $"Failed to reset course progress.\nDetails: {e.Message}");
            }
        }

        #endregion

        #region Timer Control Methods

        /// <summary>
        /// Starts the course progress timer if not already running
        /// </summary>
        public void StartCourseProgressTimer()
        {
            try
            {
                if (IsCourseCompleted || IsCourseTimerRunning)
                    return;

                if (!IsCourseTimerRunning && IsEnrolled)
                {
                    IsCourseTimerRunning = true;
                    courseProgressTimer!.Start();
                }
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Timer Start Error", $"Could not start the course timer.\nDetails: {e.Message}");
            }
        }

        /// <summary>
        /// Pauses the course progress timer and saves the current progress
        /// </summary>
        public async Task PauseCourseProgressTimer(int currentUserId)
        {
            try
            {
                if (IsCourseTimerRunning)
                {
                    courseProgressTimer!.Stop();
                    await SaveCourseProgressTime(currentUserId);
                    IsCourseTimerRunning = false;
                }
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Pause Timer Error", $"Could not pause and save the timer.\nDetails: {e.Message}");
            }
        }

        /// <summary>
        /// Saves the current course progress time to the database
        /// </summary>
        private async Task SaveCourseProgressTime(int currentUserId)
        {
            try
            {
                int secondsToSave = (totalSecondsSpentOnCourse - lastSavedTimeInSeconds) /
                                TimeTrackingDatabaseAdjustmentDivisor;

                Console.WriteLine($"Attempting to save: Current={totalSecondsSpentOnCourse}, " +
                                  $"LastSaved={lastSavedTimeInSeconds}, ToSave={secondsToSave}");

                if (secondsToSave > 0)
                {
                    Console.WriteLine($"Saving {secondsToSave} seconds");
                    await courseService.UpdateTimeSpentAsync(currentUserId, CurrentCourse.CourseId, secondsToSave);
                    lastSavedTimeInSeconds = totalSecondsSpentOnCourse;
                }
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Save Time Error", $"Could not save course progress.\nDetails: {e.Message}");
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
        public async Task RefreshCourseModulesDisplay(int currentUserId)
        {
            try
            {
                await LoadAndOrganizeCourseModules(currentUserId);
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Module Refresh Error", $"Unable to refresh modules.\nDetails: {e.Message}");
            }
        }

        #endregion

        #region Reward Handling

        /// <summary>
        /// Marks a module as completed and checks for any earned rewards
        /// </summary>
        /// <param name="targetModuleId">ID of the module to mark as completed</param>
        public async Task MarkModuleAsCompletedAndCheckRewards(int targetModuleId, int currentUserId)
        {
            try
            {
                await courseService.CompleteModuleAsync(currentUserId, targetModuleId, CurrentCourse.CourseId);

                Module completedModule = await courseService.GetModuleAsync(targetModuleId);
                if (!completedModule.IsBonus)
                {
                    await UpdateCompletionStatus(currentUserId);

                    if (IsCourseCompleted)
                    {
                        courseProgressTimer?.Stop();
                        IsCourseTimerRunning = false;
                        await CheckForCompletionReward(currentUserId);
                        await CheckForTimedReward(currentUserId);
                    }
                }
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Module Completion Error", $"Failed to mark module as completed.\nDetails: {e.Message}");
            }
        }

        /// <summary>
        /// Updates the module completion status properties
        /// </summary>
        private async Task UpdateCompletionStatus(int currentUserId)
        {
            try
            {
                CompletedModules = await courseService.GetCompletedModulesCountAsync(currentUserId, CurrentCourse.CourseId);
                OnPropertyChanged(nameof(CompletedModules));
                OnPropertyChanged(nameof(IsCourseCompleted));
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Status Update Error", $"Failed to update module completion status.\nDetails: {e.Message}");
            }
        }

        /// <summary>
        /// Checks and claims the course completion reward if eligible
        /// </summary>
        private async Task CheckForCompletionReward(int currentUserId)
        {
            try
            {
                bool rewardClaimed = await courseService.ClaimCompletionRewardAsync(currentUserId, CurrentCourse.CourseId, CourseCompletionRewardCoins);
                if (rewardClaimed)
                {
                    CompletionRewardClaimed = true;
                    OnPropertyChanged(nameof(CompletionRewardClaimed));
                    OnPropertyChanged(nameof(CoinBalance));
                    ShowCourseCompletionRewardNotification();
                }
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Completion Reward Error", $"Failed to claim course completion reward.\nDetails: {e.Message}");
            }
        }

        /// <summary>
        /// Checks and claims the timed completion reward if eligible
        /// </summary>
        private async Task CheckForTimedReward(int currentUserId)
        {
            try
            {
                if (TimeRemaining > 0)
                {
                    bool rewardClaimed = await courseService.ClaimTimedRewardAsync(currentUserId, CurrentCourse.CourseId, totalSecondsSpentOnCourse, TimedCompletionRewardCoins);
                    if (rewardClaimed)
                    {
                        TimedRewardClaimed = true;
                        OnPropertyChanged(nameof(TimedRewardClaimed));
                        OnPropertyChanged(nameof(CoinBalance));
                        ShowTimedCompletionRewardNotification();
                    }
                }
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Timed Reward Error", $"Failed to claim timed reward.\nDetails: {e.Message}");
            }
        }

        #endregion

        #region Notification Methods

        /// <summary>
        /// Shows notification for course completion reward
        /// </summary>
        private void ShowCourseCompletionRewardNotification()
        {
            try
            {
                string message = $"Congratulations! You have completed all required modules in this course. {CourseCompletionRewardCoins} coins have been added to your balance.";
                notificationHelper!.ShowTemporaryNotification(message);
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Notification Error", $"Failed to show course completion reward notification.\nDetails: {e.Message}");
            }
        }

        /// <summary>
        /// Shows notification for timed completion reward
        /// </summary>
        private void ShowTimedCompletionRewardNotification()
        {
            try
            {
                string message = $"Congratulations! You completed the course within the time limit. {TimedCompletionRewardCoins} coins have been added to your balance.";
                notificationHelper!.ShowTemporaryNotification(message);
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Notification Error", $"Failed to show timed completion reward notification.\nDetails: {e.Message}");
            }
        }

        /// <summary>
        /// Shows notification for successful module purchase
        /// </summary>
        /// <param name="module">The module that was purchased</param>
        private async Task ShowModulePurchaseNotificationAsync(Module module, int currentUserId)
        {
            try
            {
                string message = $"Congratulations! You have purchased bonus module {module.Title}, {module.Cost} coins have been deducted from your balance.";
                notificationHelper!.ShowTemporaryNotification(message);
                await RefreshCourseModulesDisplay(currentUserId);
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Purchase Notification Error", $"Failed to show purchase success notification.\nDetails: {e.Message}");
            }
        }

        #endregion

        #region Module Purchase

        /// <summary>
        /// Attempts to purchase a bonus module
        /// </summary>
        /// <param name="module">The module to purchase</param>
        public async Task AttemptBonusModulePurchaseAsync(Module? module, int currentUserId)
        {
            ArgumentNullException.ThrowIfNull(module);

            try
            {
                if (await courseService.IsModuleCompletedAsync(currentUserId, module.ModuleId))
                {
                    return;
                }

                bool purchaseSuccessful = await courseService.BuyBonusModuleAsync(currentUserId, module.ModuleId);

                if (purchaseSuccessful)
                {
                    await UpdatePurchasedModuleStatus(module, currentUserId);
                    await ShowModulePurchaseNotificationAsync(module, currentUserId);
                    CoinBalance = await coinsService.GetCoinBalanceAsync(currentUserId);
                    OnPropertyChanged(nameof(ModuleRoadmap));
                    OnPropertyChanged(nameof(CoinBalance));
                }
                else
                {
                    ShowPurchaseFailedNotification();
                }
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Purchase Error", $"Could not complete purchase of module '{module.Title}'.\nDetails: {e.Message}");
            }
        }

        /// <summary>
        /// Updates the status of a purchased module
        /// </summary>
        /// <param name="module">The module that was purchased</param>
        private async Task UpdatePurchasedModuleStatus(Module module, int currentUserId)
        {
            try
            {
                var moduleToUpdate = ModuleRoadmap.FirstOrDefault(m => m.Module!.ModuleId == module.ModuleId);
                if (moduleToUpdate != null)
                {
                    moduleToUpdate.IsUnlocked = true;
                    moduleToUpdate.IsCompleted = false;
                    try
                    {
                        await courseService.OpenModuleAsync(currentUserId, module.ModuleId);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        Console.WriteLine($"OpenModuleAsync failed: {ex.Message}");
                        notificationHelper?.ShowTemporaryNotification("Something went wrong. Please try again.");
                    }
                }
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Update Error", $"Failed to update the status of purchased module '{module.Title}'.\nDetails: {e.Message}");
            }
        }

        /// <summary>
        /// Shows notification for failed module purchase
        /// </summary>
        private void ShowPurchaseFailedNotification()
        {
            try
            {
                notificationHelper!.ShowTemporaryNotification("You do not have enough coins to buy this module.");
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Notification Error", $"Failed to show failed purchase notification.\nDetails: {e.Message}");
            }
        }

        #endregion
    }
}
