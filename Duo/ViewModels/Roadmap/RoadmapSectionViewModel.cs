using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Sections;
using DuoClassLibrary.Services;
using Duo.ViewModels.Base;
using DuoClassLibrary.Models;
using Microsoft.UI.Dispatching;
using DuoClassLibrary.Services.Interfaces;

namespace Duo.ViewModels.Roadmap
{
    public class RoadmapSectionViewModel : ViewModelBase
    {
        private ISectionService sectionService;
        private int sectionId;
        private Section section;
        private User user;

        public Section Section
        {
            get => section;
        }

        private bool isCompleted;
        private int nrOfCompletedQuizzes;

        public ICommand OpenQuizPreviewCommand { get; set; }

        private ObservableCollection<RoadmapButtonTemplate> quizButtonTemplates;

        public ObservableCollection<RoadmapButtonTemplate> QuizButtonTemplates
        {
            get => quizButtonTemplates;
            private set
            {
                quizButtonTemplates = value;
                OnPropertyChanged(nameof(QuizButtonTemplates));
            }
        }

        private RoadmapButtonTemplate examButtonTemplate;

        public RoadmapButtonTemplate ExamButtonTemplate
        {
            get => examButtonTemplate;
            private set
            {
                examButtonTemplate = value;
                OnPropertyChanged(nameof(ExamButtonTemplate));
            }
        }

        public RoadmapSectionViewModel()
        {
            try
            {
                sectionService = (ISectionService)(App.ServiceProvider.GetService(typeof(ISectionService)));
                var mainPageViewModel = (RoadmapMainPageViewModel)(App.ServiceProvider.GetService(typeof(RoadmapMainPageViewModel)));
                OpenQuizPreviewCommand = mainPageViewModel.OpenQuizPreviewCommand;
                quizButtonTemplates = new ObservableCollection<RoadmapButtonTemplate>();
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize RoadmapSectionViewModel.\nDetails: {ex.Message}");
            }
        }

        public async Task SetupForSection(int sectionId, bool isCompleted, int nrOfCompletedQuizzes, bool isPreviousSectionCompleted)
        {
            try
            {
                Debug.WriteLine($"--------------------Setting up section with ID {sectionId}, Compl {isCompleted}, nr_quiz {nrOfCompletedQuizzes}");
                this.sectionId = sectionId;
                section = await sectionService.GetSectionById(this.sectionId);

                IQuizService quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService));
                IUserService userService = (IUserService)App.ServiceProvider.GetService(typeof(IUserService));
                this.user = await userService.GetUserById(1);
                section.Quizzes = await quizService.GetAllQuizzesFromSection(this.sectionId);
                section.Exam = await quizService.GetExamFromSection(this.sectionId);

                this.isCompleted = isCompleted;
                this.nrOfCompletedQuizzes = nrOfCompletedQuizzes;

                OnPropertyChanged(nameof(Section));

                List<bool> completedQuizzes = new List<bool>();
                foreach (Quiz quiz in section.GetAllQuizzes())
                {
                    completedQuizzes.Add(await quizService.IsQuizCompleted(user.UserId, quiz.Id));
                }
                PopulateQuizButtonTemplates(completedQuizzes, isPreviousSectionCompleted);
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Setup Section Error", $"Failed to set up section with ID {sectionId}.\nDetails: {ex.Message}");
            }
        }

        private void PopulateQuizButtonTemplates(List<bool> completedQuizzes, bool isPreviousSectionCompleted)
        {
            try
            {
                IQuizService quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService));

                Debug.WriteLine($"++++++ Populating section {section.Id}");
                quizButtonTemplates.Clear();
                bool previousIsCompleted = isPreviousSectionCompleted;
                bool currentIsCompleted = false;
                int counter = 0;
                foreach (Quiz quiz in section.GetAllQuizzes())
                {
                    currentIsCompleted = completedQuizzes[counter];
                    RoadmapButtonTemplate.QUIZ_STATUS quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.LOCKED;
                    if (isCompleted)
                    {
                        quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.COMPLETED;
                    }
                    else if (currentIsCompleted)
                    {
                        quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.COMPLETED;
                    }
                    else if (previousIsCompleted && !currentIsCompleted)
                    {
                        quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.INCOMPLETE;
                    }

                    previousIsCompleted = currentIsCompleted;
                    counter++;
                    Debug.WriteLine($"++++++++++ Populating quiz {quiz.Id} -> {quizStatus}");
                    quizButtonTemplates.Add(new RoadmapButtonTemplate(quiz, OpenQuizPreviewCommand, quizStatus));
                }

                RoadmapButtonTemplate.QUIZ_STATUS examStatus = RoadmapButtonTemplate.QUIZ_STATUS.LOCKED;
                if (isCompleted)
                {
                    examStatus = RoadmapButtonTemplate.QUIZ_STATUS.COMPLETED;
                }
                else if (previousIsCompleted)
                {
                    examStatus = RoadmapButtonTemplate.QUIZ_STATUS.INCOMPLETE;
                }
                Debug.WriteLine($"++++++++++ Populating exam -> {examStatus}");
                examButtonTemplate = new RoadmapButtonTemplate(section.GetFinalExam(), OpenQuizPreviewCommand, examStatus);

                OnPropertyChanged(nameof(QuizButtonTemplates));
                OnPropertyChanged(nameof(ExamButtonTemplate));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Populate Buttons Error", $"Failed to populate quiz and exam button templates.\nDetails: {ex.Message}");
            }
        }
    }

    public class RoadmapButtonTemplate
    {
        public enum QUIZ_STATUS
        {
            LOCKED = 1,
            COMPLETED = 2,
            INCOMPLETE = 3
        }

        public BaseQuiz Quiz { get; }
        public ICommand OpenQuizPreviewCommand { get; }

        public QUIZ_STATUS QuizStatus
        {
            get; set;
        }

        public RoadmapButtonTemplate(BaseQuiz quiz, ICommand openQuizPreviewCommand, QUIZ_STATUS status)
        {
            Quiz = quiz;
            OpenQuizPreviewCommand = openQuizPreviewCommand;
            QuizStatus = status;
        }
    }
}