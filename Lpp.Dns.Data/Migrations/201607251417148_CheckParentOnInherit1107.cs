namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckParentOnInherit1107 : DbMigration
    {
        public override void Up()
        {
            
            //aclGlobal
            Sql(@"ALTER TRIGGER [dbo].[AclGlobal_DeleteItem] 
                        ON  [dbo].[AclGlobal]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclGlobal] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID
							WHERE a.Overridden = 0

							INSERT INTO [AclGlobal](SecurityGroupID, PermissionID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclGlobal g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID
						END
                    END");
            

        }

        public override void Down()
        {
        }
    }
}
