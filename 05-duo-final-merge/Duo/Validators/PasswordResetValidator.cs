// <copyright file="PasswordResetValidator.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Validators
{
    /// <summary>
    /// Provides validation methods used during the password reset process.
    /// </summary>
    public class PasswordResetValidator
    {
        /// <summary>
        /// Validates whether the specified email address has a basic valid format.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns><c>true</c> if the email format is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) &&
                   email.Contains('@') &&
                   email.Contains('.');
        }

        /// <summary>
        /// Validates whether the verification code is non-empty.
        /// </summary>
        /// <param name="code">The verification code to validate.</param>
        /// <returns><c>true</c> if the code is not empty or whitespace; otherwise, <c>false</c>.</returns>
        public static bool IsValidVerificationCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code);
        }

        /// <summary>
        /// Validates that the password and its confirmation match exactly.
        /// </summary>
        /// <param name="password">The original password.</param>
        /// <param name="confirmPassword">The password confirmation input.</param>
        /// <returns><c>true</c> if both passwords match; otherwise, <c>false</c>.</returns>
        public static bool DoPasswordsMatch(string password, string confirmPassword)
        {
            return password == confirmPassword;
        }

        /// <summary>
        /// Validates whether the new password is non-empty.
        /// </summary>
        /// <param name="password">The new password to validate.</param>
        /// <returns><c>true</c> if the password is not empty or whitespace; otherwise, <c>false</c>.</returns>
        public static bool IsValidNewPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password);
        }
    }
}
