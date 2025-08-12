using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Duo.Models.Quizzes;
using Duo.ViewModels.Base;
using Duo.ViewModels.Roadmap;
using Duo.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Duo.Views.Components
{
    public sealed partial class RoadmapQuizPreview : UserControl
    {
        public RoadmapQuizPreview()
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
                _ = ShowErrorMessage("Initialization Error", $"Failed to initialize RoadmapQuizPreview.\nDetails: {ex.Message}");
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

        public void OpenQuizButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && this.DataContext is RoadmapQuizPreviewViewModel viewModel)
                {
                    Frame parentFrame = Helpers.Helpers.FindParent<Frame>(this);
                    if (parentFrame != null)
                    {
                        if (viewModel.Quiz is Exam)
                        {
                            Debug.WriteLine("Navigating to Exam");
                            parentFrame.Navigate(typeof(QuizPage), (viewModel.Quiz.Id, true));
                        }
                        else
                        {
                            Debug.WriteLine("Navigating to Quiz");
                            parentFrame.Navigate(typeof(QuizPage), (viewModel.Quiz.Id, false));
                        }
                    }
                    else
                    {
                        _ = ShowErrorMessage("Navigation Error", "Failed to find parent frame for navigation.");
                    }
                }
                else
                {
                    _ = ShowErrorMessage("Click Error", "Invalid button or ViewModel type.");
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Navigation Error", $"Failed to navigate to quiz page.\nDetails: {ex.Message}");
            }
        }

        public async Task Load(int quizId, bool isExam)
        {
            try
            {
                if (this.DataContext is RoadmapQuizPreviewViewModel viewModel)
                {
                    await viewModel.OpenForQuiz(quizId, isExam);
                }
                else
                {
                    _ = ShowErrorMessage("Load Error", "DataContext is not set to a valid ViewModel.");
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Load Error", $"Failed to load quiz with ID {quizId}.\nDetails: {ex.Message}");
            }
        }
    }
}