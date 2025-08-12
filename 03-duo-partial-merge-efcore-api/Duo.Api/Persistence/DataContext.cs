using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Sections;
using Duo.Api.Models.Roadmaps;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Persistence
{
    /// <summary>
    /// Represents the database context for the application.
    /// This class is responsible for configuring the database schema and relationships.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DataContext"/> class with the specified options.
    /// </remarks>
    /// <param name="options">The options to configure the database context.</param>
    [ExcludeFromCodeCoverage]
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        #region DbSets

        /// <summary>
        /// Gets or sets the users in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the tags in the database.
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the modules in the database.
        /// </summary>
        public DbSet<Module> Modules { get; set; }

        /// <summary>
        /// Gets or sets the course completions in the database.
        /// </summary>
        public DbSet<CourseCompletion> CourseCompletions { get; set; }

        /// <summary>
        /// Gets or sets the courses in the database.
        /// </summary>
        public DbSet<Course> Courses { get; set; }

        /// <summary>
        /// Gets or sets the enrollments in the database.
        /// </summary>
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<QuizSubmissionEntity> QuizSubmissions { get; set; }
        public DbSet<AnswerSubmissionEntity> AnswerSubmissions { get; set; }

        /// <summary>
        /// Gets or sets the exams in the database.
        /// </summary>
        public DbSet<Exam> Exams { get; set; }

        /// <summary>
        /// Gets or sets the quizzes in the database.
        /// </summary>
        public DbSet<Quiz> Quizzes { get; set; }

        /// <summary>
        /// Gets or sets the sections in the database.
        /// </summary>
        public DbSet<Section> Sections { get; set; }

        /// <summary>
        /// Gets or sets the exercises in the database.
        /// </summary>
        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<Roadmap> Roadmaps { get; set; }

        public DbSet<UserProgress> UserProgresses { get; set; }

        public DbSet<CourseTag> CourseTags { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the database schema and relationships.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure the database schema.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureSectionRelationships(modelBuilder);
            ConfigureExerciseHierarchy(modelBuilder);
            ConfigureExerciseSpecificProperties(modelBuilder);
            ConfigureCourseTagRelationships(modelBuilder);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Configures relationships between sections, quizzes, and exams.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure relationships.</param>
        private static void ConfigureSectionRelationships(ModelBuilder modelBuilder)
        {
            // Section -> Quizzes (one-to-many)
            modelBuilder.Entity<Section>()
                .HasMany(s => s.Quizzes)
                .WithOne(q => q.Section)
                .HasForeignKey(q => q.SectionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Section -> Exam (one-to-one)
            modelBuilder.Entity<Section>()
                .HasOne(s => s.Exam)
                .WithOne(e => e.Section)
                .HasForeignKey<Exam>(e => e.SectionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Quiz -> Exercises (many-to-many)
            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Exercises)
                .WithMany(e => e.Quizzes)
                .UsingEntity(j => j.ToTable("QuizExercises"));

            // Exam -> Exercises (many-to-many)
            modelBuilder.Entity<Exam>()
                .HasMany(e => e.Exercises)
                .WithMany(e => e.Exams)
                .UsingEntity(j => j.ToTable("ExamExercises"));

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CourseId });

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.CourseId).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Enrollments)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
        }

        /// <summary>
        /// Configures the table-per-hierarchy (TPH) inheritance for exercises.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure inheritance.</param>
        private static void ConfigureExerciseHierarchy(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exercise>()
                .HasDiscriminator<string>("ExerciseType")
                .HasValue<AssociationExercise>("Association")
                .HasValue<FillInTheBlankExercise>("Fill in the blank")
                .HasValue<FlashcardExercise>("Flashcard")
                .HasValue<MultipleChoiceExercise>("Multiple Choice");

            // Index for Exercise Question (search optimization)
            modelBuilder.Entity<Exercise>()
                .HasIndex(e => e.Question)
                .HasDatabaseName("IX_Exercise_Question");
        }

        /// <summary>
        /// Configures specific properties for exercises, such as JSON serialization.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure properties.</param>
        private static void ConfigureExerciseSpecificProperties(ModelBuilder modelBuilder)
        {
            // MultipleChoiceExercise -> MultipleChoiceAnswerModel (real one-to-many)
            modelBuilder.Entity<MultipleChoiceExercise>()
                .HasMany(m => m.Choices)
                .WithOne(c => c.Exercise)
                .HasForeignKey(c => c.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Roadmap -> Sections (one-to-many)
            modelBuilder.Entity<Roadmap>()
                .HasMany(r => r.Sections)
                .WithOne(s => s.Roadmap)
                .HasForeignKey(s => s.RoadmapId)
                .OnDelete(DeleteBehavior.Cascade);
            // Course -> CourseCompleion (one-to-many)
            modelBuilder.Entity<CourseCompletion>()
                .HasKey(cc => new { cc.UserId, cc.CourseId });

            modelBuilder.Entity<UserProgress>()
            .HasKey(up => new { up.UserId, up.ModuleId });

            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.Module)
                .WithMany()
                .HasForeignKey(up => up.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        /// <param name="modelBuilder">The model builder used to configure the relationship.</param>
        private static void ConfigureCourseTagRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseTag>()
                .ToTable("CourseTags");

            modelBuilder.Entity<CourseTag>()
                .HasKey(ct => new { ct.CourseId, ct.TagId });

            modelBuilder.Entity<CourseTag>()
                .HasOne(ct => ct.Course)
                .WithMany(c => c.CourseTags)
                .HasForeignKey(ct => ct.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseTag>()
                .HasOne(ct => ct.Tag)
                .WithMany(t => t.CourseTags)
                .HasForeignKey(ct => ct.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion
    }
}