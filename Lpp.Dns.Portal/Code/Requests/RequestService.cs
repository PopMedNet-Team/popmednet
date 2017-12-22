using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Lpp.Audit;
using Lpp.Composition;
//using Lpp.Data;
//using Lpp.Dns.Model;
using Lpp.Dns.Portal.Models;
using Lpp.Security;
using System.Data.Entity;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IRequestService)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    internal class RequestService : IRequestService
    {
        const int MaxRequestNameLength = 255;

        [ImportMany]
        public IEnumerable<IDnsRequestValidator> Validators { get; set; }
        [Import]
        public IPluginService Plugins { get; set; }

        public DataContext DataContext
        {
            get
            {
                return System.Web.HttpContext.Current.Items["DataContext"] as DataContext;
            }
        }

        [Import]
        public IAuthenticationService Auth { get; set; }
        [Import]
        public IDocumentService DocService { get; set; }
        [Import]
        public IAuditService<Lpp.Dns.Model.DnsDomain> Audit { get; set; }

        public IRequestContext CreateRequest(Project project, PluginRequestType requestType)
        {

            Guid? projectID = project == null  ? new Nullable<Guid>() : project.ID;
            if(!GetGrantedRequestTypeIDs(projectID).Contains(requestType.RequestType.ID))
                throw new UnauthorizedAccessException();

            if (project == null)
            {
                /** 
                 * BMS: When saving or submitting the request, edit rules prevent the project from being null, however if the user navigates away w/o saving the request, 
                 * the request is effectively lost from any view.  To prevent this, always initialize the view with a "default" project which in this case is the first one 
                 * in the list.  Better would be to designate a "Default" project and choose it when it doubt, or use the project chosen on the last request saved/submitted.
                 **/
                
                project = GetGrantedProjects(requestType.RequestType.ID).FirstOrDefault();

                if (project == null)
                {
                    //final fallback to use the project chosen on the last request saved/submitted
                    project = DataContext.Requests.Where(r => r.SubmittedByID == Auth.CurrentUserID && !r.Deleted && r.Project.Active && !r.Project.Deleted).Select(r => r.Project).FirstOrDefault();
                }

            }

            if (project == null)
                throw new UnauthorizedAccessException("A Project was not specified and is required to create a request; could not determine a default project to assign based on the current security permissions. Please make sure permission has been granted for the desired request type to at least one Project.");

            string requestName = requestType.RequestType.Name + " - " + DataContext.Requests.Count(r => r.RequestTypeID == requestType.RequestType.ID);
            var request = DataContext.Requests.Add(new Request
            {
                Name = requestName.Substring(0, Math.Min(requestName.Length, MaxRequestNameLength)),
                Description = "",
                AdditionalInstructions = "",
                Project = project,
                RequestTypeID = requestType.RequestType.ID,
                CreatedByID = Auth.CurrentUserID,
                UpdatedByID = Auth.CurrentUserID,
                OrganizationID = Auth.CurrentUser.OrganizationID.Value,
                Priority = Priorities.Medium,
                DataMarts = new List<RequestDataMart>()
            });

            foreach (var dm in GetGrantedDataMarts(project, requestType))
            {
                var datamart = dm;
                var requestDataMart = new RequestDataMart { DataMart = datamart, Responses = new HashSet<Response>() };
                requestDataMart.Responses.Add(new Response { RequestDataMart = requestDataMart, SubmittedByID = Auth.CurrentUser.ID });
                requestDataMart.Priority = Priorities.Medium;
                request.DataMarts.Add(requestDataMart);
            }

            //TODO: does default security need to be explicitly set or is it handled in triggers?
            //SecurityHierarchy.SetObjectInheritanceParent(res, VirtualSecObjects.AllRequests);

            return new RequestContext(this, request, requestType, true);
        }

        public IQueryable<DataMart> GetGrantedDataMarts(Project project, PluginRequestType rt)
        {
            return GetGrantedDataMarts(project, rt.RequestType.ID, rt.Model.ID);
        }

        public IQueryable<DataMart> GetGrantedDataMarts(Project project, Guid requestTypeID, Guid modelID)
        {
            return GetGrantedDataMarts(DataContext, project, requestTypeID, modelID);
        }

        IQueryable<DataMart> GetGrantedDataMarts(DataContext datacontext, Project project, Guid requestTypeID, Guid modelID)
        {
            var datamarts = datacontext.DataMarts
                                            .Include(dm => dm.Organization)
                                            .Include(dm => dm.Models)
                                            .Include(dm => dm.Projects)
                                            .Secure<DataMart>(datacontext, Auth.ApiIdentity)
                                            .Where(dm => !dm.Deleted
                                                && dm.Models.Any(m => m.ModelID == modelID && m.Model.RequestTypes.Any(r => r.RequestTypeID == requestTypeID)))
                                            .If(project == null, 
                                                    q => q.Where(dm => dm.DataMartRequestTypeAcls.Any(a => a.RequestTypeID == requestTypeID && a.Permission > 0)), 

                                                //if project != null
                                                    q => q.Where(dm => 
                                                        dm.Projects.Any(p => p.ProjectID == project.ID) 
                                                        && 
                                                        (
                                                            (dm.DataMartRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.SecurityGroup.Users.Any(u => u.UserID == Auth.CurrentUserID)).Any(a => a.Permission > 0) && dm.DataMartRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.SecurityGroup.Users.Any(u => u.UserID == Auth.CurrentUserID)).All(a => a.Permission > 0))
                                                            || (dm.ProjectDataMartRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.ProjectID == project.ID && a.SecurityGroup.Users.Any(u => u.UserID == Auth.CurrentUserID)).Any(a => a.Permission > 0) && dm.ProjectDataMartRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.ProjectID == project.ID && a.SecurityGroup.Users.Any(u => u.UserID == Auth.CurrentUserID)).All(a => a.Permission > 0))
                                                            || (datacontext.ProjectRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.ProjectID == project.ID && a.SecurityGroup.Users.Any(u => u.UserID == Auth.CurrentUserID)).Any(a => a.Permission > 0) && datacontext.ProjectRequestTypeAcls.Where(a => a.RequestTypeID == requestTypeID && a.ProjectID == project.ID && a.SecurityGroup.Users.Any(u => u.UserID == Auth.CurrentUserID)).All(a => a.Permission > 0))
                                                        )
                                                    ) //End where
                                               ); //End if

            return datamarts;
        }

        public DnsResult DeleteRequest(IRequestContext ctx)
        {
            try
            {
                //can delete if you are the creator and it is still a draft (and you don't have delete permission) or you have explicit delete permission.
                if (!(Auth.CurrentUserID == ctx.Request.CreatedByID && (int)ctx.Request.Status < 300) && !AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Request>(Auth.ApiIdentity, ctx.Request, PermissionIdentifiers.Request.Delete)))
                {
                    return DnsResult.Failed("Access Denied, you do not have permission to delete the request.");
                } 
              
                //do full delete if all the responses for the request are in draft status, else do soft delete
                var allAreDrafts = DataContext.RequestDataMarts.Where(dm => dm.RequestID == ctx.RequestID).All(dm => dm.Status == RoutingStatus.Draft);
                if (allAreDrafts)
                {
                    DataContext.Requests.Remove(ctx.Request);
                    DataContext.SaveChanges();
                }
                else
                {
                    ctx.Request.Deleted = true;
                    ctx.Request.UpdatedBy = Auth.CurrentUser;
                    ctx.Request.UpdatedOn = DateTime.UtcNow;
                    DataContext.SaveChanges();
                }
                ctx.Close();               

                return DnsResult.Success;
            }
            catch (Exception ex)
            {
                return DnsResult.FromException(ex);
            }
        }

        public Request CopyRequest(IRequestContext ctx)
        {

            var pluginRequestType = Plugins.GetPluginRequestType(ctx.Request.RequestTypeID);
            if (pluginRequestType == null) 
                throw new InvalidOperationException(MissingPluginStub.MissingMessage);

            Project project = DataContext.Projects.SingleOrDefault(p => p.ID == ctx.Request.ProjectID);
            var newRequest = CreateRequest(project, pluginRequestType).Request;
            var newRequestHeader = CreateHeader(ctx.Request);

            newRequestHeader.Name =
                Enumerable.Range(2, int.MaxValue - 2)
                .Select(i => " " + i).StartWith("")
                .Select(suffix => " (Copy" + suffix + ")")
                .Select(suffix => newRequestHeader.Name.Substring(0, Math.Min(newRequestHeader.Name.Length, MaxRequestNameLength - suffix.Length)) + suffix)
                .Where(name => !DataContext.Requests.Any(r => r.Name == name))
                .FirstOrDefault();

            if (newRequestHeader.Name == null)
                newRequestHeader.Name = newRequest.Name;

            newRequestHeader.MSRequestID = 
                Enumerable.Range(2, int.MaxValue - 2)
                .Select(i => " " + i).StartWith("")
                .Select(suffix => " (Copy" + suffix + ")")
                .Select(suffix => newRequestHeader.MSRequestID.Substring(0, Math.Min(newRequestHeader.MSRequestID.Length, MaxRequestNameLength - suffix.Length)) + suffix)
                .Where(msId => !DataContext.Requests.Any(r => r.MSRequestID == msId))
                .FirstOrDefault();

            if (newRequestHeader.MSRequestID == null)
                newRequestHeader.MSRequestID = "";

            ApplyHeader(newRequestHeader, newRequest);

            newRequest.DataMarts.Clear();

            foreach (var dm in DataContext.RequestDataMarts.Where(rdm => rdm.RequestID == ctx.RequestID && rdm.Status != RoutingStatus.Canceled && DataContext.DataMarts.Any(dm => dm.ID == rdm.DataMartID)).ToArray())
            {
                var originalDataMart = dm;
                var requestDataMart = new RequestDataMart
                {
                    DataMartID = originalDataMart.DataMartID,
                    RequestID = newRequest.ID,
                    Responses = new HashSet<Response>()
                };

                newRequest.DataMarts.Add(requestDataMart);

                requestDataMart.Responses.Add(new Response
                {
                    RequestDataMartID = requestDataMart.ID,
                    RequestDataMart = requestDataMart,
                    SubmittedByID = Auth.CurrentUser.ID
                });
            }
            
            var documents = DataContext.Documents.Where(d => d.ItemID == ctx.Request.ID).ToArray();
            Dictionary<Guid, Document> newDocuments = new Dictionary<Guid, Document>();
            foreach (var document in documents)
            {
                var newDocument = new Document
                {
                    CreatedOn = DateTime.UtcNow,
                    FileName = document.FileName,
                    ItemID = newRequest.ID,
                    Kind = document.Kind,
                    Length = document.Length,
                    MimeType = document.MimeType,
                    Name = document.Name,
                    Viewable = document.Viewable
                };

                newDocuments.Add(document.ID, newDocument);
                DataContext.Documents.Add(newDocument);
            }            

            DataContext.SaveChanges();

            foreach (var document in newDocuments)
            {
                document.Value.CopyData(DataContext, document.Key);
            }

            DataContext.SaveChanges();

            return newRequest;
        }

        public DnsResult AddDataMarts(IRequestContext ctx, IEnumerable<IDnsDataMart> dataMarts)
        {
            IEnumerable<Guid> dataMartIDs = dataMarts.Select(dm => dm.ID).ToArray();

            try
            {
                using (var datacontext = new DataContext())
                {
                    if (!dataMartIDs.Any())
                        return DnsResult.Failed("Please select at least one DataMart to add");

                    var request = datacontext.Requests
                                             .Include(r => r.Project)
                                             .Single(r => r.ID == ctx.RequestID);

                    if (request.Template)
                        return DnsResult.Failed("Cannot modify a request template");

                    if (!AsyncHelpers.RunSync<bool>(() => datacontext.HasPermissions<Request>(Auth.ApiIdentity, request, PermissionIdentifiers.Request.ChangeRoutings)))
                        return DnsResult.Failed("Adding of DataMart(s) failed, you do not have permission to perform this action.");

                    //confirm has permission to all the datamarts
                    var authorizedDataMartIDs = GetGrantedDataMarts(datacontext, request.Project, ctx.RequestType.ID, ctx.Model.ID).Select(d => d.ID).ToArray();
                    var nonAuthorizedDataMartIDs = dataMartIDs.Except(authorizedDataMartIDs).ToArray();
                    if (nonAuthorizedDataMartIDs.Any())
                    {
                        return NonGrantedDataMartsResult(ctx.RequestType.Name, nonAuthorizedDataMartIDs);
                    }

                    var targetStatus = RoutingStatus.Submitted;

                    //reset the status of the request if already exists
                    var existingDataMarts = datacontext.RequestDataMarts.Where(dm => dm.RequestID == request.ID).ToArray();
                    foreach (var d in existingDataMarts)
                    {
                        if (d.Status == RoutingStatus.Canceled && dataMartIDs.Contains(d.DataMartID))
                        {
                            d.Status = targetStatus;
                        }
                    }

                    var newDataMartIDs = dataMartIDs.Except(existingDataMarts.Select(x => x.DataMartID)).ToArray();
                    var newDataMarts = datacontext.DataMarts.Where(dm => !dm.Deleted && newDataMartIDs.Contains(dm.ID)).ToArray();
                    newDataMarts.Select(dm => RequestDataMart.Create(ctx.RequestID, dm.ID, Auth.CurrentUserID))
                                .ToArray()
                                .ForEach(dm =>
                                {
                                    dm.Status = targetStatus;
                                    dm.Priority = ctx.Request.Priority;
                                    dm.DueDate = ctx.Request.DueDate;
                                    request.DataMarts.Add(dm);
                                });

                    if (request.CancelledOn.HasValue || request.CancelledByID.HasValue)
                    {
                        request.CancelledOn = null;
                        request.CancelledByID = null;
                    }

                    if (request.SubmittedOn == null)
                    {
                        datacontext.SaveChanges();
                        return DnsResult.Success;
                    }

                    var validationResult = ValidateRequest(ctx);
                    if (validationResult.IsSuccess)
                    {
                        datacontext.SaveChanges();
                    }

                    return validationResult;
                }
            }
            catch (Exception ex)
            {
                return DnsResult.FromException(ex);
            }
        }

        public DnsResult RemoveDataMarts(IRequestContext ctx, IEnumerable<Guid> dataMartIDs)
        {
            try
            {
                if (dataMartIDs == null || !dataMartIDs.Any()) 
                    return DnsResult.Failed("Please select at least one DataMart to remove");

                if (ctx.Request.Template) 
                    return DnsResult.Failed("Cannot modify a request template");

                if (!AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Request>(Auth.ApiIdentity, ctx.Request, PermissionIdentifiers.Request.ChangeRoutings)))
                    return DnsResult.Failed("Removing of DataMart(s) failed, you do not have permission to perform this action.");

                var ids = dataMartIDs.ToArray();
                using (var db = new DataContext())
                {
                    if (db.RequestDataMarts.Where(d => d.RequestID == ctx.RequestID && !ids.Contains(d.ID)).All(d => d.Status == RoutingStatus.Canceled))
                    {
                        var request = db.Requests.Single(r => r.ID == ctx.RequestID);
                        request.CancelledOn = DateTime.UtcNow;
                        request.CancelledByID = Auth.CurrentUserID;
                    }

                    var datamarts = db.RequestDataMarts
                                        .Where(d => d.RequestID == ctx.RequestID
                                            && ids.Contains(d.ID)
                                            && (
                                                d.Status == RoutingStatus.Resubmitted ||
                                                d.Status == RoutingStatus.Submitted ||
                                                d.Status == RoutingStatus.Draft ||
                                                d.Status == RoutingStatus.AwaitingRequestApproval
                                            )).ToArray();

                    if (datamarts.Length == 0)
                        return DnsResult.Success;

                    datamarts.ForEach(d => d.Status = RoutingStatus.Canceled);

                    if (ctx.Request.SubmittedOn == null)
                    {
                        db.SaveChanges();
                        return DnsResult.Success;
                    }

                    var validationResult = ValidateRequest(ctx);
                    if (validationResult.IsSuccess)
                    {
                        db.SaveChanges();
                    }

                    return validationResult;
                }

            }
            catch (Exception ex)
            {
                return DnsResult.FromException(ex);
            }
        }

        public IRequestContext GetRequestContext(Guid requestID)
        {
            return GetRequestContext(requestID, DataContext);
        }

        IRequestContext GetRequestContext(Guid requestID, DataContext db)
        {
            var request = db.Requests
                                    .Include("Activity")
                                    .Include("Activity.ParentActivity")
                                    .Include("Activity.ParentActivity.ParentActivity")
                                    .Include("SourceActivity")
                                    .Include("SourceActivityProject")
                                    .Include("SourceTaskOrder")
                                    .Include("Organization")
                                    .Include("Project")
                                    .Include("RequestType")
                                    .Include("RequesterCenter")
                                    .Include("WorkplanType")
                                    .Include("CreatedBy")
                                    .Include("UpdatedBy")
                                    .Include("SubmittedBy")
                                    .Include("DataMarts")
                                    .Include("Folders")
                                    .Include("SearchTerms")
                                    //.AsNoTracking()
                                    .SingleOrDefault(r => r.ID == requestID);

            if (request == null || (request.SubmittedOn == null && request.CreatedByID != Auth.CurrentUser.ID))
                throw new UnauthorizedAccessException();;

            return new RequestContext(this, request, Plugins.GetPluginRequestType(request.RequestTypeID) ?? Plugins.MissingPluginStub);
        }

        public DnsResult ValidateRequest(IRequestContext ctx)
        {
            return Validators.Aggregate(ctx.Plugin.ValidateForSubmission(ctx), (res, v) => res.Merge(v.Validate(ctx)));
        }

        public DnsResult TimeShift(IRequestContext ctx, TimeSpan timeDifference)
        {
            return ApplyTransaction(ctx, ctx.Plugin.TimeShift(ctx, timeDifference));
        }

        public DnsResult UpdateRequest(RequestUpdateOperation op)
        {
            try
            {
                if (op.Context.Request.SubmittedOn.HasValue)
                    return DnsResult.Failed("Cannot modify a request that is already submitted");

                if (Auth.CurrentUserID != op.Context.Request.CreatedByID && !AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Request>(Auth.ApiIdentity, op.Context.Request, PermissionIdentifiers.Request.Edit)))
                    return DnsResult.Failed("Access Denied, you do not have permission to update a request.");                

                if (op.Header.Name.NullOrEmpty() || op.Header.Name.Length > MaxRequestNameLength) 
                    return DnsResult.Failed(string.Format("Request name must be between 1 and {0} characters long", MaxRequestNameLength));

                if (!op.Header.MSRequestID.NullOrEmpty() &&
                    DataContext.Requests.Any(req => req.MSRequestID == op.Header.MSRequestID && req.ID != op.Context.Request.ID))
                    return DnsResult.Failed("The Request ID entered is not unique. Please enter in a different Request ID.");

                //Name does not need to be unique as of PMNDEV-4114
                //if(DataContext.Requests.Any(r => r.Name == op.Header.Name && r.ID != op.Context.RequestID))
                //    return DnsResult.Failed("There is already a request by the same name in the system. Please choose a different name.");

                if(op.ProjectID == null)
                    return DnsResult.Failed("Project must be selected.");

                var request = op.Context.Request;
                ApplyHeader(op.Header, request);
                request.UpdatedBy = Auth.CurrentUser;
                request.UpdatedOn = DateTime.UtcNow;
                request.Project = op.ProjectID == null ? null : DataContext.Projects.SingleOrDefault(p => p.ID == op.ProjectID);
                UpdateDataMartRoutings(request, op.AssignedDataMarts);
                //save the main request info.
                DataContext.SaveChanges();

                var trn = op.Context.Plugin.EditRequestPost(op.Context, op.Post);
                var result = ApplyTransaction(op.Context, trn);

                if (result.IsSuccess == false)
                    return result;               

                return result;
            }
            catch (Exception ex)
            {
                return DnsResult.FromException(ex);
            }
        }

        void UpdateDataMartRoutings(Request request, IEnumerable<IDnsDataMart> selectedDataMarts)
        {
            //remove any that have been unselected
            request.DataMarts.Where(dm => !selectedDataMarts.Any(dd => dd.ID == dm.ID)).ToList()
                             .ForEach(dm =>
                                {
                                    request.DataMarts.Remove(dm);
                                    DataContext.RequestDataMarts.Remove(dm);
                                });
            
             //add any missing request.DataMarts.
            selectedDataMarts.Where(dm => !request.DataMarts.Any(dd => dd.ID == dm.ID))
                             .Select(dm => RequestDataMart.Create(request.ID, dm.ID, Auth.CurrentUserID))
                             .ForEach(dm => request.DataMarts.Add(dm));            
            
            //update request DataMart due dates and priorities
            request.DataMarts.ForEach((rdm) => {
                var dataMart = selectedDataMarts.Where((sdm) => sdm.ID == rdm.DataMartID).First();
                rdm.Priority = dataMart.Priority;
                rdm.DueDate = dataMart.DueDate;
            });
        }

        DnsResult ApplyTransaction(IRequestContext ctx, DnsRequestTransaction trn)
        {
            if (trn.IsFailed)
                return new DnsResult { ErrorMessages = trn.ErrorMessages };

            //add new documents
            if (trn.NewDocuments == null)
                trn.NewDocuments = Enumerable.Empty<DocumentDTO>();

            foreach (var newDoc in trn.NewDocuments)
            {
                var document = new Document
                {
                    FileName = newDoc.FileName,
                    ItemID = ctx.RequestID,
                    Kind = newDoc.Kind,
                    Length = newDoc.Data.LongLength,
                    MimeType = newDoc.MimeType,
                    Name = newDoc.Name,
                    Viewable = newDoc.Viewable
                };
                DataContext.Documents.Add(document);
                DataContext.SaveChanges();
                document.SetData(DataContext, newDoc.Data);
            }

            //remove deleted documents
            var removeIDs = (trn.RemoveDocuments ?? Enumerable.Empty<Document>()).Select(d => d.ID).ToArray();
            if (removeIDs.Length > 0)
            {
                DataContext.Documents.RemoveRange(DataContext.Documents.Where(d => removeIDs.Contains(d.ID)));
                DataContext.SaveChanges();
            }

            //update search terms
            DataContext.RequestSearchTerms.RemoveRange(DataContext.RequestSearchTerms.Where(s => s.RequestID == ctx.RequestID));
            ctx.Request.SearchTerms.Clear();
            if (trn.SearchTerms != null)
            {
                trn.SearchTerms.ForEach(t => ctx.Request.SearchTerms.Add(new RequestSearchTerm { Request = ctx.Request, Type = (int)t.Type, DateFrom = t.DateFrom, DateTo = t.DateTo, NumberFrom = t.NumberFrom, NumberTo = t.NumberTo, NumberValue = t.NumberValue, StringValue = t.StringValue }));
            }
            DataContext.SaveChanges();

            ctx.Reset();

            return DnsResult.Success;
        }

        public void ExecuteIfLocalRequest(IRequestContext ctx, DataContext db)
        {
            //db.Requests.Load();
            //db.Responses.Load();
            //db.RequestDataMarts.Load();
            var responses = (from dm in db.RequestDataMarts
                                where dm.DataMart.IsLocal && ctx.RequestID == dm.RequestID
                                from r in dm.Responses
                                where r.Count == dm.Responses.Max(c => c.Count)
                                select new { Routing = dm, Response = r }
                            ).ToArray();

            if (responses.Length > 0)
            {

                foreach (var item in responses)
                {
                    RequestDataMart routing = item.Routing;
                    Response response = item.Response;

                    DnsResponseTransaction pluginResponse = ctx.Plugin.ExecuteRequest(GetRequestContext(ctx.RequestID, db));

                    db.Entry(response).Reload();
                    db.Entry(routing).Reload();
                    db.ChangeTracker.Entries<Request>().First().Reload(); //Hack

                    response.RespondedByID = Auth.CurrentUserID;
                    response.ResponseMessage = string.Empty;
                    response.ResponseTime = DateTime.UtcNow;
                    routing.Status = RoutingStatus.Completed;
                }

                db.SaveChanges();

            }

        }

        public DnsResult SubmitRequest(IRequestContext ctx)
        {
            /**
             * Due to concurrency issues that are being created somehow by doing the update first
             * this operation is being done using a standalone datacontext and fresh data from db.
             * */
            try
            {
                RoutingStatus targetStatus = RoutingStatus.AwaitingRequestApproval;

                using (var datacontext = new DataContext())
                {
                    var request = datacontext.Requests.Include(r => r.DataMarts.Select(rr => rr.Responses)).Single(r => r.ID == ctx.RequestID);
                    var project = datacontext.Projects.SingleOrDefault(p => p.ID == request.ProjectID);

                    if (project == null)
                    {
                        /**
                         * BMS: We used to allow a metadata request to be submitted w/o a project, however this causes the request to be lost from any view, 
                         * so now I force the request to a project until we figure out how to display requests w/o project assignments.
                         **/
                        return DnsResult.Failed("Cannot submit a request outside of a Project context. Please select a Project.");
                    }

                    if (!project.Active || project.Deleted)
                        return DnsResult.Failed("Cannot submit requests for project " + ctx.Request.Project.Name + ", because the project is marked inactive.");

                    if (project.StartDate != null && project.StartDate > DateTime.UtcNow)
                        return DnsResult.Failed("Cannot submit requests for project " + ctx.Request.Project.Name + ", because the project has not started yet.");

                    if (project.EndDate != null && project.EndDate < DateTime.UtcNow)
                        return DnsResult.Failed("Cannot submit requests for project " + ctx.Request.Project.Name + ", because the project has already finished.");

                    var dueDate = request.DueDate;
                    if (dueDate != null && dueDate < DateTime.UtcNow.Date)
                        return DnsResult.Failed("Due date must be set in the future.");

                    var pastDueDate = false;
                    foreach (var dm in request.DataMarts) {
                        if (dm.DueDate != null && dm.DueDate < DateTime.UtcNow.Date)
                            pastDueDate = true;
                    }
                    if (pastDueDate)
                        return DnsResult.Failed("Request's DataMart Due dates must be set in the future.");

                    var grantedDataMarts = GetGrantedDataMarts(datacontext, project, request.RequestTypeID, ctx.Model.ID).ToArray();
                    var nonGrantedDataMartIDs = datacontext.RequestDataMarts.Where(dm => dm.RequestID == request.ID).Select(dm => dm.DataMartID).Except(grantedDataMarts.Select(d => d.ID)).ToArray();
                    if (nonGrantedDataMartIDs.Length > 0)
                    {
                        return NonGrantedDataMartsResult(ctx.RequestType.Name, nonGrantedDataMartIDs);
                    }

                    // Remove datamarts that do not belong to the Project                
                    var invalidDataMarts = (from dm in datacontext.RequestDataMarts.Where(d => d.RequestID == request.ID)
                                            join pdm in datacontext.ProjectDataMarts.Where(p => p.ProjectID == request.ProjectID) on dm.DataMartID equals pdm.DataMartID into pdms
                                            where !pdms.Any()
                                            select dm).ToList();
                    if (invalidDataMarts.Count > 0)
                    {
                        datacontext.RequestDataMarts.RemoveRange(invalidDataMarts);
                    }

                    if (request.SubmittedOn.HasValue)
                        return DnsResult.Failed("Cannot submit a request that has already been submitted");

                    if (request.Template)
                        return DnsResult.Failed("Cannot submit a request template");

                    if (request.Scheduled)
                        return DnsResult.Failed("Cannot submit a scheduled request");

                    var filters = new ExtendedQuery
                    {
                        Projects = (a) => a.ProjectID == request.ProjectID,
                        ProjectOrganizations = a => a.ProjectID == request.ProjectID && a.OrganizationID == request.OrganizationID,
                        Organizations = a => a.OrganizationID == request.OrganizationID,
                        Users = a => a.UserID == request.CreatedByID
                    };

                    if (request.DataMarts.Count < 2)
                    {
                        var skip2DataMartRulePerms = AsyncHelpers.RunSync(() => datacontext.HasGrantedPermissions<Request>(Auth.ApiIdentity, request, filters, PermissionIdentifiers.Portal.SkipTwoDataMartRule));

                        if (!skip2DataMartRulePerms.Contains(PermissionIdentifiers.Portal.SkipTwoDataMartRule))
                            return DnsResult.Failed("Cannot submit a request with less than 2 datamarts.");
                    }

                    var permissions = AsyncHelpers.RunSync(() => datacontext.HasGrantedPermissions<Request>(Auth.ApiIdentity, request, filters, PermissionIdentifiers.Request.SkipSubmissionApproval));

                    if (permissions.Contains(PermissionIdentifiers.Request.SkipSubmissionApproval))
                    {
                        targetStatus = RoutingStatus.Submitted;
                    }
                    request.Status = targetStatus == RoutingStatus.Submitted ? RequestStatuses.Submitted : RequestStatuses.AwaitingRequestApproval;
                    request.SubmittedOn = DateTime.UtcNow;
                    request.SubmittedByID = Auth.CurrentUserID;

                    var pluginRequestType = Plugins.GetPluginRequestType(request.RequestTypeID);

                    //set the version on the request
                    request.AdapterPackageVersion = pluginRequestType.Plugin.Version;

                    foreach (var d in request.DataMarts)
                    {
                        if (grantedDataMarts.Any(dm => dm.ID == d.DataMartID))
                        {
                            d.Status = targetStatus;
                            d.Responses.ForEach(response =>
                            {
                                response.SubmittedByID = Auth.CurrentUserID;
                                response.SubmittedOn = request.SubmittedOn ?? DateTime.UtcNow;
                            });
                        }
                        else
                        {
                            datacontext.RequestDataMarts.Remove(d);
                        }
                    }
                    ctx.Request.Status = request.Status;
                    var result = ValidateRequest(ctx);
                    datacontext.SaveChanges();
                    if (result.IsSuccess && targetStatus == RoutingStatus.Submitted)
                    {
                        ExecuteIfLocalRequest(ctx, datacontext);
                    }

                    
                    

                    return result;
                }

            }
            catch (Exception ex)
            {
                return DnsResult.FromException(ex);
            }
            
        }

        public DnsResult ApproveRequest(IRequestContext ctx)
        {
            return ApproveReject(ctx, RoutingStatus.Submitted);
        }

        public DnsResult RejectRequest(IRequestContext ctx)
        {
            return ApproveReject(ctx, RoutingStatus.RequestRejected);
        }

        DnsResult ApproveReject(IRequestContext ctx, RoutingStatus newStatus)
        {
            try
            {
                if (!ctx.Request.SubmittedOn.HasValue)
                    return DnsResult.Failed("Cannot approve a request that has not been submitted");

                if (ctx.Request.Template)
                    return DnsResult.Failed("Cannot approve a request template");

                if (!AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Request>(Auth.ApiIdentity, ctx.Request, PermissionIdentifiers.Request.ApproveRejectSubmission)))
                {
                    return DnsResult.Failed("Access Denied");
                }

                foreach (var routing in ctx.Request.DataMarts.Where(dm => dm.Status == RoutingStatus.AwaitingRequestApproval))
                {
                    routing.Status = newStatus;
                }

                if (newStatus == RoutingStatus.Submitted)
                {
                    var request = DataContext.Requests.Find(ctx.RequestID);
                    request.Status = RequestStatuses.Submitted;
                }

                var res = ValidateRequest(ctx);
                if (res.IsSuccess)
                {
                    DataContext.SaveChanges();
                }                

                return res;
            }
            catch (Exception ex)
            {
                return DnsResult.FromException(ex);
            }
        }

        private IAuditEventBuilder LogStatusChange<T>(Lpp.Security.SecurityTarget st, IRequestContext ctx, RoutingStatus newStatus, RequestDataMart datamart) where T : Events.RequestStatusChangeBase, new()
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Audit.CreateEvent(st, new T
            //{
            //    ActingUser = Auth.CurrentUser.ID,
            //    Request = ctx.RequestId,
            //    Project = (ctx.Request.Project ?? VirtualSecObjects.AllProjects).ID,
            //    RequestType = ctx.RequestType.Id,
            //    Name = ctx.Request.Name,
            //    DataMart = datamart.ID,
            //    OldStatus = RoutingStatus.AwaitingRequestApproval.ToString(),
            //    NewStatus = newStatus.ToString(),
            //});
        }

        public DnsResult MakeTemplate(IRequestContext ctx)
        {
            try
            {
                if (ctx.Request.SubmittedOn.HasValue) 
                    return DnsResult.Failed("Cannot create a template out of a request that has already been submitted");

                if (ctx.Request.Template) 
                    return DnsResult.Failed("Cannot create a template out of request, because this request already is a template");

                if (!AsyncHelpers.RunSync<bool>(() => DataContext.HasPermissions<Request>(Auth.ApiIdentity, ctx.Request, PermissionIdentifiers.Request.Edit)))
                {
                    return DnsResult.Failed("Access Denied, you do not have permissions to edit requests.");
                }

                var result = ValidateRequest(ctx);
                if (!result.IsSuccess) 
                    return result;

                ctx.Request.Template = true;
                DataContext.SaveChanges();

                return DnsResult.Success;
            }
            catch (Exception ex)
            {
                return DnsResult.FromException(ex);
            }
        }

        public IQueryable<Request> GetGrantedRequests(Guid? projectID)
        {
            var requests = DataContext.Secure<Request>(Auth.ApiIdentity)
                                               .Include(x => x.Statistics)
                                               .Include(x => x.DataMarts)
                                               .Include(x => x.UpdatedBy)
                                               .Include(x => x.CreatedBy)
                                               .Include(x => x.Project)                                               
                                               .Where(r => !r.Deleted && !r.Organization.Deleted && !r.Project.Deleted);
            if (projectID.HasValue)
                requests = requests.Where(r => r.ProjectID == projectID.Value);

            return requests;

            //using (var db = new DataContext())
            //{
            //    var results = from r in db.Requests.Include(x => x.RoutingCounts).Include(x => x.Routings).Include(x => x.UpdatedByUser).Include(x => x.CreatedByUser).Include(x => x.Project)
            //                  where db.SecurityTuple3s.Any(t3 =>
            //                      (t3.Id1 == r.Project.SID || t3.Id1 == VirtualSecObjects.AllProjects.SID)
            //                      && (t3.Id2 == r.Organization.SID || t3.Id2 == VirtualSecObjects.AllOrganizations.SID)
            //                      && t3.Id3 == r.CreatedByUser.SID
            //                      && t3.SubjectId == Auth.CurrentUser.SID
            //                      && t3.PrivilegeId == SecPrivileges.Crud.Read.SID
            //                      && t3.ExplicitDeniedEntries == 0)
            //                  ||
            //                  db.SecurityTuple3s.Any(t3 =>
            //                      (t3.Id1 == r.Project.SID || t3.Id1 == VirtualSecObjects.AllProjects.SID)
            //                      && (t3.Id2 == r.Organization.SID || t3.Id2 == VirtualSecObjects.AllOrganizations.SID)
            //                      && (t3.Id3 == VirtualSecObjects.AllDataMarts.SID || r.Routings.Any(rr => rr.DataMart.SID == t3.Id3))
            //                      && t3.SubjectId == Auth.CurrentUser.SID
            //                      && t3.PrivilegeId == SecPrivileges.DataMartInProject.SeeRequests.SID
            //                      && t3.ExplicitDeniedEntries == 0
            //                      )
            //                    ||
            //                    db.SecurityTuple3s.Any(t3 =>
            //                        (t3.Id1 == r.Project.SID || t3.Id1 == VirtualSecObjects.AllProjects.SID)
            //                        && r.Routings.Any(rr => (rr.DataMart.SID == t3.Id3 || t3.Id3 == VirtualSecObjects.AllDataMarts.SID)
            //                            && (rr.DataMart.Organization.SID == t3.Id2 || t3.Id2 == VirtualSecObjects.AllOrganizations.SID))
            //                      && t3.SubjectId == Auth.CurrentUser.SID
            //                      && t3.PrivilegeId == SecPrivileges.DataMartInProject.ApproveResponses.SID
            //                      && t3.ExplicitDeniedEntries == 0
            //                      )
            //                      ||
            //                      db.SecurityTuple3s.Any(t3 =>
            //                        (t3.Id1 == r.Project.SID || t3.Id1 == VirtualSecObjects.AllProjects.SID)
            //                        && (t3.Id2 == r.Organization.SID || t3.Id2 == VirtualSecObjects.AllOrganizations.SID)
            //                        && r.Routings.Any(rr => (rr.DataMart.SID == t3.Id3 || t3.Id3 == VirtualSecObjects.AllDataMarts.SID))
            //                        && t3.SubjectId == Auth.CurrentUser.SID
            //                        && t3.PrivilegeId == SecPrivileges.DataMartInProject.RejectRequest.SID
            //                        && t3.ExplicitDeniedEntries == 0
            //                      )
            //                  select r;

            //    if (proj != null)
            //        results = results.Where(r => r.Project.SID == proj.SID);

            //    var arr = results.ToArray();
            //    arr.Where(r => r.RoutingCounts == null).ForEach(r => r.RoutingCounts = new RequestRoutingCounts());

            //    return arr.AsQueryable();
            //}
        }

        public IQueryable<Project> GetGrantedProjects(Guid requestTypeID)
        {

            return DataContext.Secure<Project>(Auth.ApiIdentity)
                .Where(p => !p.Deleted 
                    && !p.Group.Deleted 
                    && !p.Deleted
                    && (p.ProjectDataMartRequestTypeAcls.Any(a => a.RequestTypeID == requestTypeID && a.Permission > 0 && a.SecurityGroup.Users.Any(u => u.UserID == Auth.CurrentUserID))
                    || p.ProjectRequestTypeAcls.Any(a => a.RequestTypeID == requestTypeID && a.Permission > 0 && a.SecurityGroup.Users.Any(u => u.UserID == Auth.CurrentUserID))));
        }

        public IQueryable<Project> GetVisibleProjects()
        {

            return DataContext.Secure<Project>(Auth.ApiIdentity).Where(p => !p.Deleted && !p.Group.Deleted);
        }

        ISet<Guid> GetGrantedRequestTypeIDs(Guid? projectID)
        {
            return DataContext.Secure<RequestType>(Auth.ApiIdentity)
                              .If(projectID.HasValue, q => q.Where(r => 
                                  DataContext.ProjectRequestTypeAcls.Any(a => a.RequestTypeID == r.ID && a.ProjectID == projectID.Value && a.Permission > 0)
                                  || DataContext.ProjectDataMartRequestTypeAcls.Any(a => a.RequestTypeID == r.ID && a.ProjectID == projectID.Value && a.Permission > 0)
                                  || DataContext.ProjectDataMarts.Where(pd => pd.ProjectID == projectID.Value).Any(dm => dm.DataMart.DataMartRequestTypeAcls.Any(a => a.RequestTypeID == r.ID && a.Permission > 0))
                                  ))
                              //.Where(r => r.ProcessorID.HasValue && DataContext.Projects.Where(p => p.ID == projectID.Value && !p.Deleted).Any())
                              .Where(r => r.ProcessorID.HasValue && DataContext.Projects.Where(p => !p.Deleted).Any())
                              .Select(r => r.ID)
                              .ToSet();
        }

        public IDictionary<IDnsModel, PluginRequestType[]> GetGrantedRequestTypesByModel(Guid? projectID)
        {
            var pluginRequestTypes = GetGrantedRequestTypeIDs(projectID)
                    .Select(id => Plugins.GetPluginRequestType(id))
                    .Where(x => x != null);

            return pluginRequestTypes.GroupBy(x => x.Model).ToDictionary(x => x.Key, k => k.ToArray());
        }

        public IDictionary<Guid, PluginRequestType> GetGrantedRequestTypes(Guid? projectID)
        {
            return GetGrantedRequestTypeIDs(projectID)
                .Select(id => new { id, req = Plugins.GetPluginRequestType(id) })
                .Where(x => x.req != null)
                .ToDictionary(x => x.id, x => x.req);
        }

        IEnumerable<Guid> GrantedDataMartIDs(Project project, Guid requestTypeID, Guid modelID)
        {
            return GetGrantedDataMarts(project, requestTypeID, modelID).Select(dm => dm.ID);
        }

        DnsResult NonGrantedDataMartsResult(string requestTypeName, IEnumerable<Guid> datamartIDs)
        {
            var nonGrantedDmNames = DataContext.DataMarts.Where(dm => datamartIDs.Contains(dm.ID)).Select(dm => dm.Name);
            return DnsResult.Failed("You do not have permission to submit requests of type '" + requestTypeName + "' to the following data marts: " + string.Join(", ", nonGrantedDmNames));
        }

        

        private IEnumerable<System.Action> RequestTransaction_UpdateDocuments(DnsRequestTransaction trn)
        {
            if (trn.UpdateDocuments == null || !trn.UpdateDocuments.Any()) 
                return Enumerable.Empty<System.Action>();

            var updateIds = trn.UpdateDocuments.Keys.Select(k => k.ID).ToArray();
            return
                DataContext.Documents
                .Where(d => updateIds.Contains(d.ID))
                .ToList()
                .Select(d =>
                {
                    var from = trn.UpdateDocuments.First(u => u.Key.ID == d.ID).Value;
                    d.Viewable = from.Viewable;
                    d.MimeType = from.MimeType;
                    d.FileName = d.FileName;
                    d.Name = from.Name;
                    d.Kind = from.Kind;
                    return new { from, d };
                })
                .ToList()
                .Select(x => new System.Action(() =>
                {
                    x.d.SetData(DataContext, x.from.ReadStream().ToArray());
                }));
        }

        public RequestHeader CreateHeader(Request r)
        {
            return new RequestHeader
            {
                Name = r.Name,
                Description = r.Description,
                AdditionalInstructions = r.AdditionalInstructions,
                PurposeOfUse = r.PurposeOfUse,
                PhiDisclosureLevel = r.PhiDisclosureLevel,
                Priority = r.Priority,
                ActivityID = r.ActivityID,
                ActivityDescription = r.ActivityDescription,
                SourceActivityID = r.SourceActivityID,
                SourceActivityProjectID = r.SourceActivityProjectID,
                SourceTaskOrderID = r.SourceTaskOrderID,
                DueDate = r.DueDate == null ? "" : r.DueDate.Value.ToString("MM/dd/yyyy"),
                WorkplanTypeID = r.WorkplanTypeID,
                RequesterCenterID = r.RequesterCenterID,
                MirrorBudgetFields = r.MirrorBudgetFields,
                MSRequestID = r.MSRequestID,
                ReportAggregationLevelID = r.ReportAggregationLevelID

            };
        }

        void ApplyHeader(RequestHeader h, Request r)
        {
            if (string.IsNullOrWhiteSpace(h.Name)) 
                throw new InvalidOperationException("Request name cannot be empty");

            if (h.MSRequestID != null && h.MSRequestID != "" &&
                (DataContext.Requests.Any(req => req.MSRequestID == h.MSRequestID && req.ID != r.ID)))
            {
                throw new InvalidOperationException("The Request ID entered is not unique. Please enter in a different Request ID.");
            }

            r.Name = h.Name;
            r.PurposeOfUse = h.PurposeOfUse;
            r.PhiDisclosureLevel = h.PhiDisclosureLevel;
            r.Description = h.Description ?? string.Empty;
            r.AdditionalInstructions = h.AdditionalInstructions ?? string.Empty;
            r.Priority = (Priorities)h.Priority;
            r.Activity = h.ActivityID == null ? null : DataContext.Activities.SingleOrDefault(a => a.ID == h.ActivityID);
            r.ActivityDescription = h.ActivityDescription ?? string.Empty;
            r.SourceActivity = h.SourceActivityID == null ? null : DataContext.Activities.SingleOrDefault(a => a.ID == h.SourceActivityID);
            r.SourceActivityProject = h.SourceActivityProjectID == null ? null : DataContext.Activities.SingleOrDefault(a => a.ID == h.SourceActivityProjectID);
            r.SourceTaskOrder = h.SourceTaskOrderID == null ? null : DataContext.Activities.SingleOrDefault(a => a.ID == h.SourceTaskOrderID);
            r.RequesterCenterID = h.RequesterCenterID;
            r.WorkplanTypeID = h.WorkplanTypeID;
            //r.MirrorBudgetFields = h.MirrorBudgetFields;
            r.MirrorBudgetFields = Convert.ToBoolean(h.MirrorBudgetFields);
            r.MSRequestID = h.MSRequestID;
            r.ReportAggregationLevelID = h.ReportAggregationLevelID;

            if (h.MSRequestID == null || h.MSRequestID == "")
            {
                r.MSRequestID = "Request " + r.Identifier.ToString();
            }

            DateTime dt;
            if (DateTime.TryParse(h.DueDate, out dt))
            {
                r.DueDate = dt.AddHours(12);
            }
        }        

        void CopyStreams(Func<Stream> openInput, Func<Stream> openOutput)
        {
            using (var inStream = openInput())
            using (var outStream = openOutput())
                inStream.CopyTo(outStream);
        }        
    }
}