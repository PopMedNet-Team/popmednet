sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO
sp_configure 'clr enabled', 1;
GO
RECONFIGURE;
GO

create table Activities ( Id int identity primary key, Name nvarchar(max) )
GO

alter table Queries add ActivityId int null references Activities
alter table Queries add IsScheduled bit not null default 0
alter table queries alter column QueryTypeId int null
alter table queries add RequestTypeId uniqueidentifier not null
alter table queries add Submitted datetime null
alter table queries add UpdatedByUserId int not null references users
alter table queries add Updated datetime not null default getdate()
alter table queries add IsTemplate bit not null default 0
alter table queries add [SID] uniqueidentifier unique not null default [dbo].[newsqlguid]()
alter table queries add OrganizationId int references Organizations not null
alter table documents add IsViewable bit not null default 0
alter table documents add Size as datalength(DocumentContent)
alter table DataMartAvailabilityPeriods drop constraint FK_DataMartAvailabilityPeriods_Queries_QueryId
alter table DataMarts add [SID] uniqueidentifier unique not null default [dbo].[newsqlguid]()
alter table users add MiddleName nvarchar(100)
alter table users add Phone nvarchar(50)
alter table users add Fax nvarchar(50)
alter table users add [SID] uniqueidentifier unique not null default [dbo].[newsqlguid]()
alter table users add PasswordExpiration datetime not null default dateadd(month,6,getdate())
alter table users add PasswordRestorationToken uniqueidentifier not null default newid()
alter table users add PasswordRestorationTokenExpiration datetime not null default getdate()
alter table Organizations add [SID] uniqueidentifier unique not null default [dbo].[newsqlguid]()
alter table Organizations add [ParentId] int null references Organizations
alter table Groups add [SID] uniqueidentifier unique not null default [dbo].[newsqlguid]()
go

create index Queries_Updated on Queries([Updated])
create index Queries_Type on Queries([RequestTypeId])
create index Queries_Submitted on Queries(Submitted)
create index QueriesDataMarts_Status on QueriesDataMarts(QueryStatusTypeId)

create index DataMart_Org on DataMarts(OrganizationId)
create index User_Org on Users(OrganizationId)
create index Request_Org on Queries(OrganizationId)
create index RequestDataMart_Request on QueriesDataMarts(QueryId)
create index RequestDataMart_DataMart on QueriesDataMarts(DataMartId)
create index Response_Request on Responses(QueryId)

create index DocRequest_Request on QueriesDocuments(QueryId)
create index DocRequest_Doc on QueriesDocuments(DocId)
create index DocResponse_Response on ResponsesDocuments(ResponseId)
create index DocResponse_Doc on ResponsesDocuments(DocId)
go

drop table queriesdocuments
go

create table queriesdocuments 
(
    docid int not null references documents on update cascade on delete cascade,
    queryid int not null references queries on update cascade on delete cascade
)
go

drop table responsesdocuments
go

create table responsesdocuments 
(
    docid int not null references documents on update cascade on delete cascade,
    responseid int not null references responses on update cascade on delete cascade
)
go

alter table QueriesDataMarts add PropertiesXml nvarchar(max)
go

create table SecurityGroups 
( 
	[SID] uniqueidentifier primary key not null, 
	DisplayName nvarchar(max),
	OrganizationId int not null references Organizations,
	Kind int not null default 0
)
go

create table SecurityGroupsSecurityGroups 
( 
	[ParentId] uniqueidentifier not null references SecurityGroups on delete cascade, 
	[ChildId] uniqueidentifier not null references SecurityGroups
)
go

create table SecurityGroupsUsers
( 
	[GroupId] uniqueidentifier not null references SecurityGroups on delete cascade, 
	[UserId] int not null references Users on delete cascade
)
go

create table ResponseGroups
(
	Id int identity primary key,
	Name nvarchar(max)
)
go

create table Subscriptions
(
	Id int identity primary key,
	OwnerId int not null references [Users],
	LastRunTime datetime null,
	NextDueTime datetime null,
	FiltersDefinitionXml nvarchar(max),
	Schedule int not null default 0
)
go

alter table responses add GroupId int null references ResponseGroups on delete set null on update cascade
go

create table DataMartInstalledModels
(
	DataMartId int not null references DataMarts on delete cascade,
	ModelId uniqueidentifier not null
	constraint dmim_ID primary key (DataMartId, ModelId)
)
go

