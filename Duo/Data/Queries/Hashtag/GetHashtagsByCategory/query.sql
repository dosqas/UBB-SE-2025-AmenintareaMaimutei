CREATE OR ALTER PROCEDURE GetHashtagsByCategory
    @CategoryID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT h.Id, h.Tag
    FROM Hashtags h
    INNER JOIN PostHashtags ph ON h.Id = ph.HashtagId
    INNER JOIN Posts p ON ph.PostId = p.Id
    WHERE p.CategoryID = @CategoryID
    ORDER BY h.Tag ASC;
END; 