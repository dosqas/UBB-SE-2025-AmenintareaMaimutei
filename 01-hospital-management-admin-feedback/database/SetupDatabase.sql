-- Change the database context to the master database
USE master;
GO

-- Create the HospitalManagement database
IF DB_ID('HospitalManagement') IS NULL
BEGIN
    CREATE DATABASE HospitalManagement;
END
GO

-- Change the database context to the HospitalManagement database
USE HospitalManagement;
GO

-- Run the CreateTables procedure
:r .\CreateTables.sql
GO

-- Run the AddConstraints procedure
:r .\AddConstraints.sql
GO

-- Run the InsertData procedure
:r .\InsertData.sql
GO

-- Run the DropTables procedure
:r .\DropTables.sql
GO

-- Build the database
:r .\RebuildDB.sql
GO
