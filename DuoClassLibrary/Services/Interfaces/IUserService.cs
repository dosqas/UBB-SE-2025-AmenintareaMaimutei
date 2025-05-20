
using DuoClassLibrary.Models;

namespace DuoClassLibrary.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Sets the current user by username, creating a new user if one doesn't exist.
        /// </summary>
        /// <param name="username">The username to set or create.</param>
        /// <exception cref="ArgumentException">Thrown when username is null or empty.</exception>
        /// <exception cref="Exception">Thrown when user creation or retrieval fails.</exception>
        Task SetUser(string username);

        /// <summary>
        /// Gets the currently logged in user.
        /// </summary>
        /// <returns>The current user.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no user is logged in.</exception>
        User GetCurrentUser();

        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user if found, null otherwise.</returns>
        /// <exception cref="Exception">Thrown when user retrieval fails.</exception>
        Task<User> GetUserById(int id);

        /// <summary>
        /// Gets a user by their username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The user if found, null otherwise.</returns>
        Task<User> GetUserByUsername(string username);

        /// <summary>
        /// Clears the current user.
        /// </summary>
        void ClearCurrentUser();
    }
} 