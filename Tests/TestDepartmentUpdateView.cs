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
public class TestDepartmentUpdateView
{
    private DepartmentUpdateViewModel _departmentUpdateViewModel = new DepartmentUpdateViewModel();

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
            _departmentUpdateViewModel = new DepartmentUpdateViewModel();
            _departmentUpdateViewModel.Departments.Should().NotBeNull();
            _departmentUpdateViewModel.Departments.Count.Should().Be(10);
            _departmentUpdateViewModel.ErrorMessage.Should().Be(string.Empty);
            _departmentUpdateViewModel.SaveChangesCommand.Should().NotBeNull();
        }
    }

    [TestMethod]
    public void SaveChanges_ShouldUpdateDepartment_WhenValidData()
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
            _departmentUpdateViewModel.Departments[0].Name = "Updated Department";
            _departmentUpdateViewModel.SaveChangesCommand.Execute(null);
            _departmentUpdateViewModel.ErrorMessage.Should().Be("Changes saved successfully");
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
            _departmentUpdateViewModel.Departments[0].Name = string.Empty;
            _departmentUpdateViewModel.SaveChangesCommand.Execute(null);
            _departmentUpdateViewModel.ErrorMessage.Should().NotBe(string.Empty);
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
            _departmentUpdateViewModel.PropertyChanged += (sender, e) => eventRaised = true;
            _departmentUpdateViewModel.ErrorMessage = "New Error Message";
            eventRaised.Should().BeTrue();
        }
    }

}
