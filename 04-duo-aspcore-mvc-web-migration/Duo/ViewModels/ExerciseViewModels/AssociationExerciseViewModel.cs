using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Services;
using Duo.ViewModels.Base;

namespace Duo.ViewModels.ExerciseViewModels
{
    public class AssociationExerciseViewModel : ViewModelBase
    {
        private readonly IExerciseService exerciseService;
        private AssociationExercise? exercise;
        private ObservableCollection<(string, string)>? userAnswers;

        public ObservableCollection<(string, string)>? UserAnswers
        {
            get => userAnswers;
            set => SetProperty(ref userAnswers, value);
        }

        public AssociationExerciseViewModel(IExerciseService exerciseService)
        {
            try
            {
                this.exerciseService = exerciseService ?? throw new ArgumentNullException(nameof(exerciseService));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize AssociationExerciseViewModel.\nDetails: {ex.Message}");
            }
        }

        public async Task GetExercise(int id)
        {
            try
            {
                Exercise exercise = await exerciseService.GetExerciseById(id);
                if (exercise is AssociationExercise associationExercise)
                {
                    this.exercise = associationExercise;
                    userAnswers = new ObservableCollection<(string, string)>();
                }
                else
                {
                    RaiseErrorMessage("Exercise Error", $"Invalid exercise type for ID {id}. Expected AssociationExercise.");
                }
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Exercise Load Error", $"Failed to load exercise with ID {id}.\nDetails: {ex.Message}");
            }
        }

        public bool VerifyIfAnswerIsCorrect()
        {
            try
            {
                if (exercise == null || userAnswers == null)
                {
                    RaiseErrorMessage("Validation Error", "Exercise or UserAnswers is not initialized.");
                    return false;
                }
                return exercise.ValidateAnswer(userAnswers.ToList());
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Validation Error", $"Failed to verify answer.\nDetails: {ex.Message}");
                return false;
            }
        }
    }
}