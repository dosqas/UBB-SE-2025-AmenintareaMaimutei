USE HospitalManagement;
GO

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
            CASE WHEN @i % 3 = 0 THEN '16:00:00' WHEN @i % 3 = 1 THEN '08:00:00' ELSE '16:00:00' END
        );

        -- Insert into Schedules
        INSERT INTO Schedules (DoctorID, ShiftID)
        VALUES (
            @i,
            @i
        );

        SET @i = @i + 1;
    END
END;
