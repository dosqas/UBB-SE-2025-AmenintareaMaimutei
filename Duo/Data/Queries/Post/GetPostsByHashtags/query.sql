CREATE PROCEDURE GetByHashtags
    @hashtags NVARCHAR(MAX),
    @PageSize INT,
    @Offset INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @HashtagTable TABLE (Hashtag NVARCHAR(100));

    INSERT INTO @HashtagTable
    SELECT value FROM STRING_SPLIT(@hashtags, ',');

    SELECT DISTINCT p.*
    FROM Posts p
    INNER JOIN PostHashtags ph ON p.Id = ph.PostId
    INNER JOIN Hashtags h ON ph.HashtagId = h.Id
    WHERE h.Name IN (SELECT Hashtag FROM @HashtagTable)
    ORDER BY p.CreatedAt DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END