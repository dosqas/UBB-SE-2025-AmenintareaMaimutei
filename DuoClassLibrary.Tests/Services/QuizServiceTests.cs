using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DuoClassLibrary.Exceptions;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Quizzes.API;
using DuoClassLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Duo.Tests.Services
{
    [TestClass]
    public class QuizServiceTests
    {
        private Mock<IQuizServiceProxy> proxyMock = null!;
        private QuizService service = null!;

        [TestInitialize]
        public void Setup()
        {
            proxyMock = new Mock<IQuizServiceProxy>(MockBehavior.Strict);
            service = new QuizService(proxyMock.Object);
        }

        private class TestExercise : Exercise
        {
            public TestExercise(int id)
                : base(id, $"Q{id}", DuoClassLibrary.Models.Difficulty.Easy)
            {
            }
        }

        private Quiz Q(int id) => new Quiz(id, sectionId: null, orderNumber: null);

        private Exam E(int id) => new Exam(id, sectionId: null);

        private QuizSubmission DS(int quizId) =>
            new QuizSubmission { QuizId = quizId, Answers = new List<AnswerSubmission>() };

        private QuizResult R(int quizId) =>
            new QuizResult { QuizId = quizId, CorrectAnswers = 1, TotalQuestions = 2, TimeTaken = TimeSpan.Zero };

        [TestMethod]
        public async Task Get_ReturnsList()
        {
            var list = new List<Quiz> { Q(1), Q(2) };
            proxyMock.Setup(p => p.GetAsync()).ReturnsAsync(list);

            var result = await service.GetAllQuizzes();

            CollectionAssert.AreEqual(list, result);
            proxyMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Get_ThrowsQuizServiceException_OnError()
        {
            proxyMock.Setup(p => p.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
            await service.GetAllQuizzes();
        }

        [TestMethod]
        public async Task GetAllAvailableExams_ReturnsList()
        {
            var list = new List<Exam> { E(3) };
            proxyMock.Setup(p => p.GetAllExams()).ReturnsAsync(list);

            var result = await service.GetAllExams();

            CollectionAssert.AreEqual(list, result);
            proxyMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetAllAvailableExams_Throws_OnError()
        {
            proxyMock.Setup(p => p.GetAllExams())
                     .ThrowsAsync(new Exception("fail"));
            await service.GetAllExams();
        }

        [TestMethod]
        public async Task GetQuizById_ReturnsQuiz()
        {
            var quiz = Q(5);
            proxyMock.Setup(p => p.GetQuizByIdAsync(5)).ReturnsAsync(quiz);

            var result = await service.GetQuizById(5);

            Assert.AreEqual(quiz, result);
            proxyMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetQuizById_Throws_OnError()
        {
            proxyMock.Setup(p => p.GetQuizByIdAsync(6))
                     .ThrowsAsync(new Exception("err"));
            await service.GetQuizById(6);
        }

        [TestMethod]
        public async Task GetExamById_ReturnsExam()
        {
            var exam = E(7);
            proxyMock.Setup(p => p.GetExamByIdAsync(7)).ReturnsAsync(exam);

            var result = await service.GetExamById(7);

            Assert.AreEqual(exam, result);
            proxyMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetExamById_Throws_OnError()
        {
            proxyMock.Setup(p => p.GetExamByIdAsync(8)).ThrowsAsync(new Exception());
            await service.GetExamById(8);
        }

        [TestMethod]
        public async Task GetAllQuizzesFromSection_ReturnsList()
        {
            var list = new List<Quiz> { Q(9) };
            proxyMock.Setup(p => p.GetAllQuizzesFromSectionAsync(9)).ReturnsAsync(list);

            var result = await service.GetAllQuizzesFromSection(9);

            CollectionAssert.AreEqual(list, result);
            proxyMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetAllQuizzesFromSection_Throws_OnError()
        {
            proxyMock.Setup(p => p.GetAllQuizzesFromSectionAsync(10)).ThrowsAsync(new Exception());
            await service.GetAllQuizzesFromSection(10);
        }

        [TestMethod]
        public async Task CountQuizzesFromSection_ReturnsCount()
        {
            proxyMock.Setup(p => p.CountQuizzesFromSectionAsync(11)).ReturnsAsync(5);

            var result = await service.CountQuizzesFromSection(11);

            Assert.AreEqual(5, result);
            proxyMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task CountQuizzesFromSection_Throws_OnError()
        {
            proxyMock.Setup(p => p.CountQuizzesFromSectionAsync(12)).ThrowsAsync(new Exception());
            await service.CountQuizzesFromSection(12);
        }

        [TestMethod]
        public async Task LastOrderNumberFromSection_ReturnsValue()
        {
            proxyMock.Setup(p => p.LastOrderNumberFromSectionAsync(13)).ReturnsAsync(2);

            var result = await service.LastOrderNumberFromSection(13);

            Assert.AreEqual(2, result);
            proxyMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task LastOrderNumberFromSection_Throws_OnError()
        {
            proxyMock.Setup(p => p.LastOrderNumberFromSectionAsync(14)).ThrowsAsync(new Exception());
            await service.LastOrderNumberFromSection(14);
        }

        [TestMethod]
        public async Task GetExamFromSection_ReturnsExam()
        {
            var exam = E(15);
            proxyMock.Setup(p => p.GetExamFromSectionAsync(15)).ReturnsAsync(exam);

            var result = await service.GetExamFromSection(15);

            Assert.AreEqual(exam, result);
            proxyMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetExamFromSection_Throws_OnError()
        {
            proxyMock.Setup(p => p.GetExamFromSectionAsync(16)).ThrowsAsync(new Exception());
            await service.GetExamFromSection(16);
        }

        [TestMethod]
        public async Task DeleteQuiz_CallsProxy()
        {
            proxyMock.Setup(p => p.DeleteQuizAsync(17)).Returns(Task.CompletedTask);

            await service.DeleteQuiz(17);

            proxyMock.Verify(p => p.DeleteQuizAsync(17), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task DeleteQuiz_Throws_OnError()
        {
            proxyMock.Setup(p => p.DeleteQuizAsync(18)).ThrowsAsync(new Exception());
            await service.DeleteQuiz(18);
        }

        [TestMethod]
        public async Task UpdateQuiz_CallsProxy()
        {
            var quiz = Q(19);
            proxyMock.Setup(p => p.UpdateQuizAsync(quiz)).Returns(Task.CompletedTask);

            await service.UpdateQuiz(quiz);

            proxyMock.Verify(p => p.UpdateQuizAsync(quiz), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UpdateQuiz_Throws_OnError()
        {
            var quiz = Q(20);
            proxyMock.Setup(p => p.UpdateQuizAsync(quiz)).ThrowsAsync(new Exception());
            await service.UpdateQuiz(quiz);
        }

        [TestMethod]
        public async Task CreateQuiz_ReturnsId()
        {
            var quiz = Q(21);
            proxyMock.Setup(p => p.CreateQuizAsync(quiz)).Returns(Task.CompletedTask);

            var result = await service.CreateQuiz(quiz);

            Assert.AreEqual(21, result);
            proxyMock.Verify(p => p.CreateQuizAsync(quiz), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task CreateQuiz_Throws_OnError()
        {
            var quiz = Q(22);
            proxyMock.Setup(p => p.CreateQuizAsync(quiz)).ThrowsAsync(new Exception());
            await service.CreateQuiz(quiz);
        }

        [TestMethod]
        public async Task AddExercisesToQuiz_CallsProxy_WithCorrectIds()
        {
            var exs = new List<Exercise> { new TestExercise(101), new TestExercise(102) };
            proxyMock.Setup(p => p.AddExercisesToQuizAsync(23, It.Is<List<int>>(l =>
                l.Count == 2 && l.Contains(101) && l.Contains(102))))
                .Returns(Task.CompletedTask);

            await service.AddExercisesToQuiz(23, exs);

            proxyMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task AddExercisesToQuiz_Throws_OnError()
        {
            proxyMock.Setup(p => p.AddExercisesToQuizAsync(24, It.IsAny<List<int>>()))
                     .ThrowsAsync(new Exception());
            await service.AddExercisesToQuiz(24, new List<Exercise>());
        }

        [TestMethod]
        public async Task AddExerciseToQuiz_CallsProxy()
        {
            proxyMock.Setup(p => p.AddExerciseToQuizAsync(25, 555)).Returns(Task.CompletedTask);

            await service.AddExerciseToQuiz(25, 555);

            proxyMock.Verify(p => p.AddExerciseToQuizAsync(25, 555), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task AddExerciseToQuiz_Throws_OnError()
        {
            proxyMock.Setup(p => p.AddExerciseToQuizAsync(26, 666)).ThrowsAsync(new Exception());
            await service.AddExerciseToQuiz(26, 666);
        }

        [TestMethod]
        public async Task RemoveExerciseFromQuiz_CallsProxy()
        {
            proxyMock.Setup(p => p.RemoveExerciseFromQuizAsync(27, 777)).Returns(Task.CompletedTask);

            await service.RemoveExerciseFromQuiz(27, 777);

            proxyMock.Verify(p => p.RemoveExerciseFromQuizAsync(27, 777), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task RemoveExerciseFromQuiz_Throws_OnError()
        {
            proxyMock.Setup(p => p.RemoveExerciseFromQuizAsync(28, 888)).ThrowsAsync(new Exception());
            await service.RemoveExerciseFromQuiz(28, 888);
        }

        [TestMethod]
        public async Task DeleteExam_CallsProxy()
        {
            proxyMock.Setup(p => p.DeleteExamAsync(29)).Returns(Task.CompletedTask);

            await service.DeleteExam(29);

            proxyMock.Verify(p => p.DeleteExamAsync(29), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task DeleteExam_Throws_OnError()
        {
            proxyMock.Setup(p => p.DeleteExamAsync(30)).ThrowsAsync(new Exception());
            await service.DeleteExam(30);
        }

        [TestMethod]
        public async Task UpdateExam_CallsProxy()
        {
            var exam = E(31);
            proxyMock.Setup(p => p.UpdateExamAsync(exam)).Returns(Task.CompletedTask);

            await service.UpdateExam(exam);

            proxyMock.Verify(p => p.UpdateExamAsync(exam), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UpdateExam_Throws_OnError()
        {
            var exam = E(32);
            proxyMock.Setup(p => p.UpdateExamAsync(exam)).ThrowsAsync(new Exception());
            await service.UpdateExam(exam);
        }

        [TestMethod]
        public async Task CreateExam_ReturnsId()
        {
            var exam = E(33);
            proxyMock.Setup(p => p.CreateExamAsync(exam)).ReturnsAsync(exam);

            var result = await service.CreateExam(exam);

            Assert.AreEqual(33, result);
            proxyMock.Verify(p => p.CreateExamAsync(exam), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task CreateExam_Throws_OnError()
        {
            var exam = E(34);
            proxyMock.Setup(p => p.CreateExamAsync(exam)).ThrowsAsync(new Exception());
            await service.CreateExam(exam);
        }

        [TestMethod]
        public async Task SubmitQuizAsync_CallsProxy()
        {
            var sub = DS(35);
            proxyMock.Setup(p => p.SubmitQuizAsync(sub)).Returns(Task.CompletedTask);

            await service.SubmitQuizAsync(sub);

            proxyMock.Verify(p => p.SubmitQuizAsync(sub), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task SubmitQuizAsync_Throws_OnError()
        {
            var sub = DS(36);
            proxyMock.Setup(p => p.SubmitQuizAsync(sub)).ThrowsAsync(new Exception());
            await service.SubmitQuizAsync(sub);
        }

        [TestMethod]
        public async Task GetResultAsync_ReturnsResult()
        {
            var res = R(37);
            proxyMock.Setup(p => p.GetResultAsync(37)).ReturnsAsync(res);

            var result = await service.GetResultAsync(37);

            Assert.AreEqual(res, result);
            proxyMock.Verify(p => p.GetResultAsync(37), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetResultAsync_Throws_OnError()
        {
            proxyMock.Setup(p => p.GetResultAsync(38)).ThrowsAsync(new Exception());
            await service.GetResultAsync(38);
        }
    }
}