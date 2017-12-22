namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingSiteWideRights : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('ECF3B864-7DB3-497B-A2E4-F2B435EF2803', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('64A00001-A1D6-41DD-AB20-A2B200EEB9A3', 0)");
        }
        
        public override void Down()
        {
        }
    }
}
