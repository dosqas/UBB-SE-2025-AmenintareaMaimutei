using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Sections;
using DuoClassLibrary.Services;
using Duo.ViewModels.Base;
using Microsoft.IdentityModel.Tokens;

namespace Duo.ViewModels
{
    internal partial class CreateSectionViewModel : AdminBaseViewModel
    {
        private readonly ISectionService sectionService;
        private readonly IQuizService quizService;
        private readonly IExerciseService exerciseService;
        private string subjectText;
        public ObservableCollection<Quiz> Quizes { get; set; } = new ObservableCollection<Quiz>();
        public ObservableCollection<Quiz> SelectedQuizes { get; private set; } = new ObservableCollection<Quiz>();
        public ObservableCollection<Exam> Exams { get; set; } = new ObservableCollection<Exam>();
        public ObservableCollection<Exam> SelectedExams { get; private set; } = new ObservableCollection<Exam>();

        // commands and actions
        public event Action<List<Quiz>> ShowListViewModalQuizes;
        public event Action<List<Exam>> ShowListViewModalExams;
        public ICommand RemoveQuizCommand { get; }
        public ICommand RemoveExamCommand { get; }
        public ICommand SaveButtonCommand { get; }
        public ICommand OpenSelectQuizesCommand { get; }
        public ICommand OpenSelectExamsCommand { get; }

        public CreateSectionViewModel()
        {
            try
            {
                sectionService = (ISectionService)App.ServiceProvider.GetService(typeof(ISectionService));
                quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService));
                exerciseService = (IExerciseService)App.ServiceProvider.GetService(typeof(IExerciseService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Initialization error", ex.Message);
            }
            // OpenSelectQuizesCommand = new RelayCommand(OpenSelectQuizes);
            // Update the RelayCommand initialization to use a lambda expression that matches the expected Func<object?, Task> signature.
            OpenSelectQuizesCommand = new RelayCommand(_ => OpenSelectQuizes());
            OpenSelectExamsCommand = new RelayCommand(_ => OpenSelectExams());
            SaveButtonCommand = new RelayCommand(_ => CreateSection());
            // OpenSelectExamsCommand = new RelayCommand(OpenSelectExams);
            SaveButtonCommand = new RelayCommand((_) => _ = CreateSection());

            RemoveQuizCommand = new RelayCommandWithParameter<Quiz>(RemoveSelectedQuiz);
            RemoveExamCommand = new RelayCommandWithParameter<Exam>(RemoveSelectedExam);

            _ = GetQuizesAsync();
            _ = GetExamsAsync();
        }

        public string SubjectText
        {
            get => subjectText;
            set
            {
                if (subjectText != value)
                {
                    subjectText = value;
                    OnPropertyChanged(nameof(SubjectText));
                }
            }
        }

        public void RemoveSelectedQuiz(Quiz quizToBeRemoved)
        {
            try
            {
                Debug.WriteLine("Removing quiz...");
                SelectedQuizes.Remove(quizToBeRemoved);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to remove quiz", ex.Message);
            }
        }

        public void RemoveSelectedExam(Exam examToBeRemoved)
        {
            try
            {
                Debug.WriteLine("Removing quiz...");
                SelectedExams.Remove(examToBeRemoved);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to remove quiz", ex.Message);
            }
        }

        public void OpenSelectQuizes()
        {
            try
            {
                Debug.WriteLine("Opening select quizes...");
                ShowListViewModalQuizes?.Invoke(GetAvailableQuizes());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to open quiz selection", ex.Message);
            }
        }

        public void OpenSelectExams()
        {
            try
            {
                Debug.WriteLine("Opening select exams...");
                ShowListViewModalExams?.Invoke(GetAvailableExams());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to open exam selection", ex.Message);
            }
        }

        public async Task GetQuizesAsync()
        {
            try
            {
                Quizes.Clear();
                List<Quiz> quizes = await quizService.GetAllAvailableQuizzes();
                foreach (var quiz in quizes)
                {
                    Debug.WriteLine(quiz);
                    Quizes.Add(quiz);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during GetQuizesAsync: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                RaiseErrorMessage("Failed to fetch quizzes", ex.Message);
            }
        }

        public async Task GetExamsAsync()
        {
            try
            {
                Exams.Clear();
                List<Exam> exams = await quizService.GetAllAvailableExams();
                foreach (var exam in exams)
                {
                    Exams.Add(exam);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during GetExamAsync: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                RaiseErrorMessage("Failed to fetch exam", ex.Message);
            }
        }

        public List<Exam> GetAvailableExams()
        {
            try
            {
                return Exams.Where(exam => !SelectedExams.Contains(exam)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to get available exams", ex.Message);
                return new List<Exam>();
            }
        }

        public List<Quiz> GetAvailableQuizes()
        {
            try
            {
                return Quizes.Where(quiz => !SelectedQuizes.Contains(quiz)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to get available quizzes", ex.Message);
                return new List<Quiz>();
            }
        }

        public void AddQuiz(Quiz newQuiz)
        {
            try
            {
                if (newQuiz == null)
                {
                    RaiseErrorMessage("Quiz is null", "Cannot add a null quiz.");
                    return;
                }
                if (!SelectedQuizes.Contains(newQuiz))
                {
                    Debug.WriteLine("Adding quiz..." + newQuiz.Id);
                    SelectedQuizes.Add(newQuiz);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to add quiz", ex.Message);
            }
        }

        public void AddExam(Exam newExam)
        {
            try
            {
                if (newExam == null)
                {
                    RaiseErrorMessage("Exam is null", "Cannot add a null exam.");
                    return;
                }
                if (!SelectedExams.Contains(newExam))
                {
                    Debug.WriteLine("Adding exam..." + newExam.Id);
                    SelectedExams.Clear();
                    SelectedExams.Add(newExam);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to add exam", ex.Message);
            }
        }

        public async Task CreateSection()
        {
            try
            {
                if (SubjectText.IsNullOrEmpty())
                {
                    throw new Exception("Missing Title");
                }

                Section newSection = new Section(0, 1, SubjectText, "placeholder description", 1, null);
                newSection.Quizzes = SelectedQuizes.ToList();
                foreach (var quiz in newSection.Quizzes)
                {
                    quiz.Exercises = await exerciseService.GetAllExercisesFromQuiz(quiz.Id);
                }
                foreach (var quiz in newSection.Quizzes)
                {
                    Debug.WriteLine(quiz);
                }
                if (SelectedExams.Count != 1)
                {
                    RaiseErrorMessage("You must have exactly one exam selected!", string.Empty);
                    return;
                }
                newSection.Exam = SelectedExams.ToList()[0];
                newSection.Exam.Exercises = await exerciseService.GetAllExercisesFromExam(newSection.Exam.Id);
                int sectionId = await sectionService.AddSection(newSection);
                /*foreach (var quiz in SelectedQuizes.ToList())
                {
                    quiz.SectionId = sectionId;
                    await quizService.UpdateQuiz(quiz);
                }*/
                Debug.WriteLine("Section created: " + newSection);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
            GoBack();
        }
    }
}
