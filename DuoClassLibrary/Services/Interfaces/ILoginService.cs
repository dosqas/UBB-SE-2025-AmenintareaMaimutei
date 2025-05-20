using System;
using System.Threading.Tasks;
using DuoClassLibrary.Models;

namespace DuoClassLibrary.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for login-related operations.
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if authentication is successful; otherwise, false.</returns>
        Task<bool> AuthenticateUser(string username, string password);
        
        /// <summary>
        /// Gets a user by their credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The user if credentials are valid; otherwise, null.</returns>
        Task<User> GetUserByCredentials(string username, string password);
        
        /// <summary>
        /// Updates a user's status to offline when logging out.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when user is null.</exception>
        Task UpdateUserStatusOnLogout(User user);
    }
} 