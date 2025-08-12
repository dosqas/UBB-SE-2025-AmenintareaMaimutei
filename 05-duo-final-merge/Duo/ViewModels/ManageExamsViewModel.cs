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
    internal class ManageExamsViewModel : AdminBaseViewModel
    {
        private readonly IExerciseService exerciseService;
        private readonly IQuizService quizService;

        public ObservableCollection<Exam> Exams { get; set; } = new ObservableCollection<Exam>();
        public ObservableCollection<Exercise> ExamExercises { get; private set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> AvailableExercises { get; private set; } = new ObservableCollection<Exercise>();

        public event Action<List<Exercise>> ShowListViewModal;

        private Exam selectedExam;

        public ManageExamsViewModel()
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

            DeleteExamCommand = new RelayCommandWithParameter<Exam>(exam => _ = DeleteExam(exam));

            OpenSelectExercisesCommand = new RelayCommand(_ =>
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

            RemoveExerciseFromQuizCommand = new RelayCommandWithParameter<Exercise>(exercise => _ = RemoveExerciseFromExam(exercise));
            InitView();
        }

        public async void InitView()
        {
            await LoadExercisesAsync();
            await InitializeViewModel();
        }

        public Exam SelectedExam
        {
            get => selectedExam;
            set
            {
                selectedExam = value;
                _ = UpdateExamExercises(SelectedExam);
                OnPropertyChanged();
            }
        }

        public ICommand DeleteExamCommand { get; }
        public ICommand OpenSelectExercisesCommand { get; }
        public ICommand RemoveExerciseFromQuizCommand { get; }

        public async Task DeleteExam(Exam examToBeDeleted)
        {
            Debug.WriteLine("Deleting exam...");

            try
            {
                if (examToBeDeleted == SelectedExam)
                {
                    SelectedExam = null;
                    await UpdateExamExercises(SelectedExam);
                }

                foreach (var exercise in examToBeDeleted.Exercises)
                {
                    AvailableExercises.Add(exercise);
                }

                await quizService.DeleteExam(examToBeDeleted.Id);
                Exams.Remove(examToBeDeleted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to delete exam", ex.Message);
            }
        }

        public async Task InitializeViewModel()
        {
            try
            {
                List<Exam> exams = await quizService.GetAllExams();
                Exams.Clear();

                foreach (var exam in exams)
                {
                    Exams.Add(exam);
                    Debug.WriteLine(exam);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to load exams", ex.Message);
            }
        }

        public async Task UpdateExamExercises(Exam selectedExam)
        {
            try
            {
                Debug.WriteLine("Updating exam exercises...");
                ExamExercises.Clear();

                if (selectedExam == null)
                {
                    Debug.WriteLine("No exam selected. Skipping update.");
                    return;
                }

                List<Exercise> exercises = await exerciseService.GetAllExercisesFromExam(selectedExam.Id);
                foreach (var exercise in exercises)
                {
                    ExamExercises.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during UpdateExamExercises: {ex.Message}");
                RaiseErrorMessage("Failed to load exam exercises", ex.Message);
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

                if (SelectedExam == null)
                {
                    RaiseErrorMessage("No exam selected", "Please select an exam before adding exercises.");
                    return;
                }

                SelectedExam.AddExercise(selectedExercise);

                await quizService.AddExerciseToExam(SelectedExam.Id, selectedExercise.ExerciseId);
                await UpdateExamExercises(SelectedExam);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while adding exercise: {ex.Message}");
                RaiseErrorMessage("Failed to add exercise to exam", ex.Message);
            }
        }

        public async Task RemoveExerciseFromExam(Exercise selectedExercise)
        {
            try
            {
                Debug.WriteLine("Removing exercise...");

                await quizService.RemoveExerciseFromExam(SelectedExam.Id, selectedExercise.ExerciseId);
                SelectedExam.RemoveExercise(selectedExercise);
                await UpdateExamExercises(SelectedExam);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while removing exercise: {ex.Message}");
                RaiseErrorMessage("Failed to remove exercise from exam", ex.Message);
            }
        }
    }
}
