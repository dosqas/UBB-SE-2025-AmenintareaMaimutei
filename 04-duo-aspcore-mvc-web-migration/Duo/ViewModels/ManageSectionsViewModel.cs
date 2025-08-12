using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Sections;
using DuoClassLibrary.Services;
using Duo.ViewModels.Base;

namespace Duo.ViewModels
{
    internal class ManageSectionsViewModel : AdminBaseViewModel
    {
        private readonly ISectionService sectionService;
        private readonly IQuizService quizService;

        public ObservableCollection<Section> Sections { get; set; } = new ObservableCollection<Section>();
        public ObservableCollection<Quiz> SectionQuizes { get; private set; } = new ObservableCollection<Quiz>();

        private Section selectedSection;
        public Section SelectedSection
        {
            get => selectedSection;
            set
            {
                selectedSection = value;
                _ = UpdateSectionQuizes(SelectedSection);
                OnPropertyChanged();
            }
        }

        public ICommand DeleteSectionCommand { get; }

        public ManageSectionsViewModel()
        {
            try
            {
                sectionService = (ISectionService)App.ServiceProvider.GetService(typeof(ISectionService));
                quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Initialization error", ex.Message);
            }

            DeleteSectionCommand = new RelayCommandWithParameter<Section>(section => _ = DeleteSection(section));
            _ = LoadSectionsAsync();
        }

        public async Task LoadSectionsAsync()
        {
            try
            {
                var sections = await sectionService.GetAllSections();
                Sections.Clear();

                foreach (var section in sections)
                {
                    Sections.Add(section);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadSectionsAsync error: {ex.Message}");
                RaiseErrorMessage("Failed to load sections", ex.Message);
            }
        }

        public async Task UpdateSectionQuizes(Section selectedSection)
        {
            try
            {
                Debug.WriteLine("Updating section quizzes...");
                SectionQuizes.Clear();

                if (selectedSection == null)
                {
                    Debug.WriteLine("No section selected. Skipping update.");
                    return;
                }

                var quizzes = await quizService.GetAllQuizzesFromSection(selectedSection.Id);
                foreach (var quiz in quizzes)
                {
                    SectionQuizes.Add(quiz);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateSectionQuizes error: {ex.Message}");
                RaiseErrorMessage("Failed to load quizzes for section", ex.Message);
            }
        }

        public async Task DeleteSection(Section sectionToBeDeleted)
        {
            try
            {
                Debug.WriteLine("Deleting section...");

                if (sectionToBeDeleted == SelectedSection)
                {
                    SelectedSection = null;
                }

                await sectionService.DeleteSection(sectionToBeDeleted.Id);
                Sections.Remove(sectionToBeDeleted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeleteSection error: {ex.Message}");
                RaiseErrorMessage("Failed to delete section", ex.Message);
            }
        }
    }
}
