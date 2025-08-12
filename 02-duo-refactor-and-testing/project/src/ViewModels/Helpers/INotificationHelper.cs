namespace CourseApp.ViewModels.Helpers
{
    /// <summary>
    /// Interface for managing temporary notifications in the course view.
    /// </summary>
    public interface INotificationHelper
    {
        /// <summary>
        /// Displays a temporary notification message for the configured duration.
        /// </summary>
        /// <param name="message">The notification message to display.</param>
        void ShowTemporaryNotification(string message);

        /// <summary>
        /// Disposes of the notification helper by unsubscribing from timer events.
        /// </summary>
        void Dispose();
    }
}
