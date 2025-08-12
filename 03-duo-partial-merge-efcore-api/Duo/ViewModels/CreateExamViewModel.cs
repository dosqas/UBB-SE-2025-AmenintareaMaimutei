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

namespace Duo.ViewModels
{
    internal class CreateExamViewModel : AdminBaseViewModel
    {
        private readonly IQuizService quizService;
        private readonly IExerciseService exerciseService;
        private readonly List<Exercise> availableExercises;
        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> SelectedExercises { get; private set; } = new ObservableCollection<Exercise>();

        public event Action<List<Exercise>> ShowListViewModal;

        private const int NO_ORDER_NUMBER = -1;
        private const int NO_SECTION_ID = -1;
        private const int MAX_EXERCISES = 25;

        public ICommand RemoveExerciseCommand { get; }
        public ICommand SaveButtonCommand { get; }
        public ICommand OpenSelectExercisesCommand { get; }

        public CreateExamViewModel()
        {
            try
            {
                quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService));
                exerciseService = (IExerciseService)App.ServiceProvider.GetService(typeof(IExerciseService));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", ex.Message);
            }
            _ = Task.Run(async () => await LoadExercisesAsync());
            SaveButtonCommand = new RelayCommand((_) => _ = CreateExam());
            // Update the RelayCommand initialization for OpenSelectExercisesCommand to match the expected signature.
            // OpenSelectExercisesCommand = new RelayCommand(async (_) => await Task.Run(OpenSelectExercises));
            OpenSelectExercisesCommand = new RelayCommand(_ => OpenSelectExercises());
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
                RaiseErrorMessage("Load Exercises Error", ex.Message);
            }
        }

        public void OpenSelectExercises()
        {
            try
            {
                ShowListViewModal?.Invoke(GetAvailableExercises());
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Open Exercise Selector Error", ex.Message);
            }
        }

        public List<Exercise> GetAvailableExercises()
        {
            try
            {
                List<Exercise> availableExercises = new List<Exercise>();
                foreach (var exercise in Exercises)
                {
                    if (!SelectedExercises.Contains(exercise))
                    {
                        availableExercises.Add(exercise);
                    }
                }
                return availableExercises;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Get Available Exercises Error", ex.Message);
                return new List<Exercise>();
            }
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
                    RaiseErrorMessage("Add Exercise Error", $"Maximum number of exercises ({MAX_EXERCISES}) reached.");
                }
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Add Exercise Error", ex.Message);
            }
        }

        public void RemoveExercise(Exercise exerciseToBeRemoved)
        {
            try
            {
                SelectedExercises.Remove(exerciseToBeRemoved);
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Remove Exercise Error", ex.Message);
            }
        }

        public async Task CreateExam()
        {
            try
            {
                Exam newExam = new Exam(0, null);

                foreach (var exercise in SelectedExercises)
                {
                    newExam.AddExercise(exercise);
                }

                int examId = await quizService.CreateExam(newExam);
                GoBack();
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Create Exam Error", ex.Message);
            }
        }
    }
}
