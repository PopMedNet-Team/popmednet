namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDataCheckerWorkflow : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Workflows (ID, Name, Description) VALUES ('942A2B39-0E9C-4ECE-9E2C-C85DF0F42663', 'Data Checker', 'Workflow for Data Checker Queries.')");
        }
        
        public override void Down()
        {
            Sql("Delete from Workflows where ID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");
        }
    }
}
