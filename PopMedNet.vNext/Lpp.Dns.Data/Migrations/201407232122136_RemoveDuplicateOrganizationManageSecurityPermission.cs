namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDuplicateOrganizationManageSecurityPermission : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM Permissions WHERE ID = '768E7007-E95F-435C-8FAF-0B9FBC9CA997'");
        }
        
        public override void Down()
        {
        }
    }
}
