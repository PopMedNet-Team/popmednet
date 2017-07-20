namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventsToDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name);
            
            CreateIndex("dbo.AclGroupEvents", "EventID");
            CreateIndex("dbo.AclDataMartEvents", "EventID");
            CreateIndex("dbo.AclEvents", "EventID");
            CreateIndex("dbo.AclOrganizationEvents", "EventID");
            CreateIndex("dbo.ProjectDataMartEvents", "EventID");
            CreateIndex("dbo.AclProjectEvents", "EventID");
            CreateIndex("dbo.ProjectOrganizationEvents", "EventID");
            CreateIndex("dbo.AclRegistryEvents", "EventID");
            CreateIndex("dbo.AclUserEvents", "EventID");

            //Add all of the events to the table here
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('06E30001-ED86-4427-9936-A22200CC74F0', 'New Request Submitted', 'Users will be notified whenever a new request is submitted that they have permission to see.')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('0A850001-FC8A-4DE2-9AA5-A22200E82398', 'Request Status Changed', 'Users will be notified whenever a request''s status changes that they have permission to see.')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('5AB90001-8072-42CD-940F-A22200CC24A2', 'Routing Status Changed', 'Users will be notified whenever a request''s DataMart routing status changes that they have permission to see.')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('25EC0001-3AC0-45FB-AF72-A22200CC334C', 'Results Views', 'Users will be notified whenever a request''s results are viewed that they have permission to see.')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('B7B30001-2704-4A57-A71A-A22200CC1736', 'Submitted Request Needs Approval', 'Users will be notified whenever a submitted request requires approval that they have permission to see.')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('E39A0001-A4CA-46B8-B7EF-A22200E72B08', 'Results Reminder', 'Users will be reminded whenever a request has unexamined results that they have permission to see.')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('688B0001-1572-41CA-8298-A22200CBD542', 'Submitted Request Awaits a Response', 'Users will be reminded whenever a submitted request is awaiting response that they have permission to see.')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('F31C0001-6900-4BDB-A03A-A22200CC019C', 'Uploaded Result Needs Approval', 'Users will be notified whenever an uploaded result needs approval for a request that they have permission to see.')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('B7640001-7247-49B8-A818-A22200CCEAF7', 'User Change', 'Users will be notified whenever a change occurs to a user that they have permission to see')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('B8A50001-B556-43D2-A1B8-A22200CD12DC', 'Organization Change', 'Users will be notified whenever a change occurs to an organization that they have permission to see')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('59A90001-539E-4C21-A4F2-A22200CD3C7D', 'DataMart Change', 'Users will be notified whenever a change occurs to a DataMart that they have permission to see')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('D80E0001-27BC-4FCB-BA75-A22200CD2426', 'Group Change', 'Users will be notified whenever a change occurs to a Group that they have permission to see')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('1C090001-9F95-400C-9780-A22200CD0234', 'Project Change', 'Users will be notified whenever a change occurs to a Project that they have permission to see')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('553FD350-8F3B-40C6-9E31-11D8BC7420A2', 'Registry Change', 'Users will be notified whenever a change occurs to a Registry that they have permission to see')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('C2790001-2FF6-456C-9497-A22200CCCD1F', 'Password Expiration Reminder', 'Users will be notified whenever their password is about to expire')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('B6B10001-07FB-47F5-83B8-A22200CCDB90', 'Profile Updated', 'Users will be notified whenever their profile is changed')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('60F20001-77FF-4F4B-9153-A2220129E466', 'New DataMart is Available', 'Users will be notified whenever there is a new DataMart Client available')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('3AC20001-D8A4-4BE7-957C-A22200CC84BB', 'Registration Submitted', 'Users will be notified whenever there is a new user registration submitted')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('76B10001-2B49-453C-A8E1-A22200CC9356', 'Registration Status Change', 'Users will be notified whenever a new user registration''s status changes')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('D2460001-F0FA-4BAA-AEE1-A22200CCADB4', 'Project Assignment', 'Users will be notified whenever a project is assigned to them')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('D5EF0001-4122-477E-9C55-A2210142C609', 'All Personal Events', 'Users will be notified whenever a personal event fires')");

            Sql("DELETE FROM ProjectOrganizationEvents WHERE NOT EXISTS(SELECT NULL FROM Events WHERE ID = EventID)");

            AddForeignKey("dbo.AclDataMartEvents", "EventID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclEvents", "EventID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclGroupEvents", "EventID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclOrganizationEvents", "EventID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectDataMartEvents", "EventID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclProjectEvents", "EventID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectOrganizationEvents", "EventID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclRegistryEvents", "EventID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclUserEvents", "EventID", "dbo.Events", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AclUserEvents", "EventID", "dbo.Events");
            DropForeignKey("dbo.AclRegistryEvents", "EventID", "dbo.Events");
            DropForeignKey("dbo.ProjectOrganizationEvents", "EventID", "dbo.Events");
            DropForeignKey("dbo.AclProjectEvents", "EventID", "dbo.Events");
            DropForeignKey("dbo.ProjectDataMartEvents", "EventID", "dbo.Events");
            DropForeignKey("dbo.AclOrganizationEvents", "EventID", "dbo.Events");
            DropForeignKey("dbo.AclGroupEvents", "EventID", "dbo.Events");
            DropForeignKey("dbo.AclEvents", "EventID", "dbo.Events");
            DropForeignKey("dbo.AclDataMartEvents", "EventID", "dbo.Events");
            DropIndex("dbo.AclUserEvents", new[] { "EventID" });
            DropIndex("dbo.AclRegistryEvents", new[] { "EventID" });
            DropIndex("dbo.ProjectOrganizationEvents", new[] { "EventID" });
            DropIndex("dbo.AclProjectEvents", new[] { "EventID" });
            DropIndex("dbo.ProjectDataMartEvents", new[] { "EventID" });
            DropIndex("dbo.AclOrganizationEvents", new[] { "EventID" });
            DropIndex("dbo.AclEvents", new[] { "EventID" });
            DropIndex("dbo.AclDataMartEvents", new[] { "EventID" });
            DropIndex("dbo.Events", new[] { "Name" });
            DropIndex("dbo.AclGroupEvents", new[] { "EventID" });
            DropTable("dbo.Events");
        }
    }
}
