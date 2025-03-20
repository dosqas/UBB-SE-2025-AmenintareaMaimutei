using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using Project.Models;
using Project.ClassModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Project.Gui.ConsultationReview
{

    public sealed partial class ConsultationReviewPage : Page
    {
        private int _selectedRating = 0;
        private Guid _medicalRecordID;
        private ReviewModel _reviewModel = new();

        public ConsultationReviewPage(string doctorName, DateTime time, Guid medicalRecordID)
        {
            //TODO: add check if medicalRecordID exists or make sure it is initiated with a correct one
            _medicalRecordID = medicalRecordID;
            this.InitializeComponent();
            DoctorNameText.Text = doctorName;
            ConsultationDateText.Text = time.ToString("yyyy-MM-dd HH:mm");
            UpdateStarButtons();
        }

        private void StarClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button starButton && starButton.Tag is string tag)
            {
                if(int.TryParse(tag, out int starNumber))
                {
                    _selectedRating = starNumber;
                    UpdateStarButtons();
                    RatingError.Visibility = Visibility.Collapsed;
                }
            }    
        }

        private void UpdateStarButtons()
        {
            for(int i = 1; i <= 5; i++ )
            {
                if(FindName($"Star{i}") is Button starButton)
                {
                    starButton.Foreground = i <= _selectedRating ? new SolidColorBrush(Microsoft.UI.Colors.Gold) : new SolidColorBrush(Microsoft.UI.Colors.Gray);
                }
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (FeedbackTextBox.Text.Length < 5 || FeedbackTextBox.Text.Length > 255)
            {
                FeedbackError.Text = "Feedback must be between 5 and 255 characters.";
                FeedbackError.Visibility = Visibility.Visible;
            }
            else
            {
                FeedbackError.Visibility = Visibility.Collapsed;
            }

            if (_selectedRating < 1 || _selectedRating > 5)
            {
                RatingError.Text = "Please select a rating.";
                RatingError.Visibility = Visibility.Visible;
            }
            else
            {
                RatingError.Visibility = Visibility.Collapsed;
            }

            if (FeedbackError.Visibility == Visibility.Collapsed && RatingError.Visibility == Visibility.Collapsed)
            {
                Review review = new Review(
                    reviewID: Guid.NewGuid(),
                    medicalRecordID: _medicalRecordID,
                    text: FeedbackTextBox.Text,
                    nrStars: _selectedRating
                );

                bool isSuccess = _reviewModel.AddReview(review);

                if (isSuccess)
                {
                    StatusMessage.Text = "Thank you for your feedback!";
                    StatusMessage.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    StatusMessage.Text = "Failed to submit feedback. Please try again later.";
                    StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                }


            }
            else
            {
                StatusMessage.Text = "Please fix the errors before submitting.";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
