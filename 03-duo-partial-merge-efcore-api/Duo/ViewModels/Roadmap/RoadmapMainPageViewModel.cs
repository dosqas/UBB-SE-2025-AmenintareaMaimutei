using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels.Base;

namespace Duo.ViewModels.Roadmap
{
    public class RoadmapMainPageViewModel : ViewModelBase
    {
        private IRoadmapService roadmapService;
        private Duo.Models.Roadmap.Roadmap roadmap;
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
                user = await userService.GetByIdAsync(1);

                ISectionService sectionService = (ISectionService)App.ServiceProvider.GetService(typeof(ISectionService));
                List<Section> sections = (List<Section>)await sectionService.GetByRoadmapId(1);

                sectionViewModels = new ObservableCollection<RoadmapSectionViewModel>();
                for (int i = 1; i <= sections.Count; i++)
                {
                    var sectionViewModel = (RoadmapSectionViewModel)App.ServiceProvider.GetService(typeof(RoadmapSectionViewModel));
                    if (sectionViewModel == null)
                    {
                        RaiseErrorMessage("Setup Error", $"Failed to resolve RoadmapSectionViewModel for section {i}.");
                        continue;
                    }

                    if (i <= user.NumberOfCompletedSections)
                    {
                        await sectionViewModel.SetupForSection(sections[i - 1].Id, true, 0);
                    }
                    else if (i == user.NumberOfCompletedSections + 1)
                    {
                        await sectionViewModel.SetupForSection(sections[i - 1].Id, false, user.NumberOfCompletedQuizzesInSection);
                    }
                    else
                    {
                        await sectionViewModel.SetupForSection(sections[i - 1].Id, false, -1);
                    }
                    sectionViewModels.Add(sectionViewModel);
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