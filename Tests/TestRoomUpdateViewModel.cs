using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ViewModels.UpdateViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        // Arrange
        var expectedMessage = "Some error occurred";

        // Act
        this._roomUpdateViewModel.ErrorMessage = expectedMessage;

        // Assert
        this._roomUpdateViewModel.ErrorMessage.Should().Be(expectedMessage, "because the ErrorMessage should be set correctly when assigned a new value");
    }

    [TestMethod]
    public void ErrorMessage_ShouldTriggerPropertyChangedEvent_WhenSet()
    {
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

    [TestMethod]
    public void ErrorMessage_ShouldNotTriggerPropertyChangedEvent_IfSameValueAssigned()
    {
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
