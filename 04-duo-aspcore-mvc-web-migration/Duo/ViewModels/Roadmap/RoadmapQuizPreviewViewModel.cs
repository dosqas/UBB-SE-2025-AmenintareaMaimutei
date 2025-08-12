using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Sections;
using DuoClassLibrary.Services;
using Duo.ViewModels.Base;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

namespace Duo.ViewModels.Roadmap
{
    public class RoadmapQuizPreviewViewModel : ViewModelBase
    {
        private BaseQuiz quiz;
        public BaseQuiz Quiz
        {
            get => quiz;
            set => SetProperty(ref quiz, value);
        }
        private Section section;
        private Visibility isPreviewVisible;
        private readonly IQuizService quizService;
        private readonly ISectionService sectionService;

        public Visibility IsPreviewVisible
        {
            get => isPreviewVisible;
            set => SetProperty(ref isPreviewVisible, value);
        }

        public string QuizOrderNumber
        {
            get
            {
                try
                {
                    if (this.quiz == null)
                    {
                        return "-1";
                    }
                    if (this.quiz is Exam)
                    {
                        return "Final Exam";
                    }
                    if (this.quiz is Quiz quizInstance)
                    {
                        return $"Quiz nr. {quizInstance.OrderNumber.ToString()}" ?? "-1";
                    }
                    return "-1";
                }
                catch (Exception ex)
                {
                    RaiseErrorMessage("Quiz Order Error", $"Failed to get quiz order number.\nDetails: {ex.Message}");
                    return "-1";
                }
            }
            set
            {
                try
                {
                    if (this.quiz is Quiz quizInstance)
                    {
                        quizInstance.OrderNumber = int.Parse(value);
                        OnPropertyChanged(nameof(QuizOrderNumber));
                    }
                }
                catch (Exception ex)
                {
                    RaiseErrorMessage("Quiz Order Error", $"Failed to set quiz order number.\nDetails: {ex.Message}");
                }
            }
        }

        public string SectionTitle
        {
            get
            {
                try
                {
                    return section?.Title ?? "Unknown Section";
                }
                catch (Exception ex)
                {
                    RaiseErrorMessage("Section Title Error", $"Failed to get section title.\nDetails: {ex.Message}");
                    return "Unknown Section";
                }
            }
        }

        public ICommand StartQuizCommand { get; set; }
        public ICommand BackButtonCommand { get; set; }

        private DispatcherQueue dispatcherQueue;

        public RoadmapQuizPreviewViewModel()
        {
            try
            {
                quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService));
                sectionService = (ISectionService)App.ServiceProvider.GetService(typeof(ISectionService));
                isPreviewVisible = Visibility.Visible;

                var mainPageViewModel = (RoadmapMainPageViewModel)App.ServiceProvider.GetService(typeof(RoadmapMainPageViewModel));
                StartQuizCommand = mainPageViewModel.StartQuizCommand;
                BackButtonCommand = new RelayCommand((_) =>
                {
                    try
                    {
                        isPreviewVisible = Visibility.Collapsed;
                        OnPropertyChanged(nameof(IsPreviewVisible));
                    }
                    catch (Exception ex)
                    {
                        RaiseErrorMessage("Back Button Error", $"Failed to handle back button action.\nDetails: {ex.Message}");
                    }
                });

                dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize RoadmapQuizPreviewViewModel.\nDetails: {ex.Message}");
            }
        }

        public async Task OpenForQuiz(int quizId, bool isExam)
        {
            try
            {
                dispatcherQueue.TryEnqueue(() =>
                {
                    try
                    {
                        IsPreviewVisible = Visibility.Visible;
                        OnPropertyChanged(nameof(IsPreviewVisible));
                    }
                    catch (Exception ex)
                    {
                        RaiseErrorMessage("UI Update Error", $"Failed to update preview visibility.\nDetails: {ex.Message}");
                    }
                });

                if (isExam)
                {
                    quiz = await quizService.GetExamById(quizId);
                }
                else
                {
                    quiz = await quizService.GetQuizById(quizId);
                    Debug.WriteLine($"Opening quiz: {quiz}");
                }

                section = await sectionService.GetSectionById((int)quiz.SectionId);

                dispatcherQueue.TryEnqueue(() =>
                {
                    try
                    {
                        Quiz = quiz;
                        OnPropertyChanged(nameof(Quiz));
                        OnPropertyChanged(nameof(SectionTitle));
                        OnPropertyChanged(nameof(QuizOrderNumber));
                    }
                    catch (Exception ex)
                    {
                        RaiseErrorMessage("UI Update Error", $"Failed to update UI properties.\nDetails: {ex.Message}");
                    }
                });

                Debug.WriteLine($"VALUE OF QUIZ: {QuizOrderNumber}, {SectionTitle}, {IsPreviewVisible}");
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Open Quiz Error", $"Failed to open quiz with ID {quizId}.\nDetails: {ex.Message}");
            }
        }
    }
}