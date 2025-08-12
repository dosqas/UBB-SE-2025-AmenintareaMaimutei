using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTests.Utils
{
    public class TestExercise : Exercise
    {
        public TestExercise(int id, string question, Difficulty difficulty)
            : base(id, question, difficulty)
        {
        }
    }
}
