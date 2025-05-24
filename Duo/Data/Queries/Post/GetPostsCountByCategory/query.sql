CREATE OR ALTER PROCEDURE GetCategoryPostCounts
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