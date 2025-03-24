using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Project.Gui.DeleteViews
{
    public sealed partial class DeleteMainPage : Page
    {
        public DeleteMainPage()
        {
            this.InitializeComponent();
        }

        private void DeleteDoctors_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DeleteDoctorView));
        }

        private void DeleteRooms_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DeleteRoomView));
        }

        private void DeleteDepartments_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DeleteDepartmentView));
        }

        private void DeleteSchedules_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DeleteScheduleView));
        }

        private void DeleteDrugs_Click(object sender, RoutedEventArgs e)
        {
           // Frame.Navigate(typeof(DeleteDrugView));
        }

        private void DeleteEquipments_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(DeleteEquipmentView));
        }
    }
}