using System;
using System.Windows.Input;
using DuoClassLibrary.Models;
using Duo.Services;
using Microsoft.UI.Xaml;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Duo.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DuoClassLibrary.Services.Interfaces;

namespace Duo.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private readonly ICategoryService _categoryService;
        private string _categoryName = string.Empty;
        private List<Category> _categories = new List<Category>();
        private bool _isLoading;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Category> Categories
        {
            get => _categories;
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public CategoryViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _ = LoadCategoriesAsync();
        }

        public async Task LoadCategoriesAsync()
        {
            try
            {
                IsLoading = true;
                Categories = await _categoryService.GetAllCategories();
            }
            catch (Exception ex)
            {
                // Log the error or show a message to the user
                Console.WriteLine($"Error loading categories: {ex.Message}");
                Categories = new List<Category>();
            }
            finally
            {
                IsLoading = false;
            }
        }

        public List<string> GetCategoryNames()
        {
            if (Categories == null || Categories.Count == 0)
            {
                return new List<string>();
            }
            var catNames = Categories.Select(c => c.Name).ToList();
            return catNames;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
