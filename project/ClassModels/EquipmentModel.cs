namespace Project.ClassModels
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Data.SqlClient;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// EquipmentModel class handles database operations for equipment.
    /// </summary>
    public class EquipmentModel
    {
        private readonly string connectionString = DatabaseHelper.GetConnectionString();

        /// <summary>
        /// Adds a new equipment to the database.
        /// </summary>
        /// <param name="equipment">The equipment to add.</param>
        /// <returns>True if the equipment was added successfully, otherwise false.</returns>
        public bool AddEquipment(Equipment equipment)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "INSERT INTO Equipments (Name, Type, Specification, Stock) VALUES (@Name, @Type, @Specification, @Stock)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", equipment.Name);
                command.Parameters.AddWithValue("@Type", equipment.Type);
                command.Parameters.AddWithValue("@Specification", equipment.Specification);
                command.Parameters.AddWithValue("@Stock", equipment.Stock);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Updates an existing equipment in the database.
        /// </summary>
        /// <param name="equipment">The equipment to update.</param>
        /// <returns>True if the equipment was updated successfully, otherwise false.</returns>
        public bool UpdateEquipment(Equipment equipment)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Equipments SET Name = @Name, Specification = @Specification, Type = @Type, Stock = @Stock WHERE EquipmentID = @EquipmentID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", equipment.Name);
                    command.Parameters.AddWithValue("@Specification", equipment.Specification);
                    command.Parameters.AddWithValue("@Type", equipment.Type);
                    command.Parameters.AddWithValue("@Stock", equipment.Stock);
                    command.Parameters.AddWithValue("@EquipmentID", equipment.EquipmentID);

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
        /// Deletes an equipment from the database.
        /// </summary>
        /// <param name="equipmentID">The ID of the equipment to delete.</param>
        /// <returns>True if the equipment was deleted successfully, otherwise false.</returns>
        public bool DeleteEquipment(int equipmentID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "DELETE FROM Equipments WHERE EquipmentID = @EquipmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EquipmentID", equipmentID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Checks if an equipment exists in the database.
        /// </summary>
        /// <param name="equipmentID">The ID of the equipment to check.</param>
        /// <returns>True if the equipment exists, otherwise false.</returns>
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
        /// Retrieves all equipment from the database.
        /// </summary>
        /// <returns>A list of equipment.</returns>
        public List<Equipment> GetEquipments()
        {
            List<Equipment> equipments = new List<Equipment>();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT * FROM Equipments";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Equipment equipment = new Equipment
                    {
                        EquipmentID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Specification = reader.GetString(2),
                        Type = reader.GetString(3),
                        Stock = reader.GetInt32(4),
                    };
                    equipments.Add(equipment);
                }
            }

            return equipments;
        }
    }
}