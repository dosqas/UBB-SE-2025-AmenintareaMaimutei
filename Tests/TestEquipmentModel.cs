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
public class TestEquipmentModel
{
    private EquipmentModel _equipmentModel = new EquipmentModel();

    [TestInitialize]
    public void Setup()
    {
        _equipmentModel = new EquipmentModel();
    }

    [TestMethod]
    public void AddEquipment_ShouldReturnTrue_WhenInsertSucceeds()
    {
        using (var scope = new TransactionScope())
        {
            var equipment = new Equipment { Name = "Test", Type = "Type", Specification = "Spec", Stock = 5 };
            var result = _equipmentModel.AddEquipment(equipment);
            result.Should().BeTrue();
        }

    }

    [TestMethod]
    public void UpdateEquipment_ShouldReturnTrue_WhenUpdateSucceeds()
    {
        using (var scope = new TransactionScope())
        {
            var equipment = new Equipment { EquipmentID = 11, Name = "Test", Type = "Type", Specification = "Spec", Stock = 5 };
            var result = _equipmentModel.UpdateEquipment(equipment);
            result.Should().BeTrue();
        }
    }

    [TestMethod]
    public void UpdateEquipment_ShouldReturnFalse_WhenExceptionOccurs()
    {
        using (var scope = new TransactionScope())
        {
            var equipment = new Equipment { EquipmentID = -1 };
            var result = _equipmentModel.UpdateEquipment(equipment);
            result.Should().BeFalse();
        }
    }

    [TestMethod]
    public void DeleteEquipment_ShouldReturnTrue_WhenDeleteSucceeds()
    {
        using (var scope = new TransactionScope())
        {
            // Reset the database to a known state
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                using (var command = new SqlCommand(DatabaseHelper.GetResetProcedureSql(), connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            var result = _equipmentModel.DeleteEquipment(2);
            result.Should().BeTrue();
        }
    }

    [TestMethod]
    public void DoesEquipmentExist_ShouldReturnTrue_WhenEquipmentExists()
    {
        using (var scope = new TransactionScope())
        {
            var result = _equipmentModel.DoesEquipmentExist(5);
            result.Should().BeTrue();
        }
    }

    [TestMethod]
    public void GetEquipments_ShouldReturnEquipmentList_WhenDataExists()
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
            var result = _equipmentModel.GetEquipments();
            result.Should().NotBeNull();
            result.Count.Should().Be(10);
            result[0].EquipmentID.Should().Be(1);
        }
    }
}
