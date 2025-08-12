using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Services;
using Duo.ViewModels.Base;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Duo.ViewModels
{
    internal class CreateQuizViewModel : AdminBaseViewModel
    {
        private readonly IQuizService quizService;
        private readonly IExerciseService exerciseService;
        private readonly List<Exercise> availableExercises;

        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> SelectedExercises { get; private set; } = new ObservableCollection<Exercise>();

        public event Action<List<Exercise>>? ShowListViewModal;

        private const int MAX_EXERCISES = 10;
        private const int NO_ORDER_NUMBER = -1;
        private const int NO_SECTION_ID = -1;

        public ICommand RemoveExerciseCommand { get; }
        public ICommand SaveButtonCommand { get; }
        public ICommand OpenSelectExercisesCommand { get; }

        public CreateQuizViewModel()
        {
            try
            {
                if (App.ServiceProvider != null)
                {
                    quizService = (IQuizService?)App.ServiceProvider.GetService(typeof(IQuizService));
                    exerciseService = (IExerciseService?)App.ServiceProvider.GetService(typeof(IExerciseService));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Initialization Error", ex.Message);
            }

            _ = Task.Run(async () => await LoadExercisesAsync());

            SaveButtonCommand = new RelayCommand((_) => _ = CreateQuiz());
            OpenSelectExercisesCommand = new RelayCommand(async (_) => await Task.Run(() => OpenSelectExercises()));
            RemoveExerciseCommand = new RelayCommandWithParameter<Exercise>(RemoveExercise);
        }

        private async Task LoadExercisesAsync()
        {
            try
            {
                Exercises.Clear();
                var exercises = await exerciseService.GetAllExercises();
                foreach (var exercise in exercises)
                {
                    Exercises.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading exercises: {ex.Message}");
                RaiseErrorMessage("Load Error", "Failed to load exercises.");
            }
        }

        public void OpenSelectExercises()
        {
            try
            {
                Debug.WriteLine("Opening select exercises...");
                ShowListViewModal?.Invoke(GetAvailableExercises());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening exercise selection: {ex.Message}");
                RaiseErrorMessage("Open Dialog Error", "Could not open the exercise selection dialog.");
            }
        }

        public List<Exercise> GetAvailableExercises()
        {
            var availableExercises = new List<Exercise>();
            try
            {
                foreach (var exercise in Exercises)
                {
                    if (!SelectedExercises.Contains(exercise))
                    {
                        availableExercises.Add(exercise);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error filtering available exercises: {ex.Message}");
                RaiseErrorMessage("Filter Error", "Could not get available exercises.");
            }

            return availableExercises;
        }

        public void AddExercise(Exercise selectedExercise)
        {
            try
            {
                if (SelectedExercises.Count < MAX_EXERCISES)
                {
                    SelectedExercises.Add(selectedExercise);
                }
                else
                {
                    RaiseErrorMessage("Limit Reached", $"Maximum number of exercises ({MAX_EXERCISES}) reached.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding exercise: {ex.Message}");
                RaiseErrorMessage("Add Error", "Could not add the selected exercise.");
            }
        }

        public void RemoveExercise(Exercise exerciseToBeRemoved)
        {
            try
            {
                SelectedExercises.Remove(exerciseToBeRemoved);
                Debug.WriteLine("Removing exercise...");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error removing exercise: {ex.Message}");
                RaiseErrorMessage("Remove Error", "Could not remove the selected exercise.");
            }
        }

        public async Task CreateQuiz()
        {
            try
            {
                Debug.WriteLine("Creating quiz...");
                var newQuiz = new Quiz(0, null, null);

                foreach (var exercise in SelectedExercises)
                {
                    newQuiz.AddExercise(exercise);
                }

                int quizId = await quizService.CreateQuiz(newQuiz);
                // await quizService.AddExercisesToQuiz(quizId, newQuiz.Exercises);
                GoBack();
                Debug.WriteLine(newQuiz);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during CreateQuiz: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                RaiseErrorMessage("Quiz Creation Failed", "Something went wrong while creating the quiz.");
            }
        }
    }
}
