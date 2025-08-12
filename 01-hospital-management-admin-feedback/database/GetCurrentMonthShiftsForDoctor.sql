CREATE OR ALTER FUNCTION GetCurrentMonthShiftsForDoctor (@DoctorID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT s.*
    FROM Schedules sc
    JOIN Shifts s ON sc.ShiftID = s.ShiftID
    WHERE sc.DoctorID = @DoctorID
      AND MONTH(s.Date) = MONTH(GETDATE())
      AND YEAR(s.Date) = YEAR(GETDATE())
);