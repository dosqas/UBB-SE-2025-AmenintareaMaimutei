namespace Project.Gui.AddViews
{
    using Microsoft.UI.Xaml.Controls;
    using Project.ViewModels.AddViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddEquipmentView : Page
    {
        private EquipmentAddViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEquipmentView"/> class.
        /// </summary>
        public AddEquipmentView()
        {
            this.InitializeComponent();
            this.viewModel = new EquipmentAddViewModel();
            this.DataContext = this.viewModel;
        }
    }
}
