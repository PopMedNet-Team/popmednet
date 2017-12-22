IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Queries_Activities]') AND parent_object_id = OBJECT_ID(N'[dbo].[Queries]'))
ALTER TABLE [dbo].[Queries] DROP CONSTRAINT [FK_Queries_Activities]
GO

DECLARE @FK_QueriesActivitiesConstraint varchar(max);
SET @FK_QueriesActivitiesConstraint = (SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
WHERE CONSTRAINT_NAME LIKE 'FK[_][_]Queries[_][_]Activit%');

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(@FK_QueriesActivitiesConstraint) AND parent_object_id = OBJECT_ID(N'[dbo].[Queries]'))
EXEC('ALTER TABLE [dbo].[Queries] DROP CONSTRAINT ' + @FK_QueriesActivitiesConstraint);
GO

DECLARE @FK_ActivitiesProjConstraint varchar(max);
SET @FK_ActivitiesProjConstraint = (SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
WHERE CONSTRAINT_NAME LIKE 'FK[_][_]Activitie[_][_]Proje%');

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(@FK_ActivitiesProjConstraint) AND parent_object_id = OBJECT_ID(N'[dbo].[Activities]'))
EXEC('ALTER TABLE [dbo].[Activities] DROP CONSTRAINT ' + @FK_ActivitiesProjConstraint);
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Activities_Activities]') AND parent_object_id = OBJECT_ID(N'[dbo].[Activities]'))
ALTER TABLE [dbo].[Activities] DROP CONSTRAINT [FK_Activities_Activities]
GO


/****** Object:  Table [dbo].[Activities]    Script Date: 09/25/2013 10:17:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Activities]') AND type in (N'U'))
DROP TABLE [dbo].[Activities]
GO


/****** Object:  Table [dbo].[Activities]    Script Date: 09/25/2013 10:17:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Activities](
	[Id] [int] ,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[ProjectId] [uniqueidentifier] NULL,
	[ParentId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Activities]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([SID])
GO

ALTER TABLE [dbo].[Activities]  WITH CHECK ADD  CONSTRAINT [FK_Activities_Activities] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Activities] ([Id])
GO

ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [FK_Activities_Activities]
GO

--UPDATE [dbo].[Queries] SET ActivityId=NULL

--ALTER TABLE [dbo].[Queries]  WITH CHECK ADD CONSTRAINT [FK_Queries_Activities] FOREIGN KEY([ActivityId]) 
--REFERENCES [dbo].[Activities] ([Id])
--GO

INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '078.2013.09.25')
GO 