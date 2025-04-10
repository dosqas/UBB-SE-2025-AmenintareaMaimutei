using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ClassModels;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using System;
using Project.Utils;
using System.Transactions;
using DrugNamespace = Project.Models;

namespace Tests
{
    [TestClass]
    public class TestDrugClassModel
    {
        private DrugModel _drugModel = new DrugModel();

        [TestInitialize]
        public void Setup()
        {
            _drugModel = new DrugModel();
        }

        [TestMethod]
        public void AddDrug_ShouldReturnTrue_WhenInsertSucceeds()
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

                    // No insert needed here for Add
                }

                var drug = new DrugNamespace.Drug
                {
                    DrugID = 0,
                    Name = "Paracetamol",
                    Administration = "Oral",
                    Specification = "500mg",
                    Supply = 100
                };

                var result = _drugModel.AddDrug(drug);
                result.Should().BeTrue();
            }
        }

        [TestMethod]
        public void UpdateDrug_ShouldReturnTrue_WhenUpdateSucceeds()
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

                var drug = new DrugNamespace.Drug
                {
                    DrugID = 1,
                    Name = "Ibuprofen",
                    Administration = "Oral",
                    Specification = "200mg",
                    Supply = 50
                };

                var result = _drugModel.UpdateDrug(drug);
                result.Should().BeTrue();
            }
        }

        [TestMethod]
        public void UpdateDrug_ShouldReturnFalse_WhenExceptionOccurs()
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
                }

                var drug = new DrugNamespace.Drug
                {
                    DrugID = -1
                };

                var result = _drugModel.UpdateDrug(drug);
                result.Should().BeFalse();
            }
        }

        [TestMethod]
        public void DeleteDrug_ShouldReturnTrue_WhenDeleteSucceeds()
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

                var result = _drugModel.DeleteDrug(1);
                result.Should().BeTrue();
            }
        }

        [TestMethod]
        public void DoesDrugExist_ShouldReturnTrue_WhenDrugExists()
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

                var result = _drugModel.DoesDrugExist(1);
                result.Should().BeTrue();
            }
        }

        [TestMethod]
        public void GetDrugs_ShouldReturnDrugList_WhenDataExists()
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

                var result = _drugModel.GetDrugs();
                result.Should().NotBeNull();
                result.Count.Should().Be(10);
                result[0].DrugID.Should().Be(1);
            }
        }
    }
}
