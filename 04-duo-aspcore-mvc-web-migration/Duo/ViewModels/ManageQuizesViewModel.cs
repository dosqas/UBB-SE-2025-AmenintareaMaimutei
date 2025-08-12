using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using Duo.Commands;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Services;

namespace Duo.ViewModels
{
    internal class ManageQuizesViewModel : AdminBaseViewModel
    {
        private readonly IExerciseService exerciseService;
        private readonly IQuizService quizService;

        public ObservableCollection<Quiz> Quizes { get; set; } = new ObservableCollection<Quiz>();
        public ObservableCollection<Exercise> QuizExercises { get; private set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> AvailableExercises { get; private set; } = new ObservableCollection<Exercise>();

        public event Action<List<Exercise>> ShowListViewModal;

        private Quiz selectedQuiz;

        public ManageQuizesViewModel()
        {
            try
            {
                exerciseService = (IExerciseService)App.ServiceProvider.GetService(typeof(IExerciseService));
                quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Initialization error", ex.Message);
            }

            DeleteQuizCommand = new RelayCommandWithParameter<Quiz>(quiz => _ = DeleteQuiz(quiz));

            OpenSelectExercisesCommand = new RelayCommand((_) =>
            {
                try
                {
                    OpenSelectExercises();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"OpenSelectExercises error: {ex.Message}");
                    RaiseErrorMessage("Failed to open exercise selection.", ex.Message);
                }
                return Task.CompletedTask;
            });

            RemoveExerciseFromQuizCommand = new RelayCommandWithParameter<Exercise>(exercise => _ = RemoveExerciseFromQuiz(exercise));
            InitView();
        }

        public async void InitView()
        {
            await LoadExercisesAsync();
            await InitializeViewModel();
        }

        public Quiz SelectedQuiz
        {
            get => selectedQuiz;
            set
            {
                selectedQuiz = value;
                _ = UpdateQuizExercises(SelectedQuiz);
                OnPropertyChanged();
            }
        }

        public ICommand DeleteQuizCommand { get; }
        public ICommand OpenSelectExercisesCommand { get; }
        public ICommand RemoveExerciseFromQuizCommand { get; }

        public async Task DeleteQuiz(Quiz quizToBeDeleted)
        {
            try
            {
                Debug.WriteLine("Deleting quiz...");

                if (quizToBeDeleted == SelectedQuiz)
                {
                    SelectedQuiz = null;
                    await UpdateQuizExercises(SelectedQuiz);
                }

                foreach (var exercise in quizToBeDeleted.Exercises)
                {
                    AvailableExercises.Add(exercise);
                }

                await quizService.DeleteQuiz(quizToBeDeleted.Id);
                Quizes.Remove(quizToBeDeleted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during DeleteQuiz: {ex.Message}");
                RaiseErrorMessage("Failed to delete quiz", ex.Message);
            }
        }

        public async Task InitializeViewModel()
        {
            try
            {
                List<Quiz> quizes = await quizService.GetAllQuizzes();
                Quizes.Clear();

                foreach (var quiz in quizes)
                {
                    Quizes.Add(quiz);
                    Debug.WriteLine(quiz);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to load quizzes", ex.Message);
            }
        }

        public async Task UpdateQuizExercises(Quiz selectedQuiz)
        {
            try
            {
                Debug.WriteLine("Updating quiz exercises...");
                QuizExercises.Clear();

                if (selectedQuiz == null)
                {
                    Debug.WriteLine("No quiz selected. Skipping update.");
                    return;
                }

                List<Exercise> exercisesOfSelectedQuiz = await exerciseService.GetAllExercisesFromQuiz(selectedQuiz.Id);

                foreach (var exercise in exercisesOfSelectedQuiz)
                {
                    QuizExercises.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during UpdateQuizExercises: {ex.Message}");
                RaiseErrorMessage("Failed to load quiz exercises", ex.Message);
            }
        }

        public void OpenSelectExercises()
        {
            try
            {
                Debug.WriteLine("Opening select exercises...");
                ShowListViewModal?.Invoke(AvailableExercises.ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while opening select exercises: {ex.Message}");
                RaiseErrorMessage("Failed to open select exercises", ex.Message);
            }
        }

        private async Task LoadExercisesAsync()
        {
            try
            {
                AvailableExercises.Clear();

                var exercises = await exerciseService.GetAllExercises();

                foreach (var exercise in exercises)
                {
                    AvailableExercises.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during LoadExercisesAsync: {ex.Message}");
                RaiseErrorMessage("Failed to load available exercises", ex.Message);
            }
        }

        public async Task AddExercise(Exercise selectedExercise)
        {
            try
            {
                Debug.WriteLine("Adding exercise...");

                if (SelectedQuiz == null)
                {
                    RaiseErrorMessage("No quiz selected", "Please select a quiz before adding exercises.");
                    return;
                }

                SelectedQuiz.AddExercise(selectedExercise);

                await quizService.AddExerciseToQuiz(SelectedQuiz.Id, selectedExercise.ExerciseId);
                await UpdateQuizExercises(SelectedQuiz);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while adding exercise: {ex.Message}");
                RaiseErrorMessage("Failed to add exercise to quiz", ex.Message);
            }
        }

        public async Task RemoveExerciseFromQuiz(Exercise selectedExercise)
        {
            try
            {
                await quizService.RemoveExerciseFromQuiz(SelectedQuiz.Id, selectedExercise.ExerciseId);
                SelectedQuiz.RemoveExercise(selectedExercise);
                await UpdateQuizExercises(SelectedQuiz);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while removing exercise: {ex.Message}");
                RaiseErrorMessage("Failed to remove exercise from quiz", ex.Message);
            }
        }
    }
}