using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using CourseApp.Models;
using CourseApp.Data;

namespace CourseApp.ModelViews
{
    /// <summary>
    /// Provides methods for retrieving course data from the database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CourseModelView : DataLink
    {
        /// <summary>
        /// Retrieves a course by its ID from the database.
        /// </summary>
        /// <param name="courseId">The ID of the course to retrieve.</param>
        /// <returns>A <see cref="Course"/> object if found, otherwise <c>null</c>.</returns>
        public static Course? GetCourse(int courseId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT CourseId, Title, Description, isPremium, Cost, ImageUrl, timeToComplete, difficulty FROM Courses WHERE CourseId = @courseId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@courseId", courseId);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Course
                {
                    CourseId = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    IsPremium = reader.GetBoolean(3),
                    Cost = reader.GetInt32(4),
                    ImageUrl = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    TimeToComplete = reader.GetInt32(6),
                    Difficulty = reader.IsDBNull(7) ? "Easy" : reader.GetString(7)
                };
            }
            return null;
        }

        /// <summary>
        /// Retrieves all courses from the database.
        /// </summary>
        /// <returns>A list of <see cref="Course"/> objects representing all courses in the database.</returns>
        public static List<Course> GetAllCourses()
        {
            var courses = new List<Course>();
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT CourseId, Title, Description, isPremium, Cost, ImageUrl, timeToComplete, difficulty FROM Courses";
            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                courses.Add(new Course
                {
                    CourseId = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    IsPremium = reader.GetBoolean(3),
                    Cost = reader.GetInt32(4),
                    ImageUrl = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    TimeToComplete = reader.GetInt32(6),
                    Difficulty = reader.IsDBNull(7) ? "Easy" : reader.GetString(7)
                });
            }
            return courses;
        }
    }
}