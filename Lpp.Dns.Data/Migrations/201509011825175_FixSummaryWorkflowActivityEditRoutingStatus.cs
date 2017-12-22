namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSummaryWorkflowActivityEditRoutingStatus : DbMigration
    {
        public override void Up()
        {

            Sql(@"--Summary Query View Responses Task needs Edit Routing Status Activity Result ID
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = '3CF0FEA0-26B9-4042-91F3-7192D44F6F7C' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('3CF0FEA0-26B9-4042-91F3-7192D44F6F7C','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");
        }
        
        public override void Down()
        {
            Sql(@" DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = '3CF0FEA0-26B9-4042-91F3-7192D44F6F7C' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");
        }
    }
}
