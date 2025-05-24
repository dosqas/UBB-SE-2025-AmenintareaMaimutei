CREATE PROCEDURE UpdateComment
    @CommentID INT,
    @NewContent NVARCHAR(1000)
AS
BEGIN
    UPDATE Comments
    SET Content = @NewContent
    WHERE Id = @CommentID;
END;
