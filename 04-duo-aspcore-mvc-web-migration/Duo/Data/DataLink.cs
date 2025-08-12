using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;

namespace Duo.Data
{
    /// <summary>
    /// Provides a static method to retrieve a SQL connection to the CourseApp database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DataLink
    {
		private static readonly string ConnectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=CourseApp;Integrated Security=True;TrustServerCertificate=True";

        /// <summary>
        /// Retrieves a new SQL connection using the predefined connection string.
        /// </summary>
        /// <returns>A new instance of <see cref="SqlConnection"/> connected to the CourseApp database.</returns>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
