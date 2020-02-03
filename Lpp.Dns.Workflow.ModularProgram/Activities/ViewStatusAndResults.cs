using Lpp.Dns.Data;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Logging;
using System.Security;

namespace Lpp.Dns.Workflow.ModularProgram.Activities
{
    public class ViewStatusAndResults : ActivityBase<Request>
    {
        static readonly Guid GroupResultID = new Guid("49F9C682-9FAD-4AE5-A2C5-19157E227186");
        static readonly Guid UngroupResultID = new Guid("7821FC45-9FD5-4597-A405-B021E5ED14FA");

        static readonly Guid CompleteWorkflowResultID = new Guid("E93CED3B-4B55-4991-AF84-07058ABE315C");

        static readonly Guid CompleteResultID = new Guid("EBEF0001-09B6-4BDE-89A2-A3F4012D282C");
        static readonly Guid TerminateResultID = new Guid("53579F36-9D20-47D9-AC33-643D9130080B");
        static readonly Guid RedistributeResultID = new Guid("5C5E0001-10A6-4992-A8BE-A3F4012D5FEB");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        static readonly Guid EditRoutingStatusResultID = new Guid("3CF0FEA0-26B9-4042-91F3-7192D44F6F7C");
        static readonly Guid RoutingsBulkEditID = new Guid("4F7E1762-E453-4D12-8037-BAE8A95523F7");
        static readonly Guid RemoveDataMartsResultID = new Guid("5E010001-1353-44E9-9204-A3B600E263E9");
        static readonly Guid AddDataMartsResultID = new Guid("15BDEF13-6E86-4E0F-8790-C07AE5B798A8");

