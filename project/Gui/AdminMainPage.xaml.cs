namespace Project.Gui
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Project.Gui.DeleteViews;
    using Project.Gui.ModifyViews;

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminMainPage : Window
    {
        /// <summary>
        /// Initializes a new instance of the  <see cref="AdminMainPage"/> class.
        /// </summary>
        public AdminMainPage()
        {
            this.InitializeComponent();
            Instance = this;
            this.ContentFrame.Navigate(typeof(HomePage));
        }

        /// <summary>
        /// Gets and Sets the Instance.
        /// </summary>
        public static AdminMainPage? Instance { get; private set; }

        /// <summary>
        /// Gets the Frame.
        /// </summary>
        public Frame Frame => this.ContentFrame;

        /// <summary>
        /// Gets the content of the frame.
        /// </summary>
        /// <returns>Frame.</returns>
        public Frame GetContentFrame()
        {
            return this.ContentFrame;
        }

        private void NavigationView_SelectionChanged(global::Microsoft.UI.Xaml.Controls.NavigationView sender, global::Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                string? invokedItemName = args.SelectedItemContainer.Tag.ToString();
                switch (invokedItemName)
                {
                    case "HomePage":
                        this.ContentFrame.Navigate(typeof(HomePage));
                        break;
                    case "Add":
                        this.ContentFrame.Navigate(typeof(AddPage));
                        break;
                    case "Modify":
                        this.ContentFrame.Navigate(typeof(ModifyPage));
                        break;
                    case "LogOut":
                        var loginPage = new LoginPage();
                        loginPage.Activate();
                        this.Close();
                        break;
                    case "Delete":
                        this.ContentFrame.Navigate(typeof(DeleteMainPage));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
