CREATE OR ALTER PROCEDURE GetPaginatedPosts
    @Offset INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.Id, 
        p.Title, 
        p.Description, 
        p.UserID, 
        p.CategoryID, 
        p.CreatedAt, 
        p.UpdatedAt, 
        p.LikeCount, 
        u.Username
    FROM Posts p
    JOIN Users u ON p.UserID = u.userID
    JOIN Categories c ON p.CategoryID = c.Id 
    ORDER BY p.CreatedAt DESC  
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END; 