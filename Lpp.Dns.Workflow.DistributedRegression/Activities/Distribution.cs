using Lpp.Dns.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Workflow.Engine;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using System.Data.Entity;

namespace Lpp.Dns.Workflow.DistributedRegression.Activities
{
    /// <summary>
    /// The Initial Activity for the Distributed Regression Workflow.  It Saves, Copy's, Terminates, and Submits a Workflow Request.
    /// </summary>
    public class Distribution : ActivityBase<Request>
    {
        /// <summary>
        /// The Result ID passed by the User to only Save the Request and does not alter the Request Status
        /// </summary>
        private static readonly Guid SaveResultID = new Guid("DFF3000B-B076-4D07-8D83-05EDE3636F4D");
        /// <summary>
        /// The Result ID passed by the User to Copy the Request and does not alter the Request Status
        /// </summary>
        private static readonly Guid CopyResultID = new Guid("47538F13-9257-4161-BCD0-AA7DEA897AE5");
        /// <summary>
        /// The Result ID Passed by the User to Submit the Request to the DataPartners.  This Closes the current task and Sets the Request Status to Submitted
        /// </summary>
        private static readonly Guid SubmitResultID = new Guid("5445DC6E-72DC-4A6B-95B6-338F0359F89E");
        /// <summary>
        /// The Result ID Passed by the User to Terminate the Request.  This Closes the current task and Sets the Request Status to cancelled.
        /// </summary>
        private static readonly Guid TerminateResultID = new Guid("53579F36-9D20-47D9-AC33-643D9130080B");

        /// <summary>
        /// Gets the Name of the Current Activity
        /// </summary>
        public override string ActivityName
        {
            get
            {
                return "Distribution";
            }
        }

        /// <summary>
        /// The ID of the Current Activity
        /// </summary>
        public override Guid ID
        {
            get
            {
                return new Guid("94E90001-A620-4624-9003-A64F0121D0D7");
            }
        }

        /// <summary>
        /// The String that shows in the Task Subject Window
        /// </summary>
        public override string CustomTaskSubject
        {
            get
            {
                return "Distribution";
            }
        }

        /// <summary>
        /// The URL that should be passed back to the User
        /// </summary>
        public override string Uri
        {
            get
            {
                return "requests/details?ID=" + _entity.ID;
            }
        }

