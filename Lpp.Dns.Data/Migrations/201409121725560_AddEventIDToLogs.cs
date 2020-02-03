namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventIDToLogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogsDataMartChange", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("59A90001-539E-4C21-A4F2-A22200CD3C7D")));
            AddColumn("dbo.LogsGroupChange", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("D80E0001-27BC-4FCB-BA75-A22200CD2426")));
            AddColumn("dbo.LogsNewDataMartClient", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("60F20001-77FF-4F4B-9153-A2220129E466")));
            AddColumn("dbo.LogsOrganizationChange", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("B8A50001-B556-43D2-A1B8-A22200CD12DC")));
            AddColumn("dbo.LogsPasswordExpiration", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("C2790001-2FF6-456C-9497-A22200CCCD1F")));
            AddColumn("dbo.LogsProfileUpdated", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("B6B10001-07FB-47F5-83B8-A22200CCDB90")));
            AddColumn("dbo.LogsProjectChange", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("1C090001-9F95-400C-9780-A22200CD0234")));
            AddColumn("dbo.LogsRegistryChange", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("553FD350-8F3B-40C6-9E31-11D8BC7420A2")));
            AddColumn("dbo.LogsRequestStatusChange", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("0A850001-FC8A-4DE2-9AA5-A22200E82398")));
            AddColumn("dbo.LogsResponseViewed", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("25EC0001-3AC0-45FB-AF72-A22200CC334C")));
            AddColumn("dbo.LogsResultsReminder", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("E39A0001-A4CA-46B8-B7EF-A22200E72B08")));
            AddColumn("dbo.LogsSubmittedRequestAwaitsResponse", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("688B0001-1572-41CA-8298-A22200CBD542")));
            AddColumn("dbo.LogsSubmittedRequestNeedsApproval", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("B7B30001-2704-4A57-A71A-A22200CC1736")));
            AddColumn("dbo.LogsUploadedResultNeedsApproval", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("F31C0001-6900-4BDB-A03A-A22200CC019C")));
            AddColumn("dbo.LogsUserChange", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("B7640001-7247-49B8-A818-A22200CCEAF7")));
            AddColumn("dbo.LogsUserRegistrationChanged", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("76B10001-2B49-453C-A8E1-A22200CC9356")));
            AddColumn("dbo.LogsUserRegistrationSubmitted", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("3AC20001-D8A4-4BE7-957C-A22200CC84BB")));

            Sql("UPDATE dbo.Events SET Name = '(UNUSED) ' + Name WHERE Name IN ('All Personal Events', 'Project Assignment')");
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogsDataMartChange", "EventID");
            DropColumn("dbo.LogsGroupChange", "EventID");
            DropColumn("dbo.LogsNewDataMartClient", "EventID");
            DropColumn("dbo.LogsOrganizationChange", "EventID");
            DropColumn("dbo.LogsPasswordExpiration", "EventID");
            DropColumn("dbo.LogsProfileUpdated", "EventID");
            DropColumn("dbo.LogsProjectChange", "EventID");
            DropColumn("dbo.LogsRegistryChange", "EventID");
            DropColumn("dbo.LogsRequestStatusChange", "EventID");
            DropColumn("dbo.LogsResponseViewed", "EventID");
            DropColumn("dbo.LogsResultsReminder", "EventID");
            DropColumn("dbo.LogsSubmittedRequestAwaitsResponse", "EventID");
            DropColumn("dbo.LogsSubmittedRequestNeedsApproval", "EventID");
            DropColumn("dbo.LogsUploadedResultNeedsApproval", "EventID");
            DropColumn("dbo.LogsUserChange", "EventID");
            DropColumn("dbo.LogsUserRegistrationChanged", "EventID");
            DropColumn("dbo.LogsUserRegistrationSubmitted", "EventID");

        }
    }
}
