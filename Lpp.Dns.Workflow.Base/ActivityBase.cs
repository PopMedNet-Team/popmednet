using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Logging;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;
using System.Data.Entity;
using Lpp.Utilities;
using System.Security;

namespace Lpp.Dns.Workflow
{
    /// <summary>
    /// Base workflow activity implementation.
    /// </summary>
    /// <typeparam name="TEntity">The root entity the activity is working from.</typeparam>
    public abstract class ActivityBase<TEntity> : IActivity<DataContext, TEntity>
        where TEntity : Request
    {
        /// <summary>
        /// The workflow instance.
        /// </summary>
        protected Workflow<DataContext, TEntity> _workflow = null;
        /// <summary>
        /// The current datacontext instance.
        /// </summary>
        protected DataContext db = null;
        /// <summary>
        /// The current root entity instance.
        /// </summary>
        protected TEntity _entity = null;
        /// <summary>
        /// A workflow activity logger instance.
        /// </summary>
        protected readonly RequestLogConfiguration _requestLogger = new Lpp.Dns.Data.RequestLogConfiguration();
        /// <summary>
        /// Gets the ID of the workflow activity.
        /// </summary>
        public abstract Guid ID { get; }
        /// <summary>
        /// Gets the display name of the current activity.
        /// </summary>
        public abstract string ActivityName { get; }
        /// <summary>
        /// Gets the uri of the edit/view page for the entity.
        /// </summary>
        public abstract string Uri { get; }
        /// <summary>
        /// Gets subject to set for the activities task, by default is null which will set the subject to the Activity Name. If not null or empty the specified value will be used instead.
        /// </summary>
        public virtual string CustomTaskSubject
        {
            get { return null; }
        }
        /// <summary>
        /// Initializes the activity implementation instance.
        /// </summary>
        /// <param name="workflow"></param>
        public virtual void Initialize(Workflow<DataContext, TEntity> workflow)
        {
            this._workflow = workflow;
            this.db = (DataContext)workflow.DataContext;
            this._entity = workflow.Entity;
            
        }
        /// <summary>
        /// Performs tasks associated to the start of the activity. By default confirms that an active PmnTask has been created for the current activity, and also records any comment entered when leaving the previous activity.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public virtual async Task Start(string comment)
        {
            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ID, db);
            if (task == null)
            {
                task = db.Actions.Add(PmnTask.CreateForWorkflowActivity(_entity.ID, ID, _workflow.ID, db, CustomTaskSubject));
                await db.SaveChangesAsync();
            }

            if (!string.IsNullOrWhiteSpace(comment))
            {
                var cmt = db.Comments.Add(new Comment
                {
                    CreatedByID = _workflow.Identity.ID,
                    ItemID = _entity.ID,
                    Text = comment
                });

                db.CommentReferences.Add(new CommentReference
                {
                    CommentID = cmt.ID,
                    Type = DTO.Enums.CommentItemTypes.Task,
                    ItemTitle = task.Subject,
                    ItemID = task.ID
                });

                await db.SaveChangesAsync();
            }
        }
     
        /// <summary>
        /// Returns a boolean based on user's Approve/Reject Submission rights
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected virtual async Task<bool> ApproveRejectSubmission()
        {
            //Locations: Global, Organizations, Projects, Users, Project Organizations
            var globalAcls = db.GlobalAcls.FilterAcl(_workflow.Identity, PermissionIdentifiers.Request.ApproveRejectSubmission);
            var organizationAcls = db.OrganizationAcls.FilterAcl(_workflow.Identity, PermissionIdentifiers.Request.ApproveRejectSubmission);
            var projectAcls = db.ProjectAcls.FilterAcl(_workflow.Identity, PermissionIdentifiers.Request.ApproveRejectSubmission);
            var userAcls = db.UserAcls.FilterAcl(_workflow.Identity, PermissionIdentifiers.Request.ApproveRejectSubmission);
            var projectOrgAcls = db.ProjectOrganizationAcls.FilterAcl(_workflow.Identity, PermissionIdentifiers.Request.ApproveRejectSubmission);


            var results = await (from r in db.Secure<Request>(_workflow.Identity)
                                 where r.ID == _entity.ID
                                 let gAcls = globalAcls
                                 let oAcls = organizationAcls.Where(a => a.OrganizationID == r.OrganizationID)
                                 let pAcls = projectAcls.Where(a => a.ProjectID == r.ProjectID)
                                 let uAcls = userAcls.Where(a => a.UserID == r.SubmittedByID)
                                 let poAcls = projectOrgAcls.Where(a => a.ProjectID == r.ProjectID && a.OrganizationID == r.OrganizationID)
                                 where (
                                   (gAcls.Any() || oAcls.Any() || pAcls.Any() || uAcls.Any() || poAcls.Any())
                                   &&
                                   (gAcls.All(a => a.Allowed) && oAcls.All(a => a.Allowed) && pAcls.All(a => a.Allowed) && uAcls.All(a => a.Allowed) && poAcls.All(a => a.Allowed))
                                 )
                                 select r.ID).AnyAsync();

            return results;
        }

