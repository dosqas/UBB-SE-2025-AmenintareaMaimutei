using System.Diagnostics.CodeAnalysis;
using Duo.Data;
using Microsoft.Data.SqlClient;

namespace Duo.ModelViews
{
    [ExcludeFromCodeCoverage]
    public class EnrollmentModelView : DataLink
    {
        /// <summary>
        /// Checks if a user is enrolled in a specific course.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the user is enrolled in the course, otherwise false.</returns>
        public static bool IsUserEnrolled(int userId, int courseId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT COUNT(*) FROM Enrollment WHERE UserId = @userId AND CourseId = @courseId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@courseId", courseId);
            return (int)command.ExecuteScalar() > 0;
        }

        /// <summary>
        /// Enrolls a user in a specific course if they are not already enrolled.
        /// </summary>
        /// <param name="userId">The ID of the user to enroll.</param>
        /// <param name="courseId">The ID of the course to enroll in.</param>
        public static void EnrollUser(int userId, int courseId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM Enrollment WHERE UserId=@userId AND CourseId=@courseId)
                INSERT INTO Enrollment (UserId, CourseId, EnrolledAt, isCompleted) 
                VALUES (@userId, @courseId, GETDATE(), 0)";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@courseId", courseId);
            command.ExecuteNonQuery();
        }
    }
}