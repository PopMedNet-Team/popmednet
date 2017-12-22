namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModularProgramWorkflowActivityMapping : DbMigration
    {
        public override void Up()
        {
            //on reject it review working specification should go back to submit working specifications
            Sql(@"UPDATE WorkflowActivityCompletionMaps
SET DestinationWorkflowActivityID = '31C60BB1-2F6A-423B-A7B7-B52626FD9E97'
WHERE WorkflowActivityResultID = 'A95899AC-F4F6-41AB-AD4B-D41E05563486' 
AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'
AND SourceWorkflowActivityID = 'C8891CFD-80BF-4F71-90DE-6748BF71566C'
AND DestinationWorkflowActivityID = 'C8891CFD-80BF-4F71-90DE-6748BF71566C'");
        }
        
        public override void Down()
        {
            Sql(@"UPDATE WorkflowActivityCompletionMaps
SET DestinationWorkflowActivityID = 'C8891CFD-80BF-4F71-90DE-6748BF71566C'
WHERE WorkflowActivityResultID = 'A95899AC-F4F6-41AB-AD4B-D41E05563486' 
AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'
AND SourceWorkflowActivityID = 'C8891CFD-80BF-4F71-90DE-6748BF71566C'
AND DestinationWorkflowActivityID = '31C60BB1-2F6A-423B-A7B7-B52626FD9E97'");
        }
    }
}
