using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DuoClassLibrary.Models.Quizzes;

namespace Duo.Views.Pages
{
    public sealed partial class EndQuizPage : Page
    {
        private readonly Quiz quiz;
        private readonly TimeSpan timeTaken;

        public EndQuizPage(Quiz quiz, TimeSpan timeTaken)
        {
            this.InitializeComponent();
            this.quiz = quiz;
            this.timeTaken = timeTaken;

            DisplayResults();
        }

        private void DisplayResults()
        {
            double scorePercentage = ((double)quiz.GetNumberOfCorrectAnswers() / quiz.GetNumberOfAnswersGiven()) * 100;

            ScoreTextBlock.Text = $"{quiz.GetNumberOfCorrectAnswers()}/{quiz.GetNumberOfAnswersGiven()} ({scorePercentage:F1}%)";
            TimeTextBlock.Text = $"{timeTaken.Minutes}m {timeTaken.Seconds}s";

            if (scorePercentage >= quiz.GetPassingThreshold())
            {
                FeedbackTextBlock.Text = "Great job! You've passed the quiz!";
                FeedbackTextBlock.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
            }
            else
            {
                FeedbackTextBlock.Text = "Keep practicing! You can do better next time.";
                FeedbackTextBlock.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
            }
        }

        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
                else
                {
                    Frame.Navigate(typeof(RoadmapMainPage));
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage("Navigation Error", ex.Message);
            }
        }

        private async Task ShowErrorMessage(string title, string message)
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
    }
}
