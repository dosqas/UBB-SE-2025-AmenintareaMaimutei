USE Duo
go

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

select *from UserAchievements
------ inserst community
GO

INSERT INTO Users (
    UserName,
    Email,
    Password,
    PrivacyStatus,
    OnlineStatus,
    DateJoined,
    ProfileImage,
    TotalPoints,
    CoursesCompleted,
    QuizzesCompleted,
    Streak,
	LastActivityDate,
	Accuracy
)
VALUES
('Alice', 'alice@example.com', 'hashedpassword123', 1, 1, GETDATE(), 'alice.jpg', 1200, 5, 10, 7,NULL,95.50),
('Bob', 'bob@example.com', 'securepassword456', 0, 0, GETDATE(), 'bob.jpg', 800, 3, 6, 2,'2024-12-31 14:30:00',95.50),
('Charlie', 'charlie@example.com', 'mypassword789', 1, 1, GETDATE(), 'charlie.jpg', 2000, 10, 15, 12,NULL,95.50),
('David', 'david@example.com', 'pass123', 1, 0, GETDATE(), 'david.jpg', 1500, 7, 12, 5,'2025-03-25 8:30:00',95.50),
('Emma', 'emma@example.com', 'emmaPass456', 1, 1, GETDATE(), 'emma.jpg', 1800, 9, 18, 10,NULL,95.50),
('Frank', 'frank@example.com', 'frankSecure', 0, 1, GETDATE(), 'frank.jpg', 2200, 12, 20, 15,NULL,95.50),
('Alice2', 'alice@example.com', 'hashedpassword123', 1, 1, GETDATE(), 'alice.jpg', 1200, 5, 10, 7,NULL,95.50),
('Bob2', 'bob@example.com', 'securepassword456', 0, 0, GETDATE(), 'bob.jpg', 800, 3, 6, 2,'2024-12-31 14:30:00',95.50),
('Charli2e', 'charlie@example.com', 'mypassword789', 1, 1, GETDATE(), 'charlie.jpg', 2000, 10, 15, 12,NULL,95.50),
('David2', 'david@example.com', 'pass123', 1, 0, GETDATE(), 'david.jpg', 1500, 7, 12, 5,'2025-03-25 8:30:00',95.50),
('Emma2', 'emma@example.com', 'emmaPass456', 1, 1, GETDATE(), 'emma.jpg', 1800, 9, 18, 10,NULL,95.50),
('Frank2', 'frank@example.com', 'frankSecure', 0, 1, GETDATE(), 'frank.jpg', 2200, 12, 20, 15,NULL,95.50),
('Ali2ce', 'alice@example.com', 'hashedpassword123', 1, 1, GETDATE(), 'alice.jpg', 1200, 5, 10, 7,NULL,95.50),
('Bo4b', 'bob@example.com', 'securepassword456', 0, 0, GETDATE(), 'bob.jpg', 800, 3, 6, 2,'2024-12-31 14:30:00',95.50),
('Chdsarlie', 'charlie@example.com', 'mypassword789', 1, 1, GETDATE(), 'charlie.jpg', 2000, 10, 15, 12,NULL,95.50),
('Da2vid', 'david@example.com', 'pass123', 1, 0, GETDATE(), 'david.jpg', 1500, 7, 12, 5,'2025-03-25 8:30:00',95.50),
('Esdfmma', 'emma@example.com', 'emmaPass456', 1, 1, GETDATE(), 'emma.jpg', 1800, 9, 18, 10,NULL,95.50),
('Frsdfank', 'frank@example.com', 'frankSecure', 0, 1, GETDATE(), 'frank.jpg', 2200, 12, 20, 15,NULL,95.50),
('Alsdfice2', 'alice@example.com', 'hashedpassword123', 1, 1, GETDATE(), 'alice.jpg', 1200, 5, 10, 7,NULL,95.50),
('Bosdfb2', 'bob@example.com', 'securepassword456', 0, 0, GETDATE(), 'bob.jpg', 800, 3, 6, 2,'2024-12-31 14:30:00',95.50),
('Chsdfarli2e', 'charlie@example.com', 'mypassword789', 1, 1, GETDATE(), 'charlie.jpg', 2000, 10, 15, 12,NULL,95.50),
('Dsdfavid2', 'david@example.com', 'pass123', 1, 0, GETDATE(), 'david.jpg', 1500, 7, 12, 5,'2025-03-25 8:30:00',95.50),
('Emdsfsma2', 'emma@example.com', 'emmaPass456', 1, 1, GETDATE(), 'emma.jpg', 1800, 9, 18, 10,NULL,95.50),
('Frdsfank2', 'frank@example.com', 'frankSecure', 0, 1, GETDATE(), 'frank.jpg', 2200, 12, 20, 15,NULL,95.50),
('Alfsdfice', 'alice@example.com', 'hashedpassword123', 1, 1, GETDATE(), 'alice.jpg', 1200, 5, 10, 7,NULL,95.50),
('Bsdfb', 'bob@example.com', 'securepassword456', 0, 0, GETDATE(), 'bob.jpg', 800, 3, 6, 2,'2024-12-31 14:30:00',95.50),
('Chsdfarlie', 'charlie@example.com', 'mypassword789', 1, 1, GETDATE(), 'charlie.jpg', 2000, 10, 15, 12,NULL,95.50),
('Dasdfvid', 'david@example.com', 'pass123', 1, 0, GETDATE(), 'david.jpg', 1500, 7, 12, 5,'2025-03-25 8:30:00',95.50),
('Edfsmma', 'emma@example.com', 'emmaPass456', 1, 1, GETDATE(), 'emma.jpg', 1800, 9, 18, 10,NULL,95.50),
('Fradsfnk', 'frank@example.com', 'frankSecure', 0, 1, GETDATE(), 'frank.jpg', 2200, 12, 20, 15,NULL,95.50),
('Aldsfice2', 'alice@example.com', 'hashedpassword123', 1, 1, GETDATE(), 'alice.jpg', 1200, 5, 10, 7,NULL,95.50),
('Bobsdf2', 'bob@example.com', 'securepassword456', 0, 0, GETDATE(), 'bob.jpg', 800, 3, 6, 2,'2024-12-31 14:30:00',95.50),
('Chdsfarli2e', 'charlie@example.com', 'mypassword789', 1, 1, GETDATE(), 'charlie.jpg', 2000, 10, 15, 12,NULL,95.50),
('Dadsfvid2', 'david@example.com', 'pass123', 1, 0, GETDATE(), 'david.jpg', 1500, 7, 12, 5,'2025-03-25 8:30:00',95.50),
('Esdfmma2', 'emma@example.com', 'emmaPass456', 1, 1, GETDATE(), 'emma.jpg', 1800, 9, 18, 10,NULL,95.50),
('Frdsfank2', 'frank@example.com', 'frankSecure', 0, 1, GETDATE(), 'frank.jpg', 2200, 12, 20, 15,NULL,95.50);

GO
INSERT INTO Categories (Id, Name) VALUES
(1, 'General-Discussion'),
(2, 'Lesson-Help'),
(3, 'Off-topic'),
(4, 'Discover'),
(5, 'Announcements');
go

INSERT INTO Friends (UserId1, UserId2)
VALUES 
    (2, 3),  -- Bob and Charlie
    (4, 5),  -- David and Emma
    (5, 6);  -- Emma and Frank

