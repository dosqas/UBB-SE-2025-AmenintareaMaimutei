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
                //string query = "INSERT INTO Drugs (DrugID, Name, Administration, Specification, Supply) VALUES (@DrugID, @Name, @Administration, @Specification, @Supply)";
                string query = "INSERT INTO Drugs (Name, Administration, Specification, Supply) VALUES (@Name, @Administration, @Specification, @Supply)";
                SqlCommand command = new SqlCommand(query, connection);
                //command.Parameters.AddWithValue("@DrugID", drug.DrugID);
                command.Parameters.AddWithValue("@Name", drug.Name);
                command.Parameters.AddWithValue("@Administration", drug.Administration);
                command.Parameters.AddWithValue("@Specification", drug.Specification);
                command.Parameters.AddWithValue("@Supply", drug.Supply);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool UpdateDrug(Drug drug)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    //string query = "UPDATE Drugs SET Name = @Name, Administration = @Administration, Specification = @Specification, Supply = @Supply WHERE DrugID = @DrugID";
                    string query = "UPDATE Drugs SET Name = @Name, Administration = @Administration, Specification = @Specification, Supply = @Supply WHERE DrugID = @DrugID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", drug.Name);
                    command.Parameters.AddWithValue("@Administration", drug.Administration);
                    command.Parameters.AddWithValue("@Specification", drug.Specification);
                    command.Parameters.AddWithValue("@Supply", drug.Supply);
                    command.Parameters.AddWithValue("@DrugID", drug.DrugID);

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
        public bool DeleteDrug(int drugID)
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

        public bool DoesDrugExist(int drugID)
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

        public List<Drug> GetDrugs()
        {
            List<Drug> drugs = new List<Drug>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Drugs";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Drug drug = new Drug
                    {
                        DrugID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Administration = reader.GetString(2),
                        Supply = reader.GetInt32(3),
                        Specification = reader.GetString(4)
                    };
                    drugs.Add(drug);
                }
            }
            return drugs;
        }
    }
}