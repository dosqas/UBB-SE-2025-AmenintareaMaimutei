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

        public bool AddSchedule(Schedule schedule)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //string query = "INSERT INTO Schedules (ScheduleID, DoctorID, ShiftID) VALUES (@ScheduleID, @DoctorID, @ShiftID)";
                string query = "INSERT INTO Schedules (DoctorID, ShiftID) VALUES (@DoctorID, @ShiftID)";
                SqlCommand command = new SqlCommand(query, connection);
                //command.Parameters.AddWithValue("@ScheduleID", schedule.ScheduleID);
                command.Parameters.AddWithValue("@DoctorID", schedule.DoctorID);
                command.Parameters.AddWithValue("@ShiftID", schedule.ShiftID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool UpdateSchedule(Schedule schedule)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE Schedules SET DoctorID = @DoctorID, ShiftID = @ShiftID WHERE ScheduleID = @ScheduleID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@DoctorID", schedule.DoctorID);
                    command.Parameters.AddWithValue("@ShiftID", schedule.ShiftID);
                    command.Parameters.AddWithValue("@ScheduleID", schedule.ScheduleID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
            catch(InvalidOperationException ex)
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

        public bool DeleteSchedule(int scheduleID)
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

        public bool DoesScheduleExist(int scheduleID)
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

        public bool DoesDoctorExist(int doctorID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Doctors WHERE DoctorID = @DoctorID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DoctorID", doctorID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
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

        public List<Schedule> GetSchedules()
        {
            List<Schedule> schedules = new List<Schedule>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                string query = "SELECT * FROM Schedules";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    schedules.Add(new Schedule
                    {
                        ScheduleID = reader.GetInt32(reader.GetOrdinal("ScheduleID")),
                        DoctorID = reader.GetInt32(reader.GetOrdinal("DoctorID")),
                        ShiftID = reader.GetInt32(reader.GetOrdinal("ShiftID"))
                    });
                }
            }
            return schedules;
        }
    }
}