create procedure GetUserByUsername
    @Username nvarchar(50)
as
begin
    select * from Users where Username = @Username
end

