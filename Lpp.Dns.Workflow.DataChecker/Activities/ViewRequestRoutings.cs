using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security;
using Lpp.Utilities.Logging;

namespace Lpp.Dns.Workflow.DataChecker.Activities
{
    /// <summary>
    /// Now called View Responses
    /// </summary>
    public class ViewRequestRoutings : ActivityBase<Request>
    {
        static readonly Guid GroupResultID = new Guid("49F9C682-9FAD-4AE5-A2C5-19157E227186");
        static readonly Guid UngroupResultID = new Guid("7821FC45-9FD5-4597-A405-B021E5ED14FA");
        
        static readonly Guid AddDataMartsResultID = new Guid("15BDEF13-6E86-4E0F-8790-C07AE5B798A8");
        static readonly Guid RemoveDataMartsResultID = new Guid("5E010001-1353-44E9-9204-A3B600E263E9");
        static readonly Guid EditRoutingStatusResultID = new Guid("3CF0FEA0-26B9-4042-91F3-7192D44F6F7C");
        static readonly Guid RoutingsBulkEditID = new Guid("4F7E1762-E453-4D12-8037-BAE8A95523F7");

        static readonly Guid ResubmitResultID = new Guid("22AE0001-0B5A-4BA9-BB55-A3B600E2728C");
        static readonly Guid CompleteResultID = new Guid("E1C90001-B582-4180-9A71-A3B600EA0C27");
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        static readonly Guid[] EditTaskResults = new[] { GroupResultID, UngroupResultID, AddDataMartsResultID, RemoveDataMartsResultID, EditRoutingStatusResultID, SaveResultID, RoutingsBulkEditID };
        static readonly Guid[] CloseTaskResults = new[] { ResubmitResultID, CompleteResultID };
        
        public override Guid ID
        {
            get { return DataCheckerWorkflowConfiguration.ViewRequestRoutingsID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Complete Distribution";
            }
        }

        public override string Uri
        {
            get { return "requests/details?ID=" + _entity.ID; }
        }

        public override async Task<Lpp.Workflow.Engine.ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                return new ValidationResult
                {
                    Success = false,
                    Errors = CommonMessages.ActivityResultIDRequired
                };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity);

