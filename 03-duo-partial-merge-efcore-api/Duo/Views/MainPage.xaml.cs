using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Duo.Models;
using Duo.Services;
using Duo.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Duo.Views
{
    public sealed partial class MainPage : Page
    {
        private static bool isDialogShown = false;
        private int CurrentUserId { get; init; } = 1;

        public MainPage()
        {
            try
            {
                this.InitializeComponent();

                HttpClient httpClient = new HttpClient();

                var serviceProxy = new CoinsServiceProxy(httpClient);
                var courseServiceProxy = new CourseServiceProxy(httpClient);

                var courseService = new CourseService(courseServiceProxy);
                var coinsService = new CoinsService(serviceProxy);

                var vm = new MainViewModel(
                    serviceProxy,
                    courseServiceProxy,
                    CurrentUserId,
                    courseService,
                    coinsService);

                vm.ShowErrorMessageRequested += ViewModel_ShowErrorMessageRequested;

                this.DataContext = vm;

                CoursesListView.ItemClick += CoursesListView_ItemClick;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Initialization error: {ex.Message}");
            }
        }

        private async void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isDialogShown)
                {
                    isDialogShown = true;

                    bool dailyLoginRewardEligible = await (this.DataContext as MainViewModel) !.TryDailyLoginReward();

                    if (dailyLoginRewardEligible)
                    {
                        ContentDialog welcomeDialog = new ContentDialog
                        {
                            Title = "Welcome!",
                            Content = "You have been granted the daily login reward! 100 coins Just for you <3",
                            CloseButtonText = "Cheers!",
                            XamlRoot = RootGrid.XamlRoot
                        };
                        await welcomeDialog.ShowAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during RootGrid_Loaded: {ex.Message}");
            }
        }

        private async void CoursesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (e.ClickedItem is Course selectedCourse)
                {
                    var courseVM = new CourseViewModel(selectedCourse, CurrentUserId);
                    await courseVM.InitializeAsync(CurrentUserId);
                    this.Frame.Navigate(typeof(CoursePage), courseVM);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Course navigation error: {ex.Message}");
            }
        }

        private async void ViewModel_ShowErrorMessageRequested(object sender, (string Title, string Message) e)
        {
            try
            {
                await ShowErrorMessage(e.Title, e.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to show error dialog: {ex.Message}");
            }
        }

        private async Task ShowErrorMessage(string title, string message)
        {
            try
            {
                var dialog = new ContentDialog
                {
                    Title = title,
                    Content = message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error displaying ContentDialog: {ex.Message}");
            }
        }
    }
}
