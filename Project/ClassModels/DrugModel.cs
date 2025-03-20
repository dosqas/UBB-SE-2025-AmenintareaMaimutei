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
    public class DrugModel
    {
        private readonly string _connectionString = DatabaseHelper.GetConnectionString();

        public bool AddDrug(Drug drug)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Drugs (DrugID, Name, Administration, Specification, Supply) VALUES (@DrugID, @Name, @Administration, @Specification, @Supply)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DrugID", drug.DrugID);
                command.Parameters.AddWithValue("@Name", drug.Name);
                command.Parameters.AddWithValue("@Administration", drug.Administration);
                command.Parameters.AddWithValue("@Specification", drug.Specification);
                command.Parameters.AddWithValue("@Supply", drug.Supply);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool DeleteDrug(Guid drugID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Drugs WHERE DrugID = @DrugID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DrugID", drugID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool DoesDrugExist(Guid drugID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Drugs WHERE DrugID = @DrugID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DrugID", drugID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}