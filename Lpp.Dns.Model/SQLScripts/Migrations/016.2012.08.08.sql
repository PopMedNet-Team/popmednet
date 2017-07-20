IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FileDistributionDocumentSegments_FileDistributionDocuments]') AND parent_object_id = OBJECT_ID(N'[dbo].[FileDistributionDocumentSegments]'))
ALTER TABLE [dbo].[FileDistributionDocumentSegments] DROP CONSTRAINT [FK_FileDistributionDocumentSegments_FileDistributionDocuments]
GO

/****** Object:  Table [dbo].[FileDistributionDocuments]    Script Date: 08/08/2012 22:38:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FileDistributionDocuments]') AND type in (N'U'))
DROP TABLE [dbo].[FileDistributionDocuments]
GO

/****** Object:  Table [dbo].[FileDistributionDocuments]    Script Date: 08/08/2012 22:38:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FileDistributionDocuments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[MimeType] [varchar](50) NOT NULL,
	[Created] [datetime] NOT NULL,
	[RequestId] [int] NULL,
 CONSTRAINT [PK_FileDistributionDocuments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FileDistributionDocumentSegments_FileDistributionDocuments]') AND parent_object_id = OBJECT_ID(N'[dbo].[FileDistributionDocumentSegments]'))
ALTER TABLE [dbo].[FileDistributionDocumentSegments] DROP CONSTRAINT [FK_FileDistributionDocumentSegments_FileDistributionDocuments]
GO

/****** Object:  Table [dbo].[FileDistributionDocumentSegments]    Script Date: 08/08/2012 22:38:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FileDistributionDocumentSegments]') AND type in (N'U'))
DROP TABLE [dbo].[FileDistributionDocumentSegments]
GO

/****** Object:  Table [dbo].[FileDistributionDocumentSegments]    Script Date: 08/08/2012 22:38:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FileDistributionDocumentSegments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Content] [varbinary](max) NULL,
	[Document_Id] [int] NOT NULL,
 CONSTRAINT [PK_FileDistributionDocumentSegments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[FileDistributionDocumentSegments]  WITH CHECK ADD  CONSTRAINT [FK_FileDistributionDocumentSegments_FileDistributionDocuments] FOREIGN KEY([Document_Id])
REFERENCES [dbo].[FileDistributionDocuments] ([Id])
GO

ALTER TABLE [dbo].[FileDistributionDocumentSegments] CHECK CONSTRAINT [FK_FileDistributionDocumentSegments_FileDistributionDocuments]
GO


