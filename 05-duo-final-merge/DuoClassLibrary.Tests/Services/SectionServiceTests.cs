using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Sections;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Moq;

namespace Duo.Tests.Services
{
    [TestClass]
    public sealed class SectionServiceTests
    {
        private Mock<ISectionServiceProxy> proxyMock = null!;
        private SectionService service = null!;

        #region Test Initialization

        [TestInitialize]
        public void Init()
        {
            this.proxyMock = new Mock<ISectionServiceProxy>(MockBehavior.Strict);
            this.service = new SectionService(this.proxyMock.Object);
        }

        #endregion

        #region Helpers

        private sealed class TestExercise : Exercise
        {
            public TestExercise(int id)
                : base(id, question: $"Q{id}", difficulty: DuoClassLibrary.Models.Difficulty.Easy)
            {
            }
        }

        private static Exercise DummyEx(int id) => new TestExercise(id);

        private static Quiz MakeValidQuiz(int id)
        {
            var q = new Quiz(id, sectionId: null, orderNumber: null);
            for (var i = 0; i < 10; i++)
            {
                q.AddExercise(DummyEx(id * 100 + i));
            }
            return q;
        }

        private static Exam MakeValidExam(int id)
        {
            var e = new Exam(id, sectionId: null);
            for (var i = 0; i < 25; i++)
            {
                e.AddExercise(DummyEx(id * 1000 + i));
            }
            return e;
        }

        private static Section NewValidSection(int id)
        {
            var s = new Section
            {
                Id = id,
                Title = $"Title {id}",
                Description = $"Desc {id}",
                RoadmapId = 1,
                OrderNumber = null
            };

            s.AddQuiz(MakeValidQuiz(id * 10));
            s.AddQuiz(MakeValidQuiz(id * 10 + 1));
            s.AddExam(MakeValidExam(id));
            return s;
        }

        private static SectionDependency Dep(bool done) => new() { IsCompleted = done };

        #endregion

        [TestMethod]
        public async Task CountSectionsFromRoadmap_ReturnsCount()
        {
            this.proxyMock.Setup(p => p.CountSectionsFromRoadmap(5)).ReturnsAsync(4);

            var count = await this.service.CountSectionsFromRoadmap(5);

            Assert.AreEqual(4, count);
            this.proxyMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow(typeof(HttpRequestException))]
        public async Task CountSectionsFromRoadmap_ReturnsZero_OnErrors(Type exType)
        {
            this.proxyMock.Setup(p => p.CountSectionsFromRoadmap(5))
                          .ThrowsAsync((Exception)Activator.CreateInstance(exType)!);

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => this.service.CountSectionsFromRoadmap(5));

            this.proxyMock.VerifyAll();
        }

        [TestMethod]
        public async Task DeleteSection_CallsProxy()
        {
            this.proxyMock.Setup(p => p.DeleteSection(7)).Returns(Task.CompletedTask);

            await this.service.DeleteSection(7);

            this.proxyMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow(typeof(Exception))]
        public async Task DeleteSection_SwallowsErrors(Type exType)
        {
            this.proxyMock.Setup(p => p.DeleteSection(7))
                          .ThrowsAsync((Exception)Activator.CreateInstance(exType)!);

            await Assert.ThrowsExceptionAsync<Exception>(() => this.service.DeleteSection(7));
            this.proxyMock.VerifyAll();
        }

        [TestMethod]
        public async Task GetAllSections_ReturnsList()
        {
            var list = new List<Section> { NewValidSection(1) };
            this.proxyMock.Setup(p => p.GetAllSections()).ReturnsAsync(list);

            var result = await this.service.GetAllSections();

            Assert.AreEqual(1, result.Count);
            this.proxyMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow(typeof(Exception))]
        public async Task GetAllSections_ReturnsEmpty_OnErrors(Type exType)
        {
            this.proxyMock.Setup(p => p.GetAllSections())
                          .ThrowsAsync((Exception)Activator.CreateInstance(exType)!);

            await Assert.ThrowsExceptionAsync<Exception>(() => this.service.GetAllSections());

            this.proxyMock.VerifyAll();
        }

        [TestMethod]
        public async Task GetByRoadmapId_ReturnsList()
        {
            var list = new List<Section> { NewValidSection(10) };
            this.proxyMock.Setup(p => p.GetByRoadmapId(3)).ReturnsAsync(list);

            var result = await this.service.GetByRoadmapId(3);

            Assert.AreEqual(1, result.Count);
            this.proxyMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow(typeof(Exception))]
        public async Task GetByRoadmapId_ReturnsEmpty_OnErrors(Type exType)
        {
            this.proxyMock.Setup(p => p.GetByRoadmapId(3))
                          .ThrowsAsync((Exception)Activator.CreateInstance(exType)!);

            await Assert.ThrowsExceptionAsync<Exception>(() => this.service.GetByRoadmapId(3));

            this.proxyMock.VerifyAll();
        }

        [TestMethod]
        public async Task GetSectionById_ReturnsSection()
        {
            var sec = NewValidSection(4);
            this.proxyMock.Setup(p => p.GetSectionById(4)).ReturnsAsync(sec);

            var result = await this.service.GetSectionById(4);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Id);
            this.proxyMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow(typeof(Exception))]
        public async Task GetSectionById_ReturnsNull_OnErrors(Type exType)
        {
            this.proxyMock.Setup(p => p.GetSectionById(4))
                          .ThrowsAsync((Exception)Activator.CreateInstance(exType)!);

            await Assert.ThrowsExceptionAsync<Exception>(() => this.service.GetSectionById(4));

            this.proxyMock.VerifyAll();
        }

        [TestMethod]
        public async Task LastOrderNumberFromRoadmap_ReturnsValue()
        {
            this.proxyMock.Setup(p => p.LastOrderNumberFromRoadmap(8)).ReturnsAsync(6);

            var value = await this.service.LastOrderNumberFromRoadmap(8);

            Assert.AreEqual(6, value);
            this.proxyMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow(typeof(Exception))]
        public async Task LastOrderNumberFromRoadmap_ReturnsZero_OnErrors(Type exType)
        {
            this.proxyMock.Setup(p => p.LastOrderNumberFromRoadmap(8))
                          .ThrowsAsync((Exception)Activator.CreateInstance(exType)!);

            await Assert.ThrowsExceptionAsync<Exception>(() => this.service.LastOrderNumberFromRoadmap(8));

            this.proxyMock.VerifyAll();
        }
    }
}