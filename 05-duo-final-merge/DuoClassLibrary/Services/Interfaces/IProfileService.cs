using DuoClassLibrary.Models;
using System.Threading.Tasks;

namespace DuoClassLibrary.Services.Interfaces
{
    /// <summary>
    /// Interface for profile service operations
    /// </summary>
    public interface IProfileService
    {
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userToCreate">The user to create</param>
        Task CreateUser(User userToCreate);

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="userToUpdate">The user to update</param>
        Task UpdateUser(User userToUpdate);

        /// <summary>
        /// Gets user statistics
        /// </summary>
        /// <param name="userIdentifier">The user identifier</param>
        /// <returns>The user with statistics</returns>
        Task<User> GetUserStats(int userIdentifier);
    }
} 