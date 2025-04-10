namespace Project.Gui
{
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage"/> class.
        /// </summary>
        public HomePage()
        {
            this.InitializeComponent();
            this.HomePageFrame.Navigate(typeof(DoctorsPage));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                string? selectedTag = selectedItem.Tag.ToString();

                if (selectedTag == "Doctors")
                {
                    this.HomePageFrame.Navigate(typeof(DoctorsPage));
                }
                else if (selectedTag == "Equipment")
                {
                    this.HomePageFrame.Navigate(typeof(EquipmentPage));
                }
                else if (selectedTag == "Rooms")
                {
                    this.HomePageFrame.Navigate(typeof(RoomAndDepartments));
                }
                else if (selectedTag == "Schedule")
                {
                    this.HomePageFrame.Navigate(typeof(ScheduleAndShifts));
                }
                else if (selectedTag == "DrugPage")
                {
                    this.HomePageFrame.Navigate(typeof(Drugs));
                }
            }
        }
    }
}
