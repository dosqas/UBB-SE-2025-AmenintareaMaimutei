using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Services;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Duo.Models;
using System.Diagnostics.CodeAnalysis;
using DuoTests.Utils;

namespace Duo.Tests.Services
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ExerciseServiceTests
    {
        private Mock<IExerciseService> _mockProxy;
        private ExerciseService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockProxy = new Mock<IExerciseService>();
            _service = new ExerciseService(_mockProxy.Object);
        }

        [TestMethod]
        public async Task GetAllExercises_ProxyReturnsList_ReturnsSameList()
        {
            // Arrange
            var expected = new List<Exercise>
            {
                new TestExercise(1, "Q1", Difficulty.Easy),
                new TestExercise(2, "Q2", Difficulty.Normal)
            };
            _mockProxy.Setup(p => p.GetAllExercises()).ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllExercises();

            // Assert
            Assert.AreEqual(expected.Count, result.Count);
            Assert.AreEqual(expected[0].Id, result[0].Id);
        }

        [TestMethod]
        public async Task GetAllExercises_ProxyThrowsException_ReturnsEmptyList()
        {
            // Arrange
            _mockProxy.Setup(p => p.GetAllExercises()).ThrowsAsync(new Exception("error"));

            // Act
            var result = await _service.GetAllExercises();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetExerciseById_ValidId_ReturnsExercise()
        {
            // Arrange
            var exercise = new TestExercise(42, "Sample", Difficulty.Normal);
            _mockProxy.Setup(p => p.GetExerciseById(42)).ReturnsAsync(exercise);

            // Act
            var result = await _service.GetExerciseById(42);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(42, result.Id);
        }

        [TestMethod]
        public async Task GetExerciseById_ProxyThrowsException_ReturnsNull()
        {
            // Arrange
            _mockProxy.Setup(p => p.GetExerciseById(1)).ThrowsAsync(new Exception());

            // Act
            var result = await _service.GetExerciseById(1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllExercisesFromQuiz_ValidQuizId_ReturnsList()
        {
            // Arrange
            var expected = new List<Exercise> { new TestExercise(1, "Quiz Q", Difficulty.Easy) };
            _mockProxy.Setup(p => p.GetAllExercisesFromQuiz(5)).ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllExercisesFromQuiz(5);

            // Assert
            Assert.AreEqual(expected.Count, result.Count);
        }

        [TestMethod]
        public async Task GetAllExercisesFromQuiz_ProxyThrowsException_ReturnsEmptyList()
        {
            // Arrange
            _mockProxy.Setup(p => p.GetAllExercisesFromQuiz(5)).ThrowsAsync(new Exception());

            // Act
            var result = await _service.GetAllExercisesFromQuiz(5);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetAllExercisesFromExam_ValidExamId_ReturnsList()
        {
            // Arrange
            var expected = new List<Exercise> { new TestExercise(2, "Exam Q", Difficulty.Hard) };
            _mockProxy.Setup(p => p.GetAllExercisesFromExam(3)).ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllExercisesFromExam(3);

            // Assert
            Assert.AreEqual(expected.Count, result.Count);
        }

        [TestMethod]
        public async Task GetAllExercisesFromExam_ProxyThrowsException_ReturnsEmptyList()
        {
            // Arrange
            _mockProxy.Setup(p => p.GetAllExercisesFromExam(3)).ThrowsAsync(new Exception());

            // Act
            var result = await _service.GetAllExercisesFromExam(3);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task DeleteExercise_ValidId_CallsProxyOnce()
        {
            // Act
            await _service.DeleteExercise(7);

            // Assert
            _mockProxy.Verify(p => p.DeleteExercise(7), Times.Once);
        }

        [TestMethod]
        public async Task DeleteExercise_ProxyThrowsException_DoesNotThrow()
        {
            // Arrange
            _mockProxy.Setup(p => p.DeleteExercise(It.IsAny<int>())).ThrowsAsync(new Exception());

            // Act
            await _service.DeleteExercise(7);

            // Assert
            _mockProxy.Verify(p => p.DeleteExercise(7), Times.Once);
        }

        [TestMethod]
        public async Task CreateExercise_ValidExercise_CallsProxy()
        {
            // Arrange
            var exercise = new AssociationExercise(
                11, "Fail Q", Difficulty.Hard, new List<String> { "A", "B", "C" }, new List<String> { "D", "E", "F" });

            // Act
            await _service.CreateExercise(exercise);

            // Assert
            _mockProxy.Verify(p => p.CreateExercise(exercise), Times.Once);
        }

        [TestMethod]
        public async Task CreateExercise_ProxyThrowsException_DoesNotThrow()
        {
            // Arrange
            var exercise = new AssociationExercise(
                11, "Fail Q", Difficulty.Hard, new List<String> { "A", "B", "C" }, new List<String> { "D", "E", "F" });
            _mockProxy.Setup(p => p.CreateExercise(exercise)).ThrowsAsync(new Exception());

            // Act
            await _service.CreateExercise(exercise);

            // Assert
            _mockProxy.Verify(p => p.CreateExercise(exercise), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullProxy_ThrowsArgumentNullException()
        {
            // Act
            var service = new ExerciseService(null);
        }
    }
}
