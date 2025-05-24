CREATE or alter PROCEDURE CreateComment
    @Content NVARCHAR(1000),
    @UserID INT,
    @PostID INT,
    @ParentCommentID INT = NULL, 
    @Level INT
AS
BEGIN
    INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, CreatedAt, LikeCount, Level)
    VALUES (@Content, @UserID, @PostID, @ParentCommentID, GETDATE(), 0, @Level);
END;
