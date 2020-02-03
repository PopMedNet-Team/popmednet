if not exists( select * from sys.tables where object_id = object_id( 'ProjectSecurityGroups' ) )
	create table ProjectSecurityGroups
	(
		[SID] uniqueidentifier primary key not null, 
		DisplayName nvarchar(max),
		ProjectId uniqueidentifier not null references Projects,
		Kind int not null default 0
	)
GO