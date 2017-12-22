SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MigrationScript](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[ScriptRun] [varchar](max) NOT NULL,
 CONSTRAINT [PK_MigrationScript] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '001.2012.06.19')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '002.2012.06.20')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '003.2012.06.20')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '004.2012.06.25')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '005.2012.07.02')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '006.2012.07.04')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '007.2012.07.10')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '008.2012.07.11')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '009.2012.07.13')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '010.2012.07.19')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '011.2012.07.20')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '012.2012.07.20')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '013.2012.07.24')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '014.2012.07.25')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '015.2012.08.02')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '016.2012.08.08')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '017.2012.08.08')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '018.2012.08.19')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '019.2012.08.20')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '020.2012.08.30')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '021.2012.09.06')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '022.2012.09.06')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '023.2012.09.14')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '024.2012.09.14')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '025.2012.09.19')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '026.2012.09.25')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '027.2012.09.25')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '028.2012.10.14')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '029.2012.10.05')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '030.2012.10.09')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '031.2012.10.17')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '032.2012.10.17')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '033.2012.10.22')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '034.2012.10.25')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '035.2012.10.31')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '036.2012.11.05')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '037.2012.11.06')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '038.2012.11.08')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '039.2012.11.08')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '040.2012.11.14')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '041.2012.11.17')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '042.2012.11.21')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '043.2012.11.26')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '044.2012.11.27')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '045.2012.11.28')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '046.2012.11.29')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '047.2012.11.29')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '048.2012.12.04')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '049.2012.12.05')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '050.2012.12.05')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '051.2012.12.18')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '052.2012.12.19')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '053.2012.12.27')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '054.2013.01.02')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '055.2013.01.02')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '056.2013.01.10')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '057.2013.01.22')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '058.2013.01.24')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '059.2013.02.15')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '060.2013.02.21')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '061.2013.03.13')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '062.2013.03.21')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '063.2013.03.24')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '064.2013.04.16')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '065.2013.05.03')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '066.2013.05.06')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '067.2013.05.28')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '068.2013.06.09')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '068.2013.06.14')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '069.2013.06.19')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '070.2013.06.26')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '071.2013.07.13')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '072.2013.07.24')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '073.2013.08.02')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '074.2013.08.08')
INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '075.2013.25.08')
GO
