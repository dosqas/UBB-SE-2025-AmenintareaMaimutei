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
public class TestDepartmentDeleteView
{
    private DepartmentDeleteViewModel _departmentDeleteViewModel = new DepartmentDeleteViewModel();

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
            _departmentDeleteViewModel = new DepartmentDeleteViewModel();
            _departmentDeleteViewModel.Departments.Should().NotBeNull();
            _departmentDeleteViewModel.Departments.Count.Should().Be(10);
            _departmentDeleteViewModel.DepartmentID.Should().Be(0);
            _departmentDeleteViewModel.ErrorMessage.Should().Be(string.Empty);
            _departmentDeleteViewModel.MessageColor.Should().Be("Red");
            _departmentDeleteViewModel.DeleteDepartmentCommand.Should().NotBeNull();
        }
    }

    [TestMethod]
    public void DeleteDepartmentCommand_ShouldDeleteDepartment_WhenValidID()
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
            _departmentDeleteViewModel.DepartmentID = 1;
            _departmentDeleteViewModel.DeleteDepartmentCommand.Execute(null);
            _departmentDeleteViewModel.ErrorMessage.Should().Be("Department deleted successfully");
            _departmentDeleteViewModel.MessageColor.Should().Be("Green");
        }
    }

    [TestMethod]
    public void DeleteDepartmentCommand_ShouldNotDeleteDepartment_WhenInvalidID()
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
            _departmentDeleteViewModel.DepartmentID = 999; // Invalid ID
            _departmentDeleteViewModel.DeleteDepartmentCommand.Execute(null);
            _departmentDeleteViewModel.ErrorMessage.Should().Be("DepartmentID doesn't exist in the records");
            _departmentDeleteViewModel.MessageColor.Should().Be("Red");
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
            var propertyChanged = false;
            _departmentDeleteViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_departmentDeleteViewModel.DepartmentID))
                {
                    propertyChanged = true;
                }
            };
            _departmentDeleteViewModel.DepartmentID = 1;
            propertyChanged.Should().BeTrue();
        }
    }
}
