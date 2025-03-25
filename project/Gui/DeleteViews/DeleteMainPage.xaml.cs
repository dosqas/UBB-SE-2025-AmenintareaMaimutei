using Microsoft.UI.Xaml.Controls;

namespace Project.Gui.DeleteViews
{
    public sealed partial class DeleteMainPage : Page
    {
        public DeleteMainPage()
        {
            this.InitializeComponent();
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                switch (selectedItem.Tag)
                {
                    case "DeleteDoctors":
                        ContentFrame.Navigate(typeof(DeleteDoctorView));
                        break;
                    case "DeleteRooms":
                        ContentFrame.Navigate(typeof(DeleteRoomView));
                        break;
                    case "DeleteDepartments":
                        ContentFrame.Navigate(typeof(DeleteDepartmentView));
                        break;
                    case "DeleteSchedules":
                        ContentFrame.Navigate(typeof(DeleteScheduleView));
                        break;
                    case "DeleteDrugs":
                        ContentFrame.Navigate(typeof(DeleteDrugView));
                        break;
                    case "DeleteEquipments":
                        ContentFrame.Navigate(typeof(DeleteEquipmentView));
                        break;
                }
            }
        }
    }
}