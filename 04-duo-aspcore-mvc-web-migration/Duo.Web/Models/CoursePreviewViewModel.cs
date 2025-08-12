using DuoClassLibrary.Models;

namespace Duo.Web.Models
{
    public class CoursePreviewViewModel
    {
        public Course Course { get; set; }
        public List<ModuleViewModel> Modules { get; set; } = new();
        public bool IsEnrolled { get; set; }
        public int CoinBalance { get; set; }
        public int CompletedModules { get; set; }
        public int RequiredModules { get; set; }
        public string FormattedTimeRemaining { get; set; }
        public List<Tag> Tags { get; set; } = new();

        // Notification
        public bool ShowNotification { get; set; }
        public string NotificationMessage { get; set; }

        // Visibility for coin cost section (used in XAML; you can interpret this as a flag in web)
        public bool ShowCourseCost => !IsEnrolled && Course?.Cost > 0;
    }
}