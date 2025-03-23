USE HospitalManagement;
GO

--ADD INTEGRITY CONSTRAINTS
CREATE OR ALTER PROCEDURE AddConstraints
AS
BEGIN

-- USERS
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Role')
BEGIN
    ALTER TABLE Users DROP CONSTRAINT CK_Role;
END
ALTER TABLE Users
ADD CONSTRAINT CK_Role CHECK (Role IN ('Patient', 'Doctor', 'Admin')); --role can be only Patient, Doctor or Admin

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_PhoneNumber')
BEGIN
    ALTER TABLE Users DROP CONSTRAINT CK_PhoneNumber;
END
ALTER TABLE Users
ADD CONSTRAINT CK_PhoneNumber CHECK (PhoneNumber NOT LIKE '%[^0-9]%' AND LEN(PhoneNumber) = 10); --contains only digits

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Cnp')
BEGIN
    ALTER TABLE Users DROP CONSTRAINT CK_Cnp;
END
ALTER TABLE Users
ADD CONSTRAINT CK_Cnp CHECK (Cnp NOT LIKE '%[^0-9]%' AND LEN(Cnp) = 13); --contains only digits

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Address')
BEGIN
    ALTER TABLE Users DROP CONSTRAINT CK_Address;
END
ALTER TABLE Users
ADD CONSTRAINT CK_Address CHECK (Address NOT LIKE '%[^a-zA-Z0-9 ,.-]%'); --contains only alphanumeric characters, spaces, commas, period, dash


-- DEPARTMENTS
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_DepartmentName')
BEGIN
    ALTER TABLE Departments DROP CONSTRAINT CK_DepartmentName;
END
ALTER TABLE Departments
ADD CONSTRAINT CK_DepartmentName CHECK (Name NOT LIKE '%[^a-zA-Z0-9 ]%'); --contains only alphanumeric characters


-- DOCTORS
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_LicenseNumber')
BEGIN
    ALTER TABLE Doctors DROP CONSTRAINT CK_LicenseNumber;
END
ALTER TABLE Doctors
ADD CONSTRAINT CK_LicenseNumber CHECK (LicenseNumber NOT LIKE '%[^a-zA-Z0-9 ]%'); --contains only alphanumeric characters

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Experience')
BEGIN
    ALTER TABLE Doctors DROP CONSTRAINT CK_Experience;
END
ALTER TABLE Doctors
ADD CONSTRAINT CK_Experience CHECK (Experience >= 0); --experience must be positive

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Rating')
BEGIN
    ALTER TABLE Doctors DROP CONSTRAINT CK_Rating;
END
ALTER TABLE Doctors
ADD CONSTRAINT CK_Rating CHECK (Rating >= 0 AND Rating <= 5); --rating must be between 0 and 5


-- DRUGS
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Supply')
BEGIN
    ALTER TABLE Drugs DROP CONSTRAINT CK_Supply;
END
ALTER TABLE Drugs
ADD CONSTRAINT CK_Supply CHECK (Supply >= 0); --supply must be positive

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_DrugName')
BEGIN
    ALTER TABLE Drugs DROP CONSTRAINT CK_DrugName;
END
ALTER TABLE Drugs
ADD CONSTRAINT CK_DrugName CHECK (Name NOT LIKE '%[^a-zA-Z0-9 ]%'); --contains only alphanumeric characters


-- EQUIPMENTS
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Stock')
BEGIN
    ALTER TABLE Equipments DROP CONSTRAINT CK_Stock;
END
ALTER TABLE Equipments
ADD CONSTRAINT CK_Stock CHECK (Stock >= 0); --stock must be positive

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_EquipmentName')
BEGIN
    ALTER TABLE Equipments DROP CONSTRAINT CK_EquipmentName;
END
ALTER TABLE Equipments
ADD CONSTRAINT CK_EquipmentName CHECK (Name NOT LIKE '%[^a-zA-Z0-9 ]%'); --contains only alphanumeric characters

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Type')
BEGIN
    ALTER TABLE Equipments DROP CONSTRAINT CK_Type;
END
ALTER TABLE Equipments
ADD CONSTRAINT CK_Type CHECK (Type NOT LIKE '%[^a-zA-Z0-9 ]%'); --type can be only alphanumeric characters

IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Specification')
BEGIN
    ALTER TABLE Equipments DROP CONSTRAINT CK_Specification;
END
ALTER TABLE Equipments
ADD CONSTRAINT CK_Specification CHECK (Specification NOT LIKE '%[^a-zA-Z0-9 ,.-]%'); --contains only alphanumeric characters, spaces, commas, period, dash


-- ROOMS
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Capacity')
BEGIN
	ALTER TABLE Rooms DROP CONSTRAINT CK_Capacity;
END
ALTER TABLE Rooms
ADD CONSTRAINT CK_Capacity CHECK (Capacity > 0); --capacity must be positive


-- Reviews

-- Add check constraint for NrStars
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_NrStars')
BEGIN
	ALTER TABLE Reviews DROP CONSTRAINT CK_NrStars;
END
ALTER TABLE Reviews
ADD CONSTRAINT CK_NrStars CHECK (NrStars >= 1 AND NrStars <= 5);



-- SHIFTS
-- IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Time')
-- BEGIN
	-- ALTER TABLE Shifts DROP CONSTRAINT CK_Date;
-- END
-- ALTER TABLE Shifts
-- ADD CONSTRAINT CK_Time CHECK (StartTime >= EndTime);

END;