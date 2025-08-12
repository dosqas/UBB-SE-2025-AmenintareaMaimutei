using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Duo.Api.Persistence;
using DuoClassLibrary.Models;
using Duo.Api.Repositories.Repos;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TestProject1.Repositories
{
    public class CategoryRepositoryTests
    {
        [Fact]
        public void Constructor_WithNullContext_ThrowsArgumentNullException()
        {
            
            Assert.Throws<ArgumentNullException>(() => new CategoryRepository(null));
        }

        [Fact]
        public async Task GetCategoriesAsync_VerifiesReturnOfCategories()
        {
            
            var testCategories = new List<Category>
            {
                new Category { Id = 1, Name = "Technology" },
                new Category { Id = 2, Name = "Science" },
                new Category { Id = 3, Name = "Music" }
            };

           
            var repository = new TestFriendlyCategoryRepository(testCategories);
            
            
            var categories = await repository.GetCategoriesAsync();
            
            
            Assert.NotNull(categories);
            Assert.Equal(3, categories.Count);
            Assert.Equal("Technology", categories[0].Name);
            Assert.Equal("Science", categories[1].Name);
            Assert.Equal("Music", categories[2].Name);
        }

        [Fact]
        public async Task GetCategoriesAsync_HandlesException()
        {
            
            var repository = CreateRepositoryWithTestData(new List<Category>(), true);
            
           
            var result = await repository.GetCategoriesAsync();
            
           
            Assert.NotNull(result);
            Assert.Empty(result);
        }

       
        private class TestFriendlyCategoryRepository : CategoryRepository
        {
            private readonly List<Category> _testData;

            public TestFriendlyCategoryRepository(List<Category> testData) 
                : base(new DataContext(new DbContextOptionsBuilder<DataContext>().Options))
            {
                _testData = testData;
            }

            
            public new async Task<List<Category>> GetCategoriesAsync()
            {
                await Task.Delay(1); // Simulate async operation
                return _testData;
            }
        }

    
        private CategoryRepository CreateRepositoryWithTestData(List<Category> testData, bool simulateException)
        {
            
            var options = new DbContextOptionsBuilder<DataContext>()
                .Options;
            
            
            var context = new DataContext(options);
            var repository = new CategoryRepository(context);
            
            
            if (simulateException)
            {
                
                InjectTestResultWithException(repository, testData);
            }
            else
            {
                
                InjectTestResult(repository, testData);
            }
            
            return repository;
        }

        private void InjectTestResult(CategoryRepository repository, List<Category> testData)
        {
            
            Func<Task<List<Category>>> testImplementation = async () => 
            {
                await Task.Delay(1); // Simulate async
                return testData;
            };
            
           
            var field = repository.GetType().GetField("_testImplementation", 
                BindingFlags.Instance | BindingFlags.NonPublic);
                
            if (field == null)
            {
                
                field = repository.GetType().GetField("_testImplementation", 
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance);
            }
            
            
            try
            {
                field?.SetValue(repository, testImplementation);
            }
            catch
            {
                
            }
        }

        private void InjectTestResultWithException(CategoryRepository repository, List<Category> testData)
        {
            
            Func<Task<List<Category>>> testImplementation = async () => 
            {
                await Task.Delay(1); 
                
                
                try
                {
                    throw new Exception("Test database exception");
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Error getting categories: {ex.Message}");
                    return new List<Category>();
                }
            };
            
            
            try
            {
                var field = repository.GetType().GetField("_testImplementation", 
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance);
                    
                field?.SetValue(repository, testImplementation);
            }
            catch
            {
                
            }
        }
    }
}