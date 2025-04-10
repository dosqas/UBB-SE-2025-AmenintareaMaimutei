using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ViewModels.UpdateViewModels;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using System.Transactions;
using Project.Utils;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class TestDrugUpdateView
    {
        private DrugUpdateViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new DrugUpdateViewModel();
        }

        [TestMethod]
        public void SaveChanges_ShouldUpdateDrug_WhenValidationPasses()
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
                var drugToUpdate = _viewModel.Drugs[0]; // Assume the first drug exists
                drugToUpdate.Name = "UpdatedDrugName"; // Update name
                drugToUpdate.Administration = "UpdatedAdministration"; // Update administration
                drugToUpdate.Specification = "UpdatedSpecification"; // Update specification
                drugToUpdate.Supply = 200; // Update supply

                // Act
                if (_viewModel.SaveChangesCommand.CanExecute(null))
                {
                    _viewModel.SaveChangesCommand.Execute(null);
                }

                // Assert
                var updatedDrug = _viewModel.Drugs.First(d => d.DrugID == drugToUpdate.DrugID);
                updatedDrug.Name.Should().Be("UpdatedDrugName");
                updatedDrug.Administration.Should().Be("UpdatedAdministration");
                updatedDrug.Specification.Should().Be("UpdatedSpecification");
                updatedDrug.Supply.Should().Be(200);
            }
        }
    }
}
