namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Queries", "RequesterCenterID", c => c.Guid());
            AddColumn("dbo.Queries", "WorkplanTypeID", c => c.Guid());
            DropColumn("dbo.Queries", "RequesterCenter");
            DropColumn("dbo.Queries", "WorkplanType");

            Sql(@"DROP View [dbo].[Requests]");
            Sql(@"    CREATE VIEW [dbo].[Requests]
	AS
	SELECT     QueryId AS Id, CreatedByUserId, CreatedAt AS Created, RequestTypeId, QueryText AS QueryText_deprecated, Name, 
						  QueryDescription AS Description, RequestorEmail AS RequestorEmail_deprecated, IsDeleted, IsAdminQuery AS IsAdminQuery_deprecated, Priority, 
						  ActivityOfQuery AS ActivityOfQuery_deprecated, ActivityPriority AS ActivityPriority_deprecated, ActivityDescription, ActivityDueDate AS DueDate, 
						  IRBApprovalNo AS ApprovalCode, Submitted, updated, updatedbyuserid, ActivityId, IsTemplate, IsScheduled, [SID], OrganizationId,
						  PurposeOfUse, PhiDisclosureLevel, Schedule, ScheduleCount, ProjectId, RequesterCenterID, WorkplanTypeID
	FROM         dbo.Queries");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Queries", "WorkplanType", c => c.Int());
            AddColumn("dbo.Queries", "RequesterCenter", c => c.Int());
            DropColumn("dbo.Queries", "WorkplanTypeID");
            DropColumn("dbo.Queries", "RequesterCenterID");

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
    }
}
