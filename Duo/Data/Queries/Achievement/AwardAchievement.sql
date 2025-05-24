CREATE OR ALTER PROCEDURE AwardAchievement
    @UserId INT,
    @AchievementId INT,
    @AwardedDate DATETIME
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM UserAchievements WHERE UserId = @UserId AND AchievementId = @AchievementId)
    BEGIN
        INSERT INTO UserAchievements (UserId, AchievementId, AwardedDate)
        VALUES (@UserId, @AchievementId, @AwardedDate);
    END
END;