namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProcessorIDToDataMart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "ProcessorID", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataMarts", "ProcessorID");
        }
    }
}
