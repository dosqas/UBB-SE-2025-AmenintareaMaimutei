using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.ViewManagement;
using Duo.ViewModels.Base;
using Windows.Foundation;
using Microsoft.UI.Xaml.Shapes;

namespace Duo.Views.Components
{
    public sealed partial class AssociationExercise : UserControl
    {
        public event EventHandler<AssociationExerciseEventArgs> OnSendClicked;
        public event RoutedEventHandler Click;
        private Button selectedLeftButton;
        private Button selectedRightButton;

        public static readonly DependencyProperty QuestionProperty =
            DependencyProperty.Register(nameof(Question), typeof(string), typeof(AssociationExercise), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty FirstAnswersListProperty =
            DependencyProperty.Register(nameof(FirstAnswersList), typeof(ObservableCollection<string>), typeof(AssociationExercise), new PropertyMetadata(new ObservableCollection<string>()));

        public static readonly DependencyProperty SecondAnswersListProperty =
            DependencyProperty.Register(nameof(SecondAnswersList), typeof(ObservableCollection<string>), typeof(AssociationExercise), new PropertyMetadata(new ObservableCollection<string>()));

        private static readonly UISettings UiSettings = new UISettings();
        private readonly SolidColorBrush accentBrush;

        private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        private static readonly SolidColorBrush SelectedBrush = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
        private static readonly SolidColorBrush MappedBrush = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
        private static readonly SolidColorBrush DefaultBorderBrush = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
        private static readonly SolidColorBrush LineBrush = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));

        private List<Tuple<Button, Button, Line>> pairs = new List<Tuple<Button, Button, Line>>();

