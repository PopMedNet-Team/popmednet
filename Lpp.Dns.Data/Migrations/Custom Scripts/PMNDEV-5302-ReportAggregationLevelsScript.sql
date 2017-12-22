/**

This script adds new Report Aggregation Levels specified by PMNDEV-4654.

**/

BEGIN TRANSACTION

DECLARE @NetworkID uniqueidentifier = (SELECT TOP 1 ID FROM Networks)

IF NOT EXISTS(SELECT NULL FROM ReportAggregationLevels WHERE [Name] = 'Aggregated by Site' and NetworkID = @NetworkID)
	INSERT INTO ReportAggregationLevels(ID, NetworkID, Name) VALUES (dbo.NewSqlGuid(), @NetworkID,'Aggregated by Site')

IF NOT EXISTS(SELECT NULL FROM ReportAggregationLevels WHERE [Name] = 'Aggregated by CDRN / PPRN' and NetworkID = @NetworkID)
	INSERT INTO ReportAggregationLevels(ID, NetworkID, Name) VALUES (dbo.NewSqlGuid(), @NetworkID,'Aggregated by CDRN / PPRN')

IF NOT EXISTS(SELECT NULL FROM ReportAggregationLevels WHERE [Name] = 'Aggregated Across CDRNs' and NetworkID = @NetworkID)
	INSERT INTO ReportAggregationLevels(ID, NetworkID, Name) VALUES (dbo.NewSqlGuid(), @NetworkID,'Aggregated Across CDRNs')

IF NOT EXISTS(SELECT NULL FROM ReportAggregationLevels WHERE [Name] = 'Aggregated Across PPRNs' and NetworkID = @NetworkID)
	INSERT INTO ReportAggregationLevels(ID, NetworkID, Name) VALUES (dbo.NewSqlGuid(), @NetworkID,'Aggregated Across PPRNs')

IF NOT EXISTS(SELECT NULL FROM ReportAggregationLevels WHERE [Name] = 'Aggregated Across PCORnet DRN' and NetworkID = @NetworkID)
	INSERT INTO ReportAggregationLevels(ID, NetworkID, Name) VALUES (dbo.NewSqlGuid(), @NetworkID,'Aggregated Across PCORnet DRN')

COMMIT TRANSACTION