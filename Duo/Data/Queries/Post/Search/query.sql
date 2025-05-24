CREATE PROCEDURE SearchPosts
    @Query NVARCHAR(255),
    @Offset INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Posts
    WHERE Title LIKE @Query OR Description LIKE @Query
    ORDER BY CreatedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END