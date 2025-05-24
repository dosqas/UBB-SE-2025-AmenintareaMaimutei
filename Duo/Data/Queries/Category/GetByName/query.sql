CREATE OR ALTER PROCEDURE GetCategoryByName
	@name varchar(50)
AS
BEGIN
	SELECT * from Categories C
	where C.Name = @name
END
