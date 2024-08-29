namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssignRequestLevelNotificationsPermission : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Permissions (ID, Name, Description) VALUES ('3EB92A0A-5A9B-4860-898D-E32ACC2D5EEA', 'Assign Request-Level Notifications', 'Allows the user to assign users to receive request notifications.')");

            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3EB92A0A-5A9B-4860-898D-E32ACC2D5EEA', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3EB92A0A-5A9B-4860-898D-E32ACC2D5EEA', 4)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('3EB92A0A-5A9B-4860-898D-E32ACC2D5EEA', 3)");
        }
        
        public override void Down()
        {
            Sql(@"Delete from PermissionLocations Where PermissionID '3EB92A0A-5A9B-4860-898D-E32ACC2D5EEA'");
            Sql(@"Delete from Permissions Where ID = '3EB92A0A-5A9B-4860-898D-E32ACC2D5EEA'");
        }
    }
}
