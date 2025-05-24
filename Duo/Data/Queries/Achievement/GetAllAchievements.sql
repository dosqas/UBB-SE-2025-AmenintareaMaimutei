
CREATE OR ALTER PROCEDURE GetAllAchievements
AS
BEGIN
    SELECT Id, Name, Description, Rarity
    FROM Achievements
END
