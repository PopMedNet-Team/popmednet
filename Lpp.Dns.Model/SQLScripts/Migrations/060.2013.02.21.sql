
/****** Object:  View [dbo].[Requests]    Script Date: 02/21/2013 12:33:14 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Requests]'))
DROP VIEW [dbo].[Requests]
GO

ALTER TABLE [dbo].[Queries]
ALTER COLUMN [Name] [nvarchar](255) NULL
GO

/****** Object:  View [dbo].[Requests]    Script Date: 02/21/2013 12:33:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


    CREATE VIEW [dbo].[Requests]
	AS
	SELECT     QueryId AS Id, CreatedByUserId, CreatedAt AS Created, RequestTypeId, QueryText AS QueryText_deprecated, Name, 
						  QueryDescription AS Description, RequestorEmail AS RequestorEmail_deprecated, IsDeleted, IsAdminQuery AS IsAdminQuery_deprecated, Priority, 
						  ActivityOfQuery AS ActivityOfQuery_deprecated, ActivityPriority AS ActivityPriority_deprecated, ActivityDescription, ActivityDueDate AS DueDate, 
						  IRBApprovalNo AS ApprovalCode, Submitted, updated, updatedbyuserid, ActivityId, IsTemplate, IsScheduled, [SID], OrganizationId,
						  PurposeOfUse, PhiDisclosureLevel, Schedule, ScheduleCount, ProjectId
	FROM         dbo.Queries		


GO


