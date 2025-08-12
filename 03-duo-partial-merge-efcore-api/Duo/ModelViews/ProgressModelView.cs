using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Duo.Data;

namespace Duo.ModelViews
{
    [ExcludeFromCodeCoverage]
    public class ProgressModelView : DataLink
    {
        /// <summary>
        /// Updates the time spent by a user on a specific course by adding the provided seconds to the current time spent.
        /// </summary>
        /// <param name="userId">The ID of the user whose time spent will be updated.</param>
        /// <param name="courseId">The ID of the course where time will be updated.</param>
        /// <param name="seconds">The number of seconds to add to the current time spent.</param>
        public static void UpdateTimeSpent(int userId, int courseId, int seconds)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "UPDATE Enrollment SET TimeSpent = TimeSpent + @seconds WHERE UserId = @userId AND CourseId = @courseId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@courseId", courseId);
            command.Parameters.AddWithValue("@seconds", seconds);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves the total time spent by a user on a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user whose time spent is to be fetched.</param>
        /// <param name="courseId">The ID of the course for which time spent is to be fetched.</param>
        /// <returns>The total time spent by the user on the course in seconds.</returns>
        public static int GetTimeSpent(int userId, int courseId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT TimeSpent FROM Enrollment WHERE UserId = @userId AND CourseId = @courseId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@courseId", courseId);
            var result = command.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// Retrieves the total number of required modules for a course (excluding bonus modules).
        /// </summary>
        /// <param name="courseId">The ID of the course for which the module count is to be fetched.</param>
        /// <returns>The number of required modules for the specified course.</returns>
        public static int GetRequiredModulesCount(int courseId)
        {
            int count = 0;
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Modules WHERE CourseId = @courseId AND IsBonus = 0";
                using SqlCommand command = new (query, connection);
                command.Parameters.AddWithValue("@courseId", courseId);
                count = (int)command.ExecuteScalar();
            }
            return count;
        }

        /// <summary>
        /// Retrieves the number of completed modules for a user in a specific course (excluding bonus modules).
        /// </summary>
        /// <param name="userId">The ID of the user whose completed modules are to be counted.</param>
        /// <param name="courseId">The ID of the course for which the completed module count is to be fetched.</param>
        /// <returns>The number of completed modules for the user in the course.</returns>
        public static int GetCompletedModulesCount(int userId, int courseId)
        {
            int count = 0;
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"SELECT COUNT(*) FROM UserProgress up
                        INNER JOIN Modules m ON up.ModuleId = m.ModuleId
                        WHERE up.UserId = @userId AND m.CourseId = @courseId
                        AND up.status = 'completed' AND m.IsBonus = 0";
                using SqlCommand command = new (query, connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@courseId", courseId);
                count = (int)command.ExecuteScalar();
            }
            return count;
        }

        /// <summary>
        /// Checks if a user has completed all the required modules for a course (excluding bonus modules).
        /// </summary>
        /// <param name="userId">The ID of the user whose course completion status is to be checked.</param>
        /// <param name="courseId">The ID of the course to check for completion.</param>
        /// <returns><c>true</c> if the user has completed all required modules, otherwise <c>false</c>.</returns>
        public static bool IsCourseCompleted(int userId, int courseId)
        {
            int requiredModules = GetRequiredModulesCount(courseId);
            int completedModules = GetCompletedModulesCount(userId, courseId);
            return requiredModules > 0 && requiredModules == completedModules;
        }

        /// <summary>
        /// Marks a course as completed for a user and records the completion in the database.
        /// </summary>
        /// <param name="userId">The ID of the user who completed the course.</param>
        /// <param name="courseId">The ID of the course that is marked as completed.</param>
        public static void MarkCourseAsCompleted(int userId, int courseId)
        {
            using SqlConnection connection = GetConnection();
            connection.Open();
            string query = @"
                    IF NOT EXISTS (
                        SELECT 1 FROM CourseCompletions 
                        WHERE UserId = @userId AND CourseId = @courseId
                    )
                    BEGIN
                        INSERT INTO CourseCompletions (UserId, CourseId, CompletionRewardClaimed, TimedRewardClaimed, CompletedAt)
                        VALUES (@userId, @courseId, 0, 0, GETDATE())
                    END";

            using SqlCommand command = new (query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@courseId", courseId);
            command.ExecuteNonQuery();
        }
    }
}