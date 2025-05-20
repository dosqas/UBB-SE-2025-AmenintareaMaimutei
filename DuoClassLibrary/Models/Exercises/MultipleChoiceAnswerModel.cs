using System;
using System.Text.Json.Serialization;

namespace DuoClassLibrary.Models.Exercises;

public class MultipleChoiceAnswerModel
{
    public int AnswerModelId { get; set; }
    public string Answer { get; set; }
    public bool IsCorrect { get; set; }

    [JsonIgnore]
    public Exercise Exercise { get; set; }

    public MultipleChoiceAnswerModel()
    {
    }

    public MultipleChoiceAnswerModel(string answer, bool isCorrect)
    {
        Answer = answer;
        IsCorrect = isCorrect;
    }

    public override string ToString()
    {
        return $"{Answer}{(IsCorrect ? " (Correct)" : string.Empty)}";
    }
}