namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWorkflowValues : DbMigration
    {
        public override void Up()
        {
            //Summary Table Workflow
            //Request Form -> New Request
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='New Request'
WHERE ID='197AF4BA-F079-48DD-9E7C-C7BE7F8DC896'");

            //Review Request Form -> Request Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Request Review'
WHERE ID='CC1BCADD-4487-47C7-BDCA-1010F2C68FE0'");


            //Review Draft Report -> Draft Report Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Draft Report Review'
WHERE ID='2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81'");

            //Review Final Report -> Final Report Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Final Report Review'
WHERE ID='2E7A3263-C87E-47BA-AC35-A78ABF8FE606'");



            //Modular Program Workflow specific
            //Request Form -> New Request
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='New Request'
WHERE ID='0321E17F-AA1F-4B23-A145-85B159E74F0F'");

            //Review Request Form -> Request Form Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Request Form Review'
WHERE ID='A96FBAD0-8FD8-4D10-8891-D749A71912F8'");

            //Submit Working Specifications -> Working Specifications 
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Working Specifications'
WHERE ID='31C60BB1-2F6A-423B-A7B7-B52626FD9E97'");

            //Review Working Specifications -> Working Specification Review 
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Working Specification Review'
WHERE ID='C8891CFD-80BF-4F71-90DE-6748BF71566C'");

            //Submit Specifications -> Specifications
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Specifications'
WHERE ID='C3B13067-3B9D-41E4-8D4A-7114A6E81930'");

            //Review Specifications -> Specification Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Specification Review'
WHERE ID='948B60F0-8CE5-4B14-9AD6-C50EC37DFC77'");

            //Perform Pre-Distribution Testing -> Pre-Distribution Testing
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Pre-Distribution Testing'
WHERE ID='49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7'");

            //Review Pre-Distribution Testing -> Pre-Distribution Testing Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Pre-Distribution Testing Review'
WHERE ID='EA69E5ED-6029-47E8-9B45-F0F00B07FDC2'");

            //Distribute Request -> Request Distribution
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Request Distribution'
WHERE ID='E6CCD61B-81C4-4217-A958-ADAFB5EE5554'");

            //Request Form -> New Request
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='New Request'
WHERE ID='C1380001-4524-49BA-B4B6-A3B5013A3343'");

            //View Responses -> View Status and Results
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='View Status and Results'
WHERE ID='ACBA0001-0CE4-4C00-8DD3-A3B5013A3344'");

            //Review Request Form -> Request Review 
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Request Review'
WHERE ID='73740001-A942-47B0-BF6E-A3B600E7D9EC'");
        }

        public override void Down()
        {
            //Summary Table Workflow
            //undo Request Form -> New Request
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Request Form'
WHERE ID='197AF4BA-F079-48DD-9E7C-C7BE7F8DC896'");

            //undo Review Request Form -> Request Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Review Request Form'
WHERE ID='CC1BCADD-4487-47C7-BDCA-1010F2C68FE0'");


            //undo Review Draft Report -> Draft Report Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Review Draft Report'
WHERE ID='2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81'");

            //undo Review Final Report -> Final Report Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Review Final Report'
WHERE ID='2E7A3263-C87E-47BA-AC35-A78ABF8FE606'");



            //Modular Program Workflow specific
            //undo Request Form -> New Request
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Request Form'
WHERE ID='0321E17F-AA1F-4B23-A145-85B159E74F0F'");

            //undo Review Request Form -> Request Form Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Review Request Form'
WHERE ID='A96FBAD0-8FD8-4D10-8891-D749A71912F8'");

            //undo Submit Working Specifications -> Working Specifications 
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Submit Working Specifications'
WHERE ID='31C60BB1-2F6A-423B-A7B7-B52626FD9E97'");

            //undo Review Working Specifications -> Working Specification Review 
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Review Working Specifications'
WHERE ID='C8891CFD-80BF-4F71-90DE-6748BF71566C'");

            //undo Submit Specifications -> Specifications
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Submit Specifications'
WHERE ID='C3B13067-3B9D-41E4-8D4A-7114A6E81930'");

            //undo Review Specifications -> Specification Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Review Specifications'
WHERE ID='948B60F0-8CE5-4B14-9AD6-C50EC37DFC77'");

            //undo Perform Pre-Distribution Testing -> Pre-Distribution Testing
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Perform Pre-Distribution Testing'
WHERE ID='49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7'");

            //undo Review Pre-Distribution Testing -> Pre-Distribution Testing Review
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Review Pre-Distribution Testing'
WHERE ID='EA69E5ED-6029-47E8-9B45-F0F00B07FDC2'");

            //undo Distribute Request -> Request Distribution
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Distribute Request'
WHERE ID='E6CCD61B-81C4-4217-A958-ADAFB5EE5554'");

            //Default Section
            //undo Request Form -> New Request
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Request Form'
WHERE ID='C1380001-4524-49BA-B4B6-A3B5013A3343'");

            //undo View Responses -> View Status and Results
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='View Responses'
WHERE ID='ACBA0001-0CE4-4C00-8DD3-A3B5013A3344'");

            //undo Review Request Form -> Request Review 
            Sql(@"UPDATE dbo.WorkflowActivities
SET Name='Review Request Form'
WHERE ID='73740001-A942-47B0-BF6E-A3B600E7D9EC'");
        }
    }
}
