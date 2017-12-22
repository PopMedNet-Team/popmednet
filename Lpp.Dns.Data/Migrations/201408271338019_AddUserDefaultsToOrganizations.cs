namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserDefaultsToOrganizations : DbMigration
    {
        public override void Up()
        {
            //Edit
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 3)");

            //Delete
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 3)");

            //View
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2CCB0EC2-006D-4345-895E-5DD2C6C8C791', 3)");

            //Manage Security
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('268E7007-E95F-435C-8FAF-0B9FBC9CA997', 3)");

            //Change Password
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('4A7C9495-BB01-4EA7-9419-65ACE6B24865', 3)");

            //Change Login
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('92687123-6F38-400E-97EC-C837AA92305F', 3)");

            //Manage Notifications
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('22FB4F13-0492-417F-ACA1-A1338F705748', 3)");

            //Change Certificate
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('FDE2D32E-A045-4062-9969-00962E182367', 3)");
        }
        
        public override void Down()
        {
        }
    }
}
