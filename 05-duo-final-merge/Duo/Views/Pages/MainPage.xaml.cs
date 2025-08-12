using Duo.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace Duo.Views.Pages
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var userName = App.CurrentUser?.UserName ?? "User";
            WelcomeTextBlock.Text = $"Welcome, {userName}!";
        }

        /// <summary>
        /// Logout button handler
        /// </summary>
        public void LogoutButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ((MainPageViewModel)App.ServiceProvider.GetService(typeof(MainPageViewModel))).HandleLogoutClick();

            Frame loginFrame = new Frame();
            loginFrame.Navigate(typeof(LoginPage));
            App.MainAppWindow.Content = loginFrame;
        }
    }
}
