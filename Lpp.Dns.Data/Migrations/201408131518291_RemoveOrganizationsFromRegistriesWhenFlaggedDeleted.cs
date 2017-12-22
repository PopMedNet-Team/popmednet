namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveOrganizationsFromRegistriesWhenFlaggedDeleted : DbMigration
    {
        public override void Up()            
        {
            Sql(@"ALTER TRIGGER [dbo].[Organizations_InsertUpdateItem] 
    ON  [dbo].[Organizations]
    AFTER INSERT, UPDATE
AS 
BEGIN
    IF (EXISTS(SELECT NULL FROM INSERTED))    
	    UPDATE SecurityGroups SET Path = (SELECT TOP 1 Name FROM Organizations WHERE Organizations.ID = SecurityGroups.OwnerID) + '\' + SecurityGroups.Name WHERE SecurityGroups.ID IN (SELECT I.ID FROM INSERTED I JOIN DELETED D ON I.ID = D.ID WHERE I.Name <> D.Name)
		UPDATE SecurityGroups SET Owner = (SELECT TOP 1 Name FROM Organizations WHERE Organizations.ID = SecurityGroups.OwnerID) WHERE SecurityGroups.ID IN (SELECT I.ID FROM INSERTED I JOIN DELETED D ON I.ID = D.ID WHERE I.Name <> D.Name)
		DELETE FROM OrganizationRegistries WHERE OrganizationID IN (SELECT ID FROM inserted WHERE inserted.IsDeleted = 1)
END");

            Sql(@"DELETE FROM OrganizationRegistries WHERE OrganizationID IN (SELECT ID FROM Organizations WHERE IsDeleted = 1)");
        }
        
        public override void Down()
        {
        }
    }
}
