using System;
using System.Collections.Generic;
using System.Windows.Input;
using Duo.Commands;
using DuoClassLibrary.Models;
using Duo.ViewModels.Base;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using System.Threading.Tasks;
using DuoClassLibrary.Services.Interfaces;

namespace Duo.ViewModels
{
    public class CategoryPageViewModel : ViewModelBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly CategoryViewModel _categoryViewModel;
        
        private int _currentCategoryId = 0;
        private string _currentCategoryName = string.Empty;
        private string _username = "Guest";
        private bool _isLoading;

        public event EventHandler<Type> NavigationRequested;
        public event EventHandler<string> CategoryNavigationRequested;
        public event EventHandler<bool> PostCreationSucceeded;

        public CategoryPageViewModel()
        {
            _categoryService = App._categoryService;
            _userService = App.userService;
            _categoryViewModel = new CategoryViewModel(_categoryService);
            
            _ = InitializeAsync();
            
            SelectCategoryCommand = new RelayCommandWithParameter<string>(SelectCategory);
            CreatePostCommand = new RelayCommand(_ => CreatePost());
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public int CurrentCategoryId
        {
            get => _currentCategoryId;
            set => SetProperty(ref _currentCategoryId, value);
        }

        public string CurrentCategoryName
        {
            get => _currentCategoryName;
            set => SetProperty(ref _currentCategoryName, value);
        }

        public List<string> CategoryNames => _categoryViewModel.GetCategoryNames();

        public ICommand SelectCategoryCommand { get; }
        public ICommand CreatePostCommand { get; }

        private async Task InitializeAsync()
        {
            try
            {
                IsLoading = true;
                await GetUserInfoAsync();
                await _categoryViewModel.LoadCategoriesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Initialization failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetUserInfoAsync()
        {
            try
            {
                User currentUser = _userService.GetCurrentUser();
                Username = currentUser.UserName;
            }
            catch (Exception ex)
            {
                Username = "Guest";
                Debug.WriteLine($"Failed to get username: {ex.Message}");
            }
        }

        public void HandleNavigationSelectionChanged(NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is not NavigationViewItem selectedItem || string.IsNullOrEmpty(selectedItem.Tag?.ToString()))
                return;

            // Skip if it's a parent menu item
            if (selectedItem.MenuItems.Count > 0)
                return;

            var tag = selectedItem.Tag.ToString();

            if (IsCategoryTag(tag))
            {
                HandleCategorySelection(tag);
            }
            else
            {
                HandlePageNavigation(tag);
            }
        }

        private async void HandleCategorySelection(string categoryName)
        {
            CurrentCategoryName = categoryName;
            
            try
            {
                var category = await _categoryService.GetCategoryByName(categoryName);
                if (category != null)
                {
                    CurrentCategoryId = category.Id;
                    CategoryNavigationRequested?.Invoke(this, categoryName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting category ID: {ex.Message}");
            }
        }

        private void HandlePageNavigation(string tag)
        {
            CurrentCategoryName = string.Empty;
            CurrentCategoryId = 0;
            
            if (tag == "MainPage")
            {
                NavigationRequested?.Invoke(this, typeof(Views.Pages.MainPage));
            }
            else
            {
                Debug.WriteLine($"Unknown page tag: {tag}");
            }
        }

        private async void SelectCategory(string category)
        {
            if (IsCategoryTag(category))
            {
                CurrentCategoryName = category;
                try
                {
                    var categoryObj = await _categoryService.GetCategoryByName(category);
                    if (categoryObj != null)
                    {
                        CurrentCategoryId = categoryObj.Id;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error getting category ID: {ex.Message}");
                }
                
                CategoryNavigationRequested?.Invoke(this, category);
            }
        }

        private bool IsCategoryTag(string tag)
        {
            return CategoryNames.Contains(tag);
        }

        private void CreatePost()
        {
            // This is just a placeholder as the actual post creation is handled via the dialog
            // The view will handle showing the dialog when this command is executed
        }

        public async void HandlePostCreation(bool success)
        {
            if (success && !string.IsNullOrEmpty(CurrentCategoryName))
            {
                // Notify the view to refresh the content
                PostCreationSucceeded?.Invoke(this, true);
                
                // Navigate back to the category to refresh the posts
                CategoryNavigationRequested?.Invoke(this, CurrentCategoryName);
            }
        }
    }
} 