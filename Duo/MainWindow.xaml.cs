using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Duo.Views.Pages;
using Duo.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using DuoClassLibrary.Services.Interfaces;
using Microsoft.UI;
using Windows.ApplicationModel;
using System.IO;

namespace Duo
{
    /// <summary>
    /// The main window of the application.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly ILoginService loginService;

        public MainWindow()
        {
            this.InitializeComponent();

            // Get the login service from the dependency injection container
            loginService = App.ServiceProvider.GetRequiredService<ILoginService>();

            // Navigate to the login page
            MainFrame.Navigate(typeof(LoginPage));

            // Handle the Closed event
            this.Closed += MainWindow_Closed;

            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            var iconPath = Path.Combine(Package.Current.InstalledLocation.Path, "Assets/icon.ico");
            appWindow.SetIcon(iconPath);
        }

        private void MainWindow_Closed(object sender, WindowEventArgs e)
        {
            if (App.CurrentUser != null)
            {
                // Update the user's status to offline
                loginService.UpdateUserStatusOnLogout(App.CurrentUser);

                // Write diagnostic information
                System.Diagnostics.Debug.WriteLine($"{App.CurrentUser.UserName} (ID: {App.CurrentUser.UserId}) has logged out.");
            }
        }
    }
}