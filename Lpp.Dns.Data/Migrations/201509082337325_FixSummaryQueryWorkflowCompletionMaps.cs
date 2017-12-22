namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSummaryQueryWorkflowCompletionMaps : DbMigration
    {
        public override void Up()
        {
            Sql("If Exists (select Null from WorkflowActivityCompletionMaps Where SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' And DestinationWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' And WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') Begin Update WorkflowActivityCompletionMaps Set WorkflowActivityResultID = 'E1C90001-B582-4180-9A71-A3B600EA0C27' Where SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' And DestinationWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' And WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' End");

            Sql("If Exists (select Null from WorkflowActivityCompletionMaps Where WorkflowActivityResultID = '22AE0001-0B5A-4BA9-BB55-A3B600E2728C' And WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') Begin Update WorkflowActivityCompletionMaps Set DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' Where WorkflowActivityResultID = '22AE0001-0B5A-4BA9-BB55-A3B600E2728C' And WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' End");

            Sql("If Exists (select Null from WorkflowActivityCompletionMaps Where WorkflowActivityResultID = '5E010001-1353-44E9-9204-A3B600E263E9' And WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') Begin Update WorkflowActivityCompletionMaps Set DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' Where WorkflowActivityResultID = '5E010001-1353-44E9-9204-A3B600E263E9' And WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' End");

            Sql("If Exists (select Null from WorkflowActivityCompletionMaps Where WorkflowActivityResultID = '15BDEF13-6E86-4E0F-8790-C07AE5B798A8' And WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') Begin Update WorkflowActivityCompletionMaps Set DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' Where WorkflowActivityResultID = '15BDEF13-6E86-4E0F-8790-C07AE5B798A8' And WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' End");
        }
        
        public override void Down()
        {
        }
    }
}