INSERT INTO Friends (UserId1, UserId2)
VALUES 
    (1, 6),  -- Bob and Charlie
    (1, 5),  -- David and Emma
    (1, 2),  -- Emma and Frank
	(1,4);

SELECT * FROM Friends;
GO

insert into Hashtags values ('a'), ('Mac'), ('Tech'), ('Innovation'), ('AI'), ('MachineLearning'), ('Coding'), ('Cybersecurity'), ('CloudComputing'), ('WebDevelopment'), ('MobileTech'), ('FutureOfTech');

INSERT INTO Posts (Title, Description, UserID, CategoryID) VALUES
('New Gadget', 'A review of the latest tech gadget.', 1, 2),
('Paris Trip', 'My amazing trip to Paris and the Eiffel Tower.', 2, 2),
('Delicious Pizza', 'Tried a new pizza place, it was fantastic!', 3, 3),
('Coding Tips', 'Some useful tips for beginner programmers.', 1, 1),
('Mountain Hike', 'Hiking in the beautiful mountains.', 2, 2),
('Homemade Pasta', 'Making fresh pasta at home.', 3, 3),
('AI Advancements', 'The latest developments in artificial intelligence.', 1, 1),
('Tokyo Adventures', 'Exploring the streets of Tokyo.', 2, 2),
('Chocolate Cake','A delicious chocolate cake recipe.', 3, 3),
('Cloud Security', 'Understanding the importance of cloud security.', 1, 1),
('London Calling', 'Exploring the iconic landmarks of London.', 2, 2),
('Burger Mania', 'Trying out the best burger joint in town.', 3, 3),
('Python Tricks', 'Advanced techniques in Python programming.', 1, 1),
('Italian Dolomites', 'A breathtaking hike in the Italian Dolomites.', 28, 2),
('Vegan Feast', 'Preparing a delicious and healthy vegan meal.', 3, 3),
('Quantum Computing', 'An overview of the basics of quantum computing.', 1, 1),
('New York City Guide', 'My recommendations for visiting New York City.', 3, 2),
('Sushi Night', 'Enjoying a delightful sushi dinner.', 2, 3),
('VR Development', 'Getting started with Virtual Reality development.', 4, 1);
GO

INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Great post!', 1, 1, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('I agree!', 2, 1, 1, 2);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Totally!', 3, 1, 2, 3);
go

INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Thanks for sharing!', 3, 2, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('My pleasure!', 1, 2, 4, 2);
go

INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('This is helpful.', 1, 3, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Glad you found it so!',2, 3, 7, 2);
go

INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Interesting perspective.', 4, 4, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Care to elaborate?', 3, 4, 9, 2);

INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Where did you take this hike?', 5, 5, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('In the mountains nearby!', 7, 5, 11, 2);

INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Looks delicious!', 6, 6, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('It was indeed!', 8, 6, 13, 2);

INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Can you elaborate on that?', 7, 7, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Sure thing...', 9, 7, 15, 2);

INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Beautiful scenery!', 8, 8, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Absolutely stunning!', 10, 8, 17, 2);

INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('I should try this recipe.', 9, 9, NULL, 1);

INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('What are your thoughts on this?', 10, 10, NULL, 1);
go

-- Additional 10 comments for posts 1 to 10 --
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Love this content!', 3, 1, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Very insightful.', 4, 2, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Well explained.', 5, 3, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Could you clarify?', 6, 4, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Great hike!', 7, 5, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('I have the same recipe!', 8, 6, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Very thought-provoking.', 9, 7, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Planning to visit soon!', 10, 8, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Great discussion.', 1, 9, NULL, 1);
INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level) VALUES
('Really useful information!', 2, 10, NULL, 1);
go

INSERT INTO PostHashtags (PostID, HashtagID) VALUES
(1, 3),  -- New Gadget -> Tech
(1, 4),  -- New Gadget -> Innovation
(1, 11), -- New Gadget -> MobileTech

(2, 1),  -- Paris Trip -> a
(2, 12), -- Paris Trip -> FutureOfTech

(3, 1),  -- Delicious Pizza -> a

(4, 7),  -- Coding Tips -> Coding
(4, 3),  -- Coding Tips -> Tech

(5, 1),  -- Mountain Hike -> a

(6, 1),  -- Homemade Pasta -> a

(7, 5),  -- AI Advancements -> AI
(7, 6),  -- AI Advancements -> MachineLearning
(7, 4),  -- AI Advancements -> Innovation

(8, 1),  -- Tokyo Adventures -> a

(9, 1),  -- Chocolate Cake -> a

(10, 8), -- Cloud Security -> Cybersecurity
(10, 9), -- Cloud Security -> CloudComputing

(11, 1), -- London Calling

(12, 1), -- Burger Mania

(13, 7), -- Python Tricks -> Coding
(13, 3), -- Python Tricks -> Tech

(14, 1), -- Italian Dolomites -> a

(15, 1), -- Vegan Feast -> a

(16, 4), -- Quantum Computing -> Innovation
(16, 6), -- Quantum Computing -> MachineLearning
(16, 10), -- Quantum Computing -> WebDevelopment

(17, 1), -- New York City Guide

(18, 1), -- Sushi Night 

(19, 4), -- VR Development -> Innovation
(19, 11); -- VR Development -> MobileTech
go
-- Insert achievements for streaks
INSERT INTO Achievements (Name, Description, Rarity) VALUES
('10 Day Streak', 'Achieve a 10-day streak', 'Common'),
('50 Day Streak', 'Achieve a 50-day streak', 'Uncommon'),
('100 Day Streak', 'Achieve a 100-day streak', 'Rare'),
('250 Day Streak', 'Achieve a 250-day streak', 'Epic'),
('500 Day Streak', 'Achieve a 500-day streak', 'Legendary'),
('1000 Day Streak', 'Achieve a 1000-day streak', 'Mythic');

-- Insert achievements for quizzes completed
INSERT INTO Achievements (Name, Description, Rarity) VALUES
('10 Quizzes Completed', 'Complete 10 quizzes', 'Common'),
('50 Quizzes Completed', 'Complete 50 quizzes', 'Uncommon'),
('100 Quizzes Completed', 'Complete 100 quizzes', 'Rare'),
('250 Quizzes Completed', 'Complete 250 quizzes', 'Epic'),
('500 Quizzes Completed', 'Complete 500 quizzes', 'Legendary'),
('1000 Quizzes Completed', 'Complete 1000 quizzes', 'Mythic');

-- Insert achievements for courses completed
INSERT INTO Achievements (Name, Description, Rarity) VALUES
('1 Course Completed', 'Complete 10 courses', 'Common'),
('5 Courses Completed', 'Complete 50 courses', 'Uncommon'),
('10 Courses Completed', 'Complete 100 courses', 'Rare'),
('25 Courses Completed', 'Complete 250 courses', 'Epic'),
('50 Courses Completed', 'Complete 500 courses', 'Legendary'),
('100 Courses Completed', 'Complete 1000 courses', 'Mythic');
go

INSERT INTO FriendRequests (SenderId, ReceiverId, RequestDate)
VALUES
-- Pending friend requests
(1, 2, GETDATE()), -- Alice → Bob (Pending)
(3, 4,  GETDATE()), -- Charlie → David (Pending)

-- Accepted friend requests
(2, 3, GETDATE()), -- Bob → Charlie (Accepted)
(4, 5, GETDATE()), -- David → Emma (Accepted)
(5, 6, GETDATE()); -- Emma → Frank (Accepted)
GO

