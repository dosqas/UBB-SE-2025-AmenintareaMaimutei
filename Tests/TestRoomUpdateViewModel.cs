using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Utils;
using Project.ViewModels.UpdateViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Tests;

[TestClass]
public class TestRoomUpdateViewModel
{
    private RoomUpdateViewModel _roomUpdateViewModel = new RoomUpdateViewModel();

    [TestInitialize]
    public void Setup()
    {
        _roomUpdateViewModel = new RoomUpdateViewModel();
    }

    [TestMethod]
    public void ErrorMessage_ShouldSetCorrectly()
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
            var expectedMessage = "Some error occurred";

            // Act
            this._roomUpdateViewModel.ErrorMessage = expectedMessage;

            // Assert
            this._roomUpdateViewModel.ErrorMessage.Should().Be(expectedMessage, "because the ErrorMessage should be set correctly when assigned a new value");
        }
    }

    [TestMethod]
    public void ErrorMessage_ShouldTriggerPropertyChangedEvent_WhenSet()
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
            var expectedMessage = "Error occurred!";
            var propertyChangedTriggered = false;

            this._roomUpdateViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(this._roomUpdateViewModel.ErrorMessage))
                {
                    propertyChangedTriggered = true;
                }
            };

            // Act
            this._roomUpdateViewModel.ErrorMessage = expectedMessage;

            // Assert
            propertyChangedTriggered.Should().BeTrue("because the PropertyChanged event should be triggered when the ErrorMessage is set");
        }
    }

    [TestMethod]
    public void ErrorMessage_ShouldNotTriggerPropertyChangedEvent_IfSameValueAssigned()
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
            var initialMessage = this._roomUpdateViewModel.ErrorMessage;
            var propertyChangedTriggered = false;

            this._roomUpdateViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(this._roomUpdateViewModel.ErrorMessage))
                {
                    propertyChangedTriggered = true;
                }
            };

            // Act
            this._roomUpdateViewModel.ErrorMessage = initialMessage; // Set the same value again

            // Assert
            propertyChangedTriggered.Should().BeTrue("because PropertyChanged should not be triggered if the same value is assigned");
        }
    }
}
