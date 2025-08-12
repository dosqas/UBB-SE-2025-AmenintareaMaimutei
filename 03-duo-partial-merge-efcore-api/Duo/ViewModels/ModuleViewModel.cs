using System.Windows.Input;
using Duo.Models;
using Duo.Services;
using Duo.Commands;

namespace Duo.ViewModels
{
    public partial class ModuleViewModel : BaseViewModel, IModuleViewModel
    {
        private readonly ICourseService courseService;
        private readonly ICoinsService coinsService;
        private readonly ICourseViewModel courseViewModel;
        public Module CurrentModule { get; set; }
        public bool IsCompleted { get; set; }
        public ICommand CompleteModuleCommand { get; set; }

        public ICommand ModuleImageClickCommand { get; set; }

        public ModuleViewModel(Models.Module module, ICourseViewModel courseVM,
            ICourseService? courseServiceOverride = null,
            ICoinsService? coinsServiceOverride = null)
        {
            // Corrected initialization: Use the proper concrete service classes
            courseService = courseServiceOverride ?? new CourseService();
            coinsService = coinsServiceOverride ?? new CoinsService();

            CurrentModule = module;
            IsCompleted = courseService.IsModuleCompleted(module.ModuleId);
            CompleteModuleCommand = new RelayCommand(ExecuteCompleteModule, CanCompleteModule);
            ModuleImageClickCommand = new RelayCommand(HandleModuleImageClick);
            courseViewModel = courseVM;

            courseService.OpenModule(module.ModuleId);

            courseViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(courseViewModel.FormattedTimeRemaining))
                {
                    OnPropertyChanged(nameof(TimeSpent));
                }
            };

            courseService.OpenModule(module.ModuleId);
        }

        public void HandleModuleImageClick(object? obj)
        {
            var confirmStatus = courseService.ClickModuleImage(CurrentModule.ModuleId);
            if (confirmStatus)
            {
                OnPropertyChanged(nameof(CoinBalance));
            }
        }

        public string TimeSpent => courseViewModel.FormattedTimeRemaining;

        public int CoinBalance
        {
            get => coinsService.GetCoinBalance(0);
        }

        private bool CanCompleteModule(object? parameter)
        {
            return !IsCompleted;
        }

        private void ExecuteCompleteModule(object? parameter)
        {
            courseViewModel.MarkModuleAsCompletedAndCheckRewards(CurrentModule.ModuleId);
            IsCompleted = true;
            OnPropertyChanged(nameof(IsCompleted));
            courseViewModel.RefreshCourseModulesDisplay();
        }
        public void ExecuteModuleImageClick(object? obj)
        {
            if (courseService.ClickModuleImage(CurrentModule.ModuleId))
            {
                OnPropertyChanged(nameof(CoinBalance));
                courseViewModel.RefreshCourseModulesDisplay();
            }
        }
    }
}
