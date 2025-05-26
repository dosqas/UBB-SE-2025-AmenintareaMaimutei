using Microsoft.UI.Xaml.Controls;

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
    }
}
