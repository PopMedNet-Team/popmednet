namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSecurityTargetsOnDelete2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE TRIGGER [dbo].[QueryDeleteTargets] 
    ON  [dbo].[Queries]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT SID FROM deleted) OR ObjectId2 IN (SELECT SID FROM deleted) OR ObjectId3 IN (SELECT SID FROM deleted) OR ObjectId3 IN (SELECT SID FROM deleted) OR ObjectId4 IN (SELECT SID FROM deleted)
END");
            Sql(@"CREATE TRIGGER [dbo].[RegistriesDeleteTargets] 
    ON  [dbo].[Registries]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT ID FROM deleted) OR ObjectId2 IN (SELECT ID FROM deleted) OR ObjectId3 IN (SELECT ID FROM deleted) OR ObjectId3 IN (SELECT ID FROM deleted) OR ObjectId4 IN (SELECT ID FROM deleted)
END");

            Sql(@"CREATE TRIGGER [dbo].[OrganizationDeleteTargets] 
    ON  [dbo].[Organizations]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT SID FROM deleted) OR ObjectId2 IN (SELECT SID FROM deleted) OR ObjectId3 IN (SELECT SID FROM deleted) OR ObjectId3 IN (SELECT SID FROM deleted) OR ObjectId4 IN (SELECT SID FROM deleted)
END");
            Sql(@"CREATE TRIGGER [dbo].[ProjectDeleteTargets] 
    ON  [dbo].[Projects]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT SID FROM deleted) OR ObjectId2 IN (SELECT SID FROM deleted) OR ObjectId3 IN (SELECT SID FROM deleted) OR ObjectId3 IN (SELECT SID FROM deleted) OR ObjectId4 IN (SELECT SID FROM deleted)
END");

            Sql(@"CREATE TRIGGER [dbo].[UserDeleteTargetsMembershipAndSubjects] 
    ON  [dbo].[Users]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT SID FROM deleted) OR ObjectId2 IN (SELECT SID FROM deleted) OR ObjectId3 IN (SELECT SID FROM deleted) OR ObjectId3 IN (SELECT SID FROM deleted) OR ObjectId4 IN (SELECT SID FROM deleted)
	DELETE FROM AclEntries WHERE SubjectId IN (SELECT SID FROM deleted)
	DELETE FROM SecurityMembership WHERE [Start] IN (SELECT SID FROM deleted) OR [End] IN (SELECT SID FROM deleted)
END");

            Sql(@"CREATE TRIGGER [dbo].[UserUpdateTargetsMembershipAndSubjects] 
    ON  [dbo].[Users]
    AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT SID FROM inserted WHERE isDeleted = 1) OR ObjectId2 IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR ObjectId4 IN (SELECT SID FROM deleted WHERE isDeleted = 1)
	DELETE FROM AclEntries WHERE SubjectId IN (SELECT SID FROM deleted WHERE isDeleted = 1)
	DELETE FROM SecurityMembership WHERE [Start] IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR [End] IN (SELECT SID FROM deleted WHERE isDeleted = 1)
END");
            Sql(@"CREATE TRIGGER [dbo].[ProjectUpdateTargets] 
    ON  [dbo].[Projects]
    AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR ObjectId2 IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR ObjectId4 IN (SELECT SID FROM deleted WHERE isDeleted = 1)
END");
            Sql(@"CREATE TRIGGER [dbo].[OrganizationUpdateTargets] 
    ON  [dbo].[Organizations]
    AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT SID FROM deleted WHERE IsDeleted = 1) OR ObjectId2 IN (SELECT SID FROM deleted WHERE IsDeleted = 1) OR ObjectId3 IN (SELECT SID FROM deleted WHERE IsDeleted = 1) OR ObjectId3 IN (SELECT SID FROM deleted WHERE IsDeleted = 1) OR ObjectId4 IN (SELECT SID FROM deleted WHERE IsDeleted = 1)
END
");
            Sql(@"CREATE TRIGGER [dbo].[RegistriesUpdateTargets] 
    ON  [dbo].[Registries]
    AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT ID FROM deleted WHERE isDeleted = 1) OR ObjectId2 IN (SELECT ID FROM deleted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT ID FROM deleted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT ID FROM deleted WHERE isDeleted = 1) OR ObjectId4 IN (SELECT ID FROM deleted WHERE isDeleted = 1)
END");
            Sql(@"CREATE TRIGGER [dbo].[QueryUpdateTargets] 
    ON  [dbo].[Queries]
    AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SecurityTargets WHERE ObjectId1 IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR ObjectId2 IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR ObjectId3 IN (SELECT SID FROM deleted WHERE isDeleted = 1) OR ObjectId4 IN (SELECT SID FROM deleted WHERE isDeleted = 1)
END");
        }

        public override void Down()
        {
            Sql("DROP TRIGGER [dbo].[QueryDeleteTargets]");
            Sql("DROP TRIGGER [dbo].[RegistriesDeleteTargets]");
            Sql("DROP TRIGGER [dbo].[OrganizationDeleteTargets]");
            Sql("DROP TRIGGER [dbo].[ProjectDeleteTargets]");
            Sql("DROP TRIGGER [dbo].[UserDeleteTargetsMembershipAndSubjects]");
            Sql("DROP TRIGGER [dbo].[UserUpdateTargetsMembershipAndSubjects]");
            Sql("DROP TRIGGER [dbo].[ProjectUpdateTargets]");
            Sql("DROP TRIGGER [dbo].[OrganizationUpdateTargets]");
            Sql("DROP TRIGGER [dbo].[RegistriesUpdateTargets]");
            Sql("DROP TRIGGER [dbo].[QueryUpdateTargets]");
        }
    }
}
