namespace Project.Gui.AddViews
{
    using Microsoft.UI.Xaml.Controls;
    using Project.ViewModels.AddViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddScheduleView : Page
    {
        private ScheduleAddViewModel viewModel;

        public AddScheduleView()
        {
            this.InitializeComponent();
            this.viewModel = new ScheduleAddViewModel();
            this.DataContext = this.viewModel;
        }
    }
}