        /// <summary>
        /// The Method to Validate the User Permissions and to Validate the Request Information before being passed to the Complete Method
        /// </summary>
        /// <param name="activityResultID">The Result ID passed by the User to indicate which step to proceed to.</param>
        /// <returns></returns>
        public override async Task<ValidationResult> Validate(Guid? activityResultID)
        {
            if(!activityResultID.HasValue)
                activityResultID = SaveResultID;

            var permissions = await db.GetGrantedWorkflowActivityPermissionsForRequestAsync(_workflow.Identity, _entity, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask);

            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditTask) && (activityResultID.Value == SaveResultID || activityResultID.Value == SubmitResultID))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToEditTask };
            }

            if (!permissions.Contains(PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.CloseTask) && (activityResultID.Value == SaveResultID || activityResultID.Value == SubmitResultID || activityResultID.Value == TerminateResultID))
            {
                return new ValidationResult { Success = false, Errors = CommonMessages.RequirePermissionToCloseTask };
            }

            string errors;
            if (activityResultID.Value == SubmitResultID)
            {
                errors = await PerformSubmitValidation();
            }
            else
            {
                errors = string.Empty;
            }


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

        /// <summary>
        /// The Method to do what the User decieded
        /// </summary>
        /// <param name="data">The Data payload passed by the User</param>
        /// <param name="activityResultID">The Result ID Passed by the User to indicate which step to proceed to.</param>
        /// <returns></returns>
        public override async Task<CompletionResult> Complete(string data, Guid? activityResultID)
        {
            if (!activityResultID.HasValue)
                activityResultID = SaveResultID;

            var task = PmnTask.GetActiveTaskForRequestActivity(_entity.ID, ID, db);

            if (activityResultID == SaveResultID)
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
            else if(activityResultID == SubmitResultID)
            {

                await db.LoadCollection(_entity, (r) => r.DataMarts);

                if (!_entity.DataMarts.Any())
                    throw new Exception("At least one routing needs to be specified when submitting a requests.");

                IList<Guid> documentRevisionSets = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Guid>>(data);

                IEnumerable<Document> documents = await (from d in db.Documents.AsNoTracking()
                                                         join x in (
                                                             db.Documents.Where(dd => documentRevisionSets.Contains(dd.RevisionSetID.Value))
                                                             .GroupBy(k => k.RevisionSetID)
                                                             .Select(k => k.OrderByDescending(d => d.MajorVersion).ThenByDescending(d => d.MinorVersion).ThenByDescending(d => d.BuildVersion).ThenByDescending(d => d.RevisionVersion).Select(y => y.ID).Distinct().FirstOrDefault())
                                                         ) on d.ID equals x
                                                         orderby d.ItemID descending, d.RevisionSetID descending, d.CreatedOn descending
                                                         select d).ToArrayAsync();

                //reload the request since altering the routings triggers a change of the request status in the db by a trigger.
                await db.Entry(_entity).ReloadAsync();

                //update the request
                _entity.SubmittedByID = _workflow.Identity.ID;
                _entity.SubmittedOn = DateTime.UtcNow;
                _entity.AdapterPackageVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(this.GetType().Assembly.Location).FileVersion;
                _entity.RejectedByID = null;
                _entity.RejectedOn = null;
                _entity.Private = false;

                //save the changes to the request now since the trigger for routings will change the status invalidating the object before save
                await db.SaveChangesAsync();

                var originalStatus = _entity.Status;
                await SetRequestStatus(DTO.Enums.RequestStatuses.Submitted, false);

                foreach (var dm in _entity.DataMarts.Where(dm => dm.Status == 0 || dm.Status == DTO.Enums.RoutingStatus.AwaitingRequestApproval || dm.Status == DTO.Enums.RoutingStatus.Draft))
                {
                    if (dm.RoutingType != DTO.Enums.RoutingType.AnalysisCenter)
                        dm.Status = DTO.Enums.RoutingStatus.Submitted;
                    else
                        dm.Status = DTO.Enums.RoutingStatus.Draft;

                    var currentResponse = db.Responses.FirstOrDefault(r => r.RequestDataMartID == dm.ID && r.Count == r.RequestDataMart.Responses.Max(rr => rr.Count));
                    if (currentResponse == null && dm.RoutingType != DTO.Enums.RoutingType.AnalysisCenter)
                    {
                        currentResponse = db.Responses.Add(new Response { RequestDataMartID = dm.ID });
                    }
                    
                    if (dm.RoutingType != DTO.Enums.RoutingType.AnalysisCenter)
                    {
                        currentResponse.SubmittedByID = _workflow.Identity.ID;
                        currentResponse.SubmittedOn = DateTime.UtcNow;
                        //add the request document associations
                        for (int i = 0; i < documentRevisionSets.Count; i++)
                        {
                            db.RequestDocuments.Add(new RequestDocument { RevisionSetID = documentRevisionSets[i], ResponseID = currentResponse.ID, DocumentType = DTO.Enums.RequestDocumentType.Input });
                        }
                    }
                }

                await db.SaveChangesAsync();

                //reload the request since altering the routings triggers a change of the request status in the db by a trigger.
                await db.Entry(_entity).ReloadAsync();

                DTO.QueryComposer.QueryComposerRequestDTO qcRequestDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(_entity.Query);
                var modularTerm = qcRequestDTO.Where.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == DistributedRegressionConfiguration.ModularProgramTermID)).FirstOrDefault();
                var termValues = Newtonsoft.Json.JsonConvert.DeserializeObject<ModularProgramTermValues>(modularTerm.Values["Values"].ToString());

                //update the request.json term value to include system generated documents revisionsetIDs
                termValues.Documents.Clear();

                for (int i = 0; i < documentRevisionSets.Count; i++)
                {
                    termValues.Documents.Add(new ModularProgramTermValues.Document { RevisionSetID = documentRevisionSets[i] });
                }

                modularTerm.Values["Values"] = termValues;
                _entity.Query = Newtonsoft.Json.JsonConvert.SerializeObject(qcRequestDTO);

                await db.SaveChangesAsync();

                await NotifyRequestStatusChanged(originalStatus, DTO.Enums.RequestStatuses.Submitted);

                await MarkTaskComplete(task);

                return new CompletionResult
                {
                    ResultID = SubmitResultID
                };
            }
            else if(activityResultID == CopyResultID)
            {
                return new CompletionResult
                {
                    ResultID = CopyResultID
                };
            }
            else if(activityResultID == TerminateResultID)
            {

                db.Requests.Remove(_entity);

                if (task != null)
                {
                    db.Actions.Remove(task);
                }

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
