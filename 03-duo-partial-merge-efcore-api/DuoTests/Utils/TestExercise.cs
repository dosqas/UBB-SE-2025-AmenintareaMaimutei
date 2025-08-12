using Duo.Models.Exercises;
using Duo.Models;
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
