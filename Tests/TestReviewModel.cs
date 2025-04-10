using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Tests;

[TestClass]
public class TestReviewModel
{
    private ReviewModel _reviewModel = new ReviewModel();

    [TestInitialize]
    public void Setup()
    {
        _reviewModel = new ReviewModel();
    }

    [TestMethod]
    public void FetchReview_ShouldReturnReview_WhenReviewExists()
    {
        using (var scope = new TransactionScope())
        {
            int insertedReviewId;

            // Step 1: Set up the database
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();

                using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand("EXEC DeleteData", connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand("EXEC InsertData @nrOfRows", connection))
                {
                    command.Parameters.AddWithValue("@nrOfRows", 10);
                    command.ExecuteNonQuery();
                }

                // Step 2: Insert a test review and retrieve its generated ID
                using (var insertCmd = new SqlCommand(@"
                INSERT INTO Reviews (MedicalRecordID, Text, NrStars) 
                OUTPUT INSERTED.ReviewID
                VALUES (@MedicalRecordID, @Text, @NrStars)", connection))
                {
                    insertCmd.Parameters.AddWithValue("@MedicalRecordID", 1);
                    insertCmd.Parameters.AddWithValue("@Text", "Test review for FetchReview.");
                    insertCmd.Parameters.AddWithValue("@NrStars", 4);

                    insertedReviewId = (int)insertCmd.ExecuteScalar();
                }
            }

            // Step 3: Call FetchReview and assert result
            var result = _reviewModel.FetchReview(1);

            result.Should().NotBeNull();
            result.ReviewID.Should().Be(insertedReviewId);
            result.MedicalRecordID.Should().Be(1);
            result.Text.Should().Be("Test review for FetchReview.");
            result.NrStars.Should().Be(4);
        }
    }

    [TestMethod]
    public void AddReview_ShouldInsertReview_WhenDataIsValid()
    {
        using (var scope = new TransactionScope())
        {
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();

                using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand("EXEC DeleteData", connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand("EXEC InsertData @nrOfRows", connection))
                {
                    command.Parameters.AddWithValue("@nrOfRows", 10);
                    command.ExecuteNonQuery();
                }
            }

            // Create a new review. Assuming MedicalRecordID 1 exists after InsertData
            var review = new Review(
                reviewID: 0, // Placeholder if ReviewID is auto-incremented
                medicalRecordID: 1,
                text: "Test review from integration test.",
                nrStars: 5
            );

            var result = _reviewModel.AddReview(review);

            result.Should().BeTrue();

            // Optional: verify it was inserted
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();

                using (var verifyCmd = new SqlCommand(@"
                SELECT COUNT(*) FROM Reviews 
                WHERE MedicalRecordID = @MedicalRecordID AND Text = @Text AND NrStars = @NrStars", connection))
                {
                    verifyCmd.Parameters.AddWithValue("@MedicalRecordID", review.MedicalRecordID);
                    verifyCmd.Parameters.AddWithValue("@Text", review.Text);
                    verifyCmd.Parameters.AddWithValue("@NrStars", review.NrStars);

                    int count = (int)verifyCmd.ExecuteScalar();
                    count.Should().BeGreaterThan(0);
                }
            }
        }
    }

    [TestMethod]
    public void RemoveReview_ShouldDeleteReview_WhenReviewExists()
    {
        using (var scope = new TransactionScope())
        {
            int insertedReviewId;

            // Step 1: Set up the database
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();

                using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand("EXEC DeleteData", connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand("EXEC InsertData @nrOfRows", connection))
                {
                    command.Parameters.AddWithValue("@nrOfRows", 10);
                    command.ExecuteNonQuery();
                }

                // Step 2: Insert a test review and retrieve its generated ID
                using (var insertCmd = new SqlCommand(@"
                INSERT INTO Reviews (MedicalRecordID, Text, NrStars) 
                OUTPUT INSERTED.ReviewID
                VALUES (@MedicalRecordID, @Text, @NrStars)", connection))
                {
                    insertCmd.Parameters.AddWithValue("@MedicalRecordID", 1);
                    insertCmd.Parameters.AddWithValue("@Text", "Review to be deleted");
                    insertCmd.Parameters.AddWithValue("@NrStars", 3);

                    insertedReviewId = (int)insertCmd.ExecuteScalar();
                }
            }

            // Step 3: Call RemoveReview and assert result
            var result = _reviewModel.RemoveReview(insertedReviewId);

            result.Should().BeTrue();

            // Step 4: Confirm review no longer exists in DB
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();

                using (var verifyCmd = new SqlCommand("SELECT COUNT(*) FROM Reviews WHERE ReviewID = @ReviewID", connection))
                {
                    verifyCmd.Parameters.AddWithValue("@ReviewID", insertedReviewId);
                    int count = (int)verifyCmd.ExecuteScalar();
                    count.Should().Be(0);
                }
            }
        }
    }
}
