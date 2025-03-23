USE HospitalManagement;
GO

CREATE OR ALTER VIEW UserDoctorDepartmentView AS
SELECT 
    u.UserID AS UserID,
    u.Username AS Username,
    u.Mail AS Mail,
    u.Role AS Role,
    u.Name AS Name,
    u.Birthdate AS Birthdate,
    u.Cnp AS Cnp,
    u.Address AS Address,
    u.PhoneNumber AS PhoneNumber,
    u.RegistrationDate AS RegistrationDate,
    d.DoctorID AS DoctorID,
    d.LicenseNumber AS LicenseNumber,
    d.Experience AS Experience,
    d.Rating AS Rating,
    d.DepartmentID AS DepartmentID,
    d.DepartmentName AS DepartmentName
FROM
    Users u
JOIN
    DoctorDepartmentView d ON u.UserID = d.UserID