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
using Project.Gui;
using System.Diagnostics;
using Project.Gui.ModifyViews;
using Project.Gui.AddViews;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Project.Gui
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminMainPage : Window
    {
        public static AdminMainPage Instance { get; private set; }
        public Frame Frame => ContentFrame;

        public AdminMainPage()
        {
            this.InitializeComponent();
            Instance = this;
            ContentFrame.Navigate(typeof(HomePage));
        }

        public Frame getContentFrame()
        {
            return ContentFrame;
        }

        private void NavigationView_SelectionChanged(global::Microsoft.UI.Xaml.Controls.NavigationView sender, global::Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                string invokedItemName = args.SelectedItemContainer.Tag.ToString();
                switch (invokedItemName)
                {
                    case "HomePage":
                        ContentFrame.Navigate(typeof(HomePage));
                        break;
                    case "Add":
                        ContentFrame.Navigate(typeof(AddPage));
                        break;
                    case "Modify":
                        ContentFrame.Navigate(typeof(ModifyRoomView));
                        break;
                    case "LogOut":
                        var loginPage = new LoginPage();
                        loginPage.Activate();
                        this.Close();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
