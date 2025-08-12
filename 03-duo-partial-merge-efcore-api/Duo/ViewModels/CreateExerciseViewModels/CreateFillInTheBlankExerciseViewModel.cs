using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.ViewModels.Base;
using Duo.ViewModels.ExerciseViewModels;

namespace Duo.ViewModels.CreateExerciseViewModels
{
    partial class CreateFillInTheBlankExerciseViewModel : CreateExerciseViewModelBase
    {
        private readonly ExerciseCreationViewModel parentViewModel;
        public ObservableCollection<Answer> Answers { get; set; } = new ObservableCollection<Answer>();
        public const int MAX_ANSWERS = 3;

        public ICommand AddNewAnswerCommand { get; }

        public CreateFillInTheBlankExerciseViewModel(ExerciseCreationViewModel parentViewModel)
        {
            try
            {
                this.parentViewModel = parentViewModel ?? throw new ArgumentNullException(nameof(parentViewModel));
                Answers.Add(new Answer(string.Empty));
                AddNewAnswerCommand = new RelayCommand(_ => AddNewAnswer());
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize CreateFillInTheBlankExerciseViewModel.\nDetails: {ex.Message}");
            }
        }

        public override Exercise CreateExercise(string question, Difficulty difficulty)
        {
            try
            {
                Exercise newExercise = new Models.Exercises.FillInTheBlankExercise(0, question, difficulty, GenerateAnswerList(Answers));
                return newExercise;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Create Exercise Error", $"Failed to create fill-in-the-blank exercise.\nDetails: {ex.Message}");
                return null; // Fallback, though ideally handled by caller
            }
        }

        public List<string> GenerateAnswerList(ObservableCollection<Answer> answers)
        {
            try
            {
                List<string> answerList = new List<string>();
                foreach (Answer answer in answers)
                {
                    answerList.Add(answer.Value);
                }
                return answerList;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Generate Answers Error", $"Failed to generate answer list.\nDetails: {ex.Message}");
                return new List<string>();
            }
        }

        public void AddNewAnswer()
        {
            try
            {
                if (Answers.Count >= MAX_ANSWERS)
                {
                    parentViewModel.RaiseErrorMessage("Maximum Answers Reached", $"You can only have {MAX_ANSWERS} answers for a fill-in-the-blank exercise.");
                    return;
                }
                Answers.Add(new Answer(string.Empty));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Add Answer Error", $"Failed to add new answer.\nDetails: {ex.Message}");
            }
        }

        public class Answer : ViewModelBase
        {
            private string value;

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

            public Answer(string value)
            {
                try
                {
                    this.value = value;
                }
                catch (Exception ex)
                {
                    RaiseErrorMessage("Answer Initialization Error", $"Failed to initialize Answer.\nDetails: {ex.Message}");
                }
            }
        }
    }
}