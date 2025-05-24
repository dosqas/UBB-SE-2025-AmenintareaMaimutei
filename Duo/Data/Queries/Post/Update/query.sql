
CREATE OR ALTER PROCEDURE UpdatePost (
    @Id INT,
    @Title NVARCHAR(20),
    @Description NVARCHAR(4000),
    @UserID INT,
    @CategoryID INT,
    @UpdatedAt DATETIME,
    @LikeCount INT
) AS
BEGIN
    UPDATE Posts
    SET Title = @Title,
        Description = @Description,
        UserID = @UserID,
        CategoryID = @CategoryID,
        UpdatedAt = @UpdatedAt,
        LikeCount = @LikeCount
    WHERE Id = @Id
END
