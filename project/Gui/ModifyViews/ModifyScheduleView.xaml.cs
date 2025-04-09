namespace Project.Gui.ModifyViews
{
    using Microsoft.UI.Xaml.Controls;
    using Project.ViewModels.UpdateViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModifyScheduleView : Page
    {
        private ScheduleUpdateViewModel viewModel;

        public ModifyScheduleView()
        {
            this.InitializeComponent();
            this.viewModel = new ScheduleUpdateViewModel();
            this.DataContext = this.viewModel;
        }
    }
}
