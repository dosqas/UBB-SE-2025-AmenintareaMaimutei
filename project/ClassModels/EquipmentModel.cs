using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Project.Utils;
using System.Runtime.InteropServices.Marshalling;

namespace Project.ClassModels
{
    public class EquipmentModel
    {
        private readonly string _connectionString = DatabaseHelper.GetConnectionString();

        public bool AddEquipment(Equipment equipment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //string query = "INSERT INTO Equipments (EquipmentID, Name, Type, Specification, Stock) VALUES (@EquipmentID, @Name, @Type, @Specification, @Stock)";
                string query = "INSERT INTO Equipments (Name, Type, Specification, Stock) VALUES (@Name, @Type, @Specification, @Stock)";
                SqlCommand command = new SqlCommand(query, connection);
                //command.Parameters.AddWithValue("@EquipmentID", equipment.EquipmentID);
                command.Parameters.AddWithValue("@Name", equipment.Name);
                command.Parameters.AddWithValue("@Type", equipment.Type);
                command.Parameters.AddWithValue("@Specification", equipment.Specification);
                command.Parameters.AddWithValue("@Stock", equipment.Stock);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool DeleteEquipment(int equipmentID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Equipments WHERE EquipmentID = @EquipmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EquipmentID", equipmentID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
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
        public List<Equipment> GetEquipments()
        {
            List<Equipment> equipments = new();

            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Equipments";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    equipments.Add(new Equipment
                    {
                        EquipmentID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Specification = reader.GetString(2),
                        Type = reader.GetString(3),
                        Stock = reader.GetInt32(4)
                    });
                }
            }

            return equipments;
        }

    }
}