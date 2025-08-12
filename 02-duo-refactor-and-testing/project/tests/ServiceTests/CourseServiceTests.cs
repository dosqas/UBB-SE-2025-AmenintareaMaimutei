// <copyright file="CourseServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
#pragma warning disable IDE0028
#pragma warning disable CA1822

namespace Tests.ServiceTests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using CourseApp.Models;
    using CourseApp.Repository;
    using CourseApp.Services;
    using Moq;
    using Xunit;

    /// <summary>
    /// <see cref="CourseServiceTests"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CourseServiceTests
    {
        private const int UserId = 0;
        private readonly Mock<ICourseRepository> mockRepository;
        private readonly Mock<ICoinsRepository> mockCoinsRepository;
        private readonly CourseService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseServiceTests"/> class.
        /// </summary>
        public CourseServiceTests()
        {
            this.mockRepository = new Mock<ICourseRepository>();
            this.mockCoinsRepository = new Mock<ICoinsRepository>();
            this.service = new CourseService(this.mockRepository.Object, this.mockCoinsRepository.Object);
        }

        /// <summary>
        /// Tests that GetCourses returns all courses from the repository.
        /// </summary>
        [Fact]
        public void GetCourses_ReturnsAllCoursesFromRepository()
        {
            // Arrange
            var expectedCourses = new List<Course>
            {
                this.CreateTestCourse(1, "Course 1"),
                this.CreateTestCourse(2, "Course 2"),
            };
            this.mockRepository.Setup(r => r.GetAllCourses()).Returns(expectedCourses);

            // Act
            var result = this.service.GetCourses();

            // Assert
            Assert.Equal(expectedCourses, result);
            this.mockRepository.Verify(r => r.GetAllCourses(), Times.Once);
        }

        /// <summary>
        /// Tests that GetFilteredCourses filters courses by title when search text is provided.
        /// </summary>
        [Fact]
        public void GetFilteredCoursesWithSearchTextFiltersByTitle()
        {
            // Arrange
            var courses = new List<Course>
            {
                this.CreateTestCourse(1, "C# Programming"),
                this.CreateTestCourse(2, "Python Basics"),
                this.CreateTestCourse(3, "Advanced C#"),
            };
            this.mockRepository.Setup(r => r.GetAllCourses()).Returns(courses);

            // Act
            var result = this.service.GetFilteredCourses("C#", false, false, false, false, []);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Title == "C# Programming");
            Assert.Contains(result, c => c.Title == "Advanced C#");
        }

        /// <summary>
        /// Tests that GetFilteredCourses returns only free courses when free filter is enabled.
        /// </summary>
        [Fact]
        public void GetFilteredCourses_WithFreeFilter_ReturnsOnlyFreeCourses()
        {
            // Arrange
            var courses = new List<Course>
    {
        this.CreateTestCourse(1, "Free Course", false),
        this.CreateTestCourse(2, "Premium Course", true, 100),
    };
            this.mockRepository.Setup(r => r.GetAllCourses()).Returns(courses);

            // Act
            var result = this.service.GetFilteredCourses(string.Empty, false, true, false, false, []);

            // Assert
            Assert.Single(result);
            Assert.Equal("Free Course", result[0].Title);
            Assert.False(result[0].IsPremium);
        }

        /// <summary>
        /// Tests that GetFilteredCourses returns only not-enrolled courses when not-enrolled filter is enabled.
        /// </summary>
        [Fact]
        public void GetFilteredCourses_WithNotEnrolledFilter_ReturnsOnlyNotEnrolledCourses()
        {
            // Arrange
            var courses = new List<Course>
    {
        this.CreateTestCourse(1, "Course 1"),
        this.CreateTestCourse(2, "Course 2"),
    };
            this.mockRepository.Setup(r => r.GetAllCourses()).Returns(courses);
            this.mockRepository.Setup(r => r.IsUserEnrolled(UserId, 1)).Returns(true);
            this.mockRepository.Setup(r => r.IsUserEnrolled(UserId, 2)).Returns(false);

            // Act
            var result = this.service.GetFilteredCourses(string.Empty, false, false, false, true, []);

            // Assert
            Assert.Single(result);
            Assert.Equal("Course 2", result[0].Title);
        }

        /// <summary>
        /// Tests that GetFilteredCourses returns empty list when both enrolled and not-enrolled filters are applied.
        /// </summary>
        [Fact]
        public void GetFilteredCourses_WithBothEnrollmentFilters_ReturnsEmptyList()
        {
            // Arrange
            var courses = new List<Course>
    {
        this.CreateTestCourse(1, "Course 1"),
        this.CreateTestCourse(2, "Course 2"),
    };
            this.mockRepository.Setup(r => r.GetAllCourses()).Returns(courses);

            // Act
            var result = this.service.GetFilteredCourses(string.Empty, false, false, true, true, new List<int>());

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests that GetFilteredCourses returns empty list when both premium and free filters are applied.
        /// </summary>
        [Fact]
        public void GetFilteredCourses_WithBothTypeFilters_ReturnsEmptyList()
        {
            // Arrange
            var courses = new List<Course>
    {
        this.CreateTestCourse(1, "Course 1", true),
        this.CreateTestCourse(2, "Course 2", false),
    };
            this.mockRepository.Setup(r => r.GetAllCourses()).Returns(courses);

            // Act
            var result = this.service.GetFilteredCourses(string.Empty, true, true, false, false, new List<int>());

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests that GetFilteredCourses returns only premium courses when premium filter is enabled.
        /// </summary>
        [Fact]
        public void GetFilteredCourses_WithPremiumFilter_ReturnsOnlyPremiumCourses()
        {
            // Arrange
            var courses = new List<Course>
            {
                this.CreateTestCourse(1, "Free Course", false),
                this.CreateTestCourse(2, "Premium Course", true, 100),
            };
            this.mockRepository.Setup(r => r.GetAllCourses()).Returns(courses);

            // Act
            var result = this.service.GetFilteredCourses(string.Empty, true, false, false, false, new List<int>());

            // Assert
            Assert.Single(result);
            Assert.Equal("Premium Course", result[0].Title);
        }

        /// <summary>
        /// Tests that GetFilteredCourses returns only enrolled courses when enrolled filter is enabled.
        /// </summary>
        [Fact]
        public void GetFilteredCourses_WithEnrolledFilter_ReturnsOnlyEnrolledCourses()
        {
            // Arrange
            var courses = new List<Course>
            {
                this.CreateTestCourse(1, "Course 1"),
                this.CreateTestCourse(2, "Course 2"),
            };
            this.mockRepository.Setup(r => r.GetAllCourses()).Returns(courses);
            this.mockRepository.Setup(r => r.IsUserEnrolled(UserId, 1)).Returns(true);
            this.mockRepository.Setup(r => r.IsUserEnrolled(UserId, 2)).Returns(false);

            // Act
            var result = this.service.GetFilteredCourses(string.Empty, false, false, true, false, new List<int>());

            // Assert
            Assert.Single(result);
            Assert.Equal("Course 1", result[0].Title);
        }

        /// <summary>
        /// Tests that GetFilteredCourses filters courses by all selected tags.
        /// </summary>
        [Fact]
        public void GetFilteredCourses_WithTags_FiltersByAllSelectedTags()
        {
            // Arrange
            var courses = new List<Course>
            {
                this.CreateTestCourse(1, "Course 1"),
                this.CreateTestCourse(2, "Course 2"),
            };
            var tags1 = new List<Tag> { this.CreateTestTag(1, "Programming"), this.CreateTestTag(2, "C#") };
            var tags2 = new List<Tag> { this.CreateTestTag(1, "Programming") };

            this.mockRepository.Setup(r => r.GetAllCourses()).Returns(courses);
            this.mockRepository.Setup(r => r.GetTagsForCourse(1)).Returns(tags1);
            this.mockRepository.Setup(r => r.GetTagsForCourse(2)).Returns(tags2);

            // Act - filter for courses that have both tags 1 and 2
            var result = this.service.GetFilteredCourses(string.Empty, false, false, false, false, new List<int> { 1, 2 });

            // Assert
            Assert.Single(result);
            Assert.Equal("Course 1", result[0].Title);
        }

        /// <summary>
        /// Tests that GetFilteredCourses returns empty list when conflicting filters are applied.
        /// </summary>
        [Fact]
        public void GetFilteredCourses_WithConflictingFilters_ReturnsEmptyList()
        {
            // Arrange
            var courses = new List<Course>
            {
                this.CreateTestCourse(1, "Course 1", true),
                this.CreateTestCourse(2, "Course 2", false),
            };
            this.mockRepository.Setup(r => r.GetAllCourses()).Returns(courses);

            // Act - filter for courses that are both premium and free (impossible)
            var result = this.service.GetFilteredCourses(string.Empty, true, true, false, false, new List<int>());

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests that GetModules returns all modules for a specific course.
        /// </summary>
        [Fact]
        public void GetModules_ReturnsModulesForCourse()
        {
            // Arrange
            var expectedModules = new List<Module>
            {
                this.CreateTestModule(1, 1),
                this.CreateTestModule(2, 1),
            };
            this.mockRepository.Setup(r => r.GetModulesByCourseId(1)).Returns(expectedModules);

            // Act
            var result = this.service.GetModules(1);

            // Assert
            Assert.Equal(expectedModules, result);
            this.mockRepository.Verify(r => r.GetModulesByCourseId(1), Times.Once);
        }

        /// <summary>
        /// Tests that GetNormalModules returns only non-bonus modules for a course.
        /// </summary>
        [Fact]
        public void GetNormalModules_ReturnsOnlyNonBonusModules()
        {
            // Arrange
            var modules = new List<Module>
            {
                this.CreateTestModule(1, 1, false),
                this.CreateTestModule(2, 1, true),
            };
            this.mockRepository.Setup(r => r.GetModulesByCourseId(1)).Returns(modules);

            // Act
            var result = this.service.GetNormalModules(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].ModuleId);
            Assert.False(result[0].IsBonus);
        }

        /// <summary>
        /// Tests that OpenModule opens a module when it's not already open.
        /// </summary>
        [Fact]
        public void OpenModule_WhenModuleNotAlreadyOpen_OpensModule()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsModuleOpen(UserId, 1)).Returns(false);

            // Act
            this.service.OpenModule(1);

            // Assert
            this.mockRepository.Verify(r => r.OpenModule(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that OpenModule does nothing when module is already open.
        /// </summary>
        [Fact]
        public void OpenModule_WhenModuleAlreadyOpen_DoesNothing()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsModuleOpen(UserId, 1)).Returns(true);

            // Act
            this.service.OpenModule(1);

            // Assert
            this.mockRepository.Verify(r => r.OpenModule(UserId, 1), Times.Never);
        }

        /// <summary>
        /// Tests that CompleteModule only marks module as complete when not all modules are completed.
        /// </summary>
        [Fact]
        public void CompleteModule_WhenNotAllModulesCompleted_MarksModuleCompleteOnly()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsCourseCompleted(UserId, 1)).Returns(false);

            // Act
            this.service.CompleteModule(1, 1);

            // Assert
            this.mockRepository.Verify(r => r.CompleteModule(UserId, 1), Times.Once);
            this.mockRepository.Verify(r => r.MarkCourseAsCompleted(UserId, 1), Times.Never);
        }

        /// <summary>
        /// Tests that CompleteModule marks both module and course as complete when all modules are completed.
        /// </summary>
        [Fact]
        public void CompleteModule_WhenAllModulesCompleted_MarksCourseComplete()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsCourseCompleted(UserId, 1)).Returns(true);

            // Act
            this.service.CompleteModule(1, 1);

            // Assert
            this.mockRepository.Verify(r => r.CompleteModule(UserId, 1), Times.Once);
            this.mockRepository.Verify(r => r.MarkCourseAsCompleted(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that IsModuleAvailable returns the value from repository.
        /// </summary>
        [Fact]
        public void IsModuleAvailable_ReturnsRepositoryValue()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsModuleAvailable(UserId, 1)).Returns(true);

            // Act
            var result = this.service.IsModuleAvailable(1);

            // Assert
            Assert.True(result);
            this.mockRepository.Verify(r => r.IsModuleAvailable(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that IsModuleCompleted returns the value from repository.
        /// </summary>
        [Fact]
        public void IsModuleCompleted_ReturnsRepositoryValue()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsModuleCompleted(UserId, 1)).Returns(true);

            // Act
            var result = this.service.IsModuleCompleted(1);

            // Assert
            Assert.True(result);
            this.mockRepository.Verify(r => r.IsModuleCompleted(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that IsModuleInProgress returns the value from repository.
        /// </summary>
        [Fact]
        public void IsModuleInProgress_ReturnsRepositoryValue()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsModuleInProgress(UserId, 1)).Returns(true);

            // Act
            var result = this.service.IsModuleInProgress(1);

            // Assert
            Assert.True(result);
            this.mockRepository.Verify(r => r.IsModuleInProgress(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that ClickModuleImage adds coins and returns true when module image wasn't previously clicked.
        /// </summary>
        [Fact]
        public void ClickModuleImage_WhenNotAlreadyClicked_AddsCoinsAndReturnsTrue()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsModuleImageClicked(UserId, 1)).Returns(false);

            // Act
            var result = this.service.ClickModuleImage(1);

            // Assert
            Assert.True(result);
            this.mockRepository.Verify(r => r.ClickModuleImage(UserId, 1), Times.Once);
            this.mockCoinsRepository.Verify(c => c.AddCoinsToUserWallet(UserId, 10), Times.Once);
        }

        /// <summary>
        /// Tests that ClickModuleImage returns false when module image was already clicked.
        /// </summary>
        [Fact]
        public void ClickModuleImage_WhenAlreadyClicked_ReturnsFalse()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsModuleImageClicked(UserId, 1)).Returns(true);

            // Act
            var result = this.service.ClickModuleImage(1);

            // Assert
            Assert.False(result);
            this.mockRepository.Verify(r => r.ClickModuleImage(UserId, 1), Times.Never);
            this.mockCoinsRepository.Verify(c => c.AddCoinsToUserWallet(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Tests that BuyBonusModule returns false when module is not a bonus module.
        /// </summary>
        [Fact]
        public void BuyBonusModule_WhenModuleIsNotBonus_ReturnsFalse()
        {
            // Arrange
            var module = this.CreateTestModule(1, 1, false);
            this.mockRepository.Setup(r => r.GetModule(1)).Returns(module);

            // Act
            var result = this.service.BuyBonusModule(1, 1);

            // Assert
            Assert.False(result);
            this.mockCoinsRepository.Verify(c => c.TryDeductCoinsFromUserWallet(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Tests that BuyBonusModule returns false when module is null.
        /// </summary>
        [Fact]
        public void BuyBonusModule_WhenModuleIsNull_ReturnsFalse()
        {
            // Arrange
            this.mockRepository.Setup(r => r.GetModule(It.IsAny<int>())).Returns((Module?)null);

            // Act
            var result = this.service.BuyBonusModule(1, 1);

            // Assert
            Assert.False(result);
            this.mockCoinsRepository.Verify(c => c.TryDeductCoinsFromUserWallet(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Tests that BuyBonusModule returns false when module is already open.
        /// </summary>
        [Fact]
        public void BuyBonusModule_WhenModuleAlreadyOpen_ReturnsFalse()
        {
            // Arrange
            var module = this.CreateTestModule(1, 1, true, 100);
            this.mockRepository.Setup(r => r.GetModule(1)).Returns(module);
            this.mockRepository.Setup(r => r.IsModuleOpen(UserId, 1)).Returns(true);

            // Act
            var result = this.service.BuyBonusModule(1, 1);

            // Assert
            Assert.False(result);
            this.mockCoinsRepository.Verify(c => c.TryDeductCoinsFromUserWallet(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Tests that BuyBonusModule returns false when course is not found.
        /// </summary>
        [Fact]
        public void BuyBonusModule_WhenCourseNotFound_ReturnsFalse()
        {
            // Arrange
            var module = this.CreateTestModule(1, 1, true, 100);
            this.mockRepository.Setup(r => r.GetModule(1)).Returns(module);
            this.mockRepository.Setup(r => r.IsModuleOpen(UserId, 1)).Returns(false);
            this.mockRepository.Setup(r => r.GetCourse(1)).Returns((Course?)null);

            // Act
            var result = this.service.BuyBonusModule(1, 1);

            // Assert
            Assert.False(result);
            this.mockCoinsRepository.Verify(c => c.TryDeductCoinsFromUserWallet(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Tests that BuyBonusModule returns false when user doesn't have enough coins.
        /// </summary>
        [Fact]
        public void BuyBonusModule_WhenNotEnoughCoins_ReturnsFalse()
        {
            // Arrange
            var module = this.CreateTestModule(1, 1, true, 100);
            var course = this.CreateTestCourse(1, "Course");
            this.mockRepository.Setup(r => r.GetModule(1)).Returns(module);
            this.mockRepository.Setup(r => r.IsModuleOpen(UserId, 1)).Returns(false);
            this.mockRepository.Setup(r => r.GetCourse(1)).Returns(course);
            this.mockCoinsRepository.Setup(c => c.TryDeductCoinsFromUserWallet(UserId, 100)).Returns(false);

            // Act
            var result = this.service.BuyBonusModule(1, 1);

            // Assert
            Assert.False(result);
            this.mockCoinsRepository.Verify(c => c.TryDeductCoinsFromUserWallet(UserId, 100), Times.Once);
            this.mockRepository.Verify(r => r.OpenModule(UserId, 1), Times.Never);
        }

        /// <summary>
        /// Tests that BuyBonusModule successfully opens module and returns true when all conditions are met.
        /// </summary>
        [Fact]
        public void BuyBonusModule_WhenSuccessful_OpensModuleAndReturnsTrue()
        {
            // Arrange
            var module = this.CreateTestModule(1, 1, true, 100);
            var course = this.CreateTestCourse(1, "Course");
            this.mockRepository.Setup(r => r.GetModule(1)).Returns(module);
            this.mockRepository.Setup(r => r.IsModuleOpen(UserId, 1)).Returns(false);
            this.mockRepository.Setup(r => r.GetCourse(1)).Returns(course);
            this.mockCoinsRepository.Setup(c => c.TryDeductCoinsFromUserWallet(UserId, 100)).Returns(true);

            // Act
            var result = this.service.BuyBonusModule(1, 1);

            // Assert
            Assert.True(result);
            this.mockCoinsRepository.Verify(c => c.TryDeductCoinsFromUserWallet(UserId, 100), Times.Once);
            this.mockRepository.Verify(r => r.OpenModule(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that IsUserEnrolled returns the value from repository.
        /// </summary>
        [Fact]
        public void IsUserEnrolled_ReturnsRepositoryValue()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsUserEnrolled(UserId, 1)).Returns(true);

            // Act
            var result = this.service.IsUserEnrolled(1);

            // Assert
            Assert.True(result);
            this.mockRepository.Verify(r => r.IsUserEnrolled(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that EnrollInCourse returns false when user is already enrolled.
        /// </summary>
        [Fact]
        public void EnrollInCourse_WhenAlreadyEnrolled_ReturnsFalse()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsUserEnrolled(UserId, 1)).Returns(true);

            // Act
            var result = this.service.EnrollInCourse(1);

            // Assert
            Assert.False(result);
            this.mockRepository.Verify(r => r.EnrollUser(UserId, 1), Times.Never);
        }

        /// <summary>
        /// Tests that EnrollInCourse returns false when course is not found.
        /// </summary>
        [Fact]
        public void EnrollInCourse_WhenCourseNotFound_ReturnsFalse()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsUserEnrolled(UserId, 1)).Returns(false);
            this.mockRepository.Setup(r => r.GetCourse(1)).Returns((Course?)null);

            // Act
            var result = this.service.EnrollInCourse(1);

            // Assert
            Assert.False(result);
            this.mockRepository.Verify(r => r.EnrollUser(UserId, 1), Times.Never);
        }

        /// <summary>
        /// Tests that EnrollInCourse returns false for premium course when user doesn't have enough coins.
        /// </summary>
        [Fact]
        public void EnrollInCourse_WhenPremiumCourseAndNotEnoughCoins_ReturnsFalse()
        {
            // Arrange
            var course = this.CreateTestCourse(1, "Premium Course", true, 100);
            this.mockRepository.Setup(r => r.IsUserEnrolled(UserId, 1)).Returns(false);
            this.mockRepository.Setup(r => r.GetCourse(1)).Returns(course);
            this.mockCoinsRepository.Setup(c => c.TryDeductCoinsFromUserWallet(UserId, 100)).Returns(false);

            // Act
            var result = this.service.EnrollInCourse(1);

            // Assert
            Assert.False(result);
            this.mockCoinsRepository.Verify(c => c.TryDeductCoinsFromUserWallet(UserId, 100), Times.Once);
            this.mockRepository.Verify(r => r.EnrollUser(UserId, 1), Times.Never);
        }

        /// <summary>
        /// Tests that EnrollInCourse successfully enrolls user in free course.
        /// </summary>
        [Fact]
        public void EnrollInCourse_WhenFreeCourse_EnrollsSuccessfully()
        {
            // Arrange
            var course = this.CreateTestCourse(1, "Free Course", false);
            this.mockRepository.Setup(r => r.IsUserEnrolled(UserId, 1)).Returns(false);
            this.mockRepository.Setup(r => r.GetCourse(1)).Returns(course);

            // Act
            var result = this.service.EnrollInCourse(1);

            // Assert
            Assert.True(result);
            this.mockCoinsRepository.Verify(c => c.TryDeductCoinsFromUserWallet(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            this.mockRepository.Verify(r => r.EnrollUser(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that EnrollInCourse successfully enrolls user in premium course with enough coins.
        /// </summary>
        [Fact]
        public void EnrollInCourse_WhenPremiumCourseWithEnoughCoins_EnrollsSuccessfully()
        {
            // Arrange
            var course = this.CreateTestCourse(1, "Premium Course", true, 100);
            this.mockRepository.Setup(r => r.IsUserEnrolled(UserId, 1)).Returns(false);
            this.mockRepository.Setup(r => r.GetCourse(1)).Returns(course);
            this.mockCoinsRepository.Setup(c => c.TryDeductCoinsFromUserWallet(UserId, 100)).Returns(true);

            // Act
            var result = this.service.EnrollInCourse(1);

            // Assert
            Assert.True(result);
            this.mockCoinsRepository.Verify(c => c.TryDeductCoinsFromUserWallet(UserId, 100), Times.Once);
            this.mockRepository.Verify(r => r.EnrollUser(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that UpdateTimeSpent calls repository with correct parameters.
        /// </summary>
        [Fact]
        public void UpdateTimeSpent_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            const int courseId = 1;
            const int seconds = 3600;

            // Act
            this.service.UpdateTimeSpent(courseId, seconds);

            // Assert
            this.mockRepository.Verify(r => r.UpdateTimeSpent(UserId, courseId, seconds), Times.Once);
        }

        /// <summary>
        /// Tests that GetTimeSpent returns the value from repository.
        /// </summary>
        [Fact]
        public void GetTimeSpent_ReturnsRepositoryValue()
        {
            // Arrange
            const int expectedTime = 3600;
            this.mockRepository.Setup(r => r.GetTimeSpent(UserId, 1)).Returns(expectedTime);

            // Act
            var result = this.service.GetTimeSpent(1);

            // Assert
            Assert.Equal(expectedTime, result);
            this.mockRepository.Verify(r => r.GetTimeSpent(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that IsCourseCompleted returns the value from repository.
        /// </summary>
        [Fact]
        public void IsCourseCompleted_ReturnsRepositoryValue()
        {
            // Arrange
            this.mockRepository.Setup(r => r.IsCourseCompleted(UserId, 1)).Returns(true);

            // Act
            var result = this.service.IsCourseCompleted(1);

            // Assert
            Assert.True(result);
            this.mockRepository.Verify(r => r.IsCourseCompleted(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that GetCompletedModulesCount returns the value from repository.
        /// </summary>
        [Fact]
        public void GetCompletedModulesCount_ReturnsRepositoryValue()
        {
            // Arrange
            this.mockRepository.Setup(r => r.GetCompletedModulesCount(UserId, 1)).Returns(5);

            // Act
            var result = this.service.GetCompletedModulesCount(1);

            // Assert
            Assert.Equal(5, result);
            this.mockRepository.Verify(r => r.GetCompletedModulesCount(UserId, 1), Times.Once);
        }

        /// <summary>
        /// Tests that GetRequiredModulesCount returns the value from repository.
        /// </summary>
        [Fact]
        public void GetRequiredModulesCount_ReturnsRepositoryValue()
        {
            // Arrange
            this.mockRepository.Setup(r => r.GetRequiredModulesCount(1)).Returns(10);

            // Act
            var result = this.service.GetRequiredModulesCount(1);

            // Assert
            Assert.Equal(10, result);
            this.mockRepository.Verify(r => r.GetRequiredModulesCount(1), Times.Once);
        }

        /// <summary>
        /// Tests that ClaimCompletionReward returns false when reward was not claimed.
        /// </summary>
        [Fact]
        public void ClaimCompletionReward_WhenNotClaimed_ReturnsFalse()
        {
            // Arrange
            this.mockRepository.Setup(r => r.ClaimCompletionReward(UserId, 1)).Returns(false);

            // Act
            var result = this.service.ClaimCompletionReward(1);

            // Assert
            Assert.False(result);
            this.mockCoinsRepository.Verify(c => c.AddCoinsToUserWallet(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Tests that ClaimCompletionReward adds coins and returns true when reward is claimed.
        /// </summary>
        [Fact]
        public void ClaimCompletionReward_WhenClaimed_AddsCoinsAndReturnsTrue()
        {
            // Arrange
            this.mockRepository.Setup(r => r.ClaimCompletionReward(UserId, 1)).Returns(true);

            // Act
            var result = this.service.ClaimCompletionReward(1);

            // Assert
            Assert.True(result);
            this.mockCoinsRepository.Verify(c => c.AddCoinsToUserWallet(UserId, 50), Times.Once);
        }

        /// <summary>
        /// Tests that ClaimTimedReward returns false when reward was not claimed.
        /// </summary>
        [Fact]
        public void ClaimTimedReward_WhenNotClaimed_ReturnsFalse()
        {
            // Arrange
            this.mockRepository.Setup(r => r.GetCourseTimeLimit(1)).Returns(3600);
            this.mockRepository.Setup(r => r.ClaimTimedReward(UserId, 1, 1800, 3600)).Returns(false);

            // Act
            var result = this.service.ClaimTimedReward(1, 1800);

            // Assert
            Assert.False(result);
            this.mockCoinsRepository.Verify(c => c.AddCoinsToUserWallet(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Tests that ClaimTimedReward adds coins and returns true when reward is claimed.
        /// </summary>
        [Fact]
        public void ClaimTimedReward_WhenClaimed_AddsCoinsAndReturnsTrue()
        {
            // Arrange
            this.mockRepository.Setup(r => r.GetCourseTimeLimit(1)).Returns(3600);
            this.mockRepository.Setup(r => r.ClaimTimedReward(UserId, 1, 3600, 3600)).Returns(true);

            // Act
            var result = this.service.ClaimTimedReward(1, 3600);

            // Assert
            Assert.True(result);
            this.mockCoinsRepository.Verify(c => c.AddCoinsToUserWallet(UserId, 300), Times.Once);
        }

        /// <summary>
        /// Tests that GetCourseTimeLimit returns the value from repository.
        /// </summary>
        [Fact]
        public void GetCourseTimeLimit_ReturnsRepositoryValue()
        {
            // Arrange
            this.mockRepository.Setup(r => r.GetCourseTimeLimit(1)).Returns(3600);

            // Act
            var result = this.service.GetCourseTimeLimit(1);

            // Assert
            Assert.Equal(3600, result);
            this.mockRepository.Verify(r => r.GetCourseTimeLimit(1), Times.Once);
        }

        /// <summary>
        /// Tests that GetTags returns all tags from repository.
        /// </summary>
        [Fact]
        public void GetTags_ReturnsAllTagsFromRepository()
        {
            // Arrange
            var expectedTags = new List<Tag>
            {
                this.CreateTestTag(1, "Programming"),
                this.CreateTestTag(2, "Design"),
            };
            this.mockRepository.Setup(r => r.GetAllTags()).Returns(expectedTags);

            // Act
            var result = this.service.GetTags();

            // Assert
            Assert.Equal(expectedTags, result);
            this.mockRepository.Verify(r => r.GetAllTags(), Times.Once);
        }

        /// <summary>
        /// Tests that GetCourseTags returns tags for specific course.
        /// </summary>
        [Fact]
        public void GetCourseTags_ReturnsTagsForCourse()
        {
            // Arrange
            var expectedTags = new List<Tag>
            {
                this.CreateTestTag(1, "Programming"),
                this.CreateTestTag(2, "C#"),
            };
            this.mockRepository.Setup(r => r.GetTagsForCourse(1)).Returns(expectedTags);

            // Act
            var result = this.service.GetCourseTags(1);

            // Assert
            Assert.Equal(expectedTags, result);
            this.mockRepository.Verify(r => r.GetTagsForCourse(1), Times.Once);
        }

        // Helper methods for test data
        private Course CreateTestCourse(int id, string title, bool isPremium = false, int cost = 0)
        {
            return new Course { CourseId = id, Title = title, IsPremium = isPremium, Cost = cost, Description = string.Empty, Difficulty = "Easy", ImageUrl = string.Empty };
        }

        private Module CreateTestModule(int id, int courseId, bool isBonus = false, int cost = 0)
        {
            return new Module { ModuleId = id, CourseId = courseId, IsBonus = isBonus, Cost = cost, Title = "Title1", Description = "Description1", ImageUrl = string.Empty };
        }

        private Tag CreateTestTag(int id, string name)
        {
            return new Tag { TagId = id, Name = name };
        }
    }
}