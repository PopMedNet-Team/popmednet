﻿using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Logging;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.SummaryQuery.Activities
{
    public class DraftRequest : ActivityBase<Request>
    {
        public static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        public static readonly Guid SubmitResultID = new Guid("F14B4432-804A-4052-A8EE-64260CE5DCB7");
        public static readonly Guid SubmitToDistributionID = new Guid("34285004-D933-4B27-AC7C-52F65B01891F");
        public static readonly Guid DeleteResultID = new Guid("10AC80A5-850B-49D1-9E13-AE6AE2D63701");

        public override Guid ID
        {
            get { return SummaryQueryWorkflowConfiguration.DraftRequestID; }
        }

        public override string ActivityName
        {
            get
            {
                return "Request Form";
            }
        }

        public override string Uri
        {
            get { return "/requests/details?ID=" + _entity.ID; }
        }

        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            //default to save result ID if not specified.
            if (!activityResultID.HasValue)
                activityResultID = SaveResultID;

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask);

            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask) && activityResultID.Value == SaveResultID)
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToEditTask };
            }

            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask) && (activityResultID.Value == SubmitResultID || activityResultID.Value == DeleteResultID))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }

            return new ValidationResult {
                Success = true
            };
        }

        public override async Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
                activityResultID = SaveResultID;

            var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);
            
            if (activityResultID.Value == SaveResultID)
            {
                await task.LogAsModifiedAsync(_workflow.Identity, db);
                await db.Entry(_entity).ReloadAsync();
                _entity.Private = false;
                await db.SaveChangesAsync();

                //Do nothing, it was already saved.
                return new CompletionResult
                {
                    ResultID = SaveResultID
                };
            }
            else if (activityResultID.Value == SubmitResultID)
            {
                await db.Entry(_entity).ReloadAsync();
                _entity.Private = false;
                _entity.SubmittedByID = _workflow.Identity.ID;

                //Reset reject for resubmit.
                _entity.RejectedByID = null;
                _entity.RejectedOn = null;

                bool hasFileUploadTerm = GetAllTerms(QueryComposer.ModelTermsFactory.FileUploadID, ParseRequestJSON()).Any();

                if (hasFileUploadTerm)
                {

                    var originalStatus = _entity.Status;
                    //file distribution never requires review before submission, So go right to Distribute.
                    await SetRequestStatus(DTO.Enums.RequestStatuses.RequestPendingDistribution);

                    await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.RequestPendingDistribution);

                    await MarkTaskComplete(task);

                    return new CompletionResult
                    {
                        ResultID = SubmitToDistributionID
                    };
                }
                else
                {

                    await db.SaveChangesAsync();

                    await SetRequestStatus(DTO.Enums.RequestStatuses.DraftReview);

                    await NotifyRequestStatusChanged(DTO.Enums.RequestStatuses.Draft, DTO.Enums.RequestStatuses.DraftReview);

                    await MarkTaskComplete(task);

                    return new CompletionResult
                    {
                        ResultID = SubmitResultID
                    };
                }
            }
            else if (activityResultID.Value == DeleteResultID)
            {
                db.Requests.Remove(_entity);

                if (task != null)
                {
                    db.Actions.Remove(task);
                }

                await db.SaveChangesAsync();

                return null;
            }
            else
            {
                throw new NotSupportedException(CommonMessages.ActivityResultNotSupported);
            }
        }
    }
}
