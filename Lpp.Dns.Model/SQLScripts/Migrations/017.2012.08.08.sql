if not exists( select * from sys.tables where object_id = object_id( 'DocumentSegments' ) )
begin

	alter table Documents drop column Size
	alter table Documents alter column DocumentContent varbinary(max) null
	alter table Documents add SegmentSize int not null default 0
	alter table Documents add NumberOfSegments int not null default 1
	alter table Documents add LastSegmentFill int not null default 0

	create table DocumentSegments
	(
		DocumentId int not null references Documents on delete cascade,
		[Index] int not null,
		[Data] varbinary(max)
	)

	insert into DocumentSegments( DocumentId, [Index], [Data] ) select DocId, 0, [DocumentContent] from Documents
	exec sp_sqlexec 'update Documents set SegmentSize = datalength(DocumentContent), LastSegmentFill = datalength(DocumentContent)'

end
go

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_Documents]'))
	DROP VIEW [dbo].[DNS3_Documents]
GO

CREATE VIEW [dbo].[DNS3_Documents]
AS
	SELECT     DocId AS Id, Name, [FileName], Mimetype, DocumentContent AS [Content], DateAdded, IsViewable, 
			   NumberOfSegments, SegmentSize, LastSegmentFill
	FROM       Documents
go