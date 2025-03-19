using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Project.Utils;

namespace Project.ClassModels
{
    public class DoctorModel
    {
        private readonly string _connectionString = DatabaseHelper.GetConnectionString();

        public bool UpdateDoctor(Doctor doctor)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Doctors SET UserID = @UserID, DepartmentID = @DepartmentID, Experience = @Experience, LicenseNumber = @LicenseNumber WHERE DoctorID = @DoctorID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", doctor.UserID);
                command.Parameters.AddWithValue("@DepartmentID", doctor.DepartmentID);
                command.Parameters.AddWithValue("@YearsOfExperience", doctor.Experience);
                command.Parameters.AddWithValue("@LicenseNumber", doctor.LicenseNumber);
                command.Parameters.AddWithValue("@DoctorID", doctor.DoctorID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
