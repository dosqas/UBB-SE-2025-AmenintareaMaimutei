CREATE OR ALTER PROCEDURE CreatePost (
    @Title NVARCHAR (50),
    @Description VARCHAR (4000),
    @UserID INT,
    @CategoryID INT,
    @CreatedAt DATETIME,
    @UpdatedAt DATETIME,
    @LikeCount INT
) AS
BEGIN
    INSERT INTO Posts
        (Title, Description, UserID, CategoryID, CreatedAt, UpdatedAt, LikeCount)
    VALUES
        (@Title, @Description, @UserID, @CategoryID, @CreatedAt, @UpdatedAt, @LikeCount);
        
    -- Return the ID of the newly inserted post
    SELECT SCOPE_IDENTITY() AS NewPostID;
END