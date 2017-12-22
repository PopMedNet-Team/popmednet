namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixViewRoutingsActivityName : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE WorkflowActivities SET [Name] = 'View Status and Results' WHERE ID ='6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'View Response' WHERE ID ='675F0001-6B44-4910-AD89-A3B600E98CE9'");
        }
        
        public override void Down()
        {
        }
    }
}
