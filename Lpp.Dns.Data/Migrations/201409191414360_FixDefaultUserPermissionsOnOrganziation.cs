namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDefaultUserPermissionsOnOrganziation : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM AclOrganizations WHERE PermissionID = '2B42D2D7-F7A7-4119-9CC5-22991DC12AD3'");
            Sql("DELETE FROM AclOrganizations WHERE PermissionID = '2C019772-1B9D-48F8-9FCD-AC44BC6FD97B'");
            Sql("DELETE FROM AclOrganizations WHERE PermissionID = '2CCB0EC2-006D-4345-895E-5DD2C6C8C791'");
            Sql("DELETE FROM AclOrganizations WHERE PermissionID = '268E7007-E95F-435C-8FAF-0B9FBC9CA997'");
            Sql("DELETE FROM AclOrganizations WHERE PermissionID = '4A7C9495-BB01-4EA7-9419-65ACE6B24865'");
            Sql("DELETE FROM AclOrganizations WHERE PermissionID = '92687123-6F38-400E-97EC-C837AA92305F'");
            Sql("DELETE FROM AclOrganizations WHERE PermissionID = '22FB4F13-0492-417F-ACA1-A1338F705748'");
            Sql("DELETE FROM AclOrganizations WHERE PermissionID = 'FDE2D32E-A045-4062-9969-00962E182367'");

            Sql("INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT DISTINCT ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple2 WHERE EXISTS(SELECT NULL FROM Organizations WHERE Organizations.ID = ID1) AND ID2 = '1D3A0001-4717-40A3-98A1-A22100FDE0ED' AND PrivilegeID IN ('2B42D2D7-F7A7-4119-9CC5-22991DC12AD3', '2C019772-1B9D-48F8-9FCD-AC44BC6FD97B', '2CCB0EC2-006D-4345-895E-5DD2C6C8C791', '268E7007-E95F-435C-8FAF-0B9FBC9CA997', '4A7C9495-BB01-4EA7-9419-65ACE6B24865', '92687123-6F38-400E-97EC-C837AA92305F', '22FB4F13-0492-417F-ACA1-A1338F705748', 'FDE2D32E-A045-4062-9969-00962E182367') AND EXISTS(SELECT NULL FROM SecurityGroups WHERE SecurityGroups.ID = SubjectID)");
        }
        
        public override void Down()
        {
        }
    }
}
