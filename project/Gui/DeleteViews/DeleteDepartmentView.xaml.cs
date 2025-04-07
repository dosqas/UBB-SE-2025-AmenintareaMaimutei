namespace Project.Gui.DeleteViews
{
    using Microsoft.UI.Xaml.Controls;
    using Project.ViewModels.DeleteViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeleteDepartmentView : Page
    {
        private DepartmentDeleteViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDepartmentView"/> class.
        /// </summary>
        public DeleteDepartmentView()
        {
            this.InitializeComponent();
            this.viewModel = new DepartmentDeleteViewModel();
            this.DataContext = this.viewModel;
        }
    }
}
