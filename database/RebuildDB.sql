USE HospitalManagement;

exec DropTables;
exec CreateTables;
exec AddConstraints;

exec InsertData @nrOfRows = 10;

-- Create the views and functions
:r .\GetCurrentMonthShiftsForDoctor.sql
GO
:r .\DoctorDepartmentView.sql
GO
:r .\UserDoctorDepartmentView.sql
GO