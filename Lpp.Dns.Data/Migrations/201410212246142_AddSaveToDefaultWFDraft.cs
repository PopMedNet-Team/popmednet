namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSaveToDefaultWFDraft : DbMigration
    {
        public override void Up()
        {
            Sql(@"-- Create Request Save result on Default WF
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','C1380001-4524-49BA-B4B6-A3B5013A3343','C1380001-4524-49BA-B4B6-A3B5013A3343')");
        }
        
        public override void Down()
        {
            Sql(@"DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' and SourceWorkflowActivityID = 'C1380001-4524-49BA-B4B6-A3B5013A3343' and DestinationWorkflowActivityID = 'C1380001-4524-49BA-B4B6-A3B5013A3343'
DELETE WorkflowActivityResults WHERE ID in ('DFF3000B-B076-4D07-8D83-05EDE3636F4D')
");
        }
    }
}
