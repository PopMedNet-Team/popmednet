namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteTriggerCheckForParentAllAcls : DbMigration
    {
        public override void Up()
        {
            //AclDataMartRequestTypes
            Sql(@"ALTER TRIGGER [dbo].[AclDataMartRequestTypes_DeleteItem] 
                        ON  [dbo].[AclDataMartRequestTypes]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclDataMartRequestTypes] a 
							INNER JOIN deleted ON a.RequestTypeID = deleted.RequestTypeID AND a.DataMartID = deleted.DataMartID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclDataMartRequestTypes](SecurityGroupID, RequestTypeID, DataMartID, Permission) 
							SELECT d.SecurityGroupID, d.RequestTypeID, d.DataMartID, r.Permission FROM deleted d  
							JOIN SecurityGroups s ON s.ID = d.SecurityGroupID 
							JOIN AclDataMartRequestTypes r ON r.SecurityGroupID = s.ParentSecurityGroupID AND r.DataMartID = d.DataMartID AND r.RequestTypeID = d.RequestTypeID
						END
                    END");
            //aclDatamarts
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
							DELETE a FROM [AclDataMarts] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID AND a.DataMartID = deleted.DataMartID 
							WHERE a.Overridden = 0

							INSERT INTO [AclDataMarts](SecurityGroupID, PermissionID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclDataMarts g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.DataMartID = d.DataMartID
						END
                    END");
            
            //AclGroups
            Sql(@"ALTER TRIGGER [dbo].[AclGroups_DeleteItem] 
                        ON  [dbo].[AclGroups]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclGroups] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclGroups](SecurityGroupID, PermissionID, GroupID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, d.GroupID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclGroups g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.GroupID = d.GroupID
						END
                    END");
            //AclOrganizationDataMarts
            Sql(@"ALTER TRIGGER [dbo].[AclOrganizationDataMarts_DeleteItem] 
                        ON  [dbo].[AclOrganizationDataMarts]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclOrganizationDataMarts] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclOrganizationDataMarts](SecurityGroupID, OrganizationID, DataMartID, PermissionID, Allowed) SELECT d.SecurityGroupID, d.OrganizationID, d.DataMartID, d.PermissionID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclOrganizationDataMarts g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.DataMartID = d.DataMartID AND g.OrganizationID = d.OrganizationID
						END
                    END");
            //AclOrganizations
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
							DELETE a FROM [AclOrganizations] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID AND a.OrganizationID = deleted.OrganizationID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclOrganizations](SecurityGroupID, PermissionID, OrganizationID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, d.OrganizationID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclOrganizations g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.OrganizationID = d.OrganizationID
						END
                    END");
            //AclOrganizationUsers
            Sql(@"ALTER  TRIGGER [dbo].[AclOrganizationUsers_DeleteItem] 
                        ON  [dbo].[AclOrganizationUsers]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclOrganizationUsers] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclOrganizationUsers](SecurityGroupID, PermissionID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclOrganizationUsers g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.OrganizationID = d.OrganizationID AND g.UserID = d.UserID
						END
                    END");
            //AclProjectDataMartRequestTypes
            Sql(@"ALTER TRIGGER [dbo].[AclProjectDataMartRequestTypes_DeleteItem] 
                        ON  [dbo].[AclProjectDataMartRequestTypes]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclProjectDataMartRequestTypes] a 
							INNER JOIN deleted ON a.RequestTypeID = deleted.RequestTypeID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID
							 AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID
							  AND a.DataMartID = deleted.DataMartID 
							  AND a.ProjectID = deleted.ProjectID 
							WHERE a.Overridden = 0

							INSERT INTO [AclProjectDataMartRequestTypes](SecurityGroupID, ProjectID, DataMartID, RequestTypeID, Permission) SELECT d.SecurityGroupID, d.ProjectID, d.DataMartID, d.RequestTypeID, g.Permission FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclProjectDataMartRequestTypes g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.DataMartID = d.DataMartID AND g.ProjectID = d.ProjectID AND g.RequestTypeID = d.RequestTypeID
						END
                    END");
            //AclProjectDataMarts
            Sql(@"ALTER TRIGGER [dbo].[AclProjectDataMarts_DeleteItem] 
                        ON  [dbo].[AclProjectDataMarts]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclProjectDataMarts] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID 
							AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclProjectDataMarts](SecurityGroupID, PermissionID, ProjectID, DataMartID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, d.ProjectID, d.DataMartID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclProjectDataMarts g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.ProjectID = d.ProjectID AND g.DataMartID = d.DataMartID
						END
                    END");
            //AclProjectFieldOptions
            Sql(@"ALTER TRIGGER [dbo].[AclProjectFieldOptions_DeleteItem] 
                        ON  [dbo].[AclProjectFieldOptions]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclProjectFieldOptions] a 
							INNER JOIN deleted ON a.FieldIdentifier = deleted.FieldIdentifier 
                            JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclProjectFieldOptions](SecurityGroupID, ProjectID, FieldIdentifier, Permission) SELECT d.SecurityGroupID, d.ProjectID, d.FieldIdentifier, g.Permission FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclProjectFieldOptions g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.ProjectID = d.ProjectID AND g.FieldIdentifier = d.FieldIdentifier
						END
                    END");
            //AclProjectOrganizations
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
							DELETE a FROM [AclProjectOrganizations] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0 AND a.ProjectID = deleted.ProjectID AND a.OrganizationID = deleted.OrganizationID

							INSERT INTO [AclProjectOrganizations](SecurityGroupID, PermissionID, ProjectID, OrganizationID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, d.ProjectID, d.OrganizationID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclProjectOrganizations g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.ProjectID = d.ProjectID AND g.OrganizationID = d.OrganizationID
						END
                    END");
            //AclProjectRequestTypes
            Sql(@"ALTER TRIGGER [dbo].[AclProjectRequestTypes_DeleteItem] 
                        ON  [dbo].[AclProjectRequestTypes]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclProjectRequestTypes] a 
							INNER JOIN deleted ON a.RequestTypeID = deleted.RequestTypeID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID 
							AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							AND a.ProjectID = deleted.ProjectID 
							WHERE a.Overridden = 0

							INSERT INTO [AclProjectRequestTypes](SecurityGroupID, ProjectID, RequestTypeID, Permission) SELECT d.SecurityGroupID, d.ProjectID, d.RequestTypeID, g.Permission FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclProjectRequestTypes g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.ProjectID = d.ProjectID AND g.RequestTypeID = d.RequestTypeID
						END
                    END");
            //AclProjectRequestTypeWorkflowActivities
            Sql(@"ALTER  TRIGGER [dbo].[AclProjectRequestTypeWorkflowActivities_DeleteItem] 
                        ON  [dbo].[AclProjectRequestTypeWorkflowActivities]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclProjectRequestTypeWorkflowActivities] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0 AND a.ProjectID = deleted.ProjectID AND a.RequestTypeID = deleted.RequestTypeID AND a.WorkflowActivityID = deleted.WorkflowActivityID

							INSERT INTO [AclProjectRequestTypeWorkflowActivities](SecurityGroupID, PermissionID, ProjectID, RequestTypeID, WorkflowActivityID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, d.ProjectID, d.RequestTypeID, d.WorkflowActivityID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclProjectRequestTypeWorkflowActivities g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.ProjectID = d.ProjectID AND g.RequestTypeID = d.RequestTypeID AND g.WorkflowActivityID = d.WorkflowActivityID
						END
                    END");
            //AclProjects
            Sql(@"ALTER TRIGGER [dbo].[AclProjects_DeleteItem] 
                        ON  [dbo].[AclProjects]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclProjects] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclProjects](SecurityGroupID, PermissionID, ProjectID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, d.ProjectID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclProjects g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.ProjectID = d.ProjectID
						END
                    END");
            //AclRegistries
            Sql(@"ALTER TRIGGER [dbo].[AclRegistries_DeleteItem] 
                        ON  [dbo].[AclRegistries]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclRegistries] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclRegistries](SecurityGroupID, PermissionID, RegistryID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, d.RegistryID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclRegistries g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.RegistryID = d.RegistryID
						END
                    END");
            //AclRequests
            Sql(@"ALTER TRIGGER [dbo].[AclRequests_DeleteItem] 
                        ON  [dbo].[AclRequests]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclRequests] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclRequests](SecurityGroupID, PermissionID, RequestID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, d.RequestID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclRequests g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.RequestID = d.RequestID
						END
                    END");
            //AclRequestSharedFolders
            Sql(@"ALTER TRIGGER [dbo].[AclRequestSharedFolders_DeleteItem] 
                        ON  [dbo].[AclRequestSharedFolders]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclRequestSharedFolders] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclRequestSharedFolders](SecurityGroupID, PermissionID, RequestSharedFolderID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, d.RequestSharedFolderID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclRequestSharedFolders g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.RequestSharedFolderID = d.RequestSharedFolderID
						END
                    END");
            //AclTemplates
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
							DELETE a FROM [AclTemplates] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID AND a.TemplateID = deleted.TemplateID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0

							INSERT INTO [AclTemplates](SecurityGroupID, PermissionID, TemplateID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, d.TemplateID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclTemplates g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.TemplateID = d.TemplateID
						END
                    END");
            //AclUsers
            Sql(@"ALTER TRIGGER [dbo].[AclUsers_DeleteItem] 
                        ON  [dbo].[AclUsers]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclUsers] a 
							INNER JOIN deleted ON a.PermissionID = deleted.PermissionID 
							JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID 
							WHERE a.Overridden = 0


							INSERT INTO [AclUsers](SecurityGroupID, PermissionID, UserID, Allowed) SELECT d.SecurityGroupID, d.PermissionID, d.UserID, g.Allowed FROM deleted d  
							JOIN SecurityGroups s ON d.SecurityGroupID = s.ID 
							JOIN AclUsers g ON s.ParentSecurityGroupID = g.SecurityGroupID AND g.PermissionID = d.PermissionID AND g.UserID = d.UserID
						END
                    END");
        }
        
        public override void Down()
        {
        }
    }
}
