declare @projectid uniqueidentifier
declare @allProjects uniqueidentifier
set @projectid = (select sid from projects where name='Default')
set @allProjects = '6A690001-7579-4C74-ADE1-A2210107FA29'

if not exists( select * from SecurityInheritance where [start] = @projectid and [end] = @allProjects )
begin
	insert into SecurityInheritance ([Start],[end])values(@projectid, @allProjects)
end
go