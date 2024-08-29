namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDatamartAclsToOrganizationsandProjects : DbMigration
    {
        public override void Up()
        {
            //Edit
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 3)");

            //Delete
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 3)");

            //View
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6CCB0EC2-006D-4345-895E-5DD2C6C8C791', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6CCB0EC2-006D-4345-895E-5DD2C6C8C791', 3)");

            //Manage Security
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('668E7007-E95F-435C-8FAF-0B9FBC9CA997', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('668E7007-E95F-435C-8FAF-0B9FBC9CA997', 3)");

            //Request Metadata Update
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('F487C17A-873B-489B-A0AC-92EC07976D4A', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('F487C17A-873B-489B-A0AC-92EC07976D4A', 3)");

            //Install Models
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7710B3EA-B91E-4C85-978F-6BFCDE8C817C', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7710B3EA-B91E-4C85-978F-6BFCDE8C817C', 3)");

            //Un-Install Models
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('D4770F67-7DB5-4D47-9413-CA1C777179C9', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('D4770F67-7DB5-4D47-9413-CA1C777179C9', 3)");

            //Run Audit Report
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('EFC6DA52-1625-4209-9BBA-5C4BF1D38188', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('EFC6DA52-1625-4209-9BBA-5C4BF1D38188', 3)");

            //Copy
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('BB640001-5BA7-4658-93AF-A2B201579BFA', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('BB640001-5BA7-4658-93AF-A2B201579BFA', 3)");
        }
        
        public override void Down()
        {
        }
    }
}
