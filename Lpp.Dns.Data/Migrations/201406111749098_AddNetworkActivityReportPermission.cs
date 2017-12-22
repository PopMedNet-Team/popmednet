namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNetworkActivityReportPermission : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('BA7687E8-E149-4772-8F3F-7C8568769998', 'Run Network Activity Report', 'Allows the selected user to run the Network Activity Report across the entire system')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('BA7687E8-E149-4772-8F3F-7C8568769998', 0)");
        }
        
        public override void Down()
        {
        }
    }
}
