namespace Project.ClassModels
{
    using Microsoft.Data.SqlClient;
    using Project.Utils;

    /// <summary>
    /// User model class.
    /// </summary>
    public class UserModel
    {
        private readonly string connectionString = DatabaseHelper.GetConnectionString();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userID">The id of the user.</param>
        /// <param name="role">The role of.</param>
        /// <returns>The joined names.</returns>
        public bool UserExistsWithRole(int userID, string role)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE UserID = @UserID AND Role = @Role";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@Role", role);
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
