using System;
using System.Threading.Tasks;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services.Interfaces;
using DuoClassLibrary.Constants;

namespace Duo.Services
{
    /// <summary>
    /// Service for handling password reset functionality.
    /// </summary>
    public class ForgotPassService
    {
        private readonly IUserHelperService _userHelperService;
        private string _verificationCode;
        private string _userEmail;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPassService"/> class.
        /// </summary>
        /// <param name="userHelperService">The user helper service.</param>
        public ForgotPassService(IUserHelperService userHelperService)
        {
            _userHelperService = userHelperService ?? throw new ArgumentNullException(nameof(userHelperService));
            _verificationCode = string.Empty;
            _userEmail = string.Empty;
        }

        /// <summary>
        /// Sends a verification code to the user's email.
        /// </summary>
        /// <param name="email">The email address to send the code to.</param>
        /// <returns>True if the email was sent successfully; otherwise, false.</returns>
        public async Task<bool> SendVerificationCode(string email)
        {
            var user = await _userHelperService.GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }

            _userEmail = email;
            
            // Generate a random 6-digit code
            Random randomNumberGenerator = new Random();
            _verificationCode = randomNumberGenerator.Next(
                VerificationConstants.MinimumVerificationCodeValue, 
                VerificationConstants.MaximumVerificationCodeValue
            ).ToString();

            // In a real app, send this code via email
            // For now, just simulate an API call
            await Task.Delay(VerificationConstants.VerificationCodeSendingDelayMilliseconds);

            return true;
        }

        /// <summary>
        /// Verifies the code entered by the user.
        /// </summary>
        /// <param name="code">The verification code to check.</param>
        /// <returns>True if the code is valid; otherwise, false.</returns>
        public bool VerifyCode(string code)
        {
            return code == _verificationCode;
        }

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>True if the password was reset successfully; otherwise, false.</returns>
        public async Task<bool> ResetPassword(string email, string newPassword)
        {
            var user = await _userHelperService.GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }

            user.Password = newPassword;
            await _userHelperService.UpdateUser(user);
            return true;
        }
    }
}