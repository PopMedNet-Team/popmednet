namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestDataMartPriorityDueDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestDataMarts", "Priority", c => c.Byte(nullable: false, defaultValue:0));
            AddColumn("dbo.RequestDataMarts", "DueDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestDataMarts", "DueDate");
            DropColumn("dbo.RequestDataMarts", "Priority");
        }
    }
}
