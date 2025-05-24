CREATE or ALTER PROCEDURE GetTopFriendsByCompletedQuizzes
    @userId INT
AS
BEGIN
    -- Return the user and their friends, if any
    SELECT 
        u.UserId, 
        u.UserName,  
        u.ProfileImage,
        u.Accuracy,
        u.QuizzesCompleted
    FROM Users u
    WHERE 
        u.UserId = @UserId -- Always include the current user
        OR u.UserId IN ( -- Include the user's friends
            SELECT CASE 
                WHEN f.UserId1 = @UserId THEN f.UserId2
                WHEN f.UserId2 = @UserId THEN f.UserId1
                ELSE NULL
            END
            FROM Friends f
            WHERE f.UserId1 = @UserId OR f.UserId2 = @UserId
        )
    ORDER BY
        u.QuizzesCompleted DESC;
END

