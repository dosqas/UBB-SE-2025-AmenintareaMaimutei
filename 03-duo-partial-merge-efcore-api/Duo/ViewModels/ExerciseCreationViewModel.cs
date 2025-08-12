using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Helpers;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.Base;
using Duo.ViewModels.CreateExerciseViewModels;
using Duo.Views.Components;
using Duo.Views.Components.CreateExerciseComponents;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Duo.ViewModels
{
    internal partial class ExerciseCreationViewModel : AdminBaseViewModel
    {
        private readonly IExerciseService exerciseService;
        private readonly IExerciseViewFactory exerciseViewFactory;
        private object selectedExerciseContent;

        private string questionText = string.Empty;
        public ObservableCollection<string> ExerciseTypes { get; set; }

        public ObservableCollection<string> Difficulties { get; set; }

        private string selectedExerciseType;

        private string selectedDifficulty;

        private object currentExerciseViewModel;

        public CreateAssociationExerciseViewModel CreateAssociationExerciseViewModel { get; }
        public CreateFillInTheBlankExerciseViewModel CreateFillInTheBlankExerciseViewModel { get; }
        public CreateMultipleChoiceExerciseViewModel CreateMultipleChoiceExerciseViewModel { get; }
        public CreateFlashcardExerciseViewModel CreateFlashcardExerciseViewModel { get; } = new ();

        public ExerciseCreationViewModel(IExerciseService exerciseService, IExerciseViewFactory exerciseViewFactory)
        {
            this.exerciseService = exerciseService;
            this.exerciseViewFactory = exerciseViewFactory;
            CreateMultipleChoiceExerciseViewModel = new CreateMultipleChoiceExerciseViewModel(this);
            CreateAssociationExerciseViewModel = new CreateAssociationExerciseViewModel(this);
            CreateFillInTheBlankExerciseViewModel = new CreateFillInTheBlankExerciseViewModel(this);

            SaveButtonCommand = new RelayCommand((_) => _ = CreateExercise());
            ExerciseTypes = new ObservableCollection<string>(Models.Exercises.ExerciseTypes.EXERCISE_TYPES);
            Difficulties = new ObservableCollection<string>(Models.DifficultyList.DIFFICULTIES);
            SelectedExerciseContent = "Select an exercise type.";
            currentExerciseViewModel = CreateAssociationExerciseViewModel;
        }

        public ExerciseCreationViewModel()
        {
            try
            {
                exerciseService = (IExerciseService)App.ServiceProvider.GetService(typeof(IExerciseService));
                exerciseViewFactory = (IExerciseViewFactory)App.ServiceProvider.GetService(typeof(IExerciseViewFactory));

                CreateMultipleChoiceExerciseViewModel = new CreateMultipleChoiceExerciseViewModel(this);
                CreateAssociationExerciseViewModel = new CreateAssociationExerciseViewModel(this);
                CreateFillInTheBlankExerciseViewModel = new CreateFillInTheBlankExerciseViewModel(this);

                SaveButtonCommand = new RelayCommand((_) => _ = CreateExercise());

                ExerciseTypes = new ObservableCollection<string>(Models.Exercises.ExerciseTypes.EXERCISE_TYPES);

                Difficulties = new ObservableCollection<string>(Models.DifficultyList.DIFFICULTIES);

                SelectedExerciseContent = new TextBlock { Text = "Select an exercise type." };
                currentExerciseViewModel = CreateAssociationExerciseViewModel;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to initialize ExerciseCreationViewModel", ex.Message);
            }
        }

        public string QuestionText
        {
            get => questionText;
            set
            {
                if (questionText != value)
                {
                    questionText = value;
                    OnPropertyChanged(nameof(QuestionText));
                }
            }
        }

        public string SelectedExerciseType
        {
            get => selectedExerciseType;
            set
            {
                if (selectedExerciseType != value)
                {
                    selectedExerciseType = value;
                    OnPropertyChanged(nameof(SelectedExerciseType));
                    Debug.WriteLine(value);
                    UpdateExerciseContent(value);
                }
            }
        }

        public string SelectedDifficulty
        {
            get => selectedDifficulty;
            set
            {
                if (selectedDifficulty != value)
                {
                    selectedDifficulty = value;
                    OnPropertyChanged(nameof(SelectedDifficulty));
                }
            }
        }

        public object SelectedExerciseContent
        {
            get => selectedExerciseContent;
            set
            {
                if (selectedExerciseContent != value)
                {
                    selectedExerciseContent = value;
                    OnPropertyChanged(nameof(SelectedExerciseContent));
                }
            }
        }

        public object CurrentExerciseViewModel
        {
            get => currentExerciseViewModel;
            set
            {
                currentExerciseViewModel = value;
                OnPropertyChanged(nameof(CurrentExerciseViewModel));
            }
        }

        private bool isSuccessMessageVisible;
        public bool IsSuccessMessageVisible
        {
            get => isSuccessMessageVisible;
            set => SetProperty(ref isSuccessMessageVisible, value);
        }

        public async void ShowSuccessMessage()
        {
            try
            {
                IsSuccessMessageVisible = true;
                await Task.Delay(3000);
                IsSuccessMessageVisible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while showing success message: {ex.Message}");
            }
        }

        private void UpdateExerciseContent(string exerciseType)
        {
            try
            {
                SelectedExerciseContent = exerciseViewFactory.CreateExerciseView(exerciseType);

                switch (exerciseType)
                {
                    case "Association":
                        CurrentExerciseViewModel = CreateAssociationExerciseViewModel;
                        break;
                    case "Fill in the blank":
                        CurrentExerciseViewModel = CreateFillInTheBlankExerciseViewModel;
                        break;
                    case "Multiple Choice":
                        CurrentExerciseViewModel = CreateMultipleChoiceExerciseViewModel;
                        break;
                    case "Flashcard":
                        CurrentExerciseViewModel = CreateFlashcardExerciseViewModel;
                        break;
                    default:
                        SelectedExerciseContent = new TextBlock { Text = "Select an exercise type." };
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to update exercise content", ex.Message);
            }
        }

        public void SetTypeForTest(string exerciseType)
        {
            selectedExerciseType = exerciseType;
        }

        public ICommand SaveButtonCommand { get; }

        public async Task CreateExercise()
        {
            try
            {
                Debug.WriteLine(SelectedExerciseType);
                switch (SelectedExerciseType)
                {
                    case "Multiple Choice":
                        await CreateMultipleChoiceExercise(); break;
                    case "Association":
                        await CreateAssocitationExercise(); break;
                    case "Flashcard":
                        await CreateFlashcardExercise(); break;
                    case "Fill in the blank":
                        await CreateFillInTheBlankExercise(); break;
                    default:
                        RaiseErrorMessage("Invalid type", "No valid exercise type selected.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Exercise creation failed", ex.Message);
            }
        }

        public async Task CreateMultipleChoiceExercise()
        {
            try
            {
                Duo.Models.Difficulty difficulty = GetDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateMultipleChoiceExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async Task CreateAssocitationExercise()
        {
            try
            {
                Duo.Models.Difficulty difficulty = GetDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateAssociationExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async Task CreateFlashcardExercise()
        {
            try
            {
                Duo.Models.Difficulty difficulty = GetDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateFlashcardExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async Task CreateFillInTheBlankExercise()
        {
            try
            {
                Duo.Models.Difficulty difficulty = GetDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateFillInTheBlankExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        private Duo.Models.Difficulty GetDifficulty(string difficulty)
        {
            try
            {
                return difficulty switch
                {
                    "Easy" => Models.Difficulty.Easy,
                    "Normal" => Models.Difficulty.Normal,
                    "Hard" => Models.Difficulty.Hard,
                    _ => Models.Difficulty.Normal
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage("Failed to parse difficulty", ex.Message);
                return Models.Difficulty.Normal;
            }
        }
    }
}