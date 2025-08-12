using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Roadmap;
using DuoClassLibrary.Models.Sections;
using DuoClassLibrary.Services;
using Duo.Services.Interfaces;
using Moq;

namespace Duo.Tests.Services;

[TestClass]
public class RoadmapServiceTests
{
    private Mock<IRoadmapServiceProxy> mockProxy;
    private RoadmapService roadmapService;

    [TestInitialize]
    public void Setup()
    {
        mockProxy = new Mock<IRoadmapServiceProxy>();
        roadmapService = new RoadmapService(mockProxy.Object);
    }

    private Roadmap CreateSampleRoadmap(int id)
    {
        return new Roadmap
        {
            Id = id,
            Name = $"Roadmap {id}",
            Sections = new List<Section>
        {
            new Section
            {
                Id = 1,
                Description = $"Description {id}",
                Title = $"Title {id}",
                Exam = new Exam
                (
                    id: 1,
                    sectionId: 1
                )
                {
                    Id = 1,
                    SectionId = 1,
                    Exercises = new List<Exercise>
                    {
                        new AssociationExercise
                        (
                            id: 1,
                            question: "Question 1",
                            difficulty: Duo.Models.Difficulty.Easy,
                            firstAnswers: new List<string> { "Answer 1", "Answer 2" },
                            secondAnswers: new List<string> { "Answer A", "Answer B" }
                        )
                    }
                }
            }
        }
        };
    }

    //[TestMethod]
    //public async Task GetAllAsync_ReturnsEmptyList_WhenNoRoadmaps()
    //{
    //    // Arrange
    //    mockProxy.Setup(p => p.GetAllAsync()).ReturnsAsync(new List<Roadmap>());
    //    // Act
    //    var result = await roadmapService.GetAllAsync();
    //    // Assert
    //    Assert.AreEqual(0, result.Count);
    //}

    //[TestMethod]
    //public async Task GetAllAsync_ReturnsListOfRoadmaps()
    //{
    //    // Arrange
    //    var roadmaps = new List<Roadmap>
    //    {
    //        CreateSampleRoadmap(1),
    //        CreateSampleRoadmap(2)
    //    };
    //    mockProxy.Setup(p => p.GetAllAsync()).ReturnsAsync(roadmaps);
    //    // Act
    //    var result = await roadmapService.GetAllAsync();
    //    // Assert
    //    Assert.AreEqual(2, result.Count);
    //}

    [TestMethod]
    public async Task GetByIdAsync_ReturnsNull_WhenRoadmapNotFound()
    {
        // Arrange
        mockProxy.Setup(p => p.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Roadmap)null);
        // Act
        var result = await roadmapService.GetByIdAsync(999);
        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsRoadmap_WhenRoadmapFound()
    {
        // Arrange
        var roadmap = CreateSampleRoadmap(1);
        mockProxy.Setup(p => p.GetByIdAsync(1)).ReturnsAsync(roadmap);
        // Act
        var result = await roadmapService.GetByIdAsync(1);
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(roadmap.Id, result.Id);
    }

    //[TestMethod]
    //public async Task GetByNameAsync_ReturnsNull_WhenRoadmapNotFound()
    //{
    //    // Arrange
    //    mockProxy.Setup(p => p.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Roadmap)null);
    //    // Act
    //    var result = await roadmapService.GetByNameAsync("NonExistentRoadmap");
    //    // Assert
    //    Assert.IsNull(result);
    //}

    //[TestMethod]
    //public async Task GetByNameAsync_ReturnsRoadmap_WhenRoadmapFound()
    //{
    //    // Arrange
    //    var roadmap = CreateSampleRoadmap(1);
    //    mockProxy.Setup(p => p.GetByNameAsync("Roadmap 1")).ReturnsAsync(roadmap);
    //    // Act
    //    var result = await roadmapService.GetByNameAsync("Roadmap 1");
    //    // Assert
    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(roadmap.Name, result.Name);
    //}

    //[TestMethod]
    //public async Task AddAsync_ReturnsNewId_WhenRoadmapAdded()
    //{
    //    // Arrange
    //    var roadmap = CreateSampleRoadmap(1);
    //    mockProxy.Setup(p => p.AddAsync(roadmap)).ReturnsAsync(roadmap.Id);
    //    // Act
    //    var result = await roadmapService.AddAsync(roadmap);
    //    // Assert
    //    Assert.AreEqual(roadmap.Id, result);
    //}

    //[TestMethod]
    //public async Task AddAsync_ReturnsZero_WhenRoadmapNotAdded()
    //{
    //    // Arrange
    //    var roadmap = CreateSampleRoadmap(1);
    //    mockProxy.Setup(p => p.AddAsync(roadmap)).ReturnsAsync(0);
    //    // Act
    //    var result = await roadmapService.AddAsync(roadmap);
    //    // Assert
    //    Assert.AreEqual(0, result);
    //}

    //[TestMethod]
    //public async Task DeleteAsync_CallsDeleteOnProxy_WhenRoadmapExists()
    //{
    //    // Arrange
    //    var roadmap = CreateSampleRoadmap(1);
    //    mockProxy.Setup(p => p.DeleteAsync(roadmap)).Returns(Task.CompletedTask);
    //    // Act
    //    await roadmapService.DeleteAsync(roadmap);
    //    // Assert
    //    mockProxy.Verify(p => p.DeleteAsync(roadmap), Times.Once);
    //}
}
