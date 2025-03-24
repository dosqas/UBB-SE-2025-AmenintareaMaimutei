using System;
using System.Data.SqlClient;
using Project.Models;
using Project.Utils;
using Microsoft.Data.SqlClient;

namespace Project.ClassModels
{
    public class DoctorInformationModel
    {
        private readonly string _connectionString = DatabaseHelper.GetConnectionString();

        public DoctorInformation GetDoctorInformation(int doctorId)
        {
            DoctorInformation doctorInformation = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
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
                                reader.GetString(reader.GetOrdinal("DepartmentName"))
                            );
                        }
                        else throw new Exception("Doctor not found");
                    }
                }
            }

            return doctorInformation;
        }
    }
}