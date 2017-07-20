using Lpp.Utilities;

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateUsers1 : DbMigration
    {
        public override void Up()
        {
            DropTable("ACL");

            AddColumn("RequestDataMarts", "UpdatedOn", c => c.DateTime(false, defaultValueSql: "GETUTCDATE()"));
            Sql(@"DROP TRIGGER UserDeleteTargetsMembershipAndSubjects");
            Sql(@"DROP TRIGGER UserUpdateTargetsMembershipAndSubjects");
            Sql(@"DROP TRIGGER QueryDataMarts_InsertUpdateDeleteItem");
            Sql(@"  CREATE TRIGGER [dbo].[RequestDataMarts_InsertUpdateDeleteItem] 
                        ON  [dbo].[RequestDataMarts]
                        AFTER INSERT, UPDATE, DELETE
                    AS 
                    BEGIN
	                    IF ((SELECT COUNT(*) FROM inserted) > 0)
		                    UPDATE Requests SET UpdatedOn = GETDATE() WHERE Requests.ID IN (SELECT RequestID FROM inserted)
                    END");

            Sql(@"  ALTER TRIGGER [dbo].[RequestDataMartResponses_InsertUpdateDelete] 
		ON  [dbo].[RequestDataMartResponses]
		AFTER INSERT, UPDATE, DELETE
	AS 
	BEGIN
		IF ((SELECT COUNT(*) FROM inserted) > 0)
        BEGIN
            UPDATE RequestDataMarts SET UpdatedOn = GETUTCDATE() WHERE RequestDataMarts.ID IN (SELECT RequestDataMartID FROM inserted)
        END
	END");

            //Activities
            DropForeignKey("Activities", "FK_Activities_Activities");
            Sql(MigrationHelpers.DropPrimaryKeyScript("Activities"));
            AddColumn("Activities", "ParentActivityID", c => c.Guid(true));
            Sql(
                "UPDATE Activities SET ParentActivityID = (SELECT TOP 1 SID FROM Activities a WHERE a.id = Activities.ParentId) WHERE NOT ParentId IS NULL");

            AddColumn("Requests", "ActivitySID", c => c.Guid(true));
            Sql(
                "UPDATE Requests SET ActivitySID = (SELECT TOP 1 SID FROM Activities WHERE Activities.Id = Requests.ActivityId) WHERE NOT Requests.ActivityId IS NULL");
            DropColumn("Requests", "ActivityId");
            RenameColumn("Requests", "ActivitySID", "ActivityID");

            DropColumn("Activities", "Id");
            DropIndex("Activities", "IX_ParentID");
            DropColumn("Activities", "ParentId");
            RenameColumn("Activities", "SID", "ID");
            AddPrimaryKey("Activities", "ID");
            CreateIndex("Activities", "ParentActivityID");
            
            //Requests
            AddColumn("Requests", "CreatedByID", c => c.Guid(true));
            Sql(
                "UPDATE Requests SET CreatedByID = (SELECT TOP 1 SID FROM Users WHERE UserId = Requests.CreatedByUserId)");
            AlterColumn("Requests", "CreatedByID", c => c.Guid(false));
            DropColumn("Requests", "CreatedbyUserId");

            AddColumn("Requests", "UpdatedByID", c => c.Guid(true));
            Sql(
                "UPDATE Requests SET UpdatedByID = (SELECT TOP 1 SID FROM Users WHERE UserId = Requests.UpdatedByUserId)");
            DropColumn("Requests", "UpdatedByUserId");

            //RequestDataMartResponses
            AddColumn("RequestDataMartResponses", "SubmittedByID", c => c.Guid(true));
            Sql(
                "UPDATE RequestDataMartResponses SET SubmittedByID = (SELECT TOP 1 SID FROM Users WHERE UserId = RequestDataMartResponses.SubmittedByUserId)");
            AlterColumn("RequestDataMartResponses", "SubmittedByID", c => c.Guid(false));
            Sql(MigrationHelpers.DropForeignKeyScript("RequestDataMartResponses", "SubmittedByUserId"));
            DropColumn("RequestDataMartResponses", "SubmittedbyUserId");

            Sql(MigrationHelpers.DropForeignKeyScript("RequestDataMartResponses", "RespondedByUserId"));
            AddColumn("RequestDataMartResponses", "RespondedByID", c => c.Guid(true));
            Sql(
                "UPDATE RequestDataMartResponses SET RespondedByID = (SELECT TOP 1 SID FROM Users WHERE UserId = RequestDataMartResponses.RespondedByUserId)");
            DropColumn("RequestDataMartResponses", "RespondedByUserId");

            //Roles
            DropForeignKey("Users", "FK_Users_RoleTypes_RoleTypeId");
            AddColumn("RoleTypes", "ID", c => c.Guid(false, defaultValueSql: MigrationHelpers.GuidDefaultValue));
            Sql(MigrationHelpers.DropPrimaryKeyScript("RoleTypes"));
            RenameColumn("RoleTypes", "RoleType", "Name");
            AlterColumn("RoleTypes", "Name", c => c.String(false, 200));
            AddColumn("RoleTypes", "D", c => c.String(false, defaultValue: ""));
            Sql("UPDATE RoleTypes SET D = Description WHERE NOT Description IS NULL");
            DropColumn("RoleTypes", "Description");
            RenameColumn("RoleTypes", "D", "Description");
            RenameTable("RoleTypes", "Roles");

            //UserPasswordTrace
            DropPrimaryKey("UserPasswordTrace", "UserPasswordTrace_PK");
            DropColumn("UserPasswordTrace", "Id");
            AddColumn("UserPasswordTrace", "UserSID", c => c.Guid(true));
            Sql(
                "UPDATE UserPasswordTrace SET UserSID = (SELECT TOP 1 SID FROM Users WHERE UserId = UserPasswordTrace.UserId)");
            DropForeignKey("UserPasswordTrace", "Fk_UserPwdTrace_Users_UserId");
            DropColumn("UserPasswordTrace", "UserId");
            RenameColumn("UserPasswordTrace", "UserSID", "UserID");
            AlterColumn("UserPasswordTrace", "UserID", c => c.Guid(false));

            AddColumn("UserPasswordTrace", "AddedByID", c => c.Guid(true));
            Sql(
                "UPDATE UserPasswordTrace SET AddedByID = (SELECT TOP 1 SID FROM Users WHERE UserId = UserPasswordTrace.AddedBy)");
            DropForeignKey("UserPasswordTrace", "Fk_UserPwdTrace_Users_ChangedBy");
            DropColumn("UserPasswordTrace", "AddedBy");
            AlterColumn("UserPasswordTrace", "AddedByID", c => c.Guid(false));

            AlterColumn("UserPasswordTrace", "Password", c => c.String(false, 100));
            RenameColumn("UserPasswordTrace", "DateAdded", "AddedOn");
            AddPrimaryKey("UserPasswordTrace", new string[] {"UserID", "AddedOn", "Password"});

            //Subscriptions
            AddColumn("Subscriptions", "SID", c => c.Guid(false, defaultValueSql: MigrationHelpers.GuidDefaultValue));
            Sql(MigrationHelpers.DropPrimaryKeyScript("Subscriptions"));
            DropColumn("Subscriptions", "Id");
            RenameColumn("Subscriptions", "SID", "ID");
            AddPrimaryKey("Subscriptions", "ID");
            AddColumn("Subscriptions", "UserID", c => c.Guid(true));
            Sql(
                "UPDATE Subscriptions SET UserID = (SELECT TOP 1 SID FROM Users WHERE Users.UserId = Subscriptions.OwnerId)");
            Sql(MigrationHelpers.DropForeignKeyScript("Subscriptions", "OwnerId"));
            DropColumn("Subscriptions", "OwnerId");
            AlterColumn("Subscriptions", "UserID", c => c.Guid(false));

            //SecurityGroupUsers
            Sql(MigrationHelpers.DropPrimaryKeyScript("SecurityGroupUsers"));
            DropForeignKey("SecurityGroupUsers", "FK_SecurityGroupUsers_Users_UserID");
            DropColumn("SecurityGroupUsers", "UserID");
            AddColumn("SecurityGroupUsers", "UserID", c => c.Guid(false));
            AddPrimaryKey("SecurityGroupUsers", new string[] { "SecurityGroupID", "UserID" });
            

            //Users
            DropPrimaryKey("Users", "PK_Users");
            DropColumn("Users", "UserId");
            RenameColumn("Users", "SID", "ID");
            AddColumn("Users", "RoleID", c => c.Guid(true));
            Sql("UPDATE Users SET RoleID = (SELECT TOP 1 ID FROM Roles WHERE RoleTypeId = Users.RoleTypeId)");
            DropColumn("Users", "RoleTypeId");


            //Roles
            DropColumn("Roles", "RoleTypeId");
            AddPrimaryKey("Roles", "ID");
            Sql(
                @"ALTER TABLE Users ADD CONSTRAINT FK_Users_Roles_RoleID FOREIGN KEY (RoleID) REFERENCES Roles(ID) ON DELETE SET NULL ON UPDATE CASCADE");

            //Requests
            Sql(
                @"ALTER TABLE Requests ADD CONSTRAINT FK_Requests_Users_SubmittedByID FOREIGN KEY (SubmittedByID) REFERENCES Users(ID) ON DELETE SET NULL ON UPDATE CASCADE");
            Sql(
                @"ALTER TABLE Requests ADD CONSTRAINT FK_Requests_Activities_ActivityID FOREIGN KEY (ActivityID) REFERENCES Activities(ID) ON DELETE SET NULL ON UPDATE CASCADE");

            AddForeignKey("Requests", "CreatedByID", "Users", "ID", false);
            AddForeignKey("Requests", "UpdatedByID", "Users", "ID", false);

            //RequestDataMartResponses
            AddForeignKey("RequestDataMartResponses", "RespondedByID", "Users", "ID", false);
            AddForeignKey("RequestDataMartResponses", "SubmittedByID", "Users", "ID", false);

            //SecurityGroupUsers
            AddForeignKey("SecurityGroupUsers", "UserID", "Users", "ID", true);
        }
        
        public override void Down()
        {

        }
    }
}
