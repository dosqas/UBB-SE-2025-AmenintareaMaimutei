using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Duo.ViewModels;

namespace Duo.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel viewModel;

        public MainPage()
        {
            try
            {
                this.InitializeComponent();
                viewModel = new MainPageViewModel();
                viewModel.NavigationRequested += OnNavigationRequested;
                viewModel.ShowErrorMessageRequested += ViewModel_ShowErrorMessageRequested;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Initialization error: {ex.Message}");
            }
        }

        private async void ViewModel_ShowErrorMessageRequested(object sender, (string Title, string Message) e)
        {
            try
            {
                await ShowErrorMessage(e.Title, e.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to show error dialog: {ex.Message}");
            }
        }

        private async Task ShowErrorMessage(string title, string message)
        {
            try
            {
                var dialog = new ContentDialog
                {
                    Title = title,
                    Content = message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ContentDialog error: {ex.Message}");
            }
        }

        private void NavigationView_SelectionChanged(object sender, NavigationViewSelectionChangedEventArgs args)
        {
            try
            {
                viewModel.HandleNavigationSelectionChanged(args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigation selection error: {ex.Message}");
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
                Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }
    }
}
