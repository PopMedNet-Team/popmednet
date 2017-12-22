namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataCheckerWorkflowActivityResults : DbMigration
    {
        public override void Up()
        {
            //Delete the previously created activities - The wrong response activity was cloned.
            Sql("Delete from WorkflowActivityResults where ID IN (Select WorkflowactivityResultID from WorkflowActivityCompletionMaps Where WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
            Sql("Delete from WorkflowActivityCompletionMaps Where WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");

            //Re-create the workflow activities with the new View Response Detail
            Sql(@"-- delete a request that has not been submitted: New Request => Terminate Request
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('61110001-1708-4869-BDCF-A3B600E24AA3','11383C00-C270-4A46-97D2-5B1AC527B7F8','CC2E0001-9B99-4C67-8DED-A3B600E1C696', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- submit a request for approval: New Request => View Status and Results
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('48B20001-BD0B-425D-8D49-A3B5015A2258','11383C00-C270-4A46-97D2-5B1AC527B7F8','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- reject request: Request Review => Terminate Request
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('EA120001-7A35-4829-9F2D-A3B600E25013','3FFBCA99-5801-4045-9FB4-072136A845FC','CC2E0001-9B99-4C67-8DED-A3B600E1C696', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')
                        -- approve request: Request Review => View Status and Results
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('50C60001-891F-40E6-B95F-A3B600E25C2B','3FFBCA99-5801-4045-9FB4-072136A845FC','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- resubmit: View Status and Results => View Status and Results
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('22AE0001-0B5A-4BA9-BB55-A3B600E2728C','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- view response: View Status and Results => View Response
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('1C1D0001-65F4-4E02-9BB7-A3B600E27A2F','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ECC689B1-C170-43BA-A181-F2762068F8FB', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- upload response: View Response => View Status and Results
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('FEB90001-19C4-48DB-A8A4-A3B600EE60C7','ECC689B1-C170-43BA-A181-F2762068F8FB','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- reject response: View Response/Review Results => View Status and Results
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('F1B10001-B0B3-45A9-AAFF-A3B600EEFC49','ECC689B1-C170-43BA-A181-F2762068F8FB','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('F1B10001-B0B3-45A9-AAFF-A3B600EEFC49','1D0D4993-EA87-4C0D-9226-43F8BB83C952','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- approve response: View Response/Review Results => View Status and Results
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('0FEE0001-ED08-48D8-8C0B-A3B600EEF30F','ECC689B1-C170-43BA-A181-F2762068F8FB','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('0FEE0001-ED08-48D8-8C0B-A3B600EEF30F','1D0D4993-EA87-4C0D-9226-43F8BB83C952','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- add datamarts: View Status and Results => View Response
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('15BDEF13-6E86-4E0F-8790-C07AE5B798A8','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- complete: View Status and Results => terminate
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('E1C90001-B582-4180-9A71-A3B600EA0C27','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','CC2E0001-9B99-4C67-8DED-A3B600E1C696', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- group: View Status and Results => View Status and Results
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('49F9C682-9FAD-4AE5-A2C5-19157E227186','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- remove datamarts: View Status and Results => View Status and Results
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('5E010001-1353-44E9-9204-A3B600E263E9','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- save: New Request => New Request
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','11383C00-C270-4A46-97D2-5B1AC527B7F8','11383C00-C270-4A46-97D2-5B1AC527B7F8', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- submit for approval: New Request => Request Review
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('C4FB25F8-8521-427E-8FB1-78A84311BF1C','11383C00-C270-4A46-97D2-5B1AC527B7F8','3FFBCA99-5801-4045-9FB4-072136A845FC', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')

                        -- ungroup: View Status and Results => View Status and Results
                        INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('7821FC45-9FD5-4597-A405-B021E5ED14FA','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
        }
        
        public override void Down()
        {
            Sql("Delete from WorkflowActivityCompletionMaps Where WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");
        }
    }
}
