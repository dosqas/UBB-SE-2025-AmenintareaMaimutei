namespace Project
{
    using Microsoft.UI.Xaml;
    using Project.Gui;

    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            AdminMainPage = new AdminMainPage();
            AdminMainPage.Activate();
        }

        private AdminMainPage? AdminMainPage;
    }
}
