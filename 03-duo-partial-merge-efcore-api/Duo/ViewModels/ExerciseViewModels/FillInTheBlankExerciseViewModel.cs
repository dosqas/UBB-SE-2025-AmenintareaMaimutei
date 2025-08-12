using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.Base;

namespace Duo.ViewModels.ExerciseViewModels
{
    public class FillInTheBlankExerciseViewModel : ViewModelBase
    {
        private FillInTheBlankExercise? exercise;
        private readonly IExerciseService exerciseService;
        private ObservableCollection<string>? userAnswers;

        public ObservableCollection<string>? UserAnswers
        {
            get => userAnswers;
            set => SetProperty(ref userAnswers, value);
        }

        public FillInTheBlankExerciseViewModel(IExerciseService service)
        {
            try
            {
                exerciseService = service ?? throw new ArgumentNullException(nameof(service));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize FillInTheBlankExerciseViewModel.\nDetails: {ex.Message}");
            }
        }

        public async Task GetExercise(int id)
        {
            try
            {
                Exercise exercise = await exerciseService.GetExerciseById(id);
                if (exercise is FillInTheBlankExercise fillInTheBlankExercise)
                {
                    this.exercise = fillInTheBlankExercise;
                    userAnswers = new ObservableCollection<string>(Enumerable.Repeat(string.Empty, this.exercise.PossibleCorrectAnswers.Count));
                }
                else
                {
                    RaiseErrorMessage("Exercise Error", $"Invalid exercise type for ID {id}. Expected FillInTheBlankExercise.");
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