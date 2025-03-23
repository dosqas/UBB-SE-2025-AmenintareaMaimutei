using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Project.Models;
using Project.ClassModels; // Add this line to include the DoctorModel class

namespace Project.Gui
{
    public sealed partial class DoctorInfoPage : Page
    {
        private DoctorModel _doctorModel; // Add a field for DoctorModel

        public DoctorInfoPage()
        {
            this.InitializeComponent();
            _doctorModel = new DoctorModel(); // Initialize the DoctorModel
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Doctor doctor)
            {
                DoctorIDTextBlock.Text = doctor.DoctorID.ToString();
                DepartmentIDTextBlock.Text = doctor.DepartmentID.ToString();
                LicenseNumberTextBlock.Text = doctor.LicenseNumber;
                ExperienceTextBlock.Text = doctor.Experience.ToString();
                ComputedSalaryTextBlock.Text = ComputeSalary(doctor).ToString("C");
            }
        }

        private double ComputeSalary(Doctor doctor)
        {
            // Use the DoctorModel to compute the salary
            return _doctorModel.ComputeDoctorSalary(doctor.DoctorID);
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