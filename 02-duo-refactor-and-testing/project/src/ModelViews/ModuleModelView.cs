using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using CourseApp.Models;
using CourseApp.Data;

namespace CourseApp.ModelViews
{
    /// <summary>
    /// Provides methods to manage modules and user progress, including retrieving, updating, and checking module status.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ModuleModelView : DataLink
    {
        /// <summary>
        /// Retrieves a module by its ID.
        /// </summary>
        /// <param name="moduleId">The ID of the module to retrieve.</param>
        /// <returns>The <see cref="Module"/> if found, otherwise <c>null</c>.</returns>
        public static Module? GetModule(int moduleId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT ModuleId, CourseId, Title, Description, Position, isBonus, Cost, ImageUrl FROM Modules WHERE ModuleId = @moduleId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@moduleId", moduleId);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Module
                {
                    ModuleId = reader.GetInt32(0),
                    CourseId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    Position = reader.GetInt32(4),
                    IsBonus = reader.GetBoolean(5),
                    Cost = reader.GetInt32(6),
                    ImageUrl = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
                };
            }
            return null;
        }

        /// <summary>
        /// Retrieves a list of modules associated with a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course to retrieve modules for.</param>
        /// <returns>A list of <see cref="Module"/> objects.</returns>
        public static List<Module> GetModulesByCourseId(int courseId)
        {
            var modules = new List<Module>();
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT ModuleId, CourseId, Title, Description, Position, isBonus, Cost, ImageUrl FROM Modules WHERE CourseId = @courseId ORDER BY Position";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@courseId", courseId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                modules.Add(new Module
                {
                    ModuleId = reader.GetInt32(0),
                    CourseId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    Position = reader.GetInt32(4),
                    IsBonus = reader.GetBoolean(5),
                    Cost = reader.GetInt32(6),
                    ImageUrl = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
                });
            }
            return modules;
        }

        /// <summary>
        /// Checks if a module is open for a user based on their progress.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <param name="moduleId">The ID of the module to check.</param>
        /// <returns><c>true</c> if the module is open for the user, otherwise <c>false</c>.</returns>
        public static bool IsModuleOpen(int userId, int moduleId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT COUNT(*) FROM UserProgress WHERE UserId = @userId AND ModuleId = @moduleId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@moduleId", moduleId);
            return (int)command.ExecuteScalar() > 0;
        }

        /// <summary>
        /// Opens a module for a user by adding it to their progress.
        /// </summary>
        /// <param name="userId">The ID of the user to open the module for.</param>
        /// <param name="moduleId">The ID of the module to open.</param>
        public static void OpenModule(int userId, int moduleId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = @"INSERT INTO UserProgress (UserId, ModuleId, status, ImageClicked) VALUES (@userId, @moduleId, 'not_completed', 0)";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@moduleId", moduleId);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks if the image for a module has been clicked by the user.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <param name="moduleId">The ID of the module to check.</param>
        /// <returns><c>true</c> if the image has been clicked, otherwise <c>false</c>.</returns>
        public static bool IsModuleImageClicked(int userId, int moduleId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT ImageClicked FROM UserProgress WHERE ModuleId = @moduleId AND UserId = @userId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@moduleId", moduleId);
            command.Parameters.AddWithValue("@userId", userId);
            var result = command.ExecuteScalar();
            return result != null && Convert.ToBoolean(result);
        }

        /// <summary>
        /// Marks the image of a module as clicked for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        public static void ClickModuleImage(int userId, int moduleId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "UPDATE UserProgress SET ImageClicked = 1 WHERE ModuleId = @moduleId AND UserId = @userId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@moduleId", moduleId);
            command.Parameters.AddWithValue("@userId", userId);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks if a module is marked as completed for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <param name="moduleId">The ID of the module to check.</param>
        /// <returns><c>true</c> if the module is completed, otherwise <c>false</c>.</returns>
        public static bool IsModuleCompleted(int userId, int moduleId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT COUNT(*) FROM UserProgress WHERE UserId = @userId AND ModuleId = @moduleId AND status = 'completed'";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@moduleId", moduleId);
            return (int)command.ExecuteScalar() > 0;
        }

        /// <summary>
        /// Marks a module as completed for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="moduleId">The ID of the module.</param>
        public static void CompleteModule(int userId, int moduleId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = @"IF EXISTS (SELECT 1 FROM UserProgress WHERE UserId=@userId AND ModuleId=@moduleId)
                             UPDATE UserProgress SET status='completed' WHERE UserId=@userId AND ModuleId=@moduleId
                             ELSE
                             INSERT INTO UserProgress (UserId, ModuleId, status) VALUES (@userId, @moduleId, 'completed')";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@moduleId", moduleId);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks if a module is available for a user based on their progress and the module's prerequisites.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <param name="moduleId">The ID of the module to check.</param>
        /// <returns><c>true</c> if the module is available, otherwise <c>false</c>.</returns>
        public static bool IsModuleAvailable(int userId, int moduleId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = @"
                SELECT 
                    CASE
                        WHEN m.Position = 1 THEN 1
                        WHEN m.IsBonus = 1 THEN 1
                        WHEN EXISTS (
                            SELECT 1 FROM UserProgress up
                            INNER JOIN Modules prev ON up.ModuleId = prev.ModuleId
                            WHERE up.UserId = @userId
                            AND prev.CourseId = m.CourseId
                            AND prev.Position = m.Position - 1
                            AND up.status = 'completed'
                        ) THEN 1
                        ELSE 0
                    END as IsAvailable
                FROM Modules m
                WHERE m.ModuleId = @moduleId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@moduleId", moduleId);
            bool available = (int)command.ExecuteScalar() == 1;
            return available;
        }

        /// <summary>
        /// Checks if a module is in progress for a user based on their progress records in the database.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <param name="moduleId">The ID of the module to check.</param>
        /// <returns><c>true</c> if the module is in progress, otherwise <c>false</c>.</returns>
        public static bool IsModuleInProgress(int userId, int moduleId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT COUNT(*) FROM UserProgress WHERE UserId = @userId AND ModuleId = @moduleId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@moduleId", moduleId);
            return (int)command.ExecuteScalar() > 0;
        }
    }
}