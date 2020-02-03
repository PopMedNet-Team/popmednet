namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAclDataMartRights_TVF : DbMigration
    {
        public override void Up()
        {
            Sql(@"DROP FUNCTION [dbo].[AclDataMartRights]");
            Sql(@"CREATE FUNCTION [dbo].[AclDataMartRights] (@UserID uniqueidentifier, @RequestID uniqueidentifier, @DataMartID uniqueidentifier)
RETURNS @userPermissions TABLE (
	PermissionID uniqueidentifier,
	Allowed bit
) 
AS
BEGIN
	DECLARE @UploadResultsPermissionID uniqueidentifier = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0'
	DECLARE @HoldRequestPermissionID uniqueidentifier = '894619BE-9A73-4DA9-A43A-10BCC563031C'
	DECLARE @RejectRequestPermissionID uniqueidentifier = '0CABF382-93D3-4DAC-AA80-2DE500A5F945'
	DECLARE @ModifyResultsPermissionID uniqueidentifier = '80500001-D58E-4EEE-8541-A7CA010034F5'
	DECLARE @ModifiyAttachmentsPermissionID uniqueidentifier = 'D59FA0D4-15FA-4088-9A98-35CDD7902EC1'
	DECLARE @ViewAttachmentsPermissionID uniqueidentifier = '50157D72-8EED-45E4-B6F4-2A935191F57F'

	INSERT INTO @userPermissions
	SELECT DISTINCT PermissionID, Allowed FROM AclGlobal acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
			WHERE sgu.UserID = @UserID AND
			PermissionID IN (@UploadResultsPermissionID, @HoldRequestPermissionID, @RejectRequestPermissionID)
		UNION
		SELECT DISTINCT PermissionID, Allowed FROM AclProjectDataMarts acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID JOIN Requests r ON acl.ProjectID = r.ProjectId
			WHERE sgu.UserID = @UserID AND
			acl.DataMartID = @DataMartID AND
			acl.ProjectID = r.ProjectId AND
			r.ID = @RequestID AND
			PermissionID IN (@UploadResultsPermissionID, @HoldRequestPermissionID, @RejectRequestPermissionID, @ModifyResultsPermissionID, @ModifiyAttachmentsPermissionID, @ViewAttachmentsPermissionID)
		UNION
		SELECT DISTINCT PermissionID, Allowed FROM AclProjects acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID JOIN Requests r ON acl.ProjectID = r.ProjectId
			WHERE sgu.UserID = @UserID AND
			acl.ProjectID = r.ProjectId AND
			r.ID = @RequestID AND
			PermissionID IN (@UploadResultsPermissionID, @HoldRequestPermissionID, @RejectRequestPermissionID, @ModifyResultsPermissionID, @ModifiyAttachmentsPermissionID, @ViewAttachmentsPermissionID)
		UNION
		SELECT DISTINCT PermissionID, Allowed FROM AclDataMarts acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
			WHERE sgu.UserID = @UserID AND
			acl.DataMartID = @DataMartID AND
			EXISTS(SELECT NULL FROM ProjectDataMarts pdm JOIN Requests r ON pdm.ProjectId = r.ProjectId WHERE pdm.ProjectId = r.ProjectId AND pdm.DataMartID = acl.DataMartID AND r.ID = @RequestID) AND
			PermissionID IN (@UploadResultsPermissionID, @HoldRequestPermissionID, @RejectRequestPermissionID, @ModifyResultsPermissionID)
		UNION
		SELECT DISTINCT PermissionID, Allowed FROM AclOrganizations acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
			WHERE sgu.UserID = @UserID AND
			EXISTS(SELECT NULL FROM DataMarts dm JOIN ProjectDataMarts pdm ON dm.ID = pdm.DataMartID JOIN Requests r ON pdm.ProjectId = r.ProjectId WHERE dm.OrganizationID = acl.OrganizationID AND dm.ID = @DataMartID AND r.ID = @RequestID) AND
			PermissionID IN (@UploadResultsPermissionID, @HoldRequestPermissionID, @RejectRequestPermissionID, @ModifyResultsPermissionID, @ModifiyAttachmentsPermissionID, @ViewAttachmentsPermissionID)
		UNION
		SELECT DISTINCT acl.PermissionID, acl.Allowed FROM AclProjectOrganizations acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID JOIN Requests r ON acl.ProjectID = r.ProjectId
			WHERE sgu.UserID = @UserID AND
			acl.ProjectID = r.ProjectId AND
			r.ID = @RequestID AND
			EXISTS(SELECT NULL FROM DataMarts dm JOIN ProjectDataMarts pdm ON dm.ID = pdm.DataMartID WHERE dm.ID = @DataMartID AND dm.OrganizationID = acl.OrganizationID AND pdm.ProjectId = acl.ProjectID) AND
			PermissionID IN (@ModifiyAttachmentsPermissionID, @ViewAttachmentsPermissionID)
		UNION
		SELECT DISTINCT acl.PermissionID, acl.Allowed FROM AclProjectRequestTypeWorkflowActivities acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
			JOIN Requests r ON (acl.ProjectID = r.ProjectId AND acl.RequestTypeID = acl.RequestTypeID AND acl.WorkflowActivityID = r.WorkFlowActivityID)
			WHERE sgu.UserID = @UserID AND
			r.ID = @RequestID AND
			EXISTS(SELECT NULL FROM DataMarts dm JOIN ProjectDataMarts pdm ON dm.ID = pdm.DataMartID WHERE dm.ID = @DataMartID AND pdm.ProjectId = acl.ProjectID) AND
			PermissionID IN (@ModifiyAttachmentsPermissionID, @ViewAttachmentsPermissionID)
	RETURN
END");
        }
        
        public override void Down()
        {
            Sql(@"DROP FUNCTION [dbo].[AclDataMartRights]");
            Sql(@"CREATE FUNCTION [dbo].[AclDataMartRights] (@UserID uniqueidentifier, @ProjectID uniqueidentifier, @DataMartID uniqueidentifier)
RETURNS TABLE 
AS
RETURN 
(
	SELECT DISTINCT PermissionID, Allowed FROM AclGlobal acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
		WHERE sgu.UserID = @UserID AND
		(PermissionID = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0' OR PermissionID = '894619BE-9A73-4DA9-A43A-10BCC563031C' OR PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945')
	UNION
	SELECT DISTINCT PermissionID, Allowed FROM AclProjectDataMarts acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
		WHERE sgu.UserID = @UserID AND
		acl.DataMartID = @DataMartID AND
		acl.ProjectID = @ProjectID AND
		(PermissionID = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0' OR PermissionID = '894619BE-9A73-4DA9-A43A-10BCC563031C' OR PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' OR PermissionID = '80500001-D58E-4EEE-8541-A7CA010034F5')
	UNION
	SELECT DISTINCT PermissionID, Allowed FROM AclProjects acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
		WHERE sgu.UserID = @UserID AND
		acl.ProjectID = @ProjectID AND
		(PermissionID = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0' OR PermissionID = '894619BE-9A73-4DA9-A43A-10BCC563031C' OR PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' OR PermissionID = '80500001-D58E-4EEE-8541-A7CA010034F5')
	UNION
	SELECT DISTINCT PermissionID, Allowed FROM AclDataMarts acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
		WHERE sgu.UserID = @UserID AND
		acl.DataMartID = @DataMartID AND
		(PermissionID = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0' OR PermissionID = '894619BE-9A73-4DA9-A43A-10BCC563031C' OR PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' OR PermissionID = '80500001-D58E-4EEE-8541-A7CA010034F5')
	UNION
	SELECT DISTINCT PermissionID, Allowed FROM AclOrganizations acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
		WHERE sgu.UserID = @UserID AND
		EXISTS(SELECT NULL FROM DataMarts WHERE DataMarts.OrganizationID = acl.OrganizationID AND DataMarts.ID = @DataMartID) AND
		(PermissionID = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0' OR PermissionID = '894619BE-9A73-4DA9-A43A-10BCC563031C' OR PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' OR PermissionID = '80500001-D58E-4EEE-8541-A7CA010034F5')
)");
        }
    }
}
