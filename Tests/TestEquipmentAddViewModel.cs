using Microsoft.Data.SqlClient;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ClassModels;
using Project.Models;
using Project.Utils;
using Project.ViewModels.AddViewModels;
using FluentAssertions;
using System.Collections.Generic;

namespace Tests;

[TestClass]
public class TestEquipmentAddViewModel
{
    private EquipmentAddViewModel _equipmentAddViewModel = new EquipmentAddViewModel();

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
            _equipmentAddViewModel = new EquipmentAddViewModel();
            _equipmentAddViewModel.Equipments.Should().NotBeNull();
            _equipmentAddViewModel.Equipments.Count.Should().Be(10);
            _equipmentAddViewModel.Name.Should().Be(string.Empty);
            _equipmentAddViewModel.Type.Should().Be(string.Empty);
            _equipmentAddViewModel.Specification.Should().Be(string.Empty);
            _equipmentAddViewModel.Stock.Should().Be(0);
            _equipmentAddViewModel.ErrorMessage.Should().Be(string.Empty);
            _equipmentAddViewModel.SaveEquipmentCommand.Should().NotBeNull();

        }
    }

    [TestMethod]
    public void SaveEquipment_ShouldAddEquipment_WhenValidData()
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
            _equipmentAddViewModel = new EquipmentAddViewModel();
            _equipmentAddViewModel.Name = "Test";
            _equipmentAddViewModel.Type = "Type";
            _equipmentAddViewModel.Specification = "Spec";
            _equipmentAddViewModel.Stock = 5;
            _equipmentAddViewModel.SaveEquipmentCommand.Execute(null);
            var equipments = _equipmentAddViewModel.Equipments;
            equipments.Should().NotBeNull();
            equipments.Count.Should().Be(11);
        }
    }

    [TestMethod]
    public void SaveEquipment_ShouldNotAddEquipment_WhenInvalidData()
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
            _equipmentAddViewModel = new EquipmentAddViewModel();
            _equipmentAddViewModel.Name = string.Empty;
            _equipmentAddViewModel.Type = string.Empty;
            _equipmentAddViewModel.Specification = string.Empty;
            _equipmentAddViewModel.Stock = -5;
            _equipmentAddViewModel.SaveEquipmentCommand.Execute(null);
            var equipments = _equipmentAddViewModel.Equipments;
            equipments.Should().NotBeNull();
            equipments.Count.Should().Be(10);
        }
    }

    [TestMethod]
    public void OnPropertyChange_ShouldRaisePropertyChangedEvent()
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
            var changedProperties = new List<string>();
            _equipmentAddViewModel = new EquipmentAddViewModel();
            _equipmentAddViewModel.PropertyChanged += (sender, args) => changedProperties.Add(args.PropertyName);
            
            _equipmentAddViewModel.Name = "Test";
            _equipmentAddViewModel.Type = "Type";
            _equipmentAddViewModel.Specification = "Spec";
            _equipmentAddViewModel.Stock = 5;
            _equipmentAddViewModel.ErrorMessage = "Error";
            
            changedProperties.Should().Contain(new List<string>
            {
                nameof(_equipmentAddViewModel.Name),
                nameof(_equipmentAddViewModel.Type),
                nameof(_equipmentAddViewModel.Specification),
                nameof(_equipmentAddViewModel.Stock),
                nameof(_equipmentAddViewModel.ErrorMessage)
            });
        }
    }
}
