namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestObserverAndEventSubscription : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestObservers",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        RequestID = c.Guid(nullable: false),
                        UserID = c.Guid(),
                        SecurityGroupID = c.Guid(),
                        DisplayName = c.String(maxLength: 150),
                        Email = c.String(maxLength: 150),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.RequestID)
                .Index(t => t.UserID)
                .Index(t => t.SecurityGroupID);
            
            CreateTable(
                "dbo.RequestObserverEventSubscriptions",
                c => new
                    {
                        RequestObserverID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        LastRunTime = c.DateTimeOffset(precision: 7),
                        NextDueTime = c.DateTimeOffset(precision: 7),
                        Frequency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequestObserverID, t.EventID })
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .ForeignKey("dbo.RequestObservers", t => t.RequestObserverID, cascadeDelete: true)
                .Index(t => t.RequestObserverID)
                .Index(t => t.EventID)
                .Index(t => t.LastRunTime)
                .Index(t => t.NextDueTime);

            Sql(@"ALTER TRIGGER [dbo].[Users_DeleteItem] 
        ON  [dbo].[Users]
        AFTER DELETE
    AS 
    BEGIN
		DELETE FROM SecurityGroups WHERE ID IN (SELECT ID FROM deleted)
		UPDATE Requests SET SubmittedByID = NULL WHERE SubmittedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET CreatedByID = NULL WHERE CreatedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET UpdatedByID = NULL WHERE UpdatedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET RejectedByID = NULL WHERE RejectedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET ApprovedForDraftByID = NULL WHERE ApprovedForDraftByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET CancelledByID = NULL WHERE CancelledByID IN (SELECT ID FROM deleted)
        DELETE FROM RequestObservers WHERE ID IN (SELECT ID FROM deleted)
	END");
            Sql(@"ALTER TRIGGER [dbo].[SecurityGroups_DeleteItem] 
        ON  [dbo].[SecurityGroups]
        AFTER DELETE
    AS 
    BEGIN
		UPDATE SecurityGroups SET ParentSecurityGroupID = NULL WHERE ParentSecurityGroupID IN (SELECT ID FROM deleted)

        DELETE FROM AclDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclGlobal WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclGroups WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclOrganizationDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclOrganizations WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjectDataMartRequestTypes WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjectDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjects WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRegistries WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequests WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequestSharedFolders WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequestTypes WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM UserEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM GlobalEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM DataMartEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM GroupEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM OrganizationEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM ProjectEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM RegistryEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclOrganizationUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclProjectOrganizations WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM ProjectOrganizationEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM ProjectDataMartEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
DELETE FROM RequestObservers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
END");
            
        }
        
        public override void Down()
        {
            Sql(@"ALTER TRIGGER [dbo].[Users_DeleteItem] 
        ON  [dbo].[Users]
        AFTER DELETE
    AS 
    BEGIN
		DELETE FROM SecurityGroups WHERE ID IN (SELECT ID FROM deleted)
		UPDATE Requests SET SubmittedByID = NULL WHERE SubmittedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET CreatedByID = NULL WHERE CreatedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET UpdatedByID = NULL WHERE UpdatedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET RejectedByID = NULL WHERE RejectedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET ApprovedForDraftByID = NULL WHERE ApprovedForDraftByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET CancelledByID = NULL WHERE CancelledByID IN (SELECT ID FROM deleted)
	END");

            Sql(@"ALTER TRIGGER [dbo].[SecurityGroups_DeleteItem] 
        ON  [dbo].[SecurityGroups]
        AFTER DELETE
    AS 
    BEGIN
		UPDATE SecurityGroups SET ParentSecurityGroupID = NULL WHERE ParentSecurityGroupID IN (SELECT ID FROM deleted)

        DELETE FROM AclDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclGlobal WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclGroups WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclOrganizationDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclOrganizations WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjectDataMartRequestTypes WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjectDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjects WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRegistries WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequests WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequestSharedFolders WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequestTypes WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM UserEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM GlobalEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM DataMartEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM GroupEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM OrganizationEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM ProjectEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM RegistryEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclOrganizationUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclProjectOrganizations WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM ProjectOrganizationEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM ProjectDataMartEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
END");

            DropForeignKey("dbo.RequestObservers", "RequestID", "dbo.Requests");
            DropForeignKey("dbo.RequestObservers", "UserID", "dbo.Users");
            DropForeignKey("dbo.RequestObservers", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.RequestObserverEventSubscriptions", "RequestObserverID", "dbo.RequestObservers");
            DropForeignKey("dbo.RequestObserverEventSubscriptions", "EventID", "dbo.Events");
            DropIndex("dbo.RequestObserverEventSubscriptions", new[] { "NextDueTime" });
            DropIndex("dbo.RequestObserverEventSubscriptions", new[] { "LastRunTime" });
            DropIndex("dbo.RequestObserverEventSubscriptions", new[] { "EventID" });
            DropIndex("dbo.RequestObserverEventSubscriptions", new[] { "RequestObserverID" });
            DropIndex("dbo.RequestObservers", new[] { "SecurityGroupID" });
            DropIndex("dbo.RequestObservers", new[] { "UserID" });
            DropIndex("dbo.RequestObservers", new[] { "RequestID" });
            DropTable("dbo.RequestObserverEventSubscriptions");
            DropTable("dbo.RequestObservers");
        }
    }
}
