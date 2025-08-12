using DuoClassLibrary.Models;
using DuoClassLibrary.Services.Interfaces;
using DuoClassLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebServerTest.ViewComponents
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public NavBarViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Category> categories = await _categoryService.GetAllCategories();
            return View(categories);
        }
    }
} 