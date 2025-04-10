using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ViewModels.AddViewModels;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using System.Transactions;
using Project.Utils;

namespace Tests
{
    [TestClass]
    public class TestDoctorAddView
    {
        private DoctorAddViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new DoctorAddViewModel();
        }

        [TestMethod]
        public void SaveDoctor_ShouldAddDoctor_WhenValidationPasses()
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
                _viewModel.UserID = 1; // Assume this UserID exists and is valid
                _viewModel.DepartmentID = 2; // Assume this DepartmentID exists
                _viewModel.Experience = 5;
                _viewModel.LicenseNumber = "VALID123";

                // Act
                if (_viewModel.SaveDoctorCommand.CanExecute(null))
                {
                    _viewModel.SaveDoctorCommand.Execute(null);
                }

                // Assert

                _viewModel.Doctors.Should().NotBeNull();
                _viewModel.Doctors.Count.Should().BeGreaterThan(0);
                _viewModel.Doctors[0].UserID.Should().Be(1);
            }
        }
    }
}
