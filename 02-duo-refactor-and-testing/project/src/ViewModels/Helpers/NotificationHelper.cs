namespace CourseApp.ViewModels.Helpers
{
    using System;
    using CourseApp.Services;
    using CourseApp.ViewModels;

    /// <summary>
    /// Helper class for managing temporary notifications in the course view.
    /// Handles displaying notifications for a specified duration and automatically hiding them.
    /// </summary>
    /// <remarks>
    /// This class implements INotificationHelper to aid testing and IDisposable to properly clean up timer event subscriptions.
    /// Notifications are displayed for the duration specified in <see cref="CourseViewModel.NotificationDisplayDurationInSeconds"/>.
    /// </remarks>
    internal partial class NotificationHelper : INotificationHelper, IDisposable
    {
        #region Fields

        /// <summary>Reference to the parent ViewModel for notification updates</summary>
        private readonly CourseViewModel parentViewModel;

        /// <summary>Timer service for managing notification display duration</summary>
        private readonly IDispatcherTimerService timer;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the NotificationHelper class
        /// </summary>
        /// <param name="parentViewModel">The parent ViewModel that owns this notification helper</param>
        /// <param name="timerService">The timer service to use for notification duration</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when either parentViewModel or timerService is null
        /// </exception>
        public NotificationHelper(CourseViewModel parentViewModel, IDispatcherTimerService timerService)
        {
            this.parentViewModel = parentViewModel ?? throw new ArgumentNullException(nameof(parentViewModel));
            this.timer = timerService ?? throw new ArgumentNullException(nameof(timerService));
            this.timer.Tick += OnNotificationTimerTick!;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Displays a temporary notification message for the configured duration
        /// </summary>
        /// <param name="message">The notification message to display</param>
        /// <remarks>
        /// The notification will automatically hide after <see cref="CourseViewModel.NotificationDisplayDurationInSeconds"/> seconds.
        /// If the timer cannot be started, the method fails silently.
        /// </remarks>
        public virtual void ShowTemporaryNotification(string message)
        {
            parentViewModel.NotificationMessage = message;
            parentViewModel.ShowNotification = true;
            timer.Interval = TimeSpan.FromSeconds(CourseViewModel.NotificationDisplayDurationInSeconds);

            try
            {
                timer.Start();
            }
            catch (InvalidOperationException)
            {
                // Silently handle timer start failures
            }
        }

        /// <summary>
        /// Disposes of the notification helper by unsubscribing from timer events
        /// </summary>
        public void Dispose()
        {
            timer.Tick -= OnNotificationTimerTick!;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles the notification timer tick event by hiding the notification
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="eventArgs">The event arguments</param>
        private void OnNotificationTimerTick(object sender, EventArgs eventArgs)
        {
            parentViewModel.ShowNotification = false;
            timer.Stop();
        }

        #endregion
    }
}