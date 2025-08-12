using System;
using System.ComponentModel.DataAnnotations;

namespace Duo.Api.Models
{
    public class User
    {
        // User identification
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // User privacy and activity settings
        public bool PrivacyStatus { get; set; } = false;
        public bool OnlineStatus { get; set; } = false;
        public DateTime DateJoined { get; set; } = DateTime.Now;
        public string ProfileImage { get; set; } = "default.jpg";

        // User statistics
        public int TotalPoints { get; set; } = 0;
        public int CoursesCompleted { get; set; } = 0;
        public int QuizzesCompleted { get; set; } = 0;
        public int Streak { get; set; } = 0;

        // Authentication
        public string Password { get; set; } = string.Empty;

        // User activity tracking
        public DateTime? LastActivityDate { get; set; }
        public decimal Accuracy { get; set; } = 0.00m;

        public string OnlineStatusDisplayText => OnlineStatus ? "Active" : "Not Active";

        //ADDED FROM 927-1
        /// <summary>
        /// Gets or sets the number of completed sections by the user.
        /// </summary>
        public int NumberOfCompletedSections { get; set; }

        /// <summary>
        /// Gets or sets the number of completed quizzes in the current section.
        /// </summary>
        public int NumberOfCompletedQuizzesInSection { get; set; }
        /// <summary>
        /// Gets or sets the coin balance of the user.
        /// </summary>
        public int CoinBalance { get; set; }

        public List<Enrollment>? Enrollments { get; set; }


        /// <summary>
        /// Gets or sets the collection of sections completed by the user.
        /// </summary>
        public ICollection<SectionCompletions> SectionCompletions { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of quizzes completed by the user.
        /// </summary>
        public ICollection<QuizCompletions> QuizCompletions { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of exams completed by the user.
        /// </summary>
        public ICollection<ExamCompletions> ExamCompletions { get; set; } = [];


        public User(int userId, string username)
        {
            UserId = userId;
            UserName = username;
        }
        public User(string username)
        {
            UserName = username;
        }
        public User()
        {
            // Default constructor
        }

        public string GetLastSeenDisplayText
        {
            get
            {
                if (OnlineStatus)
                {
                    return "Active Now";
                }

                if (LastActivityDate.HasValue)
                {
                    var timeElapsedSinceLastActivity = DateTime.Now - LastActivityDate.Value;

                    if (timeElapsedSinceLastActivity.TotalMinutes < 1)
                    {
                        return "Less than a minute ago";
                    }
                    else if (timeElapsedSinceLastActivity.TotalHours < 1)
                    {
                        return $"{Math.Floor(timeElapsedSinceLastActivity.TotalMinutes)} minutes ago";
                    }
                    else if (timeElapsedSinceLastActivity.TotalDays < 1)
                    {
                        return $"{Math.Floor(timeElapsedSinceLastActivity.TotalHours)} hours ago";
                    }
                    else
                    {
                        return $"{Math.Floor(timeElapsedSinceLastActivity.TotalDays)} days ago";
                    }
                }

                return "Never";
            }
        }

        /// <summary>
        /// Returns a string representation of the user, including their username and coin balance.
        /// </summary>
        /// <returns>A string describing the user.</returns>
        public override string ToString()
        {
            return $"User: {UserName}, Coin Balance: {CoinBalance}, Last Login: {LastActivityDate}";
        }
    }

}