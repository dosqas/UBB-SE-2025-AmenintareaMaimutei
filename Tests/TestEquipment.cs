using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Models;

namespace Tests
{
    [TestClass]
    public class TestEquipment
    {
        [TestMethod]
        public void DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            var equipment = new Equipment();

            equipment.EquipmentID.Should().Be(0);
            equipment.Name.Should().Be("Unspecified");
            equipment.Type.Should().Be("Unspecified");
            equipment.Specification.Should().Be("Unspecified");
            equipment.Stock.Should().Be(0);
        }

        [TestMethod]
        public void ParameterizedConstructor_ShouldSetPropertiesCorrectly()
        {
            int expectedId = 1;
            string expectedName = "MRI Machine";
            string expectedType = "Imaging";
            string expectedSpec = "3T MRI Scanner";
            int expectedStock = 5;

            var equipment = new Equipment(expectedId, expectedName, expectedType, expectedSpec, expectedStock);

            equipment.EquipmentID.Should().Be(expectedId);
            equipment.Name.Should().Be(expectedName);
            equipment.Type.Should().Be(expectedType);
            equipment.Specification.Should().Be(expectedSpec);
            equipment.Stock.Should().Be(expectedStock);
        }

        [TestMethod]
        public void Properties_ShouldBeSettable()
        {
            var equipment = new Equipment();

            equipment.EquipmentID = 10;
            equipment.Name = "X-Ray";
            equipment.Type = "Radiology";
            equipment.Specification = "Digital X-Ray System";
            equipment.Stock = 3;

            equipment.EquipmentID.Should().Be(10);
            equipment.Name.Should().Be("X-Ray");
            equipment.Type.Should().Be("Radiology");
            equipment.Specification.Should().Be("Digital X-Ray System");
            equipment.Stock.Should().Be(3);
        }

        [TestMethod]
        public void Name_Property_ShouldHandleNull()
        {
            var equipment = new Equipment();
            equipment.Name = null;
            equipment.Name.Should().BeNull();
        }

        [TestMethod]
        public void Type_Property_ShouldHandleNull()
        {
            var equipment = new Equipment();
            equipment.Type = null;
            equipment.Type.Should().BeNull();
        }

        [TestMethod]
        public void Specification_Property_ShouldHandleNull()
        {
            var equipment = new Equipment();
            equipment.Specification = null;
            equipment.Specification.Should().BeNull();
        }

        [TestMethod]
        public void Stock_Property_ShouldHandleNegativeValues()
        {
            var equipment = new Equipment();
            equipment.Stock = -5;
            equipment.Stock.Should().Be(-5);
        }

        [TestMethod]
        public void ParameterizedConstructor_ShouldHandleEmptyStrings()
        {
            var equipment = new Equipment(2, "", "", "", 0);

            equipment.Name.Should().Be("");
            equipment.Type.Should().Be("");
            equipment.Specification.Should().Be("");
        }
    }
}
