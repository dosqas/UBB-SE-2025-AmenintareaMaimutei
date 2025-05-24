
GO 
DROP PROCEDURE IF EXISTS AwardAchievement;
go

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
GO

DROP PROCEDURE IF EXISTS GetAllAchievements;
GO


CREATE OR ALTER PROCEDURE GetAllAchievements
AS
BEGIN
    SELECT Id, Name, Description, Rarity
    FROM Achievements
END
GO


DROP PROCEDURE IF EXISTS GetUserAchievements;
GO

CREATE PROCEDURE GetUserAchievements
    @UserId INT
AS
BEGIN
    SELECT ua.AchievementId, a.Name, a.Description, a.Rarity, ua.AwardedDate
    FROM UserAchievements ua
    INNER JOIN Achievements a ON ua.AchievementId = a.Id
    WHERE ua.UserId = @UserId
END

GO

DROP PROCEDURE IF EXISTS GetFriends
GO

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

DROP PROCEDURE IF EXISTS RemoveFriend
GO

CREATE OR ALTER PROCEDURE RemoveFriend
    @UserId1 INT,
    @UserId2 INT
AS
BEGIN
    DELETE FROM Friends
    WHERE (UserId1 = @UserId1 AND UserId2 = @UserId2)
       OR (UserId1 = @UserId2 AND UserId2 = @UserId1);
END;
GO

DROP PROCEDURE IF EXISTS GetTopFriendsByAccuracy
GO

CREATE OR ALTER PROCEDURE GetTopFriendsByAccuracy
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
        u.Accuracy DESC;
END;

GO

DROP PROCEDURE IF EXISTS GetTopFriendsByCompletedQuizzes;
go

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

go

drop procedure if exists GetTopUsersByAccuracy
go

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
go

drop procedure if exists GetTopUsersByCompletedQuizzes
GO

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

GO

DROP PROCEDURE IF EXISTS CreateUser
GO

CREATE OR ALTER PROCEDURE CreateUser
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
    INSERT INTO Users (
        UserName, Email, Password, PrivacyStatus, OnlineStatus, DateJoined,
        ProfileImage, TotalPoints, CoursesCompleted, QuizzesCompleted, Streak, LastActivityDate, Accuracy
    )
    VALUES (
        @UserName, @Email, @Password, @PrivacyStatus, @OnlineStatus, @DateJoined,
        @ProfileImage, @TotalPoints, @CoursesCompleted, @QuizzesCompleted, @Streak, @LastActivityDate, @Accuracy
    );

    SELECT SCOPE_IDENTITY() AS NewUserId;
END;
GO

DROP PROCEDURE IF EXISTS DeleteUser
GO

CREATE OR ALTER PROCEDURE DeleteUser
    @UserId INT
AS
BEGIN
    DELETE FROM Users WHERE UserId = @UserId;
END;
GO

DROP PROCEDURE IF EXISTS GetAllUsers
GO

CREATE OR ALTER PROCEDURE GetAllUsers
AS
BEGIN
    SELECT * FROM Users;
END;
GO

DROP PROCEDURE IF EXISTS GetUserByEmail
GO

CREATE OR ALTER PROCEDURE GetUserByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT *
    FROM Users
    WHERE Email = @Email;
END;
GO

DROP PROCEDURE IF EXISTS GetUserById
GO


CREATE OR ALTER PROCEDURE GetUserById
    @UserId INT
AS
BEGIN
    SELECT * FROM Users WHERE UserId = @UserId;
END;
GO

DROP PROCEDURE IF EXISTS GetUserByUsername
GO

CREATE OR ALTER PROCEDURE GetUserByUsername
    @UserName NVARCHAR(100)
AS
BEGIN
    SELECT *
    FROM Users
    WHERE UserName = @UserName;
END;
GO

DROP PROCEDURE IF EXISTS GetUserStats
GO

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

GO

DROP PROCEDURE IF EXISTS UpdateUser
GO


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


