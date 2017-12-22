namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCreateRegistriesPermissions : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF(NOT EXISTS(SELECT NULL FROM Permissions WHERE ID = '39A642B4-E782-4051-9329-3A7246052E16'))
	INSERT INTO Permissions (ID, Name, Description) VALUES ('39A642B4-E782-4051-9329-3A7246052E16', 'Create Registries', 'Allows the creation of Registries')

IF(NOT EXISTS(SELECT NULL FROM PermissionLocations WHERE PermissionID = '39A642B4-E782-4051-9329-3A7246052E16'))
	INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('39A642B4-E782-4051-9329-3A7246052E16', 0)");

            //Create Registries
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '39A642B4-E782-4051-9329-3A7246052E16' AND NOT EXISTS(SELECT NULL FROM AclGlobal a WHERE a.SecurityGroupID = SubjectID AND a.PermissionID = PrivilegeID) AND EXISTS(SELECT NULL FROM SecurityGroups WHERE SecurityGroups.ID = SubjectID)");
        }
        
        public override void Down()
        {
        }
    }
}
