if not exists( select * from sys.columns where name='ProjectId' and object_id = object_id('queries') )
begin

    alter table queries add ProjectId uniqueidentifier

    ALTER TABLE [dbo].[queries]  WITH CHECK ADD FOREIGN KEY([ProjectId])
    REFERENCES [dbo].[Projects] ([SID])
    ON DELETE CASCADE

end
go

if not exists( select projectid from [dbo].[queries] where projectid is not null )
begin
    declare @projectid uniqueidentifier
    set @projectid = (select sid from projects where name='Default')

    update queries set projectid = @projectid	

end

if exists( select * from sys.views where object_id = object_id('requests'))
    drop view requests
go

    CREATE VIEW Requests
	AS
	SELECT     QueryId AS Id, CreatedByUserId, CreatedAt AS Created, RequestTypeId, QueryText AS QueryText_deprecated, Name, 
						  QueryDescription AS Description, RequestorEmail AS RequestorEmail_deprecated, IsDeleted, IsAdminQuery AS IsAdminQuery_deprecated, Priority, 
						  ActivityOfQuery AS ActivityOfQuery_deprecated, ActivityPriority AS ActivityPriority_deprecated, ActivityDescription, ActivityDueDate AS DueDate, 
						  IRBApprovalNo AS ApprovalCode, Submitted, updated, updatedbyuserid, ActivityId, IsTemplate, IsScheduled, [SID], OrganizationId,
						  PurposeOfUse, PhiDisclosureLevel, Schedule, ScheduleCount, ProjectId
	FROM         dbo.Queries		

go
