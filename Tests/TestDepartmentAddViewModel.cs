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
public class TestDepartmentAddViewModel
{
    private DepartmentAddViewModel _departmentAddViewModel = new DepartmentAddViewModel();

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
            _departmentAddViewModel = new DepartmentAddViewModel();
            _departmentAddViewModel.Departments.Should().NotBeNull();
            _departmentAddViewModel.Departments.Count.Should().Be(10);
            _departmentAddViewModel.Name.Should().Be(string.Empty);
            _departmentAddViewModel.ErrorMessage.Should().Be(string.Empty);
            _departmentAddViewModel.SaveDepartmentCommand.Should().NotBeNull();
        }
    }

    [TestMethod]
    public void SaveDepartment_ShouldAddDepartment_WhenValidData()
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
            _departmentAddViewModel.Name = "Test Department";
            _departmentAddViewModel.SaveDepartmentCommand.Execute(null);
            _departmentAddViewModel.Departments.Count.Should().Be(11);
        }
    }

    [TestMethod]
    public void SaveDepartment_ShouldNotAddDepartment_WhenInvalidData()
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
            _departmentAddViewModel.Name = string.Empty;
            _departmentAddViewModel.SaveDepartmentCommand.Execute(null);
            _departmentAddViewModel.Departments.Count.Should().Be(10);
        }
    }

    [TestMethod]
    public void OnPropertyChanged_ShouldNotifyPropertyChange()
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
            var propertyChangedCalled = false;
            _departmentAddViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_departmentAddViewModel.Name))
                {
                    propertyChangedCalled = true;
                }
            };
            _departmentAddViewModel.Name = "New Name";
            propertyChangedCalled.Should().BeTrue();
        }
    }
}
