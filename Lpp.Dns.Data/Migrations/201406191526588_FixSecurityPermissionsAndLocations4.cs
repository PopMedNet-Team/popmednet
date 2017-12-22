namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecurityPermissionsAndLocations4 : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('15F3179B-7217-40C1-9345-CF371A7C79B3', 'Skip Two DataMart Rule', 'Allows the selected group to submit requests to a single DataMart')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('15F3179B-7217-40C1-9345-CF371A7C79B3', 0)");

            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '15F3179B-7217-40C1-9345-CF371A7C79B3'");

        }
        
        public override void Down()
        {
        }
    }
}
