namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecuityGroupTrigger : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[SecurityGroups_InsertUpdateItem] 
    ON  [dbo].[SecurityGroups]
    AFTER INSERT, UPDATE
AS 
BEGIN
    IF (EXISTS(SELECT NULL FROM INSERTED))    
	BEGIN
	    UPDATE SecurityGroups SET Path = ISNULL((SELECT TOP 1 Name FROM Organizations WHERE Organizations.ID = SecurityGroups.OwnerID), (SELECT TOP 1 Name FROM Projects WHERE Projects.ID = SecurityGroups.OwnerID)) + '\' + SecurityGroups.Name WHERE SecurityGroups.ID IN (SELECT I.ID FROM INSERTED I LEFT OUTER JOIN DELETED D ON I.ID = D.ID WHERE D.Name IS NULL OR I.Name <> D.Name OR I.OwnerID <> D.OwnerID)
		UPDATE SecurityGroups SET Owner = ISNULL((SELECT TOP 1 Name FROM Organizations WHERE Organizations.ID = SecurityGroups.OwnerID), (SELECT TOP 1 Name FROM Projects WHERE Projects.ID = SecurityGroups.OwnerID)) WHERE SecurityGroups.ID IN (SELECT I.ID FROM INSERTED I LEFT OUTER JOIN DELETED D ON I.ID = D.ID WHERE D.Name IS NULL OR I.Name <> D.Name OR I.OwnerID <> D.OwnerID)
	END
END");
        }
        
        public override void Down()
        {
        }
    }
}
