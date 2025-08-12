using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.ViewManagement;
using Duo.ViewModels.Base;

namespace Duo.Views.Components
{
    public sealed partial class FillInTheBlanksExercise : UserControl
    {
        public event EventHandler<FillInTheBlanksExerciseEventArgs> OnSendClicked;
        public event RoutedEventHandler Click;
        private Button selectedLeftButton;
        private Button selectedRightButton;

        public static readonly DependencyProperty QuestionProperty =
            DependencyProperty.Register(nameof(Question), typeof(string), typeof(FillInTheBlanksExercise), new PropertyMetadata(string.Empty));

        private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        private static readonly SolidColorBrush SelectedBrush = new SolidColorBrush(Microsoft.UI.Colors.Coral);

        public ObservableCollection<UIElement> QuestionElements { get; set; } = new ObservableCollection<UIElement>();

        public FillInTheBlanksExercise()
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
                _ = ShowErrorMessage("Initialization Error", $"Failed to initialize FillInTheBlanksExercise.\nDetails: {ex.Message}");
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
            set
            {
                try
                {
                    SetValue(QuestionProperty, value);
                    ParseQuestion(value);
                }
                catch (Exception ex)
                {
                    _ = ShowErrorMessage("Question Set Error", $"Failed to set question.\nDetails: {ex.Message}");
                }
            }
        }

        private void ParseQuestion(string question)
        {
            try
            {
                QuestionElements.Clear();
                var parts = Regex.Split(question, @"({})");
                var uiSettings = new UISettings();
                SolidColorBrush textColor = new SolidColorBrush(uiSettings.GetColorValue(UIColorType.Foreground));

                foreach (var part in parts)
                {
                    if (part.Contains("{}"))
                    {
                        var textBox = new TextBox
                        {
                            Width = 150,
                            Height = 40,
                            FontSize = 16,
                            PlaceholderText = "Type here...",
                            BorderThickness = new Thickness(1),
                            BorderBrush = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200)),
                            Background = new SolidColorBrush(Color.FromArgb(255, 245, 245, 245)),
                            Padding = new Thickness(8, 4, 8, 4),
                            Margin = new Thickness(4),
                            CornerRadius = new CornerRadius(4),
                            SelectionHighlightColor = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215)),
                            SelectionHighlightColorWhenNotFocused = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215)),
                            VerticalContentAlignment = VerticalAlignment.Center
                        };

                        textBox.GotFocus += (s, e) =>
                        {
                            try
                            {
                                textBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
                                textBox.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
                            }
                            catch (Exception ex)
                            {
                                _ = ShowErrorMessage("TextBox Focus Error", $"Failed to update TextBox focus style.\nDetails: {ex.Message}");
                            }
                        };

                        textBox.LostFocus += (s, e) =>
                        {
                            try
                            {
                                textBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
                                textBox.Background = new SolidColorBrush(Color.FromArgb(255, 245, 245, 245));
                            }
                            catch (Exception ex)
                            {
                                _ = ShowErrorMessage("TextBox Focus Error", $"Failed to update TextBox lost focus style.\nDetails: {ex.Message}");
                            }
                        };

                        QuestionElements.Add(textBox);
                    }
                    else
                    {
                        var textBlock = new TextBlock
                        {
                            Text = part,
                            FontSize = 16,
                            Foreground = textColor,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(4)
                        };
                        QuestionElements.Add(textBlock);
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Parse Question Error", $"Failed to parse question.\nDetails: {ex.Message}");
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> inputValues = QuestionElements
                    .OfType<TextBox>()
                    .Select(textBox => textBox.Text)
                    .ToList();

                OnSendClicked?.Invoke(this, new FillInTheBlanksExerciseEventArgs(inputValues));
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Send Click Error", $"Failed to process send action.\nDetails: {ex.Message}");
            }
        }

        public class FillInTheBlanksExerciseEventArgs : EventArgs
        {
            public List<string> ContentPairs { get; }

            public FillInTheBlanksExerciseEventArgs(List<string> inputValues)
            {
                ContentPairs = inputValues;
            }
        }
    }
}