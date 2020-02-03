namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSummaryWFMappings : DbMigration
    {
        public override void Up()
        {
            //Update ResultID's when view routing status for Summary Workflow mapping to match Default workflow so that view routings pages can be shared
            
            Sql(@"DECLARE @destinationActivityID uniqueidentifier = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'
DECLARE @oldResultID uniqueidentifier = '61D4C5E0-07AC-4FDF-9F60-FC073D7BECDA'
DECLARE @newResultID uniqueidentifier = '49F9C682-9FAD-4AE5-A2C5-19157E227186'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )

--UnGroup
SET @destinationActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'
SET @oldResultID = 'CD6B90AB-91B8-42E7-A3F7-8795AB405C48'
SET @newResultID = '7821FC45-9FD5-4597-A405-B021E5ED14FA'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )

--View Response, double check result ID for delete
SET @destinationActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'
SET @oldResultID = '1A9EBEAC-09CB-4BBC-952C-52A1DEB31094'
SET @newResultID = '1C1D0001-65F4-4E02-9BB7-A3B600E27A2F'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )

--Add DataMart
SET @destinationActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C'
SET @oldResultID = '4186A06D-D5CC-439D-8B7B-D2A1A97D3ADE'
SET @newResultID = '15BDEF13-6E86-4E0F-8790-C07AE5B798A8'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )

--Remove DataMart
SET @destinationActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C'
SET @oldResultID = '7828CAD1-6547-4605-A361-6E76A796326B'
SET @newResultID = '5E010001-1353-44E9-9204-A3B600E263E9'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )

--Resubmit
SET @destinationActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C'
SET @oldResultID = '3FB86142-D6A1-45A5-A988-EF45B10D5C83'
SET @newResultID = '22AE0001-0B5A-4BA9-BB55-A3B600E2728C'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )");

        }
        
        public override void Down()
        {
            Sql(@"--Group
DECLARE @destinationActivityID uniqueidentifier = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'
DECLARE @oldResultID uniqueidentifier = '49F9C682-9FAD-4AE5-A2C5-19157E227186'
DECLARE @newResultID uniqueidentifier = '61D4C5E0-07AC-4FDF-9F60-FC073D7BECDA'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )

--UnGroup
SET @destinationActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'
SET @oldResultID = '7821FC45-9FD5-4597-A405-B021E5ED14FA'
SET @newResultID = 'CD6B90AB-91B8-42E7-A3F7-8795AB405C48'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )

--View Response, double check result ID for delete
SET @destinationActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'
SET @oldResultID = '1C1D0001-65F4-4E02-9BB7-A3B600E27A2F'
SET @newResultID = '1A9EBEAC-09CB-4BBC-952C-52A1DEB31094'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )

--Add DataMart
SET @destinationActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C'
SET @oldResultID = '15BDEF13-6E86-4E0F-8790-C07AE5B798A8'
SET @newResultID = '4186A06D-D5CC-439D-8B7B-D2A1A97D3ADE'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )

--Remove DataMart
SET @destinationActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C'
SET @oldResultID = '5E010001-1353-44E9-9204-A3B600E263E9'
SET @newResultID = '7828CAD1-6547-4605-A361-6E76A796326B'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )

--Resubmit
SET @destinationActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C'
SET @oldResultID = '22AE0001-0B5A-4BA9-BB55-A3B600E2728C'
SET @newResultID = '3FB86142-D6A1-45A5-A988-EF45B10D5C83'
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
AND DestinationWorkflowActivityID = @destinationActivityID AND WorkflowActivityResultID = @oldResultID

IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 
	AND WorkflowActivityResultID = @newResultID AND DestinationWorkflowActivityID = @destinationActivityID)
		INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID) 
		VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', @destinationActivityID, @newResultID )");
        }
    }
}
