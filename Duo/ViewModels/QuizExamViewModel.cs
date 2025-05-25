using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuoClassLibrary.Models;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Sections;
using DuoClassLibrary.Services;
using Duo.ViewModels.Base;
using DuoClassLibrary.Services.Interfaces;

namespace Duo.ViewModels
{
    public class QuizExamViewModel : ViewModelBase
    {
        private readonly IExerciseService exerciseService;
        private readonly IQuizService quizService;
        private int quizId;
        private int examId;
        private Quiz currentQuiz;
        private Exam currentExam;
        private List<Exercise> exercises;
        private Exercise currentExercise;
        private int currentExerciseIndex;
        private bool? validatedCurrent = null;

        public int QuizId
        {
            get => quizId;
            private set
            {
                quizId = value;
                OnPropertyChanged(nameof(QuizId));
            }
        }

        public int ExamId
        {
            get => examId;
            private set
            {
                examId = value;
                OnPropertyChanged(nameof(ExamId));
            }
        }

        public List<Exercise> Exercises
        {
            get => exercises;
            private set
            {
                exercises = value;
                OnPropertyChanged(nameof(Exercises));
            }
        }

        public Exercise CurrentExercise
        {
            get => currentExercise;
            set
            {
                currentExercise = value;
                OnPropertyChanged(nameof(CurrentExercise));
            }
        }

        public Quiz CurrentQuiz
        {
            get => currentQuiz;
            set
            {
                currentQuiz = value;
                OnPropertyChanged(nameof(CurrentQuiz));
            }
        }

        public Exam CurrentExam
        {
            get => currentExam;
            set
            {
                currentExam = value;
                OnPropertyChanged(nameof(CurrentExam));
            }
        }

        public int CurrentExerciseIndex
        {
            get => currentExerciseIndex;
            set
            {
                currentExerciseIndex = value;
                OnPropertyChanged(nameof(CurrentExerciseIndex));
            }
        }

        public bool? ValidatedCurrent
        {
            get => validatedCurrent;
            set
            {
                validatedCurrent = value;
                OnPropertyChanged(nameof(ValidatedCurrent));
            }
        }

        public string CorrectAnswersText
        {
            get
            {
                try
                {
                    if (QuizId != -1)
                    {
                        return $"{CurrentQuiz.GetNumberOfCorrectAnswers()}/{CurrentQuiz.GetNumberOfAnswersGiven()}";
                    }
                    return $"{CurrentExam.GetNumberOfCorrectAnswers()}/{CurrentExam.GetNumberOfAnswersGiven()}";
                }
                catch (Exception ex)
                {
                    RaiseErrorMessage("Correct Answers Error", $"Failed to get correct answers text.\nDetails: {ex.Message}");
                    return "0/0";
                }
            }
        }

        public string PassingPercentText
        {
            get
            {
                try
                {
                    return $"{(int)Math.Round(GetPercentageCorrect() * 100)}%";
                }
                catch (Exception ex)
                {
                    RaiseErrorMessage("Passing Percent Error", $"Failed to get passing percent text.\nDetails: {ex.Message}");
                    return "0%";
                }
            }
        }

        public string IsPassedText
        {
            get
            {
                try
                {
                    if (IsPassed())
                    {
                        return "Great job! You passed this one.";
                    }
                    return "You need to redo this one.";
                }
                catch (Exception ex)
                {
                    RaiseErrorMessage("Pass Status Error", $"Failed to get pass status text.\nDetails: {ex.Message}");
                    return "Error determining pass status.";
                }
            }
        }

        public QuizExamViewModel(IExerciseService exerciseService)
        {
            try
            {
                this.exerciseService = exerciseService ?? throw new ArgumentNullException(nameof(exerciseService));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize QuizExamViewModel.\nDetails: {ex.Message}");
            }
        }

