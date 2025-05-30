using Microsoft.AspNetCore.Mvc;
using Duo.Api.Persistence;
using Duo.Api.Repositories.Interfaces;
using Duo.Api.Models;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        [HttpGet(Name = "GetAllCategories")]
        public async Task<IEnumerable<Category>> Get()
        {
            return await _categoryRepository.GetCategoriesAsync();
        }
    }
}
