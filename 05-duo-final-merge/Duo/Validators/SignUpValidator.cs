// <copyright file="SignUpValidator.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Validators
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides validation methods used during the sign-up process.
    /// </summary>
    public partial class SignUpValidator
    {
        /// <summary>
        /// Validates whether the specified username is in a valid format.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <returns><c>true</c> if the username is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidUsername(string username)
        {
            return UsernameRegex().IsMatch(username);
        }

        /// <summary>
        /// Validates whether the specified password meets strong password criteria.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns><c>true</c> if the password is strong; otherwise, <c>false</c>.</returns>
        public static bool IsValidPassword(string password)
        {
            return StrongPasswordRegex().IsMatch(password);
        }

        /// <summary>
        /// Evaluates the strength of a given password.
        /// </summary>
        /// <param name="password">The password to evaluate.</param>
        /// <returns>A string representing the password strength: "Weak", "Medium", or "Strong".</returns>
        public static string GetPasswordStrength(string password)
        {
            if (password.Length < 6)
            {
                return "Weak";
            }

            if (StrongPasswordRegex().IsMatch(password))
            {
                return "Strong";
            }

            if (MediumPasswordRegex().IsMatch(password))
            {
                return "Medium";
            }

            return "Weak";
        }

        /// <summary>
        /// Checks if the two provided passwords match exactly.
        /// </summary>
        /// <param name="password">The original password.</param>
        /// <param name="confirmPassword">The confirmation password.</param>
        /// <returns><c>true</c> if the passwords match; otherwise, <c>false</c>.</returns>
        public static bool DoPasswordsMatch(string password, string confirmPassword)
        {
            return password == confirmPassword;
        }

        /// <summary>
        /// Gets a regex for validating usernames:
        /// 5–20 characters, alphanumeric or underscores.
        /// </summary>
        /// <returns>A compiled regex for username validation.</returns>
        [GeneratedRegex("^[A-Za-z0-9_]{5,20}$")]
        private static partial Regex UsernameRegex();

        /// <summary>
        /// Gets a regex for validating strong passwords:
        /// 6–15 characters, including uppercase letters, digits, and special characters.
        /// </summary>
        /// <returns>A compiled regex for strong password validation.</returns>
        [GeneratedRegex("^(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,15}$")]
        private static partial Regex StrongPasswordRegex();

        /// <summary>
        /// Gets a regex for evaluating medium password strength:
        /// 6–15 characters, must include at least one of: uppercase letter, digit, or special character.
        /// </summary>
        /// <returns>A compiled regex for medium strength password evaluation.</returns>
        [GeneratedRegex("^(?=.*[A-Z])|(?=.*\\d)|(?=.*[@$!%*?&]).{6,15}$")]
        private static partial Regex MediumPasswordRegex();
    }
}
