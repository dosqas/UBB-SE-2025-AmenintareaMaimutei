using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using DuoClassLibrary.Models;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Sections;
using DuoClassLibrary.Services;
using Duo.ViewModels.Base;
using DuoClassLibrary.Services.Interfaces;

namespace Duo.ViewModels.Roadmap
{
    public class RoadmapMainPageViewModel : ViewModelBase
    {
        private IRoadmapService roadmapService;
        private DuoClassLibrary.Models.Roadmap.Roadmap roadmap;
        private IUserService userService;
        private User user;
        private BaseQuiz selectedQuiz;

        private ObservableCollection<RoadmapSectionViewModel> sectionViewModels;
        public ObservableCollection<RoadmapSectionViewModel> SectionViewModels
        {
            get => sectionViewModels;
            set => SetProperty(ref sectionViewModels, value);
        }

        public ICommand OpenQuizPreviewCommand;
        public ICommand StartQuizCommand;

        public RoadmapMainPageViewModel()
        {
            try
            {
                roadmapService = (IRoadmapService)App.ServiceProvider.GetService(typeof(IRoadmapService));
                userService = (IUserService)App.ServiceProvider.GetService(typeof(IUserService));

                StartQuizCommand = new RelayCommand((_) => _ = StartQuiz());
                OpenQuizPreviewCommand = new RelayCommandWithParameter<Tuple<int, bool>>(tuple => _ = OpenQuizPreview(tuple));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Initialization Error", $"Failed to initialize RoadmapMainPageViewModel.\nDetails: {ex.Message}");
            }
        }

        public async Task SetupViewModel()
        {
            try
            {
                roadmap = await roadmapService.GetByIdAsync(1);
                user = userService.GetCurrentUser();

                ISectionService sectionService = (ISectionService)App.ServiceProvider.GetService(typeof(ISectionService));
                List<Section> sections = null;
                try
                {
                    sections = (List<Section>)await sectionService.GetByRoadmapId(1);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("404"))
                    {
                        RaiseErrorMessage("No Sections", "Roadmap does not contain any sections yet.");
                        SectionViewModels = new ObservableCollection<RoadmapSectionViewModel>();
                        return;
                    }
                    throw;
                }

                sectionViewModels = new ObservableCollection<RoadmapSectionViewModel>();

                if (sections == null || sections.Count == 0)
                {
                    RaiseErrorMessage("No Sections", "This roadmap does not contain any sections yet.");
                    OnPropertyChanged(nameof(SectionViewModels));
                    return;
                }

                bool isPreviousCompleted = true;
                bool currentIsCompleted = false;
                for (int i = 1; i <= sections.Count; i++)
                {
                    var sectionViewModel = (RoadmapSectionViewModel)App.ServiceProvider.GetService(typeof(RoadmapSectionViewModel));
                    if (sectionViewModel == null)
                    {
                        RaiseErrorMessage("Setup Error", $"Failed to resolve RoadmapSectionViewModel for section {i}.");
                        continue;
                    }

                    currentIsCompleted = await sectionService.IsSectionCompleted(user.UserId, sections[i - 1].Id);
                    if (currentIsCompleted)
                    {
                        await sectionViewModel.SetupForSection(sections[i - 1].Id, true, 0, isPreviousCompleted);
                    }
                    else if (isPreviousCompleted)
                    {
                        await sectionViewModel.SetupForSection(sections[i - 1].Id, false, user.NumberOfCompletedQuizzesInSection, isPreviousCompleted);
                    }
                    else
                    {
                        await sectionViewModel.SetupForSection(sections[i - 1].Id, false, -1, isPreviousCompleted);
                    }
                    sectionViewModels.Add(sectionViewModel);
                    isPreviousCompleted = currentIsCompleted;
                }

                OnPropertyChanged(nameof(SectionViewModels));
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Setup Error", $"Failed to set up RoadmapMainPageViewModel.\nDetails: {ex.Message}");
            }
        }
        private async Task OpenQuizPreview(Tuple<int, bool> args)
        {
            try
            {
                Debug.WriteLine($"Opening quiz with ID: {args.Item1}");
                var quizPreviewViewModel = (RoadmapQuizPreviewViewModel)App.ServiceProvider.GetService(typeof(RoadmapQuizPreviewViewModel));

                if (quizPreviewViewModel == null)
                {
                    RaiseErrorMessage("Quiz Preview Error", "Failed to resolve RoadmapQuizPreviewViewModel from service provider.");
                    return;
                }

                await quizPreviewViewModel.OpenForQuiz(args.Item1, args.Item2);
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Quiz Preview Error", $"Failed to open quiz with ID {args.Item1}.\nDetails: {ex.Message}");
            }
        }

        private async Task StartQuiz()
        {
            try
            {
                if (selectedQuiz == null)
                {
                    RaiseErrorMessage("Start Quiz Error", "No quiz selected to start.");
                    return;
                }

                Debug.WriteLine($"Starting quiz with ID: {selectedQuiz.Id}");
                // Navigation logic goes here (placeholder as per original comment)
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Start Quiz Error", $"Failed to start quiz with ID {selectedQuiz?.Id ?? -1}.\nDetails: {ex.Message}");
            }
        }
    }
}