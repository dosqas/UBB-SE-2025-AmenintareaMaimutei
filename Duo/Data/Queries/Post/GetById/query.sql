CREATE OR ALTER PROCEDURE GetPostById (
    @Id INT
) AS
BEGIN
    SELECT Id,
           Title,
           Description,
           UserID,
           CategoryID,
           CreatedAt,
           UpdatedAt,
           LikeCount
    FROM Posts
    WHERE Id = @Id
END