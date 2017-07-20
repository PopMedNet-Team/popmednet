namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixProjectsAndSEcurityGroupsSID : DbMigration
    {
        public override void Up()
        {
            //Projects
            RenameColumn("Projects", "SID", "ID");

            //SecurityGroups
            RenameColumn("SecurityGroups", "SID", "ID");

            //User password version
            DropColumn("Users", "Version");
            AddColumn("Users", "PasswordVersion", c => c.Int(false, defaultValue: 1));
            Sql(@"  CREATE TRIGGER [dbo].[Users_InsertUpdateDelete] 
		ON  [dbo].[Users]
		AFTER INSERT, UPDATE, DELETE
	AS 
	BEGIN
		IF ((SELECT COUNT(*) FROM inserted) > 0)
        BEGIN
            UPDATE Users SET LastUpdatedOn = GETUTCDATE() WHERE Users.ID IN (SELECT ID FROM inserted)
        END
	END");
        }
        
        public override void Down()
        {
        }
    }
}
