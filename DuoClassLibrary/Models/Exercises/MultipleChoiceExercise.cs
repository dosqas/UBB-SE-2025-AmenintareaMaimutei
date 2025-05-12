﻿using DuoClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuoClassLibrary.Models.Exercises;

public class MultipleChoiceExercise : Exercise
{
    public List<MultipleChoiceAnswerModel>? Choices { get; set; }

    public MultipleChoiceExercise(int id, string question, Difficulty difficulty, List<MultipleChoiceAnswerModel> choices)
        : base(id, question, difficulty)
    {
        if (choices == null || !choices.Any(c => c.IsCorrect))
        {
            throw new ArgumentException("At least one choice must be correct", nameof(choices));
        }

        Choices = choices;
        Type = "MultipleChoice";
    }

    public MultipleChoiceExercise()
    {
        Type = "MultipleChoice";
    }

    public bool ValidateAnswer(List<string> userAnswers)
    {
        if (userAnswers == null || userAnswers.Count == 0)
        {
            return false;
        }

        var correctAnswers = Choices.Where(a => a.IsCorrect).Select(a => a.Answer).OrderBy(a => a).ToList();
        var userSelection = userAnswers.OrderBy(a => a).ToList();

        return correctAnswers.SequenceEqual(userSelection);
    }

    public override string ToString()
    {
        var choices = string.Join(", ", Choices.Select(c => $"{c.Answer}{(c.IsCorrect ? " (Correct)" : string.Empty)}"));
        return $"{base.ToString()} [Multiple Choice] Choices: {choices}";
    }
}

