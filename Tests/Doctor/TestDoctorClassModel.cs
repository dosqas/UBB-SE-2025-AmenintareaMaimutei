using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ClassModels;
using FluentAssertions;
using Project.Models;
using Microsoft.Data.SqlClient;
using System;
using Project.Utils;
using System.Transactions;

namespace Tests;

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
            var doctor = new Doctor
            {
                DoctorID = 0,
                UserID = 1,
                DepartmentID = 2,
                Experience = 10,
                Rating = 4.5f,
                LicenseNumber = "ABC123"
            };
            var result = _doctorModel.AddDoctor(doctor);
            result.Should().BeTrue();
        }
    }

    [TestMethod]
    public void UpdateDoctor_ShouldReturnTrue_WhenUpdateSucceeds()
    {
        using (var scope = new TransactionScope())
        {
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
                {
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
            var result = _doctorModel.GetDoctors();
            result.Should().NotBeNull();
            result.Count.Should().Be(10);
            result[0].DoctorID.Should().Be(1);
        }
    }
}
