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
using Duo.Views.Components;

namespace Duo.Views.Pages
{
    public sealed partial class ExamPage : Page
    {
        private static readonly SolidColorBrush CorrectBrush = new SolidColorBrush(Microsoft.UI.Colors.Green);
        private static readonly SolidColorBrush IncorrectBrush = new SolidColorBrush(Microsoft.UI.Colors.Red);

        public ExamPage()
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
                _ = ShowErrorMessage("Initialization Error", $"Failed to initialize ExamPage.\nDetails: {ex.Message}");
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
                    else if (currentExercise is FillInTheBlankExercise fillInTheBlanksExercise)
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
                    ExerciseContentControl.Content = null;
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Load Exercise Error", $"Failed to load current exercise.\nDetails: {ex.Message}");
            }
        }

        private void AssociationControl_OnSendClicked(object sender, AssociationExerciseEventArgs e)
        {
            try
            {
                var contentPairs = e.ContentPairs;
                var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);
                var loadedNext = ViewModel.LoadNext();

                if (loadedNext)
                {
                    LoadCurrentExercise();
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
                var contentPairs = e.ContentPairs;
                var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);
                var loadedNext = ViewModel.LoadNext();

                if (loadedNext)
                {
                    LoadCurrentExercise();
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
                var contentPairs = e.ContentPairs;
                var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);
                var loadedNext = ViewModel.LoadNext();

                if (loadedNext)
                {
                    LoadCurrentExercise();
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
                var contentPairs = e.ContentPairs;
                var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);
                var loadedNext = ViewModel.LoadNext();

                if (loadedNext)
                {
                    LoadCurrentExercise();
                }
            }
            catch (Exception ex)
            {
                _ = ShowErrorMessage("Flashcard Exercise Error", $"Failed to process flashcard exercise response.\nDetails: {ex.Message}");
            }
        }
    }
}