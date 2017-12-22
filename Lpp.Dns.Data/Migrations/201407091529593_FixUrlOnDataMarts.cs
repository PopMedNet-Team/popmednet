namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUrlOnDataMarts : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DataMarts", "Url", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DataMarts", "Url", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
