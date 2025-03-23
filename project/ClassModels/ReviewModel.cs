using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml.Controls;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.ClassModels
{
    class ReviewModel
    {
        private readonly string _connectionString = DatabaseHelper.GetConnectionString();

        public Review? FetchReview(int medicalRecordID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Reviews WHERE MedicalRecordID = @MedicalRecordID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MedicalRecordID", medicalRecordID);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int reviewID = reader.GetInt32(reader.GetOrdinal("ReviewID"));
                        int medicalRecordIDFromDb = reader.GetInt32(reader.GetOrdinal("MedicalRecordID"));
                        string text = reader.GetString(reader.GetOrdinal("Text"));
                        int nrStars = reader.GetInt32(reader.GetOrdinal("NrStars"));

                        return new Review(reviewID, medicalRecordIDFromDb, text, nrStars);
                    }
                    else
                    {
                        return null;
                    }
                }
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
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return null;
            }
        }

        public bool AddReview(Review review)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    //string query = "INSERT INTO Reviews (ReviewID, MedicalRecordID, Text, NrStars) VALUES (@ReviewID, @MedicalRecordID, @Text, @NrStars)";
                    string query = "INSERT INTO Reviews (MedicalRecordID, Text, NrStars) VALUES (@MedicalRecordID, @Text, @NrStars)";
                    SqlCommand command = new SqlCommand(query, connection);
                    //command.Parameters.AddWithValue("@ReviewID", review.ReviewID);
                    command.Parameters.AddWithValue("@MedicalRecordID", review.MedicalRecordID);
                    command.Parameters.AddWithValue("@Text", review.Text);
                    command.Parameters.AddWithValue("@NrStars", review.NrStars);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in AddReview: {ex.Message}");
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid Operation in AddReview: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error in AddReview: {ex.Message}");
                return false;
            }
        }

        public bool RemoveReview(int reviewID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM Reviews WHERE ReviewID = @ReviewID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ReviewID", reviewID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error in RemoveReview: {ex.Message}");
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid Operation in RemoveReview: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error in RemoveReview: {ex.Message}");
                return false;
            }
        }
    }
}
