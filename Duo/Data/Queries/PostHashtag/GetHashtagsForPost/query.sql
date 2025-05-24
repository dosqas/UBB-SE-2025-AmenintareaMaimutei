CREATE OR ALTER PROCEDURE GetHashtagsForPost 
    @PostID INT
AS
BEGIN
    SELECT h.Id, h.Tag
    FROM Hashtags h
    INNER JOIN PostHashtags ph ON h.Id = ph.HashtagID
    WHERE ph.PostID = @PostID;
END;