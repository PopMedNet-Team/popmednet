namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDataCheckerWFRejectCompletionMap : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE WorkflowActivityCompletionMaps SET DestinationWorkflowActivityID = '11383C00-C270-4A46-97D2-5B1AC527B7F8' WHERE WorkflowActivityResultID = 'EA120001-7A35-4829-9F2D-A3B600E25013' AND SourceWorkflowActivityID = '3FFBCA99-5801-4045-9FB4-072136A845FC' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");
        }
        
        public override void Down()
        {
            Sql(@"UPDATE WorkflowActivityCompletionMaps SET DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' WHERE WorkflowActivityResultID = 'EA120001-7A35-4829-9F2D-A3B600E25013' AND SourceWorkflowActivityID = '3FFBCA99-5801-4045-9FB4-072136A845FC' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");
        }
    }
}
