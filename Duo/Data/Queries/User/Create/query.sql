CREATE PROCEDURE CreateUser
    @Username NVARCHAR(30)
AS
BEGIN
    INSERT INTO Users (Username)
    VALUES (@Username);

    -- Return the newly created ID
    SELECT SCOPE_IDENTITY() AS UserID;
END;
