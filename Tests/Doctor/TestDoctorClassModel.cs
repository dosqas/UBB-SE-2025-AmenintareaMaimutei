using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ClassModels;
using FluentAssertions;
using Project.Models;
using Microsoft.Data.SqlClient;
using System;
using Project.Utils;
using System.Transactions;

namespace Tests
{
    [TestClass]
    public class TestDoctorClassModel
    {
        private DoctorModel _doctorModel = new DoctorModel();

        [TestInitialize]
        public void Setup()
        {
            _doctorModel = new DoctorModel();
        }

        [TestMethod]
        public void AddDoctor_ShouldReturnTrue_WhenInsertSucceeds()
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

                    // Enable IDENTITY_INSERT for the Users table
                    using (var command = new SqlCommand("SET IDENTITY_INSERT Users ON", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Ensure a user with UserID = 1 exists, including values for Name, RegistrationDate, Mail, Password, Birthdate, Cnp, Address, and PhoneNumber
                    using (var command = new SqlCommand("INSERT INTO Users (UserID, UserName, Name, Role, RegistrationDate, Mail, Password, Birthdate, Cnp, Address, PhoneNumber) VALUES (1, 'TestUser', 'John Doe', 'Doctor', GETDATE(), 'testuser@example.com', 'TestPassword123', '1980-01-01', '1234567890123', '123 Main St, Springfield', '0757903540')", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Disable IDENTITY_INSERT for the Users table
                    using (var command = new SqlCommand("SET IDENTITY_INSERT Users OFF", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Enable IDENTITY_INSERT for the Departments table
                    using (var command = new SqlCommand("SET IDENTITY_INSERT Departments ON", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Ensure a department with DepartmentID = 2 exists
                    using (var command = new SqlCommand("INSERT INTO Departments (DepartmentID, Name) VALUES (2, 'Cardiology')", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Disable IDENTITY_INSERT for the Departments table
                    using (var command = new SqlCommand("SET IDENTITY_INSERT Departments OFF", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                // Create a doctor object with a unique DoctorID
                var doctor = new Doctor
                {
                    DoctorID = 1, // Ensure this value is unique
                    UserID = 1,   // Ensure this matches the UserID inserted above
                    DepartmentID = 2, // Ensure this matches the DepartmentID inserted above
                    Experience = 10,
                    Rating = 4.5f,
                    LicenseNumber = "ABC123"
                };

                // Attempt to add the doctor
                var result = _doctorModel.AddDoctor(doctor);
                result.Should().BeTrue();
            }
        }

        [TestMethod]
        public void UpdateDoctor_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();

                    using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand("EXEC DeleteData", connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand("EXEC InsertData @nrOfRows", connection))
                    {
                        command.Parameters.AddWithValue("@nrOfRows", 10);
                        command.ExecuteNonQuery();
                    }
                }

                var doctor = new Doctor
                {
                    DoctorID = 1,
                    UserID = 1,
                    DepartmentID = 2,
                    Experience = 15,
                    Rating = 4.8f,
                    LicenseNumber = "XYZ789"
                };

                var result = _doctorModel.UpdateDoctor(doctor);
                result.Should().BeTrue();
            }
        }

        [TestMethod]
        public void UpdateDoctor_ShouldReturnFalse_WhenExceptionOccurs()
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();

                    using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand("EXEC DeleteData", connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                        command.ExecuteNonQuery();
                }

                var doctor = new Doctor { DoctorID = -1 };

                var result = _doctorModel.UpdateDoctor(doctor);
                result.Should().BeFalse();
            }
        }

        [TestMethod]
        public void DeleteDoctor_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();

                    using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand("EXEC DeleteData", connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand("EXEC InsertData @nrOfRows", connection))
                    {
                        command.Parameters.AddWithValue("@nrOfRows", 10);
                        command.ExecuteNonQuery();
                    }
                }

                var result = _doctorModel.DeleteDoctor(1);
                result.Should().BeTrue();
            }
        }

        [TestMethod]
        public void DoesDoctorExist_ShouldReturnTrue_WhenDoctorExists()
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();

                    using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand("EXEC DeleteData", connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand("EXEC InsertData @nrOfRows", connection))
                    {
                        command.Parameters.AddWithValue("@nrOfRows", 10);
                        command.ExecuteNonQuery();
                    }
                }

                var result = _doctorModel.DoesDoctorExist(1);
                result.Should().BeTrue();
            }
        }

        [TestMethod]
        public void GetDoctors_ShouldReturnDoctorList_WhenDataExists()
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();

                    using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand("EXEC DeleteData", connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand(DatabaseHelper.GetInsertDataProcedureSql(), connection))
                        command.ExecuteNonQuery();

                    using (var command = new SqlCommand("EXEC InsertData @nrOfRows", connection))
                    {
                        command.Parameters.AddWithValue("@nrOfRows", 10);
                        command.ExecuteNonQuery();
                    }
                }

                var result = _doctorModel.GetDoctors();
                result.Should().NotBeNull();
                result.Count.Should().Be(10);
                result[0].DoctorID.Should().Be(1);
            }
        }
    }
}
