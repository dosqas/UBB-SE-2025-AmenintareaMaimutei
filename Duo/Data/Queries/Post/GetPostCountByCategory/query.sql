CREATE OR ALTER PROCEDURE GetPostCountByCategory
    @CategoryID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS CategoryPostCount 
    FROM Posts
    WHERE CategoryID = @CategoryID;
END; 