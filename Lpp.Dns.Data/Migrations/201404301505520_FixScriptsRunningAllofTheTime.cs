namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixScriptsRunningAllofTheTime : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[UserUpdateTargetsMembershipAndSubjects] 
    ON  [dbo].[Users]
    AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	IF (EXISTS(SELECT NULL FROM deleted JOIN inserted ON deleted.sid = inserted.sid WHERE deleted.isDeleted = 0 AND inserted.isDeleted = 1))
	BEGIN
		DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT SID FROM inserted WHERE isDeleted = 1) OR ObjectId2 IN (SELECT SID FROM inserted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT SID FROM inserted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT SID FROM inserted WHERE isDeleted = 1) OR ObjectId4 IN (SELECT SID FROM inserted WHERE isDeleted = 1)
		DELETE FROM AclEntries WHERE SubjectId IN (SELECT SID FROM inserted WHERE isDeleted = 1)
		DELETE FROM SecurityMembership WHERE [Start] IN (SELECT SID FROM inserted WHERE isDeleted = 1) OR [End] IN (SELECT SID FROM inserted WHERE isDeleted = 1)
	END
END");

            Sql(@"ALTER TRIGGER [dbo].[OrganizationUpdateTargets] 
    ON  [dbo].[Organizations]
    AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	IF (EXISTS(SELECT NULL FROM deleted JOIN inserted ON deleted.sid = inserted.sid WHERE deleted.isDeleted = 0 AND inserted.isDeleted = 1))
	BEGIN
		DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT SID FROM inserted WHERE IsDeleted = 1) OR ObjectId2 IN (SELECT SID FROM inserted WHERE IsDeleted = 1) OR ObjectId3 IN (SELECT SID FROM inserted WHERE IsDeleted = 1) OR ObjectId3 IN (SELECT SID FROM inserted WHERE IsDeleted = 1) OR ObjectId4 IN (SELECT SID FROM inserted WHERE IsDeleted = 1)
	END
END");

            Sql(@"ALTER TRIGGER [dbo].[ProjectUpdateTargets] 
    ON  [dbo].[Projects]
    AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	IF (EXISTS(SELECT NULL FROM deleted JOIN inserted ON deleted.sid = inserted.sid WHERE deleted.isDeleted = 0 AND inserted.isDeleted = 1))
	BEGIN
		DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT SID FROM inserted WHERE isDeleted = 1) OR ObjectId2 IN (SELECT SID FROM inserted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT SID FROM inserted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT SID FROM inserted WHERE isDeleted = 1) OR ObjectId4 IN (SELECT SID FROM inserted WHERE isDeleted = 1)
	END
END");

        }
        
        public override void Down()
        {
        }
    }
}
