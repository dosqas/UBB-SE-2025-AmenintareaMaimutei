namespace Project.Gui
{
    using Microsoft.UI.Xaml;

    /// <summary>
    /// LoginPage class.
    /// </summary>
    public sealed partial class LoginPage : global::Microsoft.UI.Xaml.Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the main page
            var adminMainPage = new AdminMainPage();
            adminMainPage.Activate();
            this.Close();
        }
    }
}