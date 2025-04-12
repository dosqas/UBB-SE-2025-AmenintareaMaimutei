using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Tests;

[TestClass]
public class TestRoomModel
{
    private RoomModel _roomModel = new RoomModel();

    [TestInitialize]
    public void Setup()
    {
        _roomModel = new RoomModel();
    }

    [TestMethod]
    public void AddRoom_ShouldInsertRoom_WhenDataIsValid()
    {
        using (var scope = new TransactionScope())
        {
            // Step 1: Set up the database
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

            // Step 2: Create a new room
            var room = new Room(
                roomID: 0,  // Placeholder since RoomID is auto-incremented
                capacity: 20,
                departmentID: 1,
                equipmentID: 1
            );

            // Step 3: Add the room
            var result = _roomModel.AddRoom(room);

            result.Should().BeTrue();

            // Step 4: Confirm the room is inserted by checking the database
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();

                using (var verifyCmd = new SqlCommand(@"
                SELECT COUNT(*) FROM Rooms 
                WHERE Capacity = @Capacity AND DepartmentID = @DepartmentID AND EquipmentID = @EquipmentID", connection))
                {
                    verifyCmd.Parameters.AddWithValue("@Capacity", room.Capacity);
                    verifyCmd.Parameters.AddWithValue("@DepartmentID", room.DepartmentID);
                    verifyCmd.Parameters.AddWithValue("@EquipmentID", room.EquipmentID);

                    int count = (int)verifyCmd.ExecuteScalar();
                    count.Should().BeGreaterThan(0);
                }
            }
        }
    }

    [TestMethod]
    public void UpdateRoom_ShouldUpdateRoom_WhenDataIsValid()
    {
        using (var scope = new TransactionScope())
        {
            int insertedRoomId;

            // Step 1: Set up the database
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

                // Step 2: Insert a room to be updated
                using (var insertCmd = new SqlCommand(@"
                INSERT INTO Rooms (Capacity, DepartmentID, EquipmentID) 
                OUTPUT INSERTED.RoomID
                VALUES (@Capacity, @DepartmentID, @EquipmentID)", connection))
                {
                    insertCmd.Parameters.AddWithValue("@Capacity", 20);
                    insertCmd.Parameters.AddWithValue("@DepartmentID", 1);
                    insertCmd.Parameters.AddWithValue("@EquipmentID", 1);

                    insertedRoomId = (int)insertCmd.ExecuteScalar();
                }
            }

            // Step 3: Update the room
            var updatedRoom = new Room(
                roomID: insertedRoomId,
                capacity: 30,
                departmentID: 2,
                equipmentID: 2
            );

            var result = _roomModel.UpdateRoom(updatedRoom);

            result.Should().BeTrue();

            // Step 4: Verify the room has been updated
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();

                using (var verifyCmd = new SqlCommand(@"
                SELECT Capacity, DepartmentID, EquipmentID 
                FROM Rooms 
                WHERE RoomID = @RoomID", connection))
                {
                    verifyCmd.Parameters.AddWithValue("@RoomID", insertedRoomId);
                    using (var reader = verifyCmd.ExecuteReader())
                    {
                        reader.Read();
                        reader.GetInt32(reader.GetOrdinal("Capacity")).Should().Be(30);
                        reader.GetInt32(reader.GetOrdinal("DepartmentID")).Should().Be(2);
                        reader.GetInt32(reader.GetOrdinal("EquipmentID")).Should().Be(2);
                    }
                }
            }
        }
    }

    [TestMethod]
    public void DeleteRoom_ShouldDeleteRoom_WhenRoomExists()
    {
        using (var scope = new TransactionScope())
        {
            int insertedRoomId;

            // Step 1: Set up the database
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

                // Step 2: Insert a room to be deleted
                using (var insertCmd = new SqlCommand(@"
                INSERT INTO Rooms (Capacity, DepartmentID, EquipmentID) 
                OUTPUT INSERTED.RoomID
                VALUES (@Capacity, @DepartmentID, @EquipmentID)", connection))
                {
                    insertCmd.Parameters.AddWithValue("@Capacity", 20);
                    insertCmd.Parameters.AddWithValue("@DepartmentID", 1);
                    insertCmd.Parameters.AddWithValue("@EquipmentID", 1);

                    insertedRoomId = (int)insertCmd.ExecuteScalar();
                }
            }

            // Step 3: Delete the room
            var result = _roomModel.DeleteRoom(insertedRoomId);

            result.Should().BeTrue();

            // Step 4: Verify the room has been deleted
            using (var connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();

                using (var verifyCmd = new SqlCommand(@"
                SELECT COUNT(*) 
                FROM Rooms 
                WHERE RoomID = @RoomID", connection))
                {
                    verifyCmd.Parameters.AddWithValue("@RoomID", insertedRoomId);
                    int count = (int)verifyCmd.ExecuteScalar();
                    count.Should().Be(0); // The room should no longer exist
                }
            }
        }
    }

    [TestMethod]
    public void DoesRoomExist_ShouldReturnTrue_WhenRoomExists()
    {
        using (var scope = new TransactionScope())
        {
            int insertedRoomId;

            // Step 1: Set up the database
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

                // Step 2: Insert a room to be checked
                using (var insertCmd = new SqlCommand(@"
                INSERT INTO Rooms (Capacity, DepartmentID, EquipmentID) 
                OUTPUT INSERTED.RoomID
                VALUES (@Capacity, @DepartmentID, @EquipmentID)", connection))
                {
                    insertCmd.Parameters.AddWithValue("@Capacity", 20);
                    insertCmd.Parameters.AddWithValue("@DepartmentID", 1);
                    insertCmd.Parameters.AddWithValue("@EquipmentID", 1);

                    insertedRoomId = (int)insertCmd.ExecuteScalar();
                }
            }

            // Step 3: Check if the room exists
            var result = _roomModel.DoesRoomExist(insertedRoomId);

            result.Should().BeTrue(); // Room should exist

            // Step 4: Verify that a non-existing room returns false
            var nonExistentRoomId = insertedRoomId + 1000; // Ensuring this room doesn't exist
            var nonExistentRoomResult = _roomModel.DoesRoomExist(nonExistentRoomId);

            nonExistentRoomResult.Should().BeFalse(); // Non-existing room should return false
        }
    }

    [TestMethod]
    public void GetRooms_ShouldReturnListOfRooms_WhenRoomsExist()
    {
        using (var scope = new TransactionScope())
        {
            // Step 1: Set up the database
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

            // Step 2: Test GetRooms method from RoomModel
            var roomModel = new RoomModel();
            var rooms = roomModel.GetRooms();

            rooms.Should().NotBeNull();  // Assert that the result is not null
            rooms.Should().HaveCountGreaterThan(0); // Assert that the list contains rooms
        }
    }

    [TestMethod]
    public void GetRooms_ShouldReturnEmptyList_WhenNoRoomsExist()
    {
        using (var scope = new TransactionScope())
        {
            // Step 1: Set up the database with no rooms
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

                // No rooms inserted here
            }

            // Step 2: Test GetRooms method from RoomModel when no rooms exist
            var roomModel = new RoomModel();
            var rooms = roomModel.GetRooms();

            rooms.Should().NotBeNull(); // Assert that the result is not null
            rooms.Should().HaveCount(0); // Assert that the list is empty
        }
    }
}