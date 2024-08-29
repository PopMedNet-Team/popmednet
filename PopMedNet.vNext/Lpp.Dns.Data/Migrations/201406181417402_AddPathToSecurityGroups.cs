namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPathToSecurityGroups : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SecurityGroups", "Path", c => c.String());

            Sql(@"CREATE TRIGGER [dbo].[SecurityGroups_InsertUpdateItem] 
    ON  [dbo].SecurityGroups
    AFTER INSERT, UPDATE
AS 
BEGIN
	UPDATE SecurityGroups SET Path = ISNULL((SELECT TOP 1 Name FROM Organizations WHERE Organizations.ID = SecurityGroups.OwnerID), (SELECT TOP 1 Name FROM Projects WHERE Projects.ID = SecurityGroups.OwnerID)) WHERE SecurityGroups.ID IN (SELECT I.ID FROM INSERTED I JOIN DELETED D ON I.ID = D.ID WHERE I.Name <> D.Name OR I.OwnerID <> D.OwnerID)
END");
        }
        
        public override void Down()
        {
            DropColumn("dbo.SecurityGroups", "Path");
            Sql(@"DROP TRIGGER [dbo].[SecurityGroups_InsertUpdateItem]");
        }
    }
}
