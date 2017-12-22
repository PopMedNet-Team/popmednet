namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoutingsBulkEditWorkflowActivityResult : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('4F7E1762-E453-4D12-8037-BAE8A95523F7', 'Routings Bulk Edit', '')");
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('4F7E1762-E453-4D12-8037-BAE8A95523F7','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','F64E0001-4F9A-49F0-BF75-A3B501396946')");
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('4F7E1762-E453-4D12-8037-BAE8A95523F7','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('4F7E1762-E453-4D12-8037-BAE8A95523F7','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('4F7E1762-E453-4D12-8037-BAE8A95523F7','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '4F7E1762-E453-4D12-8037-BAE8A95523F7'");
            Sql(@"DELETE FROM WorkflowActivityResults WHERE ID = '4F7E1762-E453-4D12-8037-BAE8A95523F7'");
        }
    }
}
