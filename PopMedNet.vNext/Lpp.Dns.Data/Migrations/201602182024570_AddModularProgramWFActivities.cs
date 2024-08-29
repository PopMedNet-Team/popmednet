namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModularProgramWFActivities : DbMigration
    {
        public override void Up()
        {

            Sql(@"IF NOT EXISTS (SELECT * FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '15BDEF13-6E86-4E0F-8790-C07AE5B798A8' AND SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
                INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('15BDEF13-6E86-4E0F-8790-C07AE5B798A8', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D' )");

            Sql(@"IF NOT EXISTS(SELECT * FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '5E010001-1353-44E9-9204-A3B600E263E9' AND SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
                INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('5E010001-1353-44E9-9204-A3B600E263E9', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D' ) ");

        }
        
        public override void Down()
        {

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '15BDEF13-6E86-4E0F-8790-C07AE5B798A8' AND SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '5E010001-1353-44E9-9204-A3B600E263E9' AND SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

        }
    }
}
