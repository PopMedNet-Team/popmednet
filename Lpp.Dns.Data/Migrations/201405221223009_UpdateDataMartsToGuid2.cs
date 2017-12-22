using Lpp.Utilities;

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataMartsToGuid2 : DbMigration
    {
        public override void Up()
        {
            //Removes
            Sql("DROP View RoutingCounts");
            Sql("DROP View _RoutingCounts");

            //DataMartTypes
            DropForeignKey("DataMarts", "FK_DataMarts_DataMartTypes");
            DropPrimaryKey("DataMartTypes");
            DropColumn("DataMarts", "DataMartTypeId");

            //QueriesDataMarts
            Sql(MigrationHelpers.ClearStatsScript("QueriesDataMarts"));
            DropIndex("QueriesDataMarts", "RequestDataMart_DataMart");

            DropPrimaryKey("QueriesDataMarts");
            DropColumn("QueriesDataMarts", "DataMartId");

            //RequestDataMartSearchResults
            DropForeignKey("RequestDataMartSearchResults", "FK_dbo.RequestDataMartSearchResults_dbo.DataMarts_ResultDataMartId");
            DropForeignKey("RequestDataMartSearchResults", "FK_dbo.RequestDataMartSearchResults_dbo.Queries_SearchRequestId");
            Sql("ALTER TABLE RequestDataMartSearchResults DROP CONSTRAINT [PK_dbo.RequestDataMartSearchResults]");
            DropIndex("RequestDataMartSearchResults", "IX_ResultDataMartId");
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1 Tab.CONSTRAINT_NAME from 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
WHERE 
    Col.Constraint_Name = Tab.Constraint_Name
    AND Col.Table_Name = Tab.Table_Name
    AND Constraint_Type = 'PRIMARY KEY '
    AND Col.Table_Name = 'RequestDataMartSearchResults')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE RequestDataMartSearchResults DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");

            DropColumn("RequestDataMartSearchResults", "ResultDataMartId");

            //RequestRoutingInstances
            DropForeignKey("RequestRoutingInstances", "DataMartID", "DataMarts");
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'RequestRoutingInstances' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'DataMartID')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE RequestRoutingInstances DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");
            DropColumn("RequestRoutingInstances", "DataMartId");

            //Projects_DataMarts
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1 Tab.CONSTRAINT_NAME from 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
WHERE 
    Col.Constraint_Name = Tab.Constraint_Name
    AND Col.Table_Name = Tab.Table_Name
    AND Constraint_Type = 'PRIMARY KEY '
    AND Col.Table_Name = 'Projects_DataMarts')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Projects_DataMarts DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");

            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'Projects_DataMarts' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'DataMartId')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Projects_DataMarts DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");

            DropColumn("Projects_DataMarts", "DataMartId");

            //DataMartAvailabilityPeriods
            DropForeignKey("DataMartAvailabilityPeriods", "FK_DataMartAvailabilityPeriods_DataMarts_DataMartId");
            AddColumn("DataMartAvailabilityPeriods", "DataMartSID", c => c.Guid(true));
            Sql(
                "UPDATE DataMartAvailabilityPeriods SET DataMartSID = (SELECT TOP 1 SID FROM DataMarts WHERE DataMartId = DataMartAvailabilityPeriods.DataMartId)");
            DropColumn("DataMartAvailabilityPeriods", "DataMartId");
            
            //DataMartInstalledModels
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1 Tab.CONSTRAINT_NAME from 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
WHERE 
    Col.Constraint_Name = Tab.Constraint_Name
    AND Col.Table_Name = Tab.Table_Name
    AND Constraint_Type = 'PRIMARY KEY '
    AND Col.Table_Name = 'DataMartInstalledModels')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE DataMartInstalledModels DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");

            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'DataMartInstalledModels' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'DataMartId')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE DataMartInstalledModels DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");
            DropColumn("DataMartInstalledModels", "DataMartId");

            //DataMarts
            DropPrimaryKey("DataMarts");
            DropColumn("DataMarts", "DataMartId");

            //Adds

            //DataMartTypes
            RenameColumn("DataMartTypes", "SID", "ID");
            AddPrimaryKey("DataMartTypes", "ID");

            //DataMarts
            RenameColumn("DataMarts", "SID", "ID");
            AddPrimaryKey("DataMarts", "ID");
            RenameColumn("DataMarts", "DataMartTypeSID", "DataMartTypeID");
            AlterColumn("DataMarts", "DataMartTypeID", c => c.Guid(true));
            AddForeignKey("DataMarts", "DataMartTypeID", "DataMartTypes", "ID", false);
            CreateIndex("DataMarts", "DataMartTypeID");

            //DataMartAvailabilityPeriods
            RenameColumn("DataMartAvailabilityPeriods", "DataMartSID", "DataMartID");
            AddForeignKey("DataMartAvailabilityPeriods", "DataMartID", "DataMarts", "ID");

            //DataMartInstalledModels
            RenameColumn("DataMartInstalledModels", "DataMartSID", "DataMartID");
            AddPrimaryKey("DataMartInstalledModels", new string[] {"DataMartID", "ModelID"});
            AddForeignKey("DataMartInstalledModels", "DataMartID", "DataMarts", "ID", true);

            //QueriesDataMarts
            RenameTable("QueriesDataMarts", "RequestDataMarts");
            RenameColumn("RequestDataMarts", "DataMartSID", "DataMartID");
            AddPrimaryKey("RequestDataMarts", new string[] { "QueryId", "DataMartID" });
            AddForeignKey("RequestDataMarts", "DataMartID", "DataMarts", "ID", true);

            //RequestDataMartSearchResults
            RenameColumn("RequestDataMartSearchResults", "ResultDataMartSID", "ResultDataMartID");
            AddPrimaryKey("RequestDataMartSearchResults", new string[] { "SearchRequestId", "ResultDatamartID" });
            AddForeignKey("RequestDataMartSearchResults", "ResultDatamartID", "DataMarts", "ID", true);
            
            //Request Routing Instances
            RenameTable("RequestRoutingInstances", "RequestResponses");
            RenameColumn("RequestResponses", "DataMartSID", "DataMartID");
            AddForeignKey("RequestResponses", "DataMartID", "DataMarts", "ID", true);

            //Projects_DataMarts
            RenameTable("Projects_DataMarts", "ProjectDataMarts");
            RenameColumn("ProjectDataMarts", "DataMartSID", "DataMartID");
            AddPrimaryKey("ProjectDataMarts", new string[] { "ProjectID", "DataMartID" });
            AddForeignKey("ProjectDataMarts", "DataMartID", "DataMarts", "ID", true);

            //RoutingCounts
            Sql(@"create view [dbo].[_RoutingCounts]
with schemabinding
as
select QueryId,
	Sum(case when QueryStatusTypeId = 2 then 1 else 0 end) as Submitted,
	Sum(case when QueryStatusTypeId = 3 OR QueryStatusTypeId = 14 then 1 else 0 end) as Completed,
	Sum(case when QueryStatusTypeId = 4 then 1 else 0 end) as AwaitingRequestApproval,
	Sum(case when QueryStatusTypeId = 10 then 1 else 0 end) as AwaitingResponseApproval,
	Sum(case when QueryStatusTypeId = 5 then 1 else 0 end) as RejectedRequest,
	Sum(case when QueryStatusTypeId = 12 then 1 else 0 end) as RejectedBeforeUploadResults,
	Sum(case when QueryStatusTypeId = 13 then 1 else 0 end) as RejectedAfterUploadResults,
	COUNT_BIG(*) as Total
from
	dbo.RequestDataMarts
group by QueryId");

            Sql(@"create view [dbo].[RoutingCounts] as select * from _RoutingCounts
");
        }
        
        public override void Down()
        {
        }
    }
}
