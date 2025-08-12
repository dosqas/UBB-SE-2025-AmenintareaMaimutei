USE HospitalManagement;
GO

CREATE OR ALTER VIEW DoctorDepartmentView AS
SELECT 
    d.DoctorID,
	d.UserId,
    d.LicenseNumber,
    d.Experience,
    d.Rating,
    dep.DepartmentID,
    dep.Name AS DepartmentName
FROM 
    Doctors d
JOIN 
    Departments dep ON d.DepartmentID = dep.DepartmentID;