--------------------------------------------------------------------------------------------------------------
--PMN-842: Make a Master/Detail for Activity list to display a sub-list called Tasks 
--------------------------------------------------------------------------------------------------------------
IF Not exists (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Activities' and COLUMN_NAME = 'ParentId')
	ALTER TABLE Activities Add ParentId int NULL
	
GO	

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Activities_Activities]') AND parent_object_id = OBJECT_ID(N'[dbo].[Activities]'))
ALTER TABLE [dbo].[Activities] DROP CONSTRAINT [FK_Activities_Activities]
GO

ALTER TABLE [dbo].[Activities]  WITH CHECK ADD  CONSTRAINT [FK_Activities_Activities] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Activities] ([Id])
GO

ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [FK_Activities_Activities]

GO
--------------------------------------------------------------------------------------------------------------
