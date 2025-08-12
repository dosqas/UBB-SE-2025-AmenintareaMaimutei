using DuoClassLibrary.Models;
using Duo.Services;
using System;
using System.Threading.Tasks;
using DuoClassLibrary.Services;

namespace Duo.ViewModels
{
    /// <summary>
    /// ViewModel for the user profile management
    /// </summary>
    public class ProfileViewModel
    {
        private readonly ProfileService profileService;

        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public User CurrentUser { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileViewModel"/> class with a specific user
        /// </summary>
        /// <param name="profileService">The profile service</param>
        /// <param name="user">The user</param>
        public ProfileViewModel(ProfileService profileService, User user)
        {
            this.profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            CurrentUser = user ?? throw new ArgumentNullException(nameof(user));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileViewModel"/> class using the application's current user
        /// </summary>
        /// <param name="profileService">The profile service</param>
        public ProfileViewModel(ProfileService profileService)
        {
            this.profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            
            // This will be null until App.CurrentUser is set
            CurrentUser = App.CurrentUser;
        }

        /// <summary>
        /// Saves changes to the user profile
        /// </summary>
        /// <param name="isPrivate">Whether the profile is private</param>
        /// <param name="newBase64Image">The new profile image in base64 format</param>
        public void SaveChanges(bool isPrivate, string newBase64Image)
        {
            if (CurrentUser == null)
            {
                throw new InvalidOperationException("CurrentUser is not set");
            }

            // Only update if a new image is provided
            if (!string.IsNullOrWhiteSpace(newBase64Image))
            {
                CurrentUser.ProfileImage = newBase64Image;
            }

            CurrentUser.PrivacyStatus = isPrivate;

            profileService.UpdateUser(CurrentUser);
        }

        /// <summary>
        /// Gets the user statistics
        /// </summary>
        /// <returns>User with updated statistics</returns>
        public async Task<User> GetUserStats()
        {
            if (CurrentUser == null)
            {
                throw new InvalidOperationException("CurrentUser is not set");
            }
            
            return await profileService.GetUserStats(CurrentUser.UserId);
        }
    }
}
