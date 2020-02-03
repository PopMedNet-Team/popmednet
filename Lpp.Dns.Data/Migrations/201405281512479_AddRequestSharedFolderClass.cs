namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestSharedFolderClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestSharedFolders", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));            
        }
        
        public override void Down()
        {
            DropColumn("dbo.NetworkMessages", "Timestamp");
        }
    }
}