        public AssociationExercise()
        {
            try
            {
                this.InitializeComponent();
                accentBrush = new SolidColorBrush(UiSettings.GetColorValue(UIColorType.Accent));
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
                _ = ShowErrorMessage("Initialization Error", $"Failed to initialize AssociationExercise.\nDetails: {ex.Message}");
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

        public ObservableCollection<string> FirstAnswersList
        {
            get
            {
                try
                {
                    var list = (ObservableCollection<string>)GetValue(FirstAnswersListProperty);
                    return new ObservableCollection<string>(list.OrderBy(_ => Guid.NewGuid()));
                }
                catch (Exception ex)
                {
                    _ = ShowErrorMessage("Answers Error", $"Failed to get FirstAnswersList.\nDetails: {ex.Message}");
                    return new ObservableCollection<string>();
                }
            }
            set => SetValue(FirstAnswersListProperty, value);
        }

        public ObservableCollection<string> SecondAnswersList
        {
            get => (ObservableCollection<string>)GetValue(SecondAnswersListProperty);
            set => SetValue(SecondAnswersListProperty, value);
        }

        private void HandleOptionClick(ref Button selectedButton, Button clickedButton)
        {
            try
            {
                if (selectedButton == clickedButton)
                {
                    selectedButton.Background = TransparentBrush;
                    selectedButton = null;
                }
                else if (selectedButton != clickedButton && selectedButton != null)
                {
                    selectedButton.Background = TransparentBrush;
                    selectedButton = clickedButton;
                    selectedButton.Background = accentBrush;
                    selectedButton.BorderBrush = accentBrush;
                }
                else
                {
                    selectedButton = clickedButton;
                    selectedButton.Background = accentBrush;
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Option Click Error", $"Failed to handle option click.\nDetails: {ex.Message}");
            }
        }

        private void DestroyExistingConnections(Button clickedButton)
        {
            try
            {
                foreach (var mapping in pairs.ToList())
                {
                    Button leftButtonContent = mapping.Item1;
                    Button rightButtonContent = mapping.Item2;
                    Line line = mapping.Item3;

                    if (leftButtonContent == clickedButton || rightButtonContent == clickedButton)
                    {
                        pairs.Remove(mapping);
                        leftButtonContent.Background = TransparentBrush;
                        rightButtonContent.Background = TransparentBrush;
                        clickedButton.Background = accentBrush;
                        LinesCanvas.Children.Remove(line);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Connection Error", $"Failed to destroy existing connections.\nDetails: {ex.Message}");
            }
        }

        private void CheckConnection()
        {
            try
            {
                if (selectedLeftButton == null || selectedRightButton == null)
                {
                    return;
                }

                var line = new Line
                {
                    Stroke = accentBrush,
                    StrokeThickness = 2,
                    X1 = GetCirclePosition(selectedLeftButton, true).X,
                    Y1 = GetCirclePosition(selectedLeftButton, true).Y,
                    X2 = GetCirclePosition(selectedRightButton, false).X,
                    Y2 = GetCirclePosition(selectedRightButton, false).Y
                };

                LinesCanvas.Children.Add(line);
                pairs.Add(new Tuple<Button, Button, Line>(selectedLeftButton, selectedRightButton, line));

                selectedLeftButton.Background = accentBrush;
                selectedRightButton.Background = accentBrush;
                selectedLeftButton = null;
                selectedRightButton = null;
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Connection Error", $"Failed to check connection.\nDetails: {ex.Message}");
            }
        }

        private Point GetCirclePosition(Button button, bool isLeftCircle)
        {
            try
            {
                var transform = button.TransformToVisual(LinesCanvas);
                var buttonPosition = transform.TransformPoint(new Point(0, 0));

                var buttonCenterY = buttonPosition.Y + (button.ActualHeight / 2);

                var stackPanel = button.Parent as StackPanel;
                if (stackPanel != null)
                {
                    var circle = stackPanel.Children.OfType<Ellipse>().FirstOrDefault();
                    if (circle != null)
                    {
                        var circleTransform = circle.TransformToVisual(LinesCanvas);
                        var circlePosition = circleTransform.TransformPoint(new Point(0, 0));

                        return new Point(
                            circlePosition.X + (circle.ActualWidth / 2),
                            circlePosition.Y + (circle.ActualHeight / 2));
                    }
                }

                var circleX = isLeftCircle
                    ? buttonPosition.X + button.ActualWidth + 12
                    : buttonPosition.X - 12;

                return new Point(circleX, buttonCenterY);
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Position Error", $"Failed to get circle position.\nDetails: {ex.Message}");
                return new Point(0, 0); // Fallback position
            }
        }

        private void LeftOption_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clickedButton = sender as Button;
                if (clickedButton == null)
                {
                    _ = ShowErrorMessage("Click Error", "Invalid button clicked.");
                    return;
                }
                HandleOptionClick(ref selectedLeftButton, clickedButton);
                DestroyExistingConnections(clickedButton);
                CheckConnection();
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Left Option Click Error", $"Failed to handle left option click.\nDetails: {ex.Message}");
            }
        }

        private void RightOption_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clickedButton = sender as Button;
                if (clickedButton == null)
                {
                    _ = ShowErrorMessage("Click Error", "Invalid button clicked.");
                    return;
                }
                HandleOptionClick(ref selectedRightButton, clickedButton);
                DestroyExistingConnections(clickedButton);
                CheckConnection();
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Right Option Click Error", $"Failed to handle right option click.\nDetails: {ex.Message}");
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<(string, string)> contentPairs = pairs
                    .Select(mapping => (
                        mapping.Item1.Content.ToString(),
                        mapping.Item2.Content.ToString()))
                    .ToList();

                OnSendClicked?.Invoke(this, new AssociationExerciseEventArgs(contentPairs));
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Send Click Error", $"Failed to process send action.\nDetails: {ex.Message}");
            }
        }

        public class AssociationExerciseEventArgs : EventArgs
        {
            public List<(string, string)> ContentPairs { get; }

            public AssociationExerciseEventArgs(List<(string, string)> contentPairs)
            {
                ContentPairs = contentPairs;
            }
        }
    }
}