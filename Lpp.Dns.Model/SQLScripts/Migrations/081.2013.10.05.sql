INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '081.2013.10.05')
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RequestSearchResults_Queries]') AND parent_object_id = OBJECT_ID(N'[dbo].[RequestSearchResults]'))
ALTER TABLE [dbo].[RequestSearchResults] DROP CONSTRAINT [FK_RequestSearchResults_Queries]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RequestSearchResults_Queries1]') AND parent_object_id = OBJECT_ID(N'[dbo].[RequestSearchResults]'))
ALTER TABLE [dbo].[RequestSearchResults] DROP CONSTRAINT [FK_RequestSearchResults_Queries1]
GO

/****** Object:  Table [dbo].[RequestSearchResults]    Script Date: 10/05/2013 12:50:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RequestSearchResults]') AND type in (N'U'))
DROP TABLE [dbo].[RequestSearchResults]
GO

/****** Object:  Table [dbo].[RequestSearchResults]    Script Date: 10/05/2013 12:50:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RequestSearchResults](
	[SearchRequestId] [int] NOT NULL,
	[ResultRequestId] [int] NOT NULL,
 CONSTRAINT [PK_RequestSearchResults_1] PRIMARY KEY CLUSTERED 
(
	[SearchRequestId] ASC,
	[ResultRequestId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RequestSearchResults]  WITH CHECK ADD  CONSTRAINT [FK_RequestSearchResults_Queries] FOREIGN KEY([SearchRequestId])
REFERENCES [dbo].[Queries] ([QueryId])
GO

ALTER TABLE [dbo].[RequestSearchResults] CHECK CONSTRAINT [FK_RequestSearchResults_Queries]
GO

ALTER TABLE [dbo].[RequestSearchResults]  WITH CHECK ADD  CONSTRAINT [FK_RequestSearchResults_Queries1] FOREIGN KEY([ResultRequestId])
REFERENCES [dbo].[Queries] ([QueryId])
GO

ALTER TABLE [dbo].[RequestSearchResults] CHECK CONSTRAINT [FK_RequestSearchResults_Queries1]
GO


