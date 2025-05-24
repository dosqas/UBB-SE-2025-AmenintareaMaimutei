using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System.Diagnostics;
using Duo.Views;
using Microsoft.Extensions.Configuration;
using Duo.ViewModels;
using Duo.Services;
using DuoClassLibrary.Models;
using Microsoft.Extensions.DependencyInjection;
using DuoClassLibrary.Services.Interfaces;
using DuoClassLibrary.Repositories;
using DuoClassLibrary.Repositories.Interfaces;
using DuoClassLibrary.Repositories.Proxies;
using Microsoft.EntityFrameworkCore;
using DuoClassLibrary.Services;

namespace Duo
{

    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider;
        public static User? CurrentUser { get; set; }
        public static Window? MainAppWindow { get; private set; }
        public static UserService userService;
  
        public static IUserHelperService _userHelperService;
        public static IUserRepository _userRepository;
        public static IPostRepository _postRepository;
        public static IHashtagRepository _hashtagRepository;
        public static IHashtagService _hashtagService;
        public static IPostService _postService;
        public static ICategoryService _categoryService;
        public static ICommentRepository _commentRepository;
        public static ICommentService _commentService;
        public static SearchService _searchService;

        public App()
        {
            this.InitializeComponent();

            _userRepository = new UserRepositoryProxy();
            _userHelperService = new UserHelperService(_userRepository);
            _hashtagRepository = new HashtagRepositoryProxi();
            ICategoryRepository categoryRepository = new CategoryRepositoryProxi();
            _postRepository = new PostRepositoryProxi();
            _hashtagService = new HashtagService(_hashtagRepository, _postRepository);
            userService = new UserService(_userHelperService);
            _searchService = new SearchService();
            _postService = new PostService(_postRepository, _hashtagService, userService, _searchService, _hashtagRepository);
            _commentRepository = new CommentRepositoryProxi();
            _commentService = new CommentService(_commentRepository, _postRepository, userService);
            _categoryService = new CategoryService(categoryRepository);

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Register repositories
            services.AddSingleton<IUserRepository, UserRepositoryProxy>();
            services.AddSingleton<IPostRepository, PostRepositoryProxi>();
            services.AddSingleton<ICommentRepository, CommentRepositoryProxi>();

            // Register services
            services.AddSingleton<IUserHelperService, UserHelperService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ICommentService, CommentService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<FriendsService>();
            services.AddTransient<SignUpService>();
            services.AddTransient<ProfileService>();
            services.AddTransient<LeaderboardService>();

            // Register view models
            services.AddTransient<LoginViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<ResetPassViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<LeaderboardViewModel>();
        }

        /// <summary>
        /// Handles the application launch.
        /// </summary>
        /// <param name="args">Launch arguments.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainAppWindow = new MainWindow();
            MainAppWindow.Activate();
        }

        private Window? window;
    }
}