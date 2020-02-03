namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestPermissionLocations : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('8B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 20)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('8C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 20)");
        }
        
        public override void Down()
        {
        }
    }
}
