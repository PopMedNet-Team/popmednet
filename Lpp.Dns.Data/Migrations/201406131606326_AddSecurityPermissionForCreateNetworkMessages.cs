namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSecurityPermissionForCreateNetworkMessages : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 s WHERE ID1 = 'BBBA0001-2BC2-4E12-A5B4-A22100FDBAFD' AND PrivilegeID = '4F3914D9-BD36-4B9F-A6B9-A368199BA94C'
AND ((SELECT COUNT(*) FROM AclGlobal a WHERE s.SubjectID = a.SecurityGroupID AND s.PrivilegeID = a.PermissionID) <= 0)");
        }
        
        public override void Down()
        {
        }
    }
}
