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
    public class RoomModel
    {
        private readonly string _connectionString = DatabaseHelper.GetConnectionString();

        public bool AddRoom(Room room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //string query = "INSERT INTO Rooms (RoomID, Capacity, DepartmentID, EquipmentID) VALUES (@RoomID, @Capacity, @DepartmentID, @EquipmentID)";
                string query = "INSERT INTO Rooms (Capacity, DepartmentID, EquipmentID) VALUES (@Capacity, @DepartmentID, @EquipmentID)";
                SqlCommand command = new SqlCommand(query, connection);
                //command.Parameters.AddWithValue("@RoomID", room.RoomID);
                command.Parameters.AddWithValue("@Capacity", room.Capacity);
                command.Parameters.AddWithValue("@DepartmentID", room.DepartmentID);
                command.Parameters.AddWithValue("@EquipmentID", room.EquipmentID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool UpdateRoom(Room room)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE Rooms SET Capacity = @Capacity, DepartmentID = @DepartmentID, EquipmentID = @EquipmentID WHERE RoomID = @RoomID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Capacity", room.Capacity);
                    command.Parameters.AddWithValue("@DepartmentID", room.DepartmentID);
                    command.Parameters.AddWithValue("@EquipmentID", room.EquipmentID);
                    command.Parameters.AddWithValue("@RoomID", room.RoomID);
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
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool DeleteRoom(int roomID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Rooms WHERE RoomID = @RoomID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", roomID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool DoesRoomExist(int roomID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Rooms WHERE RoomID = @RoomID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", roomID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public bool DoesEquipmentExist(int equipmentID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Equipments WHERE EquipmentID = @EquipmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EquipmentID", equipmentID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public bool DoesDepartmentExist(int departmentID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Departments WHERE DepartmentID = @DepartmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentID", departmentID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public List<Room>? GetRooms()
        {
            try
            {
                List<Room> rooms = new List<Room>();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Rooms";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        rooms.Add(new Room
                        {
                            //RoomID = reader.GetGuid(0),
                            RoomID = reader.GetInt32(0),
                            Capacity = reader.GetInt32(1),
                            DepartmentID = reader.GetInt32(2),
                            EquipmentID = reader.GetInt32(3)
                            //DepartmentID = reader.GetGuid(2),
                            //EquipmentID = reader.GetGuid(3)
                        });
                    }
                }
                return rooms;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return null;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid Operation: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}