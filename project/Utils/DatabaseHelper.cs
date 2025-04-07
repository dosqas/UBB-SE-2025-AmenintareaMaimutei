namespace Project.Utils
{
    using System.Configuration;

    /// <summary>
    /// Provides utility methods for database operations.
    /// </summary>
    public class DatabaseHelper
    {
        /// <summary>
        /// Gets the connection string for the HospitalManagement database.
        /// </summary>
        /// <returns>The connection string.</returns>
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HospitalManagement"].ConnectionString;
        }
    }
}