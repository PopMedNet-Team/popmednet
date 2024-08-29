IF OBJECT_ID(N'[dbo].[FK_ModelRequestType]', 'F') IS NOT NULL
	ALTER TABLE [dbo].[RequestTypes] DROP CONSTRAINT [FK_ModelRequestType];
GO

IF OBJECT_ID(N'[dbo].[Models]', 'U') IS NOT NULL
	DROP TABLE [dbo].[Models];
GO

IF OBJECT_ID(N'[dbo].[RequestTypes]', 'U') IS NOT NULL
	DROP TABLE [dbo].[RequestTypes];
GO

IF OBJECT_ID(N'[dbo].[PluginSessions]', 'U') IS NOT NULL
	DROP TABLE [dbo].[PluginSessions];
GO

-- Creating table 'Models'
CREATE TABLE [dbo].[Models] (
	[Id] uniqueidentifier  NOT NULL,
	[ModelProcessorId] uniqueidentifier  NOT NULL,
	[Name] nvarchar(100)  NOT NULL,
	[Description] nvarchar(max)  NULL,
	[Version] nvarchar(max)  NOT NULL,
	[RSA_Modulus] varbinary(max)  NULL,
	[RSA_Exponent] varbinary(max)  NULL,
	[Created] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Models'
ALTER TABLE [dbo].[Models]
ADD CONSTRAINT [PK_Models]
	PRIMARY KEY CLUSTERED ([Id] ASC);
GO

create table RequestTypes
(
	[Id] int identity(1,1) primary key,
	[ModelId] uniqueidentifier not null references Models on delete cascade,
	[LocalId] uniqueidentifier not null,
	[Name] nvarchar(max),
	[Description] nvarchar(max),
	[IsMetadataRequest] bit not null default 0,
	[CreateRequestUrl] nvarchar(max),
	[RetrieveResponseUrl] nvarchar(max),
)
GO

create table PluginSessions
(
	Id nvarchar(100) primary key not null,
	UserId int not null,
	RequestId int not null,
	Expires datetime not null default getdate(),
	ReturnUrl nvarchar(max) null,
	IsCommitted bit not null default 0,
	IsAborted bit not null default 0,
	ResponseToken nvarchar(max)
)
GO

create table PluginSessionDocuments
(
	Id int identity(1,1) primary key,
	SessionId nvarchar(100) not null references PluginSessions on delete cascade on update cascade,
	Name nvarchar(max),
	Body varbinary(max),
	MimeType nvarchar(max),
	IsViewable bit not null default 0
)
GO