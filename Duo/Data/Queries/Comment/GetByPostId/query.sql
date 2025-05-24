CREATE OR ALTER PROCEDURE GetCommentsByPostID
    @PostID INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH CommentHierarchy AS 
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
        WHERE c.PostID = @PostID AND c.ParentCommentID IS NULL

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
        INNER JOIN CommentHierarchy ch ON c.ParentCommentID = ch.CommentID
        WHERE c.Level <= 3
    )

    SELECT * FROM CommentHierarchy ORDER BY CreatedAt;
END;