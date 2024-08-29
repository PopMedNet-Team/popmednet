namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAclDataMartTriggers : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[AclDataMarts_DeleteItem] 
                        ON  [dbo].[AclDataMarts]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclDataMarts] a INNER JOIN deleted ON a.PermissionID = deleted.PermissionID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID AND a.DataMartID = deleted.DataMartID WHERE a.Overridden = 0
						END
                    END");
            Sql(@"ALTER TRIGGER [dbo].[AclDataMarts_InsertItem] 
                        ON  [dbo].[AclDataMarts]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclDataMarts] (SecurityGroupID, PermissionID, Allowed, DataMartID) SELECT SecurityGroups.ID, inserted.PermissionID, inserted.Allowed, inserted.DataMartID FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclDataMarts] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID)
						END
                    END");
            Sql(@"ALTER TRIGGER [dbo].[AclDataMarts_UpdateItem] 
                        ON  [dbo].[AclDataMarts]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [AclDataMarts] SET [AclDataMarts].Allowed = inserted.Allowed FROM [AclDataMarts] INNER JOIN inserted ON [AclDataMarts].PermissionID = inserted.PermissionID AND [AclDataMarts].DataMartID = inserted.DataMartID JOIN SecurityGroups ON [AclDataMarts].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [AclDataMarts].Overridden = 0
						END
                    END");
        }
        
        public override void Down()
        {
        }
    }
}
