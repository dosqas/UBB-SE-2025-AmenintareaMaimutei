using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Duo.Helpers;
using Duo.Views.Pages;
using Duo.ViewModels.Base;

namespace Duo.Views.Components
{
    public sealed partial class RoadmapSectionView : UserControl
    {
        public event RoutedEventHandler Click;

        public RoadmapSectionView()
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

        private void Quiz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is QuizRoadmapButton button)
                {
                    Debug.WriteLine($"Quiz with ID {button.QuizId} clicked!");

                    Frame parentFrame = Helpers.Helpers.FindParent<Frame>(this);
                    if (parentFrame != null)
                    {
                        parentFrame.Navigate(typeof(QuizPreviewPage), (button.QuizId, button.IsExam));
                    }
                    else
                    {
                        _ = ShowErrorMessage("Navigation Error", "Failed to find parent frame for navigation.");
                    }
                }
                else
                {
                    _ = ShowErrorMessage("Click Error", "Invalid button type clicked.");
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Navigation Error", $"Failed to navigate to quiz preview.\nDetails: {ex.Message}");
            }
        }
    }
}