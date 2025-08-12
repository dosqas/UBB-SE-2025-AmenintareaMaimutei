using System.Diagnostics.CodeAnalysis;

namespace DuoClassLibrary.Models
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class UserProgress
    {
        public int UserId { get; set; }
        public int ModuleId { get; set; }
        public string Status { get; set; } = "not_completed";
        public bool ImageClicked { get; set; } = false;

        public User? User { get; set; }
        public Module? Module { get; set; }
    }
}
