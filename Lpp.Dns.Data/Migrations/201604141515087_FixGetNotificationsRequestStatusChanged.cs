namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixGetNotificationsRequestStatusChanged : DbMigration
    {
        public override void Up()
        {

            Sql(@"ALTER FUNCTION [dbo].[GetNotifications]
(
	@UserID uniqueidentifier
)
RETURNS 
@notifications TABLE 
(
	TimeStamp datetimeoffset(7),
	Event nvarchar(max),
	Message nvarchar(max)
)
AS
BEGIN
	--DataMart Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsDataMartChange.TimeStamp, 'DataMart Changed', LogsDataMartChange.Description FROM LogsDataMartChange JOIN UserEventSubscriptions ON LogsDataMartChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventDataMarts(@UserID, '59A90001-539E-4C21-A4F2-A22200CD3C7D') e WHERE e.DataMartID = LogsDataMartChange.DataMartID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '59A90001-539E-4C21-A4F2-A22200CD3C7D') e WHERE EXISTS(SELECT NULL FROM DataMarts WHERE DataMarts.OrganizationID = e.OrganizationID AND DataMarts.ID = LogsDataMartChange.DataMartID))
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '59A90001-539E-4C21-A4F2-A22200CD3C7D'))
		)
	
	--Group Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsGroupChange.TimeStamp, 'Group Changed', LogsGroupChange.Description FROM LogsGroupChange JOIN UserEventSubscriptions ON LogsGroupChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventGroups(@UserID, 'D80E0001-27BC-4FCB-BA75-A22200CD2426') e WHERE e.GroupID = LogsGroupChange.GroupID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, 'D80E0001-27BC-4FCB-BA75-A22200CD2426'))
		)

	--Organization Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsOrganizationChange.TimeStamp, 'Organization Changed', LogsOrganizationChange.Description FROM LogsOrganizationChange JOIN UserEventSubscriptions ON LogsOrganizationChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'B8A50001-B556-43D2-A1B8-A22200CD12DC') e WHERE e.OrganizationID = LogsOrganizationChange.OrganizationID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, 'B8A50001-B556-43D2-A1B8-A22200CD12DC'))
		)

	--Project Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsProjectChange.TimeStamp, 'Project Changed', LogsProjectChange.Description FROM LogsProjectChange JOIN UserEventSubscriptions ON LogsProjectChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '1C090001-9F95-400C-9780-A22200CD0234') e WHERE e.ProjectID = LogsProjectChange.ProjectID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '1C090001-9F95-400C-9780-A22200CD0234'))
		)

	--Registry Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsRegistryChange.TimeStamp, 'Registry Changed', LogsRegistryChange.Description FROM LogsRegistryChange JOIN UserEventSubscriptions ON LogsRegistryChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventRegistries(@UserID, '553FD350-8F3B-40C6-9E31-11D8BC7420A2') e WHERE e.RegistryID = LogsRegistryChange.RegistryID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '553FD350-8F3B-40C6-9E31-11D8BC7420A2'))
		)

	--User Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsUserChange.TimeStamp, 'User Changed', LogsUserChange.Description FROM LogsUserChange JOIN UserEventSubscriptions ON LogsUserChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, 'B7640001-7247-49B8-A818-A22200CCEAF7') e WHERE e.UserID = LogsUserChange.UserID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, 'B7640001-7247-49B8-A818-A22200CCEAF7'))
		)


	--New DataMart Client
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsNewDataMartClient.TimeStamp, 'New DataMart Client', LogsNewDataMartClient.Description FROM LogsNewDataMartClient JOIN UserEventSubscriptions ON LogsNewDataMartClient.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventDataMarts(@UserID, '60F20001-77FF-4F4B-9153-A2220129E466'))
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '60F20001-77FF-4F4B-9153-A2220129E466'))
		)

	--Password Expiration Log
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsPasswordExpiration.TimeStamp, 'Password Expiration', LogsPasswordExpiration.Description FROM LogsPasswordExpiration JOIN UserEventSubscriptions ON LogsPasswordExpiration.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND LogsPasswordExpiration.ExpiringUserID = @UserID
	
	--Profile Updated Log
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsProfileUpdated.TimeStamp, 'Profile Updated', LogsProfileUpdated.Description FROM LogsProfileUpdated JOIN UserEventSubscriptions ON LogsProfileUpdated.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND UserChangedID = @UserID	

	--Registration Submitted
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsUserRegistrationSubmitted.TimeStamp, 'Registration Submitted', LogsUserRegistrationSubmitted.Description FROM LogsUserRegistrationSubmitted JOIN UserEventSubscriptions ON LogsUserRegistrationSubmitted.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID AND LogsUserRegistrationSubmitted.UserID <> LogsUserRegistrationSubmitted.RegisteredUserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, '3AC20001-D8A4-4BE7-957C-A22200CC84BB') e WHERE e.UserID = LogsUserRegistrationSubmitted.UserID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '3AC20001-D8A4-4BE7-957C-A22200CC84BB'))
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '3AC20001-D8A4-4BE7-957C-A22200CC84BB') e JOIN Users u ON e.OrganizationID = u.OrganizationID WHERE u.ID = LogsUserRegistrationSubmitted.UserID)
		)

	--Registration Changed
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsUserRegistrationChanged.TimeStamp, 'Registration Changed', LogsUserRegistrationChanged.Description FROM LogsUserRegistrationChanged JOIN UserEventSubscriptions ON LogsUserRegistrationChanged.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID AND LogsUserRegistrationChanged.UserID <> LogsUserRegistrationChanged.RegisteredUserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, '76B10001-2B49-453C-A8E1-A22200CC9356') e WHERE e.UserID = LogsUserRegistrationChanged.UserID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '76B10001-2B49-453C-A8E1-A22200CC9356'))
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '76B10001-2B49-453C-A8E1-A22200CC9356') e JOIN Users u ON e.OrganizationID = u.OrganizationID WHERE u.ID = LogsUserRegistrationChanged.UserID)
		)


	--Results Viewed
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsResponseViewed.TimeStamp, 'Results Viewed', LogsResponseViewed.Description FROM LogsResponseViewed JOIN UserEventSubscriptions ON LogsResponseViewed.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r JOIN RequestDataMarts ON r.ID = RequestDataMarts.RequestID JOIN RequestDataMartResponses ON RequestDataMarts.ID = RequestDataMartResponses.RequestDataMartID WHERE RequestDataMartResponses.ID = LogsResponseViewed.ResponseID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '25EC0001-3AC0-45FB-AF72-A22200CC334C') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID JOIN RequestDataMartResponses ON RequestDataMarts.ID = RequestDataMartResponses.RequestDataMartID WHERE RequestDataMartResponses.ID = LogsResponseViewed.ResponseID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '25EC0001-3AC0-45FB-AF72-A22200CC334C') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID JOIN RequestDataMartResponses ON RequestDataMarts.ID = RequestDataMartResponses.RequestDataMartID WHERE RequestDataMartResponses.ID = LogsResponseViewed.ResponseID)
		)

	--Request Status Changed
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsRequestStatusChange.TimeStamp, 'Request Status Changed', LogsRequestStatusChange.Description FROM LogsRequestStatusChange JOIN UserEventSubscriptions ON LogsRequestStatusChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE r.ID = LogsRequestStatusChange.RequestID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '0A850001-FC8A-4DE2-9AA5-A22200E82398') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID WHERE Requests.ID = LogsRequestStatusChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '0A850001-FC8A-4DE2-9AA5-A22200E82398') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsRequestStatusChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectOrganizations(@UserID, '0A850001-FC8A-4DE2-9AA5-A22200E82398') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsRequestStatusChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM RequestUsers ru WHERE ru.RequestID = LogsRequestStatusChange.RequestID AND ru.UserID = UserEventSubscriptions.UserID)
		)
		AND 
		(
			(UserEventSubscriptions.Frequency IS NOT NULL)
			OR
			(UserEventSubscriptions.FrequencyForMy IS NOT NULL AND EXISTS(SELECT NULL FROM RequestUsers ru WHERE ru.RequestID = LogsRequestStatusChange.RequestID AND ru.UserID = UserEventSubscriptions.UserID))
		)	
		

	--Routing Status Changed
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsRoutingStatusChange.TimeStamp, 'Routing Status Changed', LogsRoutingStatusChange.Description FROM LogsRoutingStatusChange JOIN UserEventSubscriptions ON LogsRoutingStatusChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r JOIN RequestDataMarts ON r.ID = RequestDataMarts.RequestID WHERE RequestDataMarts.ID = LogsRoutingStatusChange.RequestDataMartID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '5AB90001-8072-42CD-940F-A22200CC24A2') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID WHERE RequestDataMarts.ID = LogsRoutingStatusChange.RequestDataMartID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '5AB90001-8072-42CD-940F-A22200CC24A2') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID WHERE RequestDataMarts.ID = LogsRoutingStatusChange.RequestDataMartID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '5AB90001-8072-42CD-940F-A22200CC24A2') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID AND RequestDataMarts.DataMartID = e.DataMartID WHERE RequestDataMarts.ID = LogsRoutingStatusChange.RequestDataMartID)
		)
		AND 
		(
			(UserEventSubscriptions.Frequency IS NOT NULL)
			OR
			(UserEventSubscriptions.FrequencyForMy IS NOT NULL AND EXISTS(SELECT NULL FROM RequestUsers ru JOIN Requests ON Requests.ID = ru.RequestID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID WHERE RequestDataMarts.ID = LogsRoutingStatusChange.RequestDataMartID AND ru.UserID = UserEventSubscriptions.UserID))
		)

	--Results Reminder
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsResultsReminder.TimeStamp, 'Results Reminder', LogsResultsReminder.Description FROM LogsResultsReminder JOIN UserEventSubscriptions ON LogsResultsReminder.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE r.ID = LogsResultsReminder.RequestID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'E39A0001-A4CA-46B8-B7EF-A22200E72B08') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID  WHERE Requests.ID = LogsResultsReminder.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, 'E39A0001-A4CA-46B8-B7EF-A22200E72B08') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsResultsReminder.RequestID)
		)

	--Submitted Request Awaits Response
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsSubmittedRequestAwaitsResponse.TimeStamp, 'Request Awaits Response', LogsSubmittedRequestAwaitsResponse.Description FROM LogsSubmittedRequestAwaitsResponse JOIN UserEventSubscriptions ON LogsSubmittedRequestAwaitsResponse.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE r.ID = LogsSubmittedRequestAwaitsResponse.RequestID)
		AND EXISTS(SELECT NULL FROM RequestDataMarts WHERE RequestDataMarts.RequestID = LogsSubmittedRequestAwaitsResponse.RequestID AND RequestDataMarts.QueryStatusTypeID = 2) --Must have a routing that's status is Submitted
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '688B0001-1572-41CA-8298-A22200CBD542') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID  WHERE Requests.ID = LogsSubmittedRequestAwaitsResponse.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '688B0001-1572-41CA-8298-A22200CBD542') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsSubmittedRequestAwaitsResponse.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '688B0001-1572-41CA-8298-A22200CBD542') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID AND RequestDataMarts.DataMartID = e.DataMartID WHERE Requests.ID = LogsSubmittedRequestAwaitsResponse.RequestID)
		)


	--Submitted Request Awaits Approval
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsSubmittedRequestNeedsApproval.TimeStamp, 'Request Awaits Approval', LogsSubmittedRequestNeedsApproval.Description FROM LogsSubmittedRequestNeedsApproval JOIN UserEventSubscriptions ON LogsSubmittedRequestNeedsApproval.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE r.ID = LogsSubmittedRequestNeedsApproval.RequestID AND r.Status = 300)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'B7B30001-2704-4A57-A71A-A22200CC1736') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID  WHERE Requests.ID = LogsSubmittedRequestNeedsApproval.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, 'B7B30001-2704-4A57-A71A-A22200CC1736') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsSubmittedRequestNeedsApproval.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectOrganizations(@UserID, 'B7B30001-2704-4A57-A71A-A22200CC1736') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsSubmittedRequestNeedsApproval.RequestID)
		)

	--Uploaded Result needs approval
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsUploadedResultNeedsApproval.TimeStamp, 'Uploaded Result Needs Approval', LogsUploadedResultNeedsApproval.Description FROM LogsUploadedResultNeedsApproval JOIN UserEventSubscriptions ON LogsUploadedResultNeedsApproval.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredResponseList(@UserID) r JOIN RequestDataMarts ON r.RequestDataMartID = RequestDataMarts.ID WHERE r.RequestDataMartID = LogsUploadedResultNeedsApproval.RequestDataMartID AND RequestDataMarts.QueryStatusTypeID = 10)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'F31C0001-6900-4BDB-A03A-A22200CC019C') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID  WHERE RequestDataMarts.ID = LogsUploadedResultNeedsApproval.RequestDataMartID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, 'F31C0001-6900-4BDB-A03A-A22200CC019C') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID  WHERE RequestDataMarts.ID = LogsUploadedResultNeedsApproval.RequestDataMartID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, 'F31C0001-6900-4BDB-A03A-A22200CC019C') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID AND RequestDataMarts.DataMartID = e.DataMartID WHERE RequestDataMarts.ID = LogsUploadedResultNeedsApproval.RequestDataMartID)

		)

	-- New Request Draft Submitted
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsNewRequestDraftSubmitted.TimeStamp, 'New Request Draft Submitted', LogsNewRequestDraftSubmitted.Description FROM LogsNewRequestDraftSubmitted JOIN UserEventSubscriptions ON LogsNewRequestDraftSubmitted.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE LogsNewRequestDraftSubmitted.RequestID = r.ID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '6549439E-E3E4-4F4C-92CF-88FB81FF8869') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID WHERE Requests.ID = LogsNewRequestDraftSubmitted.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '6549439E-E3E4-4F4C-92CF-88FB81FF8869') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsNewRequestDraftSubmitted.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '6549439E-E3E4-4F4C-92CF-88FB81FF8869') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsNewRequestDraftSubmitted.RequestID)
		)

	-- New Request submitted
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsNewRequestSubmitted.TimeStamp, 'New Request Submitted', LogsNewRequestSubmitted.Description FROM LogsNewRequestSubmitted JOIN UserEventSubscriptions ON LogsNewRequestSubmitted.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE LogsNewRequestSubmitted.RequestID = r.ID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '06E30001-ED86-4427-9936-A22200CC74F0') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID WHERE Requests.ID = LogsNewRequestSubmitted.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '06E30001-ED86-4427-9936-A22200CC74F0') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsNewRequestSubmitted.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '06E30001-ED86-4427-9936-A22200CC74F0') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsNewRequestSubmitted.RequestID)
		)

	-- Task Changed
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsTaskChange.TimeStamp, 'Task Changed', LogsTaskChange.Description FROM LogsTaskChange JOIN UserEventSubscriptions ON LogsTaskChange.EventID = UserEventSubscriptions.EventID
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '2DFE0001-B98D-461D-A705-A3BE01411396') e JOIN Requests r ON e.ProjectID = r.ProjectId JOIN TaskReferences tr ON r.ID = tr.ItemID WHERE tr.TaskID = LogsTaskChange.TaskID AND tr.Type = 2 )
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '2DFE0001-B98D-461D-A705-A3BE01411396') e JOIN Requests r ON e.OrganizationID = r.OrganizationID JOIN TaskReferences tr ON r.ID = tr.ItemID WHERE tr.TaskID = LogsTaskChange.TaskID AND tr.Type = 2)
			OR
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, '2DFE0001-B98D-461D-A705-A3BE01411396') e WHERE e.UserID = LogsTaskChange.UserID)
		)

	-- Request Assignment Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT 
	LogsRequestAssignmentChange.TimeStamp, 
	CASE WHEN RequestUserUserID = @UserID AND Reason = 4 THEN 'Request Assigned to You'
		WHEN RequestUserUserID = @UserID AND Reason = 8 THEN 'Unassigned from Request'
		WHEN RequestUserUserID != @UserID THEN 'Request Assignment Changed'
	END, 
	LogsRequestAssignmentChange.Description FROM LogsRequestAssignmentChange JOIN UserEventSubscriptions ON LogsRequestAssignmentChange.EventID = UserEventSubscriptions.EventID
		WHERE
		UserEventSubscriptions.UserID = @UserID 
		AND (LogsRequestAssignmentChange.WorkflowRoleID != 'B5C4CFA7-2C63-4C8B-A981-4BC5A41AAFC7' 
			AND LogsRequestAssignmentChange.WorkflowRoleID != 'B96BD897-3942-4DF0-888A-5927751E8EF1' 
			AND LogsRequestAssignmentChange.WorkflowRoleID != '06460001-288E-4532-833D-A402011F437A'
			AND LogsRequestAssignmentChange.WorkflowRoleID != 'FDB30001-028D-4E92-9357-A402011F56F4')
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '45DA0001-7E63-4578-9A19-A43B0100F7C8') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = LogsRequestAssignmentChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '45DA0001-7E63-4578-9A19-A43B0100F7C8') e JOIN Requests r ON e.OrganizationID = r.OrganizationID WHERE r.ID = LogsRequestAssignmentChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, '45DA0001-7E63-4578-9A19-A43B0100F7C8') e WHERE e.UserID = LogsRequestAssignmentChange.UserID)
		)
		AND 
		(
			(UserEventSubscriptions.Frequency IS NOT NULL)
			OR
			(UserEventSubscriptions.FrequencyForMy IS NOT NULL AND EXISTS(SELECT NULL FROM RequestUsers ru WHERE ru.UserID = LogsRequestAssignmentChange.RequestUserUserID AND ru.UserID = UserEventSubscriptions.UserID))
		)


	-- Request Document Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsRequestDocumentChange.TimeStamp, 'Request Document Changed', LogsRequestDocumentChange.Description FROM LogsRequestDocumentChange 
	JOIN UserEventSubscriptions ON LogsRequestDocumentChange.EventID = UserEventSubscriptions.EventID
	JOIN Requests request ON LogsRequestDocumentChange.RequestID = request.ID
		WHERE
		UserEventSubscriptions.UserID = @UserID
		-- make sure user has permission to the request
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestList(@UserID) fl WHERE fl.ID = LogsRequestDocumentChange.RequestID)
		AND 
		(
			-- not associated with a workflow task
			LogsRequestDocumentChange.TaskID IS NULL
			-- or has permission to view documents for the associated workflow task
			OR (
				EXISTS(
					SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
					WHERE a.PermissionID = 'FAE8FC24-362D-4382-AF31-0933AF95FDE9' AND a.ProjectID = request.ProjectId AND a.RequestTypeID = request.RequestTypeID
					AND EXISTS(SELECT NULL FROM Tasks t WHERE t.ID = LogsRequestDocumentChange.TaskID AND t.WorkflowActivityID = a.WorkflowActivityID)
					AND EXISTS(SELECT NULL FROM SecurityGroupUsers sgu WHERE sgu.SecurityGroupID = a.SecurityGroupID AND sgu.UserID = @UserID)
				)
				AND NOT EXISTS(
					SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
					WHERE a.PermissionID = 'FAE8FC24-362D-4382-AF31-0933AF95FDE9' AND a.ProjectID = request.ProjectId AND a.RequestTypeID = request.RequestTypeID
					AND EXISTS(SELECT NULL FROM Tasks t WHERE t.ID = LogsRequestDocumentChange.TaskID AND t.WorkflowActivityID = a.WorkflowActivityID)
					AND EXISTS(SELECT NULL FROM SecurityGroupUsers sgu WHERE sgu.SecurityGroupID = a.SecurityGroupID AND sgu.UserID = @UserID)
					AND a.Allowed = 0
				)
			)
		)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, 'F9C20001-E0C2-4996-B5CC-A3BF01301150') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = LogsRequestDocumentChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'F9C20001-E0C2-4996-B5CC-A3BF01301150') e JOIN Requests r ON e.OrganizationID = r.OrganizationID WHERE r.ID = LogsRequestDocumentChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, 'F9C20001-E0C2-4996-B5CC-A3BF01301150') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = LogsRequestDocumentChange.RequestID)
		)
		AND 
		(
			(UserEventSubscriptions.Frequency IS NOT NULL)
			OR
			(UserEventSubscriptions.FrequencyForMy IS NOT NULL AND EXISTS(SELECT NULL FROM RequestUsers ru WHERE ru.RequestID = LogsRequestDocumentChange.RequestID AND ru.UserID = UserEventSubscriptions.UserID))
		)

	-- Request Comment Change
	INSERT INTO @notifications (TimeStamp, [Event], [Message])
	SELECT LogsRequestCommentChange.TimeStamp, 'Request Comment Changed', LogsRequestCommentChange.Description FROM LogsRequestCommentChange 
	JOIN UserEventSubscriptions ON LogsRequestCommentChange.EventID = UserEventSubscriptions.EventID
	JOIN Comments comment on LogsRequestCommentChange.CommentID = comment.ID
	JOIN Requests request ON comment.ItemID = request.ID
		WHERE
		UserEventSubscriptions.UserID = @UserID
		-- make sure user has permission to the request
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestList(@UserID) fl WHERE fl.ID = comment.ItemID)
		AND 
		(
			-- not associated with a workflow task
			NOT EXISTS(SELECT NULL FROM CommentReferences cr JOIN Tasks t ON cr.ItemID = t.ID WHERE cr.CommentID = comment.ID)
			-- or has permission to view comments for the associated workflow task
			OR (
				EXISTS(
					SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
					WHERE a.PermissionID = '7025F490-9635-4540-B682-3A4F152E73EF' AND a.ProjectID = request.ProjectId AND a.RequestTypeID = request.RequestTypeID
					AND EXISTS(SELECT NULL FROM Tasks t JOIN CommentReferences cr ON t.ID = cr.ItemID WHERE cr.CommentID = comment.ID AND t.WorkflowActivityID = a.WorkflowActivityID)
					AND EXISTS(SELECT NULL FROM SecurityGroupUsers sgu WHERE sgu.SecurityGroupID = a.SecurityGroupID AND sgu.UserID = @UserID)
				)
				AND NOT EXISTS(
					SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
					WHERE a.PermissionID = '7025F490-9635-4540-B682-3A4F152E73EF' AND a.ProjectID = request.ProjectId AND a.RequestTypeID = request.RequestTypeID
					AND EXISTS(SELECT NULL FROM Tasks t JOIN CommentReferences cr ON t.ID = cr.ItemID WHERE cr.CommentID = comment.ID AND t.WorkflowActivityID = a.WorkflowActivityID)
					AND EXISTS(SELECT NULL FROM SecurityGroupUsers sgu WHERE sgu.SecurityGroupID = a.SecurityGroupID AND sgu.UserID = @UserID)
					AND a.Allowed = 0
				)
			)
		)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, 'E7160001-D933-476E-A706-A43C0137D4E9') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = request.ID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'E7160001-D933-476E-A706-A43C0137D4E9') e JOIN Requests r ON e.OrganizationID = r.OrganizationID WHERE r.ID = request.ID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, 'E7160001-D933-476E-A706-A43C0137D4E9') e WHERE e.UserID = LogsRequestCommentChange.UserID)
		)
		AND 
		(
			(UserEventSubscriptions.Frequency IS NOT NULL)
			OR
			(UserEventSubscriptions.FrequencyForMy IS NOT NULL AND EXISTS(SELECT NULL FROM RequestUsers ru JOIN Requests ON Requests.ID = ru.RequestID JOIN Comments ON Comments.ItemID = Requests.ID WHERE Comments.ID = LogsRequestCommentChange.CommentID AND ru.UserID = UserEventSubscriptions.UserID))
		)


	-- Request Metadata Change
	INSERT INTO @notifications (TimeStamp, [Event], [Message])
	SELECT m.TimeStamp, 'Request Metadata Changed', m.Description FROM LogsRequestMetadataChange m
	JOIN UserEventSubscriptions us ON m.EventID = us.EventID
	WHERE
		us.UserID = @UserID
		-- make sure user has permission to the request
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestList(@UserID) fl WHERE fl.ID = m.RequestID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '29AEE006-1C2A-4304-B3C9-8771D96ACDF1') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = m.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '29AEE006-1C2A-4304-B3C9-8771D96ACDF1') e JOIN Requests r ON e.OrganizationID = r.OrganizationID WHERE r.ID = m.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '29AEE006-1C2A-4304-B3C9-8771D96ACDF1') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = m.RequestID AND EXISTS(SELECT NULL FROM RequestDataMarts dm WHERE dm.RequestID = r.ID AND dm.QueryStatusTypeId <> 6))
		)

	-- Request DataMart Metadata Change
	INSERT INTO @notifications (TimeStamp, [Event], [Message])
	SELECT m.TimeStamp, 'Request DataMart Metadata Change', m.Description FROM LogsRequestDataMartMetadataChange m
	JOIN UserEventSubscriptions us ON m.EventID = us.EventID
	WHERE
		us.UserID = @UserID
		-- make sure user has permission to the request
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestList(@UserID) fl WHERE fl.ID = m.RequestID)
		AND EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '7535EE61-767E-4C36-BF45-6927B9AFE7C6') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = m.RequestID AND EXISTS(SELECT NULL FROM RequestDataMarts dm WHERE dm.RequestID = r.ID AND dm.QueryStatusTypeId <> 6))

	RETURN
