using Duo.Api.Persistence;
using Duo.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Duo.Api.Models;

namespace Duo.Api.Repositories.Repos
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error getting categories: {ex.Message}");
                return new List<Category>();
            }
        }
    }
}