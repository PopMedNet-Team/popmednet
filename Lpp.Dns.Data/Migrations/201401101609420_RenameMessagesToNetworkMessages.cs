namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameMessagesToNetworkMessages : DbMigration
    {
        public override void Up()
        {
            Sql(@"EXEC sp_rename 'dbo.Messages', 'NetworkMessages'");
        }
        
        public override void Down()
        {
            Sql(@"EXEC sp_rename 'dbo.NetworkMessages', 'Messages'");
        }
    }
}
