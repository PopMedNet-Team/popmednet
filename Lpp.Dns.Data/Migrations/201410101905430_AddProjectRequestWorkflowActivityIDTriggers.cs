namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectRequestWorkflowActivityIDTriggers : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE  TRIGGER [dbo].[AclProjectRequestTypeWorkflowActivities_DeleteItem] 
                        ON  [dbo].[AclProjectRequestTypeWorkflowActivities]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclProjectRequestTypeWorkflowActivities] a INNER JOIN deleted ON a.PermissionID = deleted.PermissionID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0 AND a.ProjectID = deleted.ProjectID AND a.RequestTypeID = deleted.RequestTypeID AND a.WorkflowActivityID = deleted.WorkflowActivityID
						END
                    END");

            Sql(@"CREATE  TRIGGER [dbo].[AclProjectRequestTypeWorkflowActivities_InsertItem] 
                        ON  [dbo].[AclProjectRequestTypeWorkflowActivities]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclProjectRequestTypeWorkflowActivities] (SecurityGroupID, ProjectID, RequestTypeID, WorkflowActivityID, PermissionID, Allowed, Overridden) SELECT SecurityGroups.ID, inserted.ProjectID, inserted.RequestTypeID, inserted.WorkflowActivityID, inserted.PermissionID, inserted.Allowed, 0 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE 
							NOT EXISTS(SELECT NULL FROM [AclProjectRequestTypeWorkflowActivities] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.RequestTypeID = inserted.RequestTypeID AND a.ProjectID = inserted.ProjectID AND a.WorkflowActivityID = inserted.WorkflowActivityID)
						END
                    END");

            Sql(@"CREATE  TRIGGER [dbo].[AclProjectRequestTypeWorkflowActivities_UpdateItem] 
                        ON  [dbo].[AclProjectRequestTypeWorkflowActivities]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [AclProjectRequestTypeWorkflowActivities] SET [AclProjectRequestTypeWorkflowActivities].Allowed = inserted.Allowed FROM [AclProjectRequestTypeWorkflowActivities] INNER JOIN inserted ON [AclProjectRequestTypeWorkflowActivities].PermissionID = inserted.PermissionID JOIN SecurityGroups ON [AclProjectRequestTypeWorkflowActivities].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [AclProjectRequestTypeWorkflowActivities].Overridden = 0 AND [AclProjectRequestTypeWorkflowActivities].ProjectID = inserted.ProjectID AND [AclProjectRequestTypeWorkflowActivities].RequestTypeID = inserted.RequestTypeID AND [AclProjectRequestTypeWorkflowActivities].WorkflowActivityID = inserted.WorkflowActivityID
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[AclProjectOrganizations_DeleteItem] 
                        ON  [dbo].[AclProjectOrganizations]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclProjectOrganizations] a INNER JOIN deleted ON a.PermissionID = deleted.PermissionID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0 AND a.ProjectID = deleted.ProjectID AND a.OrganizationID = deleted.OrganizationID
						END
                    END");
            Sql(@"ALTER  TRIGGER [dbo].[AclProjectOrganizations_UpdateItem] 
                        ON  [dbo].[AclProjectOrganizations]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [AclProjectOrganizations] SET [AclProjectOrganizations].Allowed = inserted.Allowed FROM [AclProjectOrganizations] INNER JOIN inserted ON [AclProjectOrganizations].PermissionID = inserted.PermissionID JOIN SecurityGroups ON [AclProjectOrganizations].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [AclProjectOrganizations].Overridden = 0 AND [AclProjectOrganizations].ProjectID = inserted.ProjectID AND [AclProjectOrganizations].OrganizationID = inserted.OrganizationID
						END
                    END");
        }
        
        public override void Down()
        {
        }
    }
}
