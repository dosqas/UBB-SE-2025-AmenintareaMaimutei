CREATE OR ALTER PROCEDURE CreateHashtag
    @Tag NVARCHAR(20)
AS
BEGIN
    INSERT INTO Hashtags (Tag)
    VALUES (@Tag);

    -- Return the newly created ID
    SELECT SCOPE_IDENTITY();
END;