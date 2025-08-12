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
    public class ShiftModel
    {
        private readonly string _connectionString = DatabaseHelper.GetConnectionString();

        public bool AddShift(Shift shift)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //string query = "INSERT INTO Shifts (ShiftID, Date, StartTime, EndTime) VALUES (@ShiftID, @Date, @StartTime, @EndTime)";
                string query = "INSERT INTO Shifts (Date, StartTime, EndTime) VALUES (@Date, @StartTime, @EndTime)";
                SqlCommand command = new SqlCommand(query, connection);
                //command.Parameters.AddWithValue("@ShiftID", shift.ShiftID);
                command.Parameters.AddWithValue("@Date", shift.Date);
                command.Parameters.AddWithValue("@StartTime", shift.StartTime);
                command.Parameters.AddWithValue("@EndTime", shift.EndTime);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool UpdateShift(Shift shift)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
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

        public bool DoesShiftExist(Guid shiftID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Shifts WHERE ShiftID = @ShiftID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShiftID", shiftID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public List<Shift> GetShifts()
        {
            List<Shift> shifts = new List<Shift>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
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

        public bool DoesShiftExist(int shiftID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Shifts WHERE ShiftID = @ShiftID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShiftID", shiftID);
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public bool DeleteShift(int shiftID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
