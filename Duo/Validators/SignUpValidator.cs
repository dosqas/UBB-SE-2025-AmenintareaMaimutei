using System.Text.RegularExpressions;

namespace Duo.Validators
{
    /// <summary>
    /// Provides validation methods for the sign-up process
    /// </summary>
    public class SignUpValidator
    {
        /// <summary>
        /// Validates if the username follows the required format (5-20 alphanumeric characters or underscore)
        /// </summary>
        /// <param name="username">The username to validate</param>
        /// <returns>True if the username is valid; otherwise, false</returns>
        public bool IsValidUsername(string username)
        {
            return Regex.IsMatch(username, "^[A-Za-z0-9_]{5,20}$");
        }

        /// <summary>
        /// Validates if the password meets the strong password requirements
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns>True if the password is valid; otherwise, false</returns>
        public bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, "^(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,15}$");
        }

        /// <summary>
        /// Determines the strength of a password
        /// </summary>
        /// <param name="password">The password to evaluate</param>
        /// <returns>A string describing the password strength: "Weak", "Medium", or "Strong"</returns>
        public string GetPasswordStrength(string password)
        {
            if (password.Length < 6) return "Weak";
            if (Regex.IsMatch(password, "^(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&]).{6,15}$")) return "Strong";
            if (Regex.IsMatch(password, "^(?=.*[A-Z])|(?=.*\\d)|(?=.*[@$!%*?&]).{6,15}$")) return "Medium";
            return "Weak";
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
    }
} 