using Microsoft.Data.SqlClient;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ClassModels
{
    class UserModel
    {
        private readonly string _connectionString = DatabaseHelper.GetConnectionString();

        public bool UserExistsWithRole(int userID, string role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
