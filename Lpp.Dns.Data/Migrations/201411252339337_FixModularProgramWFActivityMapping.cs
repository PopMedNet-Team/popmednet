namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixModularProgramWFActivityMapping : DbMigration
    {
        public override void Up()
        {
            /**
            * Mapping change for Modular Program Workflow:
            * Change the submit destination activity for Pre-Distribution Testing from Review Specifications to Review Pre-Distribution Testing
            * */
            Sql(@"UPDATE WorkflowActivityCompletionMaps
SET DestinationWorkflowActivityID = 'EA69E5ED-6029-47E8-9B45-F0F00B07FDC2'
WHERE WorkflowActivityResultID = '8D035265-44EF-40AE-A1CD-30C9EF9871DB' 
AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'
AND SourceWorkflowActivityID = '49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7'
AND DestinationWorkflowActivityID = '948B60F0-8CE5-4B14-9AD6-C50EC37DFC77'");
        }
        
        public override void Down()
        {
            Sql(@"UPDATE WorkflowActivityCompletionMaps
SET DestinationWorkflowActivityID = '948B60F0-8CE5-4B14-9AD6-C50EC37DFC77'
WHERE WorkflowActivityResultID = '8D035265-44EF-40AE-A1CD-30C9EF9871DB' 
AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'
AND SourceWorkflowActivityID = '49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7'
AND DestinationWorkflowActivityID = 'EA69E5ED-6029-47E8-9B45-F0F00B07FDC2'");
        }
    }
}
