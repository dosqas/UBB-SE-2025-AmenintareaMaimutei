CREATE OR ALTER PROCEDURE IncrementLikeCount
	@CommentID int
AS
BEGIN

UPDATE Comments
SET LikeCount = LikeCount + 1
WHERE Id = @CommentId

END;