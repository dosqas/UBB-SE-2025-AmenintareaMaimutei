using System;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.ViewModels.Base;
using Duo.ViewModels.ExerciseViewModels;

namespace Duo.ViewModels.CreateExerciseViewModels
{
    partial class CreateFlashcardExerciseViewModel : CreateExerciseViewModelBase
    {
        private string answer = string.Empty;

        public CreateFlashcardExerciseViewModel()
        {
            try
            {
                // No additional initialization required
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize CreateFlashcardExerciseViewModel.\nDetails: {ex.Message}");
            }
        }

        public string Answer
        {
            get => answer;
            set
            {
                try
                {
                    if (answer != value)
                    {
                        answer = value;
                        OnPropertyChanged(nameof(Answer));
                    }
                }
                catch (Exception ex)
                {
                    RaiseErrorMessage("Answer Error", $"Failed to set answer.\nDetails: {ex.Message}");
                }
            }
        }

        public override Exercise CreateExercise(string question, Difficulty difficulty)
        {
            try
            {
                return new FlashcardExercise(0, question, Answer, difficulty);
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Create Exercise Error", $"Failed to create flashcard exercise.\nDetails: {ex.Message}");
                return null; // Fallback, though ideally handled by caller
            }
        }
    }
}