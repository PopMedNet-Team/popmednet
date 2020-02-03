namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupingWFactivityforMPWF : DbMigration
    {
        public override void Up()
        {
            Sql(@"-- group: View Status and Results => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '49F9C682-9FAD-4AE5-A2C5-19157E227186' AND SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND DestinationWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('49F9C682-9FAD-4AE5-A2C5-19157E227186','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");
            Sql(@"-- ungroup: View Status and Results => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '7821FC45-9FD5-4597-A405-B021E5ED14FA' AND SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND DestinationWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('7821FC45-9FD5-4597-A405-B021E5ED14FA','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");
            Sql(@"-- group: View Status and Results => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '49F9C682-9FAD-4AE5-A2C5-19157E227186' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('49F9C682-9FAD-4AE5-A2C5-19157E227186','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");
            Sql(@"-- ungroup: View Status and Results => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '7821FC45-9FD5-4597-A405-B021E5ED14FA' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('7821FC45-9FD5-4597-A405-B021E5ED14FA','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM WorkflowActivityCompletionMaps Where WorkflowActivityResultID='49F9C682-9FAD-4AE5-A2C5-19157E227186' AND WorkflowID='5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D' ");
            Sql(@"DELETE FROM WorkflowActivityCompletionMaps Where WorkflowActivityResultID='7821FC45-9FD5-4597-A405-B021E5ED14FA' AND WorkflowID='5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D' ");
        }
    }
}
