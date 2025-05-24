using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Duo.ViewModels.Base;
using Duo.ViewModels.Roadmap;

namespace Duo.Views.Pages
{
    public sealed partial class RoadmapMainPage : Page
    {
        public RoadmapMainPage()
        {
            try
            {
                this.InitializeComponent();
                if (ViewModel is ViewModelBase viewModel)
                {
                    viewModel.ShowErrorMessageRequested += ViewModel_ShowErrorMessageRequested;
                }
                else
                {
                    _ = ShowErrorMessage("Initialization Error", "ViewModel is not set to a valid ViewModelBase instance.");
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Initialization Error", $"Failed to initialize RoadmapMainPage.\nDetails: {ex.Message}");
            }
        }

        private async void ViewModel_ShowErrorMessageRequested(object sender, (string Title, string Message) e)
        {
            await ShowErrorMessage(e.Title, e.Message);
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
                Console.WriteLine($"Error dialog failed to display. Details: {ex.Message}");
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (ViewModel is RoadmapMainPageViewModel viewModel)
                {
                    await viewModel.SetupViewModel();
                }
                else
                {
                    _ = ShowErrorMessage("Navigation Error", "ViewModel is not set to a valid RoadmapMainPageViewModel.");
                }
                base.OnNavigatedTo(e);
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Navigation Error", $"Failed to set up RoadmapMainPage.\nDetails: {ex.Message}");
            }
        }
    }
}