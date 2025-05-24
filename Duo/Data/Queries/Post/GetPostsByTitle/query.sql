CREATE OR ALTER PROCEDURE GetPostsByTitle
    @Title NVARCHAR(255)
AS
BEGIN
    SELECT 
        p.Id,
        p.Title,
        p.Description,
        p.UserID,
        p.CategoryID,
        p.CreatedAt,
        p.UpdatedAt,
        p.LikeCount
    FROM 
        Posts p
    WHERE 
        p.Title = @Title
END