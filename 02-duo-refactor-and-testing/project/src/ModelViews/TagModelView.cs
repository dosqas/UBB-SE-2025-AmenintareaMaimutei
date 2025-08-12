using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;
using CourseApp.Models;
using CourseApp.Data;

namespace CourseApp.ModelViews
{
    [ExcludeFromCodeCoverage]
    public class TagModelView : DataLink
    {
        /// <summary>
        /// Retrieves all tags from the database.
        /// </summary>
        /// <returns>A list of all tags.</returns>
        public static List<Tag> GetAllTags()
        {
            var tags = new List<Tag>();
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT TagId, Name FROM Tags";
            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tags.Add(new Tag
                {
                    TagId = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return tags;
        }

        /// <summary>
        /// Retrieves the tags associated with a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course for which tags are to be fetched.</param>
        /// <returns>A list of tags associated with the specified course.</returns>
        public static List<Tag> GetTagsForCourse(int courseId)
        {
            var tags = new List<Tag>();
            using var connection = GetConnection();
            connection.Open();
            string query = @"
                SELECT t.TagId, t.Name 
                FROM Tags t
                INNER JOIN CourseTags ct ON t.TagId = ct.TagId
                WHERE ct.CourseId = @courseId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@courseId", courseId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tags.Add(new Tag
                {
                    TagId = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return tags;
        }
    }
}
