using DuoClassLibrary.Models;

namespace WebServerTest.Models

{
    public class ModuleViewModel
    {
        // The Module object which will hold the module's properties (title, description, image URL, etc.)
        public Module Module { get; set; } = null!;

        // The ID of the course the module belongs to
        public int CourseId { get; set; }

        // Indicates whether the module is completed
        public bool IsCompleted { get; set; }

        // Indicates whether the module is unlocked
        public bool IsUnlocked { get; set; }

        // Time spent in the module (used in the Razor page for display)
        public string TimeSpent { get; set; } = "0h 0m";

        // Coin balance of the user (used in the Razor page for display)
        public string CoinBalance { get; set; } = "0";
    }
}
