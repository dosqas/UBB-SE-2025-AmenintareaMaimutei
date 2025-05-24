CREATE OR ALTER PROCEDURE GetUserIdByPostId (
    @PostId INT
) AS
BEGIN 
    SELECT UserId
    FROM Posts p
    WHERE p.PostId = @PostId
END