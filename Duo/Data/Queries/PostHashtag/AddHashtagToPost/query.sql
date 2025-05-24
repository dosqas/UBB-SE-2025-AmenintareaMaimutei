CREATE OR ALTER PROCEDURE AddHashtagToPost
    @PostID INT,
    @HashtagID INT
AS
BEGIN
    INSERT INTO PostHashtags (PostID, HashtagID)
   VALUES (@PostID, @HashtagID);
END;