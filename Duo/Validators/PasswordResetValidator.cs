using System;

namespace Duo.Validators
{
    /// <summary>
    /// Provides validation methods for the password reset process
    /// </summary>
    public class PasswordResetValidator
    {
        /// <summary>
        /// Validates if the email is in a valid format
        /// </summary>
        /// <param name="email">The email to validate</param>
        /// <returns>True if the email is valid; otherwise, false</returns>
        public bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && 
                   email.Contains("@") && 
                   email.Contains(".");
        }

        /// <summary>
        /// Validates if the verification code is not empty
        /// </summary>
        /// <param name="code">The verification code to validate</param>
        /// <returns>True if the code is not empty; otherwise, false</returns>
        public bool IsValidVerificationCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code);
        }

        /// <summary>
        /// Validates if the passwords match
        /// </summary>
        /// <param name="password">The password</param>
        /// <param name="confirmPassword">The password confirmation</param>
        /// <returns>True if the passwords match; otherwise, false</returns>
        public bool DoPasswordsMatch(string password, string confirmPassword)
        {
            return password == confirmPassword;
        }

        /// <summary>
        /// Validates if the new password is not empty
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns>True if the password is not empty; otherwise, false</returns>
        public bool IsValidNewPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password);
        }
    }
} 