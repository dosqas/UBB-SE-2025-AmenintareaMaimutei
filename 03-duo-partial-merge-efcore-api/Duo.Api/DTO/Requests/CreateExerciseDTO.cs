using Duo.Api.Models;

namespace Duo.Api.DTO.Requests
{
    public class CreateExerciseDto
    {
        public string Question { get; set; } = null!;
        public Difficulty Difficulty { get; set; }
        public List<string>? Exams { get; set; }
        public List<string>? Quizzes { get; set; }
        public string Type { get; set; }
    }
}
