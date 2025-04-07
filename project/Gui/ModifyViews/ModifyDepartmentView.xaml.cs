namespace Project.Gui.ModifyViews
{
    using Microsoft.UI.Xaml.Controls;
    using Project.ViewModels.UpdateViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModifyDepartmentView : Page
    {
        private DepartmentUpdateViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyDepartmentView"/> class.
        /// </summary>
        public ModifyDepartmentView()
        {
            this.InitializeComponent();
            this.viewModel = new DepartmentUpdateViewModel();
            this.DataContext = this.viewModel;
        }
    }
}