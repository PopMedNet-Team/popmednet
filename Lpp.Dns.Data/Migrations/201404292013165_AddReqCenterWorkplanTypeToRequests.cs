namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReqCenterWorkplanTypeToRequests : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Queries", "RequesterCenter", c => c.Int());
            AddColumn("dbo.Queries", "WorkplanType", c => c.Int());

            Sql(@"DROP View [dbo].[Requests]");
            Sql(@"    CREATE VIEW [dbo].[Requests]
	AS
	SELECT     QueryId AS Id, CreatedByUserId, CreatedAt AS Created, RequestTypeId, QueryText AS QueryText_deprecated, Name, 
						  QueryDescription AS Description, RequestorEmail AS RequestorEmail_deprecated, IsDeleted, IsAdminQuery AS IsAdminQuery_deprecated, Priority, 
						  ActivityOfQuery AS ActivityOfQuery_deprecated, ActivityPriority AS ActivityPriority_deprecated, ActivityDescription, ActivityDueDate AS DueDate, 
						  IRBApprovalNo AS ApprovalCode, Submitted, updated, updatedbyuserid, ActivityId, IsTemplate, IsScheduled, [SID], OrganizationId,
						  PurposeOfUse, PhiDisclosureLevel, Schedule, ScheduleCount, ProjectId, RequesterCenter, WorkplanType
	FROM         dbo.Queries");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Queries", "WorkplanType");
            DropColumn("dbo.Queries", "RequesterCenter");

            Sql(@"DROP View [dbo].[Requests]");
            Sql(@"    CREATE VIEW [dbo].[Requests]
	AS
	SELECT     QueryId AS Id, CreatedByUserId, CreatedAt AS Created, RequestTypeId, QueryText AS QueryText_deprecated, Name, 
						  QueryDescription AS Description, RequestorEmail AS RequestorEmail_deprecated, IsDeleted, IsAdminQuery AS IsAdminQuery_deprecated, Priority, 
						  ActivityOfQuery AS ActivityOfQuery_deprecated, ActivityPriority AS ActivityPriority_deprecated, ActivityDescription, ActivityDueDate AS DueDate, 
						  IRBApprovalNo AS ApprovalCode, Submitted, updated, updatedbyuserid, ActivityId, IsTemplate, IsScheduled, [SID], OrganizationId,
						  PurposeOfUse, PhiDisclosureLevel, Schedule, ScheduleCount, ProjectId
	FROM         dbo.Queries");
        }
    }
}
