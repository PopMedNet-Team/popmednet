namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGetWorkflowHistoryTVF : DbMigration
    {
        public override void Up()
        {
            //update inclusion of document, comment, and request user assignments logs

            Sql(@"ALTER FUNCTION [dbo].[GetWorkflowHistory]
(
	@RequestID uniqueidentifier
)
RETURNS 
@items TABLE 
(
	TaskID uniqueidentifier NOT NULL,
	TaskName nvarchar(255),
	UserID uniqueidentifier NOT NULL,
	UserName nvarchar(50),
	UserFullName nvarchar(255),
	[Message] nvarchar(max),
	[Date] datetimeoffset(7) NOT NULL,
	RoutingID uniqueidentifier,
	DataMart nvarchar(255)
)
AS
BEGIN

DECLARE @emptyID uniqueidentifier = '00000000-0000-0000-0000-000000000000'

	-- New request submitted
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
	FROM LogsNewRequestSubmitted l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
	WHERE l.RequestID = @RequestID

-- request status changed
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
	FROM LogsRequestStatusChange l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
	WHERE l.RequestID = @RequestID

-- routing status changed
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], RoutingID, DataMart)
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], dm.DataMartID, (datamartOrg.Name + '/' + d.Name) as DataMart
	FROM LogsRoutingStatusChange l INNER JOIN RequestDataMarts dm ON l.RequestDataMartID = dm.ID INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID 
		LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID LEFT OUTER JOIN DataMarts d ON dm.DataMartID = d.ID LEFT OUTER JOIN Organizations datamartOrg ON d.OrganizationID = datamartOrg.ID
	WHERE dm.RequestID = @RequestID

---- submitted request awaits response !!don't think we need this event
--INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
--	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
--	FROM LogsSubmittedRequestAwaitsResponse l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
--	WHERE l.RequestID = @RequestID

-- uploaded result needs approval
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], RoutingID, DataMart)
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], dm.DataMartID, (datamartOrg.Name + '/' + d.Name) as DataMart
	FROM LogsUploadedResultNeedsApproval l INNER JOIN RequestDataMarts dm ON l.RequestDataMartID = dm.ID INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID 
		LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID LEFT OUTER JOIN DataMarts d ON dm.DataMartID = d.ID LEFT OUTER JOIN Organizations datamartOrg ON d.OrganizationID = datamartOrg.ID
	WHERE dm.RequestID = @RequestID

-- workflow activity for a request changed
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
	FROM LogsRequestWorkflowActivityChange l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
	WHERE l.RequestID = @RequestID

-- task change
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
	FROM LogsTaskChange l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
	WHERE EXISTS(SELECT NULL FROM TaskReferences tr WHERE tr.ItemID = @RequestID AND tr.TaskID = l.TaskID)
	
