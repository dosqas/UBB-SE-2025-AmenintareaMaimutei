CREATE OR ALTER PROCEDURE GetPostCountByHashtags
    @Hashtags NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @HashtagTable TABLE (Hashtag NVARCHAR(100));

    INSERT INTO @HashtagTable
    SELECT value FROM STRING_SPLIT(@Hashtags, ',');

    SELECT COUNT(DISTINCT p.Id) AS HashtagPostCount
    FROM Posts p
    INNER JOIN PostHashtags ph ON p.Id = ph.PostId
    INNER JOIN Hashtags h ON ph.HashtagId = h.Id
    WHERE h.Name IN (SELECT Hashtag FROM @HashtagTable);
END; 