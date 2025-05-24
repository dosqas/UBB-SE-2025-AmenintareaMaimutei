CREATE OR ALTER PROCEDURE GetHashtagByText
    @text VARCHAR(20)
AS
BEGIN
    SELECT * FROM Hashtags WHERE Tag = @text;
END;