        /// <summary>
        /// Performs validation prior to executing Complete.
        /// </summary>
        /// <param name="activityResultID"></param>
        /// <returns></returns>
        public abstract Task<ValidationResult> Validate(Guid? activityResultID);

        /// <summary>
        /// Executes actions based on the specified activity result.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="activityResultID"></param>
        /// <returns></returns>
        public abstract Task<CompletionResult> Complete(string data, Guid? activityResultID);

        /// <summary>
        /// Creates immediate notifications notifications and sends.
        /// </summary>
        /// <param name="logger">The workflow activity logger.</param>
        /// <param name="logs"></param>
        /// <returns></returns>
        protected async Task ProcessNotifications(IEnumerable<AuditLog> logs)
        {
            await Task.Run(() => {

                List<Notification> notifications = new List<Notification>();

                foreach (Lpp.Dns.Data.Audit.RequestStatusChangedLog logitem in logs)
                {
                    var items = _requestLogger.CreateNotifications(logitem, db, true);
                    if (items != null && items.Any())
                        notifications.AddRange(items);
                }

                if (notifications.Any())
                    _requestLogger.SendNotification(notifications);
            });
        }

        /// <summary>
        /// Performs common submit validation of a request.
        /// </summary>
        /// <returns>Any error messages, empty if valid.</returns>
        protected async Task<string> PerformSubmitValidation()
        {
            var errors = new StringBuilder();
            if (_entity.ProjectID == null)
                errors.AppendHtmlLine("Please ensure that you have selected a project for the request.");

            if (_entity.DueDate.HasValue && _entity.DueDate.Value < DateTime.UtcNow)
                errors.AppendHtmlLine("The Request Due Date must be set in the future.");

            var dataMartsDueDate = false;
            foreach (var dm in _entity.DataMarts)
            {
                if (dm.DueDate.HasValue && dm.DueDate.Value.Date < DateTime.UtcNow.Date)
                    dataMartsDueDate = true;
            }
            if (dataMartsDueDate)
                errors.AppendHtmlLine("The Request's DataMart Due Dates must be set in the future.");

            if (_entity.SubmittedOn.HasValue && _entity.Status != Lpp.Dns.DTO.Enums.RequestStatuses.RequestRejected) //Rejected requests can be re-submitted.
                errors.AppendHtmlLine("Cannot submit a request that has already been submitted");

            if (_entity.Template)
                errors.AppendHtmlLine("Cannot submit a request template");

            if (_entity.Scheduled)
                errors.AppendHtmlLine("Cannot submit a scheduled request");

            await db.LoadReference(_entity, (r) => r.Project);
            await db.LoadCollection(_entity, (r) => r.DataMarts);

            //If a project loaded successfully check it.
            if (_entity.Project != null)
            {
                if (!_entity.Project.Active || _entity.Project.Deleted)
                    errors.AppendHtmlLine("Cannot submit a request for an inactive or deleted project.");

                if (_entity.Project.EndDate < DateTime.UtcNow)
                    errors.AppendHtmlLine("Cannot submit a request for a project that has ended.");

                await db.LoadCollection(_entity.Project, (p) => p.DataMarts);

                if (_entity.DataMarts.Any(dm => !_entity.Project.DataMarts.Any(pdm => pdm.DataMartID == dm.DataMartID)))
                    errors.AppendHtmlLine("The request contains datamarts that are not part of the project specified and thus cannot be processed. Please remove these datamarts and try again.");
            }


            var dataMarts = await GetAllAvailableDataMarts();

            if (_entity.DataMarts.Any(dm => !dataMarts.Any(gdm => gdm == dm.DataMartID)))
                errors.AppendHtmlLine("This request contains datamarts you are not permitted to submit to. Please remove them and try again.");


            var filters = new ExtendedQuery
            {
                Projects = (a) => a.ProjectID == _entity.ProjectID,
                ProjectOrganizations = a => a.ProjectID == _entity.ProjectID && a.OrganizationID == _entity.OrganizationID,
                Organizations = a => a.OrganizationID == _entity.OrganizationID,
                Users = a => a.UserID == _entity.CreatedByID
            };

            if (_entity.DataMarts.Count < 2)
            {
                var skip2DataMartRulePerms = await db.HasGrantedPermissions<Request>(_workflow.Identity, _entity, filters, PermissionIdentifiers.Portal.SkipTwoDataMartRule);

                if (!skip2DataMartRulePerms.Contains(PermissionIdentifiers.Portal.SkipTwoDataMartRule))
                    errors.AppendHtmlLine("Cannot submit a request with less than 2 datamarts");
            }

            return errors.ToString();
        }

