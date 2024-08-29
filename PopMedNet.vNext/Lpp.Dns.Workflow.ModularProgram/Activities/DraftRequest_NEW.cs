using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Lpp.Dns.DTO.Security;
using System.Data.SqlClient;

namespace Lpp.Dns.Workflow.ModularProgram.Activities
{
    public class DraftRequest : IActivity<DataContext, Request>
    {
        static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        static readonly Guid SubmitResultID = new Guid("6248E8B1-7C7C-4959-993F-352C722821A6");
        static readonly Guid DeleteResultID = new Guid("7E8661F2-E540-4E91-A3CF-982DB52EF965");

        private const string DocumentKind = "Lpp.Dns.Workflow.ModularProgram.Activities.DraftRequest";

        Workflow<DataContext, Request> Workflow;
        DataContext db;
        Request Request;

        public void Initialize(Workflow<DataContext, Request> workflow)
        {
            this.Workflow = workflow;
            this.db = (DataContext)workflow.DataContext;
            this.Request = this.Workflow.Entity;
        }

        public Guid ID
        {
            get { return ModularProgramWorkflowConfiguration.DraftRequestID; }
        }

        public string Uri
        {
            get { return "/requests/details?ID=" + this.Workflow.Entity.ID; }
        }

        public async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            //default to the save result ID if not specified.
            if (!activityResultID.HasValue)
                activityResultID = SaveResultID;

            if (activityResultID.Value != SaveResultID && 
                activityResultID.Value != DeleteResultID && 
                activityResultID.Value != SubmitResultID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.ActivityResultNotSupported };
            }

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(Workflow.Identity, Workflow.Entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask);

            if (activityResultID.Value == SaveResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToEditTask };
            }

            if (activityResultID.Value == SubmitResultID && !permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }

            return new ValidationResult
            {
                Success = true
            };
        }

        public async Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {

            if (!activityResultID.HasValue)
                throw new ArgumentNullException(CommonMessages.ActivityResultIDRequired);

            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(Request.ID, ID, db);

            if (activityResultID.Value == SaveResultID ||
                activityResultID.Value == SubmitResultID)
            {
                if (activityResultID.Value == SubmitResultID)
                {
                    task.Status = DTO.Enums.TaskStatuses.Complete;
                    task.EndOn = DateTime.UtcNow;
                }
                else
                {
                    await task.LogAsModifiedAsync(Workflow.Identity, db);
                }

                await db.SaveChangesAsync();

                return new CompletionResult
                {
                    ResultID = activityResultID.Value
                };
            }
            else if (activityResultID.Value == DeleteResultID)
            {
                Request.CancelledByID = Workflow.Identity.ID;
                Request.CancelledOn = DateTime.UtcNow;

                task.Status = DTO.Enums.TaskStatuses.Cancelled;
                await db.SaveChangesAsync();

                return new CompletionResult
                {
                    ResultID = DeleteResultID
                };
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }


        }

        public async Task Start(string comment)
        {
            var task = await PmnTask.GetActiveTaskForRequestActivityAsync(Request.ID, ID, db);
            if (task == null)
            {
                task = db.Actions.Add(PmnTask.CreateForWorkflowActivity(Request.ID, ID, Workflow.ID, db));
                await db.SaveChangesAsync();
            }

            if (!string.IsNullOrWhiteSpace(comment))
            {
                var cmt = db.Comments.Add(new Comment
                {
                    CreatedByID = Workflow.Identity.ID,
                    ItemID = Request.ID,
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
    }
}
