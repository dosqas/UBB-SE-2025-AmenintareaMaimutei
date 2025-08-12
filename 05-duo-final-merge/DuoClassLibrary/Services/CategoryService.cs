using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using DuoClassLibrary.Services.Interfaces;

namespace DuoClassLibrary.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        // Constants for error messages
        private const string ErrorFetchingCategories = "Error fetching categories: {0}";
        private const string ErrorFetchingCategory = "Error fetching category: {0}";
        private const string CategoryNotFound = "Category not found";
        private const string CategoryNameEmptyError = "Category name cannot be empty";
        private const string CategoriesListNull = "Categories list is null";

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        /// <inheritdoc/>
        public async Task<List<Category>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetCategoriesAsync();
                return categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(ErrorFetchingCategories, ex.Message));
                return new List<Category>();
            }
        }

        /// <inheritdoc/>
        public async Task<Category> GetCategoryByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(CategoryNameEmptyError);
            }

            try
            {
                var categories = await this._categoryRepository.GetCategoriesAsync();
                if (categories == null)
                {
                    Console.WriteLine(CategoriesListNull);
                    return null;
                }

                Category category = categories.FirstOrDefault(c => c.Name == name);

                if (category == null)
                {
                    Console.WriteLine($"Category with name '{name}' not found");
                    return null;
                }

                return category;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(ErrorFetchingCategory, ex.Message));
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<string>> GetCategoryNames()
        {
            var categories = await GetAllCategories();
            if (categories == null)
            {
                return new List<string>();
            }
            return categories.ConvertAll(c => c?.Name ?? string.Empty);
        }

        /// <inheritdoc/>
        public bool IsValidCategoryName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            // Note: This implementation is synchronous because we can't use await in a sync method
            // In a real application, you might want to redesign this approach
            var task = GetCategoryByName(name);
            task.Wait();
            var category = task.Result;
            return category != null;
        }
    }
} 