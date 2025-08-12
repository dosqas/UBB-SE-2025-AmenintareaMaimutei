using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using DuoClassLibrary.Models;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Services;
using Duo.ViewModels.Base;
using Microsoft.UI.Dispatching;

namespace Duo.ViewModels
{
    partial class ManageExercisesViewModel : AdminBaseViewModel
    {
        private readonly IExerciseService exerciseService;

        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();

        public ManageExercisesViewModel()
        {
            try
            {
                exerciseService = (IExerciseService)App.ServiceProvider.GetService(typeof(IExerciseService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Service initialization failed", ex.Message);
            }

            DeleteExerciseCommand = new RelayCommandWithParameter<Exercise>(exercise => _ = DeleteExercise(exercise));

            InitializeViewModel();

            // Fire and forget
            _ = LoadExercisesAsync();
        }

        public ICommand DeleteExerciseCommand { get; }

        // Placeholder for potential future initialization logic
        public void InitializeViewModel()
        {
        }

        // Method to load exercises asynchronously
        private async Task LoadExercisesAsync()
        {
            // handle real async logic here
            try
            {
                var exercises = await exerciseService.GetAllExercises();

                DispatcherQueueHandler callback = () =>
                {
                    Exercises.Clear();
                    foreach (var exercise in exercises)
                    {
                        Exercises.Add(exercise);
                    }
                };
                bool res = DispatcherQueue.GetForCurrentThread().TryEnqueue(DispatcherQueuePriority.Normal, callback: callback);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during LoadExercisesAsync: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                RaiseErrorMessage("Failed to load exercises", ex.Message);
            }
        }

        // Method to delete an exercise and refresh the list
        public async Task DeleteExercise(Exercise exercise)
        {
            try
            {
                await exerciseService.DeleteExercise(exercise.ExerciseId);
                await LoadExercisesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting exercise: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                RaiseErrorMessage("Failed to delete exercise", ex.Message);
            }
        }
    }
}
