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

namespace Project.Gui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            HomePageFrame.Navigate(typeof(DoctorsPage));

        }
        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                // Navigate based on the selected menu item tag
                string selectedTag = selectedItem.Tag.ToString();

                if (selectedTag == "Doctors")
                {
                    HomePageFrame.Navigate(typeof(DoctorsPage));
                }
                else if (selectedTag == "Equipment")
                {
                    HomePageFrame.Navigate(typeof(EquipmentPage)); // Assuming you have this page
                }
                else if (selectedTag == "Rooms")
                {
                    HomePageFrame.Navigate(typeof(TestPage)); // Assuming you have this page
                }
            }
        }
    }
}
