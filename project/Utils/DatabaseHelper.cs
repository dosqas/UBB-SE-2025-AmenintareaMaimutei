namespace Project.Utils
{
    using System.Configuration;

    /// <summary>
    /// Provides utility methods for database operations.
    /// </summary>
    public class DatabaseHelper
    {

        private const string ResetProcedureSql = @"
CREATE OR ALTER PROCEDURE DeleteData AS
BEGIN
DELETE FROM Reviews;
DELETE FROM Schedules;
DELETE FROM Shifts;
DELETE FROM Rooms;
DELETE FROM Doctors;
DELETE FROM Admins;
DELETE FROM Users;
DELETE FROM Departments;
DELETE FROM Drugs;
DELETE FROM Equipments;

-- Reseed identities
DBCC CHECKIDENT ('Reviews', RESEED, 0);
DBCC CHECKIDENT ('Schedules', RESEED, 0);
DBCC CHECKIDENT ('Shifts', RESEED, 0);
DBCC CHECKIDENT ('Rooms', RESEED, 0);
DBCC CHECKIDENT ('Doctors', RESEED, 0);
DBCC CHECKIDENT ('Admins', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);
DBCC CHECKIDENT ('Departments', RESEED, 0);
DBCC CHECKIDENT ('Drugs', RESEED, 0);
DBCC CHECKIDENT ('Equipments', RESEED, 0);

END
";

        private const string insertDataProcedure = @"
CREATE OR ALTER PROCEDURE InsertData
    @nrOfRows INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @i INT = 1;

    WHILE @i <= @nrOfRows
    BEGIN
        -- Insert into Users
        INSERT INTO Users (Username, Mail, Password, Role, Name, Birthdate, Cnp, Address, PhoneNumber, RegistrationDate)
        VALUES (
            CONCAT('user', @i),
            CONCAT('user', @i, '@example.com'),
            'password',
            CASE WHEN @i % 3 = 0 THEN 'Admin' WHEN @i % 2 = 0 THEN 'Doctor' ELSE 'Patient' END,
            CONCAT('User ', @i),
            DATEADD(YEAR, -20, GETDATE()),
            RIGHT(CONCAT('6040322012025', @i), 13),
            CONCAT('Address Nr. ', @i),
            RIGHT(CONCAT('0765432189', @i), 10),
            GETDATE()
        );

        -- Insert into Departments
        INSERT INTO Departments (Name)
        VALUES (CONCAT('Department ', @i));

        -- Insert into Doctors
        INSERT INTO Doctors (UserId, Experience, Rating, DepartmentId, LicenseNumber)
        VALUES (
            @i,
            RAND() * 10,
            RAND() * 5,
            @i,
            CONCAT('License', @i)
        );

        -- Insert into Drugs
        INSERT INTO Drugs (Name, Administration, Supply, Specification)
        VALUES (
            CONCAT('Drug ', @i),
            'Pill',
            @i * 10,
            CONCAT('Specification ', @i)
        );

        -- Insert into Equipments
        INSERT INTO Equipments (Name, Specification, Type, Stock)
        VALUES (
            CONCAT('Equipment ', @i),
            CONCAT('Specification ', @i),
            'Type A',
            @i * 5
        );

        -- Insert into Rooms
        INSERT INTO Rooms (Capacity, DepartmentID, EquipmentID)
        VALUES (
            @i * 2,
            @i,
            @i
        );

        -- Insert into Shifts
        INSERT INTO Shifts (Date, StartTime, EndTime)
        VALUES (
            GETDATE(),
            CASE WHEN @i % 3 = 0 THEN '08:00:00' WHEN @i % 3 = 1 THEN '20:00:00' ELSE '08:00:00' END,
            CASE WHEN @i % 3 = 0 THEN '20:00:00' WHEN @i % 3 = 1 THEN '08:00:00' ELSE '08:00:00' END
        );

        -- Insert into Schedules
        INSERT INTO Schedules (DoctorID, ShiftID)
        VALUES (
            @i,
            @i
        );

        SET @i = @i + 1;
    END
END
";

        /// <summary>
        /// Gets the connection string for the HospitalManagement database.
        /// </summary>
        /// <returns>The connection string.</returns>
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["HospitalManagement"].ConnectionString;
        }

        /// <summary>
        /// 
        public static string GetResetProcedureSql()
        {
            return ResetProcedureSql;
        }

        /// <summary>
        /// 
        public static string GetInsertDataProcedureSql()
        {
            return insertDataProcedure;
        }
    }
}