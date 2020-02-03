namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveESPPluginsNotUsedAnymore : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE FROM RequestTypes WHERE ID = '6A900001-CFC3-439C-9978-A22200FC5253'");
            Sql(@"DELETE FROM RequestTypes WHERE ID = '84CF0001-475C-46AB-998B-A22200FC6439'");
        }
        
        public override void Down()
        {
        }
    }
}
