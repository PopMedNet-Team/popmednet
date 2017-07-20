if not exists( select * from sys.columns where name = 'description' and object_id = object_id( 'Projects' ) )
	alter table Projects add Description text
GO

declare @projectid uniqueidentifier
set @projectid = (select sid from projects where name='Default')

if not exists( select * from projects_users where projectid = @projectid)
begin
insert into projects_users (projectid, userid) select @projectid, userid from users
end
go