INSERT INTO Categories (Id, Name) VALUES
(1, 'General-Discussion'),
(2, 'Lesson-Help'),
(3, 'Off-topic'),
(4, 'Discover'),
(5, 'Announcements');

-- =============================================
-- INSERT Hashtags (IF NOT EXISTS)
-- =============================================
INSERT INTO Hashtags (Tag)
SELECT Tag
FROM (VALUES 
	('Geography'),
	('CulturalGeography'),
	('PhysicalGeography'),
	('PlateTectonics'),
	('ClimateChange'),
	('Geopolitics'),
	('Travel'),
	('Adventure'),
	('LessonHelp'),
	('Announcements'),
	('Discover'),
	('OffTopic'),
	('UrbanPlanning'),
	('Geospatial'),
	('Cartography'),
	('GIS'),
	('RemoteSensing'),
	('Sustainability'),
	('CulturalHeritage'),
	('PopulationDynamics')
) AS source (Tag)
WHERE NOT EXISTS (
	SELECT 1 
	FROM Hashtags 
	WHERE Hashtags.Tag = source.Tag
);


BEGIN TRY
    BEGIN TRANSACTION;

	-- Check and update Posts.Title if needed
    IF COL_LENGTH('Posts', 'Title') < 200
    BEGIN
        ALTER TABLE Posts
        ALTER COLUMN Title NVARCHAR(200) NOT NULL;
    END

    -- Check and update Comments.Content if needed
    IF COL_LENGTH('Comments', 'Content') < 2000
    BEGIN
        ALTER TABLE Comments
        ALTER COLUMN Content NVARCHAR(2000) NOT NULL;
    END

    -- Declare variables to hold IDs
    DECLARE @PostID INT;
    DECLARE @CommentID INT;

	--General-Discussion

    -- =============================================
    -- POST 1: Rising Tides: The Future of Coastal Urban Centers
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Rising Tides: The Future of Coastal Urban Centers')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Rising Tides: The Future of Coastal Urban Centers',
            CONCAT(
                '## Sea-Level Rise and Urban Vulnerability  ', CHAR(13), CHAR(10),
                '*Key points:*  ', CHAR(13), CHAR(10),
                '- Projected sea-level rise by 2100: 0.3-2.5 meters  ', CHAR(13), CHAR(10),
                '- Cities at risk: Miami, Venice, Dhaka, Jakarta  ', CHAR(13), CHAR(10),
                '- Adaptation strategies:  ', CHAR(13), CHAR(10),
                '  1. Sea walls and barriers  ', CHAR(13), CHAR(10),
                '  2. Floating architecture  ', CHAR(13), CHAR(10),
                '  3. Managed retreat  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                '### Economic Impacts:  ', CHAR(13), CHAR(10),
                '| City    | Potential GDP Loss by 2050 |  ', CHAR(13), CHAR(10),
                '|---------|----------------------------|  ', CHAR(13), CHAR(10),
                '| Miami   | $3.5 billion               |  ', CHAR(13), CHAR(10),
                '| Jakarta | $10 billion                |  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                'How should cities prioritize their adaptation efforts?'
            ),
            2,
            1
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'ClimateChange')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'UrbanPlanning')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Sustainability'));
        
        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('What are the most vulnerable cities?', 3, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Miami is already experiencing ''sunny day flooding.''', 4, @PostID, @CommentID, 2);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Interesting, I thought Venice was the worst case.', 5, @PostID, @CommentID, 3);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Are there successful adaptation examples?', 6, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('The Netherlands has advanced flood management.', 7, @PostID, @CommentID, 2);
    END

    -- =============================================
    -- POST 2: Renewable Energy: Path to 100%?
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Renewable Energy: Path to 100%?')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Renewable Energy: Path to 100%?',
            CONCAT(
                '## The Renewable Revolution  ', CHAR(13), CHAR(10),
                '*Debate points:*  ', CHAR(13), CHAR(10),
                '- Global renewable energy share: 29%  ', CHAR(13), CHAR(10),
                '- Challenges to 100% renewables:  ', CHAR(13), CHAR(10),
                '  1. Intermittency  ', CHAR(13), CHAR(10),
                '  2. Storage technology  ', CHAR(13), CHAR(10),
                '  3. Grid infrastructure  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                '### Country Comparisons:  ', CHAR(13), CHAR(10),
                '| Country | Renewable % | Target Year |  ', CHAR(13), CHAR(10),
                '|---------|-------------|-------------|  ', CHAR(13), CHAR(10),
                '| Iceland | 100%        | Achieved    |  ', CHAR(13), CHAR(10),
                '| Germany | 46%         | 2050        |  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                'Is 100% renewable feasible by 2050?'
            ),
            3,
            1
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'ClimateChange')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Sustainability')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Geopolitics'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('What about nuclear energy?', 8, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Nuclear has waste and safety issues.', 9, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Battery tech seems key.', 10, @PostID, 1);
    END

    -- =============================================
    -- POST 3: Water Wars: Geopolitics of Scarcity
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Water Wars: Geopolitics of Scarcity')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Water Wars: Geopolitics of Scarcity',
            CONCAT(
                '## The Blue Gold  ', CHAR(13), CHAR(10),
                '*Key issues:*  ', CHAR(13), CHAR(10),
                '- 2.2 billion lack safe water  ', CHAR(13), CHAR(10),
                '- Conflicts: Nile, Indus  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                '### Solutions:  ', CHAR(13), CHAR(10),
                '- Water treaties  ', CHAR(13), CHAR(10),
                '- Desalination  ', CHAR(13), CHAR(10),
                'Can diplomacy prevent water wars?'
            ),
            4,
            1
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Geopolitics')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'ClimateChange'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Is desalination viable?', 11, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('It’s energy-intensive but improving.', 12, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('What about water trade?', 13, @PostID, 1);
    END

    -- =============================================
    -- POST 4: Saving Languages: Cultural Geography
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Saving Languages: Cultural Geography')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Saving Languages: Cultural Geography',
            CONCAT(
                '## Linguistic Diversity  ', CHAR(13), CHAR(10),
                '*Facts:*  ', CHAR(13), CHAR(10),
                '- 7,000+ languages worldwide  ', CHAR(13), CHAR(10),
                '- 40% at risk  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                '### Efforts:  ', CHAR(13), CHAR(10),
                '- UNESCO programs  ', CHAR(13), CHAR(10),
                'Why preserve languages?'
            ),
            5,
            1
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'CulturalGeography')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'CulturalHeritage'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Language ties to identity.', 14, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Totally, it’s our heritage!', 1, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Tech can help, right?', 1, @PostID, 1);
    END

    -- =============================================
    -- POST 5: Living with Tectonics
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Living with Tectonics')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Living with Tectonics',
            CONCAT(
                '## The Dynamic Earth  ', CHAR(13), CHAR(10),
                '*Science:*  ', CHAR(13), CHAR(10),
                '- Plate tectonics  ', CHAR(13), CHAR(10),
                '- Seismic zones  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                '### Preparedness:  ', CHAR(13), CHAR(10),
                '- Warning systems  ', CHAR(13), CHAR(10),
                'How to prepare better?'
            ),
            6,
            1
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PhysicalGeography')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PlateTectonics'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Japan’s preparedness is top-notch.', 2, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Still faces tsunami risks.', 3, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Can we predict quakes?', 4, @PostID, 1);
    END

    -- =============================================
    -- POST 6: Urbanization in the Global South
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Urbanization in the Global South')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Urbanization in the Global South',
            CONCAT(
                '## Rapid Urban Growth  ', CHAR(13), CHAR(10),
                '*Stats:*  ', CHAR(13), CHAR(10),
                '- 90% urban growth here  ', CHAR(13), CHAR(10),
                '- 1 billion in slums  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                '### Challenges:  ', CHAR(13), CHAR(10),
                '- Infrastructure gaps  ', CHAR(13), CHAR(10),
                'Sustainable models?'
            ),
            7,
            1
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'UrbanPlanning')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Sustainability'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Community planning is key.', 5, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Yes, involve locals!', 6, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Smart cities viable?', 8, @PostID, 1);
    END

    -- =============================================
    -- POST 7: GIS: Mapping Our World
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'GIS: Mapping Our World')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'GIS: Mapping Our World',
            CONCAT(
                '## Geospatial Tech  ', CHAR(13), CHAR(10),
                '*Uses:*  ', CHAR(13), CHAR(10),
                '- Urban planning  ', CHAR(13), CHAR(10),
                '- Disaster response  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                '### Tools:  ', CHAR(13), CHAR(10),
                '- ArcGIS, QGIS  ', CHAR(13), CHAR(10),
                'GIS impact?'
            ),
            9,
            1
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'GIS')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Geospatial'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('GIS is a game-changer.', 10, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Totally agree!', 11, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Open-source options?', 12, @PostID, 1);
    END

    -- =============================================
    -- POST 8: Sustainable Tourism: Balancing Act
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Sustainable Tourism: Balancing Act')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Sustainable Tourism: Balancing Act',
            CONCAT(
                '## Tourism & Environment  ', CHAR(13), CHAR(10),
                '*Impacts:*  ', CHAR(13), CHAR(10),
                '- Carbon footprint  ', CHAR(13), CHAR(10),
                '- Overtourism  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                '### Solutions:  ', CHAR(13), CHAR(10),
                '- Eco-tourism  ', CHAR(13), CHAR(10),
                'More sustainable?'
            ),
            10,
            1
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Travel')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Sustainability'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Locals must benefit.', 13, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Agreed, keep profits local.', 14, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Virtual tourism?', 1, @PostID, 1);
    END

    -- =============================================
    -- POST 9: Migration Trends: Global Perspective
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Migration Trends: Global Perspective')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Migration Trends: Global Perspective',
            CONCAT(
                '## Human Mobility  ', CHAR(13), CHAR(10),
                '*Trends:*  ', CHAR(13), CHAR(10),
                '- 281M migrants (2020)  ', CHAR(13), CHAR(10),
                '- Climate migration rising  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                '### Factors:  ', CHAR(13), CHAR(10),
                '- Economy, conflict  ', CHAR(13), CHAR(10),
                'Policy solutions?'
            ),
            11,
            1
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PopulationDynamics')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Geopolitics'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Migration is complex.', 1, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Needs diverse solutions.', 2, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Cultural impacts?', 3, @PostID, 1);
    END

    -- =============================================
    -- POST 10: Food Security in a Changing Climate
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Food Security in a Changing Climate')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Food Security in a Changing Climate',
            CONCAT(
                '## Feeding the World  ', CHAR(13), CHAR(10),
                '*Challenges:*  ', CHAR(13), CHAR(10),
                '- Climate impacts on crops  ', CHAR(13), CHAR(10),
                '- Soil degradation  ', CHAR(13), CHAR(10),
                '  ', CHAR(13), CHAR(10),
                '### Innovations:  ', CHAR(13), CHAR(10),
                '- Vertical farming  ', CHAR(13), CHAR(10),
                'Food for all?'
            ),
            12,
            1
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'ClimateChange')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Sustainability'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Agroecology could help.', 4, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Yes, nature-friendly farming!', 5, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Reduce waste too?', 6, @PostID, 1);
    END

	--Lesson-Help
	IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Lat & Long Basics')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Lat & Long Basics',
            CONCAT(
                '## Understanding Coordinates', CHAR(13), CHAR(10),
                'Latitude and longitude are used to locate any point on Earth.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Key Points:', CHAR(13), CHAR(10),
                '- **Latitude**: Measures north-south (0° at Equator, 90° at poles).', CHAR(13), CHAR(10),
                '- **Longitude**: Measures east-west (0° at Prime Meridian, 180° at International Date Line).', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Example:', CHAR(13), CHAR(10),
                '| Location   | Latitude | Longitude |', CHAR(13), CHAR(10),
                '|------------|----------|-----------|', CHAR(13), CHAR(10),
                '| New York   | 40.7128° N | 74.0060° W |', CHAR(13), CHAR(10),
                '| Sydney     | 33.8688° S | 151.2093° E |', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                'How do you read coordinates on a map?'
            ),
            1,
            2
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Geography')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Cartography'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Does longitude affect time?', 2, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Yes, every 15° of longitude equals 1 hour of time difference.', 3, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('What’s the easiest way to remember lat vs long?', 4, @PostID, 1);
    END

    -- **Post 2: Volcanoes: Types and Formation**
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Volcano Types')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Volcano Types',
            CONCAT(
                '## Exploring Volcanoes', CHAR(13), CHAR(10),
                'Volcanoes form where tectonic plates interact or magma escapes.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Types of Volcanoes:', CHAR(13), CHAR(10),
                '- **Shield**: Broad, gentle slopes (e.g., Mauna Loa).', CHAR(13), CHAR(10),
                '- **Stratovolcano**: Steep, explosive (e.g., Mount Fuji).', CHAR(13), CHAR(10),
                '- **Cinder Cone**: Small, steep-sided (e.g., Paricutin).', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Formation:', CHAR(13), CHAR(10),
                '| Type         | Cause                  |', CHAR(13), CHAR(10),
                '|--------------|------------------------|', CHAR(13), CHAR(10),
                '| Shield       | Low-viscosity lava     |', CHAR(13), CHAR(10),
                '| Stratovolcano| High-viscosity lava    |', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                'What causes some volcanoes to be more explosive?'
            ),
            5,
            2
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PlateTectonics')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PhysicalGeography'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Why are stratovolcanoes so dangerous?', 6, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Their thick lava traps gases, leading to explosive eruptions.', 7, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Are there active volcanoes near me?', 8, @PostID, 1);
    END

    -- **Post 3: Biomes: Earth’s Ecosystems**
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Biome Basics')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Biome Basics',
            CONCAT(
                '## What Are Biomes?', CHAR(13), CHAR(10),
                'Biomes are large regions defined by climate, flora, and fauna.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Major Biomes:', CHAR(13), CHAR(10),
                '- **Tundra**: Cold, treeless, permafrost.', CHAR(13), CHAR(10),
                '- **Rainforest**: Warm, wet, biodiverse.', CHAR(13), CHAR(10),
                '- **Desert**: Dry, extreme temperatures.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Examples:', CHAR(13), CHAR(10),
                '| Biome      | Location         |', CHAR(13), CHAR(10),
                '|------------|------------------|', CHAR(13), CHAR(10),
                '| Tundra     | Arctic           |', CHAR(13), CHAR(10),
                '| Rainforest | Amazon Basin     |', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                'How do biomes affect biodiversity?'
            ),
            9,
            2
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PhysicalGeography')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Sustainability'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('What’s the difference between a biome and an ecosystem?', 10, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('A biome is a large region; an ecosystem is a smaller, specific community.', 11, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Which biome has the most species?', 12, @PostID, 1);
    END

    -- **Post 4: Urbanization: Growth of Cities**
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Urbanization 101')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Urbanization 101',
            CONCAT(
                '## Rise of Urban Areas', CHAR(13), CHAR(10),
                'Urbanization is the shift from rural to urban living.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Causes:', CHAR(13), CHAR(10),
                '- **Industrialization**: Job opportunities in cities.', CHAR(13), CHAR(10),
                '- **Migration**: People seeking better services.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Impacts:', CHAR(13), CHAR(10),
                '| Aspect     | Effect            |', CHAR(13), CHAR(10),
                '|------------|-------------------|', CHAR(13), CHAR(10),
                '| Population | Density increases |', CHAR(13), CHAR(10),
                '| Environment| Pollution rises   |', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                'What are the pros and cons of urbanization?'
            ),
            13,
            2
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'CulturalGeography')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PopulationDynamics'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How does urbanization affect rural areas?', 14, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Rural areas may lose population and resources.', 1, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Are megacities sustainable?', 1, @PostID, 1);
    END

    -- **Post 5: Glaciers: Formation and Movement**
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Glacier Dynamics')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Glacier Dynamics',
            CONCAT(
                '## Ice on the Move', CHAR(13), CHAR(10),
                'Glaciers form from compacted snow and shape landscapes.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Formation:', CHAR(13), CHAR(10),
                '- Snow accumulates and compresses into ice.', CHAR(13), CHAR(10),
                '- Requires cold climate and high snowfall.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Movement:', CHAR(13), CHAR(10),
                '| Type       | Description         |', CHAR(13), CHAR(10),
                '|------------|---------------------|', CHAR(13), CHAR(10),
                '| Internal   | Ice deforms         |', CHAR(13), CHAR(10),
                '| Basal      | Slides over ground  |', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                'How do glaciers carve valleys?'
            ),
            2,
            2
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PhysicalGeography')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'ClimateChange'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Are glaciers shrinking due to climate change?', 3, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Yes, many are retreating rapidly.', 4, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('What’s the biggest glacier in the world?', 5, @PostID, 1);
    END

    -- **Post 6: Migration Patterns Explained**
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Migration Patterns')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Migration Patterns',
            CONCAT(
                '## Why People Move', CHAR(13), CHAR(10),
                'Migration shapes populations and cultures globally.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Types:', CHAR(13), CHAR(10),
                '- **Forced**: Refugees fleeing conflict.', CHAR(13), CHAR(10),
                '- **Voluntary**: Seeking jobs or education.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Examples:', CHAR(13), CHAR(10),
                '| Type       | Example             |', CHAR(13), CHAR(10),
                '|------------|---------------------|', CHAR(13), CHAR(10),
                '| Forced     | Syrian refugees     |', CHAR(13), CHAR(10),
                '| Voluntary  | Rural-to-urban move |', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                'What drives migration in your region?'
            ),
            6,
            2
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'CulturalGeography')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PopulationDynamics'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How does migration affect culture?', 7, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('It can blend traditions or spark new ones.', 8, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('What’s the difference between immigration and emigration?', 9, @PostID, 1);
    END

    -- **Post 7: Deserts: Features and Life**
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Desert Ecosystems')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Desert Ecosystems',
            CONCAT(
                '## Life in the Dry', CHAR(13), CHAR(10),
                'Deserts are arid regions with unique adaptations.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Features:', CHAR(13), CHAR(10),
                '- **Hot Deserts**: High daytime temps (e.g., Sahara).', CHAR(13), CHAR(10),
                '- **Cold Deserts**: Freezing nights (e.g., Gobi).', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Adaptations:', CHAR(13), CHAR(10),
                '| Organism   | Adaptation         |', CHAR(13), CHAR(10),
                '|------------|--------------------|', CHAR(13), CHAR(10),
                '| Cactus     | Stores water       |', CHAR(13), CHAR(10),
                '| Camel      | Hump for fat       |', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                'How do plants survive in deserts?'
            ),
            10,
            2
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PhysicalGeography'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('What’s the largest desert?', 11, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Antarctica, technically—it’s a cold desert!', 12, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How do animals cope with no water?', 13, @PostID, 1);
    END

    -- **Post 8: Ocean Currents: Global Flow**
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Ocean Currents')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Ocean Currents',
            CONCAT(
                '## Water on the Move', CHAR(13), CHAR(10),
                'Ocean currents distribute heat and nutrients worldwide.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Types:', CHAR(13), CHAR(10),
                '- **Surface Currents**: Driven by wind (e.g., Gulf Stream).', CHAR(13), CHAR(10),
                '- **Deep Currents**: Driven by density (e.g., Thermohaline).', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Effects:', CHAR(13), CHAR(10),
                '| Current    | Impact             |', CHAR(13), CHAR(10),
                '|------------|--------------------|', CHAR(13), CHAR(10),
                '| Gulf Stream| Warms Western Europe |', CHAR(13), CHAR(10),
                '| El Niño    | Alters weather     |', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                'How do currents affect climate?'
            ),
            14,
            2
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PhysicalGeography')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'ClimateChange'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('What causes ocean currents?', 5, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Wind, temperature, and salinity differences drive them.', 1, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How does El Niño change rainfall?', 2, @PostID, 1);
    END

    -- **Post 9: Landforms: Shaping Earth**
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Landform Lessons')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Landform Lessons',
            CONCAT(
                '## Earth’s Features', CHAR(13), CHAR(10),
                'Landforms are natural shapes on Earth’s surface.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Types:', CHAR(13), CHAR(10),
                '- **Mountains**: Tectonic uplift (e.g., Alps).', CHAR(13), CHAR(10),
                '- **Plains**: Flat, depositional (e.g., Great Plains).', CHAR(13), CHAR(10),
                '- **Plateaus**: Elevated flatlands (e.g., Tibetan Plateau).', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Formation:', CHAR(13), CHAR(10),
                '| Landform  | Process            |', CHAR(13), CHAR(10),
                '|-----------|--------------------|', CHAR(13), CHAR(10),
                '| Mountains | Plate collision    |', CHAR(13), CHAR(10),
                '| Plains    | Sediment deposit   |', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                'What’s your favorite landform?'
            ),
            3,
            2
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PhysicalGeography')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'PlateTectonics'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How long does it take to form a mountain?', 4, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Millions of years through tectonic activity.', 5, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Are plateaus good for farming?', 6, @PostID, 1);
    END

    -- **Post 10: Economic Geography Basics**
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Economic Geography')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Economic Geography',
            CONCAT(
                '## Resources and Trade', CHAR(13), CHAR(10),
                'Economic geography studies how location affects economies.', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Key Concepts:', CHAR(13), CHAR(10),
                '- **Primary**: Extraction (e.g., mining).', CHAR(13), CHAR(10),
                '- **Secondary**: Manufacturing (e.g., factories).', CHAR(13), CHAR(10),
                '- **Tertiary**: Services (e.g., retail).', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                '### Examples:', CHAR(13), CHAR(10),
                '| Sector    | Region             |', CHAR(13), CHAR(10),
                '|-----------|--------------------|', CHAR(13), CHAR(10),
                '| Primary   | Oil-rich Middle East |', CHAR(13), CHAR(10),
                '| Tertiary  | Silicon Valley     |', CHAR(13), CHAR(10),
                CHAR(13), CHAR(10),
                'How does geography influence your local economy?'
            ),
            7,
            2
        );
        
        SET @PostID = SCOPE_IDENTITY();
        
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'CulturalGeography')),
            (@PostID, (SELECT Id FROM Hashtags WHERE Tag = 'Sustainability'));
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Why do some regions specialize in one sector?', 8, @PostID, 1);
        
        SET @CommentID = SCOPE_IDENTITY();
        
        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('It depends on resources and infrastructure.', 9, @PostID, @CommentID, 2);
        
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How does trade connect economic geography?', 10, @PostID, 1);
    END

	--Off-topic

	-- Post 1: Coastal Cities at Risk
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Coastal Cities at Risk')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Coastal Cities at Risk',
            CONCAT(
                '## Rising Sea Levels Threaten Urban Centers  ', CHAR(13), CHAR(10),
                '*Key points:*  ', CHAR(13), CHAR(10),
                '- Global sea level rise: 0.3-1.2m by 2100  ', CHAR(13), CHAR(10),
                '- Case studies:  ', CHAR(13), CHAR(10),
                '  1. Miami: Frequent flooding  ', CHAR(13), CHAR(10),
                '  2. Jakarta: Sinking city  ', CHAR(13), CHAR(10),
                '  3. Venice: MOSE barriers  ', CHAR(13), CHAR(10),
                '### Mitigation Strategies:  ', CHAR(13), CHAR(10),
                '| Approach      | Effectiveness | Cost |  ', CHAR(13), CHAR(10),
                '|---------------|---------------|------|  ', CHAR(13), CHAR(10),
                '| Sea Walls     | Medium        | High |  ', CHAR(13), CHAR(10),
                '| Mangroves     | High          | Low  |  ', CHAR(13), CHAR(10),
                'How can cities adapt?'
            ),
            2,  -- UserID
            3   
        );

        DECLARE @CoastalPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@CoastalPostID, (SELECT Id FROM Hashtags WHERE Tag = 'ClimateChange')),
            (@CoastalPostID, (SELECT Id FROM Hashtags WHERE Tag = 'UrbanPlanning'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('What about mangroves?', 3, @CoastalPostID, 1);

        DECLARE @MangroveCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('They’re natural barriers!', 4, @CoastalPostID, @MangroveCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Are they enough though?', 5, @CoastalPostID, 1);
    END

    -- Post 2: Mapping Language Diversity
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Mapping Language Diversity')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Mapping Language Diversity',
            CONCAT(
                '## Language and Culture  ', CHAR(13), CHAR(10),
                '*Highlights:*  ', CHAR(13), CHAR(10),
                '- Maps reveal history  ', CHAR(13), CHAR(10),
                '- Examples:  ', CHAR(13), CHAR(10),
                '  1. India: 22 languages  ', CHAR(13), CHAR(10),
                '  2. Switzerland: 4 languages  ', CHAR(13), CHAR(10),
                '### Techniques:  ', CHAR(13), CHAR(10),
                '| Method       | Pros          | Cons          |  ', CHAR(13), CHAR(10),
                '|--------------|---------------|---------------|  ', CHAR(13), CHAR(10),
                '| Choropleth   | Easy to read  | Oversimplifies|  ', CHAR(13), CHAR(10),
                '| Dot Density  | Detailed      | Cluttered     |  ', CHAR(13), CHAR(10),
                'What can maps teach us?'
            ),
            3,  -- UserID
            3   -- CategoryID: General-Discussion
        );

        DECLARE @LanguagePostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@LanguagePostID, (SELECT Id FROM Hashtags WHERE Tag = 'CulturalGeography')),
            (@LanguagePostID, (SELECT Id FROM Hashtags WHERE Tag = 'Cartography'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How to map dialects?', 6, @LanguagePostID, 1);

        DECLARE @DialectCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('As subgroups, usually.', 7, @LanguagePostID, @DialectCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Any software tips?', 8, @LanguagePostID, 1);
    END

    -- Post 1: Plate Tectonics 101
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Plate Tectonics 101')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Plate Tectonics 101',
            CONCAT(
                '## Earth’s Moving Plates  ', CHAR(13), CHAR(10),
                '*Basics:*  ', CHAR(13), CHAR(10),
                '- Theory explained  ', CHAR(13), CHAR(10),
                '- Boundary types:  ', CHAR(13), CHAR(10),
                '  1. Convergent: Collision  ', CHAR(13), CHAR(10),
                '  2. Divergent: Spreading  ', CHAR(13), CHAR(10),
                '  3. Transform: Sliding  ', CHAR(13), CHAR(10),
                'How do plates shape Earth?'
            ),
            4,  -- UserID
            3   -- CategoryID: Lesson-Help
        );

        DECLARE @PlatePostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PlatePostID, (SELECT Id FROM Hashtags WHERE Tag = 'PlateTectonics')),
            (@PlatePostID, (SELECT Id FROM Hashtags WHERE Tag = 'LessonHelp'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Convergent vs divergent?', 9, @PlatePostID, 1);

        DECLARE @BoundaryCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Collide vs apart!', 10, @PlatePostID, @BoundaryCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Video resources?', 11, @PlatePostID, 1);
    END

    -- Post 2: Getting Started with GIS
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Getting Started with GIS')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Getting Started with GIS',
            CONCAT(
                '## GIS for Beginners  ', CHAR(13), CHAR(10),
                '*Intro:*  ', CHAR(13), CHAR(10),
                '- What is GIS?  ', CHAR(13), CHAR(10),
                '- Tools:  ', CHAR(13), CHAR(10),
                '  1. QGIS: Free, open-source  ', CHAR(13), CHAR(10),
                '  2. ArcGIS: Industry standard  ', CHAR(13), CHAR(10),
                'Try a simple project!'
            ),
            5,  -- UserID
            3   -- CategoryID: Lesson-Help
        );

        DECLARE @GISPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@GISPostID, (SELECT Id FROM Hashtags WHERE Tag = 'GIS')),
            (@GISPostID, (SELECT Id FROM Hashtags WHERE Tag = 'LessonHelp'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Best GIS for beginners?', 12, @GISPostID, 1);

        DECLARE @GISCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('QGIS—free and easy!', 13, @GISPostID, @GISCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Tutorials anywhere?', 14, @GISPostID, 1);
    END

    -- Post 1: Must-Visit Places
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Must-Visit Places')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Must-Visit Places',
            CONCAT(
                '## Geographer’s Travel List  ', CHAR(13), CHAR(10),
                '*Destinations:*  ', CHAR(13), CHAR(10),
                '- Iceland: Volcanic wonders  ', CHAR(13), CHAR(10),
                '- Silk Road: Historic routes  ', CHAR(13), CHAR(10),
                'Where’s your next trip?'
            ),
            6,  -- UserID
            3   -- CategoryID: Off-topic
        );

        DECLARE @TravelPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@TravelPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Travel')),
            (@TravelPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Adventure'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Atacama Desert?', 7, @TravelPostID, 1);

        DECLARE @DesertCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Yes, super dry!', 8, @TravelPostID, @DesertCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Urban spots?', 9, @TravelPostID, 1);
    END

    -- Post 2: Geography in Movies
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Geography in Movies')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Geography in Movies',
            CONCAT(
                '## Geography on Screen  ', CHAR(13), CHAR(10),
                '*Examples:*  ', CHAR(13), CHAR(10),
                '- Inception: Cityscapes  ', CHAR(13), CHAR(10),
                '- Planet Earth: Nature  ', CHAR(13), CHAR(10),
                'What’s your fave?'
            ),
            10,  -- UserID
            3   -- CategoryID: Off-topic
        );

        DECLARE @MoviesPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@MoviesPostID, (SELECT Id FROM Hashtags WHERE Tag = 'OffTopic')),
            (@MoviesPostID, (SELECT Id FROM Hashtags WHERE Tag = 'CulturalGeography'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Game of Thrones map!', 11, @MoviesPostID, 1);

        DECLARE @GOTCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Westeros is epic!', 12, @MoviesPostID, @GOTCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Board games?', 13, @MoviesPostID, 1);
    END


    -- Post 1: Lost City in Amazon
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Lost City in Amazon')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Lost City in Amazon',
            CONCAT(
                '## Archaeological Find  ', CHAR(13), CHAR(10),
                '*Details:*  ', CHAR(13), CHAR(10),
                '- LiDAR reveals cities  ', CHAR(13), CHAR(10),
                '- Amazon rainforest  ', CHAR(13), CHAR(10),
                'What else is hidden?'
            ),
            14,  -- UserID
            3   -- CategoryID: Discover
        );

        DECLARE @AmazonPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@AmazonPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Discover')),
            (@AmazonPostID, (SELECT Id FROM Hashtags WHERE Tag = 'CulturalHeritage'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How does LiDAR work?', 1, @AmazonPostID, 1);

        DECLARE @LiDARCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Penetrates trees!', 1, @AmazonPostID, @LiDARCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Virtual tours?', 2, @AmazonPostID, 1);
    END

    -- Post 2: AI in Geospatial
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'AI in Geospatial')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'AI in Geospatial',
            CONCAT(
                '## Tech Meets Geography  ', CHAR(13), CHAR(10),
                '*Uses:*  ', CHAR(13), CHAR(10),
                '- Land use analysis  ', CHAR(13), CHAR(10),
                '- Disaster prediction  ', CHAR(13), CHAR(10),
                'Future of geospatial?'
            ),
            3,  -- UserID
            3   -- CategoryID: Discover
        );

        DECLARE @AIPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@AIPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Geospatial')),
            (@AIPostID, (SELECT Id FROM Hashtags WHERE Tag = 'GIS'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('AI tools available?', 4, @AIPostID, 1);

        DECLARE @AIToolsCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Try TensorFlow!', 5, @AIPostID, @AIToolsCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Disaster accuracy?', 6, @AIPostID, 1);
    END

    -- Post 1: AAG Meeting 2024
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'AAG Meeting 2024')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'AAG Meeting 2024',
            CONCAT(
                '## Geography Conference  ', CHAR(13), CHAR(10),
                '*Details:*  ', CHAR(13), CHAR(10),
                '- Date: April 2024  ', CHAR(13), CHAR(10),
                '- Call for papers now!  ', CHAR(13), CHAR(10),
                'Join us there?'
            ),
            7,  -- UserID
            3   -- CategoryID: Announcements
        );

        DECLARE @AAGPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@AAGPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Announcements')),
            (@AAGPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Geopolitics'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Virtual option?', 8, @AAGPostID, 1);

        DECLARE @VirtualCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Yes, hybrid!', 9, @AAGPostID, @VirtualCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Theme this year?', 10, @AAGPostID, 1);
    END

    -- Post 2: GIS Analyst Job
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'GIS Analyst Job')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'GIS Analyst Job',
            CONCAT(
                '## Job Opportunity  ', CHAR(13), CHAR(10),
                '*Details:*  ', CHAR(13), CHAR(10),
                '- 3-5 yrs experience  ', CHAR(13), CHAR(10),
                '- Environmental firm  ', CHAR(13), CHAR(10),
                'Apply now!'
            ),
            11,  -- UserID
            3   -- CategoryID: Announcements
        );

        DECLARE @JobPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@JobPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Announcements')),
            (@JobPostID, (SELECT Id FROM Hashtags WHERE Tag = 'GIS'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Experience needed?', 12, @JobPostID, 1);

        DECLARE @ExpCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Mid-level, 3-5 yrs.', 13, @JobPostID, @ExpCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Remote work?', 14, @JobPostID, 1);
    END

	--Discover
	 -- Post 1: Volcanoes of Mars
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Volcanoes of Mars')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Volcanoes of Mars',
            CONCAT(
                '## Red Planet Giants  ', CHAR(13), CHAR(10),
                '*Highlights:*  ', CHAR(13), CHAR(10),
                '- Olympus Mons: 22 km high  ', CHAR(13), CHAR(10),
                '- Shield volcanoes dominate  ', CHAR(13), CHAR(10),
                'What can we learn?'
            ),
            1,  -- UserID
            4   -- CategoryID: Discover
        );

        DECLARE @MarsPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@MarsPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Discover')),
            (@MarsPostID, (SELECT Id FROM Hashtags WHERE Tag = 'PhysicalGeography'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How tall compared to Earth?', 2, @MarsPostID, 1);

        DECLARE @MarsCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('3x Everest!', 3, @MarsPostID, @MarsCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Active volcanoes?', 4, @MarsPostID, 1);
    END

    -- Post 2: Deep Ocean Trenches
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Deep Ocean Trenches')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Deep Ocean Trenches',
            CONCAT(
                '## Earth’s Hidden Depths  ', CHAR(13), CHAR(10),
                '*Key Facts:*  ', CHAR(13), CHAR(10),
                '- Mariana Trench: 11 km  ', CHAR(13), CHAR(10),
                '- Subduction zones  ', CHAR(13), CHAR(10),
                'What lives down there?'
            ),
            5,  -- UserID
            4   -- CategoryID: Discover
        );

        DECLARE @OceanPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@OceanPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Discover')),
            (@OceanPostID, (SELECT Id FROM Hashtags WHERE Tag = 'PhysicalGeography')),
            (@OceanPostID, (SELECT Id FROM Hashtags WHERE Tag = 'PlateTectonics'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Any new species found?', 6, @OceanPostID, 1);

        DECLARE @SpeciesCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Yes, weird fish!', 7, @OceanPostID, @SpeciesCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How do they map it?', 8, @OceanPostID, 1);
    END

    -- Post 3: Sahara’s Lost Rivers
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Sahara’s Lost Rivers')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Sahara’s Lost Rivers',
            CONCAT(
                '## Ancient Waterways  ', CHAR(13), CHAR(10),
                '*Discovery:*  ', CHAR(13), CHAR(10),
                '- Radar shows old rivers  ', CHAR(13), CHAR(10),
                '- Once lush Sahara  ', CHAR(13), CHAR(10),
                'Climate clues?'
            ),
            9,  -- UserID
            4   -- CategoryID: Discover
        );

        DECLARE @SaharaPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@SaharaPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Discover')),
            (@SaharaPostID, (SELECT Id FROM Hashtags WHERE Tag = 'ClimateChange'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How old are these?', 10, @SaharaPostID, 1);

        DECLARE @SaharaCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('10,000+ years!', 11, @SaharaPostID, @SaharaCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('More deserts like this?', 12, @SaharaPostID, 1);
    END

    -- Post 4: Ice Age Migration
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Ice Age Migration')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Ice Age Migration',
            CONCAT(
                '## Human Journeys  ', CHAR(13), CHAR(10),
                '*Findings:*  ', CHAR(13), CHAR(10),
                '- Bering Land Bridge  ', CHAR(13), CHAR(10),
                '- DNA evidence  ', CHAR(13), CHAR(10),
                'Where did we go?'
            ),
            13,  -- UserID
            4   -- CategoryID: Discover
        );

        DECLARE @IcePostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@IcePostID, (SELECT Id FROM Hashtags WHERE Tag = 'Discover')),
            (@IcePostID, (SELECT Id FROM Hashtags WHERE Tag = 'CulturalGeography'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How did they survive?', 14, @IcePostID, 1);

        DECLARE @IceCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Adaptation, tools!', 1, @IcePostID, @IceCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Other routes?', 1, @IcePostID, 1);
    END

    -- Post 5: Antarctica’s Secrets
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Antarctica’s Secrets')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Antarctica’s Secrets',
            CONCAT(
                '## Icy Mysteries  ', CHAR(13), CHAR(10),
                '*Revelations:*  ', CHAR(13), CHAR(10),
                '- Subglacial lakes  ', CHAR(13), CHAR(10),
                '- Ancient ice cores  ', CHAR(13), CHAR(10),
                'What’s beneath?'
            ),
            2,  -- UserID
            4   -- CategoryID: Discover
        );

        DECLARE @AntarcticaPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@AntarcticaPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Discover')),
            (@AntarcticaPostID, (SELECT Id FROM Hashtags WHERE Tag = 'PhysicalGeography'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Life in those lakes?', 3, @AntarcticaPostID, 1);

        DECLARE @LakeCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Microbes, yes!', 4, @AntarcticaPostID, @LakeCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How deep?', 5, @AntarcticaPostID, 1);
    END

    -- Post 6: New Island Birth
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'New Island Birth')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'New Island Birth',
            CONCAT(
                '## Volcanic Creation  ', CHAR(13), CHAR(10),
                '*Event:*  ', CHAR(13), CHAR(10),
                '- Hunga Tonga eruption  ', CHAR(13), CHAR(10),
                '- New land formed  ', CHAR(13), CHAR(10),
                'How long will it last?'
            ),
            6,  -- UserID
            4   -- CategoryID: Discover
        );

        DECLARE @IslandPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@IslandPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Discover')),
            (@IslandPostID, (SELECT Id FROM Hashtags WHERE Tag = 'PlateTectonics'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Erosion risk?', 7, @IslandPostID, 1);

        DECLARE @ErosionCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('High, it’s young!', 8, @IslandPostID, @ErosionCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('More examples?', 9, @IslandPostID, 1);
    END

    -- Post 7: Ancient Trade Routes
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Ancient Trade Routes')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Ancient Trade Routes',
            CONCAT(
                '## Paths of History  ', CHAR(13), CHAR(10),
                '*Routes:*  ', CHAR(13), CHAR(10),
                '- Amber Road: Europe  ', CHAR(13), CHAR(10),
                '- Incense Route: Arabia  ', CHAR(13), CHAR(10),
                'What did they carry?'
            ),
            10,  -- UserID
            4   -- CategoryID: Discover
        );

        DECLARE @TradePostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@TradePostID, (SELECT Id FROM Hashtags WHERE Tag = 'Discover')),
            (@TradePostID, (SELECT Id FROM Hashtags WHERE Tag = 'CulturalHeritage'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Goods traded?', 11, @TradePostID, 1);

        DECLARE @GoodsCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Spices, gems!', 12, @TradePostID, @GoodsCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Maps available?', 13, @TradePostID, 1);
    END

    -- Post 8: Magnetic Pole Shift
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Magnetic Pole Shift')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Magnetic Pole Shift',
            CONCAT(
                '## Earth’s Compass  ', CHAR(13), CHAR(10),
                '*Phenomenon:*  ', CHAR(13), CHAR(10),
                '- North Pole moving  ', CHAR(13), CHAR(10),
                '- Past reversals  ', CHAR(13), CHAR(10),
                'Impact on navigation?'
            ),
            14,  -- UserID
            4   -- CategoryID: Discover
        );

        DECLARE @PolePostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@PolePostID, (SELECT Id FROM Hashtags WHERE Tag = 'Discover')),
            (@PolePostID, (SELECT Id FROM Hashtags WHERE Tag = 'PhysicalGeography'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How fast is it?', 1, @PolePostID, 1);

        DECLARE @SpeedCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('55 km/year now!', 1, @PolePostID, @SpeedCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Affects GPS?', 2, @PolePostID, 1);
    END

    -- Post 9: Caves of Altamira
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Caves of Altamira')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Caves of Altamira',
            CONCAT(
                '## Prehistoric Art  ', CHAR(13), CHAR(10),
                '*Discovery:*  ', CHAR(13), CHAR(10),
                '- Spain, 35,000 yrs  ', CHAR(13), CHAR(10),
                '- Bison paintings  ', CHAR(13), CHAR(10),
                'What do they tell us?'
            ),
            3,  -- UserID
            4   -- CategoryID: Discover
        );

        DECLARE @CavePostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@CavePostID, (SELECT Id FROM Hashtags WHERE Tag = 'Discover')),
            (@CavePostID, (SELECT Id FROM Hashtags WHERE Tag = 'CulturalHeritage'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Art or symbols?', 4, @CavePostID, 1);

        DECLARE @ArtCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Maybe both!', 5, @CavePostID, @ArtCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('More caves like this?', 6, @CavePostID, 1);
    END

    -- Post 10: Exoplanet Climates
    IF NOT EXISTS (SELECT 1 FROM Posts WHERE Title = 'Exoplanet Climates')
    BEGIN
        INSERT INTO Posts (Title, Description, UserID, CategoryID)
        VALUES (
            'Exoplanet Climates',
            CONCAT(
                '## Beyond Earth  ', CHAR(13), CHAR(10),
                '*Exploration:*  ', CHAR(13), CHAR(10),
                '- TRAPPIST-1 system  ', CHAR(13), CHAR(10),
                '- Weather models  ', CHAR(13), CHAR(10),
                'Habitable worlds?'
            ),
            7,  -- UserID
            4   -- CategoryID: Discover
        );

        DECLARE @ExoPostID INT = SCOPE_IDENTITY();

        -- Link Hashtags
        INSERT INTO PostHashtags (PostID, HashtagID)
        VALUES 
            (@ExoPostID, (SELECT Id FROM Hashtags WHERE Tag = 'Discover')),
            (@ExoPostID, (SELECT Id FROM Hashtags WHERE Tag = 'ClimateChange'));

        -- Add Comments
        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('Water possible?', 8, @ExoPostID, 1);

        DECLARE @WaterCommentID INT = SCOPE_IDENTITY();

        INSERT INTO Comments (Content, UserID, PostID, ParentCommentID, Level)
        VALUES ('Some, maybe!', 9, @ExoPostID, @WaterCommentID, 2);

        INSERT INTO Comments (Content, UserID, PostID, Level)
        VALUES ('How do we know?', 10, @ExoPostID, 1);
    END

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error Number: ' + CAST(ERROR_NUMBER() AS VARCHAR(10));
    PRINT 'Error Message: ' + ERROR_MESSAGE();
    PRINT 'Error Severity: ' + CAST(ERROR_SEVERITY() AS VARCHAR(10));
    PRINT 'Error State: ' + CAST(ERROR_STATE() AS VARCHAR(10));
    PRINT 'Error Line: ' + CAST(ERROR_LINE() AS VARCHAR(10));
END CATCH;
GO



---procedures

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
