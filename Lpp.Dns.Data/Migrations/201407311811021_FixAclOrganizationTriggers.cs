namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAclOrganizationTriggers : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[AclOrganizations_DeleteItem] 
                        ON  [dbo].[AclOrganizations]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclOrganizations] a INNER JOIN deleted ON a.PermissionID = deleted.PermissionID AND a.OrganizationID = deleted.OrganizationID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");

            Sql(@"ALTER TRIGGER [dbo].[AclOrganizations_UpdateItem] 
                        ON  [dbo].[AclOrganizations]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [AclOrganizations] SET [AclOrganizations].Allowed = inserted.Allowed FROM [AclOrganizations] INNER JOIN inserted ON [AclOrganizations].PermissionID = inserted.PermissionID JOIN SecurityGroups ON [AclOrganizations].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [AclOrganizations].Overridden = 0
						END
                    END");
        }
        
        public override void Down()
        {
        }
    }
}
