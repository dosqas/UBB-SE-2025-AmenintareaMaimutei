CREATE OR ALTER PROCEDURE GetFriends
    @UserId INT
AS
BEGIN
    SELECT 
        u.UserId, 
        u.UserName, 
        u.Email, 
        u.PrivacyStatus,
        u.OnlineStatus,
        u.DateJoined,
        u.ProfileImage,
        u.TotalPoints,
        u.CoursesCompleted,
        u.QuizzesCompleted,
        u.Streak,
        u.Password,
		u.LastActivityDate,
		u.Accuracy
    FROM Friends f
    JOIN Users u ON (f.UserId1 = @UserId AND f.UserId2 = u.UserId)
                OR (f.UserId2 = @UserId AND f.UserId1 = u.UserId);
END;
GO