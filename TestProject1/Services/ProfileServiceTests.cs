using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Duo.Api.Repositories.Interfaces;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services;
using Moq;
using Xunit;
using System.Threading.Tasks;
using DuoClassLibrary.Services.Interfaces;

namespace TestProject1.Services
{
    public class ProfileServiceTests
    {
        private readonly Mock<IUserHelperService> _mockUserHelperService;
        private readonly ProfileService _profileService;
        private readonly User _testUser;
        private readonly List<Achievement> _testAchievements;

        public ProfileServiceTests()
        {
            _testUser = new User 
            { 
                UserId = 1, 
                UserName = "testuser",
                Streak = 15,
                QuizzesCompleted = 75,
                CoursesCompleted = 30
            };

            _testAchievements = new List<Achievement>
            {
                new Achievement { Id = 1, Name = "10 Day Streak" },
                new Achievement { Id = 2, Name = "50 Quizzes Completed" },
                new Achievement { Id = 3, Name = "100 Courses Completed" }
            };

            _mockUserHelperService = new Mock<IUserHelperService>();
            _mockUserHelperService.Setup(service => service.GetUserStats(It.IsAny<int>()))
                .ReturnsAsync(_testUser);
            _mockUserHelperService.Setup(service => service.GetAllAchievements())
                .ReturnsAsync(_testAchievements);
            _mockUserHelperService.Setup(service => service.GetUserAchievements(It.IsAny<int>()))
                .ReturnsAsync(new List<Achievement>());

            _profileService = new ProfileService(_mockUserHelperService.Object);
        }

        [Fact]
        public void Constructor_NullRepository_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ProfileService(null));
        }

        [Fact]
        public async Task CreateUser_CallsUserHelperService()
        {
            await _profileService.CreateUser(_testUser);

            _mockUserHelperService.Verify(service => service.CreateUser(_testUser), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_CallsUserHelperService()
        {
            await _profileService.UpdateUser(_testUser);

            _mockUserHelperService.Verify(service => service.UpdateUser(_testUser), Times.Once);
        }

        [Fact]
        public async Task GetUserStats_ReturnsUserStats()
        {
            var result = await _profileService.GetUserStats(1);

            Assert.NotNull(result);
            Assert.Equal(_testUser.UserId, result.UserId);
            Assert.Equal(_testUser.UserName, result.UserName);
        }
    }
}