using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using DotNetEnv;
using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Roadmaps;
using Duo.Api.Models.Sections;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Duo.Models.Quizzes.API;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api
{
    /// <summary>
    /// The entry point of the application.
    /// Configures services, middleware, and the HTTP request pipeline.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Program
    {
        #region Methods

        /// <summary>
        /// The main method, which serves as the entry point of the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            // Load environment variables from the .env file.
            Env.Load(".env");

            // Create a WebApplication builder.
            var builder = WebApplication.CreateBuilder(args);

            // Configure services for the application.
            ConfigureServices(builder);

            // Build the application.
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            ConfigureMiddleware(app);

            // Apply database migrations and seed data.
            ApplyMigrationsAndSeedData(app);

            // Run the application.
            app.Run();
        }

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <param name="builder">The WebApplication builder.</param>
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Add controllers with JSON options.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            // Register IRepository and Repository for dependency injection.
            builder.Services.AddScoped<IRepository, Repository>();

            // Configure Swagger/OpenAPI.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure the database context with a connection string.
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__WebApiDatabase");
            Console.WriteLine("Connection string: " + connectionString);

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionString));
        }

        /// <summary>
        /// Configures middleware for the application.
        /// </summary>
        /// <param name="app">The WebApplication instance.</param>
        private static void ConfigureMiddleware(WebApplication app)
        {
            // Enable Swagger in development environments.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable HTTPS redirection and authorization.
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Map controllers to endpoints.
            app.MapControllers();
        }

        /// <summary>
        /// Applies database migrations and seeds initial data.
        /// </summary>
        /// <param name="app">The WebApplication instance.</param>
        private static void ApplyMigrationsAndSeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();

            // Apply database migrations.
            db.Database.Migrate();

            // Seed initial data (e.g., add a default user if necessary).
            // Uncomment and modify the following block if seeding is required.
            if (!db.Users.Any(u => u.UserId == 0))
            {
                db.Users.Add(new User
                {
                    UserId = 0,
                    CoinBalance = 1000, // Example balance
                    LastLoginTime = DateTime.Now
                });
            }
            db.SaveChanges();

            // Tags
            if (!db.Tags.Any())
            {
                db.Tags.AddRange(Enumerable.Range(1, 10).Select(i => new Tag
                {
                    Name = $"Tag{i}",
                }));
            }
            db.SaveChanges();

            // Modules
            if (!db.Modules.Any())
            {
                db.Modules.AddRange(Enumerable.Range(1, 10).Select(i => new Module
                {
                    CourseId = i,
                    Title = $"Module {i}",
                    Description = $"Module {i} Description",
                    Position = i,
                    IsBonus = i % 2 == 0, // Example bonus status
                    Cost = i * 10, // Example cost
                    ImageUrl = $"module{i}.jpg",
                }));
            }
            db.SaveChanges();

            // User Progresses
            if (!db.UserProgresses.Any())
            {
                // Add UserProgress entries
                // Add UserProgress entries
                var users = db.Users.ToList(); // Fetch existing users
                var modules = db.Modules.ToList(); // Fetch existing modules
                db.UserProgresses.AddRange(users.Select((user, index) => new UserProgress
                {
                    UserId = user.UserId, // Use existing UserId
                    ModuleId = modules[index % modules.Count].ModuleId, // Use existing ModuleId
                    Status = index % 2 == 0 ? "Completed" : "In Progress",
                    ImageClicked = index % 2 == 0
                }));
            }
            db.SaveChanges();

            // Courses
            if (!db.Courses.Any())
            {
                db.Courses.AddRange(Enumerable.Range(1, 10).Select(i => new Course
                {
                    Title = $"Course {i}",
                    Description = $"Description for Course {i}",
                    IsPremium = i % 2 == 0, // Example premium status
                    Cost = i * 100, // Example cost
                    ImageUrl = $"course{i}.jpg",
                    TimeToComplete = i * 3600, // Example time to complete in seconds
                    Difficulty = i % 2 == 0 ? "Beginner" : "Advanced",
                }));
            }
            db.SaveChanges();

            // Enrollments
            if (!db.Enrollments.Any())
            {
                // Add Enrollment entries
                var users = db.Users.ToList(); // Fetch existing users
                var courses = db.Courses.ToList(); // Fetch existing courses
                db.Enrollments.AddRange(users.Select((user, index) => new Enrollment
                {
                    UserId = user.UserId, // Use existing UserId
                    CourseId = courses[index % courses.Count].CourseId, // Use existing CourseId
                    EnrolledAt = DateTime.Now.AddDays(-index), // Example enrollment date
                    TimeSpent = index * 3600, // Example time spent in seconds
                    IsCompleted = index % 2 == 0 // Example completion status
                }));
            }
            db.SaveChanges();

            // Course Completions
            if (!db.CourseCompletions.Any())
            {
                // Add CourseCompletion entries
                var users = db.Users.ToList(); // Fetch existing users
                var courses = db.Courses.ToList(); // Fetch existing courses
                db.CourseCompletions.AddRange(users.Select((user, index) => new CourseCompletion
                {
                    UserId = user.UserId, // Use existing UserId
                    CourseId = courses[index % courses.Count].CourseId, // Use existing CourseId
                    CompletionRewardClaimed = index % 2 == 0, // Example reward claimed status
                    TimedRewardClaimed = index % 2 == 0, // Example timed reward claimed status
                    CompletedAt = DateTime.UtcNow.AddDays(-index) // Example completion date
                }));
            }
            db.SaveChanges();

            // Roadmaps
            if (!db.Roadmaps.Any())
            {
                db.Roadmaps.AddRange(Enumerable.Range(1, 10).Select(i => new Roadmap
                {
                    Name = $"Roadmap {i}",
                }));
            }
            db.SaveChanges();

            // Sections
            if (!db.Sections.Any())
            {
                db.Sections.AddRange(Enumerable.Range(1, 3).Select(i => new Section
                {
                    SubjectId = i,
                    Title = $"Section {i}",
                    Description = $"Description for Section {i}",
                    RoadmapId = 1,
                    OrderNumber = i,
                }));
            }
            db.SaveChanges();

            // Exercises
            if (!db.Exercises.Any())
            {
                var random = new Random();
                var exercises = new List<Exercise>();

                for (int i = 1; i <= 10; i++)
                {
                    var exerciseType = random.Next(0, 2); // Randomly choose between two exercise types
                    Exercise exercise;

                    switch (exerciseType)
                    {
                        case 0:
                            exercise = new AssociationExercise
                            {
                                Question = $"Match the items for Association Exercise {i}",
                                Difficulty = (Difficulty)random.Next(0, 3), // Random difficulty
                                FirstAnswersList = new List<string> { "Item1", "Item2", "Item3" },
                                SecondAnswersList = new List<string> { "Match1", "Match2", "Match3" }
                            };
                            break;

                        case 1:
                            exercise = new FillInTheBlankExercise
                            {
                                Question = $"Complete the sentence for Fill in the Blank Exercise {i}",
                                Difficulty = (Difficulty)random.Next(0, 3),
                                PossibleCorrectAnswers = new List<string> { "Answer1", "Answer2", "Answer3" },
                            };
                            break;

/*                        case ExerciseType.MultipleChoice:

                            exercise = new MultipleChoiceExercise
                            {
                                Question = $"Choose the correct answer for Multiple Choice Exercise {i}",
                                Difficulty = (Difficulty)random.Next(0, 3),
                                Choices = new List<MultipleChoiceAnswerModel> {  },
                            };
                            break;

                        case ExerciseType.Flashcard:
                            exercise = new FlashcardExercise
                            {
                                Question = $"Flashcard Question {i}",
                                Difficulty = (Difficulty)random.Next(0, 3),
                                Answer = $"Flashcard Answer {i}",
                                ElapsedTime = new TimeSpan(0, new Random().Next(0, 60), new Random.Next(0, 60))
                            };
                            break;*/

                        default:
                            throw new InvalidOperationException("Unknown exercise type");
                    }

                    exercises.Add(exercise);
                }

                db.Exercises.AddRange(exercises);
            }
            db.SaveChanges();

            // Exams
            if (!db.Exams.Any())
            {
                db.Exams.AddRange(Enumerable.Range(1, 3).Select(i => new Exam
                {
                    SectionId = (i % 3) + 1,
                }));
            }
            db.SaveChanges();

            // Quizzes
            if (!db.Quizzes.Any())
            {
                db.Quizzes.AddRange(Enumerable.Range(1, 9).Select(i => new Quiz
                {
                    SectionId = (i % 3) + 1,
                    OrderNumber = (i / 3) + (i % 3),
                }));
            }
            db.SaveChanges();
        }

        #endregion
    }
}
