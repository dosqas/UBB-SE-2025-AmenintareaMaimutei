using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Duo.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Sections;
using Duo.Api.DTO;

namespace Duo.Api.Tests.Repositories
{
    [TestClass]
    public class RepositoryTests
    {
        private DataContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            return new DataContext(options);
        }

        [TestMethod]
        public void Repository_Constructor_WithValidContext_DoesNotThrowException()
        {
            // Arrange
            var context = GetInMemoryDbContext(); // Creates an in-memory DataContext with a valid database

            // Act & Assert
            var repository = new Repository(context);  // Should not throw any exception
            Assert.IsNotNull(repository); // Verify repository is successfully created
        }


        #region User
        [TestMethod]
        public async Task GetUsersFromDbAsync_ReturnsAllUsers()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Users.Add(new User { UserId = 1, UserName = "Alice" });
            context.Users.Add(new User { UserId = 2, UserName = "Bob" });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var users = await repository.GetUsersFromDbAsync();

            // Assert
            Assert.AreEqual(2, users.Count);
        }

        [TestMethod]
        public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User
            {
                UserId = 1,
                UserName = "TestUser",
                CoinBalance = 100,
                LastActivityDate = DateTime.Now
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetUserByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.UserId, result.UserId);
            Assert.AreEqual(user.UserName, result.UserName);
        }

        [TestMethod]
        public async Task GetUserByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            // Act
            var result = await repository.GetUserByIdAsync(999); // A user that does not exist

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task AddUserAsync_AddsUserSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            var user = new User
            {
                UserId = 3,
                UserName = "Charlie",
                Email = "charlie@example.com",
                CoinBalance = 100,
                LastActivityDate = DateTime.UtcNow
            };

            // Act
            await repository.AddUserAsync(user);

            // Assert
            var savedUser = await context.Users.FindAsync(3);
            Assert.IsNotNull(savedUser);
            Assert.AreEqual("Charlie", savedUser.UserName);
            Assert.AreEqual("charlie@example.com", savedUser.Email);
            Assert.AreEqual(100, savedUser.CoinBalance);
        }

        [TestMethod]
        public async Task DeleteUserAsync_RemovesUserSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 4, UserName = "Dave" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteUserAsync(4);

            // Assert
            var deletedUser = await context.Users.FindAsync(4);
            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        public async Task UpdateUserAsync_UpdatesUserSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User
            {
                UserId = 5,
                UserName = "Eve",
                Email = "eve@original.com",
                CoinBalance = 50,
                LastActivityDate = DateTime.UtcNow
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            user.UserName = "Updated Eve";
            user.Email = "eve@updated.com";
            user.CoinBalance = 200;
            await repository.UpdateUserAsync(user);

            // Assert
            var updatedUser = await context.Users.FindAsync(5);
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual("Updated Eve", updatedUser.UserName);
            Assert.AreEqual("eve@updated.com", updatedUser.Email);
            Assert.AreEqual(200, updatedUser.CoinBalance);
        }
        #endregion

        #region Tag
        [TestMethod]
        public async Task GetTagsFromDbAsync_ReturnsAllTags()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Tags.Add(new Tag { TagId = 1, Name = "Math" });
            context.Tags.Add(new Tag { TagId = 2, Name = "Science" });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var tags = await repository.GetTagsFromDbAsync();

            // Assert
            Assert.AreEqual(2, tags.Count);
        }

        [TestMethod]
        public async Task AddTagAsync_AddsTagSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            var tag = new Tag
            {
                TagId = 3,
                Name = "History"
            };

            await repository.AddTagAsync(tag);

            // Act
            var savedTag = await context.Tags.FindAsync(3);

            // Assert
            Assert.IsNotNull(savedTag);
            Assert.AreEqual("History", savedTag.Name);
        }

        [TestMethod]
        public async Task DeleteTagAsync_RemovesTagSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var tag = new Tag { TagId = 4, Name = "Geography" };
            context.Tags.Add(tag);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteTagAsync(4);
            var deletedTag = await context.Tags.FindAsync(4);

            // Assert
            Assert.IsNull(deletedTag);
        }

        [TestMethod]
        public async Task UpdateTagAsync_UpdatesTagSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var tag = new Tag { TagId = 5, Name = "Art" };
            context.Tags.Add(tag);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            tag.Name = "Fine Art";
            await repository.UpdateTagAsync(tag);
            var updatedTag = await context.Tags.FindAsync(5);

            // Assert
            Assert.IsNotNull(updatedTag);
            Assert.AreEqual("Fine Art", updatedTag.Name);
        }

        [TestMethod]
        public async Task GetTagByIdAsync_ReturnsCorrectTag()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var tag = new Tag { TagId = 6, Name = "Programming" };
            context.Tags.Add(tag);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var foundTag = await repository.GetTagByIdAsync(6);

            // Assert
            Assert.IsNotNull(foundTag);
            Assert.AreEqual("Programming", foundTag.Name);
        }
        #endregion

        #region Module
        [TestMethod]
        public async Task GetModulesFromDbAsync_ReturnsAllModules()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Modules.Add(new Module { ModuleId = 1, Title = "Module 1", Description = "Desc 1", ImageUrl = "img1.png" });
            context.Modules.Add(new Module { ModuleId = 2, Title = "Module 2", Description = "Desc 2", ImageUrl = "img2.png" });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var modules = await repository.GetModulesFromDbAsync();

            // Assert
            Assert.AreEqual(2, modules.Count);
        }

        [TestMethod]
        public async Task GetModuleByIdAsync_ReturnsCorrectModule()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var module = new Module { ModuleId = 3, Title = "Module 3", Description = "Desc 3", ImageUrl = "img3.png" };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var foundModule = await repository.GetModuleByIdAsync(3);

            // Assert
            Assert.IsNotNull(foundModule);
            Assert.AreEqual("Module 3", foundModule.Title);
        }

        [TestMethod]
        public async Task AddModuleAsync_AddsModuleSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            var module = new Module
            {
                ModuleId = 4,
                Title = "Module 4",
                Description = "Desc 4",
                ImageUrl = "img4.png"
            };

            await repository.AddModuleAsync(module);

            // Act
            var savedModule = await context.Modules.FindAsync(4);

            // Assert
            Assert.IsNotNull(savedModule);
            Assert.AreEqual("Module 4", savedModule.Title);
        }

        [TestMethod]
        public async Task UpdateModuleAsync_UpdatesModuleSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var module = new Module
            {
                ModuleId = 5,
                Title = "Old Title",
                Description = "Old Desc",
                ImageUrl = "old.png"
            };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            module.Title = "New Title";
            module.Description = "New Desc";
            await repository.UpdateModuleAsync(module);

            var updatedModule = await context.Modules.FindAsync(5);

            // Assert
            Assert.IsNotNull(updatedModule);
            Assert.AreEqual("New Title", updatedModule.Title);
            Assert.AreEqual("New Desc", updatedModule.Description);
        }

        [TestMethod]
        public async Task DeleteModuleAsync_RemovesModuleSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var module = new Module { ModuleId = 6, Title = "Module 6", Description = "Desc 6", ImageUrl = "img6.png" };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteModuleAsync(6);

            var deletedModule = await context.Modules.FindAsync(6);

            // Assert
            Assert.IsNull(deletedModule);
        }

        [TestMethod]
        public async Task OpenModuleAsync_UpdatesModuleSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var module = new Module
            {
                ModuleId = 7,
                Title = "Module 7",
                Description = "Desc 7",
                ImageUrl = "img7.png"
            };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.OpenModuleAsync(userId: 1, moduleId: 7);

            var openedModule = await context.Modules.FindAsync(7);

            // Assert
            Assert.IsNotNull(openedModule);
            Assert.AreEqual("Module 7", openedModule.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "Module not found")]
        public async Task OpenModuleAsync_ThrowsException_WhenModuleNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            // Act
            await repository.OpenModuleAsync(userId: 1, moduleId: 999);
        }

        [TestMethod]
        public async Task CompleteModuleAsync_UpdatesStatus_WhenProgressExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.UserProgresses.Add(new UserProgress
            {
                UserId = 1,
                ModuleId = 1,
                Status = "started"
            });
            await context.SaveChangesAsync();

            // Act
            await repo.CompleteModuleAsync(1, 1);
            var progress = await context.UserProgresses.FirstOrDefaultAsync();

            // Assert
            Assert.AreEqual("completed", progress?.Status);
        }

        [TestMethod]
        public async Task CompleteModuleAsync_UpdatesExistingProgressStatusToCompleted()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.UserProgresses.Add(new UserProgress
            {
                UserId = 100,
                ModuleId = 200,
                Status = "started",
                ImageClicked = false
            });
            await context.SaveChangesAsync();

            // Act
            await repo.CompleteModuleAsync(100, 200);

            // Assert
            var progress = await context.UserProgresses
                .FirstOrDefaultAsync(p => p.UserId == 100 && p.ModuleId == 200);

            Assert.IsNotNull(progress);
            Assert.AreEqual("completed", progress!.Status);
        }

        [TestMethod]
        public async Task CompleteModuleAsync_AddsNewProgressWhenNoneExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            int userId = 101;
            int moduleId = 201;

            Assert.IsFalse(await context.UserProgresses
                .AnyAsync(p => p.UserId == userId && p.ModuleId == moduleId));

            // Act
            await repo.CompleteModuleAsync(userId, moduleId);

            // Assert
            var progress = await context.FindAsync<UserProgress>(userId, moduleId);

            Assert.IsNotNull(progress);
            Assert.AreEqual("completed", progress!.Status);
            Assert.IsFalse(progress.ImageClicked);
        }



        [TestMethod]
        public async Task IsModuleOpenAsync_ReturnsTrue_IfProgressExists()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.UserProgresses.Add(new UserProgress { UserId = 3, ModuleId = 3 });
            await context.SaveChangesAsync();

            var result = await repo.IsModuleOpenAsync(3, 3);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsModuleOpenAsync_ReturnsFalse_IfProgressNotExists()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            var result = await repo.IsModuleOpenAsync(999, 999);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsModuleAvailableAsync_ReturnsFalse_WhenModuleDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            int nonExistentModuleId = 999;
            int userId = 1;

            // Act
            var result = await repo.IsModuleAvailableAsync(userId, nonExistentModuleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsModuleAvailableAsync_ReturnsTrue_WhenModulePositionIs1()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            var module = new Module
            {
                ModuleId = 1,
                CourseId = 10,
                Title = "Intro",
                Description = "First module",
                Position = 1,
                IsBonus = false,
                Cost = 0,
                ImageUrl = "image.png"
            };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.IsModuleAvailableAsync(userId: 1, moduleId: 1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsModuleAvailableAsync_ReturnsTrue_WhenModuleIsBonus()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            var module = new Module
            {
                ModuleId = 2,
                CourseId = 10,
                Title = "Bonus",
                Description = "Bonus module",
                Position = 5,
                IsBonus = true,
                Cost = 0,
                ImageUrl = "bonus.png"
            };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.IsModuleAvailableAsync(userId: 1, moduleId: 2);

            // Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public async Task IsModuleCompletedAsync_ReturnsTrue_IfCompleted()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.UserProgresses.Add(new UserProgress
            {
                UserId = 4,
                ModuleId = 4,
                Status = "completed"
            });
            await context.SaveChangesAsync();

            var result = await repo.IsModuleCompletedAsync(4, 4);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsModuleCompletedAsync_ReturnsFalse_IfNotCompleted()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.UserProgresses.Add(new UserProgress
            {
                UserId = 5,
                ModuleId = 5,
                Status = "started"
            });
            await context.SaveChangesAsync();

            var result = await repo.IsModuleCompletedAsync(5, 5);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsModuleAvailableAsync_ReturnsTrue_IfPosition1()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.Modules.Add(new Module
            {
                ModuleId = 10,
                Title = "Start",
                Description = "Intro",
                Position = 1,
                IsBonus = false,
                Cost = 0,
                ImageUrl = "img.jpg",
                CourseId = 1
            });
            await context.SaveChangesAsync();

            var result = await repo.IsModuleAvailableAsync(1, 10);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsModuleAvailableAsync_ReturnsTrue_IfBonus()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.Modules.Add(new Module
            {
                ModuleId = 11,
                Title = "Bonus",
                Description = "Optional",
                Position = 2,
                IsBonus = true,
                Cost = 0,
                ImageUrl = "img.jpg",
                CourseId = 1
            });
            await context.SaveChangesAsync();

            var result = await repo.IsModuleAvailableAsync(1, 11);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsModuleAvailableAsync_ReturnsFalse_IfNoPreviousModule()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.Modules.Add(new Module
            {
                ModuleId = 12,
                Title = "Module 2",
                Description = "Second",
                Position = 2,
                CourseId = 1,
                IsBonus = false,
                Cost = 0,
                ImageUrl = "img.jpg"
            });
            await context.SaveChangesAsync();

            var result = await repo.IsModuleAvailableAsync(1, 12);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsModuleAvailableAsync_ReturnsTrue_IfPreviousCompleted()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.Modules.AddRange(
                new Module
                {
                    ModuleId = 20,
                    Title = "First",
                    Description = "Start",
                    Position = 1,
                    CourseId = 2,
                    IsBonus = false,
                    Cost = 0,
                    ImageUrl = "img1.jpg"
                },
                new Module
                {
                    ModuleId = 21,
                    Title = "Second",
                    Description = "Next",
                    Position = 2,
                    CourseId = 2,
                    IsBonus = false,
                    Cost = 0,
                    ImageUrl = "img2.jpg"
                }
            );

            context.UserProgresses.Add(new UserProgress
            {
                UserId = 2,
                ModuleId = 20,
                Status = "completed"
            });

            await context.SaveChangesAsync();

            var result = await repo.IsModuleAvailableAsync(2, 21);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ClickModuleImageAsync_SetsImageClicked()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.UserProgresses.Add(new UserProgress
            {
                UserId = 6,
                ModuleId = 6,
                ImageClicked = false
            });
            await context.SaveChangesAsync();

            await repo.ClickModuleImageAsync(6, 6);
            var progress = await context.UserProgresses.FirstOrDefaultAsync();

            Assert.IsTrue(progress.ImageClicked);
        }

        [TestMethod]
        public async Task IsModuleImageClickedAsync_ReturnsTrue_IfClicked()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.UserProgresses.Add(new UserProgress
            {
                UserId = 7,
                ModuleId = 7,
                ImageClicked = true
            });
            await context.SaveChangesAsync();

            var result = await repo.IsModuleImageClickedAsync(7, 7);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsModuleImageClickedAsync_ReturnsFalse_IfNotClicked()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            context.UserProgresses.Add(new UserProgress
            {
                UserId = 8,
                ModuleId = 8,
                ImageClicked = false
            });
            await context.SaveChangesAsync();

            var result = await repo.IsModuleImageClickedAsync(8, 8);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsModuleImageClickedAsync_ReturnsFalse_IfProgressMissing()
        {
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            var result = await repo.IsModuleImageClickedAsync(99, 99);

            Assert.IsFalse(result);
        }
        #endregion

        #region Exercise
        [TestMethod]
        public async Task GetExercisesFromDbAsync_ReturnsAllExercises()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var ex1 = new AssociationExercise { ExerciseId = 1, Question = "Match colors with fruits" };
            var ex2 = new AssociationExercise { ExerciseId = 2, Question = "Match countries with capitals" };
            context.Exercises.Add(ex1);
            context.Exercises.Add(ex2);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var exercises = await repository.GetExercisesFromDbAsync();

            // Assert
            Assert.AreEqual(2, exercises.Count);
        }

        [TestMethod]
        public async Task GetExerciseByIdAsync_ReturnsCorrectExercise()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exercise = new AssociationExercise();
            exercise.ExerciseId = 3;
            exercise.Question = "Match fruits with their colors";
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var foundExercise = await repository.GetExerciseByIdAsync(3);

            // Assert
            Assert.IsNotNull(foundExercise);
            Assert.AreEqual("Match fruits with their colors", foundExercise.Question);
        }

        [TestMethod]
        public async Task AddExerciseAsync_AddsExerciseSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            var exercise = new AssociationExercise();
            exercise.ExerciseId = 4;
            exercise.Question = "Match animals with their sounds";

            await repository.AddExerciseAsync(exercise);

            // Act
            var savedExercise = await context.Exercises.FindAsync(4);

            // Assert
            Assert.IsNotNull(savedExercise);
            Assert.AreEqual("Match animals with their sounds", savedExercise.Question);
        }

        [TestMethod]
        public async Task UpdateExerciseAsync_UpdatesExerciseSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exercise = new AssociationExercise();
            exercise.ExerciseId = 5;
            exercise.Question = "Match European countries with their capitals";
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();

            var repository = new Repository(context);


            await repository.UpdateExerciseAsync(exercise);

            // Act
            var updatedExercise = await context.Exercises.FindAsync(5);

            // Assert
            Assert.IsNotNull(updatedExercise);
            Assert.AreEqual("Match European countries with their capitals", updatedExercise.Question);
        }

        [TestMethod]
        public async Task DeleteExerciseAsync_RemovesExerciseSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exercise = new AssociationExercise();
            exercise.ExerciseId = 6;
            exercise.Question = "Match Asian countries with their capitals";
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            await repository.DeleteExerciseAsync(6);

            // Act
            var deletedExercise = await context.Exercises.FindAsync(6);

            // Assert
            Assert.IsNull(deletedExercise);
        }
        #endregion

        #region Quiz
        [TestMethod]
        public async Task GetQuizzesFromDbAsync_ReturnsAllQuizzes()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section 1", Description = "Description of Section 1" };
            context.Sections.Add(section);

            var quiz1 = new Quiz { Id = 1, SectionId = section.Id, OrderNumber = 1 };
            var quiz2 = new Quiz { Id = 2, SectionId = section.Id, OrderNumber = 2 };

            context.Quizzes.Add(quiz1);
            context.Quizzes.Add(quiz2);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var quizzes = await repository.GetQuizzesFromDbAsync();

            // Assert
            Assert.AreEqual(2, quizzes.Count);
        }


        [TestMethod]
        public async Task GetQuizByIdAsync_ReturnsCorrectQuiz()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section 1", Description = "Description of Section 1" };
            context.Sections.Add(section);

            var quiz = new Quiz { Id = 1, SectionId = section.Id, OrderNumber = 1 };
            context.Quizzes.Add(quiz);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetQuizByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Section 1", result.Section?.Title);
        }


        [TestMethod]
        public async Task AddQuizAsync_AddsQuizToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section 1", Description = "Description of Section 1" };
            context.Sections.Add(section);

            var quiz = new Quiz { Id = 1, SectionId = section.Id, OrderNumber = 1 };
            var repository = new Repository(context);

            // Act
            await repository.AddQuizAsync(quiz);

            // Assert
            var addedQuiz = await context.Quizzes.FindAsync(1);
            Assert.IsNotNull(addedQuiz);
            Assert.AreEqual(1, addedQuiz?.Id);
            Assert.AreEqual("Section 1", addedQuiz?.Section?.Title);
        }


        [TestMethod]
        public async Task UpdateQuizAsync_UpdatesQuizInDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section 1", Description = "Description of Section 1" };
            context.Sections.Add(section);

            var quiz = new Quiz { Id = 1, SectionId = section.Id, OrderNumber = 1 };
            context.Quizzes.Add(quiz);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            quiz.OrderNumber = 2;
            await repository.UpdateQuizAsync(quiz);

            // Assert
            var updatedQuiz = await context.Quizzes.FindAsync(1);
            Assert.AreEqual(2, updatedQuiz?.OrderNumber);
        }


        [TestMethod]
        public async Task DeleteQuizAsync_DeletesQuizFromDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section 1", Description = "Description of Section 1" };
            context.Sections.Add(section);

            var quiz = new Quiz { Id = 1, SectionId = section.Id, OrderNumber = 1 };
            context.Quizzes.Add(quiz);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteQuizAsync(1);

            // Assert
            var deletedQuiz = await context.Quizzes.FindAsync(1);
            Assert.IsNull(deletedQuiz);
        }

        [TestMethod]
        public async Task GetAllQuizzesFromSectionAsync_ReturnsQuizzes_WhenQuizzesExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Quizzes.Add(new Quiz { Id = 1, SectionId = 5 });
            context.Quizzes.Add(new Quiz { Id = 2, SectionId = 5 });
            context.Quizzes.Add(new Quiz { Id = 3, SectionId = 6 });
            await context.SaveChangesAsync();

            var repo = new Repository(context);

            // Act
            var result = await repo.GetAllQuizzesFromSectionAsync(5);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task CountQuizzesFromSectionAsync_ReturnsCorrectCount()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Quizzes.Add(new Quiz { Id = 1, SectionId = 10 });
            context.Quizzes.Add(new Quiz { Id = 2, SectionId = 10 });
            await context.SaveChangesAsync();

            var repo = new Repository(context);

            // Act
            var result = await repo.CountQuizzesFromSectionAsync(10);

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public async Task CountQuizzesFromSectionAsync_ReturnsZero_WhenNoQuizzes()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            // Act
            var result = await repo.CountQuizzesFromSectionAsync(999);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task GetLastOrderNumberFromSectionAsync_ReturnsHighestOrderNumber()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Quizzes.Add(new Quiz { Id = 1, SectionId = 20, OrderNumber = 2 });
            context.Quizzes.Add(new Quiz { Id = 2, SectionId = 20, OrderNumber = 5 });
            context.Quizzes.Add(new Quiz { Id = 3, SectionId = 20, OrderNumber = 3 });
            await context.SaveChangesAsync();

            var repo = new Repository(context);

            // Act
            var result = await repo.GetLastOrderNumberFromSectionAsync(20);

            // Assert
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public async Task GetLastOrderNumberFromSectionAsync_ReturnsZero_WhenNoOrderNumbers()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Quizzes.Add(new Quiz { Id = 1, SectionId = 30 });
            await context.SaveChangesAsync();

            var repo = new Repository(context);

            // Act
            var result = await repo.GetLastOrderNumberFromSectionAsync(30);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task AddExercisesToQuizAsync_AddsExercises_WhenQuizExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var quiz = new Quiz { Id = 1, Exercises = new List<Exercise>() };
            var exercise1 = new AssociationExercise
            {
                ExerciseId = 1,
                Question = "Match A",
                Difficulty = Difficulty.Easy,
                FirstAnswersList = ["A1"],
                SecondAnswersList = ["A2"]
            };
            var exercise2 = new AssociationExercise
            {
                ExerciseId = 2,
                Question = "Match B",
                Difficulty = Difficulty.Normal,
                FirstAnswersList = ["B1"],
                SecondAnswersList = ["B2"]
            };

            context.Quizzes.Add(quiz);
            context.Exercises.AddRange(exercise1, exercise2);
            await context.SaveChangesAsync();

            var repo = new Repository(context);

            // Act
            await repo.AddExercisesToQuizAsync(1, new List<int> { 1, 2 });

            // Assert
            var updatedQuiz = await context.Quizzes.Include(q => q.Exercises).FirstAsync(q => q.Id == 1);
            Assert.AreEqual(2, updatedQuiz.Exercises.Count);
        }

        [TestMethod]
        public async Task AddExercisesToQuizAsync_DoesNothing_WhenQuizNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            // Act
            await repo.AddExercisesToQuizAsync(99, new List<int> { 1, 2 }); // QuizId does not exist

            // Assert
            // No exception should occur and database remains empty
            Assert.AreEqual(0, context.Quizzes.Count());
        }

        [TestMethod]
        public async Task AddExerciseToQuizAsync_AddsSingleExercise()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var quiz = new Quiz { Id = 1, Exercises = new List<Exercise>() };
            var exercise = new AssociationExercise
            {
                ExerciseId = 1,
                Question = "Match C",
                Difficulty = Difficulty.Easy,
                FirstAnswersList = ["C1"],
                SecondAnswersList = ["C2"]
            };

            context.Quizzes.Add(quiz);
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();

            var repo = new Repository(context);

            // Act
            await repo.AddExerciseToQuizAsync(1, 1);

            // Assert
            var updatedQuiz = await context.Quizzes.Include(q => q.Exercises).FirstAsync(q => q.Id == 1);
            Assert.AreEqual(1, updatedQuiz.Exercises.Count);
        }

        [TestMethod]
        public async Task AddExerciseToQuizAsync_DoesNothing_WhenQuizOrExerciseNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            // Act
            await repo.AddExerciseToQuizAsync(999, 999); // Nonexistent quiz and exercise

            // Assert
            // Should not throw or modify database
            Assert.AreEqual(0, context.Quizzes.Count());
        }

        [TestMethod]
        public async Task RemoveExerciseFromQuizAsync_RemovesExercise_WhenExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var exercise = new AssociationExercise
            {
                ExerciseId = 1,
                Question = "Match D",
                Difficulty = Difficulty.Hard,
                FirstAnswersList = ["D1"],
                SecondAnswersList = ["D2"]
            };

            var quiz = new Quiz
            {
                Id = 1,
                Exercises = new List<Exercise> { exercise }
            };

            context.Quizzes.Add(quiz);
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();

            var repo = new Repository(context);

            // Act
            await repo.RemoveExerciseFromQuizAsync(1, 1);

            // Assert
            var updatedQuiz = await context.Quizzes.Include(q => q.Exercises).FirstAsync(q => q.Id == 1);
            Assert.AreEqual(0, updatedQuiz.Exercises.Count);
        }

        [TestMethod]
        public async Task RemoveExerciseFromQuizAsync_DoesNothing_WhenQuizOrExerciseNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            // Act
            await repo.RemoveExerciseFromQuizAsync(999, 999);

            // Assert
            Assert.AreEqual(0, context.Quizzes.Count());
        }

        [TestMethod]
        public async Task SaveQuizSubmissionAsync_SavesSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            var submission = new QuizSubmissionEntity
            {
                QuizId = 1,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(10),
                Answers = new List<AnswerSubmissionEntity>
            {
                new AnswerSubmissionEntity
                {
                    QuestionId = 101,
                    SelectedOptionIndex = 2,
                    IsCorrect = true
                }
            }
            };

            // Act
            await repo.SaveQuizSubmissionAsync(submission);

            // Assert
            var saved = await context.QuizSubmissions.Include(q => q.Answers).FirstOrDefaultAsync();
            Assert.IsNotNull(saved);
            Assert.AreEqual(1, saved.QuizId);
            Assert.AreEqual(1, saved.Answers.Count);
            Assert.AreEqual(101, saved.Answers.First().QuestionId);
        }

        [TestMethod]
        public async Task GetSubmissionByQuizIdAsync_ReturnsSubmission_WhenExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var submission = new QuizSubmissionEntity
            {
                QuizId = 10,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(15),
                Answers = new List<AnswerSubmissionEntity>
            {
                new AnswerSubmissionEntity
                {
                    QuestionId = 200,
                    SelectedOptionIndex = 1,
                    IsCorrect = false
                }
            }
            };

            context.QuizSubmissions.Add(submission);
            await context.SaveChangesAsync();

            var repo = new Repository(context);

            // Act
            var result = await repo.GetSubmissionByQuizIdAsync(10);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.QuizId);
            Assert.AreEqual(1, result.Answers.Count);
            Assert.AreEqual(200, result.Answers.First().QuestionId);
        }

        [TestMethod]
        public async Task GetSubmissionByQuizIdAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            // Act
            var result = await repo.GetSubmissionByQuizIdAsync(999); // Non-existent quiz

            // Assert
            Assert.IsNull(result);
        }

        #endregion

        #region Course
        [TestMethod]
        public async Task GetCoursesFromDbAsync_ReturnsAllCourses()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var course1 = new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            var course2 = new Course
            {
                CourseId = 2,
                Title = "Course 2",
                Description = "Description of Course 2",
                IsPremium = true,
                Cost = 200,
                ImageUrl = "http://example.com/course2.jpg",
                TimeToComplete = 7200,
                Difficulty = "Intermediate"
            };
            context.Courses.Add(course1);
            context.Courses.Add(course2);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var courses = await repository.GetCoursesFromDbAsync();

            // Assert
            Assert.AreEqual(2, courses.Count);
        }

        [TestMethod]
        public async Task GetCourseByIdAsync_ReturnsCorrectCourse()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var course = new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetCourseByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual("Course 1", result.Title);
        }

        [TestMethod]
        public async Task AddCourseAsync_AddsCourseToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var course = new Course
            {
                CourseId = 1,
                Title = "New Course",
                Description = "New Course Description",
                IsPremium = true,
                Cost = 250,
                ImageUrl = "http://example.com/newcourse.jpg",
                TimeToComplete = 5000,
                Difficulty = "Advanced"
            };
            var repository = new Repository(context);

            // Act
            await repository.AddCourseAsync(course);

            // Assert
            var addedCourse = await context.Courses.FindAsync(1);
            Assert.IsNotNull(addedCourse);
            Assert.AreEqual(1, addedCourse?.CourseId);
            Assert.AreEqual("New Course", addedCourse?.Title);
        }

        [TestMethod]
        public async Task UpdateCourseAsync_UpdatesCourseInDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var course = new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            course.Title = "Updated Course Title";
            await repository.UpdateCourseAsync(course);

            // Assert
            var updatedCourse = await context.Courses.FindAsync(1);
            Assert.AreEqual("Updated Course Title", updatedCourse?.Title);
        }

        [TestMethod]
        public async Task DeleteCourseAsync_DeletesCourseFromDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var course = new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteCourseAsync(1);

            // Assert
            var deletedCourse = await context.Courses.FindAsync(1);
            Assert.IsNull(deletedCourse);
        }
        #endregion

        #region CourseCompletion
        [TestMethod]
        public async Task EnrollUserInCourseAsync_EnrollsUserIfNotAlreadyEnrolled()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, UserName = "Test User", CoinBalance = 100, LastActivityDate = DateTime.Now };
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Test Course Description",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            context.Users.Add(user);
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.EnrollUserInCourseAsync(user.UserId, course.CourseId);

            // Assert
            var courseCompletion = await context.Enrollments
                .FirstOrDefaultAsync(cc => cc.UserId == user.UserId && cc.CourseId == course.CourseId);
            Assert.IsNotNull(courseCompletion);
            Assert.AreEqual(false, courseCompletion.IsCompleted);
        }

        [TestMethod]
        public async Task IsUserEnrolledInCourseAsync_ReturnsTrueIfUserIsEnrolled()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, UserName = "Test User", CoinBalance = 100, LastActivityDate = DateTime.Now };
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Test Course Description",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            context.Users.Add(user);
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var repository = new Repository(context);
            await repository.EnrollUserInCourseAsync(user.UserId, course.CourseId);

            // Act
            var result = await repository.IsUserEnrolledInCourseAsync(user.UserId, course.CourseId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsCourseCompletedAsync_ReturnsTrueIfCourseIsCompleted()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, UserName = "Test User", CoinBalance = 100, LastActivityDate = DateTime.Now };
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Test Course Description",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            var courseCompletion = new CourseCompletion
            {
                UserId = user.UserId,
                CourseId = course.CourseId,
                CompletionRewardClaimed = false,
                TimedRewardClaimed = false,
                CompletedAt = DateTime.Now
            };
            context.Users.Add(user);
            context.Courses.Add(course);
            context.CourseCompletions.Add(courseCompletion);
            context.Enrollments.Add(new Enrollment { CourseId = course.CourseId, UserId = user.UserId, IsCompleted = true });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.IsCourseCompletedAsync(user.UserId, course.CourseId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsCourseCompletedAsync_ReturnsFalse_WhenCompletionExistsButNotCompleted()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            var userId = 2;
            var courseId = 101;

            context.CourseCompletions.Add(new CourseCompletion
            {
                UserId = userId,
                CourseId = courseId,
                CompletedAt = DateTime.MinValue
            });
            await context.SaveChangesAsync();

            // Act
            var result = await repo.IsCourseCompletedAsync(userId, courseId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsCourseCompletedAsync_ReturnsFalse_WhenNoCompletionRecordExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            var userId = 3;
            var courseId = 102;

            // No CourseCompletion added

            // Act
            var result = await repo.IsCourseCompletedAsync(userId, courseId);

            // Assert
            Assert.IsFalse(result);
        }


        [TestMethod]
        public async Task UpdateTimeSpentAsync_UpdatesTimeSpentForUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, UserName = "Test User", CoinBalance = 100, LastActivityDate = DateTime.Now, NumberOfCompletedSections = 0 };
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Description of test course",
                IsPremium = true,
                Cost = 100,
                ImageUrl = "http://example.com/image.jpg",
                TimeToComplete = 120,
                Difficulty = "Intermediate"
            };
            context.Courses.Add(course);
            context.Enrollments.Add(new Enrollment { UserId = user.UserId, CourseId = course.CourseId});
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.UpdateTimeSpentAsync(user.UserId, 1, 500);

            // Assert
            var updatedTime = await repository.GetTimeSpentAsync(user.UserId, course.CourseId);
            Assert.AreEqual(500, updatedTime);
        }

        [TestMethod]
        public async Task ClaimCompletionRewardAsync_ClaimsRewardSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, UserName = "Test User", CoinBalance = 100, LastActivityDate = DateTime.Now };
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Test Course Description",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            var courseCompletion = new CourseCompletion
            {
                UserId = user.UserId,
                CourseId = course.CourseId,
                CompletionRewardClaimed = false,
                TimedRewardClaimed = false,
                CompletedAt = DateTime.Now
            };
            context.Users.Add(user);
            context.Courses.Add(course);
            context.CourseCompletions.Add(courseCompletion);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.ClaimCompletionRewardAsync(user.UserId, course.CourseId, user.CoinBalance);

            // Assert
            var updatedCompletion = await context.CourseCompletions
                .FirstOrDefaultAsync(cc => cc.UserId == user.UserId && cc.CourseId == course.CourseId);
            Assert.IsTrue(updatedCompletion?.CompletionRewardClaimed);
        }

        [TestMethod]
        public async Task ClaimCompletionRewardAsync_DoesNotClaim_WhenAlreadyClaimed()
        {
            var context = GetInMemoryDbContext();
            var service = new Repository(context);

            context.CourseCompletions.Add(new CourseCompletion
            {
                UserId = 1,
                CourseId = 10,
                CompletionRewardClaimed = true
            });
            await context.SaveChangesAsync();

            await service.ClaimCompletionRewardAsync(1, 10, 0);

            var unchanged = await context.CourseCompletions.FirstAsync();
            Assert.IsTrue(unchanged.CompletionRewardClaimed); // unchanged
        }

        [TestMethod]
        public async Task ClaimCompletionRewardAsync_DoesNothing_WhenNoCompletion()
        {
            var context = GetInMemoryDbContext();
            var service = new Repository(context);

            // No CourseCompletion added
            await service.ClaimCompletionRewardAsync(2, 20, 0);

            Assert.AreEqual(0, context.CourseCompletions.Count());
        }


        [TestMethod]
        public async Task ClaimTimeRewardAsync_ClaimsTimeRewardSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, UserName = "Test User", CoinBalance = 100, LastActivityDate = DateTime.Now };
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Test Course Description",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            var courseCompletion = new CourseCompletion
            {
                UserId = user.UserId,
                CourseId = course.CourseId,
                CompletionRewardClaimed = false,
                TimedRewardClaimed = false,
                CompletedAt = DateTime.Now
            };
            context.Users.Add(user);
            context.Courses.Add(course);
            context.CourseCompletions.Add(courseCompletion);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.ClaimTimeRewardAsync(user.UserId, course.CourseId, user.CoinBalance);

            // Assert
            var updatedCompletion = await context.CourseCompletions
                .FirstOrDefaultAsync(cc => cc.UserId == user.UserId && cc.CourseId == course.CourseId);
            Assert.IsTrue(updatedCompletion?.TimedRewardClaimed);
        }

        [TestMethod]
        public async Task ClaimTimeRewardAsync_DoesNotClaim_WhenAlreadyClaimed()
        {
            var context = GetInMemoryDbContext();
            var service = new Repository(context);

            context.CourseCompletions.Add(new CourseCompletion
            {
                UserId = 1,
                CourseId = 11,
                TimedRewardClaimed = true
            });
            await context.SaveChangesAsync();

            await service.ClaimTimeRewardAsync(1, 11, 0);

            var result = await context.CourseCompletions.FirstAsync();
            Assert.IsTrue(result.TimedRewardClaimed);
        }


        [TestMethod]
        public async Task GetTimeSpentAsync_ReturnsTimeSpent()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, NumberOfCompletedSections = 5 };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetTimeSpentAsync(1, 1);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task GetTimeSpentAsync_ReturnsZero_WhenUserDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            // Act
            var result = await repository.GetTimeSpentAsync(999, 1); // User ID that doesn't exist

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task GetCourseTimeLimitAsync_ReturnsTimeToComplete_WhenCourseExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Description of test course",
                IsPremium = true,
                Cost = 100,
                ImageUrl = "http://example.com/image.jpg",
                TimeToComplete = 120,
                Difficulty = "Intermediate"
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetCourseTimeLimitAsync(1);

            // Assert
            Assert.AreEqual(120, result);
        }

        [TestMethod]
        public async Task GetCourseTimeLimitAsync_ReturnsZero_WhenCourseDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            // Act
            var result = await repository.GetCourseTimeLimitAsync(999); // Course ID that doesn't exist

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task GetFilteredCoursesAsync_ReturnsCoursesThatMatchSearchText()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Courses.Add(new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = true,
                Cost = 100,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 120,
                Difficulty = "Beginner"
            });
            context.Courses.Add(new Course
            {
                CourseId = 2,
                Title = "Course 2",
                Description = "Description of Course 2",
                IsPremium = false,
                Cost = 50,
                ImageUrl = "http://example.com/course2.jpg",
                TimeToComplete = 90,
                Difficulty = "Intermediate"
            });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetFilteredCoursesAsync("Course 1", false, false, false, false, 0);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Course 1", result.First().Title);
        }

        [TestMethod]
        public async Task GetFilteredCoursesAsync_ReturnsNonEnrolledCourses_WhenFilterNotEnrolledIsTrue()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1 };
            var course1 = new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = false,
                Cost = 50,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 120,
                Difficulty = "Beginner"
            };
            var course2 = new Course
            {
                CourseId = 2,
                Title = "Course 2",
                Description = "Description of Course 2",
                IsPremium = true,
                Cost = 200,
                ImageUrl = "http://example.com/course2.jpg",
                TimeToComplete = 150,
                Difficulty = "Advanced"
            };
            context.Users.Add(user);
            context.Courses.Add(course1);
            context.Courses.Add(course2);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Simulate that the user is enrolled in course 1
            context.CourseCompletions.Add(new CourseCompletion { UserId = 1, CourseId = 1 });
            await context.SaveChangesAsync();

            context.Enrollments.Add(new Enrollment { UserId = 1, CourseId = 1 });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetFilteredCoursesAsync("", false, false, false, true, 1);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Course 2", result.First().Title);
        }

        [TestMethod]
        public async Task GetFilteredCoursesAsync_ReturnsAllCourses_WhenNoFiltersAreApplied()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Courses.AddRange(
                new Course
                {
                    CourseId = 1,
                    Title = "Course 1",
                    Description = "Description of Course 1",
                    IsPremium = true,
                    Cost = 100,
                    ImageUrl = "http://example.com/course1.jpg",
                    TimeToComplete = 120,
                    Difficulty = "Beginner"
                },
                new Course
                {
                    CourseId = 2,
                    Title = "Course 2",
                    Description = "Description of Course 2",
                    IsPremium = false,
                    Cost = 50,
                    ImageUrl = "http://example.com/course2.jpg",
                    TimeToComplete = 90,
                    Difficulty = "Intermediate"
                }
            );
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetFilteredCoursesAsync("", false, false, false, false, 0);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetFilteredCoursesAsync_ReturnsPremiumCourses_WhenFilterPremiumIsTrue()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Courses.AddRange(
                new Course
                {
                    CourseId = 1,
                    Title = "Premium Course",
                    Description = "Description of Premium Course",
                    IsPremium = true,
                    Cost = 200,
                    ImageUrl = "http://example.com/premium.jpg",
                    TimeToComplete = 150,
                    Difficulty = "Advanced"
                },
                new Course
                {
                    CourseId = 2,
                    Title = "Free Course",
                    Description = "Description of Free Course",
                    IsPremium = false,
                    Cost = 0,
                    ImageUrl = "http://example.com/free.jpg",
                    TimeToComplete = 60,
                    Difficulty = "Beginner"
                }
            );
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetFilteredCoursesAsync("", true, false, false, false, 0);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Premium Course", result.First().Title);
        }

        [TestMethod]
        public async Task GetFilteredCoursesAsync_ReturnsFreeCourses_WhenFilterFreeIsTrue()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Courses.AddRange(
                new Course
                {
                    CourseId = 1,
                    Title = "Free Course 1",
                    Description = "Description of Free Course 1",
                    IsPremium = false,
                    Cost = 0,
                    ImageUrl = "http://example.com/free1.jpg",
                    TimeToComplete = 60,
                    Difficulty = "Beginner"
                },
                new Course
                {
                    CourseId = 2,
                    Title = "Premium Course",
                    Description = "Description of Premium Course",
                    IsPremium = true,
                    Cost = 200,
                    ImageUrl = "http://example.com/premium.jpg",
                    TimeToComplete = 120,
                    Difficulty = "Advanced"
                }
            );
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetFilteredCoursesAsync("", false, true, false, false, 0);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Free Course 1", result.First().Title);
        }

        [TestMethod]
        public async Task GetFilteredCoursesAsync_ReturnsAllCourses_WhenFilterFreeAndPremiumAreTrue()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Courses.AddRange(
                new Course
                {
                    CourseId = 1,
                    Title = "Free Course 1",
                    Description = "Description of Free Course 1",
                    IsPremium = false,
                    Cost = 0,
                    ImageUrl = "http://example.com/free1.jpg",
                    TimeToComplete = 60,
                    Difficulty = "Beginner"
                },
                new Course
                {
                    CourseId = 2,
                    Title = "Premium Course",
                    Description = "Description of Premium Course",
                    IsPremium = true,
                    Cost = 200,
                    ImageUrl = "http://example.com/premium.jpg",
                    TimeToComplete = 120,
                    Difficulty = "Advanced"
                }
            );
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetFilteredCoursesAsync("", true, true, false, false, 0);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetFilteredCoursesAsync_ReturnsEnrolledCourses_WhenFilterEnrolledIsTrue()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1 };
            var course1 = new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = false,
                Cost = 50,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 120,
                Difficulty = "Beginner"
            };
            var course2 = new Course
            {
                CourseId = 2,
                Title = "Course 2",
                Description = "Description of Course 2",
                IsPremium = true,
                Cost = 200,
                ImageUrl = "http://example.com/course2.jpg",
                TimeToComplete = 150,
                Difficulty = "Advanced"
            };
            context.Users.Add(user);
            context.Courses.AddRange(course1, course2);
            await context.SaveChangesAsync();

            // Simulate enrollment
            context.CourseCompletions.Add(new CourseCompletion { UserId = 1, CourseId = 1 });
            await context.SaveChangesAsync();

            context.Enrollments.Add(new Enrollment { UserId = 1, CourseId = 1});
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetFilteredCoursesAsync("", false, false, true, false, 1);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Course 1", result.First().Title);
        }

        [TestMethod]
        public async Task GetFilteredCoursesAsync_ReturnsAllCourses_WhenFilterEnrolledAndNotEnrolledAreTrue()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1 };
            var course1 = new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = false,
                Cost = 50,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 120,
                Difficulty = "Beginner"
            };
            var course2 = new Course
            {
                CourseId = 2,
                Title = "Course 2",
                Description = "Description of Course 2",
                IsPremium = true,
                Cost = 200,
                ImageUrl = "http://example.com/course2.jpg",
                TimeToComplete = 150,
                Difficulty = "Advanced"
            };
            context.Users.Add(user);
            context.Courses.AddRange(course1, course2);
            await context.SaveChangesAsync();

            // Simulate enrollment
            context.CourseCompletions.Add(new CourseCompletion { UserId = 1, CourseId = 1 });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetFilteredCoursesAsync("", false, false, true, true, 1);

            // Assert
            Assert.AreEqual(2, result.Count);
        }


        [TestMethod]
        public async Task GetFilteredCoursesAsync_ReturnsCoursesWithFiltersAndSearchText()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Courses.AddRange(
                new Course
                {
                    CourseId = 1,
                    Title = "Advanced Premium Course",
                    Description = "Advanced Course",
                    IsPremium = true,
                    Cost = 150,
                    ImageUrl = "http://example.com/advanced.jpg",
                    TimeToComplete = 180,
                    Difficulty = "Advanced"
                },
                new Course
                {
                    CourseId = 2,
                    Title = "Beginner Free Course",
                    Description = "Beginner Course",
                    IsPremium = false,
                    Cost = 0,
                    ImageUrl = "http://example.com/beginner.jpg",
                    TimeToComplete = 60,
                    Difficulty = "Beginner"
                }
            );
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetFilteredCoursesAsync("Advanced", true, false, false, false, 0);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Advanced Premium Course", result.First().Title);
        }
        #endregion

        #region Exam
        [TestMethod]
        public async Task GetExamsFromDbAsync_ReturnsAllExams()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Exams.Add(new Exam { Id = 1, SectionId = 1 });
            context.Exams.Add(new Exam { Id = 2, SectionId = 2 });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var exams = await repository.GetExamsFromDbAsync();

            // Assert
            Assert.AreEqual(2, exams.Count);
            Assert.IsTrue(exams.Any(e => e.Id == 1));
            Assert.IsTrue(exams.Any(e => e.Id == 2));
        }

        [TestMethod]
        public async Task GetExamByIdAsync_ReturnsExam_WhenExamExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exam = new Exam { Id = 1, SectionId = 1 };
            context.Exams.Add(exam);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetExamByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task AddExamAsync_AddsExamSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exam = new Exam { SectionId = 1 };
            var repository = new Repository(context);

            // Act
            await repository.AddExamAsync(exam);
            var result = await context.Exams.FindAsync(exam.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exam.SectionId, result?.SectionId);
        }

        [TestMethod]
        public async Task UpdateExamAsync_UpdatesExamSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exam = new Exam { SectionId = 1 };
            context.Exams.Add(exam);
            await context.SaveChangesAsync();
            var repository = new Repository(context);

            // Act
            exam.SectionId = 2;
            await repository.UpdateExamAsync(exam);
            var updatedExam = await context.Exams.FindAsync(exam.Id);

            // Assert
            Assert.IsNotNull(updatedExam);
            Assert.AreEqual(2, updatedExam?.SectionId);
        }

        [TestMethod]
        public async Task DeleteExamAsync_DeletesExamSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exam = new Exam { SectionId = 1 };
            context.Exams.Add(exam);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteExamAsync(exam.Id);
            var deletedExam = await context.Exams.FindAsync(exam.Id);

            // Assert
            Assert.IsNull(deletedExam);
        }

        [TestMethod]
        public async Task GetExamFromSectionAsync_ReturnsExam_WhenExamExistsForSection()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var exam = new Exam
            {
                Id = 1,
                SectionId = 10,
                Exercises = new List<Exercise>
            {
                new AssociationExercise
                {
                    ExerciseId = 1,
                    Question = "Match countries with capitals",
                    Difficulty = Difficulty.Easy,
                    FirstAnswersList = new List<string> { "France", "Germany" },
                    SecondAnswersList = new List<string> { "Paris", "Berlin" }
                }
            }
            };

            context.Exams.Add(exam);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetExamFromSectionAsync(10);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.SectionId);
            Assert.AreEqual(1, result.Exercises.Count);
            Assert.IsInstanceOfType(result.Exercises.First(), typeof(AssociationExercise));
        }

        [TestMethod]
        public async Task GetExamFromSectionAsync_ReturnsNull_WhenNoExamForSection()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            // Act
            var result = await repository.GetExamFromSectionAsync(99); // SectionId that doesn't exist

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAvailableExamsAsync_ReturnsExamsWithNullSectionId()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var availableExam = new Exam
            {
                Id = 1,
                SectionId = null,
                Exercises = new List<Exercise>
            {
                new AssociationExercise
                {
                    ExerciseId = 2,
                    Question = "Match fruits with colors",
                    Difficulty = Difficulty.Normal,
                    FirstAnswersList = new List<string> { "Apple", "Banana" },
                    SecondAnswersList = new List<string> { "Red", "Yellow" }
                }
            }
            };

            var unavailableExam = new Exam
            {
                Id = 2,
                SectionId = 20,
                Exercises = new List<Exercise>
            {
                new AssociationExercise
                {
                    ExerciseId = 3,
                    Question = "Match animals with sounds",
                    Difficulty = Difficulty.Hard,
                    FirstAnswersList = new List<string> { "Dog", "Cat" },
                    SecondAnswersList = new List<string> { "Bark", "Meow" }
                }
            }
            };

            context.Exams.AddRange(availableExam, unavailableExam);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetAvailableExamsAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsNull(result.First().SectionId);
            Assert.AreEqual(1, result.First().Exercises.Count);
            Assert.IsInstanceOfType(result.First().Exercises.First(), typeof(AssociationExercise));
        }

        [TestMethod]
        public async Task GetAvailableExamsAsync_ReturnsEmptyList_WhenNoAvailableExams()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            // Act
            var result = await repository.GetAvailableExamsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
        #endregion

        #region Section
        [TestMethod]
        public async Task GetSectionsFromDbAsync_ReturnsAllSections()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Sections.Add(new Section { Id = 1, Title = "Section 1", Description = "Description 1", RoadmapId = 1 });
            context.Sections.Add(new Section { Id = 2, Title = "Section 2", Description = "Description 2", RoadmapId = 1 });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var sections = await repository.GetSectionsFromDbAsync();

            // Assert
            Assert.AreEqual(2, sections.Count);
            Assert.IsTrue(sections.Any(s => s.Id == 1));
            Assert.IsTrue(sections.Any(s => s.Id == 2));
        }

        [TestMethod]
        public async Task GetSectionByIdAsync_ReturnsSection_WhenSectionExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section 1", Description = "Description 1", RoadmapId = 1 };
            context.Sections.Add(section);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetSectionByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Section 1", result.Title);
        }

        [TestMethod]
        public async Task AddSectionAsync_AddsSectionSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Title = "New Section", Description = "Description of new section", RoadmapId = 1 };
            var repository = new Repository(context);

            // Act
            await repository.AddSectionAsync(section);
            var result = await context.Sections.FindAsync(section.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("New Section", result?.Title);
            Assert.AreEqual("Description of new section", result?.Description);
        }

        [TestMethod]
        public async Task UpdateSectionAsync_UpdatesSectionSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Old Section", Description = "Old Description", RoadmapId = 1 };
            context.Sections.Add(section);
            await context.SaveChangesAsync();
            var repository = new Repository(context);

            // Act
            section.Title = "Updated Section";  // Update title
            section.Description = "Updated Description";  // Update description
            await repository.UpdateSectionAsync(section);
            var updatedSection = await context.Sections.FindAsync(section.Id);

            // Assert
            Assert.IsNotNull(updatedSection);
            Assert.AreEqual("Updated Section", updatedSection?.Title);
            Assert.AreEqual("Updated Description", updatedSection?.Description);
        }

        [TestMethod]
        public async Task DeleteSectionAsync_DeletesSectionSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section to Delete", Description = "Description to Delete", RoadmapId = 1 };
            context.Sections.Add(section);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteSectionAsync(section.Id);
            var deletedSection = await context.Sections.FindAsync(section.Id);

            // Assert
            Assert.IsNull(deletedSection);
        }
        #endregion

        #region Coins
        [TestMethod]
        public async Task GetUserCoinBalanceAsync_ReturnsCorrectCoinBalance()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, CoinBalance = 100 };
            var context = GetInMemoryDbContext();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repository = new Repository(context);

            // Act
            var coinBalance = await repository.GetUserCoinBalanceAsync(userId);

            // Assert
            Assert.AreEqual(100, coinBalance);
        }

        [TestMethod]
        public async Task GetUserCoinBalanceAsync_ReturnsZero_WhenUserDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            // Act
            var result = await repo.GetUserCoinBalanceAsync(999);

            // Assert
            Assert.AreEqual(0, result);
        }


        [TestMethod]
        public async Task TryDeductCoinsFromUserWalletAsync_SuccessfulDeduction()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, CoinBalance = 100 };
            var cost = 50;
            var context = GetInMemoryDbContext();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repository = new Repository(context);

            // Act
            var result = await repository.TryDeductCoinsFromUserWalletAsync(userId, cost);

            // Assert
            Assert.IsTrue(result);
            var updatedUser = await context.Users.FindAsync(userId);
            Assert.AreEqual(50, updatedUser?.CoinBalance);
        }

        [TestMethod]
        public async Task TryDeductCoinsFromUserWalletAsync_InsufficientFunds()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, CoinBalance = 30 };
            var cost = 50;
            var context = GetInMemoryDbContext();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repository = new Repository(context);

            // Act
            var result = await repository.TryDeductCoinsFromUserWalletAsync(userId, cost);

            // Assert
            Assert.IsFalse(result);
            var updatedUser = await context.Users.FindAsync(userId);
            Assert.AreEqual(30, updatedUser?.CoinBalance);
        }

        [TestMethod]
        public async Task TryDeductCoinsFromUserWalletAsync_ReturnsFalse_WhenUserNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new Repository(context);

            // No user is added to the context

            // Act
            var success = await repo.TryDeductCoinsFromUserWalletAsync(999, 10);

            // Assert
            Assert.IsFalse(success);
        }


        [TestMethod]
        public async Task AddCoinsToUserWalletAsync_AddsCoinsSuccessfully()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, CoinBalance = 100 };
            var amountToAdd = 50;
            var context = GetInMemoryDbContext();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repository = new Repository(context);

            // Act
            await repository.AddCoinsToUserWalletAsync(userId, amountToAdd);
            var updatedUser = await context.Users.FindAsync(userId);

            // Assert
            Assert.AreEqual(150, updatedUser?.CoinBalance);
        }

        [TestMethod]
        public async Task GetUserLastLoginTimeAsync_ReturnsCorrectLastLoginTime()
        {
            // Arrange
            var userId = 1;
            var lastLoginTime = DateTime.Now.AddDays(-1);
            var user = new User { UserId = userId, LastActivityDate = lastLoginTime };
            var context = GetInMemoryDbContext();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repository = new Repository(context);

            // Act
            var result = await repository.GetUserLastLoginTimeAsync(userId);

            // Assert
            Assert.AreEqual(lastLoginTime, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Database context is not initialized.")]
        public async Task GetUserLastLoginTimeAsync_ThrowsException_WhenContextIsNull()
        {
            // Arrange
            var userId = 1;
            var repository = new Repository(null); // Passing null as context

            // Act
            await repository.GetUserLastLoginTimeAsync(userId);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "User with ID 999 not found")]
        public async Task GetUserLastLoginTimeAsync_ThrowsException_WhenUserNotFound()
        {
            // Arrange
            var userId = 999; // Assuming no user with this ID exists
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            // Act
            await repository.GetUserLastLoginTimeAsync(userId);  // Should throw exception
        }

        [TestMethod]
        public async Task UpdateUserLastLoginTimeToNowAsync_UpdatesLastLoginTimeSuccessfully()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, LastActivityDate = DateTime.Now.AddDays(-1) };
            var context = GetInMemoryDbContext();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repository = new Repository(context);

            // Act
            await repository.UpdateUserLastLoginTimeToNowAsync(userId);
            var updatedUser = await context.Users.FindAsync(userId);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(DateTime.Now.Date, updatedUser?.LastActivityDate?.Date); // Date part should match, time will be different
        }
        #endregion

    }
}