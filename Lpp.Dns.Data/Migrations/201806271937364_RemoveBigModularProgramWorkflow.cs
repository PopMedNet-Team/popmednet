namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveBigModularProgramWorkflow : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                DISABLE Trigger Documents_Delete on Documents
				GO

				DECLARE @WorkflowID uniqueidentifier
				Set @WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'

                DELETE FROM LogsNewRequestDraftSubmitted WHERE RequestID IN (Select ID from Requests where WorkflowID = @WorkflowID OR RequestTypeID IN (select ID from RequestTypes where WorkflowID = @WorkflowID))
                DELETE FROM LogsNewRequestSubmitted WHERE RequestID IN (Select ID from Requests where WorkflowID = @WorkflowID OR RequestTypeID IN (select ID from RequestTypes where WorkflowID = @WorkflowID))
                DELETE FROM LogsNewRequestSubmitted WHERE TaskID IN (
	                Select DISTINCT ID FROM Tasks t
	                JOIN WorkflowActivityCompletionMaps as swa on t.WorkflowActivityID = swa.SourceWorkflowActivityID
	                JOIN WorkflowActivityCompletionMaps as dwa on t.WorkflowActivityID = dwa.DestinationWorkflowActivityID
	                WHERE swa.WorkflowID = @WorkflowID OR dwa.WorkflowID = @WorkflowID
                )
                DELETE FROM LogsRequestAssignmentChange WHERE RequestID IN (Select ID from Requests where WorkflowID = @WorkflowID OR RequestTypeID IN (select ID from RequestTypes where WorkflowID = @WorkflowID))
                DELETE FROM LogsRequestCommentChange WHERE CommentID IN (
	                Select c.CommentID from Requests r 
	                JOIN CommentReferences as c on r.ID = c.ItemID  where r.WorkflowID = @WorkflowID)
                DELETE FROM LogsRequestDocumentChange WHERE RequestID IN (Select ID from Requests where WorkflowID = @WorkflowID OR RequestTypeID IN (select ID from RequestTypes where WorkflowID = @WorkflowID))
				DELETE FROM LogsRequestDocumentChange WHERE TaskID IN (
					Select DISTINCT ID FROM Tasks t
	                JOIN WorkflowActivityCompletionMaps as swa on t.WorkflowActivityID = swa.SourceWorkflowActivityID
	                JOIN WorkflowActivityCompletionMaps as dwa on t.WorkflowActivityID = dwa.DestinationWorkflowActivityID
	                WHERE swa.WorkflowID = @WorkflowID OR dwa.WorkflowID = @WorkflowID
				)
                DELETE FROM LogsRequestMetadataChange WHERE RequestID IN (Select ID from Requests where WorkflowID = @WorkflowID OR RequestTypeID IN (select ID from RequestTypes where WorkflowID = @WorkflowID))
                DELETE FROM LogsRequestMetadataChange WHERE TaskID IN (
	                Select DISTINCT ID FROM Tasks t
	                JOIN WorkflowActivityCompletionMaps as swa on t.WorkflowActivityID = swa.SourceWorkflowActivityID
	                JOIN WorkflowActivityCompletionMaps as dwa on t.WorkflowActivityID = dwa.DestinationWorkflowActivityID
	                WHERE swa.WorkflowID = @WorkflowID OR dwa.WorkflowID = @WorkflowID
                )
                DELETE FROM LogsRequestStatusChange WHERE RequestID IN (Select ID from Requests where WorkflowID = @WorkflowID OR RequestTypeID IN (select ID from RequestTypes where WorkflowID = @WorkflowID))
                DELETE FROM LogsRequestStatusChange WHERE TaskID IN (
	                Select DISTINCT ID FROM Tasks t
	                JOIN WorkflowActivityCompletionMaps as swa on t.WorkflowActivityID = swa.SourceWorkflowActivityID
	                JOIN WorkflowActivityCompletionMaps as dwa on t.WorkflowActivityID = dwa.DestinationWorkflowActivityID
	                WHERE swa.WorkflowID = @WorkflowID OR dwa.WorkflowID = @WorkflowID
                )
                DELETE FROM LogsRequestDataMartMetadataChange WHERE RequestID IN (Select ID from Requests where WorkflowID = @WorkflowID OR RequestTypeID IN (select ID from RequestTypes where WorkflowID = @WorkflowID))
                DELETE FROM LogsResponseViewed WHERE ResponseID IN (
	                Select resp.ID from Requests r 
	                JOIN RequestDataMarts as rdm on r.ID = rdm.RequestID
	                JOIN RequestDataMartResponses as resp on rdm.ID = resp.RequestDataMartID
	                WHERE r.WorkflowID = @WorkflowID
                )
                DELETE FROM LogsResultsReminder WHERE RequestID IN (Select ID from Requests where WorkflowID = @WorkflowID)
                DELETE FROM LogsRoutingStatusChange WHERE RequestDataMartID IN (
	                Select rdm.ID from Requests r 
	                JOIN RequestDataMarts as rdm on r.ID = rdm.RequestID
	                WHERE r.WorkflowID = @WorkflowID OR r.RequestTypeID IN (select ID from RequestTypes where WorkflowID = @WorkflowID)
                )
                DELETE FROM LogsRoutingStatusChange WHERE TaskID IN (
	                Select DISTINCT ID FROM Tasks t
	                JOIN WorkflowActivityCompletionMaps as swa on t.WorkflowActivityID = swa.SourceWorkflowActivityID
	                JOIN WorkflowActivityCompletionMaps as dwa on t.WorkflowActivityID = dwa.DestinationWorkflowActivityID
	                WHERE swa.WorkflowID = @WorkflowID OR dwa.WorkflowID = @WorkflowID
                )
                DELETE FROM LogsSubmittedRequestAwaitsResponse WHERE RequestID IN (Select ID from Requests where WorkflowID = @WorkflowID)
                DELETE FROM LogsSubmittedRequestNeedsApproval WHERE RequestID IN (Select ID from Requests where WorkflowID = @WorkflowID)
                DELETE FROM LogsUploadedResultNeedsApproval WHERE RequestDataMartID IN (
	                Select rdm.ID from Requests r 
	                JOIN RequestDataMarts as rdm on r.ID = rdm.RequestID
	                WHERE r.WorkflowID = @WorkflowID
                )
                DELETE FROM LogsUploadedResultNeedsApproval WHERE TaskID IN (
	                Select DISTINCT ID FROM Tasks t
	                JOIN WorkflowActivityCompletionMaps as swa on t.WorkflowActivityID = swa.SourceWorkflowActivityID
	                JOIN WorkflowActivityCompletionMaps as dwa on t.WorkflowActivityID = dwa.DestinationWorkflowActivityID
	                WHERE swa.WorkflowID = @WorkflowID OR dwa.WorkflowID = @WorkflowID
                )
                DELETE FROM LogsTaskChange WHERE TaskID IN (
	                Select t.ItemID from Requests r 
	                JOIN TaskReferences as t on r.ID = t.ItemID
	                WHERE r.WorkflowID = @WorkflowID
                )
                DELETE FROM LogsTaskReminder WHERE TaskID IN (
	                Select t.ItemID from Requests r 
	                JOIN TaskReferences as t on r.ID = t.ItemID
	                WHERE r.WorkflowID = @WorkflowID
                )

                DELETE FROM LogsRequestDataMartAddedRemoved where RequestDataMartID IN (
	                Select rdm.ID from Requests r 
	                JOIN RequestDataMarts as rdm on r.ID = rdm.RequestID
	                WHERE r.WorkflowID = @WorkflowID
                )

                DELETE FROM LogsRequestDataMartAddedRemoved WHERE TaskID IN (
	                Select DISTINCT ID FROM Tasks t
	                JOIN WorkflowActivityCompletionMaps as swa on t.WorkflowActivityID = swa.SourceWorkflowActivityID
	                JOIN WorkflowActivityCompletionMaps as dwa on t.WorkflowActivityID = dwa.DestinationWorkflowActivityID
	                WHERE swa.WorkflowID = @WorkflowID OR dwa.WorkflowID = @WorkflowID
                )

                DELETE FROM LogsSubmittedRequestAwaitsResponse WHERE TaskID IN (
	                Select DISTINCT ID FROM Tasks t
	                JOIN WorkflowActivityCompletionMaps as swa on t.WorkflowActivityID = swa.SourceWorkflowActivityID
	                JOIN WorkflowActivityCompletionMaps as dwa on t.WorkflowActivityID = dwa.DestinationWorkflowActivityID
	                WHERE swa.WorkflowID = @WorkflowID OR dwa.WorkflowID = @WorkflowID
                )

                DELETE FROM Documents WHERE RevisionSetID IN (
	                Select doc.RevisionSetID from Requests r 
	                JOIN RequestDataMarts as rdm on r.ID = rdm.RequestID
	                JOIN RequestDataMartResponses as resp on rdm.ID = resp.RequestDataMartID
	                join RequestDocuments as doc on resp.ID = doc.ResponseID
	                WHERE r.WorkflowID = @WorkflowID)

                DELETE FROM Documents WHERE ItemID IN (
	                Select resp.ID from Requests r 
	                JOIN RequestDataMarts as rdm on r.ID = rdm.RequestID
	                JOIN RequestDataMartResponses as resp on rdm.ID = resp.RequestDataMartID
	                WHERE r.WorkflowID = @WorkflowID)

                DELETE FROM Documents WHERE ItemID IN (
	                Select r.ID from Requests r 
	                WHERE r.WorkflowID = @WorkflowID)

                DELETE FROM Comments where ID IN (
	                Select c.CommentID from Requests r 
	                JOIN CommentReferences as c on r.ID = c.ItemID  where r.WorkflowID = @WorkflowID)
                DELETE FROM CommentReferences where CommentID IN (
	                Select c.CommentID from Requests r 
	                JOIN CommentReferences as c on r.ID = c.ItemID  where r.WorkflowID = @WorkflowID)

                DELETE from RequestDocuments WHERE ResponseID IN (
	                Select resp.ID from Requests r 
	                JOIN RequestDataMarts as rdm on r.ID = rdm.RequestID
	                JOIN RequestDataMartResponses as resp on rdm.ID = resp.RequestDataMartID
	                WHERE r.WorkflowID = @WorkflowID OR  r.RequestTypeID IN (select ID from RequestTypes where WorkflowID = @WorkflowID))

                Delete from RequestObserverEventSubscriptions WHERE RequestObserverID IN (
	                Select ro.ID from Requests r 
	                JOIN RequestObservers as ro on r.ID = ro.RequestID
	                WHERE r.WorkflowID = @WorkflowID)

                DELETE FROM RequestDataMartResponseSearchResults WHERE RequestDataMartResponseID IN (
	                Select resp.ID from Requests r 
	                JOIN RequestDataMarts as rdm on r.ID = rdm.RequestID
	                JOIN RequestDataMartResponses as resp on rdm.ID = resp.RequestDataMartID
	                WHERE r.WorkflowID = @WorkflowID)

                DELETE FROM RequestObservers where RequestID IN (select ID from Requests WHERE WorkflowID = @WorkflowID)

                DELETE FROM RequestDataMarts where RequestID IN (select ID from Requests WHERE WorkflowID = @WorkflowID)

                DELETE FROM TaskUsers WHERE TaskID IN (
	                Select t.TaskID from Requests r 
	                JOIN TaskReferences as t on r.ID = t.ItemID
	                WHERE r.WorkflowID = @WorkflowID
                )

                DELETE FROM Tasks where ID IN (
	                Select t.TaskID from Requests r 
	                JOIN TaskReferences as t on r.ID = t.ItemID
	                WHERE r.WorkflowID = @WorkflowID or r.RequestTypeID IN (select ID from RequestTypes where WorkflowID = @WorkflowID)
                )

                DELETE FROM Tasks where WorkflowActivityID IN (
	                Select SourceWorkflowActivityID from WorkflowActivityCompletionMaps where WorkflowID = @WorkflowID
                )

                DELETE FROM TaskReferences where TaskID IN (select ID from Requests WHERE WorkflowID = @WorkflowID)

                DELETE FROM Requests  WHERE WorkflowID = @WorkflowID OR RequestTypeID IN (select ID from RequestTypes where WorkflowID = @WorkflowID) 

                DELETE FROM ProjectRequestTypes WHERE RequestTypeID IN ( select ID from RequestTypes where WorkflowID = @WorkflowID )

                DELETE FROM RequestTypeTerms WHERE RequestTypeID IN ( select ID from RequestTypes where WorkflowID = @WorkflowID )

                DELETE FROM TemplateTerms WHERE TemplateID IN ( select t.ID from RequestTypes rt JOIN Templates as t on rt.TemplateID = t.ID  where rt.WorkflowID = @WorkflowID )

                DELETE FROM Templates WHERE ID IN ( select TemplateID from RequestTypes where WorkflowID = @WorkflowID )

                DELETE FROM RequestTypes where WorkflowID = @WorkflowID

                DELETE FROM WorkflowActivityCompletionMaps where WorkflowID = @WorkflowID

                DELETE wa FROM WorkflowActivities wa WHERE NOT EXISTS (select null from WorkflowActivityCompletionMaps maps where maps.SourceWorkflowActivityID = wa.ID OR maps.DestinationWorkflowActivityID = wa.ID)

                DELETE war FROM WorkflowActivityResults war WHERE NOT EXISTS (select null from WorkflowActivityCompletionMaps maps where maps.WorkflowActivityResultID = war.ID)

                DELETE from Workflows where ID = @WorkflowID
                GO
				
				Enable Trigger Documents_Delete on Documents

            ");
        }
        
        public override void Down()
        {
        }
    }
}
