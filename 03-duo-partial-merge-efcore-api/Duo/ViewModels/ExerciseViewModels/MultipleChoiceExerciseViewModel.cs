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
    public class MultipleChoiceExerciseViewModel : ViewModelBase
    {
        private MultipleChoiceExercise? exercise;
        private ObservableCollection<string>? userChoices;
        private readonly IExerciseService exerciseService;

        public ObservableCollection<string>? UserChoices
        {
            get => userChoices;
            set => SetProperty(ref userChoices, value);
        }

        public MultipleChoiceExerciseViewModel(IExerciseService service)
        {
            try
            {
                exerciseService = service ?? throw new ArgumentNullException(nameof(service));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize MultipleChoiceExerciseViewModel.\nDetails: {ex.Message}");
            }
        }

        public async Task GetExercise(int id)
        {
            try
            {
                Exercise exercise = await exerciseService.GetExerciseById(id);
                if (exercise is MultipleChoiceExercise multipleChoiceExercise)
                {
                    this.exercise = multipleChoiceExercise;
                    userChoices = new ObservableCollection<string>();
                }
                else
                {
                    RaiseErrorMessage("Exercise Error", $"Invalid exercise type for ID {id}. Expected MultipleChoiceExercise.");
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
                if (exercise == null || userChoices == null)
                {
                    RaiseErrorMessage("Validation Error", "Exercise or UserChoices is not initialized.");
                    return false;
                }

                return exercise.ValidateAnswer(userChoices.ToList());
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Validation Error", $"Failed to verify answer.\nDetails: {ex.Message}");
                return false;
            }
        }
    }
}