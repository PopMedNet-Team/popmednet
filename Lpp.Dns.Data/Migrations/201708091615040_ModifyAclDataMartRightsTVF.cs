namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyAclDataMartRightsTVF : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER FUNCTION [dbo].[AclDataMartRights] (@UserID uniqueidentifier, @ProjectID uniqueidentifier, @DataMartID uniqueidentifier)
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
        
        public override void Down()
        {
            Sql(@"ALTER FUNCTION [dbo].[AclDataMartRights] (@UserID uniqueidentifier, @ProjectID uniqueidentifier, @DataMartID uniqueidentifier)
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
		(PermissionID = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0' OR PermissionID = '894619BE-9A73-4DA9-A43A-10BCC563031C' OR PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945')
	UNION
	SELECT DISTINCT PermissionID, Allowed FROM AclProjects acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
		WHERE sgu.UserID = @UserID AND
		acl.ProjectID = @ProjectID AND
		(PermissionID = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0' OR PermissionID = '894619BE-9A73-4DA9-A43A-10BCC563031C' OR PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945')
	UNION
	SELECT DISTINCT PermissionID, Allowed FROM AclDataMarts acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
		WHERE sgu.UserID = @UserID AND
		acl.DataMartID = @DataMartID AND
		(PermissionID = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0' OR PermissionID = '894619BE-9A73-4DA9-A43A-10BCC563031C' OR PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945')
	UNION
	SELECT DISTINCT PermissionID, Allowed FROM AclOrganizations acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
		WHERE sgu.UserID = @UserID AND
		EXISTS(SELECT NULL FROM DataMarts WHERE DataMarts.OrganizationID = acl.OrganizationID AND DataMarts.ID = @DataMartID) AND
		(PermissionID = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0' OR PermissionID = '894619BE-9A73-4DA9-A43A-10BCC563031C' OR PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945')
)");
        }
    }
}
