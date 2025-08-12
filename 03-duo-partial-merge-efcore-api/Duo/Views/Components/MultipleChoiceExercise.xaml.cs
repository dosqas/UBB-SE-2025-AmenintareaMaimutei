using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.ViewManagement;
using Duo.ViewModels.Base;
using Duo.Models.Exercises;

namespace Duo.Views.Components
{
    public sealed partial class MultipleChoiceExercise : UserControl
    {
        public event EventHandler<MultipleChoiceExerciseEventArgs> OnSendClicked;
        public event RoutedEventHandler Click;
        private Button selectedLeftButton;
        private Button selectedRightButton;

        public static readonly DependencyProperty QuestionProperty =
            DependencyProperty.Register(nameof(Question), typeof(string), typeof(MultipleChoiceExercise), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty AnswersProperty =
            DependencyProperty.Register(nameof(Answers), typeof(ObservableCollection<MultipleChoiceAnswerModel>), typeof(MultipleChoiceExercise), new PropertyMetadata(new ObservableCollection<MultipleChoiceAnswerModel>()));

        private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        private List<Button> selectedButtons = new List<Button>();

        public MultipleChoiceExercise()
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
                _ = ShowErrorMessage("Initialization Error", $"Failed to initialize MultipleChoiceExercise.\nDetails: {ex.Message}");
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

        public string Question
        {
            get => (string)GetValue(QuestionProperty);
            set => SetValue(QuestionProperty, value);
        }

        public ObservableCollection<MultipleChoiceAnswerModel> Answers
        {
            get => (ObservableCollection<MultipleChoiceAnswerModel>)GetValue(AnswersProperty);
            set => SetValue(AnswersProperty, value);
        }

        public Color GetSystemAccentColor()
        {
            try
            {
                var uiSettings = new UISettings();
                return uiSettings.GetColorValue(UIColorType.Accent);
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Color Error", $"Failed to get system accent color.\nDetails: {ex.Message}");
                return Microsoft.UI.Colors.Gray; // Fallback color
            }
        }

        public Color GetSystemAccentTextColor()
        {
            try
            {
                var uiSettings = new UISettings();
                return uiSettings.GetColorValue(UIColorType.Complement);
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Color Error", $"Failed to get system accent text color.\nDetails: {ex.Message}");
                return Microsoft.UI.Colors.Black; // Fallback color
            }
        }

        private void SetDefaultButtonStyles(Button clickedButton)
        {
            try
            {
                clickedButton.Background = TransparentBrush;
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Button Style Error", $"Failed to set default button styles.\nDetails: {ex.Message}");
            }
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clickedButton = sender as Button;
                if (clickedButton == null)
                {
                    _ = ShowErrorMessage("Click Error", "Invalid button clicked.");
                    return;
                }

                if (selectedButtons.Contains(clickedButton))
                {
                    SetDefaultButtonStyles(clickedButton);
                    selectedButtons.Remove(clickedButton);
                }
                else
                {
                    foreach (var selectedButton in selectedButtons)
                    {
                        SetDefaultButtonStyles(selectedButton);
                    }
                    selectedButtons.Clear();
                    clickedButton.Background = new SolidColorBrush(GetSystemAccentColor());
                    selectedButtons.Add(clickedButton);
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Option Click Error", $"Failed to handle option click.\nDetails: {ex.Message}");
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> userChoices = selectedButtons.Select(b => b.Content.ToString()).ToList();
                OnSendClicked?.Invoke(this, new MultipleChoiceExerciseEventArgs(userChoices));
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Send Click Error", $"Failed to process send action.\nDetails: {ex.Message}");
            }
        }

        public class MultipleChoiceExerciseEventArgs : EventArgs
        {
            public List<string> ContentPairs { get; }

            public MultipleChoiceExerciseEventArgs(List<string> userChoices)
            {
                ContentPairs = userChoices;
            }
        }
    }
}