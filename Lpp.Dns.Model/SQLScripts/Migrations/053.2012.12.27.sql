if not exists( select * from sys.tables where object_id = object_id('FileSegments') ) begin
	create table Files
	(
		Id int primary key identity,
		[FileName] nvarchar(max) not null,
		[MimeType] nvarchar(max) not null,
		[Created] datetime2 not null default getdate(),

		[SegmentSize] int not null,
		[NumberOfSegments] int not null,
		[LastSegmentFill] int not null
	)

	create table FileSegments
	(
		FileId int not null references Files,
		[Index] int not null default 0,
		[Data] varbinary(max)
	)
end
go

if not exists( select * from sys.columns where name = 'fileid' and object_id = object_id('Documents') ) begin
	alter table Documents add FileId int references Files
end
go

if exists( select * from sys.tables where object_id = object_id('DocumentSegments') ) begin

	exec sp_sqlexec 'declare cur cursor for select [docid], isnull([name], [filename]), [mimetype], [DateAdded], [segmentsize], [numberofsegments], [lastsegmentfill] from documents'
	open cur

	declare @id int, @name nvarchar(max), @mimetype nvarchar(max), @created datetime2,
		@segmentsize int, @numberofsegments int, @lastsegmentfill int, @fileid int

	fetch next from cur into @id, @name, @mimetype, @created, @segmentsize, @numberofsegments, @lastsegmentfill 
	while @@fetch_status = 0 begin
		insert into files( [filename], [mimetype], [created], [segmentsize], [numberofsegments], [lastsegmentfill] )
		values( isnull(@name,''), isnull(@mimetype,''), isnull(@created,getdate()), @segmentsize, @numberofsegments, @lastsegmentfill )
		set @fileid = scope_identity()

		update documents set fileid = @fileid where docid = @id

		fetch next from cur into @id, @name, @mimetype, @created, @segmentsize, @numberofsegments, @lastsegmentfill 
	end

	close cur
	deallocate cur

	insert into filesegments(fileid, [index], [data])
	select d.fileid, ds.[index], ds.[data] from documentsegments ds
	inner join documents d on d.docid = ds.documentid

	declare @dropctrs table(s nvarchar(max))
	insert into @dropctrs select 'alter table [documents] drop constraint [' + name + ']' 
	from sys.default_constraints where parent_object_id = object_id('documents')

	declare cr cursor for select s from @dropctrs
	open cr
	declare @sql nvarchar(max)
	fetch next from cr into @sql
	while @@fetch_status = 0 begin
		print @sql
		exec sp_sqlexec @sql
		fetch next from cr into @sql
	end
	close cr
	deallocate cr

	exec sp_sqlexec 'drop table documentsegments
	alter table documents drop column [filename]
	alter table documents drop column mimetype
	alter table documents drop column numberofsegments
	alter table documents drop column lastsegmentfill
	alter table documents drop column segmentsize
	alter table documents drop column documentcontent
	alter table documents drop column dateadded'

end

if not exists( select * from sys.tables where object_id = object_id('RequestRoutingInstances') ) begin
	create table RequestRoutingInstances 
	(
		Id int identity not null primary key,
		RequestId int not null references Queries,
		DataMartId int not null references DataMarts,
		ResponseGroupId int null references ResponseGroups,
		[Message] nvarchar(max) null,
		RespondedByUserId int null references [Users],
		[ResponseTime] datetime2 null,
		IsCurrent bit not null default 0
	)

	alter table documents add RequestId int null references Queries
	alter table documents add RoutingInstanceId int null references RequestRoutingInstances
end
go

if exists( select * from sys.tables where object_id = object_id('responsesdocuments') ) begin

	insert into RequestRoutingInstances( RequestId, DataMartId, [Message], RespondedByUserId, ResponseTime, IsCurrent )
	select QueryId, DataMartId, ErrorMessage, RespondedBy, ResponseTime, 1 from QueriesDataMarts

	update d set RoutingInstanceId = ri.Id
	from documents d
	inner join responsesdocuments rd on d.DocId = rd.DocId
	inner join responses r on rd.responseid = r.responseid
	inner join requestroutinginstances ri on r.queryid = ri.requestid and r.datamartid = ri.datamartid

	update d set RequestId = qd.queryid
	from documents d
	inner join queriesdocuments qd on d.docid = qd.docid

	drop table NetworkResponsesGroups
	drop view RequestResponseViewedStatus
	drop table QueryResultsViewedStatus
	drop table responsesdocuments
	drop table queriesdocuments
	drop table responses

end
go

if exists( select * from sys.views where object_id = object_id( 'DNS3_Documents' ) )
	drop view DNS3_Documents