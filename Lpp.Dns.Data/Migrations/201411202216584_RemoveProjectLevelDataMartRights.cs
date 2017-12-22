namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveProjectLevelDataMartRights : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM AclProjects WHERE PermissionID = '6B42D2D7-F7A7-4119-9CC5-22991DC12AD3'");
            Sql("DELETE FROM AclProjects WHERE PermissionID = '6C019772-1B9D-48F8-9FCD-AC44BC6FD97B'");
            Sql("DELETE FROM AclProjects WHERE PermissionID = '668E7007-E95F-435C-8FAF-0B9FBC9CA997'");
            Sql("DELETE FROM AclProjects WHERE PermissionID = 'F487C17A-873B-489B-A0AC-92EC07976D4A'");
            Sql("DELETE FROM AclProjects WHERE PermissionID = '7710B3EA-B91E-4C85-978F-6BFCDE8C817C'");
            Sql("DELETE FROM AclProjects WHERE PermissionID = 'D4770F67-7DB5-4D47-9413-CA1C777179C9'");
            Sql("DELETE FROM AclProjects WHERE PermissionID = 'BB640001-5BA7-4658-93AF-A2B201579BFA'");
            Sql("DELETE FROM AclProjects WHERE PermissionID = 'EFC6DA52-1625-4209-9BBA-5C4BF1D38188'");

            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = '6B42D2D7-F7A7-4119-9CC5-22991DC12AD3' AND Type = 4");
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = '6C019772-1B9D-48F8-9FCD-AC44BC6FD97B' AND Type = 4");
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = '668E7007-E95F-435C-8FAF-0B9FBC9CA997' AND Type = 4");
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = 'F487C17A-873B-489B-A0AC-92EC07976D4A' AND Type = 4");
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = '7710B3EA-B91E-4C85-978F-6BFCDE8C817C' AND Type = 4");
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = 'D4770F67-7DB5-4D47-9413-CA1C777179C9' AND Type = 4");
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = 'BB640001-5BA7-4658-93AF-A2B201579BFA' AND Type = 4");
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = 'EFC6DA52-1625-4209-9BBA-5C4BF1D38188' AND Type = 4");
        }
        
        public override void Down()
        {
        }
    }
}
