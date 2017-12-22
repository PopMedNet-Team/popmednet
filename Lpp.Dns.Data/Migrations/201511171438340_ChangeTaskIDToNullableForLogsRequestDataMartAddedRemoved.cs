namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTaskIDToNullableForLogsRequestDataMartAddedRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LogsRequestDataMartAddedRemoved", "TaskID", "dbo.Tasks");
            AlterColumn("dbo.LogsRequestDataMartAddedRemoved", "TaskID", c => c.Guid(nullable: true));
            AddForeignKey("dbo.LogsRequestDatamartAddedRemoved", "TaskID", "dbo.Tasks", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsRequestDataMartAddedRemoved", "TaskID", "dbo.Tasks");
            AlterColumn("dbo.LogsRequestDataMartAddedRemoved", "TaskID", c => c.Guid(nullable: false));
            AddForeignKey("dbo.LogsRequestDatamartAddedRemoved", "TaskID", "dbo.Tasks", cascadeDelete: false);
        }
    }
}
