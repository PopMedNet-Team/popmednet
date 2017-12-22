namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResultForDRCompleteWithUpload : DbMigration
    {
        public override void Up()
        {
            //--Create Complete Routings Activity Result
            Sql(@"Insert INTO WorkflowActivityResults([ID],[Name],[Description])VALUES('7BC6CF23-D62D-4958-B0DA-78B089A23552','Complete Routings to Analysis Center','Complete Routings of Distributed Regression Requests')");

            //--DISTIBUTION Activity SAVE
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '370646FC-7A47-43B5-A4B3-659F90A188A9', '7BC6CF23-D62D-4958-B0DA-78B089A23552')");

            Sql(@"ALTER VIEW [dbo].[vwRequestStatistics]
                    AS
                    SELECT RequestID, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 1 OR QueryStatusTypeId = 0 THEN 1 ELSE 0 END), 0) AS Draft,
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 2 OR QueryStatusTypeId = 11 THEN 1 ELSE 0 END), 0) AS Submitted, --submitted or hold (which is submitted dmc state)
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 3 OR QueryStatusTypeId = 14 OR QueryStatusTypeId = 16 THEN 1 ELSE 0 END), 0) AS Completed, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 4 THEN 1 ELSE 0 END), 0) AS AwaitingRequestApproval, 
                        ISNULL(SUM(CASE WHEN QueryStatusTypeId = 10 THEN 1 ELSE 0 END), 0) AS AwaitingResponseApproval, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 5 THEN 1 ELSE 0 END), 0) AS RejectedRequest, 
                        ISNULL(SUM(CASE WHEN QueryStatusTypeId = 12 THEN 1 ELSE 0 END), 0) AS RejectedBeforeUploadResults, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 13 THEN 1 ELSE 0 END), 0) AS RejectedAfterUploadResults, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 6 THEN 1 ELSE 0 END), 0) AS Canceled, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 7 THEN 1 ELSE 0 END), 0) AS Resubmitted, 
	                    COUNT(*) AS Total
                    FROM            dbo.RequestDataMarts
					WHERE RoutingType IS NULL OR RoutingType = 1
                    GROUP BY RequestID");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM WorkflowActivityCompletionMaps where WorkflowActivityResultID = '7BC6CF23-D62D-4958-B0DA-78B089A23552'");
            Sql(@"DELETE FROM WorkflowActivityResults where ID = '7BC6CF23-D62D-4958-B0DA-78B089A23552'");
        }
    }
}