-- documents can be associated to either the request directly (request overall documents, or task specific documents)
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT COALESCE(t.ID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
	FROM LogsDocumentChange l INNER JOIN Documents d ON l.DocumentID = d.ID
	INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID
	LEFT OUTER JOIN Tasks t ON d.ItemID = t.ID
	WHERE EXISTS(SELECT NULL FROM TaskReferences tr WHERE tr.ItemID = @RequestID AND tr.TaskID = d.ItemID) OR EXISTS(SELECT NULL FROM Requests r WHERE r.ID = d.ItemID AND r.ID = @RequestID)

-- request comments - not associated with a task
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT @emptyID as TaskID, null as TaskName, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description AS [Message], l.TimeStamp as [Date]
	FROM LogsRequestCommentChange  l
	JOIN Comments c ON l.CommentID = c.ID
	JOIN Users u ON l.UserID = u.ID
	LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID	
	WHERE c.ItemID = @RequestID 
	AND NOT EXISTS(SELECT NULL FROM CommentReferences cr WHERE cr.CommentID = l.CommentID AND cr.Type = 1)

-- task comments - associated to a task for the workflow
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT COALESCE(t.ID, @emptyID) as TaskID, t.Subject as TaskName, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description AS [Message], l.TimeStamp as [Date]
	FROM LogsRequestCommentChange  l
	JOIN Comments c ON l.CommentID = c.ID
	JOIN CommentReferences cr ON (c.ID = cr.CommentID AND cr.Type = 1)
	JOIN Users u ON l.UserID = u.ID
	LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID
	LEFT OUTER JOIN Tasks t ON cr.ItemID = t.ID
	WHERE c.ItemID = @RequestID

-- request user assignments
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT @emptyID AS TaskID, null as TaskName, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName,
	l.Description AS [Message], l.TimeStamp as [Date]
	FROM LogsRequestAssignmentChange l
	JOIN Users u ON l.UserID = u.ID
	JOIN Organizations o ON u.OrganizationID = o.ID
	WHERE l.RequestID = @RequestID

	RETURN
END");
        }
        
        public override void Down()
        {
            //revert to the previous version of the function
            Sql(@"ALTER FUNCTION [dbo].[GetWorkflowHistory]
(
	@RequestID uniqueidentifier
)
RETURNS 
@items TABLE 
(
	TaskID uniqueidentifier NOT NULL,
	TaskName nvarchar(255),
	UserID uniqueidentifier NOT NULL,
	UserName nvarchar(50),
	UserFullName nvarchar(255),
	[Message] nvarchar(max),
	[Date] datetimeoffset(7) NOT NULL,
	RoutingID uniqueidentifier,
	DataMart nvarchar(255)
)
AS
BEGIN

DECLARE @emptyID uniqueidentifier = '00000000-0000-0000-0000-000000000000'

	-- New request submitted
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
	FROM LogsNewRequestSubmitted l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
	WHERE l.RequestID = @RequestID

-- request status changed
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
	FROM LogsRequestStatusChange l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
	WHERE l.RequestID = @RequestID

-- routing status changed
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], RoutingID, DataMart)
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], dm.DataMartID, (datamartOrg.Name + '/' + d.Name) as DataMart
	FROM LogsRoutingStatusChange l INNER JOIN RequestDataMarts dm ON l.RequestDataMartID = dm.ID INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID 
		LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID LEFT OUTER JOIN DataMarts d ON dm.DataMartID = d.ID LEFT OUTER JOIN Organizations datamartOrg ON d.OrganizationID = datamartOrg.ID
	WHERE dm.RequestID = @RequestID

---- submitted request awaits response !!don't think we need this event
--INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
--	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
--	FROM LogsSubmittedRequestAwaitsResponse l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
--	WHERE l.RequestID = @RequestID

-- uploaded result needs approval
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], RoutingID, DataMart)
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], dm.DataMartID, (datamartOrg.Name + '/' + d.Name) as DataMart
	FROM LogsUploadedResultNeedsApproval l INNER JOIN RequestDataMarts dm ON l.RequestDataMartID = dm.ID INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID 
		LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID LEFT OUTER JOIN DataMarts d ON dm.DataMartID = d.ID LEFT OUTER JOIN Organizations datamartOrg ON d.OrganizationID = datamartOrg.ID
	WHERE dm.RequestID = @RequestID

-- workflow activity for a request changed
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
	FROM LogsRequestWorkflowActivityChange l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
	WHERE l.RequestID = @RequestID

-- task change
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
	FROM LogsTaskChange l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
	WHERE EXISTS(SELECT NULL FROM TaskReferences tr WHERE tr.ItemID = @RequestID AND tr.TaskID = l.TaskID)

-- document change
INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
	SELECT COALESCE(t.ID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
	FROM LogsDocumentChange l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID
	INNER JOIN Documents d ON l.DocumentID = d.ID INNER JOIN Tasks t ON d.ItemID = t.ID
	WHERE EXISTS(SELECT NULL FROM TaskReferences tr WHERE tr.ItemID = @RequestID AND tr.TaskID = d.ItemID)

	RETURN
END");
        }
    }
}