        public override Guid ID
        {
            get { return SimpleModularProgramWorkflowConfiguration.ViewStatusAndResultsID; }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + _workflow.Entity.ID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Complete Distribution";
            }
        }

        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultIDRequired };
            }

            if (activityResultID.Value != TerminateResultID &&
                activityResultID.Value != CompleteResultID &&
                activityResultID.Value != RedistributeResultID &&
                activityResultID.Value != SaveResultID &&
                activityResultID.Value != EditRoutingStatusResultID &&
                activityResultID.Value != RoutingsBulkEditID &&
                activityResultID.Value != GroupResultID &&
                activityResultID.Value != UngroupResultID &&
                activityResultID.Value != CompleteWorkflowResultID &&
                activityResultID.Value != AddDataMartsResultID &&
                activityResultID.Value != RemoveDataMartsResultID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultNotSupported };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow);

            if (activityResultID.Value == TerminateResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToTerminateWorkflow };
            }

            if (activityResultID.Value == RedistributeResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToEditTask };
            }

            if (activityResultID.Value == CompleteResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }

            return new ValidationResult { Success = true };
        }

        public override async Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);

            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_workflow.Entity.ID, ID, db);
            if (task == null)
            {
                task = db.Actions.Add(PmnTask.CreateForWorkflowActivity(_workflow.Entity.ID, ID, _workflow.ID, db));
            }

            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }

            if (activityResultID.Value == EditRoutingStatusResultID)
            {
                var dataMarts = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<RoutingChangeRequestModel>>(data);
                await UpdateDataMartRoutingStatus(dataMarts);

                return new CompletionResult
                {
                    ResultID = EditRoutingStatusResultID
                };
            }
            else if (activityResultID.Value == RoutingsBulkEditID)
            {

                return new CompletionResult
                {
                    ResultID = RoutingsBulkEditID
                };
            }
            else if (activityResultID.Value == RemoveDataMartsResultID)
            {
                Guid[] guids = data.Split(',').Select(s => Guid.Parse(s)).ToArray();
                var routings = await db.RequestDataMarts.Where(dm => guids.Contains(dm.ID)).ToArrayAsync();

                foreach (var routing in routings)
                {
                    routing.Status = DTO.Enums.RoutingStatus.Canceled;
                }

                await LogTaskModified();
                await db.SaveChangesAsync();

                var originalStatus = _entity.Status;
                await db.SaveChangesAsync();

                await db.Entry(_entity).ReloadAsync();

                if (originalStatus != DTO.Enums.RequestStatuses.Complete && _entity.Status == DTO.Enums.RequestStatuses.Complete)
                {
                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Complete);
                }
            }
            else if (activityResultID.Value == AddDataMartsResultID)
            {
                DTO.QueryComposer.QueryComposerRequestDTO requestDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(_entity.Query);
                var modularTerm = requestDTO.Where.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == SimpleModularProgramWorkflowConfiguration.ModularProgramTermID)).FirstOrDefault();
                var termValues = Newtonsoft.Json.JsonConvert.DeserializeObject<ModularProgramTermValues>(modularTerm.Values["Values"].ToString());

                string[] datamartIDs = data.Split(',');

                foreach (var guid in datamartIDs)
                {
                    Guid dmGuid = new Guid(guid);

                    var dm = RequestDataMart.Create(_entity.ID, dmGuid, _workflow.Identity.ID);
                    dm.Status = DTO.Enums.RoutingStatus.Submitted;
                    dm.DueDate = _entity.DueDate;
                    dm.Priority = _entity.Priority;
                    _entity.DataMarts.Add(dm);

                    Response rsp = dm.Responses.OrderByDescending(r => r.Count).FirstOrDefault();
                    //add the request document associations
                    foreach(var revisionSetID in termValues.Documents.Select(d => d.RevisionSetID))
                    {
                        db.RequestDocuments.Add(new RequestDocument { DocumentType = DTO.Enums.RequestDocumentType.Input, ResponseID = rsp.ID, RevisionSetID = revisionSetID });
                    }

                }

                await LogTaskModified();
                await db.SaveChangesAsync();
            }
            //Grouping
            if (activityResultID.Value == GroupResultID)
            {
                GroupingRequestModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<GroupingRequestModel>(data);

                var responses = await db.Responses.Include(r => r.RequestDataMart).Where(r => model.Responses.Any(x => x == r.ID)).ToArrayAsync();

                if (responses.Select(r => r.RequestDataMart.RequestID).Distinct().Count() > 1)
                {
                    throw new ArgumentException("Cannot group responses that come from different requests.");
                }

                Guid[] requestDataMartIDs = responses.Select(rsp => rsp.RequestDataMartID).Distinct().ToArray();
                var pq = (from rdm in db.RequestDataMarts
                          let permissionID = PermissionIdentifiers.DataMartInProject.GroupResponses.ID
                          let identityID = _workflow.Identity.ID
                          let acls = db.GlobalAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID)).Select(a => a.Allowed)
                          .Concat(db.ProjectAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.Project.Requests.Any(r => r.ID == rdm.RequestID)).Select(a => a.Allowed))
                          .Concat(db.DataMartAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                          .Concat(db.ProjectDataMartAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.Project.Requests.Any(r => r.ID == rdm.RequestID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                          .Concat(db.OrganizationAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.Organization.Requests.Any(r => r.ID == rdm.RequestID)).Select(a => a.Allowed))
                          where requestDataMartIDs.Contains(rdm.ID)
                          && acls.Any() && acls.All(a => a == true)
                          select rdm.ID);

                var allowedResponses = await pq.ToArrayAsync();


                if (allowedResponses.Length != requestDataMartIDs.Length)
                {
                    throw new SecurityException("Insufficient permission to group one or more of the specified responses.");
                }

                if (string.IsNullOrWhiteSpace(model.GroupName))
                {
                    model.GroupName = string.Join(", ", db.Responses.Where(r => model.Responses.Any(x => x == r.ID)).Select(r => r.RequestDataMart.DataMart.Name).Distinct());
                }

                ResponseGroup grouping = db.ResponseGroups.Add(new ResponseGroup(model.GroupName));
                foreach (var response in responses)
                {
                    grouping.Responses.Add(response);
                    response.RequestDataMart.ResultsGrouped = true;
                }

                await LogTaskModified("Grouped results.");

                await db.SaveChangesAsync();
            }
            //Un-grouping
            if (activityResultID.Value == UngroupResultID)
            {
                IEnumerable<Guid> groupID = (from r in data.Split(',')
                                             where !string.IsNullOrWhiteSpace(r)
                                             select Guid.Parse(r.Trim())).ToArray();

                List<Guid> requestDataMartIDs = await db.ResponseGroups.Include(g => g.Responses).Where(g => groupID.Contains(g.ID)).SelectMany(g => g.Responses.Select(r => r.RequestDataMartID)).ToListAsync();

                var pq = (from rdm in db.RequestDataMarts
                          let permissionID = PermissionIdentifiers.DataMartInProject.GroupResponses.ID
                          let identityID = _workflow.Identity.ID
                          let acls = db.GlobalAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID)).Select(a => a.Allowed)
                          .Concat(db.ProjectAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.Project.Requests.Any(r => r.ID == rdm.RequestID)).Select(a => a.Allowed))
                          .Concat(db.DataMartAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                          .Concat(db.ProjectDataMartAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.Project.Requests.Any(r => r.ID == rdm.RequestID) && a.DataMartID == rdm.DataMartID).Select(a => a.Allowed))
                          .Concat(db.OrganizationAcls.Where(a => a.PermissionID == permissionID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.Organization.Requests.Any(r => r.ID == rdm.RequestID)).Select(a => a.Allowed))
                          where requestDataMartIDs.Contains(rdm.ID)
                          && acls.Any() && acls.All(a => a == true)
                          select rdm.ID);

                var allowedResponses = await pq.ToArrayAsync();


                if (allowedResponses.Length != requestDataMartIDs.Count)
                {
                    throw new SecurityException("Insufficient permission to ungroup routings for the specified request.");
                }

                var groups = await db.ResponseGroups.Include(g => g.Responses).Where(g => groupID.Contains(g.ID)).ToArrayAsync();
                foreach (var group in groups)
                {
                    requestDataMartIDs.AddRange(group.Responses.Select(r => r.RequestDataMartID));
                    group.Responses.Clear();
                    db.ResponseGroups.Remove(group);
                }

                var routings = await db.RequestDataMarts.Where(dm => requestDataMartIDs.Contains(dm.ID)).ToArrayAsync();
                foreach (var routing in routings)
                {
                    routing.ResultsGrouped = false;
                }

                await LogTaskModified("Ungrouped results.");

                await db.SaveChangesAsync();
            }

            if (activityResultID.Value == TerminateResultID)
            {
                _workflow.Entity.CancelledByID = _workflow.Identity.ID;
                _workflow.Entity.CancelledOn = DateTime.UtcNow;

                task.Status = DTO.Enums.TaskStatuses.Cancelled;
                await db.SaveChangesAsync();

                return new CompletionResult
                {
                    ResultID = TerminateResultID
                };
            }

            if (activityResultID.Value == RedistributeResultID)
            {
                if (!await db.HasPermissions<Project>(_workflow.Identity, _entity.ProjectID, Lpp.Dns.DTO.Security.PermissionIdentifiers.Project.ResubmitRequests))
                {
                    throw new System.Security.SecurityException(CommonMessages.RequirePermissionToResubmitRequest, Lpp.Dns.DTO.Security.PermissionIdentifiers.Project.ResubmitRequests.GetType());
                }

                //TODO: is this applicable in workflow?
                //bool canSkipSubmissionApproval = await db.HasPermissions<Request>(Workflow.Identity, Request.ID, Lpp.Dns.DTO.Security.PermissionIdentifiers.Request.SkipSubmissionApproval);

                //data will contain a list of routings to resubmit, the ID may also be for a group
                ResubmitRoutingsModel resubmitModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResubmitRoutingsModel>(data);

                var datamarts = await (
                            from dm in db.RequestDataMarts.Include(dm => dm.Responses)
                            where dm.RequestID == _entity.ID
                            && dm.Responses.Any(r => resubmitModel.Responses.Contains(r.ID) || (r.ResponseGroupID.HasValue && resubmitModel.Responses.Contains(r.ResponseGroupID.Value)))
                            select dm
                    ).ToArrayAsync();

                //get the previously submitted input RequestDocument links
                var previousInputRequestDocuments = await (from rd in db.RequestDocuments
                                                     where rd.DocumentType == DTO.Enums.RequestDocumentType.Input
                                                     && resubmitModel.Responses.Contains(rd.ResponseID)
                                                     select new { RequestDataMartID = rd.Response.RequestDataMartID, RevisionSetID = rd.RevisionSetID }).ToArrayAsync();

                DateTime reSubmittedOn = DateTime.UtcNow;
                foreach (var dm in datamarts)
                {

                    var response = dm.AddResponse(_workflow.Identity.ID);
                    response.SubmittedOn = reSubmittedOn;
                    response.SubmitMessage = resubmitModel.ResubmissionMessage;

                    dm.Status = DTO.Enums.RoutingStatus.Resubmitted;

                    foreach (var requestDoc in previousInputRequestDocuments.Where(rd => rd.RequestDataMartID == dm.ID))
                    {
                        db.RequestDocuments.Add(new RequestDocument { ResponseID = response.ID, RevisionSetID = requestDoc.RevisionSetID, DocumentType = DTO.Enums.RequestDocumentType.Input });
                    }
                }
                
                await SetRequestStatus(DTO.Enums.RequestStatuses.Submitted);

                await LogTaskModified();

                await MarkTaskComplete(task);

            }
            else if(activityResultID.Value == CompleteResultID)
            {
                var originalStatus = _entity.Status;
                await SetRequestStatus(DTO.Enums.RequestStatuses.PendingDraftReport);

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.PendingDraftReport);

                await MarkTaskComplete(task);
            }
            else if (activityResultID.Value == CompleteWorkflowResultID) 
            {
                var originalStatus = _entity.Status;
                if (originalStatus != DTO.Enums.RequestStatuses.Complete && originalStatus != DTO.Enums.RequestStatuses.CompleteWithReport)
                {
                    await SetRequestStatus(DTO.Enums.RequestStatuses.Complete);

                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Complete);
                }

                await MarkTaskComplete(task);

            }


            return new CompletionResult
            {
                ResultID = activityResultID.Value
            };
        }
        
    }
    public class GroupingRequestModel
    {
        public string GroupName { get; set; }

        public IEnumerable<Guid> Responses { get; set; }
    }

    public class ResubmitRoutingsModel
    {
        public IEnumerable<Guid> Responses { get; set; }

        public string ResubmissionMessage { get; set; }
    }
}
