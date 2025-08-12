using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Duo.Models.Quizzes;

namespace Duo.Models.Exercises;

public abstract class Exercise
{
    public int ExerciseId { get; set; }
    public string Question { get; set; }
    public Difficulty Difficulty { get; set; }

    public string Type { get; set; }

    [JsonIgnore]
    public ICollection<Exam> Exams { get; set; }

    [JsonIgnore]
    public ICollection<Quiz> Quizzes { get; set; }

    protected Exercise(int id, string question, Difficulty difficulty)
    {
        ExerciseId = id;
        Question = question;
        Difficulty = difficulty;
    }

    protected Exercise()
    {
    }

    public override string ToString()
    {
        return $"Exercise {ExerciseId}: {Question} (Difficulty: {Difficulty})";
    }
}