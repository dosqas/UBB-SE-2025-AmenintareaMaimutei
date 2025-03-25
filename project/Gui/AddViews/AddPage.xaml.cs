using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Project.Gui.ModifyViews;
using Project.Gui.AddViews;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Project.Gui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddPage : Page
    {
        public AddPage()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(AddDoctorView));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                string invokedItemName = args.SelectedItemContainer.Tag.ToString();
                switch (invokedItemName)
                {
                    case "Doctors":
                        ContentFrame.Navigate(typeof(AddDoctorView));
                        break;
                    case "Departments":
                        ContentFrame.Navigate(typeof(AddDepartmentView));
                        break;
                    case "Drugs":
                        ContentFrame.Navigate(typeof(AddDrugView));
                        break;
                    case "Equipments":
                        ContentFrame.Navigate(typeof(AddEquipmentView));
                        break;
                    case "Rooms":
                        ContentFrame.Navigate(typeof(AddRoomView));
                        break;
                    case "Schedules":
                        ContentFrame.Navigate(typeof(AddScheduleView));
                        break;
                    case "Shifts":
                        ContentFrame.Navigate(typeof(AddShiftView));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
