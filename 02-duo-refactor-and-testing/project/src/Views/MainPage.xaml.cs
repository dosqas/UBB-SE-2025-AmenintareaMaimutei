using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Xaml.Controls;
using CourseApp.Models;
using CourseApp.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using CourseApp.Repository;
using CourseApp.Services;

namespace CourseApp.Views
{
    [ExcludeFromCodeCoverage]
    public sealed partial class MainPage : Page
    {
        // keep this static so that the dialog is only shown once. The page is recreated every time it is navigated to.
        private static bool isDialogShown = false;

        public MainPage()
        {
            this.InitializeComponent();
            var courseService = new CourseService();
            this.DataContext = new MainViewModel(
                courseService,
                new CoinsService(),
                courseService); // as ICourseFilterService
            CoursesListView.ItemClick += CoursesListView_ItemClick;
        }

        private async void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // Ensure the dialog is only shown once. Just in case.
            if (!isDialogShown)
            {
                isDialogShown = true;

                bool dailyLoginRewardEligible = (this.DataContext as MainViewModel) !.TryDailyLoginReward();

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

        private void CoursesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Course selectedCourse)
            {
                var mainWindow = MainWindow.Instance;
                if (mainWindow != null)
                {
                    var courseVM = mainWindow.GetOrCreateCourseViewModel(selectedCourse);
                    this.Frame.Navigate(typeof(CoursePage), courseVM);
                }
                else
                {
                    // Handle the case where mainWindow is null
                    // For example, you could log an error or show a message to the user
                }
            }
        }
    }
}
