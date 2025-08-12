using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using DuoClassLibrary.Models.Exercises;
using Duo.ViewModels;
using Duo.ViewModels.Base;
using static Duo.Views.Components.AssociationExercise;
using static Duo.Views.Components.MultipleChoiceExercise;
using static Duo.Views.Components.FillInTheBlanksExercise;
using DuoClassLibrary.Models;
using Duo.Views.Components;

namespace Duo.Views.Pages
{
    public sealed partial class QuizPage : Page
    {
        private static readonly SolidColorBrush CorrectBrush = new SolidColorBrush(Microsoft.UI.Colors.Green);
        private static readonly SolidColorBrush IncorrectBrush = new SolidColorBrush(Microsoft.UI.Colors.Red);

        public QuizPage()
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
                _ = ShowErrorMessage("Initialization Error", $"Failed to initialize QuizPage.\nDetails: {ex.Message}");
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);

                if (e.Parameter is ValueTuple<int, bool> parameters)
                {
                    int quizId = parameters.Item1;
                    bool isExam = parameters.Item2;

                    if (isExam)
                    {
                        await ViewModel.SetExamIdAsync(quizId);
                    }
                    else
                    {
                        await ViewModel.SetQuizIdAsync(quizId);
                    }
                    LoadCurrentExercise();
                }
                else
                {
                    await ShowErrorMessage("Navigation Error", "Invalid navigation parameters.");
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage("Navigation Error", $"Failed to navigate to QuizPage.\nDetails: {ex.Message}");
            }
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
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
                _ = ShowErrorMessage("Back Navigation Error", $"Failed to navigate back.\nDetails: {ex.Message}");
            }
        }

        public void CancelButton_Click(object sender, RoutedEventArgs e)
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
                _ = ShowErrorMessage("Cancel Navigation Error", $"Failed to cancel and navigate back.\nDetails: {ex.Message}");
            }
        }

        private void LoadCurrentExercise()
        {
            try
            {
                if (ViewModel == null || ViewModel.Exercises == null)
                {
                    _ = ShowErrorMessage("Load Exercise Error", "ViewModel or Exercises collection is not initialized.");
                    return;
                }

                var currentExercise = ViewModel.CurrentExercise;

                if (currentExercise != null)
                {
                    var mainStackPanel = ExerciseContentControl.Parent as StackPanel;
                    if (mainStackPanel != null)
                    {
                        var existingIndicators = mainStackPanel.Children.OfType<Grid>()
                            .Where(g => g.Children.OfType<FontIcon>().Any() && g.Children.OfType<TextBlock>().Any())
                            .ToList();
                        foreach (var indicator in existingIndicators)
                        {
                            mainStackPanel.Children.Remove(indicator);
                        }

                        var existingProgressGrids = mainStackPanel.Children.OfType<Grid>()
                            .Where(g => g.Children.OfType<ProgressBar>().Any())
                            .ToList();
                        foreach (var grid in existingProgressGrids)
                        {
                            mainStackPanel.Children.Remove(grid);
                        }
                    }

                    var progressGrid = new Grid
                    {
                        Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Margin = new Thickness(0, 0, 0, 16),
                        Height = 16
                    };

                    var progressBar = new ProgressBar
                    {
                        Value = (double)ViewModel.CurrentExerciseIndex / ViewModel.Exercises.Count * 100,
                        Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
                        Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green)
                    };

                    progressGrid.Children.Add(progressBar);

                    if (mainStackPanel != null)
                    {
                        mainStackPanel.Children.Insert(1, progressGrid);
                    }

                    var difficultyGrid = new Grid
                    {
                        Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Margin = new Thickness(0, 0, 0, 16),
                        Padding = new Thickness(12, 6, 12, 6)
                    };

                    difficultyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    difficultyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    var icon = new FontIcon
                    {
                        FontSize = 16,
                        Margin = new Thickness(0, 0, 8, 0)
                    };

                    var text = new TextBlock
                    {
                        FontSize = 14,
                        Style = (Style)Application.Current.Resources["BodyStrongTextBlockStyle"]
                    };

                    switch (currentExercise.Difficulty)
                    {
                        case Difficulty.Easy:
                            icon.Glyph = "\uE73E";
                            text.Text = "Easy";
                            text.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                            break;
                        case Difficulty.Normal:
                            icon.Glyph = "\uE7C3";
                            text.Text = "Normal";
                            text.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Orange);
                            break;
                        case Difficulty.Hard:
                            icon.Glyph = "\uE783";
                            text.Text = "Hard";
                            text.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                            break;
                        default:
                            throw new InvalidOperationException("Unknown difficulty level.");
                    }

                    Grid.SetColumn(icon, 0);
                    Grid.SetColumn(text, 1);

                    difficultyGrid.Children.Add(icon);
                    difficultyGrid.Children.Add(text);

                    if (mainStackPanel != null)
                    {
                        mainStackPanel.Children.Insert(2, difficultyGrid);
                    }

                    if (currentExercise is DuoClassLibrary.Models.Exercises.AssociationExercise associationExercise)
                    {
                        var associationControl = new Components.AssociationExercise()
                        {
                            Question = associationExercise.Question,
                            FirstAnswersList = new ObservableCollection<string>(associationExercise.FirstAnswersList),
                            SecondAnswersList = new ObservableCollection<string>(associationExercise.SecondAnswersList)
                        };
                        associationControl.OnSendClicked += AssociationControl_OnSendClicked;

                        ExerciseContentControl.Content = associationControl;
                    }
                    else if (currentExercise is DuoClassLibrary.Models.Exercises.FillInTheBlankExercise fillInTheBlanksExercise)
                    {
                        var fillInTheBlanksControl = new Components.FillInTheBlanksExercise()
                        {
                            Question = fillInTheBlanksExercise.Question
                        };
                        fillInTheBlanksControl.OnSendClicked += FillInTheBlanksControl_OnSendClicked;

                        ExerciseContentControl.Content = fillInTheBlanksControl;
                    }
                    else if (currentExercise is DuoClassLibrary.Models.Exercises.MultipleChoiceExercise multipleChoiceExercise)
                    {
                        var multipleChoiceControl = new Components.MultipleChoiceExercise()
                        {
                            Question = multipleChoiceExercise.Question,
                            Answers = new ObservableCollection<MultipleChoiceAnswerModel>(multipleChoiceExercise.Choices)
                        };
                        multipleChoiceControl.OnSendClicked += MultipleChoiceControl_OnSendClicked;

                        ExerciseContentControl.Content = multipleChoiceControl;
                    }
                    else if (currentExercise is DuoClassLibrary.Models.Exercises.FlashcardExercise flashcardExercise)
                    {
                        var flashcardControl = new Components.FlashcardExercise()
                        {
                            Question = flashcardExercise.Question,
                            Answer = flashcardExercise.Answer
                        };
                        flashcardControl.OnSendClicked += FlashcardControl_OnSendClicked;

                        ExerciseContentControl.Content = flashcardControl;
                    }
                }
                else
                {
                    var mainStackPanel = ExerciseContentControl.Parent as StackPanel;
                    if (mainStackPanel != null)
                    {
                        var existingProgressGrids = mainStackPanel.Children.OfType<Grid>()
                            .Where(g => g.Children.OfType<ProgressBar>().Any())
                            .ToList();
                        foreach (var grid in existingProgressGrids)
                        {
                            mainStackPanel.Children.Remove(grid);
                        }
                    }

                    NextExerciseButton.Visibility = Visibility.Collapsed;

                    ViewModel.MarkUserProgression();

                    var endScreen = new Components.QuizEndScreen()
                    {
                        CorrectAnswersText = ViewModel.CorrectAnswersText,
                        PassingPercentText = ViewModel.PassingPercentText,
                        IsPassedText = ViewModel.IsPassedText
                    };

                    ExerciseContentControl.Content = endScreen;
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Load Exercise Error", $"Failed to load current exercise.\nDetails: {ex.Message}");
            }
        }

        private async void ShowMessage(FrameworkElement parentElement, bool valid)
        {
            try
            {
                if (ViewModel.QuizId == -1)
                {
                    var loadedNext = ViewModel.LoadNext();
                    if (loadedNext)
                    {
                        LoadCurrentExercise();
                    }
                    return;
                }

                var feedbackPopup = new AnswerFeedbackPopup
                {
                    XamlRoot = parentElement.XamlRoot
                };

                if (valid)
                {
                    feedbackPopup.ShowCorrectAnswer(ViewModel.GetCurrentExerciseCorrectAnswer());
                }
                else
                {
                    feedbackPopup.ShowWrongAnswer(ViewModel.GetCurrentExerciseCorrectAnswer());
                }

                await feedbackPopup.ShowAsync();
            }
            catch (Exception ex)
            {
                await ShowErrorMessage("Show Feedback Error", $"Failed to show feedback popup.\nDetails: {ex.Message}");
            }
        }

        private void AssociationControl_OnSendClicked(object sender, AssociationExerciseEventArgs e)
        {
            try
            {
                if (ViewModel.ValidatedCurrent == null)
                {
                    var contentPairs = e.ContentPairs;
                    var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);
                    ShowMessage((FrameworkElement)sender, valid);
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Association Exercise Error", $"Failed to process association exercise response.\nDetails: {ex.Message}");
            }
        }

        private void MultipleChoiceControl_OnSendClicked(object sender, MultipleChoiceExerciseEventArgs e)
        {
            try
            {
                if (ViewModel.ValidatedCurrent == null)
                {
                    var contentPairs = e.ContentPairs;
                    var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);
                    ShowMessage((FrameworkElement)sender, valid);
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Multiple Choice Exercise Error", $"Failed to process multiple choice exercise response.\nDetails: {ex.Message}");
            }
        }

        private void FillInTheBlanksControl_OnSendClicked(object sender, FillInTheBlanksExerciseEventArgs e)
        {
            try
            {
                if (ViewModel.ValidatedCurrent == null)
                {
                    var contentPairs = e.ContentPairs;
                    var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);
                    ShowMessage((FrameworkElement)sender, valid);
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Fill In The Blanks Exercise Error", $"Failed to process fill-in-the-blanks exercise response.\nDetails: {ex.Message}");
            }
        }

        private void FlashcardControl_OnSendClicked(object sender, FlashcardExerciseEventArgs e)
        {
            try
            {
                if (ViewModel.ValidatedCurrent == null)
                {
                    var contentPairs = e.ContentPairs;
                    var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);
                    ShowMessage((FrameworkElement)sender, valid);
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Flashcard Exercise Error", $"Failed to process flashcard exercise response.\nDetails: {ex.Message}");
            }
        }

        private void NextQuiz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var loadedNext = ViewModel.LoadNext();
                if (loadedNext)
                {
                    LoadCurrentExercise();
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Next Quiz Error", $"Failed to load next quiz.\nDetails: {ex.Message}");
            }
        }
    }
}