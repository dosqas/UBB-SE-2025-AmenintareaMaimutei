CREATE or ALTER PROCEDURE GetTopUsersByAccuracy
AS
Begin
	Select TOP 10
	u.UserId,
	u.UserName,
	u.ProfileImage,
	u.Accuracy,
	u.QuizzesCompleted
	FROM Users u
	ORDER By u.Accuracy DESC
End

EXEC GetTopUsersByAccuracy