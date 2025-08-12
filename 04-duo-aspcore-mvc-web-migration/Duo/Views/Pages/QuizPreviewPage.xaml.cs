using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Duo.ViewModels;
using Duo.ViewModels.Base;

namespace Duo.Views.Pages
{
    public sealed partial class QuizPreviewPage : Page
    {
        public QuizPreviewPage()
        {
            try
            {
                this.InitializeComponent();
                if (this.DataContext is ViewModelBase viewModel)
                {
                    viewModel.ShowErrorMessageRequested += ViewModel_ShowErrorMessageRequested;
                }
                else
                {
                    _ = ShowErrorMessage("Initialization Error", "DataContext is not set to a valid ViewModel.");
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Initialization Error", $"Failed to initialize QuizPreviewPage.\nDetails: {ex.Message}");
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
                if (e.Parameter is ValueTuple<int, bool> parameters)
                {
                    int quizId = parameters.Item1;
                    bool isExam = parameters.Item2;

                    await QuizPreview.Load(quizId, isExam);
                }
                else
                {
                    await ShowErrorMessage("Navigation Error", "Invalid navigation parameters.");
                }

                base.OnNavigatedTo(e);
            }
            catch (Exception ex)
            {
                await ShowErrorMessage("Navigation Error", $"Failed to navigate to QuizPreviewPage.\nDetails: {ex.Message}");
            }
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Frame.CanGoBack)
                {
                    this.Frame.GoBack();
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Back Navigation Error", $"Failed to navigate back.\nDetails: {ex.Message}");
            }
        }
    }
}