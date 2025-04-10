namespace Project.Gui.AddViews
{
    using Microsoft.UI.Xaml.Controls;
    using Project.ViewModels.AddViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddDepartmentView : Page
    {
        private DepartmentAddViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddDepartmentView"/> class.
        /// </summary>
        public AddDepartmentView()
        {
            this.InitializeComponent();
            this.viewModel = new DepartmentAddViewModel();
            this.DataContext = this.viewModel;
        }
    }
}
