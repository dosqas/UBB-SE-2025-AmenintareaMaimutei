using System;
using System.Threading.Tasks;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services.Interfaces;

namespace DuoClassLibrary.Services
{
    /// <summary>
    /// Implementation of profile service operations
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly IUserHelperService _userHelperService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/> class
        /// </summary>
        /// <param name="userHelperService">The user helper service</param>
        public ProfileService(IUserHelperService userHelperService)
        {
            _userHelperService = userHelperService ?? throw new ArgumentNullException(nameof(userHelperService));
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userToCreate">The user to create</param>
        public async Task CreateUser(User userToCreate)
        {
            await _userHelperService.CreateUser(userToCreate);
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="userToUpdate">The user to update</param>
        public async Task UpdateUser(User userToUpdate)
        {
            await _userHelperService.UpdateUser(userToUpdate);
        }

        /// <summary>
        /// Gets user statistics
        /// </summary>
        /// <param name="userIdentifier">The user identifier</param>
        /// <returns>The user with statistics</returns>
        public async Task<User> GetUserStats(int userIdentifier)
        {
            return await _userHelperService.GetUserStats(userIdentifier);
        }
    }
} 