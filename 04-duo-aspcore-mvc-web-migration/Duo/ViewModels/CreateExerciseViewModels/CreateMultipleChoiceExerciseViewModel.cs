using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Duo.Commands;
using DuoClassLibrary.Models;
using DuoClassLibrary.Models.Exercises;
using Duo.ViewModels.Base;
using Duo.ViewModels.ExerciseViewModels;

namespace Duo.ViewModels.CreateExerciseViewModels
{
    partial class CreateMultipleChoiceExerciseViewModel : CreateExerciseViewModelBase
    {
        private readonly ExerciseCreationViewModel parentViewModel;
        public const int MINIMUM_ANSWERS = 2;
        public const int MAXIMUM_ANSWERS = 5;

        private string selectedAnswer = string.Empty;

        public ObservableCollection<Answer> Answers { get; set; } = new ObservableCollection<Answer>();

        public CreateMultipleChoiceExerciseViewModel(ExerciseCreationViewModel parentViewModel)
        {
            try
            {
                this.parentViewModel = parentViewModel ?? throw new ArgumentNullException(nameof(parentViewModel));
                AddNewAnswerCommand = new RelayCommand(_ => AddNewAnswer());
                UpdateSelectedAnswerComand = new RelayCommandWithParameter<string>(UpdateSelectedAnswer);
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize CreateMultipleChoiceExerciseViewModel.\nDetails: {ex.Message}");
            }
        }

        public override Exercise CreateExercise(string questionText, Difficulty difficulty)
        {
            try
            {
                List<MultipleChoiceAnswerModel> multipleChoiceAnswerModelList = GenerateAnswerModelList();
                Exercise newExercise = new MultipleChoiceExercise(0, questionText, difficulty, multipleChoiceAnswerModelList);
                return newExercise;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Create Exercise Error", $"Failed to create multiple choice exercise.\nDetails: {ex.Message}");
                return null; // Fallback, though ideally handled by caller
            }
        }

        public List<MultipleChoiceAnswerModel> GenerateAnswerModelList()
        {
            try
            {
                List<Answer> finalAnswers = Answers.ToList();
                List<MultipleChoiceAnswerModel> multipleChoiceAnswerModels = new List<MultipleChoiceAnswerModel>();
                foreach (Answer answer in finalAnswers)
                {
                    multipleChoiceAnswerModels.Add(new MultipleChoiceAnswerModel
                    {
                        AnswerModelId = 0,
                        Answer = answer.Value,
                        IsCorrect = answer.IsCorrect
                    });
                }
                return multipleChoiceAnswerModels;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Generate Answers Error", $"Failed to generate answer model list.\nDetails: {ex.Message}");
                return new List<MultipleChoiceAnswerModel>();
            }
        }

        public string SelectedAnswer
        {
            get => selectedAnswer;
            set
            {
                try
                {
                    selectedAnswer = value;
                    OnPropertyChanged(nameof(SelectedAnswer));
                }
                catch (Exception ex)
                {
                    RaiseErrorMessage("Selected Answer Error", $"Failed to set selected answer.\nDetails: {ex.Message}");
                }
            }
        }

        public ICommand AddNewAnswerCommand { get; }
        public ICommand UpdateSelectedAnswerComand { get; }

        private void AddNewAnswer()
        {
            try
            {
                if (Answers.Count >= MAXIMUM_ANSWERS)
                {
                    parentViewModel.RaiseErrorMessage("Maximum Answers Reached", $"Maximum number of answers ({MAXIMUM_ANSWERS}) reached.");
                    return;
                }
                Answers.Add(new Answer(string.Empty, false));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Add Answer Error", $"Failed to add new answer.\nDetails: {ex.Message}");
            }
        }

        private void UpdateSelectedAnswer(string selectedValue)
        {
            try
            {
                foreach (var answer in Answers)
                {
                    answer.IsCorrect = answer.Value == selectedValue;
                }

                SelectedAnswer = selectedValue; // Update the selected answer reference
                OnPropertyChanged(nameof(Answers)); // Notify UI
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Update Selected Answer Error", $"Failed to update selected answer.\nDetails: {ex.Message}");
            }
        }

        public class Answer : ViewModelBase
        {
            private string value;
            private bool isCorrect;

            public string Value
            {
                get => value;
                set
                {
                    try
                    {
                        this.value = value;
                        OnPropertyChanged(nameof(Value));
                    }
                    catch (Exception ex)
                    {
                        RaiseErrorMessage("Answer Value Error", $"Failed to set answer value.\nDetails: {ex.Message}");
                    }
                }
            }

            public bool IsCorrect
            {
                get => isCorrect;
                set
                {
                    try
                    {
                        if (isCorrect != value)
                        {
                            isCorrect = value;
                            OnPropertyChanged(nameof(IsCorrect));
                        }
                    }
                    catch (Exception ex)
                    {
                        RaiseErrorMessage("Answer IsCorrect Error", $"Failed to set IsCorrect value.\nDetails: {ex.Message}");
                    }
                }
            }

            public Answer(string value, bool isCorrect)
            {
                try
                {
                    this.value = value;
                    this.isCorrect = isCorrect;
                }
                catch (Exception ex)
                {
                    RaiseErrorMessage("Answer Initialization Error", $"Failed to initialize Answer.\nDetails: {ex.Message}");
                }
            }
        }
    }
}