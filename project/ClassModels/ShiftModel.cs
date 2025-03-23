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
    }
}
