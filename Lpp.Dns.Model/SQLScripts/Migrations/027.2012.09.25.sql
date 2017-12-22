if not exists( select * from sys.tables where object_id = object_id( 'RequestSharedFolders' ) )
	create table RequestSharedFolders
	(
		Id int identity primary key,
		Name nvarchar(max),
		[SID] uniqueidentifier not null default [dbo].[NewSqlGuid]()
	)
GO

if not exists( select * from sys.tables where object_id = object_id( 'RequestSharedFolders_Request' ) )
	create table RequestSharedFolders_Request
	(
		FolderId int not null references RequestSharedFolders on delete cascade,
		RequestId int not null references Queries on delete cascade
	)

	alter table RequestSharedFolders_Request add primary key(FolderId, RequestId)
GO