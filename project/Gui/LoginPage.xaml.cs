using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Project.Gui
{
    public sealed partial class LoginPage : global::Microsoft.UI.Xaml.Window
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the main page
            // Navigate back to the main page
            var adminMainPage = new AdminMainPage();
            adminMainPage.Activate();
            this.Close();
        }
    }
}