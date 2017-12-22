namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOwnerStringToSecurityGroupsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SecurityGroups", "Owner", c => c.String(maxLength: 255));

            Sql(@"ALTER TRIGGER [dbo].[SecurityGroups_InsertUpdateItem] 
    ON  [dbo].[SecurityGroups]
    AFTER INSERT, UPDATE
AS 
BEGIN
    IF (EXISTS(SELECT NULL FROM INSERTED))    
	    UPDATE SecurityGroups SET Path = ISNULL((SELECT TOP 1 Name FROM Organizations WHERE Organizations.ID = SecurityGroups.OwnerID), (SELECT TOP 1 Name FROM Projects WHERE Projects.ID = SecurityGroups.OwnerID)) + '\' + SecurityGroups.Name WHERE SecurityGroups.ID IN (SELECT I.ID FROM INSERTED I JOIN DELETED D ON I.ID = D.ID WHERE I.Name <> D.Name OR I.OwnerID <> D.OwnerID)
		UPDATE SecurityGroups SET Owner = ISNULL((SELECT TOP 1 Name FROM Organizations WHERE Organizations.ID = SecurityGroups.OwnerID), (SELECT TOP 1 Name FROM Projects WHERE Projects.ID = SecurityGroups.OwnerID)) WHERE SecurityGroups.ID IN (SELECT I.ID FROM INSERTED I JOIN DELETED D ON I.ID = D.ID WHERE I.Name <> D.Name OR I.OwnerID <> D.OwnerID)
END");

            Sql(@"CREATE TRIGGER [dbo].[Projects_InsertUpdateItem] 
    ON  [dbo].[Projects]
    AFTER INSERT, UPDATE
AS 
BEGIN
    IF (EXISTS(SELECT NULL FROM INSERTED))    
	    UPDATE SecurityGroups SET Path = (SELECT TOP 1 Name FROM Projects WHERE Projects.ID = SecurityGroups.OwnerID) + '\' + SecurityGroups.Name WHERE SecurityGroups.ID IN (SELECT I.ID FROM INSERTED I JOIN DELETED D ON I.ID = D.ID WHERE I.Name <> D.Name)
		UPDATE SecurityGroups SET Owner = (SELECT TOP 1 Name FROM Projects WHERE Projects.ID = SecurityGroups.OwnerID) WHERE SecurityGroups.ID IN (SELECT I.ID FROM INSERTED I JOIN DELETED D ON I.ID = D.ID WHERE I.Name <> D.Name)
END");

            Sql(@"CREATE TRIGGER [dbo].[Organizations_InsertUpdateItem] 
    ON  [dbo].[Organizations]
    AFTER INSERT, UPDATE
AS 
BEGIN
    IF (EXISTS(SELECT NULL FROM INSERTED))    
	    UPDATE SecurityGroups SET Path = (SELECT TOP 1 Name FROM Organizations WHERE Organizations.ID = SecurityGroups.OwnerID) + '\' + SecurityGroups.Name WHERE SecurityGroups.ID IN (SELECT I.ID FROM INSERTED I JOIN DELETED D ON I.ID = D.ID WHERE I.Name <> D.Name)
		UPDATE SecurityGroups SET Owner = (SELECT TOP 1 Name FROM Organizations WHERE Organizations.ID = SecurityGroups.OwnerID) WHERE SecurityGroups.ID IN (SELECT I.ID FROM INSERTED I JOIN DELETED D ON I.ID = D.ID WHERE I.Name <> D.Name)
END");
        }
        
        public override void Down()
        {
            DropColumn("dbo.SecurityGroups", "Owner");
        }
    }
}
