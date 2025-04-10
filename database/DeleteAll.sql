USE HospitalManagement;
GO

CREATE OR ALTER PROCEDURE ClearTables
AS
BEGIN
    DELETE FROM Schedules;
    DELETE FROM Shifts;
    DELETE FROM Rooms;
    DELETE FROM Equipments;
    DELETE FROM Drugs;
    DELETE FROM Doctors;
    DELETE FROM Departments;
    DELETE FROM Admins;
    DELETE FROM Users;
    DELETE FROM Reviews;
END;
