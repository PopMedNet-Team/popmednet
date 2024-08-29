namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUri : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkflowActivityResults", "Uri", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkflowActivityResults", "Uri");
        }
    }
}