        public QuizExamViewModel()
        {
            try
            {
                exerciseService = (IExerciseService)App.ServiceProvider.GetService(typeof(IExerciseService))
                    ?? throw new InvalidOperationException("ExerciseService not found.");
                quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService))
                    ?? throw new InvalidOperationException("QuizService not found.");
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize QuizExamViewModel.\nDetails: {ex.Message}");
            }
        }

        public async Task SetQuizIdAsync(int value)
        {
            try
            {
                QuizId = value;
                ExamId = -1;
                await LoadQuizData();
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Set Quiz Error", $"Failed to set quiz ID and load data.\nDetails: {ex.Message}");
            }
        }

        public async Task SetExamIdAsync(int value)
        {
            try
            {
                ExamId = value;
                QuizId = -1;
                await LoadExamData();
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Set Exam Error", $"Failed to set exam ID and load data.\nDetails: {ex.Message}");
            }
        }

        private async Task LoadQuizData()
        {
            try
            {
                await LoadQuiz();
                await LoadExercises();
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Load Quiz Data Error", $"Failed to load quiz data.\nDetails: {ex.Message}");
            }
        }

        private async Task LoadExamData()
        {
            try
            {
                await LoadExam();
                await LoadExercises();
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Load Exam Data Error", $"Failed to load exam data.\nDetails: {ex.Message}");
            }
        }

        public async Task LoadQuiz()
        {
            try
            {
                CurrentQuiz = await quizService.GetQuizById(QuizId)
                    ?? throw new InvalidOperationException("Quiz not found.");
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Load Quiz Error", $"Failed to load quiz with ID {QuizId}.\nDetails: {ex.Message}");
                CurrentQuiz = null;
            }
        }

        public async Task LoadExam()
        {
            try
            {
                CurrentExam = await quizService.GetExamById(ExamId)
                    ?? throw new InvalidOperationException("Exam not found.");
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Load Exam Error", $"Failed to load exam with ID {ExamId}.\nDetails: {ex.Message}");
                CurrentExam = null;
            }
        }

        public async Task LoadExercises()
        {
            try
            {
                if (QuizId != -1)
                {
                    Exercises = await exerciseService.GetAllExercisesFromQuiz(QuizId)
                        ?? new List<Exercise>();
                    CurrentQuiz.Exercises = Exercises;
                }
                else
                {
                    Exercises = await exerciseService.GetAllExercisesFromExam(ExamId)
                        ?? new List<Exercise>();
                    CurrentExam.Exercises = Exercises;
                }

                CurrentExerciseIndex = 0;
                CurrentExercise = Exercises.Any() ? Exercises[CurrentExerciseIndex] : null;
                ValidatedCurrent = null;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Load Exercises Error", $"Failed to load exercises.\nDetails: {ex.Message}");
                Exercises = new List<Exercise>();
                CurrentExercise = null;
            }
        }

        private void UpdateQuiz(bool? isExerciseValid)
        {
            try
            {
                if (isExerciseValid == true)
                {
                    CurrentQuiz.IncrementCorrectAnswers();
                }
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Update Quiz Error", $"Failed to update quiz progress.\nDetails: {ex.Message}");
            }
        }

        private void UpdateExam(bool? isExerciseValid)
        {
            try
            {
                if (isExerciseValid == true)
                {
                    CurrentExam.IncrementCorrectAnswers();
                }
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Update Exam Error", $"Failed to update exam progress.\nDetails: {ex.Message}");
            }
        }

        public bool? ValidateCurrentExercise(object responses)
        {
            try
            {
                if (ValidatedCurrent is not null)
                {
                    return ValidatedCurrent;
                }

                bool isValid = false;

                if (CurrentExercise is AssociationExercise associationExercise)
                {
                    isValid = associationExercise.ValidateAnswer((List<(string, string)>)responses);
                }
                else if (CurrentExercise is FillInTheBlankExercise fillInTheBlanksExercise)
                {
                    isValid = fillInTheBlanksExercise.ValidateAnswer((List<string>)responses);
                }
                else if (CurrentExercise is MultipleChoiceExercise multipleChoiceExercise)
                {
                    isValid = multipleChoiceExercise.ValidateAnswer((List<string>)responses);
                }
                else if (CurrentExercise is FlashcardExercise flashcardExercise)
                {
                    isValid = flashcardExercise.ValidateAnswer((string)responses);
                }

                ValidatedCurrent = isValid;
                if (QuizId != -1)
                {
                    UpdateQuiz(ValidatedCurrent);
                }
                else
                {
                    UpdateExam(ValidatedCurrent);
                }

                return isValid;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Validate Exercise Error", $"Failed to validate current exercise.\nDetails: {ex.Message}");
                return false;
            }
        }

        public bool LoadNext()
        {
            try
            {
                if (ValidatedCurrent == null)
                {
                    return false;
                }

                CurrentExerciseIndex += 1;
                if (Exercises.Count <= CurrentExerciseIndex)
                {
                    CurrentExercise = null;
                }
                else
                {
                    CurrentExercise = Exercises[CurrentExerciseIndex];
                }

                ValidatedCurrent = null;
                return true;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Load Next Error", $"Failed to load next exercise.\nDetails: {ex.Message}");
                return false;
            }
        }

        public float GetPercentageDone()
        {
            try
            {
                if (QuizId != -1)
                {
                    return (float)CurrentQuiz.GetNumberOfAnswersGiven() / Exercises.Count;
                }
                return (float)CurrentExam.GetNumberOfAnswersGiven() / Exercises.Count;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Percentage Done Error", $"Failed to calculate percentage done.\nDetails: {ex.Message}");
                return 0f;
            }
        }

        private float GetPercentageCorrect()
        {
            try
            {
                if (QuizId != -1)
                {
                    return (float)CurrentQuiz.GetNumberOfCorrectAnswers() / Exercises.Count;
                }
                return (float)CurrentExam.GetNumberOfCorrectAnswers() / Exercises.Count;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Percentage Correct Error", $"Failed to calculate percentage correct.\nDetails: {ex.Message}");
                return 0f;
            }
        }

        private bool IsPassed()
        {
            try
            {
                int percentCorrect = (int)Math.Round(GetPercentageCorrect() * 100);
                if (QuizId != -1)
                {
                    return percentCorrect >= CurrentQuiz.GetPassingThreshold();
                }
                return percentCorrect >= CurrentExam.GetPassingThreshold();
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Pass Check Error", $"Failed to determine pass status.\nDetails: {ex.Message}");
                return false;
            }
        }

        public string GetCurrentExerciseCorrectAnswer()
        {
            try
            {
                string correctAnswers = string.Empty;

                if (CurrentExercise is AssociationExercise associationExercise)
                {
                    for (int i = 0; i < associationExercise.FirstAnswersList.Count; i++)
                    {
                        correctAnswers += associationExercise.FirstAnswersList[i] + " - " + associationExercise.SecondAnswersList[i] + "\n";
                    }
                }
                else if (CurrentExercise is FillInTheBlankExercise fillInTheBlanksExercise)
                {
                    foreach (var answer in fillInTheBlanksExercise.PossibleCorrectAnswers)
                    {
                        correctAnswers += answer + "\n";
                    }
                }
                else if (CurrentExercise is MultipleChoiceExercise multipleChoiceExercise)
                {
                    correctAnswers += multipleChoiceExercise.Choices
                        .Where(choice => choice.IsCorrect)
                        .Select(choice => choice.Answer)
                        .First();
                }
                else if (CurrentExercise is FlashcardExercise flashcardExercise)
                {
                    correctAnswers += string.Join(", ", flashcardExercise.GetCorrectAnswer());
                }

                return correctAnswers;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Correct Answer Error", $"Failed to get correct answer.\nDetails: {ex.Message}");
                return string.Empty;
            }
        }

        public async Task MarkUserProgression()
        {
            try
            {
                if (GetPercentageDone() != 1)
                {
                    return;
                }

                if (!IsPassed())
                {
                    return;
                }

                IUserService userService = (IUserService)App.ServiceProvider.GetService(typeof(IUserService))
                    ?? throw new InvalidOperationException("IUserService not found.");
                ISectionService sectionService = (ISectionService)App.ServiceProvider.GetService(typeof(ISectionService))
                    ?? throw new InvalidOperationException("ISectionService not found.");

                User user = await userService.GetUserById(1)
                    ?? throw new InvalidOperationException("User not found.");
                List<Section> sections = await sectionService.GetByRoadmapId(1)
                    ?? throw new InvalidOperationException("Sections not found.");

                int sectionId;
                if (QuizId == -1)
                {
                    sectionId = CurrentExam.SectionId.HasValue ? CurrentExam.SectionId.Value : -1;
                }
                else
                {
                    sectionId = CurrentQuiz.SectionId.HasValue ? CurrentQuiz.SectionId.Value : -1;
                }
                Section currentUserSection = await sectionService.GetSectionById(sectionId)
                    ?? throw new InvalidOperationException("Current user section not found.");
                List<Quiz> currentSectionQuizzes = await quizService.GetAllQuizzesFromSection(currentUserSection.Id)
                    ?? throw new InvalidOperationException("Quizzes not found.");

                if (QuizId == -1)
                {
                    if (CurrentExam == null)
                    {
                        RaiseErrorMessage("Progression Error", "Current exam is not set.");
                        return;
                    }
                    foreach (Quiz q in currentSectionQuizzes)
                    {
                        bool isCompleted = await quizService.IsQuizCompleted(user.UserId, q.Id);
                        if (!isCompleted)
                        {
                            RaiseErrorMessage("Progression Error", "Not all quizzes in section completed.");
                            return;
                        }
                    }
                    if (currentUserSection.GetFinalExam().Id != ExamId)
                    {
                        RaiseErrorMessage("Progression Error", "Exam ID does not match section's final exam.");
                        return;
                    }
                    bool isExamCompleted = await quizService.IsExamCompleted(user.UserId, ExamId);
                    if (isExamCompleted)
                    {
                        RaiseErrorMessage("Progression Error", "Exam already completed.");
                        return;
                    }

                    if (CurrentExam.SectionId == null)
                    {
                        RaiseErrorMessage("Error", "Exam has no section Id");
                        return;
                    }
                    else
                    {
                        await quizService.CompleteExam(user.UserId, ExamId);
                        await sectionService.CompleteSection(user.UserId, sectionId);
                    }
                }
                else
                {
                    if (CurrentQuiz == null)
                    {
                        RaiseErrorMessage("Progression Error", "Current quiz is not set.");
                        return;
                    }
                    int ct = 0;
                    foreach (Quiz q in currentSectionQuizzes)
                    {
                        bool isCompleted = await quizService.IsQuizCompleted(user.UserId, q.Id);
                        if (isCompleted)
                        {
                            ct++;
                        }
                    }
                    if (ct == currentSectionQuizzes.Count)
                    {
                        RaiseErrorMessage("Progression Error", "All quizzes already completed.");
                        return;
                    }
                    if (CurrentQuiz.Id != QuizId)
                    {
                        RaiseErrorMessage("Progression Error", "Quiz ID does not match expected quiz.");
                        return;
                    }
                    await quizService.CompleteQuiz(user.UserId, QuizId);
                }
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Mark Progression Error", $"Failed to mark user progression.\nDetails: {ex.Message}");
            }
        }
    }
}