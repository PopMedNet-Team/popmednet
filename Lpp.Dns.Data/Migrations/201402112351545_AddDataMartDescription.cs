namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataMartDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "DataMartDescription", c => c.String(maxLength: 440));
            CreateIndex("dbo.DataMarts", "DataMartDescription");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DataMarts", new string[] { "DataMartDescription" });
            DropColumn("dbo.DataMarts", "DataMartDescription");
        }
    }
}
