using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Data.SqlClient;
using System.Transactions;
using Project.ClassModels;
using Project.Models;
using Project.Utils;
using Project.ViewModels.AddViewModels;
using FluentAssertions;
using System.Collections.Generic;
using Project.ViewModels.DeleteViewModels;
namespace Tests;


[TestClass]
public class TestEquipmentDeleteView
{
    private EquipmentDeleteViewModel _equipmentDeleteViewModel = new EquipmentDeleteViewModel();

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
            _equipmentDeleteViewModel = new EquipmentDeleteViewModel();
            _equipmentDeleteViewModel.Equipments.Should().NotBeNull();
            _equipmentDeleteViewModel.Equipments.Count.Should().Be(10);
            _equipmentDeleteViewModel.EquipmentID.Should().Be(0);
            _equipmentDeleteViewModel.ErrorMessage.Should().Be(string.Empty);
            _equipmentDeleteViewModel.MessageColor.Should().Be("Red");
            _equipmentDeleteViewModel.DeleteEquipmentCommand.Should().NotBeNull();
        }
    }

    [TestMethod]
    public void DeleteEquipmentCommand_ShouldDeleteEquipment_WhenValidID()
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
            _equipmentDeleteViewModel.EquipmentID = 1;
            _equipmentDeleteViewModel.DeleteEquipmentCommand.Execute(null);
            _equipmentDeleteViewModel.ErrorMessage.Should().Be("Equipment deleted successfully");
            _equipmentDeleteViewModel.MessageColor.Should().Be("Green");
        }
    }

    [TestMethod]
    public void DeleteEquipmentCommand_ShouldNotDeleteEquipment_WhenInvalidID()
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
            _equipmentDeleteViewModel.EquipmentID = -1;
            _equipmentDeleteViewModel.DeleteEquipmentCommand.Execute(null);
            _equipmentDeleteViewModel.ErrorMessage.Should().Be("EquipmentID doesn't exist in the records");
            _equipmentDeleteViewModel.MessageColor.Should().Be("Red");
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
            var propertyChanged = false;
            _equipmentDeleteViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_equipmentDeleteViewModel.EquipmentID))
                {
                    propertyChanged = true;
                }
            };
            _equipmentDeleteViewModel.EquipmentID = 1;
            propertyChanged.Should().BeTrue();
        }
    }

}
