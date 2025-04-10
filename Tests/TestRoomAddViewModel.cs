using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Models;
using FluentAssertions;
using Project.ViewModels.AddViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

[TestClass]
public class TestRoomAddViewModel
{
    private RoomAddViewModel _roomAddViewModel = new RoomAddViewModel();

    [TestInitialize]
    public void Setup()
    {
        _roomAddViewModel = new RoomAddViewModel();
    }

    [TestMethod]
    public void Capacity_ShouldSetCorrectValue_WhenAssigned()
    {
        // Arrange
        var room = new Room();

        // Act
        room.Capacity = 25;

        // Assert
        room.Capacity.Should().Be(25);  // Assert that the Capacity is correctly set
    }

    [TestMethod]
    public void DepartmentID_ShouldSetCorrectValue_WhenAssigned()
    {
        // Arrange
        var room = new Room();

        // Act
        room.DepartmentID = 10;

        // Assert
        room.DepartmentID.Should().Be(10, "because the DepartmentID should be correctly set to the assigned value.");
    }

    [TestMethod]
    public void EquipmentID_ShouldSetCorrectValue_WhenAssigned()
    {
        // Arrange
        var room = new Room();

        // Act
        room.EquipmentID = 10;

        // Assert
        room.EquipmentID.Should().Be(10, "because the EquipmentID should be correctly set to the assigned value.");
    }
}
