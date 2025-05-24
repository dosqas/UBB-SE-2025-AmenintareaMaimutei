CREATE OR ALTER PROCEDURE UpdateUser
    @UserId INT,
    @UserName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Password NVARCHAR(255), -- Added Password Parameter
    @PrivacyStatus BIT,
    @OnlineStatus BIT,
    @DateJoined DATETIME,
    @ProfileImage NVARCHAR(MAX),
    @TotalPoints INT,
    @CoursesCompleted INT,
    @QuizzesCompleted INT,
    @Streak INT,
	@LastActivityDate DATETIME,
	@Accuracy DECIMAL(5,2)
AS
BEGIN
    UPDATE Users
    SET
        UserName = @UserName,
        Email = @Email,
        Password = @Password,
        PrivacyStatus = @PrivacyStatus,
        OnlineStatus = @OnlineStatus,
        DateJoined = @DateJoined,
        ProfileImage = @ProfileImage,
        TotalPoints = @TotalPoints,
        CoursesCompleted = @CoursesCompleted,
        QuizzesCompleted = @QuizzesCompleted,
        Streak = @Streak,
		LastActivityDate = @LastActivityDate,
		Accuracy = @Accuracy
    WHERE UserId = @UserId;
END;
GO