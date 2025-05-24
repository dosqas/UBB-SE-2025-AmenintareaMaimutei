using System;
using Duo.Services;
using DuoClassLibrary.Models;
using System.Threading.Tasks;

using Duo.Validators;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DuoClassLibrary.Services;

namespace Duo.ViewModels
{
    /// <summary>
    /// ViewModel for handling sign-up operations and validation
    /// </summary>
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private readonly SignUpService signUpService;
        private readonly SignUpValidator validator;
        private User newUser = new User();
        private string confirmPassword = string.Empty;
        private string passwordStrength = string.Empty;
        private string usernameValidationMessage = string.Empty;
        private string passwordValidationMessage = string.Empty;
        private string confirmPasswordValidationMessage = string.Empty;

        /// <summary>
        /// Event that is triggered when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpViewModel"/> class
        /// </summary>
        /// <param name="signUpService">The sign-up service</param>
        public SignUpViewModel(SignUpService signUpService)
        {
            this.signUpService = signUpService ?? throw new ArgumentNullException(nameof(signUpService));
            this.validator = new SignUpValidator();
        }

        /// <summary>
        /// Gets or sets the new user being created
        /// </summary>
        public User NewUser
        {
            get => newUser;
            set
            {
                newUser = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the password confirmation
        /// </summary>
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                confirmPassword = value;
                ValidatePasswordMatch();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the password strength indicator
        /// </summary>
        public string PasswordStrength
        {
            get => passwordStrength;
            set
            {
                passwordStrength = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the username validation message
        /// </summary>
        public string UsernameValidationMessage
        {
            get => usernameValidationMessage;
            set
            {
                usernameValidationMessage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the password validation message
        /// </summary>
        public string PasswordValidationMessage
        {
            get => passwordValidationMessage;
            set
            {
                passwordValidationMessage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the confirm password validation message
        /// </summary>
        public string ConfirmPasswordValidationMessage
        {
            get => confirmPasswordValidationMessage;
            set
            {
                confirmPasswordValidationMessage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Checks if a username is already taken
        /// </summary>
        /// <param name="username">The username to check</param>
        /// <returns>True if the username is taken; otherwise, false</returns>
        public async Task<bool> IsUsernameTaken(string username)
        {
            try
            {
                return await signUpService.IsUsernameTaken(username);
            }
            catch (Exception checkUsernameException)
            {
                Console.WriteLine($"Error checking username: {checkUsernameException.Message}");
                return true; // Fail-safe to prevent duplicate usernames if there's an error
            }
        }

        /// <summary>
        /// Creates a new user with the provided information
        /// </summary>
        /// <param name="user">The user information</param>
        /// <returns>True if the user was created successfully; otherwise, false</returns>
        public async Task<bool> CreateNewUser(User user)
        {
            try
            {
                user.DateJoined = DateTime.Now;
                return await signUpService.RegisterUser(user);
            }
            catch (Exception createUserException)
            {
                Console.WriteLine($"Error creating user: {createUserException.Message}");
                return false;
            }
        }

        /// <summary>
        /// Validates if the username follows the required format
        /// </summary>
        /// <param name="username">The username to validate</param>
        /// <returns>True if the username is valid; otherwise, false</returns>
        public bool ValidateUsername(string username)
        {
            bool isValid = validator.IsValidUsername(username);
            UsernameValidationMessage = isValid ? string.Empty : "Username must be 5-20 characters and contain only letters, digits, or underscores.";
            return isValid;
        }

        /// <summary>
        /// Updates the password strength indicator based on the provided password
        /// </summary>
        /// <param name="password">The password to evaluate</param>
        public void UpdatePasswordStrength(string password)
        {
            PasswordStrength = validator.GetPasswordStrength(password);

            if (PasswordStrength == "Weak")
            {
                PasswordValidationMessage = "Password must include uppercase, digit, and special character.";
            }
            else
            {
                PasswordValidationMessage = string.Empty;
            }
        }

        /// <summary>
        /// Validates if the password is strong enough
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns>True if the password is medium or strong; otherwise, false</returns>
        public bool ValidatePasswordStrength(string password)
        {
            string strength = validator.GetPasswordStrength(password);
            return strength != "Weak";
        }

        /// <summary>
        /// Validates if the passwords match
        /// </summary>
        /// <returns>True if the passwords match; otherwise, false</returns>
        public bool ValidatePasswordMatch()
        {
            bool match = validator.DoPasswordsMatch(NewUser.Password, ConfirmPassword);
            ConfirmPasswordValidationMessage = match ? string.Empty : "Passwords do not match.";
            return match;
        }

        /// <summary>
        /// Validates all sign-up information
        /// </summary>
        /// <returns>True if all validations pass; otherwise, false</returns>
        public bool ValidateAll()
        {
            bool usernameValid = ValidateUsername(NewUser.UserName);
            bool passwordStrengthValid = ValidatePasswordStrength(NewUser.Password);
            bool passwordsMatch = ValidatePasswordMatch();
            
            return usernameValid && passwordStrengthValid && passwordsMatch;
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