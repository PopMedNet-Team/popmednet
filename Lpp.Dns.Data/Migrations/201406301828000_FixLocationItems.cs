namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixLocationItems : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM Permissions WHERE ID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3'"); //Generic Delete no longer used
            Sql("DELETE FROM Permissions WHERE ID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B'"); //Generic Edit no longer used.


            //Group permissions not setup right
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 2)");

            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 2)");

            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3CCB0EC2-006D-4345-895E-5DD2C6C8C791', 2)");

            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('368E7007-E95F-435C-8FAF-0B9FBC9CA997', 2)");

            //Registry not setup right
            //Group permissions not setup right
            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('2B42D2E7-F7A7-4119-9CC5-22991DC12AD3', 'Edit Registry', 'Allows the selected seucrity group to edit the selected registry.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2B42D2E7-F7A7-4119-9CC5-22991DC12AD3', 5)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2B42D2E7-F7A7-4119-9CC5-22991DC12AD3', 0)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('2C019782-1B9D-48F8-9FCD-AC44BC6FD97B', 'Delete Registry', 'Allows the selected seucrity group to delete the selected registry.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2C019782-1B9D-48F8-9FCD-AC44BC6FD97B', 5)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2C019782-1B9D-48F8-9FCD-AC44BC6FD97B', 0)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('2CCB0FC2-006D-4345-895E-5DD2C6C8C791', 'View Registry', 'Allows the selected seucrity group to view the selected registry.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2CCB0FC2-006D-4345-895E-5DD2C6C8C791', 5)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2CCB0FC2-006D-4345-895E-5DD2C6C8C791', 0)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('268F7007-E95F-435C-8FAF-0B9FBC9CA997', 'Manage Registry Security', 'Allows the selected seucrity group to manage security of the selected registry.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('268F7007-E95F-435C-8FAF-0B9FBC9CA997', 5)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('268F7007-E95F-435C-8FAF-0B9FBC9CA997', 0)");
        }
        
        public override void Down()
        {
        }
    }
}
