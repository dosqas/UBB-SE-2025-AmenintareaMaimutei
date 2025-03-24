USE HospitalManagement;

exec DropTables;
exec CreateTables;
exec AddConstraints;

exec InsertData @nrOfRows = 10;