if not exists( select * from sys.columns where name='scheduleCount' and object_id = object_id('queries') )
	alter table queries add ScheduleCount int not null default 0
go

if exists(select * from sys.views where object_id = object_id(N'[dbo].[requests]'))
	drop view [dbo].[requests]
go

CREATE VIEW [dbo].[Requests]
AS
SELECT     QueryId AS Id, CreatedByUserId, CreatedAt AS Created, RequestTypeId, QueryText AS QueryText_deprecated, Name, 
                      QueryDescription AS Description, RequestorEmail AS RequestorEmail_deprecated, IsDeleted, IsAdminQuery AS IsAdminQuery_deprecated, Priority, 
                      ActivityOfQuery AS ActivityOfQuery_deprecated, ActivityPriority AS ActivityPriority_deprecated, ActivityDescription, ActivityDueDate AS DueDate, 
                      IRBApprovalNo AS ApprovalCode, Submitted, updated, updatedbyuserid, ActivityId, IsTemplate, IsScheduled, [SID], OrganizationId,
					  PurposeOfUse, PhiDisclosureLevel, Schedule, ScheduleCount
FROM         dbo.Queries


GO