END");

        }
        
        public override void Down()
        {

            Sql(@"ALTER FUNCTION [dbo].[GetNotifications]
(
	@UserID uniqueidentifier
)
RETURNS 
@notifications TABLE 
(
	TimeStamp datetimeoffset(7),
	Event nvarchar(max),
	Message nvarchar(max)
)
AS
BEGIN
	--DataMart Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsDataMartChange.TimeStamp, 'DataMart Changed', LogsDataMartChange.Description FROM LogsDataMartChange JOIN UserEventSubscriptions ON LogsDataMartChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventDataMarts(@UserID, '59A90001-539E-4C21-A4F2-A22200CD3C7D') e WHERE e.DataMartID = LogsDataMartChange.DataMartID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '59A90001-539E-4C21-A4F2-A22200CD3C7D') e WHERE EXISTS(SELECT NULL FROM DataMarts WHERE DataMarts.OrganizationID = e.OrganizationID AND DataMarts.ID = LogsDataMartChange.DataMartID))
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '59A90001-539E-4C21-A4F2-A22200CD3C7D'))
		)
	
	--Group Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsGroupChange.TimeStamp, 'Group Changed', LogsGroupChange.Description FROM LogsGroupChange JOIN UserEventSubscriptions ON LogsGroupChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventGroups(@UserID, 'D80E0001-27BC-4FCB-BA75-A22200CD2426') e WHERE e.GroupID = LogsGroupChange.GroupID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, 'D80E0001-27BC-4FCB-BA75-A22200CD2426'))
		)

	--Organization Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsOrganizationChange.TimeStamp, 'Organization Changed', LogsOrganizationChange.Description FROM LogsOrganizationChange JOIN UserEventSubscriptions ON LogsOrganizationChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'B8A50001-B556-43D2-A1B8-A22200CD12DC') e WHERE e.OrganizationID = LogsOrganizationChange.OrganizationID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, 'B8A50001-B556-43D2-A1B8-A22200CD12DC'))
		)

	--Project Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsProjectChange.TimeStamp, 'Project Changed', LogsProjectChange.Description FROM LogsProjectChange JOIN UserEventSubscriptions ON LogsProjectChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '1C090001-9F95-400C-9780-A22200CD0234') e WHERE e.ProjectID = LogsProjectChange.ProjectID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '1C090001-9F95-400C-9780-A22200CD0234'))
		)

	--Registry Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsRegistryChange.TimeStamp, 'Registry Changed', LogsRegistryChange.Description FROM LogsRegistryChange JOIN UserEventSubscriptions ON LogsRegistryChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventRegistries(@UserID, '553FD350-8F3B-40C6-9E31-11D8BC7420A2') e WHERE e.RegistryID = LogsRegistryChange.RegistryID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '553FD350-8F3B-40C6-9E31-11D8BC7420A2'))
		)

	--User Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsUserChange.TimeStamp, 'User Changed', LogsUserChange.Description FROM LogsUserChange JOIN UserEventSubscriptions ON LogsUserChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, 'B7640001-7247-49B8-A818-A22200CCEAF7') e WHERE e.UserID = LogsUserChange.UserID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, 'B7640001-7247-49B8-A818-A22200CCEAF7'))
		)


	--New DataMart Client
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsNewDataMartClient.TimeStamp, 'New DataMart Client', LogsNewDataMartClient.Description FROM LogsNewDataMartClient JOIN UserEventSubscriptions ON LogsNewDataMartClient.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventDataMarts(@UserID, '60F20001-77FF-4F4B-9153-A2220129E466'))
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '60F20001-77FF-4F4B-9153-A2220129E466'))
		)

	--Password Expiration Log
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsPasswordExpiration.TimeStamp, 'Password Expiration', LogsPasswordExpiration.Description FROM LogsPasswordExpiration JOIN UserEventSubscriptions ON LogsPasswordExpiration.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND LogsPasswordExpiration.ExpiringUserID = @UserID
	
	--Profile Updated Log
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsProfileUpdated.TimeStamp, 'Profile Updated', LogsProfileUpdated.Description FROM LogsProfileUpdated JOIN UserEventSubscriptions ON LogsProfileUpdated.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND UserChangedID = @UserID	

	--Registration Submitted
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsUserRegistrationSubmitted.TimeStamp, 'Registration Submitted', LogsUserRegistrationSubmitted.Description FROM LogsUserRegistrationSubmitted JOIN UserEventSubscriptions ON LogsUserRegistrationSubmitted.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID AND LogsUserRegistrationSubmitted.UserID <> LogsUserRegistrationSubmitted.RegisteredUserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, '3AC20001-D8A4-4BE7-957C-A22200CC84BB') e WHERE e.UserID = LogsUserRegistrationSubmitted.UserID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '3AC20001-D8A4-4BE7-957C-A22200CC84BB'))
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '3AC20001-D8A4-4BE7-957C-A22200CC84BB') e JOIN Users u ON e.OrganizationID = u.OrganizationID WHERE u.ID = LogsUserRegistrationSubmitted.UserID)
		)

	--Registration Changed
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsUserRegistrationChanged.TimeStamp, 'Registration Changed', LogsUserRegistrationChanged.Description FROM LogsUserRegistrationChanged JOIN UserEventSubscriptions ON LogsUserRegistrationChanged.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID AND LogsUserRegistrationChanged.UserID <> LogsUserRegistrationChanged.RegisteredUserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, '76B10001-2B49-453C-A8E1-A22200CC9356') e WHERE e.UserID = LogsUserRegistrationChanged.UserID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventGlobal(@UserID, '76B10001-2B49-453C-A8E1-A22200CC9356'))
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '76B10001-2B49-453C-A8E1-A22200CC9356') e JOIN Users u ON e.OrganizationID = u.OrganizationID WHERE u.ID = LogsUserRegistrationChanged.UserID)
		)


	--Results Viewed
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsResponseViewed.TimeStamp, 'Results Viewed', LogsResponseViewed.Description FROM LogsResponseViewed JOIN UserEventSubscriptions ON LogsResponseViewed.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r JOIN RequestDataMarts ON r.ID = RequestDataMarts.RequestID JOIN RequestDataMartResponses ON RequestDataMarts.ID = RequestDataMartResponses.RequestDataMartID WHERE RequestDataMartResponses.ID = LogsResponseViewed.ResponseID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '25EC0001-3AC0-45FB-AF72-A22200CC334C') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID JOIN RequestDataMartResponses ON RequestDataMarts.ID = RequestDataMartResponses.RequestDataMartID WHERE RequestDataMartResponses.ID = LogsResponseViewed.ResponseID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '25EC0001-3AC0-45FB-AF72-A22200CC334C') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID JOIN RequestDataMartResponses ON RequestDataMarts.ID = RequestDataMartResponses.RequestDataMartID WHERE RequestDataMartResponses.ID = LogsResponseViewed.ResponseID)
		)

	--Request Status Changed
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsRequestStatusChange.TimeStamp, 'Request Status Changed', LogsRequestStatusChange.Description FROM LogsRequestStatusChange JOIN UserEventSubscriptions ON LogsRequestStatusChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE r.ID = LogsRequestStatusChange.RequestID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '0A850001-FC8A-4DE2-9AA5-A22200E82398') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID WHERE Requests.ID = LogsRequestStatusChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '0A850001-FC8A-4DE2-9AA5-A22200E82398') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsRequestStatusChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectOrganizations(@UserID, '0A850001-FC8A-4DE2-9AA5-A22200E82398') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsRequestStatusChange.RequestID)
		)
		AND 
		(
			(UserEventSubscriptions.Frequency IS NOT NULL)
			OR
			(UserEventSubscriptions.FrequencyForMy IS NOT NULL AND EXISTS(SELECT NULL FROM RequestUsers ru WHERE ru.RequestID = LogsRequestStatusChange.RequestID AND ru.UserID = UserEventSubscriptions.UserID))
		)	
		

	--Routing Status Changed
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsRoutingStatusChange.TimeStamp, 'Routing Status Changed', LogsRoutingStatusChange.Description FROM LogsRoutingStatusChange JOIN UserEventSubscriptions ON LogsRoutingStatusChange.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r JOIN RequestDataMarts ON r.ID = RequestDataMarts.RequestID WHERE RequestDataMarts.ID = LogsRoutingStatusChange.RequestDataMartID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '5AB90001-8072-42CD-940F-A22200CC24A2') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID WHERE RequestDataMarts.ID = LogsRoutingStatusChange.RequestDataMartID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '5AB90001-8072-42CD-940F-A22200CC24A2') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID WHERE RequestDataMarts.ID = LogsRoutingStatusChange.RequestDataMartID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '5AB90001-8072-42CD-940F-A22200CC24A2') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID AND RequestDataMarts.DataMartID = e.DataMartID WHERE RequestDataMarts.ID = LogsRoutingStatusChange.RequestDataMartID)
		)
		AND 
		(
			(UserEventSubscriptions.Frequency IS NOT NULL)
			OR
			(UserEventSubscriptions.FrequencyForMy IS NOT NULL AND EXISTS(SELECT NULL FROM RequestUsers ru JOIN Requests ON Requests.ID = ru.RequestID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID WHERE RequestDataMarts.ID = LogsRoutingStatusChange.RequestDataMartID AND ru.UserID = UserEventSubscriptions.UserID))
		)

	--Results Reminder
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsResultsReminder.TimeStamp, 'Results Reminder', LogsResultsReminder.Description FROM LogsResultsReminder JOIN UserEventSubscriptions ON LogsResultsReminder.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE r.ID = LogsResultsReminder.RequestID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'E39A0001-A4CA-46B8-B7EF-A22200E72B08') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID  WHERE Requests.ID = LogsResultsReminder.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, 'E39A0001-A4CA-46B8-B7EF-A22200E72B08') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsResultsReminder.RequestID)
		)

	--Submitted Request Awaits Response
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsSubmittedRequestAwaitsResponse.TimeStamp, 'Request Awaits Response', LogsSubmittedRequestAwaitsResponse.Description FROM LogsSubmittedRequestAwaitsResponse JOIN UserEventSubscriptions ON LogsSubmittedRequestAwaitsResponse.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE r.ID = LogsSubmittedRequestAwaitsResponse.RequestID)
		AND EXISTS(SELECT NULL FROM RequestDataMarts WHERE RequestDataMarts.RequestID = LogsSubmittedRequestAwaitsResponse.RequestID AND RequestDataMarts.QueryStatusTypeID = 2) --Must have a routing that's status is Submitted
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '688B0001-1572-41CA-8298-A22200CBD542') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID  WHERE Requests.ID = LogsSubmittedRequestAwaitsResponse.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '688B0001-1572-41CA-8298-A22200CBD542') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsSubmittedRequestAwaitsResponse.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '688B0001-1572-41CA-8298-A22200CBD542') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID AND RequestDataMarts.DataMartID = e.DataMartID WHERE Requests.ID = LogsSubmittedRequestAwaitsResponse.RequestID)
		)


	--Submitted Request Awaits Approval
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsSubmittedRequestNeedsApproval.TimeStamp, 'Request Awaits Approval', LogsSubmittedRequestNeedsApproval.Description FROM LogsSubmittedRequestNeedsApproval JOIN UserEventSubscriptions ON LogsSubmittedRequestNeedsApproval.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE r.ID = LogsSubmittedRequestNeedsApproval.RequestID AND r.Status = 300)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'B7B30001-2704-4A57-A71A-A22200CC1736') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID  WHERE Requests.ID = LogsSubmittedRequestNeedsApproval.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, 'B7B30001-2704-4A57-A71A-A22200CC1736') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsSubmittedRequestNeedsApproval.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectOrganizations(@UserID, 'B7B30001-2704-4A57-A71A-A22200CC1736') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsSubmittedRequestNeedsApproval.RequestID)
		)

	--Uploaded Result needs approval
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsUploadedResultNeedsApproval.TimeStamp, 'Uploaded Result Needs Approval', LogsUploadedResultNeedsApproval.Description FROM LogsUploadedResultNeedsApproval JOIN UserEventSubscriptions ON LogsUploadedResultNeedsApproval.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredResponseList(@UserID) r JOIN RequestDataMarts ON r.RequestDataMartID = RequestDataMarts.ID WHERE r.RequestDataMartID = LogsUploadedResultNeedsApproval.RequestDataMartID AND RequestDataMarts.QueryStatusTypeID = 10)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'F31C0001-6900-4BDB-A03A-A22200CC019C') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID  WHERE RequestDataMarts.ID = LogsUploadedResultNeedsApproval.RequestDataMartID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, 'F31C0001-6900-4BDB-A03A-A22200CC019C') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID  WHERE RequestDataMarts.ID = LogsUploadedResultNeedsApproval.RequestDataMartID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, 'F31C0001-6900-4BDB-A03A-A22200CC019C') e JOIN Requests ON e.ProjectID = Requests.ProjectID JOIN RequestDataMarts ON Requests.ID = RequestDataMarts.RequestID AND RequestDataMarts.DataMartID = e.DataMartID WHERE RequestDataMarts.ID = LogsUploadedResultNeedsApproval.RequestDataMartID)

		)

	-- New Request Draft Submitted
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsNewRequestDraftSubmitted.TimeStamp, 'New Request Draft Submitted', LogsNewRequestDraftSubmitted.Description FROM LogsNewRequestDraftSubmitted JOIN UserEventSubscriptions ON LogsNewRequestDraftSubmitted.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE LogsNewRequestDraftSubmitted.RequestID = r.ID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '6549439E-E3E4-4F4C-92CF-88FB81FF8869') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID WHERE Requests.ID = LogsNewRequestDraftSubmitted.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '6549439E-E3E4-4F4C-92CF-88FB81FF8869') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsNewRequestDraftSubmitted.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '6549439E-E3E4-4F4C-92CF-88FB81FF8869') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsNewRequestDraftSubmitted.RequestID)
		)

	-- New Request submitted
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsNewRequestSubmitted.TimeStamp, 'New Request Submitted', LogsNewRequestSubmitted.Description FROM LogsNewRequestSubmitted JOIN UserEventSubscriptions ON LogsNewRequestSubmitted.EventID = UserEventSubscriptions.EventID 
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(@UserID, null) r WHERE LogsNewRequestSubmitted.RequestID = r.ID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '06E30001-ED86-4427-9936-A22200CC74F0') e JOIN Requests ON e.OrganizationID = Requests.OrganizationID WHERE Requests.ID = LogsNewRequestSubmitted.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '06E30001-ED86-4427-9936-A22200CC74F0') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsNewRequestSubmitted.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '06E30001-ED86-4427-9936-A22200CC74F0') e JOIN Requests ON e.ProjectID = Requests.ProjectID WHERE Requests.ID = LogsNewRequestSubmitted.RequestID)
		)

	-- Task Changed
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsTaskChange.TimeStamp, 'Task Changed', LogsTaskChange.Description FROM LogsTaskChange JOIN UserEventSubscriptions ON LogsTaskChange.EventID = UserEventSubscriptions.EventID
		WHERE
		UserEventSubscriptions.UserID = @UserID
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '2DFE0001-B98D-461D-A705-A3BE01411396') e JOIN Requests r ON e.ProjectID = r.ProjectId JOIN TaskReferences tr ON r.ID = tr.ItemID WHERE tr.TaskID = LogsTaskChange.TaskID AND tr.Type = 2 )
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '2DFE0001-B98D-461D-A705-A3BE01411396') e JOIN Requests r ON e.OrganizationID = r.OrganizationID JOIN TaskReferences tr ON r.ID = tr.ItemID WHERE tr.TaskID = LogsTaskChange.TaskID AND tr.Type = 2)
			OR
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, '2DFE0001-B98D-461D-A705-A3BE01411396') e WHERE e.UserID = LogsTaskChange.UserID)
		)

	-- Request Assignment Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT 
	LogsRequestAssignmentChange.TimeStamp, 
	CASE WHEN RequestUserUserID = @UserID AND Reason = 4 THEN 'Request Assigned to You'
		WHEN RequestUserUserID = @UserID AND Reason = 8 THEN 'Unassigned from Request'
		WHEN RequestUserUserID != @UserID THEN 'Request Assignment Changed'
	END, 
	LogsRequestAssignmentChange.Description FROM LogsRequestAssignmentChange JOIN UserEventSubscriptions ON LogsRequestAssignmentChange.EventID = UserEventSubscriptions.EventID
		WHERE
		UserEventSubscriptions.UserID = @UserID 
		AND (LogsRequestAssignmentChange.WorkflowRoleID != 'B5C4CFA7-2C63-4C8B-A981-4BC5A41AAFC7' 
			AND LogsRequestAssignmentChange.WorkflowRoleID != 'B96BD897-3942-4DF0-888A-5927751E8EF1' 
			AND LogsRequestAssignmentChange.WorkflowRoleID != '06460001-288E-4532-833D-A402011F437A'
			AND LogsRequestAssignmentChange.WorkflowRoleID != 'FDB30001-028D-4E92-9357-A402011F56F4')
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '45DA0001-7E63-4578-9A19-A43B0100F7C8') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = LogsRequestAssignmentChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '45DA0001-7E63-4578-9A19-A43B0100F7C8') e JOIN Requests r ON e.OrganizationID = r.OrganizationID WHERE r.ID = LogsRequestAssignmentChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, '45DA0001-7E63-4578-9A19-A43B0100F7C8') e WHERE e.UserID = LogsRequestAssignmentChange.UserID)
		)
		AND 
		(
			(UserEventSubscriptions.Frequency IS NOT NULL)
			OR
			(UserEventSubscriptions.FrequencyForMy IS NOT NULL AND EXISTS(SELECT NULL FROM RequestUsers ru WHERE ru.UserID = LogsRequestAssignmentChange.RequestUserUserID AND ru.UserID = UserEventSubscriptions.UserID))
		)


	-- Request Document Change
	INSERT INTO @notifications (TimeStamp, Event, Message)
	SELECT LogsRequestDocumentChange.TimeStamp, 'Request Document Changed', LogsRequestDocumentChange.Description FROM LogsRequestDocumentChange 
	JOIN UserEventSubscriptions ON LogsRequestDocumentChange.EventID = UserEventSubscriptions.EventID
	JOIN Requests request ON LogsRequestDocumentChange.RequestID = request.ID
		WHERE
		UserEventSubscriptions.UserID = @UserID
		-- make sure user has permission to the request
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestList(@UserID) fl WHERE fl.ID = LogsRequestDocumentChange.RequestID)
		AND 
		(
			-- not associated with a workflow task
			LogsRequestDocumentChange.TaskID IS NULL
			-- or has permission to view documents for the associated workflow task
			OR (
				EXISTS(
					SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
					WHERE a.PermissionID = 'FAE8FC24-362D-4382-AF31-0933AF95FDE9' AND a.ProjectID = request.ProjectId AND a.RequestTypeID = request.RequestTypeID
					AND EXISTS(SELECT NULL FROM Tasks t WHERE t.ID = LogsRequestDocumentChange.TaskID AND t.WorkflowActivityID = a.WorkflowActivityID)
					AND EXISTS(SELECT NULL FROM SecurityGroupUsers sgu WHERE sgu.SecurityGroupID = a.SecurityGroupID AND sgu.UserID = @UserID)
				)
				AND NOT EXISTS(
					SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
					WHERE a.PermissionID = 'FAE8FC24-362D-4382-AF31-0933AF95FDE9' AND a.ProjectID = request.ProjectId AND a.RequestTypeID = request.RequestTypeID
					AND EXISTS(SELECT NULL FROM Tasks t WHERE t.ID = LogsRequestDocumentChange.TaskID AND t.WorkflowActivityID = a.WorkflowActivityID)
					AND EXISTS(SELECT NULL FROM SecurityGroupUsers sgu WHERE sgu.SecurityGroupID = a.SecurityGroupID AND sgu.UserID = @UserID)
					AND a.Allowed = 0
				)
			)
		)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, 'F9C20001-E0C2-4996-B5CC-A3BF01301150') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = LogsRequestDocumentChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'F9C20001-E0C2-4996-B5CC-A3BF01301150') e JOIN Requests r ON e.OrganizationID = r.OrganizationID WHERE r.ID = LogsRequestDocumentChange.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, 'F9C20001-E0C2-4996-B5CC-A3BF01301150') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = LogsRequestDocumentChange.RequestID)
		)
		AND 
		(
			(UserEventSubscriptions.Frequency IS NOT NULL)
			OR
			(UserEventSubscriptions.FrequencyForMy IS NOT NULL AND EXISTS(SELECT NULL FROM RequestUsers ru WHERE ru.RequestID = LogsRequestDocumentChange.RequestID AND ru.UserID = UserEventSubscriptions.UserID))
		)

	-- Request Comment Change
	INSERT INTO @notifications (TimeStamp, [Event], [Message])
	SELECT LogsRequestCommentChange.TimeStamp, 'Request Comment Changed', LogsRequestCommentChange.Description FROM LogsRequestCommentChange 
	JOIN UserEventSubscriptions ON LogsRequestCommentChange.EventID = UserEventSubscriptions.EventID
	JOIN Comments comment on LogsRequestCommentChange.CommentID = comment.ID
	JOIN Requests request ON comment.ItemID = request.ID
		WHERE
		UserEventSubscriptions.UserID = @UserID
		-- make sure user has permission to the request
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestList(@UserID) fl WHERE fl.ID = comment.ItemID)
		AND 
		(
			-- not associated with a workflow task
			NOT EXISTS(SELECT NULL FROM CommentReferences cr JOIN Tasks t ON cr.ItemID = t.ID WHERE cr.CommentID = comment.ID)
			-- or has permission to view comments for the associated workflow task
			OR (
				EXISTS(
					SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
					WHERE a.PermissionID = '7025F490-9635-4540-B682-3A4F152E73EF' AND a.ProjectID = request.ProjectId AND a.RequestTypeID = request.RequestTypeID
					AND EXISTS(SELECT NULL FROM Tasks t JOIN CommentReferences cr ON t.ID = cr.ItemID WHERE cr.CommentID = comment.ID AND t.WorkflowActivityID = a.WorkflowActivityID)
					AND EXISTS(SELECT NULL FROM SecurityGroupUsers sgu WHERE sgu.SecurityGroupID = a.SecurityGroupID AND sgu.UserID = @UserID)
				)
				AND NOT EXISTS(
					SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
					WHERE a.PermissionID = '7025F490-9635-4540-B682-3A4F152E73EF' AND a.ProjectID = request.ProjectId AND a.RequestTypeID = request.RequestTypeID
					AND EXISTS(SELECT NULL FROM Tasks t JOIN CommentReferences cr ON t.ID = cr.ItemID WHERE cr.CommentID = comment.ID AND t.WorkflowActivityID = a.WorkflowActivityID)
					AND EXISTS(SELECT NULL FROM SecurityGroupUsers sgu WHERE sgu.SecurityGroupID = a.SecurityGroupID AND sgu.UserID = @UserID)
					AND a.Allowed = 0
				)
			)
		)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, 'E7160001-D933-476E-A706-A43C0137D4E9') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = request.ID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, 'E7160001-D933-476E-A706-A43C0137D4E9') e JOIN Requests r ON e.OrganizationID = r.OrganizationID WHERE r.ID = request.ID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventUsers(@UserID, 'E7160001-D933-476E-A706-A43C0137D4E9') e WHERE e.UserID = LogsRequestCommentChange.UserID)
		)
		AND 
		(
			(UserEventSubscriptions.Frequency IS NOT NULL)
			OR
			(UserEventSubscriptions.FrequencyForMy IS NOT NULL AND EXISTS(SELECT NULL FROM RequestUsers ru JOIN Requests ON Requests.ID = ru.RequestID JOIN Comments ON Comments.ItemID = Requests.ID WHERE Comments.ID = LogsRequestCommentChange.CommentID AND ru.UserID = UserEventSubscriptions.UserID))
		)


	-- Request Metadata Change
	INSERT INTO @notifications (TimeStamp, [Event], [Message])
	SELECT m.TimeStamp, 'Request Metadata Changed', m.Description FROM LogsRequestMetadataChange m
	JOIN UserEventSubscriptions us ON m.EventID = us.EventID
	WHERE
		us.UserID = @UserID
		-- make sure user has permission to the request
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestList(@UserID) fl WHERE fl.ID = m.RequestID)
		AND
		(
			EXISTS(SELECT NULL FROM dbo.EventProjects(@UserID, '29AEE006-1C2A-4304-B3C9-8771D96ACDF1') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = m.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventOrganizations(@UserID, '29AEE006-1C2A-4304-B3C9-8771D96ACDF1') e JOIN Requests r ON e.OrganizationID = r.OrganizationID WHERE r.ID = m.RequestID)
			OR
			EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '29AEE006-1C2A-4304-B3C9-8771D96ACDF1') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = m.RequestID AND EXISTS(SELECT NULL FROM RequestDataMarts dm WHERE dm.RequestID = r.ID AND dm.QueryStatusTypeId <> 6))
		)

	-- Request DataMart Metadata Change
	INSERT INTO @notifications (TimeStamp, [Event], [Message])
	SELECT m.TimeStamp, 'Request DataMart Metadata Change', m.Description FROM LogsRequestDataMartMetadataChange m
	JOIN UserEventSubscriptions us ON m.EventID = us.EventID
	WHERE
		us.UserID = @UserID
		-- make sure user has permission to the request
		AND EXISTS(SELECT NULL FROM dbo.FilteredRequestList(@UserID) fl WHERE fl.ID = m.RequestID)
		AND EXISTS(SELECT NULL FROM dbo.EventProjectDataMarts(@UserID, '7535EE61-767E-4C36-BF45-6927B9AFE7C6') e JOIN Requests r ON e.ProjectID = r.ProjectId WHERE r.ID = m.RequestID AND EXISTS(SELECT NULL FROM RequestDataMarts dm WHERE dm.RequestID = r.ID AND dm.QueryStatusTypeId <> 6))

	RETURN
END");

        }
    }
}
