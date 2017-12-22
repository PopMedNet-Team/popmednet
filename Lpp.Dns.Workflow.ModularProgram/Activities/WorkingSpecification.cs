using Lpp.Dns.Data;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.Entity;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Logging;

namespace Lpp.Dns.Workflow.ModularProgram.Activities
{
    public class WorkingSpecification : ActivityBase<Request>
    {
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        static readonly Guid TerminateResultID = new Guid("53579F36-9D20-47D9-AC33-643D9130080B");
        static readonly Guid WorkingSpecificationReviewID = new Guid("2BEF97A9-1A3A-46F8-B1D1-7E9E6F6F902A");
        static readonly Guid SpecificationReviewID = new Guid("14B7E8CF-4CF2-4C3D-A97E-E69C5D090FC0");

        private const string DocumentKind = "Lpp.Dns.Workflow.ModularProgram.Activities.WorkingSpecification";
        
        public override Guid ID
        {
            get { return ModularProgramWorkflowConfiguration.WorkingSpecificationID; }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + this._workflow.Entity.ID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Working Specifications";
            }
        }

        public override string CustomTaskSubject
        {
            get
            {
                return "Submit Working Specifications";
            }
        }

        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultIDRequired };
            }

            if (activityResultID.Value != TerminateResultID &&
                activityResultID.Value != SaveResultID &&
                activityResultID.Value != WorkingSpecificationReviewID &&
                activityResultID.Value != SpecificationReviewID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultNotSupported };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow);

            if (activityResultID.Value == TerminateResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.TerminateWorkflow))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToTerminateWorkflow };
            }

            if (activityResultID.Value == SaveResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToEditTask };
            }

            if ((activityResultID.Value == SpecificationReviewID || activityResultID.Value == WorkingSpecificationReviewID) && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }

            return new ValidationResult { Success = true };
        }

        public override async Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);

           

            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ID, db);

            if (activityResultID.Value == SaveResultID)
            {
                await task.LogAsModifiedAsync(_workflow.Identity, db);
                await db.SaveChangesAsync();
                //The request criteria has been already set on the Request.Query field, do not need to create any documents at this point
                return new CompletionResult
                {
                    ResultID = activityResultID.Value
                };
            }

            
            if (activityResultID.Value == WorkingSpecificationReviewID || activityResultID.Value == SpecificationReviewID)
            {
                var originalStatus = _entity.Status;

                //if skipping upload for specification review should copy over the current revision of any uploaded documents to that step.
                if (activityResultID.Value == SpecificationReviewID)
                {
                    //skipping working specification review and going straight to specification review,

                    var specificationTask = await PmnTask.GetActiveTaskForRequestActivityAsync(_entity.ID, ModularProgramWorkflowConfiguration.SpecificationID, db);
                    if (specificationTask == null)
                    {
                        //specificationTask = PmnTask.CreateForWorkflowActivity(Request.ID, ModularProgramWorkflowConfiguration.SpecificationReviewID, Workflow.ID, db);
                        specificationTask = db.Actions.Add(PmnTask.CreateForWorkflowActivity(_entity.ID, ModularProgramWorkflowConfiguration.SpecificationID, _workflow.ID, db));

                        await db.SaveChangesAsync();
                    }

                    //copy the current revision of any activity specific documents from this step to the specification step first. These are the documents uploaded from the activity tab, not the Documents tab.
                    
                    IEnumerable<Document> workingSpecificationDocuments = await (from d in db.Documents.AsNoTracking()
                                                                                 join x in (
                                                                                     db.Documents.Where(dd => dd.ItemID == task.ID)
                                                                                     .GroupBy(k => k.RevisionSetID)
                                                                                     .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                                                                 ) on d.ID equals x
                                                                                 where db.ActionReferences.Any(tr => tr.ItemID == d.ID && tr.Type == DTO.Enums.TaskItemTypes.ActivityDataDocument)
                                                                                 orderby d.ItemID descending, d.RevisionSetID descending, d.CreatedOn descending
                                                                                 select d).ToArrayAsync();
                    
                    List<Document> copiedDocuments = new List<Document>();
                    foreach (Document wsDocument in workingSpecificationDocuments)
                    {
                        Document sDocument = new Document
                        {
                            BuildVersion = wsDocument.BuildVersion,
                            CreatedOn = DateTime.UtcNow,
                            Description = wsDocument.Description,
                            FileName = wsDocument.FileName,
                            ItemID = specificationTask.ID,
                            Kind = wsDocument.Kind,
                            Length = wsDocument.Length,
                            MajorVersion = wsDocument.MajorVersion,
                            MimeType = wsDocument.MimeType,
                            MinorVersion = wsDocument.MinorVersion,
                            Name = wsDocument.Name,
                            ParentDocumentID = wsDocument.ID,
                            UploadedByID = _workflow.Identity.ID,
                            Viewable = wsDocument.Viewable
                        };

                        sDocument.RevisionSetID = sDocument.ID;
                        sDocument.RevisionDescription = "Copied from Working Specifications.";

                        copiedDocuments.Add(db.Documents.Add(sDocument));

                        specificationTask.References.Add(new TaskReference { ItemID = sDocument.ID, Type = DTO.Enums.TaskItemTypes.ActivityDataDocument });
                    }


                    await db.SaveChangesAsync();

                    //copy the document content
                    foreach (Document doc in copiedDocuments)
                    {
                        doc.CopyData(db, doc.ParentDocumentID.Value);
                    }

                    await MarkTaskComplete(specificationTask);

                    await SetRequestStatus(DTO.Enums.RequestStatuses.SpecificationsPendingReview);

                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.SpecificationsPendingReview);

                }
                else {

                    await SetRequestStatus(DTO.Enums.RequestStatuses.WorkingSpecificationPendingReview);

                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.WorkingSpecificationPendingReview);
                }

                await MarkTaskComplete(task);
                
                return new CompletionResult
                {
                    ResultID = activityResultID.Value
                };
            }
            else if (activityResultID.Value == TerminateResultID)
            {
                _entity.CancelledByID = _workflow.Identity.ID;
                _entity.CancelledOn = DateTime.UtcNow;

                task.Status = DTO.Enums.TaskStatuses.Cancelled;

                await db.SaveChangesAsync();

                return new CompletionResult
                {
                    ResultID = TerminateResultID
                };
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }
        }
        

    }
}
