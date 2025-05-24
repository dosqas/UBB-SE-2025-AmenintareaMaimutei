using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using DuoClassLibrary.Models;
using System.Collections.Generic;
using Duo.ViewModels;
using Duo.Views.Components;
using static Duo.App;
using DuolingoNou.Views.Pages;
using System.Threading.Tasks;

namespace Duo.Views.Pages
{
    public sealed partial class CategoryPage : Page
    {
        private CategoryPageViewModel _viewModel;

        public CategoryPage()
        {
            try
            {
                this.InitializeComponent();
                _ = InitializeAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Page initialization failed: {ex.Message}");
            }
        }

        private async Task InitializeAsync()
        {
            try
            {
                _viewModel = new CategoryPageViewModel();
                this.DataContext = _viewModel;

                // Subscribe to events
                _viewModel.NavigationRequested += OnNavigationRequested;
                _viewModel.CategoryNavigationRequested += OnCategoryNavigationRequested;
                _viewModel.PostCreationSucceeded += OnPostCreationSucceeded;

                // Wait for categories to load
                while (_viewModel.IsLoading)
                {
                    await Task.Delay(100);
                }

                PopulateCommunityMenuItems();
                
                // Set the username from the view model
                UsernameTextBlock.Text = _viewModel.Username;

                try
                {
                    contentFrame.Navigate(typeof(MainPage));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Initial navigation failed: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Async initialization failed: {ex.Message}");
            }
        }

        private void PopulateCommunityMenuItems()
        {
            try
            {
                var categoryNames = _viewModel.CategoryNames;
                if (categoryNames == null || categoryNames.Count == 0)
                {
                    Debug.WriteLine("No categories found or categoryNames is null");
                    return;
                }

                CommunityItem.MenuItems.Clear();

                foreach (string categoryName in categoryNames)
                {
                    if (string.IsNullOrEmpty(categoryName))
                    {
                        Debug.WriteLine("Skipping null or empty category name");
                        continue;
                    }

                    var item = new NavigationViewItem
                    {
                        Content = categoryName.Replace("-", " "),
                        Icon = new SymbolIcon(Symbol.Message),
                        Tag = categoryName
                    };

                    ToolTipService.SetToolTip(item, categoryName.Replace("-", " "));
                    CommunityItem.MenuItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error populating community menu items: {ex.Message}");
            }
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                switch (selectedItem.Tag)
                {
                    case "Settings":
                        contentFrame.Navigate(typeof(ProfileSettingsPage));
                        break;
                    case "HomePage":
                        contentFrame.Navigate(typeof(MainPage));
                        break;
                    default:
                        if (selectedItem.Tag is string categoryName)
                        {
                            contentFrame.Navigate(typeof(PostListPage), categoryName);
                        }
                        break;
                }
            }
        }

        private void OnNavigationRequested(object sender, Type pageType)
        {
            try
            {
                contentFrame.Navigate(pageType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigation failed: {ex.Message}");
            }
        }

        private void OnCategoryNavigationRequested(object sender, string category)
        {
            try
            {
                contentFrame.Navigate(typeof(PostListPage), category);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Category navigation failed: {ex.Message}");
            }
        }

        private void OnPostCreationSucceeded(object sender, bool success)
        {
            // This would handle any UI updates needed when post creation succeeds
            // Currently nothing specific is needed here as we navigate to refresh the list
        }

        private async void CreatePostBtn_CreatePostRequested(object sender, RoutedEventArgs e) 
        {
            // First approach: Use the dialog with the embedded ViewModel
            var dialogComponent = new DialogComponent();
            var result = await dialogComponent.ShowCreatePostDialog(this.XamlRoot, _viewModel.CurrentCategoryId);
            
            if (result.Success)
            {
                // The post is already created in the database through the ViewModel
                // Just show the success message and refresh the page
                
                // Show success message
                ContentDialog successDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Success",
                    Content = "Your post was created successfully!",
                    CloseButtonText = "OK"
                };
                await successDialog.ShowAsync();
                
                // Use the view model to handle post creation success
                _viewModel.HandlePostCreation(true);
            }
        }

        public void RefreshCurrentView()
        {
            try
            {
                if (!string.IsNullOrEmpty(_viewModel.CurrentCategoryName))
                {
                    contentFrame.Navigate(typeof(PostListPage), _viewModel.CurrentCategoryName);
                }
                else
                {
                    contentFrame.Navigate(typeof(MainPage));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Refresh current view failed: {ex.Message}");
            }
        }
    }
}