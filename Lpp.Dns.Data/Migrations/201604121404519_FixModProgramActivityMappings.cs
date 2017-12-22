namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixModProgramActivityMappings : DbMigration
    {
        public override void Up()
        {

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '668EE9C7-4930-423E-AA9E-150B646121F4' AND SourceWorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WORKFLOWID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'D0A0924F-F4B5-43BF-89A6-C7F32E764735' AND SourceWorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WORKFLOWID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

        }
        
        public override void Down()
        {

            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
VALUES ('668EE9C7-4930-423E-AA9E-150B646121F4','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
VALUES ('D0A0924F-F4B5-43BF-89A6-C7F32E764735','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

        }
    }
}
