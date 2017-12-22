namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDMCTVFs : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'AclDataMartRights') 
    AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION AclDataMartRights");

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
)
");

            Sql(@"IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'AclDataMartAllowUnattendedProcessing') 
    AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION AclDataMartAllowUnattendedProcessing");

            Sql(@"CREATE FUNCTION [dbo].[AclDataMartAllowUnattendedProcessing] (@UserID uniqueidentifier, @RequestTypeID uniqueidentifier, @ProjectID uniqueidentifier, @DataMartID uniqueidentifier)
RETURNS TABLE 
AS
RETURN 
(
	SELECT DISTINCT Permission, RequestTypeID, acl.Overridden, acl.SecurityGroupID FROM AclDataMartRequestTypes acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
		WHERE sgu.UserID = @UserID AND
		acl.DataMartID = @DataMartID AND
		acl.RequestTypeID = @RequestTypeID AND
		acl.Permission = 2
	UNION
	SELECT DISTINCT Permission, RequestTypeID, acl.Overridden, acl.SecurityGroupID FROM AclProjectDataMartRequestTypes acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
		WHERE sgu.UserID = @UserID AND
		acl.DataMartID = @DataMartID AND
		acl.ProjectID = @ProjectID AND
		acl.RequestTypeID = @RequestTypeID AND
		acl.Permission = 2
	UNION
	SELECT DISTINCT Permission, RequestTypeID, acl.Overridden, acl.SecurityGroupID FROM AclProjectRequestTypes acl JOIN SecurityGroupUsers sgu ON acl.SecurityGroupID = sgu.SecurityGroupID
		WHERE sgu.UserID = @UserID AND
		acl.ProjectID = @ProjectID AND
		acl.RequestTypeID = @RequestTypeID AND
		acl.Permission = 2
)
");
        }
        
        public override void Down()
        {

        }
    }
}