            if (EditTaskResults.Contains(activityResultID.Value) && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask))
            {
                return new ValidationResult
                {
                    Success = false,
                    Errors = CommonMessages.RequirePermissionToEditTask
                };
            }

            if (CloseTaskResults.Contains(activityResultID.Value) && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask))
            {
                return new ValidationResult
                {
                    Success = false,
                    Errors = CommonMessages.RequirePermissionToCloseTask
                };
            }

            if (activityResultID.HasValue && activityResultID.Value == ResubmitResultID)
            {
                if (!await db.HasPermissions<Project>(_workflow.Identity, _entity.ProjectID, PermissionIdentifiers.Project.ResubmitRequests))
                {
                    return new ValidationResult
                    {
                        Success = false,
                        Errors = CommonMessages.RequirePermissionToResubmitRequest
                    };
                }
            }

            return await Task.Run(() => new ValidationResult
            {
                Success = true
            });
        }

        public override async Task<Lpp.Workflow.Engine.CompletionResult> Complete(string data, Guid? activityResultID)
        {
            //Ad params that come through using Json and deserialize on each activity result and handle the logic.
            if (!activityResultID.HasValue)
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);

            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }

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

                //log the save against the current task
                await LogTaskModified();

                await db.SaveChangesAsync();
            }
            else if (activityResultID.Value == UngroupResultID)
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
                    group.Responses.Clear();
                    db.ResponseGroups.Remove(group);
                }

                var routings = await db.RequestDataMarts.Where(dm => requestDataMartIDs.Contains(dm.ID)).ToArrayAsync();
                foreach (var routing in routings)
                {
                    routing.ResultsGrouped = false;
                }

                await LogTaskModified();
                await db.SaveChangesAsync();
            }
            else if (activityResultID.Value == AddDataMartsResultID)
            {
                string[] guids = data.Split(',');

                var revisionSetIDs = await (from req in db.Requests
                                            join rdm in db.RequestDataMarts on req.ID equals rdm.RequestID
                                            join res in db.Responses on rdm.ID equals res.RequestDataMartID
                                            join reqDoc in db.RequestDocuments on res.ID equals reqDoc.ResponseID
                                            where req.ID == _entity.ID && reqDoc.DocumentType == DTO.Enums.RequestDocumentType.Input
                                            select reqDoc.RevisionSetID).Distinct().ToArrayAsync();

                var allTasks = await db.ActionReferences.Where(tr => tr.ItemID == _entity.ID
                                         && tr.Type == DTO.Enums.TaskItemTypes.Request
                                         && tr.Task.Type == DTO.Enums.TaskTypes.Task
                                        )
                                        .Select(tr => tr.Task.ID).ToArrayAsync();

                var attachments = await (from doc in db.Documents.AsNoTracking()
                                         join x in (
                                                 db.Documents.Where(dd => allTasks.Contains(dd.ItemID))
                                                 .GroupBy(k => k.RevisionSetID)
                                                 .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                             ) on doc.ID equals x
                                         where allTasks.Contains(doc.ItemID) && doc.Kind == "Attachment.Input"
                                         orderby doc.ItemID descending, doc.RevisionSetID descending, doc.CreatedOn descending
                                         select doc).ToArrayAsync();

                foreach (var guid in guids)
                {
                    Guid dmGuid = new Guid(guid);
                    var dm = RequestDataMart.Create(_entity.ID, dmGuid, _workflow.Identity.ID);
                    dm.Status = DTO.Enums.RoutingStatus.Submitted;
                    dm.Priority = _entity.Priority;
                    dm.DueDate = _entity.DueDate;
                    _entity.DataMarts.Add(dm);
                    foreach(var revset in revisionSetIDs)
                        db.RequestDocuments.Add(new RequestDocument { RevisionSetID = revset, ResponseID = dm.Responses.FirstOrDefault().ID, DocumentType = DTO.Enums.RequestDocumentType.Input });

                    foreach (var attachment in attachments)
                    {
                        db.RequestDocuments.Add(new RequestDocument { RevisionSetID = attachment.RevisionSetID.Value, ResponseID = dm.Responses.FirstOrDefault().ID, DocumentType = DTO.Enums.RequestDocumentType.AttachmentInput });
                    }
                }
                await LogTaskModified();
                await db.SaveChangesAsync();
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
            else if (activityResultID.Value == EditRoutingStatusResultID)
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
            else if (activityResultID.Value == ResubmitResultID)
            {
                //data will contain a list of routings to resubmit, the ID may also be for a group
                ResubmitRoutingsModel resubmitModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResubmitRoutingsModel>(data);

                var datamarts = await (
                            from dm in db.RequestDataMarts.Include(dm => dm.Responses)
                            where dm.RequestID == _entity.ID
                            && dm.Responses.Any(r => resubmitModel.Responses.Contains(r.ID) || (r.ResponseGroupID.HasValue && resubmitModel.Responses.Contains(r.ResponseGroupID.Value)))
                            select dm
                    ).ToArrayAsync();
                
                var revisions = await (from rdm in db.RequestDataMarts
                                       join rdmr in db.Responses on rdm.ID equals rdmr.RequestDataMartID
                                       join rdoc in db.RequestDocuments on rdmr.ID equals rdoc.ResponseID
                                       where rdoc.DocumentType == DTO.Enums.RequestDocumentType.Input && rdm.RequestID == _entity.ID
                                       select new
                                       {
                                           DataMartID = rdm.ID,
                                           RevisionSetID = rdoc.RevisionSetID,
                                           ResponseID = rdoc.ResponseID
                                       }).ToArrayAsync();

                var allTasks = await db.ActionReferences.Where(tr => tr.ItemID == _entity.ID
                                                  && tr.Type == DTO.Enums.TaskItemTypes.Request
                                                  && tr.Task.Type == DTO.Enums.TaskTypes.Task
                                                 )
                                                 .Select(tr => tr.Task.ID).ToArrayAsync();

                //attachments are associated to the task they were uploaded on, any existing attachment should be included with the routing
                var attachments = await (from doc in db.Documents.AsNoTracking()
                                         join x in (
                                                 db.Documents.Where(dd => allTasks.Contains(dd.ItemID))
                                                 .GroupBy(k => k.RevisionSetID)
                                                 .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                             ) on doc.ID equals x
                                         where allTasks.Contains(doc.ItemID) && doc.Kind == "Attachment.Input"
                                         orderby doc.ItemID descending, doc.RevisionSetID descending, doc.CreatedOn descending
                                         select doc).ToArrayAsync();


                DateTime reSubmittedOn = DateTime.UtcNow;
                foreach (var dm in datamarts)
                {
                    var response = dm.AddResponse(_workflow.Identity.ID);
                    response.SubmittedOn = reSubmittedOn;
                    response.SubmitMessage = resubmitModel.ResubmissionMessage;
                    dm.Status = DTO.Enums.RoutingStatus.Resubmitted;

                    foreach (var revset in revisions.Where(x => x.DataMartID == dm.ID).Distinct())
                    {
                        db.RequestDocuments.Add(new RequestDocument { RevisionSetID = revset.RevisionSetID, ResponseID = response.ID, DocumentType = DTO.Enums.RequestDocumentType.Input });
                    }

                    foreach (var attachment in attachments)
                    {
                        db.RequestDocuments.Add(new RequestDocument { RevisionSetID = attachment.RevisionSetID.Value, ResponseID = response.ID, DocumentType = DTO.Enums.RequestDocumentType.AttachmentInput });
                    }
                    
                }

                _entity.Status = DTO.Enums.RequestStatuses.Submitted;
                var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ID, db);

                await LogTaskModified();

                if (task != null)
                {
                    task.EndOn = DateTime.UtcNow;
                    task.Status = DTO.Enums.TaskStatuses.Complete;
                    task.PercentComplete = 100;
                    await db.SaveChangesAsync();
                }

                await db.SaveChangesAsync();

            }
            else if (activityResultID.Value == CompleteResultID)
            {
                var originalStatus = _entity.Status;
                if (originalStatus != DTO.Enums.RequestStatuses.Complete && originalStatus != DTO.Enums.RequestStatuses.CompleteWithReport)
                {
                    await SetRequestStatus(DTO.Enums.RequestStatuses.Complete);

                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Complete);
                }

                var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ID, db);
                await MarkTaskComplete(task);
                
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
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
