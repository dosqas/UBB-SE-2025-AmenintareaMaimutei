using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Duo.ViewModels;
using DuoClassLibrary.Models;
using Duo.Services;
using Duo;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using DuoClassLibrary.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DuolingoNou.Views.Pages
{
    public sealed partial class AchievementsPage : Page
    {
        private ProfileViewModel _viewModel;
        private ProfileService _profileService;

        public AchievementsPage()
        {
            this.InitializeComponent();
            _viewModel = App.ServiceProvider.GetRequiredService<ProfileViewModel>();
            _profileService = App.ServiceProvider.GetRequiredService<ProfileService>();
            
            LoadUserStats();
            LoadUserAchievements();
        }

        private void LoadUserStats()
        {
            
                TotalXPText.Text = $"Total XP: ";
                BestStreakText.Text = $"Best Streak: ";
                QuizzesCompletedText.Text = $"Quizzes Completed: ";
                CoursesCompletedText.Text = $"Courses Completed: ";
            
        }

        private async Task LoadUserAchievements()
        {
            User currentUser = App.CurrentUser;
            
        }
    }
}