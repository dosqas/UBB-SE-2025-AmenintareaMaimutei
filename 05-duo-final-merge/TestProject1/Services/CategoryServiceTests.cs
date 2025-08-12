using Xunit;
using Moq;
using DuoClassLibrary.Services;
using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestProject1.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CategoryService _categoryService;
        private readonly List<Category> _testCategories;

        public CategoryServiceTests()
        {
            _testCategories = new List<Category>
            {
                new Category { Id = 1, Name = "Technology" },
                new Category { Id = 2, Name = "Science" },
                new Category { Id = 3, Name = "Music" }
            };

            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockCategoryRepository.Setup(repo => repo.GetCategoriesAsync())
                .Returns(Task.FromResult(_testCategories));

            _categoryService = new CategoryService(_mockCategoryRepository.Object);
        }

        [Fact]
        public void Constructor_NullRepository_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CategoryService(null));
        }

        [Fact]
        public async Task GetAllCategories_ReturnsCategories()
        {
            var result = await _categoryService.GetAllCategories();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Technology", result[0].Name);
            Assert.Equal("Science", result[1].Name);
            Assert.Equal("Music", result[2].Name);
        }

        [Fact]
        public async Task GetAllCategories_HandlesException()
        {
            _mockCategoryRepository.Setup(repo => repo.GetCategoriesAsync())
                .ThrowsAsync(new Exception("Database error"));

            var result = await _categoryService.GetAllCategories();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCategoryByName_ValidName_ReturnsCategory()
        {
            var result = await _categoryService.GetCategoryByName("Technology");

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Technology", result.Name);
        }

        [Fact]
        public async Task GetCategoryByName_EmptyName_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _categoryService.GetCategoryByName(string.Empty));
        }

        [Fact]
        public async Task GetCategoryByName_NonExistentName_ReturnsNull()
        {
            var result = await _categoryService.GetCategoryByName("NonExistent");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetCategoryByName_HandlesException()
        {
            _mockCategoryRepository.Setup(repo => repo.GetCategoriesAsync())
                .ThrowsAsync(new Exception("Database error"));

            var result = await _categoryService.GetCategoryByName("Technology");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetCategoryNames_ReturnsCorrectList()
        {
            var result = await _categoryService.GetCategoryNames();

            Assert.Equal(3, result.Count);
            Assert.Contains("Technology", result);
            Assert.Contains("Science", result);
            Assert.Contains("Music", result);
        }

        [Fact]
        public async Task GetCategoryNames_HandlesException()
        {
            _mockCategoryRepository.Setup(repo => repo.GetCategoriesAsync())
                .ThrowsAsync(new Exception("Database error"));

            var result = await _categoryService.GetCategoryNames();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void IsValidCategoryName_NullOrEmpty_ReturnsFalse(string name)
        {
            var result = _categoryService.IsValidCategoryName(name);

            Assert.False(result);
        }

       
    }
}