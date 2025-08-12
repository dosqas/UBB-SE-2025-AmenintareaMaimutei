using System;
using System.Threading.Tasks;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services.Interfaces;

namespace DuoClassLibrary.Services
{
    public class SignUpService
    {
        private readonly IUserHelperService _userHelperService;

        public SignUpService(IUserHelperService userHelperService)
        {
            _userHelperService = userHelperService ?? throw new ArgumentNullException(nameof(userHelperService));
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            try
            {
                var user = await _userHelperService.GetUserByUsername(username);
                return user != null;
            }
            catch (Exception checkingException)
            {
                Console.WriteLine($"Error checking username: {checkingException.Message}");
                return true;
            }
        }

        public async Task<bool> RegisterUser(User user)
        {
            // Check if email exists
            if (await _userHelperService.GetUserByEmail(user.Email) != null)
                return false;

            // Check if username exists
            if (await IsUsernameTaken(user.UserName))
                return false;

            user.OnlineStatus = true;
            user.UserId = await _userHelperService.CreateUser(user);
            return true;
        }
    }
} 