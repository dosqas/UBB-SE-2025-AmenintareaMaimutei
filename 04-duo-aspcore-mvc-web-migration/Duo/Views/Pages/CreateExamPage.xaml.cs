using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using DuoClassLibrary.Models.Exercises;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Duo.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateExamPage : Page
    {
        public CreateExamPage()
        {
            this.InitializeComponent();
            this.Loaded += CreateExamPage_Loaded;
        }

        private void CreateExamPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowListViewModal += ViewModel_openSelectExercises;
            ViewModel.RequestGoBack += ViewModel_RequestGoBack;
            ViewModel.ShowErrorMessageRequested += ViewModel_ShowErrorMessageRequested;
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
                // You may optionally log this or suppress it to avoid recursive dialog errors
                Console.WriteLine($"Error dialog failed to display. Details: {ex.Message}");
            }
        }

        public void ViewModel_RequestGoBack(object sender, EventArgs e)
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
                _ = ShowErrorMessage("Navigation Error", $"Failed to navigate back.\nDetails: {ex.Message}");
            }
        }

        private async void ViewModel_openSelectExercises(List<Exercise> exercises)
        {
            try
            {
                var dialog = new ContentDialog
                {
                    Title = "Select Exercise",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.XamlRoot
                };

                var listView = new ListView
                {
                    ItemsSource = exercises,
                    SelectionMode = ListViewSelectionMode.Single,
                    MaxHeight = 300,
                    ItemTemplate = (DataTemplate)Resources["ExerciseSelectionItemTemplate"]
                };

                dialog.Content = listView;
                dialog.PrimaryButtonText = "Add";
                dialog.IsPrimaryButtonEnabled = false;

                listView.SelectionChanged += (s, args) =>
                {
                    dialog.IsPrimaryButtonEnabled = listView.SelectedItem != null;
                };

                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary && listView.SelectedItem is Exercise selectedExercise)
                {
                    ViewModel.AddExercise(selectedExercise);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage("Dialog Error", $"Failed to open exercise selection dialog.\nDetails: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
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
                _ = ShowErrorMessage("Navigation Error", $"Failed to cancel and go back.\nDetails: {ex.Message}");
            }
        }
    }
}
