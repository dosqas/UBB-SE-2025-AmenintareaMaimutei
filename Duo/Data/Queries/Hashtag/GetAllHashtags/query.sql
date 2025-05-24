CREATE OR ALTER PROCEDURE GetAllHashtags
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Tag, CreatedAt
    FROM Hashtags
    ORDER BY Tag ASC;
END; 