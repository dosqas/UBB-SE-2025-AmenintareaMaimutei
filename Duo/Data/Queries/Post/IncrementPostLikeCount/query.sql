CREATE OR ALTER PROCEDURE IncrementPostLikeCount
	@PostID int
AS
BEGIN

UPDATE Posts
SET LikeCount = LikeCount + 1
WHERE Id = @PostID

END