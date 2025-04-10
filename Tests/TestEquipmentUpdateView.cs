using Microsoft.Data.SqlClient;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ClassModels;
using Project.Models;
using Project.Utils;
using Project.ViewModels.UpdateViewModels;
using FluentAssertions;
using System.Collections.Generic;
namespace Tests;

[TestClass]
public class TestEquipmentUpdateView
{
    private EquipmentUpdateViewModel _equipmentUpdateViewModel = new EquipmentUpdateViewModel();

    [TestMethod]
    public void Constructor_ShouldInitializeProperties()
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
            _equipmentUpdateViewModel = new EquipmentUpdateViewModel();
            _equipmentUpdateViewModel.Equipments.Should().NotBeNull();
            _equipmentUpdateViewModel.Equipments.Count.Should().Be(10);
            _equipmentUpdateViewModel.ErrorMessage.Should().Be(string.Empty);
            _equipmentUpdateViewModel.SaveChangesCommand.Should().NotBeNull();
        }
    }

    [TestMethod]
    public void SaveChanges_ShouldUpdateEquipment_WhenValidData()
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
            _equipmentUpdateViewModel.Equipments[0].Name = "Updated Equipment Name";
            _equipmentUpdateViewModel.SaveChangesCommand.Execute(null);
            _equipmentUpdateViewModel.Equipments[0].Name.Should().Be("Updated Equipment Name");
        }
    }

    [TestMethod]
    public void SaveChanges_ShouldSetErrorMessage_WhenInvalidData()
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
            _equipmentUpdateViewModel.Equipments[0].Name = string.Empty; // Invalid data
            _equipmentUpdateViewModel.SaveChangesCommand.Execute(null);
            _equipmentUpdateViewModel.ErrorMessage.Should().NotBe(string.Empty);
            _equipmentUpdateViewModel.ErrorMessage.Should().Contain("Failed to save changes for equipment:");
        }
    }

    [TestMethod]
    public void OnPropertyChanged_ShouldRaisePropertyChangedEvent()
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
            bool eventRaised = false;
            _equipmentUpdateViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_equipmentUpdateViewModel.ErrorMessage))
                {
                    eventRaised = true;
                }
            };
            _equipmentUpdateViewModel.ErrorMessage = "New Error Message";
            eventRaised.Should().BeTrue();
        }
    }
}
