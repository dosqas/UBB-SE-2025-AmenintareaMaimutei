using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using DotNetEnv;
using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Roadmaps;
using Duo.Api.Models.Sections;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Duo.Models.Quizzes.API;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api
{
    /// <summary>
    /// The entry point of the application.
    /// Configures services, middleware, and the HTTP request pipeline.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Program
    {
        #region Methods

        /// <summary>
        /// The main method, which serves as the entry point of the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            // Load environment variables from the .env file.
            Env.Load(".env");

            // Create a WebApplication builder.
            var builder = WebApplication.CreateBuilder(args);

            // Configure services for the application.
            ConfigureServices(builder);

            // Build the application.
            var app = builder.Build();

            app.UseCors("AllowLocalHost");

            // Configure the HTTP request pipeline.
            ConfigureMiddleware(app);


            // Run the application.
            app.Run();
        }

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <param name="builder">The WebApplication builder.</param>
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalHost", builder =>
                    builder.WithOrigins(
                        "http://localhost:7037", 
                        "http://127.0.0.1:7037", 
                        "http://localhost:5198", 
                        "http://127.0.0.1:5198",
                        "https://localhost:7037",
                        "https://localhost:5198")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });

            // Add controllers with JSON options.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            // Register IRepository and Repository for dependency injection.
            builder.Services.AddScoped<IRepository, Repository>();
            builder.Services.AddScoped<Duo.Api.Repositories.Interfaces.IUserRepository, Duo.Api.Repositories.Repos.UserRepository>();
            builder.Services.AddScoped<Duo.Api.Repositories.Interfaces.IPostRepository, Duo.Api.Repositories.Repos.PostRepository>();
            builder.Services.AddScoped<Duo.Api.Repositories.Interfaces.IHashtagRepository, Duo.Api.Repositories.Repos.HashtagRepository>();
            builder.Services.AddScoped<Duo.Api.Repositories.Interfaces.IFriendsRepository, Duo.Api.Repositories.Repos.FriendsRepository>();
            builder.Services.AddScoped<Duo.Api.Repositories.Interfaces.ICommentRepository, Duo.Api.Repositories.Repos.CommentRepository>();
            builder.Services.AddScoped<Duo.Api.Repositories.Interfaces.ICategoryRepository, Duo.Api.Repositories.Repos.CategoryRepository>();
            builder.Services.AddScoped<Duo.Api.Repositories.IRepository, Duo.Api.Repositories.Repository>();
            

            // Configure Swagger/OpenAPI.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    builder.WithOrigins("https://localhost:7037")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            // Configure the database context with a connection string.
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__WebApiDatabase");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            }
            Console.WriteLine("Connection string: " + connectionString);

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionString));
        }

        /// <summary>
        /// Configures middleware for the application.
        /// </summary>
        /// <param name="app">The WebApplication instance.</param>
        private static void ConfigureMiddleware(WebApplication app)
        {
            // Enable Swagger in development environments.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable CORS
            app.UseCors("AllowFrontend");

            // Enable HTTPS redirection and authorization.
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Map controllers to endpoints.
            app.MapControllers();
        }

        /// <summary>
        /// Applies database migrations and seeds initial data.
        /// </summary>
        /// <param name="app">The WebApplication instance.</param>

        #endregion
    }
}
