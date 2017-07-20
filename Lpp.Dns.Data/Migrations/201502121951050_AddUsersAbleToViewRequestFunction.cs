namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsersAbleToViewRequestFunction : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION [dbo].[UsersAbleToViewRequest]
(	
	@RequestID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
--Rule #1
SELECT CreatedByID AS UserID FROM Requests WHERE ID = @RequestID
UNION
SELECT SubmittedByID FROM Requests WHERE ID = @RequestID

--Rule 2 and 4
UNION
--View, Reject, Approve/Reject Submit request
SELECT SecurityGroupUsers.UserID FROM dbo.AclGlobal a JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID 
	WHERE (a.PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' OR a.PermissionID = '0549F5C8-6C0E-4491-BE90-EE0F29652422' OR a.PermissionID = '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99') 
	AND EXISTS(SELECT NULL FROM Requests WHERE Requests.ID = @RequestID AND Requests.Status >= 300)
	AND NOT EXISTS(SELECT NULL FROM AclGlobal na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclProjects a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID JOIN Requests ON a.ProjectID = Requests.ProjectId 
	WHERE (a.PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' OR a.PermissionID = '0549F5C8-6C0E-4491-BE90-EE0F29652422' OR a.PermissionID = '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99') 
	AND Requests.ID = @RequestID AND Requests.Status >= 300
	AND NOT EXISTS(SELECT NULL FROM AclProjects na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND na.ProjectID = a.ProjectID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclDataMarts a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID JOIN RequestDataMarts ON a.DataMartID = RequestDataMarts.DataMartID 
	JOIN Requests ON RequestDataMarts.RequestID = Requests.ID 
	WHERE a.PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' AND Requests.ID = @RequestID AND Requests.Status >= 300
	AND NOT EXISTS(SELECT NULL FROM AclDataMarts na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND na.DataMartID = a.DataMartID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclProjectDataMarts a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID JOIN RequestDataMarts ON a.DataMartID = RequestDataMarts.DataMartID 
	JOIN Requests ON RequestDataMarts.RequestID = Requests.ID AND a.ProjectID = Requests.ProjectId 
	WHERE a.PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' AND Requests.ID = @RequestID AND Requests.Status >= 300
	AND NOT EXISTS(SELECT NULL FROM AclProjectDataMarts na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND a.DataMartID = na.DataMartID AND a.ProjectID = na.ProjectID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclOrganizations a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID JOIN Requests ON a.OrganizationID = Requests.OrganizationID 
	WHERE (a.PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' OR a.PermissionID = '0549F5C8-6C0E-4491-BE90-EE0F29652422' OR a.PermissionID = '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99') 
	AND Requests.ID = @RequestID AND Requests.Status >= 300
	AND NOT EXISTS(SELECT NULL FROM AclOrganizations na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND na.OrganizationID = a.OrganizationID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclOrganizations a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID
	JOIN DataMarts ON DataMarts.OrganizationID = a.OrganizationID 
	JOIN RequestDataMarts ON DataMarts.ID = RequestDataMarts.DataMartID 
	JOIN Requests ON RequestDataMarts.RequestID = Requests.ID
	WHERE a.PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' AND Requests.ID = @RequestID AND Requests.Status >= 300
	AND NOT EXISTS(SELECT NULL FROM AclOrganizations na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND na.OrganizationID = a.OrganizationID)
-- End Reject Request
UNION
-- Shared Folders
SELECT SecurityGroupUsers.UserID FROM dbo.AclGlobal a JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID 
	WHERE a.PermissionID = '5CCB0EC2-006D-4345-895E-5DD2C6C8C791'  AND EXISTS(SELECT NULL FROM Requests JOIN RequestSharedFolderRequests ON Requests.ID = RequestSharedFolderRequests.RequestID JOIN RequestSharedFolders ON RequestSharedFolderRequests.RequestSharedFolderID = RequestSharedFolders.ID WHERE Requests.ID = @RequestID AND Requests.Status >= 300)
	AND NOT EXISTS(SELECT NULL FROM AclGlobal na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclRequestSharedFolders a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID 
	JOIN RequestSharedFolderRequests ON a.RequestSharedFolderID = RequestSharedFolderRequests.RequestSharedFolderID
	JOIN Requests ON RequestSharedFolderRequests.RequestID = Requests.ID 
	WHERE a.PermissionID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' AND Requests.ID = @RequestID AND Requests.Status >= 300
	AND NOT EXISTS(SELECT NULL FROM AclRequestSharedFolders na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND na.RequestSharedFolderID = a.RequestSharedFolderID)
--End Shared Folders
--Rule 3 - See Requests
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclGlobal a JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID 
	WHERE (a.PermissionID = '5D6DD388-7842-40A1-A27A-B9782A445E20') 
	AND EXISTS(SELECT NULL FROM Requests WHERE Requests.ID = @RequestID AND Requests.Status >= 500)
	AND NOT EXISTS(SELECT NULL FROM AclGlobal na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclOrganizations a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID
	JOIN DataMarts ON DataMarts.OrganizationID = a.OrganizationID 
	JOIN RequestDataMarts ON DataMarts.ID = RequestDataMarts.DataMartID 
	JOIN Requests ON RequestDataMarts.RequestID = Requests.ID
	WHERE a.PermissionID = '5D6DD388-7842-40A1-A27A-B9782A445E20' AND Requests.ID = @RequestID AND Requests.Status >= 300
	AND NOT EXISTS(SELECT NULL FROM AclOrganizations na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND na.OrganizationID = a.OrganizationID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclProjectDataMarts a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID
	JOIN DataMarts ON DataMarts.ID = a.DataMartID 
	JOIN RequestDataMarts ON DataMarts.ID = RequestDataMarts.DataMartID 
	JOIN Requests ON RequestDataMarts.RequestID = Requests.ID AND a.ProjectID = Requests.ProjectId
	WHERE a.PermissionID = '5D6DD388-7842-40A1-A27A-B9782A445E20' AND Requests.ID = @RequestID AND Requests.Status >= 300
	AND NOT EXISTS(SELECT NULL FROM AclProjectDataMarts na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND a.ProjectID = na.ProjectID AND na.DataMartID = a.DataMartID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclDataMarts a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID
	JOIN DataMarts ON a.DataMartID = DataMarts.ID 
	JOIN RequestDataMarts ON DataMarts.ID = RequestDataMarts.DataMartID 
	JOIN Requests ON RequestDataMarts.RequestID = Requests.ID
	WHERE a.PermissionID = '5D6DD388-7842-40A1-A27A-B9782A445E20' AND Requests.ID = @RequestID AND Requests.Status >= 300
	AND NOT EXISTS(SELECT NULL FROM AclDataMarts na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND a.DataMartID = na.DataMartID)
--End Rule 3

-- Rule 5
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclGlobal a JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID 
	WHERE (a.PermissionID = 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6') 
	AND EXISTS(SELECT NULL FROM Requests WHERE Requests.ID = @RequestID AND Requests.Status >= 500 AND EXISTS(SELECT NULL FROM RequestDataMarts WHERE RequestDataMarts.RequestID = Requests.ID AND QueryStatusTypeId <> 1)) 	
	AND NOT EXISTS(SELECT NULL FROM AclGlobal na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclProjects a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID JOIN Requests ON a.ProjectID = Requests.ProjectId 
	WHERE (a.PermissionID = 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6') 
	AND Requests.ID = @RequestID AND Requests.Status >= 300 
	AND EXISTS(SELECT NULL FROM RequestDataMarts WHERE RequestDataMarts.RequestID = Requests.ID AND QueryStatusTypeId <> 1)
	AND NOT EXISTS(SELECT NULL FROM AclProjects na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND na.ProjectID = a.ProjectID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclOrganizations a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID JOIN Requests ON a.OrganizationID = Requests.OrganizationID 
	WHERE (a.PermissionID = 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6') 
	AND Requests.ID = @RequestID AND Requests.Status >= 300
	AND NOT EXISTS(SELECT NULL FROM AclOrganizations na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND na.OrganizationID = a.OrganizationID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclProjectDataMarts a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID
	JOIN DataMarts ON DataMarts.ID = a.DataMartID 
	JOIN RequestDataMarts ON DataMarts.ID = RequestDataMarts.DataMartID 
	JOIN Requests ON RequestDataMarts.RequestID = Requests.ID AND a.ProjectID = Requests.ProjectId
	WHERE a.PermissionID = 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6' AND Requests.ID = @RequestID AND Requests.Status >= 300 AND RequestDataMarts.QueryStatusTypeId <> 1
	AND NOT EXISTS(SELECT NULL FROM AclProjectDataMarts na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND na.ProjectID = a.ProjectID AND na.DataMartID = a.DataMartID)
UNION
SELECT SecurityGroupUsers.UserID FROM dbo.AclDataMarts a 
	JOIN SecurityGroupUsers ON a.SecurityGroupID = SecurityGroupUsers.SecurityGroupID
	JOIN DataMarts ON a.DataMartID = DataMarts.ID 
	JOIN RequestDataMarts ON DataMarts.ID = RequestDataMarts.DataMartID 
	JOIN Requests ON RequestDataMarts.RequestID = Requests.ID
	WHERE a.PermissionID = 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6' AND Requests.ID = @RequestID AND Requests.Status >= 300 AND RequestDataMarts.QueryStatusTypeId <> 1
	AND NOT EXISTS(SELECT NULL FROM AclDataMarts na WHERE na.Allowed = 0 AND na.PermissionID = a.PermissionID AND na.SecurityGroupID = a.SecurityGroupID AND na.DataMartID = a.DataMartID)
--End Rule 5
)");
        }
        
        public override void Down()
        {
        }
    }
}
