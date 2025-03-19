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
    public class ScheduleModel
    {
        private readonly string _connectionString = DatabaseHelper.GetConnectionString();

        public bool DoesScheduleExist(Guid scheduleID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Schedules WHERE ScheduleID = @ScheduleID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ScheduleID", scheduleID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        public bool DeleteSchedule(Guid scheduleID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Schedules WHERE ScheduleID = @ScheduleID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ScheduleID", scheduleID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}