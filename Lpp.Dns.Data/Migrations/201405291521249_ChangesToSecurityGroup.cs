namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToSecurityGroup : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.ProjectSecurityGroups", "SID", "ID");
            AddColumn("dbo.ProjectSecurityGroups", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.SecurityGroups", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            RenameColumn("dbo.ProjectSecurityGroups", "ID", "SID");
            DropColumn("dbo.ProjectSecurityGroups", "Timestamp");
            DropColumn("dbo.SecurityGroups", "Timestamp");
        }
    }
}
