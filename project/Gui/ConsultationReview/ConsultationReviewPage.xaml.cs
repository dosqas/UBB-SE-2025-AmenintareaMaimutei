namespace Project.Gui.ConsultationReview
{
    using System;
    using Microsoft.UI;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media;
    using Project.ClassModels;
    using Project.Models;

    /// <summary>
    /// ConsultationReview Page.
    /// </summary>
    public sealed partial class ConsultationReviewPage : Page
    {
        private int reviewID;
        private int selectedRating = 0;
        private int medicalRecordID;
        private ReviewModel reviewModel = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsultationReviewPage"/> class.
        /// </summary>
        /// <param name="doctorName">Name of the doctor.</param>
        /// <param name="time">Time of the consultation.</param>
        /// <param name="medicalRecordID">The ID of the medical record.</param>
        public ConsultationReviewPage(string doctorName, DateTime time, int medicalRecordID)
        {
            // TODO: add check if medicalRecordID exists or make sure it is initiated with a correct one
            this.medicalRecordID = medicalRecordID;
            this.InitializeComponent();
            this.DoctorNameText.Text = doctorName;
            this.ConsultationDateText.Text = time.ToString("yyyy-MM-dd HH:mm");
            this.UpdateStarButtons();
        }

        private void StarClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button starButton && starButton.Tag is string tag)
            {
                if (int.TryParse(tag, out int starNumber))
                {
                    this.selectedRating = starNumber;
                    this.UpdateStarButtons();
                    this.RatingError.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void UpdateStarButtons()
        {
            for (int i = 1; i <= 5; i++)
            {
                if (this.FindName($"Star{i}") is Button starButton)
                {
                    starButton.Foreground = i <= this.selectedRating ? new SolidColorBrush(Microsoft.UI.Colors.Gold) : new SolidColorBrush(Microsoft.UI.Colors.Gray);
                }
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.FeedbackTextBox.Text.Length < 5 || this.FeedbackTextBox.Text.Length > 255)
            {
                this.FeedbackError.Text = "Feedback must be between 5 and 255 characters.";
                this.FeedbackError.Visibility = Visibility.Visible;
            }
            else
            {
                this.FeedbackError.Visibility = Visibility.Collapsed;
            }

            if (this.selectedRating < 1 || this.selectedRating > 5)
            {
                this.RatingError.Text = "Please select a rating.";
                this.RatingError.Visibility = Visibility.Visible;
            }
            else
            {
                this.RatingError.Visibility = Visibility.Collapsed;
            }

            if (this.FeedbackError.Visibility == Visibility.Collapsed && this.RatingError.Visibility == Visibility.Collapsed)
            {
                Review review = new Review(
                    reviewID: 0,
                    medicalRecordID: this.medicalRecordID,
                    text: this.FeedbackTextBox.Text,
                    nrStars: this.selectedRating);

                bool isSuccess = this.reviewModel.AddReview(review);

                if (isSuccess)
                {
                    this.StatusMessage.Text = "Thank you for your feedback!";
                    this.StatusMessage.Foreground = new SolidColorBrush(Colors.Green);

                    if (sender is Button submitButton)
                    {
                        submitButton.IsEnabled = false;
                    }
                }
                else
                {
                    this.StatusMessage.Text = "Failed to submit feedback. Please try again later.";
                    this.StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                this.StatusMessage.Text = "Please fix the errors before submitting.";
                this.StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
