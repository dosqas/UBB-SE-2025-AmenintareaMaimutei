CREATE OR ALTER PROCEDURE RemoveFriend
    @UserId1 INT,
    @UserId2 INT
AS
BEGIN
    DELETE FROM Friends
    WHERE (UserId1 = @UserId1 AND UserId2 = @UserId2)
       OR (UserId1 = @UserId2 AND UserId2 = @UserId1);
END;
GO
