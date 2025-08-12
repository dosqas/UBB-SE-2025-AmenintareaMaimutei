using System;

namespace Duo.Models.Exercises
{
    public class FlashcardExercise : Exercise
    {
        public string Type { get; } = "Flashcard";
        public int TimeInSeconds { get; }

        private string answer;
        public string Answer
        {
            get => answer;
            set => answer = value;
        }

        private TimeSpan elapsedTime;
        public TimeSpan ElapsedTime
        {
            get => elapsedTime;
            set => elapsedTime = value;
        }

        // Property for database repository support
        public string Sentence => Question;

        public FlashcardExercise(int id, string question, string answer, Difficulty difficulty = Difficulty.Normal)
            : base(id, question, difficulty)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                throw new ArgumentException("Answer cannot be empty", nameof(answer));
            }

            this.answer = answer;

            // Default time based on difficulty
            TimeInSeconds = GetDefaultTimeForDifficulty(difficulty);
        }

        // Constructor for database repository support
        public FlashcardExercise(int id, string sentence, string answer, int timeInSeconds, Difficulty difficulty = Difficulty.Normal)
            : base(id, sentence, difficulty)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                throw new ArgumentException("Answer cannot be empty", nameof(answer));
            }

            this.answer = answer;
            TimeInSeconds = timeInSeconds;
        }

        // Helper method to determine default time based on difficulty
        private int GetDefaultTimeForDifficulty(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => 15,
                Difficulty.Normal => 30,
                Difficulty.Hard => 45,
                _ => 30
            };
        }
        public string GetCorrectAnswer()
        {
            return Answer;
        }

        // Validation method from develop branch
        public bool ValidateAnswer(string userAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer))
            {
                return false;
            }

            return userAnswer.Trim().Equals(Answer.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"Id: {Id},  Difficulty: {Difficulty}, Time: {TimeInSeconds}s";
        }
    }
}