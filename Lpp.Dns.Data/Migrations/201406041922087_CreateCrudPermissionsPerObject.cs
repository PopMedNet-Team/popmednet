namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCrudPermissionsPerObject : DbMigration
    {
        public override void Up()
        {
            Sql(@"EXEC sp_msforeachtable ""ALTER TABLE ? DISABLE TRIGGER ALL""");

            //Organization
            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('0B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 'Edit Organization', 'Allows the user to edit an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('0C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 'Delete Organization', 'Allows the user to delete an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('0CCB0EC2-006D-4345-895E-5DD2C6C8C791', 'View Organization', 'Allows the user to view an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0CCB0EC2-006D-4345-895E-5DD2C6C8C791', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0CCB0EC2-006D-4345-895E-5DD2C6C8C791', 3)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('068E7007-E95F-435C-8FAF-0B9FBC9CA997', 'Manage Organizational Security', 'Allows the user to manage an organization''s access permissions')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('068E7007-E95F-435C-8FAF-0B9FBC9CA997', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('068E7007-E95F-435C-8FAF-0B9FBC9CA997', 3)");


            Sql(
                "UPDATE AclOrganizations SET PermissionID = '0B42D2D7-F7A7-4119-9CC5-22991DC12AD3' WHERE PermissionID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3'"); //Edit
            Sql(
                "UPDATE AclOrganizations SET PermissionID = '0C019772-1B9D-48F8-9FCD-AC44BC6FD97B' WHERE PermissionID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B'"); //Delete
            Sql(
                "UPDATE AclOrganizations SET PermissionID = '0CCB0EC2-006D-4345-895E-5DD2C6C8C791' WHERE PermissionID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791'"); //View
            Sql(
                "UPDATE AclOrganizations SET PermissionID = '068E7007-E95F-435C-8FAF-0B9FBC9CA997' WHERE PermissionID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997'"); //Manage Security


            //DataMart
            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('6B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 'Edit Organization', 'Allows the user to edit an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('6C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 'Delete Organization', 'Allows the user to delete an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('6CCB0EC2-006D-4345-895E-5DD2C6C8C791', 'View Organization', 'Allows the user to view an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6CCB0EC2-006D-4345-895E-5DD2C6C8C791', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6CCB0EC2-006D-4345-895E-5DD2C6C8C791', 3)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('668E7007-E95F-435C-8FAF-0B9FBC9CA997', 'Manage Organizational Security', 'Allows the user to manage an organization''s access permissions')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('668E7007-E95F-435C-8FAF-0B9FBC9CA997', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('668E7007-E95F-435C-8FAF-0B9FBC9CA997', 3)");


            Sql(
                "UPDATE AclDataMarts SET PermissionID = '6B42D2D7-F7A7-4119-9CC5-22991DC12AD3' WHERE PermissionID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3'"); //Edit
            Sql(
                "UPDATE AclDataMarts SET PermissionID = '6C019772-1B9D-48F8-9FCD-AC44BC6FD97B' WHERE PermissionID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B'"); //Delete
            Sql(
                "UPDATE AclDataMarts SET PermissionID = '6CCB0EC2-006D-4345-895E-5DD2C6C8C791' WHERE PermissionID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791'"); //View
            Sql(
                "UPDATE AclDataMarts SET PermissionID = '668E7007-E95F-435C-8FAF-0B9FBC9CA997' WHERE PermissionID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997'"); //Manage Security

            //User
            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('2B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 'Edit Organization', 'Allows the user to edit an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('2C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 'Delete Organization', 'Allows the user to delete an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('2CCB0EC2-006D-4345-895E-5DD2C6C8C791', 'View Organization', 'Allows the user to view an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2CCB0EC2-006D-4345-895E-5DD2C6C8C791', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2CCB0EC2-006D-4345-895E-5DD2C6C8C791', 3)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('268E7007-E95F-435C-8FAF-0B9FBC9CA997', 'Manage Organizational Security', 'Allows the user to manage an organization''s access permissions')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('268E7007-E95F-435C-8FAF-0B9FBC9CA997', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('268E7007-E95F-435C-8FAF-0B9FBC9CA997', 3)");


            Sql(
                "UPDATE AclUsers SET PermissionID = '2B42D2D7-F7A7-4119-9CC5-22991DC12AD3' WHERE PermissionID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3'"); //Edit
            Sql(
                "UPDATE AclUsers SET PermissionID = '2C019772-1B9D-48F8-9FCD-AC44BC6FD97B' WHERE PermissionID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B'"); //Delete
            Sql(
                "UPDATE AclUsers SET PermissionID = '2CCB0EC2-006D-4345-895E-5DD2C6C8C791' WHERE PermissionID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791'"); //View
            Sql(
                "UPDATE AclUsers SET PermissionID = '268E7007-E95F-435C-8FAF-0B9FBC9CA997' WHERE PermissionID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997'"); //Manage Security

            //Groups
            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('3B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 'Edit Organization', 'Allows the user to edit an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('3C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 'Delete Organization', 'Allows the user to delete an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('3CCB0EC2-006D-4345-895E-5DD2C6C8C791', 'View Organization', 'Allows the user to view an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3CCB0EC2-006D-4345-895E-5DD2C6C8C791', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3CCB0EC2-006D-4345-895E-5DD2C6C8C791', 3)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('368E7007-E95F-435C-8FAF-0B9FBC9CA997', 'Manage Organizational Security', 'Allows the user to manage an organization''s access permissions')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('368E7007-E95F-435C-8FAF-0B9FBC9CA997', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('368E7007-E95F-435C-8FAF-0B9FBC9CA997', 3)");


            Sql(
                "UPDATE AclGroups SET PermissionID = '3B42D2D7-F7A7-4119-9CC5-22991DC12AD3' WHERE PermissionID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3'"); //Edit
            Sql(
                "UPDATE AclGroups SET PermissionID = '3C019772-1B9D-48F8-9FCD-AC44BC6FD97B' WHERE PermissionID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B'"); //Delete
            Sql(
                "UPDATE AclGroups SET PermissionID = '3CCB0EC2-006D-4345-895E-5DD2C6C8C791' WHERE PermissionID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791'"); //View
            Sql(
                "UPDATE AclGroups SET PermissionID = '368E7007-E95F-435C-8FAF-0B9FBC9CA997' WHERE PermissionID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997'"); //Manage Security


            //Projects
            Sql(
"INSERT INTO Permissions (ID, Name, Description) VALUES ('7B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 'Edit Organization', 'Allows the user to edit an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('7C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 'Delete Organization', 'Allows the user to delete an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('7CCB0EC2-006D-4345-895E-5DD2C6C8C791', 'View Organization', 'Allows the user to view an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7CCB0EC2-006D-4345-895E-5DD2C6C8C791', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7CCB0EC2-006D-4345-895E-5DD2C6C8C791', 3)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('768E7007-E95F-435C-8FAF-0B9FBC9CA997', 'Manage Organizational Security', 'Allows the user to manage an organization''s access permissions')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('768E7007-E95F-435C-8FAF-0B9FBC9CA997', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('768E7007-E95F-435C-8FAF-0B9FBC9CA997', 3)");


            Sql(
                "UPDATE AclProjects SET PermissionID = '7B42D2D7-F7A7-4119-9CC5-22991DC12AD3' WHERE PermissionID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3'"); //Edit
            Sql(
                "UPDATE AclProjects SET PermissionID = '7C019772-1B9D-48F8-9FCD-AC44BC6FD97B' WHERE PermissionID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B'"); //Delete
            Sql(
                "UPDATE AclProjects SET PermissionID = '7CCB0EC2-006D-4345-895E-5DD2C6C8C791' WHERE PermissionID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791'"); //View
            Sql(
                "UPDATE AclProjects SET PermissionID = '768E7007-E95F-435C-8FAF-0B9FBC9CA997' WHERE PermissionID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997'"); //Manage Security


            //Request Shared Folders
            Sql(
"INSERT INTO Permissions (ID, Name, Description) VALUES ('5B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 'Edit Organization', 'Allows the user to edit an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('5B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('5B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('5C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 'Delete Organization', 'Allows the user to delete an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('5C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('5C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 3)");


            Sql(
                "INSERT INTO Permissions (ID, Name, Description) VALUES ('5CCB0EC2-006D-4345-895E-5DD2C6C8C791', 'View Organization', 'Allows the user to view an organization')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('5CCB0EC2-006D-4345-895E-5DD2C6C8C791', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('5CCB0EC2-006D-4345-895E-5DD2C6C8C791', 3)");
            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('568E7007-E95F-435C-8FAF-0B9FBC9CA997', 'Manage Organizational Security', 'Allows the user to manage an organization''s access permissions')");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('568E7007-E95F-435C-8FAF-0B9FBC9CA997', 0)");
            Sql(
                "INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('568E7007-E95F-435C-8FAF-0B9FBC9CA997', 3)");


            Sql(
                "UPDATE AclRequestSharedFolders SET PermissionID = '5B42D2D7-F7A7-4119-9CC5-22991DC12AD3' WHERE PermissionID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3'"); //Edit
            Sql(
                "UPDATE AclRequestSharedFolders SET PermissionID = '5C019772-1B9D-48F8-9FCD-AC44BC6FD97B' WHERE PermissionID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B'"); //Delete
            Sql(
                "UPDATE AclRequestSharedFolders SET PermissionID = '5CCB0EC2-006D-4345-895E-5DD2C6C8C791' WHERE PermissionID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791'"); //View
            Sql(
                "UPDATE AclRequestSharedFolders SET PermissionID = '568E7007-E95F-435C-8FAF-0B9FBC9CA997' WHERE PermissionID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997'"); //Manage Security

            Sql(@"EXEC sp_msforeachtable ""ALTER TABLE ? ENABLE TRIGGER ALL""");
        }
        
        public override void Down()
        {
        }
    }
}
