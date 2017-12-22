namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkflowActivtySaveIDs : DbMigration
    {
        public override void Up()
        {

            Sql(@"--Add Save Request IDs to certain steps in the WF that did not previously have it
                --Modular Program
                -- Distribute Request
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'E6CCD61B-81C4-4217-A958-ADAFB5EE5554' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','E6CCD61B-81C4-4217-A958-ADAFB5EE5554','E6CCD61B-81C4-4217-A958-ADAFB5EE5554','5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"-- PreDistribution Testing
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'EA69E5ED-6029-47E8-9B45-F0F00B07FDC2' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','EA69E5ED-6029-47E8-9B45-F0F00B07FDC2','EA69E5ED-6029-47E8-9B45-F0F00B07FDC2','5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"-- Prepare/Submit Draft Report
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','9173A8E7-27C4-469D-853D-69A78501A522','9173A8E7-27C4-469D-853D-69A78501A522','5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"-- Prepare/Submit Final Report
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','F888C5D6-B8EB-417C-9DE2-4A96D75F3208','F888C5D6-B8EB-417C-9DE2-4A96D75F3208','5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"-- Review Draft Report
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81','2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81','5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"--Review Final Report
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '2E7A3263-C87E-47BA-AC35-A78ABF8FE6061' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','2E7A3263-C87E-47BA-AC35-A78ABF8FE606','2E7A3263-C87E-47BA-AC35-A78ABF8FE606','5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"--Specification Review
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '948B60F0-8CE5-4B14-9AD6-C50EC37DFC77' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','948B60F0-8CE5-4B14-9AD6-C50EC37DFC77','948B60F0-8CE5-4B14-9AD6-C50EC37DFC77','5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"--View Status and Results
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"--Working Specification Review
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'C8891CFD-80BF-4F71-90DE-6748BF71566C' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','C8891CFD-80BF-4F71-90DE-6748BF71566C','C8891CFD-80BF-4F71-90DE-6748BF71566C','5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");



            Sql(@"--Summary Query
                --View Responses
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");

            Sql(@"--View Response Detail
                /*View Response Detail: WorkflowActivityID = 675F0001-6B44-4910-AD89-A3B600E98CE9 */
                IF (NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') AND EXISTS(SELECT NULL FROM WorkflowActivities WHERE ID = '675F0001-6B44-4910-AD89-A3B600E98CE9'))
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','675F0001-6B44-4910-AD89-A3B600E98CE9','675F0001-6B44-4910-AD89-A3B600E98CE9','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");

            Sql(@"--Respond to Request
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");

            Sql(@"--Submit Draft Report
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','9173A8E7-27C4-469D-853D-69A78501A522','9173A8E7-27C4-469D-853D-69A78501A522','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");

            Sql(@"--Review Draft Report
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81','2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");

            Sql(@"--Submit Final Report
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','F888C5D6-B8EB-417C-9DE2-4A96D75F3208','F888C5D6-B8EB-417C-9DE2-4A96D75F3208','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");

            Sql(@"--Review Final Report
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '2E7A3263-C87E-47BA-AC35-A78ABF8FE606' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','2E7A3263-C87E-47BA-AC35-A78ABF8FE606','2E7A3263-C87E-47BA-AC35-A78ABF8FE606','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");



            Sql(@"--Default Query 
                --Approve Response 
                IF (NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '6CE50001-A2B7-4721-890D-A3B600EDF917' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946') AND EXISTS(SELECT NULL FROM WorkflowActivities WHERE ID = '6CE50001-A2B7-4721-890D-A3B600EDF917'))
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','6CE50001-A2B7-4721-890D-A3B600EDF917','6CE50001-A2B7-4721-890D-A3B600EDF917','F64E0001-4F9A-49F0-BF75-A3B501396946')");

            Sql(@"--Request Review 
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '73740001-A942-47B0-BF6E-A3B600E7D9EC' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','73740001-A942-47B0-BF6E-A3B600E7D9EC','73740001-A942-47B0-BF6E-A3B600E7D9EC','F64E0001-4F9A-49F0-BF75-A3B501396946')");


            Sql(@"--View Request
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','F64E0001-4F9A-49F0-BF75-A3B501396946')");

            Sql(@"--View Response
                IF (NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946') AND EXISTS(SELECT NULL FROM WorkflowActivities WHERE ID = '675F0001-6B44-4910-AD89-A3B600E98CE9'))
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','675F0001-6B44-4910-AD89-A3B600E98CE9','675F0001-6B44-4910-AD89-A3B600E98CE9','F64E0001-4F9A-49F0-BF75-A3B501396946')");



            Sql(@"--DataChecker Query
                --Request Review
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '3FFBCA99-5801-4045-9FB4-072136A845FC' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','3FFBCA99-5801-4045-9FB4-072136A845FC','3FFBCA99-5801-4045-9FB4-072136A845FC','942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");

            Sql(@"--Approve Response
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '1D0D4993-EA87-4C0D-9226-43F8BB83C952' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','1D0D4993-EA87-4C0D-9226-43F8BB83C952','1D0D4993-EA87-4C0D-9226-43F8BB83C952','942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");

            Sql(@"--View Response
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'ECC689B1-C170-43BA-A181-F2762068F8FB' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','ECC689B1-C170-43BA-A181-F2762068F8FB','ECC689B1-C170-43BA-A181-F2762068F8FB','942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");

            Sql(@"--View Request Routings
                IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663') 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
        }
        
        public override void Down()
        {
            Sql(@"--Modular Program
                -- Distribute Request
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'E6CCD61B-81C4-4217-A958-ADAFB5EE5554' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D' ");
                
            Sql(@"-- PreDistribution Testing
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'EA69E5ED-6029-47E8-9B45-F0F00B07FDC2' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@"-- Prepare/Submit Draft Report
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@"-- Prepare/Submit Final Report
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@"-- Review Draft Report
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@"--Review Final Report
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '2E7A3263-C87E-47BA-AC35-A78ABF8FE6061' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@" --Specification Review
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '948B60F0-8CE5-4B14-9AD6-C50EC37DFC77' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@"--View Status and Results
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@" --Working Specification Review
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'C8891CFD-80BF-4F71-90DE-6748BF71566C' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");



            Sql(@" --Summary Query
                --View Responses
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@" --View Response Detail
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@" --Respond to Request
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@" --Submit Draft Report
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@"--Review Draft Report
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@" --Submit Final Report
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@" --Review Final Report
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '2E7A3263-C87E-47BA-AC35-A78ABF8FE606' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");



            Sql(@" --Default Query 
                --Approve Response 
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '6CE50001-A2B7-4721-890D-A3B600EDF917' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946'");

            Sql(@" --Request Review 
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '73740001-A942-47B0-BF6E-A3B600E7D9EC' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946'");


            Sql(@" --View Request
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946'");

            Sql(@"--View Response
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946'");



            Sql(@"--DataChecker Query
                --Request Review
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '3FFBCA99-5801-4045-9FB4-072136A845FC' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");

            Sql(@" --Approve Response
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '1D0D4993-EA87-4C0D-9226-43F8BB83C952' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");

            Sql(@" --View Response
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'ECC689B1-C170-43BA-A181-F2762068F8FB' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");

            Sql(@" --View Request Routings
                DELETE FROM WorkflowActivityCompletionMaps WHERE  WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");
        }
    }
}
