using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Duo.Services;

using Duo.Validators;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services.Interfaces;

namespace Duo.ViewModels
{
    /// <summary>
    /// ViewModel for password reset functionality.
    /// </summary>
    public class ResetPassViewModel : INotifyPropertyChanged
    {
        private readonly ForgotPassService _forgotPassService;
        private readonly PasswordResetValidator _validator;
        private string _email = string.Empty;
        private string _verificationCode = string.Empty;
        private string _newPassword = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _statusMessage = string.Empty;
        private bool _isCodeVerified = false;
        private bool _isProcessing = false;
        private bool _emailPanelVisible = true;
        private bool _codePanelVisible = false;
        private bool _passwordPanelVisible = false;

        /// <summary>
        /// Event that is triggered when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the verification code.
        /// </summary>
        public string VerificationCode
        {
            get => _verificationCode;
            set
            {
                _verificationCode = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the confirmed password.
        /// </summary>
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether the code is verified.
        /// </summary>
        public bool IsCodeVerified
        {
            get => _isCodeVerified;
            private set
            {
                _isCodeVerified = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a process is running.
        /// </summary>
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the email panel is visible.
        /// </summary>
        public bool EmailPanelVisible
        {
            get => _emailPanelVisible;
            set
            {
                _emailPanelVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the code panel is visible.
        /// </summary>
        public bool CodePanelVisible
        {
            get => _codePanelVisible;
            set
            {
                _codePanelVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the password panel is visible.
        /// </summary>
        public bool PasswordPanelVisible
        {
            get => _passwordPanelVisible;
            set
            {
                _passwordPanelVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPassViewModel"/> class.
        /// </summary>
        /// <param name="userHelperService">The user helper service.</param>
        public ResetPassViewModel(IUserHelperService userHelperService)
        {
            if (userHelperService == null)
            {
                throw new ArgumentNullException(nameof(userHelperService));
            }
            
            _forgotPassService = new ForgotPassService(userHelperService);
            _validator = new PasswordResetValidator();
        }

        /// <summary>
        /// Validates the provided email address.
        /// </summary>
        /// <param name="email">The email to validate.</param>
        /// <returns>True if the email is valid; otherwise, false.</returns>
        public bool ValidateEmail(string email)
        {
            bool isValid = _validator.IsValidEmail(email);
            if (!isValid)
            {
                StatusMessage = "Please enter a valid email address.";
            }
            return isValid;
        }

        /// <summary>
        /// Sends a verification code to the specified email.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <returns>True if the code was sent successfully; otherwise, false.</returns>
        public async Task<bool> SendVerificationCode(string email)
        {
            if (!ValidateEmail(email))
            {
                return false;
            }

            Email = email;
            IsProcessing = true;
            StatusMessage = "Sending verification code...";

            try
            {
                bool isCodeSent = await _forgotPassService.SendVerificationCode(email);

                if (isCodeSent)
                {
                    StatusMessage = "Verification code sent. Please check your email.";
                    EmailPanelVisible = false;
                    CodePanelVisible = true;
                }
                else
                {
                    StatusMessage = "Failed to send verification code. Please try again.";
                }

                return isCodeSent;
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
                return false;
            }
            finally
            {
                IsProcessing = false;
            }
        }

        /// <summary>
        /// Validates the verification code format.
        /// </summary>
        /// <param name="code">The code to validate.</param>
        /// <returns>True if the code is valid; otherwise, false.</returns>
        public bool ValidateCodeFormat(string code)
        {
            bool isValid = _validator.IsValidVerificationCode(code);
            if (!isValid)
            {
                StatusMessage = "Please enter the verification code.";
            }
            return isValid;
        }

        /// <summary>
        /// Verifies the specified code.
        /// </summary>
        /// <param name="code">The verification code.</param>
        /// <returns>True if the code is valid; otherwise, false.</returns>
        public async Task<bool> VerifyCode(string code)
        {
            if (!ValidateCodeFormat(code))
            {
                return false;
            }

            VerificationCode = code;
            IsCodeVerified = _forgotPassService.VerifyCode(code);

            if (IsCodeVerified)
            {
                StatusMessage = "Code verified. Please enter your new password.";
                CodePanelVisible = false;
                PasswordPanelVisible = true;
            }
            else
            {
                StatusMessage = "Invalid verification code. Please try again.";
            }

            return IsCodeVerified;
        }

        /// <summary>
        /// Validates if the passwords match.
        /// </summary>
        /// <returns>True if the passwords match; otherwise, false.</returns>
        public bool ValidatePasswordsMatch()
        {
            bool match = _validator.DoPasswordsMatch(NewPassword, ConfirmPassword);
            if (!match)
            {
                StatusMessage = "Passwords don't match!";
            }
            return match;
        }

        /// <summary>
        /// Validates if the new password is valid.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>True if the password is valid; otherwise, false.</returns>
        public bool ValidateNewPassword(string password)
        {
            bool isValid = _validator.IsValidNewPassword(password);
            if (!isValid)
            {
                StatusMessage = "Please enter a valid password.";
            }
            return isValid;
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="newPassword">The new password.</param>
        /// <returns>True if the password was reset successfully; otherwise, false.</returns>
        public async Task<bool> ResetPassword(string newPassword)
        {
            if (!ValidateNewPassword(newPassword))
            {
                return false;
            }

            if (!ValidatePasswordsMatch())
            {
                return false;
            }

            try
            {
                bool isReset = await _forgotPassService.ResetPassword(Email, newPassword);

                if (isReset)
                {
                    StatusMessage = "Password reset successfully!";
                }
                else
                {
                    StatusMessage = "Failed to reset password. Please try again.";
                }

                return isReset;
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for a property
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}