/****** Object:  StoredProcedure [dbo].[AddHashtagToPost]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[AddHashtagToPost]
    @PostID INT,
    @HashtagID INT
AS
BEGIN
    INSERT INTO PostHashtags (PostID, HashtagID)
   VALUES (@PostID, @HashtagID);
END;
GO
/****** Object:  StoredProcedure [dbo].[CreateComment]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[CreateComment]
    @Content NVARCHAR(1000),
    @UserID INT,
    @PostID INT,
    @ParentCommentID INT = NULL, 
    @Level INT
AS
BEGIN
    INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, CreatedAt, LikeCount, Level)
    VALUES (@Content, @UserID, @PostID, @ParentCommentID, GETDATE(), 0, @Level);
END;

GO
/****** Object:  StoredProcedure [dbo].[CreateHashtag]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[CreateHashtag]
    @Tag NVARCHAR(20)
AS
BEGIN
    INSERT INTO Hashtags (Tag)
    VALUES (@Tag);

    -- Return the newly created ID
    SELECT SCOPE_IDENTITY();
END;
GO
/****** Object:  StoredProcedure [dbo].[CreatePost]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[CreatePost] (
    @Title VARCHAR (20),
    @Description VARCHAR (4000),
    @UserID INT,
    @CategoryID INT,
    @CreatedAt DATETIME,
    @UpdatedAt DATETIME,
    @LikeCount INT
) AS
BEGIN
    INSERT INTO Posts
        (Title, Description, UserID, CategoryID, CreatedAt, UpdatedAt, LikeCount)
    VALUES
        (@Title, @Description, @UserID, @CategoryID, @CreatedAt, @UpdatedAt, @LikeCount);
        
    -- Return the ID of the newly inserted post
    SELECT SCOPE_IDENTITY() AS NewPostID;
END
GO
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 24/03/2025 19:54:36 ******/
/****** Object:  StoredProcedure [dbo].[DeleteComment]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[DeleteComment]
    @CommentID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Use a CTE to recursively find all child comments
    WITH CommentTree AS (
        -- Start with the comment we want to delete
        SELECT Id
        FROM Comments
        WHERE Id = @CommentID
        
        UNION ALL
        
        -- Find all children of comments in the tree
        SELECT c.Id
        FROM Comments c
        INNER JOIN CommentTree ct ON c.ParentCommentID = ct.Id
    )
    
    -- Delete all comments in the tree, starting with the leaves (to avoid FK constraint errors)
    DELETE FROM Comments
    WHERE Id IN (SELECT Id FROM CommentTree)
    
    -- Return the number of rows affected
    RETURN @@ROWCOUNT;
END;

GO
/****** Object:  StoredProcedure [dbo].[DeleteHashtag]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[DeleteHashtag]
    @Id INT
AS
BEGIN
    DELETE FROM Hashtags WHERE Id = @Id;
END;

GO
/****** Object:  StoredProcedure [dbo].[DeleteHashtagFromPost]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[DeleteHashtagFromPost]
    @PostID INT,
    @HashtagID INT
AS
BEGIN
    -- Delete the hashtag from the given post
    DELETE FROM PostHashtags
    WHERE PostID = @PostID AND HashtagID = @HashtagID;
END;
GO
/****** Object:  StoredProcedure [dbo].[DeletePost]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[DeletePost] (
    @Id INT
) AS
BEGIN
	DELETE FROM Comments WHERE PostID = @Id;
    DELETE FROM Posts
    WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllComments]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetAllComments]
AS
BEGIN
SELECT * FROM Comments
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllHashtags]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[GetAllHashtags]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Tag
    FROM Hashtags
    ORDER BY Tag ASC;
END; 
GO
/****** Object:  StoredProcedure [dbo].[GetAllPosts]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetAllPosts]
AS
BEGIN
	select * from Posts
END

GO
/****** Object:  StoredProcedure [dbo].[GetByHashtags]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[GetByHashtags]
    @hashtags NVARCHAR(MAX),
    @PageSize INT,
    @Offset INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @HashtagTable TABLE (Hashtag NVARCHAR(100));
    
    INSERT INTO @HashtagTable
    SELECT value FROM STRING_SPLIT(@hashtags, ',');
    
    SELECT DISTINCT p.*
    FROM Posts p
    INNER JOIN PostHashtags ph ON p.Id = ph.PostId
    INNER JOIN Hashtags h ON ph.HashtagId = h.Id
    WHERE h.Tag IN (SELECT Hashtag FROM @HashtagTable)
    ORDER BY p.CreatedAt DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
/****** Object:  StoredProcedure [dbo].[GetCategories]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetCategories]
AS
Begin
	SELECT * FROM Categories
END;
GO
/****** Object:  StoredProcedure [dbo].[GetCategoryByName]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetCategoryByName]
	@name varchar(50)
AS
BEGIN
	SELECT * from Categories C
	where C.Name = @name
END

GO
/****** Object:  StoredProcedure [dbo].[GetCategoryPostCounts]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetCategoryPostCounts]
AS
BEGIN
    SELECT 
        c.Id AS CategoryID,
        c.Name AS CategoryName,
        COUNT(p.Id) AS PostCount
    FROM 
        Categories c
    LEFT JOIN 
        Posts p ON c.Id = p.CategoryID
    GROUP BY 
        c.Id, c.Name
    ORDER BY 
        c.Id;
END
GO
/****** Object:  StoredProcedure [dbo].[GetCommentByID]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetCommentByID]
	@CommentID INT
	AS
	BEGIN
		SELECT * FROM Comments WHERE Id = @CommentID;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetCommentsByPostID]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetCommentsByPostID]
    @PostID INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH CommentHierarchy AS 
    (
        SELECT 
            c.Id AS CommentID,
            c.Content,
            c.UserID,
            c.PostID,
            c.ParentCommentID,
            c.CreatedAt,
            c.Level,
            c.LikeCount
        FROM Comments c
        WHERE c.PostID = @PostID AND c.ParentCommentID IS NULL

        UNION ALL

        SELECT 
            c.Id AS CommentID,
            c.Content,
            c.UserID,
            c.PostID,
            c.ParentCommentID,
            c.CreatedAt,
            c.Level,
            c.LikeCount
        FROM Comments c
        INNER JOIN CommentHierarchy ch ON c.ParentCommentID = ch.CommentID
        WHERE c.Level <= 3
    )

    SELECT * FROM CommentHierarchy ORDER BY CreatedAt;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetCommentsCountForPost]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetCommentsCountForPost]
	@PostID int
AS
BEGIN
SELECT COUNT(*) FROM Comments WHERE PostID = @PostID
END;
GO
/****** Object:  StoredProcedure [dbo].[GetHashtagByText]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[GetHashtagByText]
    @text VARCHAR(20)
AS
BEGIN
    SELECT * FROM Hashtags WHERE Tag = @text;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetHashtagsByCategory]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[GetHashtagsByCategory]
    @CategoryID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT h.Id, h.Tag
    FROM Hashtags h
    INNER JOIN PostHashtags ph ON h.Id = ph.HashtagId
    INNER JOIN Posts p ON ph.PostId = p.Id
    WHERE p.CategoryID = @CategoryID
    ORDER BY h.Tag ASC;
END; 
GO
/****** Object:  StoredProcedure [dbo].[GetHashtagsForPost]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetHashtagsForPost] 
    @PostID INT
AS
BEGIN
    SELECT h.Id, h.Tag
    FROM Hashtags h
    INNER JOIN PostHashtags ph ON h.Id = ph.HashtagID
    WHERE ph.PostID = @PostID;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetPaginatedPosts]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[GetPaginatedPosts]
    @Offset INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.Id, 
        p.Title, 
        p.Description, 
        p.UserID, 
        p.CategoryID, 
        p.CreatedAt, 
        p.UpdatedAt, 
        p.LikeCount, 
        u.Username
    FROM Posts p
    JOIN Users u ON p.UserID = u.userID
    JOIN Categories c ON p.CategoryID = c.Id 
    ORDER BY p.CreatedAt DESC  
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END; 
GO
/****** Object:  StoredProcedure [dbo].[GetPostById]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetPostById] (
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
GO
/****** Object:  StoredProcedure [dbo].[GetPostCountByCategory]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[GetPostCountByCategory]
    @CategoryID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS CategoryPostCount 
    FROM Posts
    WHERE CategoryID = @CategoryID;
END; 
GO
/****** Object:  StoredProcedure [dbo].[GetPostCountByHashtags]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[GetPostCountByHashtags]
    @Hashtags NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @HashtagTable TABLE (Hashtag NVARCHAR(100));
    
    INSERT INTO @HashtagTable
    SELECT value FROM STRING_SPLIT(@Hashtags, ',');
    
    SELECT COUNT(DISTINCT p.Id) AS HashtagPostCount
    FROM Posts p
    INNER JOIN PostHashtags ph ON p.Id = ph.PostId
    INNER JOIN Hashtags h ON ph.HashtagId = h.Id
    WHERE h.Tag IN (SELECT Hashtag FROM @HashtagTable);
END; 
GO
/****** Object:  StoredProcedure [dbo].[GetPostsByCategory]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[GetPostsByCategory] (
    @CategoryID INT,
    @PageSize INT,
    @Offset INT
) AS
BEGIN 
    SELECT Id, Title, Description, UserID, CategoryID, CreatedAt, UpdatedAt, LikeCount
    FROM Posts
    WHERE CategoryID = @CategoryID
    ORDER BY CreatedAt DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY
END
GO
/****** Object:  StoredProcedure [dbo].[GetReplies]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetReplies]
    @ParentCommentID INT
AS
BEGIN
    WITH ReplyHierarchy AS 
    (
        SELECT 
            c.Id AS CommentID,
            c.Content,
            c.UserID,
            c.PostID,
            c.ParentCommentID,
            c.CreatedAt,
            c.Level,
            c.LikeCount
        FROM Comments c
        WHERE c.ParentCommentID = @ParentCommentID

        UNION ALL

        SELECT 
            c.Id AS CommentID,
            c.Content,
            c.UserID,
            c.PostID,
            c.ParentCommentID,
            c.CreatedAt,
            c.Level,
            c.LikeCount
        FROM Comments c
        INNER JOIN ReplyHierarchy r ON c.ParentCommentID = r.CommentID
        WHERE c.Level <= 3
    )

    SELECT * FROM ReplyHierarchy ORDER BY CreatedAt;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetTotalPostCount]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[GetTotalPostCount]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS TotalCount FROM Posts;
END; 
GO
/****** Object:  StoredProcedure [dbo].[GetUserById]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[GetUserById]
    @UserId INT
AS
BEGIN
    SELECT * FROM Users WHERE userID = @UserId;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetUserByUsername]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create or alter procedure [dbo].[GetUserByUsername]
    @Username nvarchar(50)
as
begin
    select * from Users where Username = @Username
end
GO
/****** Object:  StoredProcedure [dbo].[IncrementLikeCount]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[IncrementLikeCount]
	@CommentID int
AS
BEGIN

UPDATE Comments
SET LikeCount = LikeCount + 1
WHERE Id = @CommentId

END;
GO
/****** Object:  StoredProcedure [dbo].[ReadHashtagById]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[ReadHashtagById]
    @Id INT
AS
BEGIN
    SELECT * FROM Hashtags WHERE Id = @Id;
END;
GO
/****** Object:  StoredProcedure [dbo].[UpdateComment]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create or alter procedure [dbo].[UpdateComment]
    @CommentID INT,
    @NewContent NVARCHAR(1000)
AS
BEGIN
    UPDATE Comments
    SET Content = @NewContent
    WHERE Id = @CommentID;
END;

GO
/****** Object:  StoredProcedure [dbo].[UpdatePost]    Script Date: 24/03/2025 19:54:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

create or alter procedure [dbo].[UpdatePost] (
    @Id INT,
    @Title NVARCHAR(20),
    @Description NVARCHAR(4000),
    @UserID INT,
    @CategoryID INT,
    @UpdatedAt DATETIME,
    @LikeCount INT
) AS
BEGIN
    UPDATE Posts
    SET Title = @Title,
        Description = @Description,
        UserID = @UserID,
        CategoryID = @CategoryID,
        UpdatedAt = @UpdatedAt,
        LikeCount = @LikeCount
    WHERE Id = @Id
END

GO
