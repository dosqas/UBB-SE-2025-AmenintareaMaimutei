using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Duo.ViewModels;
using DuoClassLibrary.Models;
using System;
using System.Threading.Tasks;
using DuolingoNou.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Duo.Views.Pages
{
    /// <summary>
    /// Page for user sign-up functionality
    /// </summary>
    public sealed partial class SignUpPage : Page
    {
        /// <summary>
        /// Gets the ViewModel for this page
        /// </summary>
        public SignUpViewModel ViewModel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpPage"/> class
        /// </summary>
        public SignUpPage()
        {
            this.InitializeComponent();
            ViewModel = App.ServiceProvider.GetRequiredService<SignUpViewModel>();
            this.DataContext = ViewModel;
        }

        /// <summary>
        /// Handles the create user button click event
        /// </summary>
        private async void OnCreateUserClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NewUser.UserName = UsernameTextBox.Text;
            ViewModel.NewUser.Email = EmailTextBox.Text;
            ViewModel.NewUser.Password = PasswordBoxWithRevealMode.Password;
            ViewModel.ConfirmPassword = ConfirmPasswordBox.Password;

            // Validate username format
            if (!ViewModel.ValidateUsername(ViewModel.NewUser.UserName))
            {
                UsernameValidationTextBlock.Text = ViewModel.UsernameValidationMessage;
                return;
            }

            // Check if username is already taken
            if (await ViewModel.IsUsernameTaken(ViewModel.NewUser.UserName))
            {
                await ShowDialog("Username Taken", "This username is already in use. Please choose another.");
                return;
            }

            // Update and validate password strength
            ViewModel.UpdatePasswordStrength(ViewModel.NewUser.Password);
            if (ViewModel.PasswordStrength == "Weak")
            {
                await ShowDialog("Weak Password", "Password must be at least Medium strength. Include an uppercase letter, a special character, and a digit.");
                return;
            }

            // Validate password match
            if (!ViewModel.ValidatePasswordMatch())
            {
                ConfirmPasswordValidationTextBlock.Text = ViewModel.ConfirmPasswordValidationMessage;
                return;
            }

            // Create the user
            bool success = await ViewModel.CreateNewUser(ViewModel.NewUser);
            
            if (success)
            {
                // Set the current user globally
                App.CurrentUser = ViewModel.NewUser;

                await ShowDialog("Account Created", "Your account has been successfully created!");
                await App.userService.SetUser(App.CurrentUser.UserName);

                Frame.Navigate(typeof(CategoryPage));
            }
            else
            {
                await ShowDialog("Error", "There was a problem creating your account. Please try again.");
            }
        }

        /// <summary>
        /// Displays a dialog with the specified title and message
        /// </summary>
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

        /// <summary>
        /// Handles the password changes and updates the strength indicator
        /// </summary>
        private void PasswordBoxWithRevealMode_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdatePasswordStrength(PasswordBoxWithRevealMode.Password);
            PasswordStrengthTextBlock.Text = ViewModel.PasswordStrength;
        }

        /// <summary>
        /// Handles the reveal mode checkbox changes
        /// </summary>
        private void RevealModeCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            PasswordBoxWithRevealMode.PasswordRevealMode = RevealModeCheckBox.IsChecked == true ? PasswordRevealMode.Visible : PasswordRevealMode.Hidden;
            ConfirmPasswordBox.PasswordRevealMode = RevealModeCheckBox.IsChecked == true ? PasswordRevealMode.Visible : PasswordRevealMode.Hidden;
        }

        /// <summary>
        /// Navigates to the login page
        /// </summary>
        private void NavigateToLoginPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }
    }
}