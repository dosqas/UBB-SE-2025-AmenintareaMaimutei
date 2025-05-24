CREATE PROCEDURE GetUserAchievements
    @UserId INT
AS
BEGIN
    SELECT ua.AchievementId, a.Name, a.Description, a.Rarity, ua.AwardedDate
    FROM UserAchievements ua
    INNER JOIN Achievements a ON ua.AchievementId = a.Id
    WHERE ua.UserId = @UserId
END