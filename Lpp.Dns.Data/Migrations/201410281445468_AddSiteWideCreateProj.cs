namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSiteWideCreateProj : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES('93623C60-6425-40A0-91A0-01FA34920913', 0)");
        }
        
        public override void Down()
        {
        }
    }
}
