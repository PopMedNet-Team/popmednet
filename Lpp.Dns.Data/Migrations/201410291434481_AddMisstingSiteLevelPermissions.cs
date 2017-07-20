namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMisstingSiteLevelPermissions : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('AF37A115-9D40-4F38-8BAF-4B050AC6F185', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('135F153D-D0BE-4D51-B55C-4B8807E74584', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('F9870001-7C06-4B4B-8F76-A2A701102FF0', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('8C5E44DC-284E-45D8-A014-A0CD815883AE', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('25BD0001-4739-41D8-BC74-A2AF01733B64', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('4A7C9495-BB01-4EA7-9419-65ACE6B24865', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('92687123-6F38-400E-97EC-C837AA92305F', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('22FB4F13-0492-417F-ACA1-A1338F705748', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('FDE2D32E-A045-4062-9969-00962E182367', 0)");
        }
        
        public override void Down()
        {
        }
    }
}
