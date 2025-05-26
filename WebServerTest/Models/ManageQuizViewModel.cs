using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Exercises;
using System.Collections.Generic;

public class ManageQuizViewModel
{
    public List<Quiz> Quizzes { get; set; } = new();
    public int? SelectedQuizId { get; set; }
    public List<Exercise> AssignedExercises { get; set; } = new();
    public List<Exercise> AvailableExercises { get; set; } = new();
}
