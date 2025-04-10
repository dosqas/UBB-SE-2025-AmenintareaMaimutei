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
    /// Handles database operations related to rooms, such as adding, updating, and deleting room records.
    /// </summary>
    public class RoomModel
    {
        private readonly string connectionString = DatabaseHelper.GetConnectionString();

        /// <summary>
        /// Adds a new room to the database.
        /// </summary>
        /// <param name="room">The room object to be added.</param>
        /// <returns><c>true</c> if the room was successfully added; otherwise, <c>false</c>.</returns>
        public bool AddRoom(Room room)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                // string query = "INSERT INTO Rooms (RoomID, Capacity, DepartmentID, EquipmentID) VALUES (@RoomID, @Capacity, @DepartmentID, @EquipmentID)";
                string query = "INSERT INTO Rooms (Capacity, DepartmentID, EquipmentID) VALUES (@Capacity, @DepartmentID, @EquipmentID)";
                SqlCommand command = new SqlCommand(query, connection);

                // command.Parameters.AddWithValue("@RoomID", room.RoomID);
                command.Parameters.AddWithValue("@Capacity", room.Capacity);
                command.Parameters.AddWithValue("@DepartmentID", room.DepartmentID);
                command.Parameters.AddWithValue("@EquipmentID", room.EquipmentID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Updates an existing room in the database.
        /// </summary>
        /// <param name="room">The room object containing updated values.</param>
        /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
        public bool UpdateRoom(Room room)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
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

        /// <summary>
        /// Deletes a room from the database based on its ID.
        /// </summary>
        /// <param name="roomID">The ID of the room to delete.</param>
        /// <returns><c>true</c> if the room was deleted; otherwise, <c>false</c>.</returns>
        public bool DeleteRoom(int roomID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "DELETE FROM Rooms WHERE RoomID = @RoomID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", roomID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Checks whether a room with the specified ID exists in the database.
        /// </summary>
        /// <param name="roomID">The room ID to check for existence.</param>
        /// <returns><c>true</c> if the room exists; otherwise, <c>false</c>.</returns>
        public bool DoesRoomExist(int roomID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT COUNT(*) FROM Rooms WHERE RoomID = @RoomID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", roomID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Checks whether a piece of equipment with the given ID exists.
        /// </summary>
        /// <param name="equipmentID">The equipment ID to check.</param>
        /// <returns><c>true</c> if the equipment exists; otherwise, <c>false</c>.</returns>
        public bool DoesEquipmentExist(int equipmentID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT COUNT(*) FROM Equipments WHERE EquipmentID = @EquipmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EquipmentID", equipmentID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Checks whether a department with the given ID exists.
        /// </summary>
        /// <param name="departmentID">The department ID to check.</param>
        /// <returns><c>true</c> if the department exists; otherwise, <c>false</c>.</returns>
        public bool DoesDepartmentExist(int departmentID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT COUNT(*) FROM Departments WHERE DepartmentID = @DepartmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentID", departmentID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Retrieves a list of all rooms from the database.
        /// </summary>
        /// <returns>A list of <see cref="Room"/> objects, or <c>null</c> if an error occurred.</returns>
        public List<Room>? GetRooms()
        {
            try
            {
                List<Room> rooms = new List<Room>();
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Rooms";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        rooms.Add(new Room
                        {
                            // RoomID = reader.GetGuid(0),
                            RoomID = reader.GetInt32(0),
                            Capacity = reader.GetInt32(1),
                            DepartmentID = reader.GetInt32(2),
                            EquipmentID = reader.GetInt32(3),

                            // DepartmentID = reader.GetGuid(2),
                            // EquipmentID = reader.GetGuid(3)
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