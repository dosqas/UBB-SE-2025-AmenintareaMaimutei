
using DuoClassLibrary.Models;
using DuoClassLibrary.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Duo.ViewModels
{
    /// <summary>
    /// ViewModel that handles the login logic.
    /// </summary>
    public class LoginViewModel
    {
        private readonly ILoginService loginService;

        /// <summary>
        /// Gets or sets the username entered by the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the password entered by the user.
        /// </summary>
        public string Password { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets a value indicating whether the login was successful.
        /// </summary>
        public bool LoginStatus { get; private set; }
        
        /// <summary>
        /// Gets the logged-in user after a successful login.
        /// </summary>
        public User LoggedInUser { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        /// <param name="loginService">The login service.</param>
        public LoginViewModel(ILoginService loginService)
        {
            this.loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        /// <summary>
        /// Attempts to log in with the provided credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<bool> AttemptLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                LoginStatus = false;
                return false;
            }

            Username = username;
            Password = password;

            try
            {
                // Try to get the user
                LoggedInUser = await loginService.GetUserByCredentials(Username, Password);
                LoginStatus = LoggedInUser != null;
                return LoginStatus;
            }
            catch (Exception)
            {
                LoginStatus = false;
                return false;
            }
        }
    }
}
