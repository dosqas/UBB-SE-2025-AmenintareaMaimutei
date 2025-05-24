CREATE OR ALTER PROCEDURE GetUserStats
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        TotalPoints,
        Streak,
        QuizzesCompleted,
        CoursesCompleted
    FROM 
        Users
    WHERE 
        UserId = @UserId;
END
