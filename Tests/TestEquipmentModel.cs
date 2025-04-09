using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.ClassModels;
using FluentAssertions;
using Project.Models;

namespace Tests;

[TestClass]
public class TestEquipmentModel
{
    private EquipmentModel _equipmentModel = new EquipmentModel();

    [TestInitialize]
    public void Setup()
    {
        _equipmentModel = new EquipmentModel();
    }

    [TestMethod]
    public void AddEquipment_ShouldReturnTrue_WhenInsertSucceeds()
    {
        var equipment = new Equipment { Name = "Test", Type = "Type", Specification = "Spec", Stock = 5 };
        var result = _equipmentModel.AddEquipment(equipment);
        result.Should().BeTrue();
    }

    [TestMethod]
    public void UpdateEquipment_ShouldReturnTrue_WhenUpdateSucceeds()
    {
        var equipment = new Equipment { EquipmentID = 11, Name = "Test", Type = "Type", Specification = "Spec", Stock = 5 };
        var result = _equipmentModel.UpdateEquipment(equipment);
        result.Should().BeTrue();
    }

    [TestMethod]
    public void UpdateEquipment_ShouldReturnFalse_WhenExceptionOccurs()
    {
        var equipment = new Equipment { EquipmentID = 1 };
        var result = _equipmentModel.UpdateEquipment(equipment);
        result.Should().BeFalse();
    }

    [TestMethod]
    public void DeleteEquipment_ShouldReturnTrue_WhenDeleteSucceeds()
    {
        var result = _equipmentModel.DeleteEquipment(1);
        result.Should().BeTrue();
    }

    [TestMethod]
    public void DoesEquipmentExist_ShouldReturnTrue_WhenEquipmentExists()
    {
        var result = _equipmentModel.DoesEquipmentExist(5);
        result.Should().BeTrue();
    }

    [TestMethod]
    public void GetEquipments_ShouldReturnEquipmentList_WhenDataExists()
    {
        var result = _equipmentModel.GetEquipments();
        result.Should().NotBeNull();
        result.Count.Should().Be(12);
        result[0].EquipmentID.Should().Be(1);
        result[0].Name.Should().Be("Test1");
    }
}
