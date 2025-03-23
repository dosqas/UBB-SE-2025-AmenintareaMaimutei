using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Project.Models;

namespace Project.Gui
{
    public sealed partial class DoctorInfoPage : Page
    {
        public DoctorInfoPage()
        {
            this.InitializeComponent();
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
            // Implement your salary computation logic here
            return doctor.Experience * 1000; // Example computation
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