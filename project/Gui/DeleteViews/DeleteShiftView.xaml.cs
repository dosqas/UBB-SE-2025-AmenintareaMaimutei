namespace Project.Gui.DeleteViews
{
    using Microsoft.UI.Xaml.Controls;
    using Project.ViewModels.DeleteViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeleteShiftView : Page
    {
        private ShiftDeleteViewModel viewmodel;

        public DeleteShiftView()
        {
            this.InitializeComponent();
            this.viewmodel = new ShiftDeleteViewModel();
            this.DataContext = this.viewmodel;
        }
    }
}
