// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShiftModel.cs" company="YourCompanyName">
//   Copyright (c) YourCompanyName. All rights reserved.
// </copyright>
// <summary>
//   Represents information about a shift model.
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
    /// Class that represents the model of a shift.
    /// </summary>
    public class ShiftModel
    {
        private readonly string connectionString = DatabaseHelper.GetConnectionString();

        /// <summary>
        /// Function that adds into the database a shift.
        /// </summary>
        /// <param name="shift">The shift to be added in the database.</param>
        /// <returns>True if the shift was added, and false otherwise.</returns>
        public bool AddShift(Shift shift)
        {
            using SqlConnection connection = new SqlConnection(this.connectionString);
            string query = "INSERT INTO Shifts (Date, StartTime, EndTime) VALUES (@Date, @StartTime, @EndTime)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Date", shift.Date);
            command.Parameters.AddWithValue("@StartTime", shift.StartTime);
            command.Parameters.AddWithValue("@EndTime", shift.EndTime);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        /// <summary>
        ///  Function that updates a shift in the database.
        /// </summary>
        /// <param name="shift">The shift to be updated in the database</param>
        /// <returns>True if the shift was updated, and false otherwise.</returns>
        public bool UpdateShift(Shift shift)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(this.connectionString);
                string query = "UPDATE Shifts SET Date = @Date, StartTime = @StartTime, EndTime = @EndTime WHERE ShiftID = @ShiftID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", shift.Date);
                command.Parameters.AddWithValue("@StartTime", shift.StartTime);
                command.Parameters.AddWithValue("@EndTime", shift.EndTime);
                command.Parameters.AddWithValue("@ShiftID", shift.ShiftID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (SqlException exception)
            {
                Console.WriteLine($"SQL Error: {exception.Message}");
                return false;
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine($"Invalid Operation: {exception.Message}");
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Unexpected Error: {exception.Message}");
                return false;
            }
        }
        /// <summary>
        ///  Function that gets all the shifts from the database.
        /// </summary>
        /// <returns>A list of shifts from the database</returns>
        ///
        public List<Shift> GetShifts()
        {
            List<Shift> shifts = new List<Shift>();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT * FROM Shifts";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Shift shift = new Shift
                    {
                        ShiftID = reader.GetInt32(0),
                        Date = DateOnly.FromDateTime(reader.GetDateTime(1)),
                        StartTime = reader.GetTimeSpan(2),
                        EndTime = reader.GetTimeSpan(3)
                    };
                    shifts.Add(shift);
                }
            }
            return shifts;
        }
        
        /// <summary>
        /// Function that checks if a shift exists in the database.
        /// </summary>
        /// <param name="shiftID">The ID of the shift to check.</param>
        /// <returns>True if the shift exists, false otherwise.</returns>
        public bool DoesShiftExist(int shiftID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Shifts WHERE ShiftID = @ShiftID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShiftID", shiftID);
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        
        /// <summary>
        /// Function that deletes a shift from the database.
        /// </summary>
        /// <param name="shiftID">The ID of the shift to delete.</param>
        /// <returns>True if the shift was deleted, false otherwise.</returns>
        public bool DeleteShift(int shiftID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Shifts WHERE ShiftID = @ShiftID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShiftID", shiftID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
