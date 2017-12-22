if not exists(select * from sys.columns where Name = N'Kind' and Object_ID = Object_ID(N'Documents'))    
BEGIN
	ALTER TABLE Documents ADD Kind nvarchar(50);
	exec sp_sqlexec 'UPDATE Documents SET Kind = ''42479CA9-009D-4483-904C-89E9E7CF436E'''
END	
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_Documents]'))
DROP VIEW [dbo].[DNS3_Documents]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DNS3_Documents]
AS
	SELECT     DocId AS Id, Name, [FileName], Mimetype, DocumentContent AS [Content], DateAdded, IsViewable, 
			   NumberOfSegments, SegmentSize, LastSegmentFill, Kind
	FROM       Documents

GO