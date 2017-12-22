namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLoginPermission : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM Permissions WHERE ID = '5FBA8EF3-F9A3-4ACC-A3D0-09905FA16E8E'");
        }
        
        public override void Down()
        {
        }
    }
}
