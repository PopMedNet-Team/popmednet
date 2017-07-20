namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveResponseDetailWorkflowActivity : DbMigration
    {
        public override void Up()
        {
            Guid defaultWFResponseDetailID = new Guid("675F0001-6B44-4910-AD89-A3B600E98CE9");
            Guid summaryWFResponseDetailID = new Guid("675F0001-6B44-4910-AD89-A3B600E98CE9");
            Guid datacheckerProgramWFResponseDetailID = new Guid("ECC689B1-C170-43BA-A181-F2762068F8FB");

            //update any requests that have current activity set to view responses
            Sql(string.Format(@"UPDATE Requests SET WorkFlowActivityID = CASE Requests.WorkflowID 
	WHEN '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D' THEN 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' -- modular wf
	WHEN '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' THEN '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' -- summary wf
	WHEN '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663' THEN 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' -- datachecker wf
	ELSE 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' -- default wf
END
WHERE WorkFlowActivityID IN ('{0}','{1}','{2}')", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));

            //clean up any tasks referencing the view response detail step
            Sql(string.Format("DELETE FROM LogsRequestDataMartAddedRemoved WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsRequestMetadataChange WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsRoutingStatusChange WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsTaskReminder WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsTaskChange WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsRoutingStatusChange WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsNewRequestSubmitted WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsRequestDataMartMetadataChange WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsRequestDocumentChange WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsRequestStatusChange WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsSubmittedRequestAwaitsResponse WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsSubmittedRequestNeedsApproval WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM LogsUploadedResultNeedsApproval WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));

            Sql(string.Format("DELETE FROM TaskUsers WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM TaskReferences WHERE TaskID IN (SELECT ID FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}'))", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            Sql(string.Format("DELETE FROM Tasks WHERE WorkflowActivityID IN ('{0}','{1}','{2}')", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));

            Sql(string.Format("UPDATE RequestDataMartResponses SET WorkFlowActivityID = null, WorkflowID = null WHERE WorkFlowActivityID IN ('{0}','{1}','{2}')", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));

            //clean up project requesttype workflow activity acls
            Sql(string.Format("DELETE FROM AclProjectRequestTypeWorkflowActivities WHERE WorkflowActivityID IN ('{0}','{1}','{2}')", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));

            //cleanup workflow activity security group acls
            Sql(string.Format("DELETE FROM WorkflowActivitySecurityGroups WHERE WorkflowActivityID IN ('{0}','{1}','{2}')", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));

            //cleanup completion maps
            Sql(string.Format("DELETE FROM WorkflowActivityCompletionMaps WHERE SourceWorkflowActivityID IN ('{0}','{1}','{2}') OR DestinationWorkflowActivityID IN ('{0}','{1}','{2}')", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));

            //clean up activities
            Sql(string.Format("DELETE FROM WorkflowActivities WHERE ID IN ('{0}','{1}','{2}')", defaultWFResponseDetailID, datacheckerProgramWFResponseDetailID, summaryWFResponseDetailID));
            
        }
        
        public override void Down()
        {
            //add the activities and mappings back
            Sql("INSERT INTO WorkflowActivities (ID, Name, Description, Start, [End]) VALUES ('675F0001-6B44-4910-AD89-A3B600E98CE9', 'View Response', '', 0, 0)");
            Sql("INSERT INTO WorkflowActivities (ID, Name, Description, Start, [End]) VALUES ('ECC689B1-C170-43BA-A181-F2762068F8FB', 'View Response Detail', '', 0, 0)");

            //add the activity mappings back
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('1C1D0001-65F4-4E02-9BB7-A3B600E27A2F', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '675F0001-6B44-4910-AD89-A3B600E98CE9', 'F64E0001-4F9A-49F0-BF75-A3B501396946')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D', '675F0001-6B44-4910-AD89-A3B600E98CE9', '675F0001-6B44-4910-AD89-A3B600E98CE9', 'F64E0001-4F9A-49F0-BF75-A3B501396946')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('FEB90001-19C4-48DB-A8A4-A3B600EE60C7', '675F0001-6B44-4910-AD89-A3B600E98CE9', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'F64E0001-4F9A-49F0-BF75-A3B501396946')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('0FEE0001-ED08-48D8-8C0B-A3B600EEF30F', '675F0001-6B44-4910-AD89-A3B600E98CE9', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'F64E0001-4F9A-49F0-BF75-A3B501396946')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('F1B10001-B0B3-45A9-AAFF-A3B600EEFC49', '675F0001-6B44-4910-AD89-A3B600E98CE9', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'F64E0001-4F9A-49F0-BF75-A3B501396946')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('F1B10001-B0B3-45A9-AAFF-A3B600EEFC49', '675F0001-6B44-4910-AD89-A3B600E98CE9', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('0FEE0001-ED08-48D8-8C0B-A3B600EEF30F', '675F0001-6B44-4910-AD89-A3B600E98CE9', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('FEB90001-19C4-48DB-A8A4-A3B600EE60C7', '675F0001-6B44-4910-AD89-A3B600E98CE9', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D', '675F0001-6B44-4910-AD89-A3B600E98CE9', '675F0001-6B44-4910-AD89-A3B600E98CE9', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('1C1D0001-65F4-4E02-9BB7-A3B600E27A2F', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '675F0001-6B44-4910-AD89-A3B600E98CE9', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('1C1D0001-65F4-4E02-9BB7-A3B600E27A2F', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'ECC689B1-C170-43BA-A181-F2762068F8FB', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('FEB90001-19C4-48DB-A8A4-A3B600EE60C7', 'ECC689B1-C170-43BA-A181-F2762068F8FB', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('0FEE0001-ED08-48D8-8C0B-A3B600EEF30F', 'ECC689B1-C170-43BA-A181-F2762068F8FB', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('F1B10001-B0B3-45A9-AAFF-A3B600EEFC49', 'ECC689B1-C170-43BA-A181-F2762068F8FB', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
        }
    }
}
