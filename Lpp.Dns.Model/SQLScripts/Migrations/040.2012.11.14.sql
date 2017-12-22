if exists( select * from sys.tables where object_id = object_id('projects_users') )
begin
	drop table projects_users
end