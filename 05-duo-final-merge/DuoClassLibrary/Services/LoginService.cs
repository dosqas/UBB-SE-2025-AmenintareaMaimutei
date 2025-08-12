using System;
using System.Threading.Tasks;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services.Interfaces;

namespace DuoClassLibrary.Services
{
    /// <summary>
    /// Provides login-related functionality.
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly IUserHelperService _userHelperService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginService"/> class.
        /// </summary>
        /// <param name="userHelperService">The user helper service.</param>
        public LoginService(IUserHelperService userHelperService)
        {
            _userHelperService = userHelperService ?? throw new ArgumentNullException(nameof(userHelperService));
        }

        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if authentication is successful; otherwise, false.</returns>
        public async Task<bool> AuthenticateUser(string username, string password)
        {
            return await _userHelperService.ValidateCredentials(username, password);
        }

        /// <summary>
        /// Gets a user by their credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The user if credentials are valid; otherwise, null.</returns>
        public async Task<User> GetUserByCredentials(string username, string password)
        {
            var user = await _userHelperService.GetUserByCredentials(username, password);
            if (user != null)
            {
                user.OnlineStatus = true;
                await _userHelperService.UpdateUser(user);
            }
            return user;
        }

        /// <summary>
        /// Updates a user's status to offline when logging out.
        /// </summary>
        /// <param name="user">The user to update.</param>
        public async Task UpdateUserStatusOnLogout(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.OnlineStatus = false;
            user.LastActivityDate = DateTime.Now;
            await _userHelperService.UpdateUser(user);
        }
    }
} 