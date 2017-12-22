namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveGroupRightsFromOrganization : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = '3C019772-1B9D-48F8-9FCD-AC44BC6FD97B' AND Type = 3");
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = '3B42D2D7-F7A7-4119-9CC5-22991DC12AD3' AND Type = 3");
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = '3CCB0EC2-006D-4345-895E-5DD2C6C8C791' AND Type = 3");
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = '368E7007-E95F-435C-8FAF-0B9FBC9CA997' AND Type = 3");

            Sql(@"DELETE FROM AclOrganizations WHERE PermissionID = '3C019772-1B9D-48F8-9FCD-AC44BC6FD97B' OR PermissionID = '3B42D2D7-F7A7-4119-9CC5-22991DC12AD3' OR PermissionID = '3CCB0EC2-006D-4345-895E-5DD2C6C8C791' OR PermissionID = '368E7007-E95F-435C-8FAF-0B9FBC9CA997'");
        }
        
        public override void Down()
        {
        }
    }
}
