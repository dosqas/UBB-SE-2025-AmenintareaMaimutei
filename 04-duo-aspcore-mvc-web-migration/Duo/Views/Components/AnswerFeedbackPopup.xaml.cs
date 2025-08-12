using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;

namespace Duo.Views.Components
{
    public sealed partial class AnswerFeedbackPopup : ContentDialog
    {
        private readonly SolidColorBrush greenBrush = new SolidColorBrush(Colors.Green);
        private readonly SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);

        public AnswerFeedbackPopup()
        {
            try
            {
                this.InitializeComponent();
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Initialization Error", $"Failed to initialize AnswerFeedbackPopup.\nDetails: {ex.Message}");
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
                Console.WriteLine($"Error dialog failed to display. Details: {ex.Message}");
            }
        }

        public void ShowCorrectAnswer(string correctAnswer)
        {
            try
            {
                FeedbackIcon.Glyph = "\uE73E"; // Checkmark icon
                FeedbackIcon.Foreground = greenBrush;
                FeedbackMessage.Text = "Correct! Well done!";
                FeedbackMessage.Foreground = greenBrush;
                CorrectAnswerText.Text = correctAnswer;
                CloseButton.Background = greenBrush;
                CloseButton.Foreground = new SolidColorBrush(Colors.White);
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Show Correct Answer Error", $"Failed to display correct answer feedback.\nDetails: {ex.Message}");
            }
        }

        public void ShowWrongAnswer(string correctAnswer)
        {
            try
            {
                FeedbackIcon.Glyph = "\uE783"; // X icon
                FeedbackIcon.Foreground = redBrush;
                FeedbackMessage.Text = "Incorrect. Keep trying!";
                FeedbackMessage.Foreground = redBrush;
                CorrectAnswerText.Text = correctAnswer;
                CloseButton.Background = redBrush;
                CloseButton.Foreground = new SolidColorBrush(Colors.White);
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Show Wrong Answer Error", $"Failed to display wrong answer feedback.\nDetails: {ex.Message}");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Hide();
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Close Dialog Error", $"Failed to close feedback dialog.\nDetails: {ex.Message}");
            }
        }
    }
}