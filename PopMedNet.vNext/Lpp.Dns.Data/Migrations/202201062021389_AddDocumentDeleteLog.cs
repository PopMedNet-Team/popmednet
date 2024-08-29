namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentDeleteLog : DbMigration
    {
        public override void Up()
        {
			DropPrimaryKey("LogsDocumentChange");
			CreateTable(
				"LogsDeletedDocumentArchive",
				c => new
				{
					UserID = c.Guid(nullable: false),
					TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
					DocumentID = c.Guid(nullable: false),
					ItemID = c.Guid(nullable: false),
					EventID = c.Guid(nullable: false),
					Description = c.String(),
				})
				.Index(t => t.EventID);
			//create the primary key on the AuditLogs filegroup
			Sql("ALTER TABLE dbo.LogsDeletedDocumentArchive ADD CONSTRAINT PK_LogsDeletedDocumentArchive PRIMARY KEY (UserID, TimeStamp) ON AuditLogs");

			AddColumn("LogsDocumentChange", "ItemID", c => c.Guid(nullable: false));
			Sql(@"ALTER TABLE dbo.LogsDocumentChange ADD CONSTRAINT PK_LogsDocumentChange PRIMARY KEY (UserID, TimeStamp, DocumentID, ItemID) ON AuditLogs");

			// Copy ItemIds from existing documents
			Sql(@"UPDATE LogsDocumentChange
                    SET ItemId = (SELECT ItemId
                                    FROM Documents
                                    WHERE LogsDocumentChange.DocumentID = Documents.ID);");

			// Add trigger to LogsDocumentChange to copy rows to LogsDeletedDocumentArchive when deleted
			Sql(@"IF EXISTS (SELECT 1 FROM sys.triggers
			    WHERE Name = 'LogsDocumentChange_Delete')
                BEGIN
	                DROP TRIGGER LogsDocumentChange_Delete
                END
                GO

                CREATE TRIGGER LogsDocumentChange_Delete 
                   ON  dbo.LogsDocumentChange 
                   FOR DELETE
                AS 
                BEGIN
	                -- SET NOCOUNT ON added to prevent extra result sets from
	                -- interfering with SELECT statements.
	                SET NOCOUNT ON;

		                DECLARE 
			                @userId uniqueIdentifier, 
			                @documentId uniqueIdentifier, 
			                @ts DateTimeOffset, 
			                @description nvarchar(max),
			                @itemId uniqueIdentifier,
                            @eventId uniqueIdentifier
		                SELECT
			                @userId = d.UserID
			                ,@ts = d.TimeStamp
			                ,@documentId = d.DocumentID
			                ,@description = d.Description
			                ,@itemId = d.ItemId
                            ,@eventId = d.EventId
			                FROM deleted d

		                INSERT INTO LogsDeletedDocumentArchive (
			                [UserID]		
			                ,[TimeStamp]
			                ,[DocumentID]
			                ,[Description]
                            ,[eventId]
			                ,[ItemID])		
			                VALUES(
				                @userId
				                ,@ts
				                ,@documentId
				                ,@description
                                ,@eventId
				                ,@itemId)
	                END");

			// Update GetWorkflowHistory function
			Sql(@"ALTER FUNCTION [dbo].[GetWorkflowHistory]
				(
					@RequestID uniqueidentifier,
					@UserID uniqueidentifier
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
					DataMart nvarchar(255),
					WorkflowActivityID uniqueidentifier
				)
				AS
				BEGIN

				DECLARE @emptyID uniqueidentifier = '00000000-0000-0000-0000-000000000000'
				DECLARE @viewTaskID uniqueidentifier = 'DD20EE1B-C433-49F8-8A91-76AD10DB1BEC'
				DECLARE @RequestTypeID uniqueidentifier = (SELECT TOP 1 RequestTypeID FROM Requests WHERE ID = @RequestID)
				DECLARE @ProjectID uniqueidentifier = (SELECT TOP 1 ProjectId FROM Requests WHERE ID = @RequestID)

					-- New request submitted
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], t.WorkflowActivityID as WorkflowActivityID 
					FROM LogsNewRequestSubmitted l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
					WHERE l.RequestID = @RequestID
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

				-- request status changed
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], t.WorkflowActivityID as WorkflowActivityID 
					FROM LogsRequestStatusChange l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
					WHERE l.RequestID = @RequestID
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

				-- routing status changed
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], RoutingID, DataMart, WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], dm.DataMartID, (datamartOrg.Name + '/' + d.Name) as DataMart, t.WorkflowActivityID as WorkflowActivityID
					FROM LogsRoutingStatusChange l INNER JOIN RequestDataMarts dm ON l.RequestDataMartID = dm.ID INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID 
						LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID LEFT OUTER JOIN DataMarts d ON dm.DataMartID = d.ID LEFT OUTER JOIN Organizations datamartOrg ON d.OrganizationID = datamartOrg.ID
					WHERE dm.RequestID = @RequestID
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

				--DataMart added or removed
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], RoutingID, DataMart, WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.Timestamp as [Date], dm.DataMartID, (dataMartOrg.Name + '/' + d.Name) as DataMart, t.WorkflowActivityID as WorkflowActivityID
					FROM LogsRequestDataMartAddedRemoved l INNER JOIN RequestDataMarts dm ON l.RequestDatamartID = dm.ID INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID
					LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID LEFT OUTER JOIN DataMarts d ON dm.DataMartID = d.ID LEFT OUTER JOIN Organizations datamartOrg ON d.OrganizationID = datamartOrg.ID
					WHERE dm.RequestID = @RequestID

				---- submitted request awaits response !!don't think we need this event
				--INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
				--	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
				--	FROM LogsSubmittedRequestAwaitsResponse l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
				--	WHERE l.RequestID = @RequestID

				-- uploaded result needs approval
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], RoutingID, DataMart, WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], dm.DataMartID, (datamartOrg.Name + '/' + d.Name) as DataMart, t.WorkflowActivityID as WorkflowActivityID
					FROM LogsUploadedResultNeedsApproval l INNER JOIN RequestDataMarts dm ON l.RequestDataMartID = dm.ID INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID 
						LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID LEFT OUTER JOIN DataMarts d ON dm.DataMartID = d.ID LEFT OUTER JOIN Organizations datamartOrg ON d.OrganizationID = datamartOrg.ID
					WHERE dm.RequestID = @RequestID
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

				-- task change
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], t.WorkflowActivityID as WorkflowActivityID 
					FROM LogsTaskChange l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
					WHERE EXISTS(SELECT NULL FROM TaskReferences tr WHERE tr.ItemID = @RequestID AND tr.TaskID = l.TaskID)
					AND EXISTS(
						SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
						WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
						AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
					)
	
				-- documents can be associated to either the request directly (request overall documents, or task specific documents)
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], WorkflowActivityID)
					SELECT COALESCE(t.ID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], t.WorkflowActivityID as WorkflowActivityID 
					FROM LogsDocumentChange l INNER JOIN Documents d ON l.DocumentID = d.ID
					INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID
					LEFT OUTER JOIN Tasks t ON d.ItemID = t.ID
					WHERE EXISTS(SELECT NULL FROM TaskReferences tr WHERE tr.ItemID = @RequestID AND tr.TaskID = d.ItemID) OR EXISTS(SELECT NULL FROM Requests r WHERE r.ID = d.ItemID AND r.ID = @RequestID)
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

				-- deleted document log items
				INSERT INTO @items (TaskID, 
					TaskName, 
					UserID, 
					UserName, 
					UserFullName, 
					[Message], 
					[Date], 
					WorkflowActivityID)
				SELECT 
					COALESCE(t.ID, @emptyID) as TaskID, 
					t.Subject,
					l.UserID,
					u.Username,
					(u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName,
					l.description,
					l.TimeStamp,
					t.WorkflowActivityID as WorkflowActivityID
				FROM LogsDeletedDocumentArchive l
					INNER JOIN Users u on l.userid = u.ID
					INNER JOIN Organizations o on u.OrganizationID = o.ID
					LEFT OUTER JOIN Tasks t ON l.ItemID = t.ID
					WHERE EXISTS(SELECT NULL 
							FROM TaskReferences tr 
							WHERE tr.ItemID = @RequestID 
										AND tr.TaskID = l.ItemID) 
					OR EXISTS(SELECT NULL 
						FROM Requests r 
						WHERE r.ID = l.ItemID 
						AND r.ID = @RequestID)
					AND ( t.ID IS NULL 
							OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID 
									AND a.RequestTypeID = @RequestTypeID 
									AND a.ProjectID = @ProjectId 
									AND a.WorkflowActivityID = t.WorkflowActivityID
									AND a.Allowed = 1 
									AND EXISTS(SELECT NULL 
													FROM SecurityGroupUsers 
													WHERE SecurityGroupID = a.SecurityGroupID 
														AND UserID = @UserID )
						)
					)

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
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], WorkflowActivityID)
					SELECT COALESCE(t.ID, @emptyID) as TaskID, t.Subject as TaskName, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description AS [Message], l.TimeStamp as [Date], t.WorkflowActivityID as WorkflowActivityID
					FROM LogsRequestCommentChange  l
					JOIN Comments c ON l.CommentID = c.ID
					JOIN CommentReferences cr ON (c.ID = cr.CommentID AND cr.Type = 1)
					JOIN Users u ON l.UserID = u.ID
					LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID
					LEFT OUTER JOIN Tasks t ON cr.ItemID = t.ID
					WHERE c.ItemID = @RequestID
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

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
			// Downgrade GetWorkflowHistory function
			Sql(@"ALTER FUNCTION [dbo].[GetWorkflowHistory]
				(
					@RequestID uniqueidentifier,
					@UserID uniqueidentifier
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
					DataMart nvarchar(255),
					WorkflowActivityID uniqueidentifier
				)
				AS
				BEGIN

				DECLARE @emptyID uniqueidentifier = '00000000-0000-0000-0000-000000000000'
				DECLARE @viewTaskID uniqueidentifier = 'DD20EE1B-C433-49F8-8A91-76AD10DB1BEC'
				DECLARE @RequestTypeID uniqueidentifier = (SELECT TOP 1 RequestTypeID FROM Requests WHERE ID = @RequestID)
				DECLARE @ProjectID uniqueidentifier = (SELECT TOP 1 ProjectId FROM Requests WHERE ID = @RequestID)

					-- New request submitted
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], t.WorkflowActivityID as WorkflowActivityID 
					FROM LogsNewRequestSubmitted l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
					WHERE l.RequestID = @RequestID
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

				-- request status changed
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], t.WorkflowActivityID as WorkflowActivityID 
					FROM LogsRequestStatusChange l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
					WHERE l.RequestID = @RequestID
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

				-- routing status changed
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], RoutingID, DataMart, WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], dm.DataMartID, (datamartOrg.Name + '/' + d.Name) as DataMart, t.WorkflowActivityID as WorkflowActivityID
					FROM LogsRoutingStatusChange l INNER JOIN RequestDataMarts dm ON l.RequestDataMartID = dm.ID INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID 
						LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID LEFT OUTER JOIN DataMarts d ON dm.DataMartID = d.ID LEFT OUTER JOIN Organizations datamartOrg ON d.OrganizationID = datamartOrg.ID
					WHERE dm.RequestID = @RequestID
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

				--DataMart added or removed
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], RoutingID, DataMart, WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.Timestamp as [Date], dm.DataMartID, (dataMartOrg.Name + '/' + d.Name) as DataMart, t.WorkflowActivityID as WorkflowActivityID
					FROM LogsRequestDataMartAddedRemoved l INNER JOIN RequestDataMarts dm ON l.RequestDatamartID = dm.ID INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID
					LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID LEFT OUTER JOIN DataMarts d ON dm.DataMartID = d.ID LEFT OUTER JOIN Organizations datamartOrg ON d.OrganizationID = datamartOrg.ID
					WHERE dm.RequestID = @RequestID

				---- submitted request awaits response !!don't think we need this event
				--INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date])
				--	SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date] 
				--	FROM LogsSubmittedRequestAwaitsResponse l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
				--	WHERE l.RequestID = @RequestID

				-- uploaded result needs approval
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], RoutingID, DataMart, WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], dm.DataMartID, (datamartOrg.Name + '/' + d.Name) as DataMart, t.WorkflowActivityID as WorkflowActivityID
					FROM LogsUploadedResultNeedsApproval l INNER JOIN RequestDataMarts dm ON l.RequestDataMartID = dm.ID INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID 
						LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID LEFT OUTER JOIN DataMarts d ON dm.DataMartID = d.ID LEFT OUTER JOIN Organizations datamartOrg ON d.OrganizationID = datamartOrg.ID
					WHERE dm.RequestID = @RequestID
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

				-- task change
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], WorkflowActivityID)
					SELECT COALESCE(l.TaskID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], t.WorkflowActivityID as WorkflowActivityID 
					FROM LogsTaskChange l INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID LEFT OUTER JOIN Tasks t ON l.TaskID = t.ID
					WHERE EXISTS(SELECT NULL FROM TaskReferences tr WHERE tr.ItemID = @RequestID AND tr.TaskID = l.TaskID)
					AND EXISTS(
						SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
						WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
						AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
					)
	
				-- documents can be associated to either the request directly (request overall documents, or task specific documents)
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], WorkflowActivityID)
					SELECT COALESCE(t.ID, @emptyID) as TaskID, t.Subject, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description, l.TimeStamp as [Date], t.WorkflowActivityID as WorkflowActivityID 
					FROM LogsDocumentChange l INNER JOIN Documents d ON l.DocumentID = d.ID
					INNER JOIN Users u ON l.UserID = u.ID LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID
					LEFT OUTER JOIN Tasks t ON d.ItemID = t.ID
					WHERE EXISTS(SELECT NULL FROM TaskReferences tr WHERE tr.ItemID = @RequestID AND tr.TaskID = d.ItemID) OR EXISTS(SELECT NULL FROM Requests r WHERE r.ID = d.ItemID AND r.ID = @RequestID)
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

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
				INSERT INTO @items (TaskID, TaskName, UserID, UserName, UserFullName, [Message], [Date], WorkflowActivityID)
					SELECT COALESCE(t.ID, @emptyID) as TaskID, t.Subject as TaskName, l.UserID, u.Username, (u.FirstName + ' ' + u.LastName + ' (' + o.Name + ')') as UserFullName, l.Description AS [Message], l.TimeStamp as [Date], t.WorkflowActivityID as WorkflowActivityID
					FROM LogsRequestCommentChange  l
					JOIN Comments c ON l.CommentID = c.ID
					JOIN CommentReferences cr ON (c.ID = cr.CommentID AND cr.Type = 1)
					JOIN Users u ON l.UserID = u.ID
					LEFT OUTER JOIN Organizations o ON u.OrganizationID = o.ID
					LEFT OUTER JOIN Tasks t ON cr.ItemID = t.ID
					WHERE c.ItemID = @RequestID
					AND ( t.ID IS NULL OR EXISTS(
								SELECT NULL FROM AclProjectRequestTypeWorkflowActivities a 
								WHERE a.PermissionID = @viewTaskID AND a.RequestTypeID = @RequestTypeID AND a.ProjectID = @ProjectId AND a.WorkflowActivityID = t.WorkflowActivityID
								AND a.Allowed = 1 AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = a.SecurityGroupID AND UserID = @UserID )
							)
						)

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
			// Drop delete trigger
			Sql(@"IF EXISTS (SELECT 1 FROM sys.triggers
			    WHERE Name = 'LogsDocumentChange_Delete')
                BEGIN
	                DROP TRIGGER LogsDocumentChange_Delete
                END
                GO");
			DropIndex("LogsDeletedDocumentArchive", new[] { "EventID" });
			DropPrimaryKey("LogsDocumentChange", "PK_LogsDocumentChange");
			DropColumn("LogsDocumentChange", "ItemID");
			DropTable("LogsDeletedDocumentArchive");
			AddPrimaryKey("LogsDocumentChange", new[] { "UserID", "TimeStamp", "DocumentID" });
		}
    }
}