---------------------------------------------------------------------------
-- DNS 3 Views used to map existing DNS 2.x tables to DNS4 partial schema
---------------------------------------------------------------------------
/****** Object:  View [dbo].[DataMartRequests]    Script Date: 01/09/2012 16:55:05 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DataMartRequests]'))
DROP VIEW [dbo].[DataMartRequests]
GO

/****** Object:  View [dbo].[DataMartRequests]    Script Date: 01/09/2012 16:55:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DataMartRequests]
AS
SELECT     QueryId AS RequestId, DataMartId, QueryStatusTypeId AS RequestStatusTypeId, RequestTime, ResponseTime, ErrorMessage, ErrorDetail, RejectReason, 
                      IsResultsGrouped, RespondedBy, PropertiesXml
FROM         dbo.QueriesDataMarts

GO

/****** Object:  View [dbo].[DNS3_DataMarts]    Script Date: 01/09/2012 16:56:24 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_DataMarts]'))
DROP VIEW [dbo].[DNS3_DataMarts]
GO
/****** Object:  View [dbo].[DNS3_DataMarts]    Script Date: 01/09/2012 16:56:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DNS3_DataMarts]
AS
SELECT     DataMartId AS Id, DataMartName AS Name, Url, RequiresApproval, DataMartTypeId, AvailablePeriod, ContactEmail, ContactFirstName, ContactLastName, 
                      ContactPhone, SpecialRequirements, UsageRestrictions, isDeleted, HealthPlanDescription, OrganizationId, IsGroupDataMart,
					  [SID]
FROM         dbo.DataMarts

GO

/****** Object:  View [dbo].[DNS3_DataMartTypes]    Script Date: 01/09/2012 16:57:02 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_DataMartTypes]'))
DROP VIEW [dbo].[DNS3_DataMartTypes]
GO

/****** Object:  View [dbo].[DNS3_DataMartTypes]    Script Date: 01/09/2012 16:57:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DNS3_DataMartTypes]
AS
SELECT     DataMartTypeId AS Id, DatamartType AS Name
FROM         dbo.DataMartTypes

GO

/****** Object:  View [dbo].[DNS3_Documents]    Script Date: 01/09/2012 16:57:21 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_Documents]'))
DROP VIEW [dbo].[DNS3_Documents]
GO

/****** Object:  View [dbo].[DNS3_Documents]    Script Date: 01/09/2012 16:57:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DNS3_Documents]
AS
SELECT     DocId AS Id, Name, FileName, Mimetype, DocumentContent AS [Content], DateAdded, IsViewable, Size
FROM         dbo.Documents

GO

/****** Object:  View [dbo].[DNS3_Groups]    Script Date: 01/09/2012 16:57:52 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_Groups]'))
DROP VIEW [dbo].[DNS3_Groups]
GO

/****** Object:  View [dbo].[DNS3_Groups]    Script Date: 01/09/2012 16:57:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DNS3_Groups]
AS
SELECT     GroupId AS Id, GroupName AS Name, IsDeleted, IsApprovalRequired, [SID]
FROM         dbo.Groups

GO

/****** Object:  View [dbo].[DNS3_Organizations]    Script Date: 01/09/2012 16:58:12 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_Organizations]'))
DROP VIEW [dbo].[DNS3_Organizations]
GO

/****** Object:  View [dbo].[DNS3_Organizations]    Script Date: 01/09/2012 16:58:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DNS3_Organizations]
AS
SELECT     OrganizationId AS Id, OrganizationName AS Name, IsDeleted, IsApprovalRequired, OrganizationAcronym AS Acronym, ParentId, [SID]
FROM         dbo.Organizations

GO

/****** Object:  View [dbo].[DNS3_Responses]    Script Date: 01/09/2012 16:58:39 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_Responses]'))
DROP VIEW [dbo].[DNS3_Responses]
GO

/****** Object:  View [dbo].[DNS3_Responses]    Script Date: 01/09/2012 16:58:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DNS3_Responses]
AS
SELECT     ResponseId AS Id, QueryId AS RequestId, DataMartId, ResponseXml AS ResponseXml_deprecated, UserId, IsDeleted, GroupId
FROM         dbo.Responses

GO

/****** Object:  View [dbo].[RequestResponseViewedStatus]    Script Date: 01/09/2012 16:59:34 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[RequestResponseViewedStatus]'))
DROP VIEW [dbo].[RequestResponseViewedStatus]
GO

/****** Object:  View [dbo].[RequestResponseViewedStatus]    Script Date: 01/09/2012 16:59:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[RequestResponseViewedStatus]
AS
SELECT     QueryId AS RequestId, ResponseId, ViewedAt
FROM         dbo.QueryResultsViewedStatus

GO

/****** Object:  View [dbo].[Requests]    Script Date: 01/09/2012 16:59:50 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Requests]'))
DROP VIEW [dbo].[Requests]
GO

/****** Object:  View [dbo].[Requests]    Script Date: 01/09/2012 16:59:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Requests]
AS
SELECT     QueryId AS Id, CreatedByUserId, CreatedAt AS Created, RequestTypeId, QueryText AS QueryText_deprecated, Name, 
                      QueryDescription AS Description, RequestorEmail AS RequestorEmail_deprecated, IsDeleted, IsAdminQuery AS IsAdminQuery_deprecated, Priority, 
                      ActivityOfQuery AS ActivityOfQuery_deprecated, ActivityPriority AS ActivityPriority_deprecated, ActivityDescription, ActivityDueDate AS DueDate, 
                      IRBApprovalNo AS ApprovalCode, Submitted, updated, updatedbyuserid, ActivityId, IsTemplate, IsScheduled, [SID], OrganizationId
FROM         dbo.Queries

GO

/****** Object:  View [dbo].[RequestStatusTypes]    Script Date: 01/09/2012 17:00:12 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[RequestStatusTypes]'))
DROP VIEW [dbo].[RequestStatusTypes]
GO

/****** Object:  View [dbo].[RequestStatusTypes]    Script Date: 01/09/2012 17:00:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[RequestStatusTypes]
AS
SELECT     QueryStatusTypeId AS Id, QueryStatusType AS Name
FROM         dbo.QueryStatusTypes

GO