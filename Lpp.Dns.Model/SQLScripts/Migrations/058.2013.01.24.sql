/****** Object:  Table [dbo].[FileDocuments]    Script Date: 01/24/2013 14:08:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FileDocuments]') AND type in (N'U'))
DROP TABLE [dbo].[FileDocuments]
GO

/****** Object:  Table [dbo].[FileDocuments]    Script Date: 01/24/2013 14:08:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FileDocuments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[MimeType] [varchar](500) NOT NULL,
	[Created] [datetime] NOT NULL,
	[RequestId] [int] NULL,
 CONSTRAINT [PK_FileDocuments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FileDocumentSegments_FileDocuments]') AND parent_object_id = OBJECT_ID(N'[dbo].[FileDocumentSegments]'))
ALTER TABLE [dbo].[FileDocumentSegments] DROP CONSTRAINT [FK_FileDocumentSegments_FileDocuments]
GO

/****** Object:  Table [dbo].[FileDocumentSegments]    Script Date: 01/24/2013 14:08:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FileDocumentSegments]') AND type in (N'U'))
DROP TABLE [dbo].[FileDocumentSegments]
GO

/****** Object:  Table [dbo].[FileDocumentSegments]    Script Date: 01/24/2013 14:08:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FileDocumentSegments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Content] [varbinary](max) NULL,
	[Document_Id] [int] NOT NULL,
 CONSTRAINT [PK_FileDocumentSegments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[FileDocumentSegments]  WITH CHECK ADD  CONSTRAINT [FK_FileDocumentSegments_FileDocuments] FOREIGN KEY([Document_Id])
REFERENCES [dbo].[FileDocuments] ([Id])
GO

ALTER TABLE [dbo].[FileDocumentSegments] CHECK CONSTRAINT [FK_FileDocumentSegments_FileDocuments]
GO


