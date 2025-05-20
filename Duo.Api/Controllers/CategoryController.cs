using Microsoft.AspNetCore.Mvc;
using Duo.Api.Persistence;
using Duo.Api.Repositories.Repos;
using Duo.Api.Models;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository _categoryRepository)
        {
            this._categoryRepository = _categoryRepository;
        }

        [HttpGet(Name = "GetAllCategories")]
        public async Task<IEnumerable<Category>> Get()
        {
            return await _categoryRepository.GetCategoriesAsync();
        }
    }
}
