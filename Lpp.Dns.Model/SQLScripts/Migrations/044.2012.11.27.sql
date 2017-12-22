if not exists( select * from sys.columns where name = 'GroupId' and object_id = object_id('Projects') )
	alter table Projects add GroupId int references Groups
go

if exists( select * from Projects where GroupId is null ) begin
	if not exists( select * from Groups ) and exists( select * from Projects )
		insert into Groups( [GroupName] ) values( 'Root' )
	update Projects set GroupId = (select top 1 GroupId from Groups)
	alter table Projects alter column GroupId int not null
end