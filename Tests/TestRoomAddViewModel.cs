using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Models;
using FluentAssertions;
using Project.ViewModels.AddViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Project.Utils;
using System.Transactions;

namespace Tests;

[TestClass]
public class TestRoomAddViewModel
{
    private RoomAddViewModel _roomAddViewModel = new RoomAddViewModel();

    [TestInitialize]
    public void Setup()
    {
        _roomAddViewModel = new RoomAddViewModel();
    }

    [TestMethod]
    public void Capacity_ShouldSetCorrectValue_WhenAssigned()
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
            // Arrange
            var room = new Room();

            // Act
            room.Capacity = 25;

            // Assert
            room.Capacity.Should().Be(25);  // Assert that the Capacity is correctly set
        }
    }

    [TestMethod]
    public void DepartmentID_ShouldSetCorrectValue_WhenAssigned()
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
            // Arrange
            var room = new Room();

            // Act
            room.DepartmentID = 10;

            // Assert
            room.DepartmentID.Should().Be(10, "because the DepartmentID should be correctly set to the assigned value.");
        }
    }

    [TestMethod]
    public void EquipmentID_ShouldSetCorrectValue_WhenAssigned()
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
            // Arrange
            var room = new Room();

            // Act
            room.EquipmentID = 10;

            // Assert
            room.EquipmentID.Should().Be(10, "because the EquipmentID should be correctly set to the assigned value.");
        }
    }
}
