using System.Collections.Generic;
using System.Text.Json;
using System;
using System.Linq;
using DuoClassLibrary.Models.Exercises;

namespace DuoClassLibrary.Models.Quizzes;

public class Exam : BaseQuiz
{
    private const int MAX_EXERCISES = 25;
    private const double PASSING_THRESHOLD = 90;

    public Exam(int id, int? sectionId)
        : base(id, sectionId, MAX_EXERCISES, PASSING_THRESHOLD)
    {
    }

    public override string ToString()
    {
        return $"{base.ToString()} [Final Exam]";
    }
}
