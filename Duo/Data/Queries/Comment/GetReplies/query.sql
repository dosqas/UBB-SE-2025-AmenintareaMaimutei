CREATE OR ALTER PROCEDURE GetReplies
    @ParentCommentID INT
AS
BEGIN
    WITH ReplyHierarchy AS 
    (
        SELECT 
            c.Id AS CommentID,
            c.Content,
            c.UserID,
            c.PostID,
            c.ParentCommentID,
            c.CreatedAt,
            c.Level,
            c.LikeCount
        FROM Comments c
        WHERE c.ParentCommentID = @ParentCommentID

        UNION ALL

        SELECT 
            c.Id AS CommentID,
            c.Content,
            c.UserID,
            c.PostID,
            c.ParentCommentID,
            c.CreatedAt,
            c.Level,
            c.LikeCount
        FROM Comments c
        INNER JOIN ReplyHierarchy r ON c.ParentCommentID = r.CommentID
        WHERE c.Level <= 3
    )

    SELECT * FROM ReplyHierarchy ORDER BY CreatedAt;
END;