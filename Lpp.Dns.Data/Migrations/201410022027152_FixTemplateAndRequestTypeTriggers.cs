namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTemplateAndRequestTypeTriggers : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER  TRIGGER [dbo].[AclTemplates_DeleteItem] 
                        ON  [dbo].[AclTemplates]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclTemplates] a INNER JOIN deleted ON a.PermissionID = deleted.PermissionID AND a.TemplateID = deleted.TemplateID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[AclTemplates_InsertItem] 
                        ON  [dbo].[AclTemplates]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclTemplates] (SecurityGroupID, PermissionID, Allowed, TemplateID) SELECT SecurityGroups.ID, inserted.PermissionID, inserted.Allowed, inserted.TemplateID FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclTemplates] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.TemplateID = inserted.TemplateID)
						END
                    END");
            Sql(@"ALTER  TRIGGER [dbo].[AclTemplates_UpdateItem] 
                        ON  [dbo].[AclTemplates]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [AclTemplates] SET [AclTemplates].Allowed = inserted.Allowed FROM [AclTemplates] INNER JOIN inserted ON [AclTemplates].PermissionID = inserted.PermissionID JOIN SecurityGroups ON [AclTemplates].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [AclTemplates].Overridden = 0 AND AclTEmplates.TemplateID = inserted.TemplateID
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[AclRequestTypes_DeleteItem] 
                        ON  [dbo].[AclRequestTypes]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclRequestTypes] a INNER JOIN deleted ON a.PermissionID = deleted.PermissionID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0 AND a.RequestTypeID = deleted.RequestTypeID
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[AclRequestTypes_InsertItem] 
                        ON  [dbo].[AclRequestTypes]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclRequestTypes] (SecurityGroupID, PermissionID, Allowed, RequestTypeID) SELECT SecurityGroups.ID, inserted.PermissionID, inserted.Allowed, inserted.RequestTypeID FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclRequestTypes] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.RequestTypeID = inserted.RequestTypeID)
						END
                    END");
            Sql(@"ALTER  TRIGGER [dbo].[AclRequestTypes_UpdateItem] 
                        ON  [dbo].[AclRequestTypes]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [AclRequestTypes] SET [AclRequestTypes].Allowed = inserted.Allowed FROM [AclRequestTypes] INNER JOIN inserted ON [AclRequestTypes].PermissionID = inserted.PermissionID JOIN SecurityGroups ON [AclRequestTypes].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [AclRequestTypes].Overridden = 0 and AclRequestTypes.RequestTypeID = inserted.RequestTypeID
						END
                    END");
        }
        
        public override void Down()
        {
        }
    }
}
