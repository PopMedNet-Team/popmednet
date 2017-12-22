INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '080.2013.10.02')
GO
/****** Object:  Table [dbo].[QuerySearchTerms]    Script Date: 10/1/2013 9:23:33 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RequestSearchTerms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[StringValue] [nvarchar](255) NULL,
	[NumberValue] [decimal](18, 0) NULL,
	[DateFrom] [datetime] NULL,
	[DateTo] [datetime] NULL,
	[NumberFrom] [decimal](18, 0) NULL,
	[NumberTo] [decimal](18, 0) NULL,
 CONSTRAINT [PK_QuerySearchTerms_SearchTagId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Index [IX_QuerySearchTerms]    Script Date: 10/1/2013 9:23:47 AM ******/
CREATE NONCLUSTERED INDEX [IX_QuerySearchTerms] ON [dbo].[RequestSearchTerms]
(
	[RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_QuerySearchTerms_DateRange]    Script Date: 10/1/2013 9:27:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_QuerySearchTerms_DateRange] ON [dbo].[RequestSearchTerms]
(
	[DateFrom] ASC,
	[DateTo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_QuerySearchTerms_NumberRange] ON [dbo].[RequestSearchTerms]
(
	[NumberFrom] ASC,
	[NumberTo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_QuerySearchTerms_NumberValue]    Script Date: 10/1/2013 9:27:46 AM ******/
CREATE NONCLUSTERED INDEX [IX_QuerySearchTerms_NumberValue] ON [dbo].[RequestSearchTerms]
(
	[NumberValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_QuerySearchTerms_StringValue] ON [dbo].[RequestSearchTerms]
(
	[StringValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_QuerySearchTerms_Type]    Script Date: 10/1/2013 9:28:10 AM ******/
CREATE NONCLUSTERED INDEX [IX_QuerySearchTerms_Type] ON [dbo].[RequestSearchTerms]
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO