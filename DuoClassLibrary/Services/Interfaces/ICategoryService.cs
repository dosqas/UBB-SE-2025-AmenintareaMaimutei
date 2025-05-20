using System.Collections.Generic;
using System.Threading.Tasks;
using DuoClassLibrary.Models;

namespace DuoClassLibrary.Services.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        Task<List<Category>> GetAllCategories();

        /// <summary>
        /// Gets a category by name.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>The category, or null if not found.</returns>
        Task<Category> GetCategoryByName(string name);

        /// <summary>
        /// Gets all category names.
        /// </summary>
        /// <returns>A list of category names.</returns>
        Task<List<string>> GetCategoryNames();

        /// <summary>
        /// Checks if a category name is valid.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns>True if the category name is valid, false otherwise.</returns>
        bool IsValidCategoryName(string name);
    }
} 