        /// <summary>
        /// Marks the specified task as complete and saves.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected async Task MarkTaskComplete(PmnTask task)
        {
            task.Status = DTO.Enums.TaskStatuses.Complete;
            task.EndOn = DateTime.UtcNow;
            task.PercentComplete = 100d;

            await db.SaveChangesAsync();
        }


        /// <summary>
        /// Manually set the status of the request using a direct sql command, the status on the entity is also set and a save on the datacontext called. By default the entity is refreshed after the actions have been completed.
        /// </summary>
        /// <param name="newStatus">The status to set.</param>
        /// <param name="refreshEntity">If true the entity will be reloaded.</param>
        /// <returns></returns>
        protected async Task SetRequestStatus(DTO.Enums.RequestStatuses newStatus, bool refreshEntity = true)
        {
            //if we do not set and and save the status the entity logger will not fire based on status changes correctly
            _entity.Status = newStatus;
            await db.SaveChangesAsync();

            //manually override the request status using sql direct, EF does not allow update of computed
            await db.Database.ExecuteSqlCommandAsync("UPDATE Requests SET Status = @status WHERE ID = @ID", new System.Data.SqlClient.SqlParameter("@status", (int)newStatus), new System.Data.SqlClient.SqlParameter("@ID", _entity.ID));

            if (refreshEntity)
                await db.Entry(_entity).ReloadAsync();
        }

