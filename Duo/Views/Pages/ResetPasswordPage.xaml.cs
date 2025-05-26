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
using Duo.ViewModels;
using Duo;
using Duo.Views.Pages;

using Microsoft.Extensions.DependencyInjection;
using Duo.Views;

namespace DuolingoNou.Views.Pages
{
    /// <summary>
    /// Interaction logic for ResetPasswordPage.xaml
    /// </summary>
    public partial class ResetPasswordPage : Page
    {
        private readonly ResetPassViewModel resetPassViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordPage"/> class
        /// </summary>
        public ResetPasswordPage()
        {
            InitializeComponent();
            
            // Get the ResetPassViewModel from DI container
            resetPassViewModel = App.ServiceProvider.GetRequiredService<ResetPassViewModel>();
            DataContext = resetPassViewModel;
            
            // Set up bindings for visibility
            EmailPanel.SetBinding(VisibilityProperty, new Binding()
            {
                Path = new PropertyPath("EmailPanelVisible"),
                Source = resetPassViewModel,
                Converter = (IValueConverter)Resources["BooleanToVisibilityConverter"]
            });
            
            CodePanel.SetBinding(VisibilityProperty, new Binding()
            {
                Path = new PropertyPath("CodePanelVisible"),
                Source = resetPassViewModel,
                Converter = (IValueConverter)Resources["BooleanToVisibilityConverter"]
            });
            
            PasswordPanel.SetBinding(VisibilityProperty, new Binding()
            {
                Path = new PropertyPath("PasswordPanelVisible"),
                Source = resetPassViewModel,
                Converter = (IValueConverter)Resources["BooleanToVisibilityConverter"]
            });
        }

        /// <summary>
        /// Handles the send verification code button click
        /// </summary>
        private async void OnSendCodeClick(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            
            // Disable button while processing
            var button = sender as Button;
            if (button != null)
                button.IsEnabled = false;
                
            await resetPassViewModel.SendVerificationCode(email);
            
            // Re-enable button if failed
            if (!resetPassViewModel.CodePanelVisible && button != null)
                button.IsEnabled = true;
        }

        /// <summary>
        /// Handles the verify code button click
        /// </summary>
        private void OnVerifyCodeClick(object sender, RoutedEventArgs e)
        {
            string code = CodeTextBox.Text;
            resetPassViewModel.VerifyCode(code);
        }

        /// <summary>
        /// Handles the reset password button click
        /// </summary>
        private async void OnResetPasswordClick(object sender, RoutedEventArgs e)
        {
            resetPassViewModel.NewPassword = NewPasswordBox.Password;
            resetPassViewModel.ConfirmPassword = ConfirmPasswordBox.Password;
            
            bool isReset = await resetPassViewModel.ResetPassword(resetPassViewModel.NewPassword);
            
            if (isReset)
            {
                // Navigate back to login page
                Frame.Navigate(typeof(LoginPage));
            }
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }
    }
}