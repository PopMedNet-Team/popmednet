using Lpp.Utilities;
using Lpp.Dns.Data;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Security;
using System.Data.Entity;

namespace Lpp.Dns.Workflow.DataChecker.Activities
{
    public class RequestReview : ActivityBase<Request>
    {
        private readonly Guid ApproveResultID = new Guid("50C60001-891F-40E6-B95F-A3B600E25C2B");
        private readonly Guid RejectResultID = new Guid("EA120001-7A35-4829-9F2D-A3B600E25013");
        private readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");

        public override Guid ID
        {
            get { return DataCheckerWorkflowConfiguration.ReviewRequestActivityID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Request Review";
            }
        }

        public override string Uri
        {
            get { return "requests/details?ID=" + _workflow.Entity.ID; }
        }

        public async override Task<ValidationResult> Validate(Guid? activityResultID)
        {
            var errors = new StringBuilder();

            if (activityResultID == null)
                errors.AppendHtmlLine(CommonMessages.ActivityResultIDRequired);

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask);

            var allowApproveRejectSubmission = await ApproveRejectSubmission();
            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask) || !allowApproveRejectSubmission)
                errors.AppendHtmlLine(CommonMessages.RequirePermissionToApproveOrRejectRequestSubmission);

            if (errors.Length > 0)
            {
                return new ValidationResult
                {
                    Success = false,
                    Errors = errors.ToString()
                };
            }
            else
            {
                return new ValidationResult
                {
                    Success = true
                };
            }
        }



        public async override Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (activityResultID.Value == SaveResultID)
            {
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }

            var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);

            var allTasks = await db.ActionReferences.Where(tr => tr.ItemID == _entity.ID
                                                     && tr.Type == DTO.Enums.TaskItemTypes.Request
                                                     && tr.Task.Type == DTO.Enums.TaskTypes.Task
                                                    )
                                                    .Select(tr => tr.Task.ID).ToArrayAsync();

            await db.LoadCollection(_entity, (r) => r.DataMarts);

            if (activityResultID.Value == ApproveResultID)
            {
                var previousStatus = await db.LogsRequestStatusChanged.Where(x => x.RequestID == _entity.ID && x.NewStatus == DTO.Enums.RequestStatuses.AwaitingRequestApproval).OrderByDescending(x => x.TimeStamp).FirstOrDefaultAsync();

                _entity.SubmittedByID = previousStatus.UserID;
                _entity.SubmittedOn = previousStatus.TimeStamp.UtcDateTime;

                var responses = await db.RequestDataMarts.Where(rdm => rdm.RequestID == _entity.ID && rdm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval).SelectMany(rdm => rdm.Responses.Where(rsp => rsp.Count == rdm.Responses.Max(rr => rr.Count))).ToArrayAsync();

                var previousTask = await (from a in db.Actions
                                          join ar in db.ActionReferences on a.ID equals ar.TaskID
                                          where ar.ItemID == _entity.ID && a.Status == DTO.Enums.TaskStatuses.Complete
                                          select a).OrderByDescending(p => p.CreatedOn).FirstOrDefaultAsync();

                var document = await (from d in db.Documents.AsNoTracking()
                                      join x in (
                                          db.Documents.Where(dd => dd.ItemID == previousTask.ID && dd.FileName == "request.json" && dd.Kind == Dns.DTO.Enums.DocumentKind.Request)
                                          .GroupBy(k => k.RevisionSetID)
                                          .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                      ) on d.ID equals x
                                      orderby d.ItemID descending, d.RevisionSetID descending, d.CreatedOn descending
                                      select d).FirstOrDefaultAsync();

                var attachments = await (from doc in db.Documents.AsNoTracking()
                                         join x in (
                                                 db.Documents.Where(dd => allTasks.Contains(dd.ItemID))
                                                 .GroupBy(k => k.RevisionSetID)
                                                 .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                             ) on doc.ID equals x
                                         where allTasks.Contains(doc.ItemID) && doc.Kind == "Attachment.Input"
                                         orderby doc.ItemID descending, doc.RevisionSetID descending, doc.CreatedOn descending
                                         select doc).ToArrayAsync();

                foreach (var dm in _entity.DataMarts.Where(dm => dm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval))
                {
                    dm.Status = DTO.Enums.RoutingStatus.Submitted;

                    var currentResponse = responses.Where(rsp => rsp.RequestDataMartID == dm.ID).FirstOrDefault();
                    if (currentResponse == null)
                    {
                        currentResponse = dm.AddResponse(_workflow.Identity.ID);
                    }
                    _entity.SubmittedByID = previousStatus.UserID;
                    _entity.SubmittedOn = previousStatus.TimeStamp.UtcDateTime;

                    var existingRequestDocuments = await db.RequestDocuments.Where(rd => rd.ResponseID == currentResponse.ID).ToArrayAsync();

                    if (!existingRequestDocuments.Any(rd => rd.RevisionSetID == document.RevisionSetID.Value))
                    {
                        db.RequestDocuments.Add(new RequestDocument { RevisionSetID = document.RevisionSetID.Value, ResponseID = currentResponse.ID, DocumentType = DTO.Enums.RequestDocumentType.Input });
                    }

                    foreach (var attachment in attachments.Where(att => !existingRequestDocuments.Any(ed => ed.RevisionSetID == att.RevisionSetID.Value)))
                    {
                        db.RequestDocuments.Add(new RequestDocument { RevisionSetID = attachment.RevisionSetID.Value, ResponseID = currentResponse.ID, DocumentType = DTO.Enums.RequestDocumentType.AttachmentInput });
                    }
                }

                _entity.Status = DTO.Enums.RequestStatuses.Submitted;

                await db.SaveChangesAsync();
                await db.Entry(_entity).ReloadAsync();

                await SetRequestStatus(DTO.Enums.RequestStatuses.Submitted);

                await MarkTaskComplete(task);

                await NotifyRequestStatusChanged(DTO.Enums.RequestStatuses.AwaitingRequestApproval, DTO.Enums.RequestStatuses.Submitted);

                return new CompletionResult
                {
                    ResultID = ApproveResultID
                };
            }
            else if (activityResultID.Value == RejectResultID)
            {
                //foreach (var dm in _entity.DataMarts.Where(dm => dm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval))
                //    dm.Status = DTO.Enums.RoutingStatus.RequestRejected;

                //_entity.Status = DTO.Enums.RequestStatuses.RequestRejected;
                //_entity.RejectedByID = _workflow.Identity.ID;
                //_entity.RejectedOn = DateTime.UtcNow;

                //var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);

                //var originalStatus = _entity.Status;
                //await SetRequestStatus(DTO.Enums.RequestStatuses.RequestRejected);

                //await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.RequestRejected);

                //await MarkTaskComplete(task);

                //await db.SaveChangesAsync();

                //return new CompletionResult
                //{
                //    ResultID = RejectResultID
                //};

                foreach (var dm in _entity.DataMarts.Where(dm => dm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval))
                {
                    dm.Status = DTO.Enums.RoutingStatus.RequestRejected;
                }

                await db.SaveChangesAsync();
                await db.Entry(_entity).ReloadAsync();

                _entity.RejectedByID = _workflow.Identity.ID;
                _entity.RejectedOn = DateTime.UtcNow;
                //Update the workflow activity to request composition
                _entity.WorkFlowActivityID = DataCheckerWorkflowConfiguration.ReviewRequestActivityID;

                await SetRequestStatus(DTO.Enums.RequestStatuses.RequestRejected);

                //create a completed task to show the request was rejected.
                PmnTask rejectedTask = db.Actions.Add(PmnTask.CreateForWorkflowActivity(_entity.ID, DataCheckerWorkflowConfiguration.ReviewRequestActivityID, _workflow.ID, db));
                rejectedTask.Subject = "Request Rejected Prior to Submission";
                rejectedTask.Status = DTO.Enums.TaskStatuses.InProgress;

                if (!data.IsNullOrEmpty())
                {
                    rejectedTask.Body = data.ToStringEx();

                    var cmt = db.Comments.Add(new Comment
                    {
                        CreatedByID = _workflow.Identity.ID,
                        ItemID = _entity.ID,
                        Text = data.ToStringEx()
                    });

                    db.CommentReferences.Add(new CommentReference
                    {
                        CommentID = cmt.ID,
                        Type = DTO.Enums.CommentItemTypes.Task,
                        ItemTitle = rejectedTask.Subject,
                        ItemID = rejectedTask.ID
                    });
                }

                await MarkTaskComplete(task);

                await NotifyRequestStatusChanged(DTO.Enums.RequestStatuses.AwaitingRequestApproval, DTO.Enums.RequestStatuses.RequestRejected);

                return null;
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }
        }

    }
}
