CREATE OR ALTER PROCEDURE DeleteHashtagFromPost
    @PostID INT,
    @HashtagID INT
AS
BEGIN
    -- Delete the hashtag from the given post
    DELETE FROM PostHashtags
    WHERE PostID = @PostID AND HashtagID = @HashtagID;
END;