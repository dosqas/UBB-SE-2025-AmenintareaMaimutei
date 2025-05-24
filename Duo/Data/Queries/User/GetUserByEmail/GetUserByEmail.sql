CREATE OR ALTER PROCEDURE GetUserByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT *
    FROM Users
    WHERE Email = @Email;
END;
GO