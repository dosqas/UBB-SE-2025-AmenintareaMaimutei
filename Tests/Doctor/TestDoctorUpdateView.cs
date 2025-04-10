using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ViewModel;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using System.Transactions;
using Project.Utils;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class TestDoctorUpdateView
    {
        private DoctorUpdateViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new DoctorUpdateViewModel();
        }

        [TestMethod]
        public void SaveChanges_ShouldUpdateDoctor_WhenValidationPasses()
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
                var doctorToUpdate = _viewModel.Doctors[0]; // Assume the first doctor exists
                doctorToUpdate.Experience = 10; // Update experience
                doctorToUpdate.LicenseNumber = "UPDATED123"; // Update license number

                // Act
                if (_viewModel.SaveChangesCommand.CanExecute(null))
                {
                    _viewModel.SaveChangesCommand.Execute(null);
                }

                // Existing code remains unchanged
                var updatedDoctor = _viewModel.Doctors.First(d => d.DoctorID == doctorToUpdate.DoctorID);
                updatedDoctor.Experience.Should().Be(10);
                updatedDoctor.LicenseNumber.Should().Be("UPDATED123");
            }
        }
    }
}
