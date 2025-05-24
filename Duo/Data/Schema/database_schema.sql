
DROP TABLE IF EXISTS FriendRequests;
DROP TABLE IF EXISTS UserAchievements;
DROP TABLE IF EXISTS Achievements;
DROP TABLE IF EXISTS Friends;
DROP TABLE IF EXISTS HashTags;
DROP TABLE IF EXISTS PostHashtags;
drop table if exists Comments;
DROP TABLE IF EXISTS Posts;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Categories;
GO

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(255) NOT NULL, -- Added Password Column
    PrivacyStatus BIT NOT NULL,
    OnlineStatus BIT NOT NULL,
    DateJoined DATETIME NOT NULL DEFAULT GETDATE(),
    ProfileImage NVARCHAR(MAX),
    TotalPoints INT NOT NULL DEFAULT 0,
    CoursesCompleted INT NOT NULL DEFAULT 0,
    QuizzesCompleted INT NOT NULL DEFAULT 0,
    Streak INT NOT NULL DEFAULT 0,
	LastActivityDate DATETIME NULL,
	Accuracy DECIMAL(5,2) NOT NULL DEFAULT 0.00
);
GO

SELECT * FROM Users;
GO

CREATE TABLE Hashtags (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Tag NVARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE Categories (
    Id INT PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);
go


CREATE TABLE Posts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(50) NOT NULL,
    Description NVARCHAR(4000) NOT NULL,		--- We need to modify in the diagram(can t put 5000) -->
    UserID INT NOT NULL,
    CategoryID INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    LikeCount INT DEFAULT 0,

    CONSTRAINT fk_user FOREIGN KEY (userID) REFERENCES Users(userID) ON DELETE CASCADE,
    CONSTRAINT fk_category FOREIGN KEY (CategoryID) REFERENCES Categories(Id) ON DELETE CASCADE
);
go

--- Need to add a many to many table -->
CREATE TABLE PostHashtags (
    PostID INT NOT NULL,
    HashtagID INT NOT NULL,

    PRIMARY KEY (PostID, HashtagID),

    CONSTRAINT fk_post FOREIGN KEY (PostID) REFERENCES Posts(Id) ON DELETE CASCADE,
    CONSTRAINT fk_hashtag FOREIGN KEY (HashtagID) REFERENCES Hashtags(Id) ON DELETE CASCADE
);
go

--- Look for delete and update -->
CREATE TABLE Comments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Content NVARCHAR(1000) NOT NULL,
    UserID INT NOT NULL,
    PostID INT NOT NULL,
    ParentCommentID INT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    LikeCount INT DEFAULT 0,
    Level INT CHECK (Level BETWEEN 1 AND 3),

    CONSTRAINT fk_userid FOREIGN KEY (UserID) REFERENCES Users(userID) on delete no action,
    CONSTRAINT fk_posts FOREIGN KEY (PostID) REFERENCES Posts(Id) on delete no action,
    CONSTRAINT fk_parent_comment FOREIGN KEY (ParentCommentID) REFERENCES Comments(Id) on delete no action
);
GO
CREATE TABLE Achievements (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    Rarity NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE FriendRequests (
    RequestId INT IDENTITY(1,1) PRIMARY KEY,
    SenderId INT NOT NULL,
    ReceiverId INT NOT NULL,
    RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (SenderId) REFERENCES Users(UserId) ON DELETE NO ACTION,
    FOREIGN KEY (ReceiverId) REFERENCES Users(UserId) ON DELETE NO ACTION,
    CHECK (SenderId <> ReceiverId) -- Prevent self-requests
);
GO

CREATE TABLE Friends (
    FriendshipId INT IDENTITY(1,1) PRIMARY KEY,
    UserId1 INT NOT NULL,
    UserId2 INT NOT NULL,
    FOREIGN KEY (UserId1) REFERENCES Users(UserId) ON DELETE NO ACTION,
    FOREIGN KEY (UserId2) REFERENCES Users(UserId) ON DELETE NO ACTION,
    CHECK (UserId1 <> UserId2) -- Prevent self-friendship
);
GO


DROP TABLE IF EXISTS UserAchievements;
GO

CREATE TABLE UserAchievements (
    UserId INT,
    AchievementId INT,
    AwardedDate DATETIME,
    PRIMARY KEY (UserId, AchievementId)
);
