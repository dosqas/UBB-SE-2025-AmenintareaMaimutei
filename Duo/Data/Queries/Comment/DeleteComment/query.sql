CREATE OR ALTER PROCEDURE DeleteComment
    @CommentID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Use a CTE to recursively find all child comments
    WITH CommentTree AS (
        -- Start with the comment we want to delete
        SELECT Id
        FROM Comments
        WHERE Id = @CommentID

        UNION ALL

        -- Find all children of comments in the tree
        SELECT c.Id
        FROM Comments c
        INNER JOIN CommentTree ct ON c.ParentCommentID = ct.Id
    )

    -- Delete all comments in the tree, starting with the leaves (to avoid FK constraint errors)
    DELETE FROM Comments
    WHERE Id IN (SELECT Id FROM CommentTree)

    -- Return the number of rows affected
    RETURN @@ROWCOUNT;
END;
