using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Duo.Views.Pages
{
    /// <summary>
    /// A page for managing sections.
    /// </summary>
    public sealed partial class ManageSectionsPage : Page
    {
        public ManageSectionsPage()
        {
            try
            {
                this.InitializeComponent();
                ViewModel.ShowErrorMessageRequested += ViewModel_ShowErrorMessageRequested;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Initialization error: {ex.Message}");
            }
        }

        private async void ViewModel_ShowErrorMessageRequested(object sender, (string Title, string Message) e)
        {
            try
            {
                await ShowErrorMessage(e.Title, e.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to show error dialog: {ex.Message}");
            }
        }

        private async Task ShowErrorMessage(string title, string message)
        {
            try
            {
                var dialog = new ContentDialog
                {
                    Title = title,
                    Content = message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ContentDialog error: {ex.Message}");
            }
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Frame.CanGoBack)
                {
                    this.Frame.GoBack();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BackButton_Click error: {ex.Message}");
            }
        }
    }
}
