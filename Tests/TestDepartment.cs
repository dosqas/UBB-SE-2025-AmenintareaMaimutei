using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Models;

namespace Tests
{
    [TestClass]
    public class TestDepartment
    {
        [TestMethod]
        public void DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var department = new Department();

            // Assert
            department.DepartmentID.Should().Be(0); // default int value
            department.Name.Should().Be("Unspecified");
        }

        [TestMethod]
        public void ParameterizedConstructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            int expectedId = 1;
            string expectedName = "Cardiology";

            // Act
            var department = new Department(expectedId, expectedName);

            // Assert
            department.DepartmentID.Should().Be(expectedId);
            department.Name.Should().Be(expectedName);
        }

        [TestMethod]
        public void DepartmentID_Property_ShouldBeSettable()
        {
            // Arrange
            var department = new Department();
            int expectedId = 5;

            // Act
            department.DepartmentID = expectedId;

            // Assert
            department.DepartmentID.Should().Be(expectedId);
        }

        [TestMethod]
        public void Name_Property_ShouldBeSettable()
        {
            // Arrange
            var department = new Department();
            string expectedName = "Neurology";

            // Act
            department.Name = expectedName;

            // Assert
            department.Name.Should().Be(expectedName);
        }

        [TestMethod]
        public void Name_Property_ShouldHandleNullValue()
        {
            // Arrange
            var department = new Department();

            // Act
            department.Name = null;

            // Assert
            department.Name.Should().BeNull();
        }

        [TestMethod]
        public void ParameterizedConstructor_ShouldHandleEmptyName()
        {
            // Arrange
            int expectedId = 2;
            string emptyName = "";

            // Act
            var department = new Department(expectedId, emptyName);

            // Assert
            department.DepartmentID.Should().Be(expectedId);
            department.Name.Should().Be(emptyName);
        }
    }
}
