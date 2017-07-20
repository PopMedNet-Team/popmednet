namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AclOrganization_Insert : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[AclOrganizations_InsertItem] 
                    ON  [dbo].[AclOrganizations]
                    AFTER INSERT
                AS 
                BEGIN
	                -- SET NOCOUNT ON added to prevent extra result sets from
	                -- interfering with SELECT statements.
	                SET NOCOUNT ON;
	                IF((SELECT COUNT(*) FROM inserted) > 0)
	                BEGIN						
		                INSERT INTO [AclOrganizations] (SecurityGroupID, OrganizationID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.OrganizationID, inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclOrganizations] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID)
	                END
                END");
        }
        
        public override void Down()
        {
        }
    }
}
