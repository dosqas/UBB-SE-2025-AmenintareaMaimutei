using System.ComponentModel.DataAnnotations;

namespace DuoClassLibrary.Models;

public class LeaderboardEntry
{
    [Key]
    public int UserId { get; set; }
    
    public int Rank { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    public string ProfilePicture { get; set; } = string.Empty;
    
    public int CompletedQuizzes { get; set; }
    
    public decimal Accuracy { get; set; }

    public decimal ScoreValue { get; set; }
}
