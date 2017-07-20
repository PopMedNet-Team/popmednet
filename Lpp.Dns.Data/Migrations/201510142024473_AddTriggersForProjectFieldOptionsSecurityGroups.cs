namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTriggersForProjectFieldOptionsSecurityGroups : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TABLE [dbo].[AclProjectFieldOptions] ADD CONSTRAINT DF_AclProjectFieldOptions_Overridden  DEFAULT ((0)) FOR [Overridden]");
            
            Sql(@"CREATE TRIGGER [dbo].[AclProjectFieldOptions_InsertItem] 
                        ON  [dbo].[AclProjectFieldOptions]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclProjectFieldOptions] (SecurityGroupID, ProjectID, FieldIdentifier, Permission) 
                            SELECT SecurityGroups.ID, inserted.ProjectID, inserted.FieldIdentifier,  inserted.Permission FROM SecurityGroups 
                            JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID 
                            WHERE NOT EXISTS(SELECT NULL FROM [AclProjectFieldOptions] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID 
                            WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.FieldIdentifier = inserted.FieldIdentifier AND a.ProjectID = inserted.ProjectID AND a.Permission = inserted.Permission)
						END
                    END");

            Sql(@"CREATE TRIGGER [dbo].[AclProjectFieldOptions_DeleteItem] 
                        ON  [dbo].[AclProjectFieldOptions]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclProjectFieldOptions] a INNER JOIN deleted ON a.FieldIdentifier = deleted.FieldIdentifier 
                            JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");

            Sql(@"CREATE TRIGGER [dbo].[AclProjectFieldOptions_UpdateItem] 
                        ON  [dbo].[AclProjectFieldOptions]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [AclProjectFieldOptions] SET [AclProjectFieldOptions].Permission = inserted.Permission FROM [AclProjectFieldOptions] 
                            INNER JOIN inserted ON [AclProjectFieldOptions].FieldIdentifier = inserted.FieldIdentifier 
                            JOIN SecurityGroups ON [AclProjectFieldOptions].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID
                            WHERE [AclProjectFieldOptions].Overridden = 0
						END
                    END");

        }
        
        public override void Down()
        {
            Sql("DROP TRIGGER [dbo].[AclProjectFieldOptions_InsertItem]");

            Sql("DROP TRIGGER [dbo].[AclProjectFieldOptions_DeleteItem]");

            Sql("DROP TRIGGER [dbo].[AclProjectFieldOptions_UpdateItem]");

        }
    }
}
