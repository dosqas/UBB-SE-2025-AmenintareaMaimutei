using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Windows.System;
using Windows.UI.Core;

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
        public int UserId { get; set; }

        public ICommand ModuleImageClickCommand { get; set; }

        public ModuleViewModel(Module module, ICourseViewModel courseVM, int userId,
                    ICourseService? courseServiceOverride = null,
                    ICoinsService? coinsServiceOverride = null)
        {
            this.UserId = userId;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7174")
            };

            courseService = courseServiceOverride ?? new CourseService(new CourseServiceProxy(httpClient));
            coinsService = coinsServiceOverride ?? new CoinsService(new CoinsServiceProxy(httpClient));
            var userService = App.ServiceProvider.GetRequiredService<IUserService>();

            _ = EnsureUserExistsAsync(userService);

            CurrentModule = module;
            // Fix for CS0029: Await the asynchronous method to get the result
            courseViewModel = courseVM;

            CompleteModuleCommand = new RelayCommand(ExecuteCompleteModule, CanCompleteModule);
            ModuleImageClickCommand = new RelayCommand(HandleModuleImageClick);
            courseViewModel = courseVM;

            courseService.OpenModuleAsync(UserId, module.ModuleId);

            _ = InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            try
            {
                var userService = App.ServiceProvider.GetRequiredService<IUserService>();
                await EnsureUserExistsAsync(userService);

                IsCompleted = await courseService.IsModuleCompletedAsync(UserId, CurrentModule.ModuleId);
                await courseService.OpenModuleAsync(UserId, CurrentModule.ModuleId);

                courseViewModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(courseViewModel.FormattedTimeRemaining))
                    {
                        OnPropertyChanged(nameof(TimeSpent));
                    }
                };
                OnPropertyChanged(nameof(IsCompleted));

                await LoadCoinBalanceAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in InitializeAsync: {ex.Message}");
            }
        }

        public async Task HandleModuleImageClick(object? obj)
        {
            try
            {
                var confirmStatus = await courseService.ClickModuleImageAsync(UserId, CurrentModule.ModuleId);
                if (confirmStatus)
                {
                    OnPropertyChanged(nameof(CoinBalance));
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"ClickModuleImage failed: {ex.Message}");
            }
        }

        public string TimeSpent => courseViewModel.FormattedTimeRemaining;

        private int coinBalance;
        public int CoinBalance
        {
            get => coinBalance;
            private set
            {
                coinBalance = value;
                OnPropertyChanged(nameof(CoinBalance));
            }
        }

        // Async method to load and update the CoinBalance
        public async Task LoadCoinBalanceAsync()
        {
            CoinBalance = await coinsService.GetCoinBalanceAsync(UserId);
        }

        private bool CanCompleteModule(object? parameter)
        {
            return !IsCompleted;
        }

        private void ExecuteCompleteModule(object? parameter)
        {
            courseViewModel.MarkModuleAsCompletedAndCheckRewards(CurrentModule.ModuleId, UserId);
            IsCompleted = true;
            OnPropertyChanged(nameof(IsCompleted));
            courseViewModel.RefreshCourseModulesDisplay(UserId);
        }

        public async Task ExecuteModuleImageClick(object? obj)
        {
            var success = await courseService.ClickModuleImageAsync(UserId, CurrentModule.ModuleId);
            if (success)
            {
                OnPropertyChanged(nameof(CoinBalance));
                courseViewModel.RefreshCourseModulesDisplay(UserId);
            }
        }

        private async Task EnsureUserExistsAsync(IUserService userService)
        {
            try
            {
                var user = await userService.GetUserById(UserId);
                if (user == null)
                {
                    Console.WriteLine($"User with ID {UserId} not found. Creating...");
                    //await userService.CreateUserAsync(new DuoClassLibrary.Models.User(UserId, $"DefaultUser{UserId}"));
                }
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine($"User with ID {UserId} not found (exception). Creating...");
                //await userService.CreateUserAsync(new DuoClassLibrary.Models.User(UserId, $"DefaultUser{UserId}"));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error in EnsureUserExistsAsync: {ex.Message}");
            }
        }
    }
}
