CREATE or ALTER PROCEDURE GetTopUsersByCompletedQuizzes
AS
Begin
	Select TOP 10 
	u.UserId,
	u.UserName,
	u.ProfileImage,
	u.Accuracy,
	u.QuizzesCompleted
	FROM Users u
	ORDER By u.QuizzesCompleted DESC
End
