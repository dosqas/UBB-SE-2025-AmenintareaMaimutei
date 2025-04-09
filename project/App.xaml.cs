namespace Project
{
    using Microsoft.UI.Xaml;
    using Project.Gui;

    public partial class App : Application
    {
        private AdminMainPage? adminMainPage;

        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            this.adminMainPage = new AdminMainPage();
            this.adminMainPage.Activate();
        }


    }
}
