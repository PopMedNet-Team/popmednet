namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventIDToLogs2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.LogsProjectChange", "EventID");
            CreateIndex("dbo.LogsDataMartChange", "EventID");
            CreateIndex("dbo.LogsGroupChange", "EventID");
            CreateIndex("dbo.LogsOrganizationChange", "EventID");
            CreateIndex("dbo.LogsRegistryChange", "EventID");
            CreateIndex("dbo.LogsResponseViewed", "EventID");
            CreateIndex("dbo.LogsUploadedResultNeedsApproval", "EventID");
            CreateIndex("dbo.LogsRequestStatusChange", "EventID");
            CreateIndex("dbo.LogsResultsReminder", "EventID");
            CreateIndex("dbo.LogsSubmittedRequestAwaitsResponse", "EventID");
            CreateIndex("dbo.LogsSubmittedRequestNeedsApproval", "EventID");
            CreateIndex("dbo.LogsPasswordExpiration", "EventID");
            CreateIndex("dbo.LogsProfileUpdated", "EventID");
            CreateIndex("dbo.LogsUserRegistrationChanged", "EventID");
            CreateIndex("dbo.LogsUserRegistrationSubmitted", "EventID");
            CreateIndex("dbo.LogsUserChange", "EventID");
            CreateIndex("dbo.LogsNewDataMartClient", "EventID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LogsNewDataMartClient", new[] { "EventID" });
            DropIndex("dbo.LogsUserChange", new[] { "EventID" });
            DropIndex("dbo.LogsUserRegistrationSubmitted", new[] { "EventID" });
            DropIndex("dbo.LogsUserRegistrationChanged", new[] { "EventID" });
            DropIndex("dbo.LogsProfileUpdated", new[] { "EventID" });
            DropIndex("dbo.LogsPasswordExpiration", new[] { "EventID" });
            DropIndex("dbo.LogsSubmittedRequestNeedsApproval", new[] { "EventID" });
            DropIndex("dbo.LogsSubmittedRequestAwaitsResponse", new[] { "EventID" });
            DropIndex("dbo.LogsResultsReminder", new[] { "EventID" });
            DropIndex("dbo.LogsRequestStatusChange", new[] { "EventID" });
            DropIndex("dbo.LogsUploadedResultNeedsApproval", new[] { "EventID" });
            DropIndex("dbo.LogsResponseViewed", new[] { "EventID" });
            DropIndex("dbo.LogsRegistryChange", new[] { "EventID" });
            DropIndex("dbo.LogsOrganizationChange", new[] { "EventID" });
            DropIndex("dbo.LogsGroupChange", new[] { "EventID" });
            DropIndex("dbo.LogsDataMartChange", new[] { "EventID" });
            DropIndex("dbo.LogsProjectChange", new[] { "EventID" });
        }
    }
}
