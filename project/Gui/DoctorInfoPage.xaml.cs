using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Project.Models;
using Project.ViewModels;

namespace Project.Gui
{
    public sealed partial class DoctorInfoPage : Page
    {
        private DoctorInformationViewModel _viewModel;

        public DoctorInfoPage()
        {
            this.InitializeComponent();
            _viewModel = new DoctorInformationViewModel();
            this.DataContext = _viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Doctor doctor)
            {
                int doctorID = doctor.DoctorID;
                _viewModel.LoadDoctorInformation(doctorID);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}