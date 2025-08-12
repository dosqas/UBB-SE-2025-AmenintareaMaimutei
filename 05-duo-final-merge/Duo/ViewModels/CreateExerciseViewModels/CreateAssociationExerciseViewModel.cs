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
    internal partial class CreateAssociationExerciseViewModel : CreateExerciseViewModelBase
    {
        private readonly ExerciseCreationViewModel parentViewModel;
        public ObservableCollection<Answer> LeftSideAnswers { get; set; } = new ObservableCollection<Answer>();
        public ObservableCollection<Answer> RightSideAnswers { get; set; } = new ObservableCollection<Answer>();
        public const int MINIMUM_ANSWERS = 2;
        public const int MAXIMUM_ANSWERS = 5;

        public ICommand AddNewAnswerCommand { get; }

        public CreateAssociationExerciseViewModel(ExerciseCreationViewModel parentViewModel)
        {
            try
            {
                this.parentViewModel = parentViewModel ?? throw new ArgumentNullException(nameof(parentViewModel));
                LeftSideAnswers.Add(new Answer(string.Empty));
                RightSideAnswers.Add(new Answer(string.Empty));
                AddNewAnswerCommand = new RelayCommand(_ => AddNewAnswer());
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize CreateAssociationExerciseViewModel.\nDetails: {ex.Message}");
            }
        }

        private void AddNewAnswer()
        {
            try
            {
                if (LeftSideAnswers.Count >= MAXIMUM_ANSWERS || RightSideAnswers.Count >= MAXIMUM_ANSWERS)
                {
                    parentViewModel.RaiseErrorMessage("Maximum Answers Reached", "You can only have up to 5 answers.");
                    return;
                }
                Debug.WriteLine("New answer added");
                LeftSideAnswers.Add(new Answer(string.Empty));
                RightSideAnswers.Add(new Answer(string.Empty));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Add Answer Error", $"Failed to add new answer.\nDetails: {ex.Message}");
            }
        }

        public override Exercise CreateExercise(string question, Difficulty difficulty)
        {
            try
            {
                // Validate: No empty pairings and minimum answers
                var leftAnswers = GenerateAnswerList(LeftSideAnswers);
                var rightAnswers = GenerateAnswerList(RightSideAnswers);

                // Check for minimum answers
                if (leftAnswers.Count < MINIMUM_ANSWERS || rightAnswers.Count < MINIMUM_ANSWERS)
                {
                    parentViewModel.RaiseErrorMessage("Not enough answers", $"You must provide at least {MINIMUM_ANSWERS} answer pairs.");
                    return null;
                }

                // Check for empty values in any pairing
                for (int i = 0; i < leftAnswers.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(leftAnswers[i]) || string.IsNullOrWhiteSpace(rightAnswers[i]))
                    {
                        parentViewModel.RaiseErrorMessage("Empty Pairing", "All answer pairings must be filled in.");
                        return null;
                    }
                }

                Exercise newExercise = new AssociationExercise(0, question, difficulty, leftAnswers, rightAnswers);
                return newExercise;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Create Exercise Error", $"Failed to create association exercise.\nDetails: {ex.Message}");
                return null;
            }
        }

        public List<string> GenerateAnswerList(ObservableCollection<Answer> answers)
        {
            try
            {
                List<Answer> finalAnswers = answers.ToList();
                List<string> answersList = new List<string>();
                foreach (Answer answer in finalAnswers)
                {
                    answersList.Add(answer.Value);
                }
                return answersList;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Generate Answers Error", $"Failed to generate answer list.\nDetails: {ex.Message}");
                return new List<string>();
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