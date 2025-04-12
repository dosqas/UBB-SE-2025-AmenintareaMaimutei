using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ViewModels.DeleteViewModels;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using System.Transactions;
using Project.Utils;

namespace Tests
{
    [TestClass]
    public class TestDoctorDeleteView
    {
        private DoctorDeleteViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new DoctorDeleteViewModel();
        }

        [TestMethod]
        public void DeleteDoctor_ShouldRemoveDoctor_WhenDoctorExists()
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
                int doctorIDToDelete = 1; // Assume this DoctorID exists
                _viewModel.DoctorID = doctorIDToDelete;

                // Act
                if (_viewModel.DeleteDoctorCommand.CanExecute(null))
                {
                    _viewModel.DeleteDoctorCommand.Execute(null);
                }

                // Assert
                _viewModel.Doctors.Should().NotContain(d => d.DoctorID == doctorIDToDelete);
            }
        }
    }
}
