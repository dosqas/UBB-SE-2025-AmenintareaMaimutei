namespace Project.Gui.AddViews
{
    using Microsoft.UI.Xaml.Controls;
    using Project.ViewModels.AddViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddShiftView : Page
    {
        private ShiftAddViewModel viewModel;

        public AddShiftView()
        {
            this.InitializeComponent();
            this.viewModel = new ShiftAddViewModel();
            this.DataContext = this.viewModel;
        }
    }
}
