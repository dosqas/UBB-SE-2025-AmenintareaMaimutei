using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ClassModels;
using Project.Models;
using Project.Utils;
using Project.ViewModels.DeleteViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Tests;

[TestClass]
public class TestRoomDeleteViewModel
{
    private RoomDeleteViewModel _roomDeleteViewModel = new RoomDeleteViewModel();

    [TestInitialize]
    public void Setup()
    {
        _roomDeleteViewModel = new RoomDeleteViewModel();
    }

    [TestMethod]
    public void RoomID_ShouldUpdateCanDeleteRoomProperty_WhenSet()
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

            // Act
            this._roomDeleteViewModel.RoomID = 10;

            // Assert
            this._roomDeleteViewModel.CanDeleteRoom.Should().BeTrue("because the RoomID is greater than 0");
        }
    }

    [TestMethod]
    public void ErrorMessage_ShouldChangeColor_WhenSet()
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

            // Act
            this._roomDeleteViewModel.ErrorMessage = "Room deleted successfully";

            // Assert
            this._roomDeleteViewModel.MessageColor.Should().Be("Green", "because the message indicates success");

            // Act
            this._roomDeleteViewModel.ErrorMessage = "Failed to delete room";

            // Assert
            this._roomDeleteViewModel.MessageColor.Should().Be("Red", "because the message indicates failure");
        }
    }

    [TestMethod]
    public void CanDeleteRoom_ShouldReturnFalse_WhenRoomIDIsZero()
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

            // Act
            this._roomDeleteViewModel.RoomID = 0;

            // Assert
            this._roomDeleteViewModel.CanDeleteRoom.Should().BeFalse("because a valid RoomID is required to delete a room");
        }
    }

    [TestMethod]
    public void RemoveRoom_ShouldSetErrorMessage_WhenNoRoomIsSelected()
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
            this._roomDeleteViewModel.RoomID = 0;

            // Act
            this._roomDeleteViewModel.DeleteRoomCommand.Execute(null);

            // Assert
            this._roomDeleteViewModel.ErrorMessage.Should().Be("No room was selected", "because RoomID is zero and no room is selected");
        }
    }

    [TestMethod]
    public void RemoveRoom_ShouldUpdateRooms_WhenRoomIsDeleted()
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
            RoomModel roomModel = new RoomModel();
            var newRoom = new Room { RoomID = 1001, Capacity = 15, DepartmentID = 1, EquipmentID = 2 };
            roomModel.AddRoom(newRoom);
            this._roomDeleteViewModel.RoomID = newRoom.RoomID;

            // Act
            this._roomDeleteViewModel.DeleteRoomCommand.Execute(null);

            // Assert
            // Ensure the room list is updated (i.e., the room was removed)
            this._roomDeleteViewModel.Rooms.Should().NotContain(r => r.RoomID == newRoom.RoomID, "because the room should be removed after deletion");
        }
    }
}
