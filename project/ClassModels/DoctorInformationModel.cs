// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoctorInformationModel.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   This file contains the DoctorInformationModel class, which provides methods for retrieving and computing
//   information related to doctors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.ClassModels
{
    using System;
    using System.Data.SqlClient;
    using Microsoft.Data.SqlClient;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// Represents a model for retrieving and computing information related to doctors.
    /// </summary>
    public class DoctorInformationModel
    {
        private readonly string connectionString = DatabaseHelper.GetConnectionString();

        /// <summary>
        /// Retrieves detailed information about a doctor based on their ID.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor.</param>
        /// <returns>A <see cref="DoctorInformation"/> object containing the doctor's details.</returns>
        /// <exception cref="Exception">Thrown if the doctor is not found or the information is null.</exception>
        public DoctorInformation GetDoctorInformation(int doctorId)
        {
            DoctorInformation? doctorInformation = null; // Use nullable type

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                string query = @"
                            SELECT 
                                UserID, Username, Mail, Role, Name, Birthdate, Cnp, Address, PhoneNumber, RegistrationDate, 
                                DoctorID, LicenseNumber, Experience, Rating, DepartmentID, DepartmentName
                            FROM UserDoctorDepartmentView
                            WHERE DoctorID = @DoctorID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DoctorID", doctorId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            doctorInformation = new DoctorInformation(
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetString(reader.GetOrdinal("Username")),
                                reader.GetString(reader.GetOrdinal("Mail")),
                                reader.GetString(reader.GetOrdinal("Role")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetDateTime(reader.GetOrdinal("Birthdate")),
                                reader.GetString(reader.GetOrdinal("Cnp")),
                                reader.GetString(reader.GetOrdinal("Address")),
                                reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                                reader.GetInt32(reader.GetOrdinal("DoctorID")),
                                reader.GetString(reader.GetOrdinal("LicenseNumber")),
                                (float)reader.GetDouble(reader.GetOrdinal("Experience")),
                                (float)reader.GetDouble(reader.GetOrdinal("Rating")),
                                reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                reader.GetString(reader.GetOrdinal("DepartmentName")));
                        }
                        else
                        {
                            throw new Exception("Doctor not found");
                        }
                    }
                }
            }

            return doctorInformation ?? throw new Exception("Doctor information is null");
        }

        /// <summary>
        /// Computes the salary of a doctor based on their shifts in the current month.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor.</param>
        /// <returns>The computed salary as a decimal value.</returns>
        public decimal ComputeSalary(int doctorId)
        {
            decimal salary = 0;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                string query = @"
                            SELECT StartTime, EndTime
                            FROM GetCurrentMonthShiftsForDoctor(@DoctorID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DoctorID", doctorId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TimeSpan startTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime"));
                            TimeSpan endTime = reader.GetTimeSpan(reader.GetOrdinal("EndTime"));

                            if (startTime == new TimeSpan(8, 0, 0) && endTime == new TimeSpan(20, 0, 0))
                            {
                                salary += 100 * 12;
                            }
                            else if (startTime == new TimeSpan(20, 0, 0) && endTime == new TimeSpan(8, 0, 0))
                            {
                                salary += 100 * 1.2m * 12;
                            }
                            else if (startTime == new TimeSpan(8, 0, 0) && endTime == new TimeSpan(8, 0, 0))
                            {
                                salary += 100 * 1.5m * 24;
                            }
                        }
                    }
                }
            }

            return salary;
        }
    }
}