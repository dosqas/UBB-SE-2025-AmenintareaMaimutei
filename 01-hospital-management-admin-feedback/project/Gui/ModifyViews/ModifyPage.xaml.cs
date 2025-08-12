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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Project.Gui.ModifyViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModifyPage : Page
    {
        public ModifyPage()
        {
            this.InitializeComponent();
            ModifyPageFrame.Navigate(typeof(ModifyDoctorView));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                string selectedTag = selectedItem.Tag.ToString();

                if (selectedTag == "Doctors")
                {
                    ModifyPageFrame.Navigate(typeof(ModifyDoctorView));
                }
                else if (selectedTag == "Equipment")
                {
                    ModifyPageFrame.Navigate(typeof(ModifyEquipmentView));
                }
                else if (selectedTag == "Drugs")
                {
                    ModifyPageFrame.Navigate(typeof(ModifyDrugView));
                }
                else if (selectedTag == "Schedule")
                {
                    ModifyPageFrame.Navigate(typeof(ModifyScheduleView));
                }
                else if (selectedTag == "Shifts")
                {
                    ModifyPageFrame.Navigate(typeof(ModifyShiftView));
                }
                else if (selectedTag == "Departments")
                {
                    ModifyPageFrame.Navigate(typeof(ModifyDepartmentView));
                }
                else if (selectedTag == "Rooms")
                {
                    ModifyPageFrame.Navigate(typeof(ModifyRoomView));
                }
            }
        }
    }
}
