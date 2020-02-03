namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecurityPermissionsAndLocations : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE Permissions SET Name = 'Manage Group Security', Description = 'Allows the selected group to manage a group''s access rights.' WHERE ID = '368E7007-E95F-435C-8FAF-0B9FBC9CA997'");

            Sql(@"UPDATE Permissions SET Name = 'Manage User Security', Description = 'Allows the selected group to manage other user''s access rights.' WHERE ID = '268E7007-E95F-435C-8FAF-0B9FBC9CA997'");

            Sql(@"UPDATE Permissions SET Name = 'Manage Shared Folder Security', Description = 'Allows the selected group to manage shared folder access rights.' WHERE ID = '568E7007-E95F-435C-8FAF-0B9FBC9CA997'");

            Sql(@"UPDATE Permissions SET Name = 'Manage DataMart Security', Description='Allows the selected group to manage DataMart access rights.' WHERE ID = '668E7007-E95F-435C-8FAF-0B9FBC9CA997'");


            //Add project manage, edit, delete, view
            Sql(@"INSERT INTO Permissions (ID, Name, Description) VALUES ('468E7007-E95F-435C-8FAF-0B9FBC9CA997', 'Manage Project Security', 'Allows the selected group to manage Project access rights.')");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('468E7007-E95F-435C-8FAF-0B9FBC9CA997', 4)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('468E7007-E95F-435C-8FAF-0B9FBC9CA997', 0)");

            Sql(@"INSERT INTO Permissions (ID, Name, Description) VALUES ('4B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 'Edit Project', 'Allows the selected group to edit a project.')");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('4B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 4)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('4B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 0)");

            Sql(@"INSERT INTO Permissions (ID, Name, Description) VALUES ('4C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 'Delete Project', 'Allows the selected group to delete a project.')");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('4C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 4)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('4C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 0)");

            Sql(@"UPDATE Permissions SET Name = 'View Project', Description = 'Allows the selected group to view a project.' WHERE ID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791'");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('4CCB0EC2-006D-4345-895E-5DD2C6C8C791', 4)");

            //Copy project rights from Old tables
            //Manage
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID1, SubjectID, '468E7007-E95F-435C-8FAF-0B9FBC9CA997', CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997'
AND EXISTS(SELECT NULL FROM Projects WHERE ID = ID1) ");
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, '468E7007-E95F-435C-8FAF-0B9FBC9CA997', CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997'
AND ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29'");

            //Edit
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID1, SubjectID, '4B42D2D7-F7A7-4119-9CC5-22991DC12AD3', CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3'
AND EXISTS(SELECT NULL FROM Projects WHERE ID = ID1) ");
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, '4B42D2D7-F7A7-4119-9CC5-22991DC12AD3', CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3'
AND ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29'");

            //Delete
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID1, SubjectID, '4C019772-1B9D-48F8-9FCD-AC44BC6FD97B', CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B'
AND EXISTS(SELECT NULL FROM Projects WHERE ID = ID1) ");
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, '4C019772-1B9D-48F8-9FCD-AC44BC6FD97B', CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B'
AND ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29'");

            //View
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID1, SubjectID, '4CCB0EC2-006D-4345-895E-5DD2C6C8C791', CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791'
AND EXISTS(SELECT NULL FROM Projects WHERE ID = ID1) ");
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, '4CCB0EC2-006D-4345-895E-5DD2C6C8C791', CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 s WHERE PrivilegeID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791'
AND ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29'
AND (SELECT COUNT(*) FROM AclGlobal a WHERE a.SecurityGroupID=s.SubjectID AND a.PermissionID='4CCB0EC2-006D-4345-895E-5DD2C6C8C791') <= 0");
        }
        
        public override void Down()
        {
        }
    }
}
