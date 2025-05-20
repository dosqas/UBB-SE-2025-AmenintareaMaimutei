using DuoClassLibrary.Models;

namespace DuoClassLibrary.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetCategoriesAsync();
    }
}
