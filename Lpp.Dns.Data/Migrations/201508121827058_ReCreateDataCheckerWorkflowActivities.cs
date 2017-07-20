namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReCreateDataCheckerWorkflowActivities : DbMigration
    {
        public override void Up()
        {
            //Delete the previously created activities - The wrong response activity was cloned.
            Sql("Delete from WorkflowActivityResults where ID IN (Select WorkflowactivityResultID from WorkflowActivityCompletionMaps Where WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
            Sql("Delete from WorkflowActivityCompletionMaps Where WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");
            Sql("Delete from WorkflowActivities where ID = '11383C00-C270-4A46-97D2-5B1AC527B7F8'");
            Sql("Delete from WorkflowActivities where ID = '3FFBCA99-5801-4045-9FB4-072136A845FC'");
            Sql("Delete from WorkflowActivities where ID = '1D0D4993-EA87-4C0D-9226-43F8BB83C952'");
            Sql("Delete from WorkflowActivities where ID = 'B4ECB8DA-3140-428D-B5B5-F3A3A1A7B35E'");

            //Re-create the workflow activities:
            Sql(@"INSERT INTO WorkflowActivities (ID, Name, [Description], [Start]) VALUES ('11383C00-C270-4A46-97D2-5B1AC527B7F8', 'New Request', '', 1)
INSERT INTO WorkflowActivities (ID, Name, [Description]) VALUES ('3FFBCA99-5801-4045-9FB4-072136A845FC', 'Request Review', '')
INSERT INTO WorkflowActivities (ID, Name, [Description]) VALUES ('1D0D4993-EA87-4C0D-9226-43F8BB83C952', 'Results Review', '')
INSERT INTO WorkflowActivities (ID, Name, [Description]) VALUES ('8117F667-C888-425B-A431-F5599F7A5599', 'View Status and Results', '')

                    -- delete a request that has not been submitted: New Request => Terminate Request
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '0890D98E-81CA-4283-843D-7F6C151451C4') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('0890D98E-81CA-4283-843D-7F6C151451C4', 'Delete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('0890D98E-81CA-4283-843D-7F6C151451C4','11383C00-C270-4A46-97D2-5B1AC527B7F8','CC2E0001-9B99-4C67-8DED-A3B600E1C696', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- submit a request for approval: New Request => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '5B266B1C-95EE-44D3-B3B8-0147A58134ED') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('5B266B1C-95EE-44D3-B3B8-0147A58134ED', 'Submit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('5B266B1C-95EE-44D3-B3B8-0147A58134ED','11383C00-C270-4A46-97D2-5B1AC527B7F8','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- review a request and either approve or reject
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '4DB7427E-5AFF-4F6B-8FAE-7BBC83168CF3') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('4DB7427E-5AFF-4F6B-8FAE-7BBC83168CF3', 'Reject', '')
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = 'EDC1F67F-2930-47C1-A1B0-DFD4D5BC2183') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('EDC1F67F-2930-47C1-A1B0-DFD4D5BC2183', 'Approve', '')
-- reject request: Request Review => Terminate Request
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('4DB7427E-5AFF-4F6B-8FAE-7BBC83168CF3','3FFBCA99-5801-4045-9FB4-072136A845FC','CC2E0001-9B99-4C67-8DED-A3B600E1C696', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')
-- approve request: Request Review => View Status and Results
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('EDC1F67F-2930-47C1-A1B0-DFD4D5BC2183','3FFBCA99-5801-4045-9FB4-072136A845FC','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- request autocompletes: View Status and Results => Terminate Request
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '4BE63635-F48E-4086-A423-3CFC5ADF44A5') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('4BE63635-F48E-4086-A423-3CFC5ADF44A5', 'Auto Complete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('4BE63635-F48E-4086-A423-3CFC5ADF44A5','8117F667-C888-425B-A431-F5599F7A5599','CC2E0001-9B99-4C67-8DED-A3B600E1C696', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- cancel response: View Status and Results => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = 'F9ADA96B-8F72-4EED-A1D6-4247C0E0CD18') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('F9ADA96B-8F72-4EED-A1D6-4247C0E0CD18', 'Cancel', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('F9ADA96B-8F72-4EED-A1D6-4247C0E0CD18','8117F667-C888-425B-A431-F5599F7A5599','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')
-- resubmit: View Status and Results => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '0A1EFB70-20C1-4A15-B71B-6319A24AA6F4') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('0A1EFB70-20C1-4A15-B71B-6319A24AA6F4', 'Resubmit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('0A1EFB70-20C1-4A15-B71B-6319A24AA6F4','8117F667-C888-425B-A431-F5599F7A5599','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- view response: View Status and Results => View Response
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '2DFB0508-B6CD-42D0-90F8-D992C0455137') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('2DFB0508-B6CD-42D0-90F8-D992C0455137', 'View Response', '')
IF EXISTS(SELECT NULL FROM WorkflowActivities WHERE ID = '675F0001-6B44-4910-AD89-A3B600E98CE9') INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('2DFB0508-B6CD-42D0-90F8-D992C0455137','8117F667-C888-425B-A431-F5599F7A5599','675F0001-6B44-4910-AD89-A3B600E98CE9', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- upload response: View Response => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '8FBF25F0-8336-4944-B348-BFC53D28AC57') INSERT INTO WorkflowActivityResults(ID, Name, [Description]) VALUES ('8FBF25F0-8336-4944-B348-BFC53D28AC57', 'Upload Response', 'Upload response.')
IF EXISTS(SELECT NULL FROM WorkflowActivities WHERE ID = '675F0001-6B44-4910-AD89-A3B600E98CE9') INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('8FBF25F0-8336-4944-B348-BFC53D28AC57','675F0001-6B44-4910-AD89-A3B600E98CE9','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- reject response: View Response/Review Results => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '5D6B698F-A6EE-41D3-88E0-863CA32FAB1C') INSERT INTO WorkflowActivityResults(ID, Name, [Description]) VALUES ('5D6B698F-A6EE-41D3-88E0-863CA32FAB1C', 'Reject Response', 'Reject response.')
IF EXISTS(SELECT NULL FROM WorkflowActivities WHERE ID = '675F0001-6B44-4910-AD89-A3B600E98CE9') INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('5D6B698F-A6EE-41D3-88E0-863CA32FAB1C','675F0001-6B44-4910-AD89-A3B600E98CE9','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('5D6B698F-A6EE-41D3-88E0-863CA32FAB1C','1D0D4993-EA87-4C0D-9226-43F8BB83C952','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- approve response: View Response/Review Results => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '36DF6EA4-6397-4F79-9000-ED4D85D8B075') INSERT INTO WorkflowActivityResults(ID, Name, [Description]) VALUES ('36DF6EA4-6397-4F79-9000-ED4D85D8B075', 'Approve Response', 'Approve response.')
IF EXISTS(SELECT NULL FROM WorkflowActivities WHERE ID = '675F0001-6B44-4910-AD89-A3B600E98CE9') INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('36DF6EA4-6397-4F79-9000-ED4D85D8B075','675F0001-6B44-4910-AD89-A3B600E98CE9','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('36DF6EA4-6397-4F79-9000-ED4D85D8B075','1D0D4993-EA87-4C0D-9226-43F8BB83C952','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- add datamarts: View Status and Results => View Response
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = 'E8C1A082-6754-4BD9-9F8A-38FC1346903F') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('E8C1A082-6754-4BD9-9F8A-38FC1346903F', 'Add DataMarts', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('E8C1A082-6754-4BD9-9F8A-38FC1346903F','8117F667-C888-425B-A431-F5599F7A5599','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- complete: View Status and Results => terminate
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '9BB41FEC-D934-4558-918B-0DAE331FDF93') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('9BB41FEC-D934-4558-918B-0DAE331FDF93', 'Complete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('9BB41FEC-D934-4558-918B-0DAE331FDF93','8117F667-C888-425B-A431-F5599F7A5599','CC2E0001-9B99-4C67-8DED-A3B600E1C696', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- group: View Status and Results => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = 'EA1E10E4-3598-4F54-9271-4838785CDD78') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('EA1E10E4-3598-4F54-9271-4838785CDD78', 'Group', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('EA1E10E4-3598-4F54-9271-4838785CDD78','8117F667-C888-425B-A431-F5599F7A5599','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- remove datamarts: View Status and Results => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '39B402CF-DA37-4E9B-9D6C-A6590745E42A') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('39B402CF-DA37-4E9B-9D6C-A6590745E42A', 'Remove DataMarts', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('39B402CF-DA37-4E9B-9D6C-A6590745E42A','8117F667-C888-425B-A431-F5599F7A5599','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- save: New Request => New Request
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '8A468296-549E-4F0E-8C83-B01B116EEAFE') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('8A468296-549E-4F0E-8C83-B01B116EEAFE', 'Save', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('8A468296-549E-4F0E-8C83-B01B116EEAFE','11383C00-C270-4A46-97D2-5B1AC527B7F8','11383C00-C270-4A46-97D2-5B1AC527B7F8', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- submit for approval: New Request => Request Review
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = '4B007D82-0C04-48EC-8B82-31A2D4BB1DC3') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('4B007D82-0C04-48EC-8B82-31A2D4BB1DC3', 'Submit for Approval', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('4B007D82-0C04-48EC-8B82-31A2D4BB1DC3','11383C00-C270-4A46-97D2-5B1AC527B7F8','3FFBCA99-5801-4045-9FB4-072136A845FC', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

-- ungroup: View Status and Results => View Status and Results
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityResults WHERE ID = 'A76CE9E8-ED1D-4826-8530-6B646F06ABFE') INSERT INTO WorkflowActivityResults (ID, Name, [Description]) VALUES ('A76CE9E8-ED1D-4826-8530-6B646F06ABFE', 'Ungroup', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('A76CE9E8-ED1D-4826-8530-6B646F06ABFE','8117F667-C888-425B-A431-F5599F7A5599','8117F667-C888-425B-A431-F5599F7A5599', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
        }
        
        public override void Down()
        {
            Sql("Delete from WorkflowActivityResults where ID IN (Select WorkflowactivityResultID from WorkflowActivityCompletionMaps Where WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
            Sql("Delete from WorkflowActivityCompletionMaps Where WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");
            Sql("Delete from WorkflowActivities where ID = '11383C00-C270-4A46-97D2-5B1AC527B7F8'");
            Sql("Delete from WorkflowActivities where ID = '3FFBCA99-5801-4045-9FB4-072136A845FC'");
            Sql("Delete from WorkflowActivities where ID = '1D0D4993-EA87-4C0D-9226-43F8BB83C952'");
            Sql("Delete from WorkflowActivities where ID = '8117F667-C888-425B-A431-F5599F7A5599'");
        }
    }
}
