using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ViewModels.AddViewModels;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using System.Transactions;
using Project.Utils;

namespace Tests
{
    [TestClass]
    public class TestDrugAddView
    {
        private DrugAddViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new DrugAddViewModel();
        }

        [TestMethod]
        public void SaveDrug_ShouldAddDrug_WhenValidationPasses()
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
                }

                // Arrange
                _viewModel.Name = "Paracetamol";
                _viewModel.Administration = "Oral";
                _viewModel.Specification = "500mg";
                _viewModel.Supply = 100;

                // Act
                if (_viewModel.SaveDrugCommand.CanExecute(null))
                {
                    _viewModel.SaveDrugCommand.Execute(null);
                }

                // Assert
                _viewModel.ErrorMessage.Should().Be("Drug added successfully");
                _viewModel.Drugs.Should().NotBeNull();
                _viewModel.Drugs.Count.Should().BeGreaterThan(0);
                _viewModel.Drugs[0].Name.Should().Be("Paracetamol");
            }
        }

    }
}