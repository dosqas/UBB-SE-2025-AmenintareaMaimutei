using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ViewModels.DeleteViewModels;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using System.Transactions;
using Project.Utils;

namespace Tests
{
    [TestClass]
    public class TestDrugDeleteView
    {
        private DrugDeleteViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new DrugDeleteViewModel();
        }

        [TestMethod]
        public void DeleteDrug_ShouldRemoveDrug_WhenDrugExists()
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();

                    // Reset the database
                    using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Delete existing data
                    using (var command = new SqlCommand("EXEC DeleteData", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert initial data
                    using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert test data
                    using (var command = new SqlCommand("EXEC InsertData @nrOfRows", connection))
                    {
                        command.Parameters.AddWithValue("@nrOfRows", 10);
                        command.ExecuteNonQuery();
                    }
                }

                // Arrange
                int drugIDToDelete = 1; // Assume this DrugID exists
                _viewModel.DrugID = drugIDToDelete;

                // Act
                if (_viewModel.DeleteDrugCommand.CanExecute(null))
                {
                    _viewModel.DeleteDrugCommand.Execute(null);
                }

                // Assert
                _viewModel.Drugs.Should().NotContain(d => d.DrugID == drugIDToDelete);
                _viewModel.ErrorMessage.Should().Be("Drug deleted successfully");
            }
        }
    }
}