        /// <summary>
        /// Sets status of non-routed requests based on current Request.Private
        /// Private requests are set to Draft (200) and non-private requests to DraftReview (250) if they are not already.
        /// Saves changes and sends notification of status change. Requests with other statuses are ignored.
        /// </summary>
        /// <returns></returns>
        protected async Task SetRequestVisibility(PmnTask task)
        {
            var visibility = (_entity.Private) ? "hidden." : "visible.";
            var msg = $"Request is currently {visibility}";
            var originalStatus = _entity.Status;

            if (_entity.Private && (originalStatus != DTO.Enums.RequestStatuses.Draft))
            {
                await db.Entry(_entity).ReloadAsync();
                await SetRequestStatus(DTO.Enums.RequestStatuses.Draft, false);
                await task.LogAsModifiedAsync(_workflow.Identity, db, msg, appendDescription: true);
                await db.SaveChangesAsync();
                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Draft);
            }
            else if (!_entity.Private && (originalStatus != DTO.Enums.RequestStatuses.DraftReview))
            {
                await db.Entry(_entity).ReloadAsync();
                await SetRequestStatus(DTO.Enums.RequestStatuses.DraftReview, false);
                await task.LogAsModifiedAsync(_workflow.Identity, db, msg, appendDescription: true);
                await db.SaveChangesAsync();
                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.DraftReview);
            }
        }

        /// <summary>
        /// Create and send request status change notifications.
        /// </summary>
        /// <param name="currentStatus">The original status of the request.</param>
        /// <param name="newStatus">The new status of the request.</param>
        /// <returns></returns>
        protected async Task NotifyRequestStatusChanged(DTO.Enums.RequestStatuses currentStatus, DTO.Enums.RequestStatuses newStatus)
        {
            string[] emailText = await _requestLogger.GenerateRequestStatusChangedEmailContent(db, _entity.ID, _workflow.Identity.ID, currentStatus, newStatus);

            var logItems = _requestLogger.GenerateRequestStatusEvents(db, _workflow.Identity, false, currentStatus, newStatus, _entity.ID, emailText[1], emailText[0], "Request Status Changed");
            await db.SaveChangesAsync();

            await ProcessNotifications(logItems);
        }

        /// <summary>
        /// Gets the ID's of datamarts the current user can submit to for the current request.
        /// </summary>
        /// <returns></returns>
        protected async Task<IEnumerable<Guid>> GetAllAvailableDataMarts()
        {
            //make sure have access to the datamart
            var datamarts = db.Secure<DataMart>(_workflow.Identity);
            //make sure have access to the request
            var requests = db.Secure<Request>(_workflow.Identity);

            var query = from dm in datamarts
                        from r in requests
                        let uID = _workflow.Identity.ID
                        where r.ID == _entity.ID
                        && dm.Projects.Any(p => p.ProjectID == r.ProjectID)
                        //QE requests - only to workflow request types and datamarts with specified adapter supported
                        && r.RequestType.WorkflowID.HasValue && dm.AdapterID.HasValue
                        //make sure have permission to the request type
                        && (dm.DataMartRequestTypeAcls.Any(a => a.RequestTypeID == r.RequestTypeID && a.Permission > 0 && db.SecurityGroupUsers.Any(sgu => sgu.UserID == uID && sgu.SecurityGroupID == a.SecurityGroupID))
                        || dm.ProjectDataMartRequestTypeAcls.Any(a => a.RequestTypeID == r.RequestTypeID && a.ProjectID == r.ProjectID && a.Permission > 0 && db.SecurityGroupUsers.Any(sgu => sgu.UserID == uID && sgu.SecurityGroupID == a.SecurityGroupID))
                        || db.ProjectRequestTypeAcls.Any(a => a.RequestTypeID == r.RequestTypeID && a.ProjectID == r.ProjectID && a.Permission > 0 && db.SecurityGroupUsers.Any(sgu => sgu.UserID == uID && sgu.SecurityGroupID == a.SecurityGroupID))
                        )
                        select dm;

            return await query.Select(dm => dm.ID).ToArrayAsync();
        }

        /// <summary>
        /// Updates the routing status of the specified datamarts.
        /// </summary>
        /// <param name="changes">The collection of status change requetss for the datamarts.</param>
        /// <returns>A boolean indicating if any of the datamarts where changed.</returns>
        protected async Task<bool> UpdateDataMartRoutingStatus(IEnumerable<RoutingChangeRequestModel> routeChanges)
        {
            var requestDataMartIDs = routeChanges.Select(i => i.RequestDataMartID).ToArray();

            var routes = await (from rdm in db.RequestDataMarts
                                let userID = _workflow.Identity.ID
                                let projectID = _entity.ProjectID
                                let overrideRoutingStatusPermissionID = PermissionIdentifiers.Request.OverrideDataMartRoutingStatus.ID
                                let projectOverrideAcls = db.ProjectAcls.Where(a => a.ProjectID == projectID && a.PermissionID == overrideRoutingStatusPermissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID)).Select(a => a.Allowed)
                                let datamartOverrideAcls = db.DataMartAcls.Where(a => a.DataMartID == rdm.DataMartID && a.PermissionID == overrideRoutingStatusPermissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID)).Select(a => a.Allowed)
                                let projectDataMartOverrideAcls = db.ProjectDataMartAcls.Where(a => a.DataMartID == rdm.DataMartID && a.ProjectID == projectID && a.PermissionID == overrideRoutingStatusPermissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID)).Select(a => a.Allowed)
                                let currentResponse = db.Responses.Where(rsp => rsp.RequestDataMartID == rdm.ID && rsp.Count == rdm.Responses.Select(rr => rr.Count).Max()).FirstOrDefault()
                                where requestDataMartIDs.Contains(rdm.ID)
                                select new
                                {
                                    RequestDataMart = rdm,
                                    canOverrideRoutingStatus = (projectOverrideAcls.Any() || datamartOverrideAcls.Any() || projectDataMartOverrideAcls.Any()) && (projectOverrideAcls.All(a => a) && datamartOverrideAcls.All(a => a) && projectDataMartOverrideAcls.All(a => a)),
                                    CurrentResponse = currentResponse
                                }).ToArrayAsync();

            if (routes.Any(rt => rt.canOverrideRoutingStatus == false))
            {
                throw new SecurityException("You do not have permission to override the status of a routing for one or more of the specified DataMarts.");
            }

            bool hasChanges = false;
            foreach (var detail in routes)
            {
                var changes = routeChanges.FirstOrDefault(dm => dm.RequestDataMartID == detail.RequestDataMart.ID);
                if (changes == null || detail.RequestDataMart.Status == changes.NewStatus)
                    continue;

                detail.RequestDataMart.Status = changes.NewStatus;
                detail.RequestDataMart.UpdatedOn = DateTime.UtcNow;
                detail.CurrentResponse.ResponseMessage = changes.Message;

                if(detail.RequestDataMart.Status == DTO.Enums.RoutingStatus.Submitted && detail.CurrentResponse.Count > 1)
                {
                    //update to automatically have status be resubmitted if not on first iteration.
                    detail.RequestDataMart.Status = DTO.Enums.RoutingStatus.Resubmitted;
                }

                if (changes.NewStatus == DTO.Enums.RoutingStatus.Completed)
                {
                    detail.CurrentResponse.ResponseTime = DateTime.UtcNow;
                    detail.CurrentResponse.RespondedByID = _workflow.Identity.ID;
                }

                hasChanges = true;
            }

            if (hasChanges)
            {
                await LogTaskModified("One or more routings had their status modified.");
                var originalStatus = _entity.Status;
                await db.SaveChangesAsync();

                await db.Entry(_entity).ReloadAsync();

                if (originalStatus != DTO.Enums.RequestStatuses.Complete && _entity.Status == DTO.Enums.RequestStatuses.Complete)
                {
                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Complete);
                }
            }

            return hasChanges;
        }

        /// <summary>
        /// Marks the current activity's task as modified.
        /// </summary>
        /// <param name="optionalMessage"></param>
        /// <returns></returns>
        protected async Task LogTaskModified(string optionalMessage = null)
        {
            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ID, db);
            if (task == null)
            {
                task = db.Actions.Add(PmnTask.CreateForWorkflowActivity(_entity.ID, ID, _workflow.ID, db));
                return;
            }

            await task.LogAsModifiedAsync(_workflow.Identity, db, optionalMessage);
        }

        /// <summary>
        /// Parses the Request's Query string into a QueryComposerRequestDTO, it will upconvert old request json schema to new objects.
        /// </summary>
        /// <returns></returns>
        protected DTO.QueryComposer.QueryComposerRequestDTO ParseRequestJSON()
        {
            return Lpp.Dns.Data.QueryComposer.Helpers.ParseRequestJSON(_entity);
        }

        protected bool HasTermInAnyCriteria(Guid termTypeID, DTO.QueryComposer.QueryComposerRequestDTO requestDTO)
        {
            return Data.QueryComposer.Helpers.HasTermInAnyCriteria(termTypeID, requestDTO);
        }

        protected IEnumerable<DTO.QueryComposer.QueryComposerTermDTO> GetAllTerms(Guid termTypeID, DTO.QueryComposer.QueryComposerRequestDTO requestDTO)
        {
            return Data.QueryComposer.Helpers.GetAllTerms(termTypeID, requestDTO.Queries.SelectMany(q => q.Where.Criteria));
        }

        protected IEnumerable<DTO.QueryComposer.QueryComposerTermDTO> GetAllTerms(Guid termTypeID, IEnumerable<DTO.QueryComposer.QueryComposerCriteriaDTO> criteria)
        {
            return Data.QueryComposer.Helpers.GetAllTerms(termTypeID, criteria);
        }

    }
}
