using Duo;

using Duo.Services;
using Duo.ViewModels;
using DuolingoNou.Views;
using DuolingoNou.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DuoClassLibrary.Services.Interfaces;

namespace Duo.Views.Pages
{
    /// <summary>
    /// Login page for user authentication.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        /// <summary>
        /// Gets the ViewModel for this page.
        /// </summary>
        public LoginViewModel ViewModel { get; private set; }

        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();

            // Get services from DI container
            var loginService = App.ServiceProvider.GetRequiredService<ILoginService>();
            _userService = App.ServiceProvider.GetRequiredService<IUserService>();

            // Create ViewModel with injected service
            ViewModel = new LoginViewModel(loginService);

            this.DataContext = ViewModel;
        }

        /// <summary>
        /// Handles the password reveal mode checkbox change.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void RevealModeCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            PasswordBoxWithRevealMode.PasswordRevealMode =
                RevealModeCheckBox.IsChecked == true ? PasswordRevealMode.Visible : PasswordRevealMode.Hidden;
        }

        /// <summary>
        /// Navigates to the sign-up page.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void NavigateToSignUpPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUpPage));
        }

        /// <summary>
        /// Handles the login button click.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBoxWithRevealMode.Password;

            bool loginSuccess = await ViewModel.AttemptLogin(username, password);

            if (loginSuccess)
            {
                App.CurrentUser = ViewModel.LoggedInUser;
                LoginStatusMessage.Text = "You have successfully logged in!";
                LoginStatusMessage.Visibility = Visibility.Visible;
                
                // Set the current user in the user service
                await _userService.SetUser(ViewModel.LoggedInUser.UserName);
                await App.userService.SetUser(ViewModel.LoggedInUser.UserName);

                Frame.Navigate(typeof(CategoryPage));
            }
            else
            {
                LoginStatusMessage.Text = "Invalid username or password.";
                LoginStatusMessage.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Navigates to the reset password page.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnForgotPasswordClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ResetPasswordPage));
        }

        /// <summary>
        /// Shows a content dialog with the given title and message.
        /// </summary>
        /// <param name="title">The dialog title.</param>
        /// <param name="content">The dialog content.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ShowDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}