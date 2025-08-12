using System;
using System.Threading.Tasks;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebServerTest.Models;

namespace WebServerTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IUserHelperService _userHelperService;
        private readonly SignUpService _signUpService;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;

        public AccountController(
            ILoginService loginService, 
            IUserHelperService userHelperService,
            SignUpService signUpService,
            ILogger<AccountController> logger,
            IUserService userService)
        {
            _loginService = loginService;
            _userHelperService = userHelperService;
            _signUpService = signUpService;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // Check if user is already logged in
            var userId = HttpContext.Session.GetInt32("UserId");
            
            if (userId.HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl ?? string.Empty });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Set a default empty ReturnUrl if it's null
            model.ReturnUrl ??= string.Empty;
            
            // Remove ReturnUrl validation errors if any
            if (ModelState.ContainsKey("ReturnUrl"))
            {
                ModelState.Remove("ReturnUrl");
            }
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var isAuthenticated = await _loginService.AuthenticateUser(model.Username, model.Password);
                
                if (isAuthenticated)
                {
                    // Get the user
                    var user = await _loginService.GetUserByCredentials(model.Username, model.Password);
                    
                    if (user != null)
                    {
                        // Store user ID in session or cookie
                        HttpContext.Session.SetInt32("UserId", user.UserId);
                        HttpContext.Session.SetString("Username", user.UserName);

                        // Set the current user in UserService
                        await _userService.SetUser(user.UserName);

                        // Redirect to home page or return URL
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User account could not be retrieved.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login attempt");
                ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            // Check if user is already logged in
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterViewModel { ShowPassword = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Check if username is already taken
                if (await _signUpService.IsUsernameTaken(model.Username))
                {
                    ModelState.AddModelError("Username", "Username is already taken");
                    return View(model);
                }

                // Create the user entity
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    DateJoined = DateTime.Now,
                    OnlineStatus = true
                };

                // Register the user
                var isRegistered = await _signUpService.RegisterUser(user);
                if (isRegistered)
                {
                    // Store user ID in session
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("Username", user.UserName);

                    // Set the current user in UserService
                    await _userService.SetUser(user.UserName);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Error registering user. Email may already be in use.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId.HasValue)
                {
                    var user = await _userHelperService.GetUserById(userId.Value);
                    if (user != null)
                    {
                        await _loginService.UpdateUserStatusOnLogout(user);
                    }
                }

                // Clear session
                HttpContext.Session.Clear();

                // Clear current user in UserService
                _userService.ClearCurrentUser();

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            // Check if email exists
            var user = await _userHelperService.GetUserByEmail(model.Email);
            
            // Always show success message even if email doesn't exist (security best practice)
            model.EmailSent = true;
            return View(model);
        }
        
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }
            
            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };
            
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            // In a real implementation, verify token and reset password
            // For now, just redirect to confirmation page
            
            return RedirectToAction("ResetPasswordConfirmation");
        }
        
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
} 