// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoctorModel.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved. Licensed under the MIT License.
// </copyright>
// <summary>
//   Defines the DoctorModel class for managing doctor-related operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.ClassModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Data.SqlClient;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// Represents a model for managing doctor-related operations.
    /// </summary>
    public class DoctorModel
    {
        private const double Type0Rate = 200d;
        private const double Type1Rate = Type0Rate * 1.2d;
        private const double Type2Rate = Type1Rate * 1.5d;

        private readonly string connectionString = DatabaseHelper.GetConnectionString();

        /// <summary>
        /// Adds a new doctor to the database.
        /// </summary>
        /// <param name="doctor">The doctor to add.</param>
        /// <returns>True if the doctor was added successfully; otherwise, false.</returns>
        public bool AddDoctor(Doctor doctor)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "INSERT INTO Doctors (UserID, DepartmentID, Experience, Rating, LicenseNumber) VALUES (@UserID, @DepartmentID, @Experience, @Rating, @LicenseNumber)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", doctor.UserID);
                command.Parameters.AddWithValue("@DepartmentID", doctor.DepartmentID);
                command.Parameters.AddWithValue("@Experience", doctor.Experience);
                command.Parameters.AddWithValue("@Rating", doctor.Rating);
                command.Parameters.AddWithValue("@LicenseNumber", doctor.LicenseNumber);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Updates an existing doctor's information in the database.
        /// </summary>
        /// <param name="doctor">The doctor with updated information.</param>
        /// <returns>True if the doctor was updated successfully; otherwise, false.</returns>
        public bool UpdateDoctor(Doctor doctor)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Doctors SET UserID = @UserID, DepartmentID = @DepartmentID, Experience = @Experience, LicenseNumber = @LicenseNumber WHERE DoctorID = @DoctorID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", doctor.UserID);
                    command.Parameters.AddWithValue("@DepartmentID", doctor.DepartmentID);
                    command.Parameters.AddWithValue("@Experience", doctor.Experience);
                    command.Parameters.AddWithValue("@LicenseNumber", doctor.LicenseNumber);
                    command.Parameters.AddWithValue("@DoctorID", doctor.DoctorID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid Operation: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return false;
            }
        }

        // Add similar XML documentation for other methods in the class.
    }
}