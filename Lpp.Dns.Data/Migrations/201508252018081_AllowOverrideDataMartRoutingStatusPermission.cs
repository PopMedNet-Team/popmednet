namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowOverrideDataMartRoutingStatusPermission : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Permissions (ID, Name, Description) VALUES ('7A401F1F-46C2-4F6F-9FAE-AE94A6DDB21F', 'Override DataMart Routing Status', 'Allows the user to modify the status of a Submitted, incomplete DataMart routing.')");

            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7A401F1F-46C2-4F6F-9FAE-AE94A6DDB21F', 1)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7A401F1F-46C2-4F6F-9FAE-AE94A6DDB21F', 4)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7A401F1F-46C2-4F6F-9FAE-AE94A6DDB21F', 11)");
        }
        
        public override void Down()
        {
            Sql(@"Delete from PermissionLocations Where PermissionID '7A401F1F-46C2-4F6F-9FAE-AE94A6DDB21F'");
            Sql(@"Delete from Permissions Where ID = '7A401F1F-46C2-4F6F-9FAE-AE94A6DDB21F'");
        }
    }
}
