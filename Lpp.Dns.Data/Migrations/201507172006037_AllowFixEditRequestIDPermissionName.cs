namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowFixEditRequestIDPermissionName : DbMigration
    {
        public override void Up()
        {
            Sql(@"Update Permissions Set Name = 'Edit Request ID', [Description] = 'Allows the user to edit the request ID.' Where ID = '43BF0001-4735-4598-BBAD-A4D801478AAA'");
        }
        
        public override void Down()
        {
        }
    }
}
