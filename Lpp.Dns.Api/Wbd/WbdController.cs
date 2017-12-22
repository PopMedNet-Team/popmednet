using Lpp.Dns.Data.Documents;
using Lpp.Dns.DTO;
using Lpp.Dns.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using Lpp.Dns.WebServices;
using Lpp.Objects;
using Lpp.Utilities;
using Lpp.Utilities.WebSites.Controllers;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Api.Wbd
{
    /// <summary>
    /// This controller is used for communication between the web based data mart and the dns portal. 
    /// It should not be used for other operations
    /// </summary>
    public class WbdController : LppApiController<DataContext>
    {
        /// <summary>
        /// returns response message for Approved request
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<HttpResponseMessage> ApproveRequest(Guid requestID)
        {
            var existing = await (from q in DataContext.Requests.Include(x => x.DataMarts.Select(dm => dm.DataMart)).Include(x => x.Organization).Include(x => x.CreatedBy) where q.ID == requestID select q).SingleOrDefaultAsync();

            if (existing.SubmittedOn.HasValue)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cannot approve a request that has not been submitted");

            if (existing.Template)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cannot approve a request template");

            if (!await DataContext.HasPermissions<Request>(Identity, existing.ID, PermissionIdentifiers.Request.ApproveRejectSubmission))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Access Denied");

            existing.DataMarts.Where(dm => dm.Status == RoutingStatus.AwaitingRequestApproval).ToList().ForEach(dm => dm.Status = RoutingStatus.Submitted);

            await DataContext.SaveChangesAsync();

            //Log events
            await AddEvents(existing, EventReasons.Accept, RoutingStatus.Submitted);

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
        /// <summary>
        /// Reject particular request
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<HttpResponseMessage> RejectRequest(Guid requestID)
        {
            var existing = await (from q in DataContext.Requests.Include(x => x.DataMarts.Select(dm => dm.DataMart)).Include(x => x.Organization).Include(x => x.CreatedBy) where q.ID == requestID select q).SingleOrDefaultAsync();

            if (existing.SubmittedOn.HasValue)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cannot rejected a request that has not been submitted");

            if (existing.Template)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cannot reject a request template");

            if (!await DataContext.HasPermissions<Request>(Identity, existing.ID, PermissionIdentifiers.Request.ApproveRejectSubmission))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Access Denied");

            existing.DataMarts.Where(dm => dm.Status == RoutingStatus.AwaitingRequestApproval).ToList().ForEach(dm => dm.Status = RoutingStatus.RequestRejected);

            await DataContext.SaveChangesAsync();

            //Log events
            await AddEvents(existing, EventReasons.Decline, RoutingStatus.RequestRejected);

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Returns a request by id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RequestDTO> GetRequestByID(Guid Id)
        {
            var request = await (from r in DataContext.Requests
                                 where r.ID == Id
                                 select r).Map<Request, RequestDTO>().SingleOrDefaultAsync();

            return request;
        }

        /// <summary>
        /// Saves the request changes from wbd
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> SaveRequest(RequestDTO request)
        {
            //Save only the stuff the user could have changed

            var existing = await (from q in DataContext.Requests.Include(x => x.DataMarts.Select(dm => dm.DataMart)).Include(x => x.Organization).Include(x => x.CreatedBy) where q.ID == request.ID select q).SingleOrDefaultAsync();

            if (existing == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Could not find the request.");
            bool statusChanged = existing.SubmittedOn != request.SubmittedOn;

            var project = await DataContext.Projects.FindAsync(request.ProjectID);

            if (project == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Could not find the project associated with the request");

            if (request.SubmittedOn.HasValue)
            {
                if (project.StartDate > DateTime.Now)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not submit requests for project " + project.Name + " because it has not yet started.");

                if (project.EndDate != null && project.EndDate.Value < DateTime.Now)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not submit requests for project " + project.Name + " because it has already finished.");

                if (existing.SubmittedOn.HasValue && request.SubmittedOn.Value != existing.SubmittedOn.Value)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cannot submit a request that has already been submitted");
                if (existing.Template)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cannot submit a request template");

                if (existing.Scheduled)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cannot submit a scheduled request");
            }            

            existing.Description = request.Description;
            existing.Name = request.Name;
            existing.SubmittedOn = request.SubmittedOn == null ? (DateTime?) null : request.SubmittedOn.Value.DateTime;
            existing.UpdatedOn = DateTime.Now;
            existing.PurposeOfUse = request.PurposeOfUse;
            existing.PhiDisclosureLevel = request.PhiDisclosureLevel;
            existing.IRBApprovalNo = request.IRBApprovalNo;
            existing.ActivityID = request.ActivityID;
            existing.Priority = request.Priority;
            existing.ProjectID = request.ProjectID;
            existing.ScheduleCount = request.ScheduleCount;
            existing.Scheduled = !request.Schedule.IsEmpty() && request.SubmittedOn.HasValue;
            existing.Schedule = request.Schedule;

            //Delete any that aren't valid anymore

            //foreach (var dataMart in existing.DataMarts.Where(dm => !request.DataMarts.Any(d => d.ID == dm.DataMart.ID)))
            //    existing.DataMarts.Remove(dataMart);

            //Set the status based on if it's awaiting approval or submitted, or draft
            var status = request.SubmittedOn.HasValue ? (await DataContext.HasPermissions<Request>(Identity, existing, PermissionIdentifiers.Request.SkipSubmissionApproval) ? RoutingStatus.Submitted : RoutingStatus.AwaitingRequestApproval) : RoutingStatus.Draft;

            //UPdate the ones that are here if the status changed
            if (statusChanged)
            {
                foreach (var dataMart in existing.DataMarts)
                {
                    dataMart.Status = status;
                }
            }
            
            //Add new ones that were added.
            //foreach (var dataMartID in request.DataMarts.Where(dm => !existing.DataMarts.Any(edm => edm.DataMart.ID == dm)))
            //{
            //    existing.DataMarts.Add(new RequestDataMart
            //    {
            //        DataMartID = await (from dm in DataContext.DataMarts where dm.ID == dataMartID select dm.ID).SingleAsync(),
            //        RequestID = existing.ID,
            //        Status = existing.SubmittedOn.HasValue ? RoutingStatus.Submitted : RoutingStatus.Draft,                  
            //    });
            //}

            //Load the routings and update them as necessary here
            //If submitted then get if the user can skip approval
            var responses = await (from r in DataContext.Responses.Include(x => x.RequestDataMart.DataMart) where r.RequestDataMart.RequestID == existing.ID && !r.RequestDataMart.Responses.Any(rr => rr.Count > r.Count) select r).ToListAsync();
            
            //Remove the ones that don't match up
            //DataContext.Responses.RemoveRange(responses.Where(r => !request.DataMarts.Any(dm => dm.ID == r.RequestDataMart.DataMartID)));

            //Add the ones that aren't there
            //Update the ones that exist
            //foreach (var dmId in request.DataMarts)
            //{
            //    var route = responses.FirstOrDefault(r => r.RequestDataMart.DataMartID == dmId);
            //    var requestDataMartId = await (from dm in DataContext.RequestDataMarts where dm.DataMartID == dmId && dm.RequestID == request.ID select dm.ID).FirstOrDefaultAsync();
            //    if (route == null)
            //    {
            //        route = new Response
            //        {
            //            RequestDataMartID = requestDataMartId,
            //            SubmittedOn = DateTime.UtcNow
            //        };
            //        DataContext.Responses.Add(route);
            //    }

            //    route.SubmittedByID = Identity.ID;                
            //}

            await DataContext.SaveChangesAsync();

            if (status == RoutingStatus.Submitted || status == RoutingStatus.AwaitingRequestApproval)
                await AddEvents(existing, EventReasons.Submission, status);

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        private enum EventReasons {Submission, Accept, Decline}
        private async Task AddEvents(Request query, EventReasons reason, RoutingStatus status) {
            string requestName;
            if (query.RequestTypeID == new Guid("A3044773-8387-4C1B-8139-92B281D0467C"))
            {
                requestName = "Query Composer";
            } //Add others here
            else
            {
                throw new NotImplementedException("Put in new request type maps.");
            }


            switch(reason) {
                case EventReasons.Submission:
                case EventReasons.Accept:
                    foreach (var dm in query.DataMarts)
                    {

                        //log event for request status changed
                        AuditEvent requestStatusChangedEvent = new AuditEvent
                        {
                            KindID = new Guid("0A850001-FC8A-4DE2-9AA5-A22200E82398"),
                            Time = DateTime.Now,
                            TargetID1 = query.ProjectID,//project Sid
                            TargetID2 = query.Organization.ID,// organization Sid
                            TargetID3 = query.CreatedByID// user Sid
                        };

                        DataContext.AuditEvents.Add(requestStatusChangedEvent);
                        await DataContext.SaveChangesAsync();

                        //Add additional properties here
                                                DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("B110851D-949A-4C5B-B4A0-732D673A967E"),//CommonProperties.ActingUser
                            GuidValue = Identity.ID, //the id of the user acting on the response
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("2EE4F7CE-8BA4-4040-8597-DE0A0EA0900F"),//CommonProperties.Project
                            GuidValue = query.ProjectID //project SID
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("4B6530BA-8205-4FB2-833C-A6989FC0E7BA"),//CommonProperties.Request
                            GuidValue = query.ID //request ID
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("3F11F85F-80B6-4544-9E4E-D512B52B10A4"),//CommonProperties.RequestType
                            GuidValue = query.RequestTypeID //request type ID (not defined in dns db, easier to get from wbd)
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("D297A905-0578-4BDB-913B-D583AE63394F"),//CommonProperties.RequestType
                            StringValue = requestName //request type display name
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("7BE6FDE5-3F2A-47C6-AC4C-CF171BD951FB"),//Lpp.Dns.Portal.Events.RequestStatusChangeBase.OldStatus AudProp value
                            StringValue = status.ToString()
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("658E6DFD-957A-4745-BA65-48D862951BA6"),//Lpp.Dns.Portal.Events.RequestStatusChangeBase.NewStatus AudProp value
                            StringValue = dm.Status.ToString()
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("02595F77-AC76-433E-8892-A178F9E4F5B3"),//CommonProperties.DataMart
                            StringValue = dm.DataMartID.ToString()
                        });


                        //Routing change to whatever
                        AuditEvent routingStatusChangedEvent = new AuditEvent
                        {
                            KindID = new Guid("5AB90001-8072-42CD-940F-A22200CC24A2"),
                            Time = DateTime.Now,
                            TargetID1 = query.ProjectID,//project Sid
                            TargetID2 = query.Organization.ID,// organization Sid
                            TargetID3 = dm.DataMartID
                        };
                        DataContext.AuditEvents.Add(routingStatusChangedEvent);


                        await DataContext.SaveChangesAsync();

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("B110851D-949A-4C5B-B4A0-732D673A967E"),//CommonProperties.ActingUser
                            GuidValue = Identity.ID, //the id of the user acting on the response
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("2EE4F7CE-8BA4-4040-8597-DE0A0EA0900F"),//CommonProperties.Project
                            GuidValue = query.ProjectID //project SID
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("4B6530BA-8205-4FB2-833C-A6989FC0E7BA"),//CommonProperties.Request
                            GuidValue = query.ID //request ID
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("3F11F85F-80B6-4544-9E4E-D512B52B10A4"),//CommonProperties.RequestType
                            GuidValue = query.RequestTypeID //request type ID (not defined in dns db, easier to get from wbd)
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("D297A905-0578-4BDB-913B-D583AE63394F"),//CommonProperties.RequestType
                            StringValue = requestName //request type display name
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("7BE6FDE5-3F2A-47C6-AC4C-CF171BD951FB"),//Lpp.Dns.Portal.Events.RequestStatusChangeBase.OldStatus AudProp value
                            StringValue = status.ToString()
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("658E6DFD-957A-4745-BA65-48D862951BA6"),//Lpp.Dns.Portal.Events.RequestStatusChangeBase.NewStatus AudProp value
                            StringValue = dm.Status.ToString()
                        });

                        DataContext.AuditEventProperties.Add(
                        new AuditPropertyValue
                        {
                            AuditEventID = requestStatusChangedEvent.ID,
                            PropertyID = new Guid("02595F77-AC76-433E-8892-A178F9E4F5B3"), //CommonProperties.DataMart
                            StringValue = dm.DataMartID.ToString()
                        });
                    }
                    break;
            }
        }

        /// <summary>
        /// Save the request document
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="fileName"></param>
        /// <param name="viewable"></param>
        /// <returns></returns>
        [ClientEntityIgnore]
        [HttpPut]
        public async Task<HttpResponseMessage> SaveRequestDocument(Guid requestID, [FromUri] string fileName, bool viewable) {

            var data = await Request.Content.ReadAsByteArrayAsync();


            var document = await (from d in DataContext.Documents where d.ItemID == requestID && d.Viewable == viewable && d.FileName == fileName select d).SingleOrDefaultAsync();

            if (document == null)
            {
                document = new Document
                {
                    Viewable = viewable,
                    Name = fileName,
                    FileName = fileName,
                    ItemID = await (from q in DataContext.Requests where q.ID == requestID select q.ID).SingleAsync(),
                    MimeType = Request.Content.Headers.ContentType.MediaType
                };

                DataContext.Documents.Add(document);

                await DataContext.SaveChangesAsync();

                document.SetData(DataContext, data);
            }

            document.SetData(DataContext, data);
            

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Returns the activities available for a given project with level structure.
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ActivityDTO>> GetActivityTreeByProjectID(Guid projectID)
        {
            try
            {
                var results = await (from a in DataContext.Activities
                               where a.ProjectID == projectID
                               orderby a.DisplayOrder, a.Name
                               select a).ToArrayAsync();

                return results.Where(a => a.TaskLevel == 1).Select(a => new ActivityDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Activities = results.Where(a2 => a2.ParentActivityID == a.ID).Select(a2 => new ActivityDTO
                    {
                        ID = a2.ID,
                        Name = a2.Name,
                        Activities = results.Where(a3 => a3.ParentActivityID == a2.ID).Select(a3 => new ActivityDTO
                        {
                            ID = a3.ID,
                            Name = a3.Name,
                            Activities = new ActivityDTO[] { }
                        })
                    })
                }).AsEnumerable();
            }
            catch (Exception ex) 
            {
                System.Diagnostics.Debug.Write(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Registers a data mart on the network.
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<DataMartRegistrationResultDTO> Register(RegisterDataMartDTO registration)
        {
            //Decode the token with the password
            //Decrypt the token using AES and the password
            var decryptedToken = Lpp.Utilities.Crypto.DecryptStringAES(registration.Token, registration.Password, "34jl23kj423dfads098098234");

            //Decode the token
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<NetworkTokenData>(decryptedToken);

            if (data.ExpiresOn < DateTime.UtcNow)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "The token has expired."));

            //Look up the provider organization
            var provider = await (from p in DataContext.Organizations where p.ID == data.ProviderOrganizationID select p).SingleOrDefaultAsync();
            if (provider == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Could not find the specicfied provider."));

            //Store the x509 public key
            provider.X509PublicKey = data.X509PublicKey;

            //Save changes.
            await DataContext.SaveChangesAsync();

            try
            {
                //Respond with the data marts and anything else that needs to go out.
                var results = new DataMartRegistrationResultDTO
                {
                    DataMarts = await (from d in DataContext.DataMarts where d.OrganizationID == provider.ID select d).Map<DataMart, DataMartDTO>().ToListAsync(),
                    DataMartModels = (from m in DataContext.DataMartModels
                                      where m.DataMart.OrganizationID == provider.ID
                                      select new DataMartInstalledModelDTO
                                      {
                                          DataMartID = m.DataMartID,
                                          ModelID = m.ModelID,
                                          Properties = m.Properties,
                                      }).ToList(),
                    Users = (from u in DataContext.Users
                             where u.OrganizationID == provider.ID
                             select new UserWithSecurityDetailsDTO
                             {
                                 Active = u.Active,
                                 ActivatedOn = u.ActivatedOn,
                                 Deleted = u.Deleted,
                                 Email = u.Email,
                                 Fax = u.Fax,
                                 FirstName = u.FirstName,
                                 ID = u.ID,
                                 LastName = u.LastName,
                                 MiddleName = u.MiddleName,
                                 OrganizationID = u.OrganizationID,
                                 PasswordHash = u.PasswordHash,
                                 Phone = u.Phone,
                                 RoleID = u.RoleID,
                                 SignedUpOn = u.SignedUpOn,
                                 Title = u.Title,
                                 UserName = u.UserName
                             }).ToList(),
                    ResearchOrganization = (from o in DataContext.Organizations where o.Primary select o).Map<Organization, OrganizationDTO>().FirstOrDefault(),
                    ProviderOrganization = (from o in DataContext.Organizations where o.ID == provider.ID select o).Map<Organization, OrganizationDTO>().FirstOrDefault()
                };

                return results;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all changes since the last time that the database was checked.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WbdChangeSetDTO> GetChanges(GetChangeRequestDTO criteria)
        {
            var lastChecked = criteria.LastChecked.ToLocalTime();
            var providers = criteria.ProviderIDs.ToArray();

            var results = new WbdChangeSetDTO
            {
                Organizations = await (from o in DataContext.Organizations where /*providers.Contains(o.SID) &&*/ (o.Requests.Any(q => q.UpdatedOn >= lastChecked) || o.DataMarts.Any(dm => dm.Requests.Any(q => q.Request.UpdatedOn >= lastChecked || DataContext.Documents.Any(f => f.CreatedOn >= lastChecked && f.ItemID == q.Request.ID)))) select o).Map<Organization, OrganizationDTO>().ToArrayAsync(),
                Users = await (from u in DataContext.Users
                         where /*providers.Contains(u.Organization.SID) &&*/ (u.Organization.Requests.Any(q => q.UpdatedOn >= lastChecked) || u.SubmittedResponses.Any(r => r.RequestDataMart.Request.UpdatedOn >= lastChecked) || u.RespondedResponses.Any(r => r.RequestDataMart.Request.UpdatedOn >= lastChecked))
                         select new UserWithSecurityDetailsDTO
                         {
                             Active = u.Active,
                             ActivatedOn = u.ActivatedOn,
                             Deleted = u.Deleted,
                             Email = u.Email,
                             Fax = u.Fax,
                             FirstName = u.FirstName,
                             ID = u.ID,
                             LastName = u.LastName,
                             MiddleName = u.MiddleName,
                             OrganizationID = u.OrganizationID,
                             PasswordHash = u.PasswordHash,
                             Phone = u.Phone,
                             RoleID = u.RoleID,
                             SignedUpOn = u.SignedUpOn,
                             Title = u.Title,
                             UserName = u.UserName
                         }).ToArrayAsync(),
                Responses = await (from r in DataContext.Responses
                             where (r.RequestDataMart.Request.UpdatedOn >= lastChecked
                             //&& !DataContext.QueryDataMarts.All(d => d.QueryID == r.QueryID && (d.QueryStatusTypeID == RoutingStatus.Completed || d.QueryStatusTypeID == RoutingStatus.ExaminedByInvestigator))
                             && DataContext.RequestDataMarts.Any(d => d.RequestID == r.RequestDataMart.RequestID && (int)d.Status >= (int)RoutingStatus.Submitted))
                             && providers.Contains(r.RequestDataMart.DataMart.Organization.ID)
                             select new ResponseDetailDTO
                             {
                                 Count = r.Count,
                                 DataMart = r.RequestDataMart.DataMart.Name,
                                 DataMartID = r.RequestDataMart.DataMartID,
                                 ID = r.ID,
                                 Request = r.RequestDataMart.Request.Name,
                                 RequestID = r.RequestDataMart.RequestID,
                                 RespondedBy = r.RespondedBy.FirstName + " " + r.RespondedBy.LastName,
                                 RespondedByID = r.RespondedByID,
                                 ResponseGroupID = r.ResponseGroupID,
                                 ResponseMessage = r.ResponseMessage,
                                 ResponseTime = r.ResponseTime,
                                 SubmitMessage = r.SubmitMessage,
                                 SubmittedBy = r.SubmittedBy.FirstName + " " + r.SubmittedBy.LastName,
                                 SubmittedByID = r.SubmittedByID,
                                 SubmittedOn = r.SubmittedOn,
                                 Status = r.RequestDataMart.Status
                             }).ToArrayAsync(),
                Requests = await (from q in DataContext.Requests
                           where q.DataMarts.Any(dm => providers.Contains(dm.DataMart.Organization.ID)) && !q.Deleted &&
                               //exclude where all responses are completed
                           (
                               //include where any response contains status is greater than or equal to Submitted
                           DataContext.RequestDataMarts.Any(d => d.RequestID == q.ID && (int)d.Status >= (int)RoutingStatus.Submitted && d.Status != RoutingStatus.AwaitingRequestApproval) &&
                           (q.UpdatedOn >= lastChecked || DataContext.Documents.Any(f => f.CreatedOn >= lastChecked && f.ItemID == q.ID)))
                           select new RequestDTO
                           {
                               ActivityDescription = q.ActivityDescription,
                               DueDate = q.DueDate,
                               Deleted = q.Deleted,
                               Description = q.Description,
                               ID = q.ID,
                               IRBApprovalNo = q.IRBApprovalNo,
                               Name = q.Name,
                               OrganizationID = q.OrganizationID,
                               PhiDisclosureLevel = q.PhiDisclosureLevel,
                               Priority = q.Priority,
                               ProjectID = q.ProjectID,
                               PurposeOfUse = q.PurposeOfUse,
                               RequestTypeID = q.RequestTypeID,
                               Schedule = q.Schedule,
                               ScheduleCount = q.ScheduleCount,
                               Scheduled = q.Scheduled,
                               SubmittedOn = q.SubmittedOn,
                               Template = q.Template,
                               UpdatedOn = q.UpdatedOn
                           }).ToArrayAsync(),

                Projects = await (from p in DataContext.Projects
                            where p.Requests.Any(q => q.DataMarts.Any(dm => providers.Contains(dm.DataMart.Organization.ID)) && (q.UpdatedOn >= lastChecked || DataContext.Documents.Any(f => f.CreatedOn >= lastChecked && f.ItemID == q.ID)))
                            select new ProjectDTO
                            {
                                Acronym = p.Acronym,
                                Active = p.Active,
                                Deleted = p.Deleted,
                                Description = p.Description,
                                EndDate = p.EndDate,
                                ID = p.ID,
                                Name = p.Name,
                                StartDate = p.StartDate
                            }).ToArrayAsync(),

                // Need to grab all the DM because there may be new DMs for orgs not owning existing DMs, which is what provider is from Monitor.cs. This may become a problem
                // if DNS becomes multi-tenant. What really needs to happen is that provider has to include all orgs in the network.
                DataMarts = await (from dm in DataContext.DataMarts where /*providers.Contains(dm.Organization.SID) &&*/ (dm.Requests.Any(q => q.Request.UpdatedOn >= lastChecked || DataContext.Documents.Any(f => f.CreatedOn >= lastChecked && f.ItemID == q.Request.ID))) select dm).Map<DataMart, DataMartDTO>().ToArrayAsync(),

                DataMartModels = await (from m in DataContext.DataMartModels
                                  where providers.Contains(m.DataMart.Organization.ID) && m.DataMart.Requests.Any(q => q.Request.UpdatedOn >= lastChecked || DataContext.Documents.Any(f => f.CreatedOn >= lastChecked && f.ItemID == q.Request.ID))
                                  select new DataMartInstalledModelDTO
                                  {
                                      DataMartID = m.DataMartID,
                                      ModelID = m.ModelID,
                                      Properties = m.Properties
                                  }).ToArrayAsync(),

                RequestDataMarts = await (from qdm in DataContext.RequestDataMarts
                                  where providers.Contains(qdm.DataMart.OrganizationID) /*providers.Contains(qdm.Query.Organization.SID)*/ && (qdm.Request.UpdatedOn >= lastChecked ||
                                   DataContext.Documents.Any(f => f.CreatedOn >= lastChecked && f.ItemID == qdm.Request.ID))
                                  select new RequestDataMartDTO
                                  {
                                      DataMartID = qdm.DataMartID,
                                      ErrorDetail = qdm.ErrorDetail,
                                      ErrorMessage = qdm.ErrorMessage,
                                      Properties = qdm.Properties,
                                      RequestID = qdm.RequestID,
                                      Status = qdm.Status,
                                      RejectReason = qdm.RejectReason,
                                      RequestTime = qdm.RequestTime,
                                      ResponseTime = qdm.ResponseTime,
                                      ResultsGrouped = qdm.ResultsGrouped
                                  }).ToArrayAsync(),

                ProjectDataMarts = await (from pdm in DataContext.ProjectDataMarts
                                    where pdm.Project.Requests.Any(q => providers.Contains(q.Organization.ID) && q.UpdatedOn >= lastChecked || DataContext.Documents.Any(f => f.CreatedOn >= lastChecked && f.ItemID == q.ID))
                                    select new ProjectDataMartDTO
                                    {
                                        DataMartID = pdm.DataMartID,
                                        ProjectID = pdm.ProjectID
                                    }).ToArrayAsync(),

                Documents = await (from d in DataContext.Documents
                             where d.CreatedOn >= lastChecked && DataContext.Requests.Any(q => q.ID == d.ItemID && q.UpdatedOn >= lastChecked && providers.Contains(q.Organization.ID))
                             select new RequestDocumentDTO
                                 {
                                     ID = d.ID,
                                     FileName = d.FileName,
                                     MimeType = d.MimeType,
                                     Name = d.Name,
                                     ItemID = d.ItemID,
                                     Viewable = d.Viewable
                                 }).ToArrayAsync(),
                SecurityGroups = await (from sg in DataContext.SecurityGroups
                                        join o in DataContext.Organizations on sg.OwnerID equals o.ID
                                        where (o.Requests.Any(q => q.UpdatedOn >= lastChecked) || o.DataMarts.Any(dm => dm.Requests.Any(q => q.Request.UpdatedOn >= lastChecked || DataContext.Documents.Any(f => f.CreatedOn >= lastChecked && f.ItemID == q.Request.ID))))
                                  select new SecurityGroupWithUsersDTO
                                  {
                                      ID = sg.ID,
                                      Kind = sg.Kind,
                                      Name = sg.Name,
                                      OwnerID = sg.OwnerID,
                                      Owner = o.Name,
                                      ParentSecurityGroupID = sg.ParentSecurityGroupID,
                                      Users = sg.Users.Select(u => u.UserID)
                                  }).ToArrayAsync(),
            };

            //Fix dates
            foreach (var query in results.Requests)
            {
                if (query.DueDate.HasValue)
                    query.DueDate = query.DueDate.Value.ToUniversalTime();

                if (query.SubmittedOn.HasValue)
                    query.SubmittedOn = query.SubmittedOn.Value.ToUniversalTime();

                query.UpdatedOn = query.UpdatedOn.ToUniversalTime();
            }

            //Add the security stuff
            var organizationIDs = results.Organizations.Select(o => o.ID).ToArray();
            var projectIDs = results.Projects.Select(p => p.ID).ToArray();
            var dataMartIDs = results.DataMarts.Select(dm => dm.ID).ToArray();

            var globalProjectID = new Guid("6A690001-7579-4C74-ADE1-A2210107FA29");
            var portalID = new Guid("BBBA0001-2BC2-4E12-A5B4-A22100FDBAFD");

            var listDataMartsPrivilageID = new Guid("ECD72B1B-50F5-4E3A-BED2-375880435FD1");
            var readPrivilageID = new Guid("4CCB0EC2-006D-4345-895E-5DD2C6C8C791");
            var editPrivilageID = new Guid("1B42D2D7-F7A7-4119-9CC5-22991DC12AD3");
            var deletePrivilageID = new Guid("1C019772-1B9D-48F8-9FCD-AC44BC6FD97B");

            var seeQueuePrivilageID = new Guid("5D6DD388-7842-40A1-A27A-B9782A445E20");
            var uploadResultsPrivilageID = new Guid("0AC48BA6-4680-40E5-AE7A-F3436B0852A0");
            var holdRequestPrivilageID = new Guid("894619BE-9A73-4DA9-A43A-10BCC563031C");
            var rejectRequestPrivilageID = new Guid("0CABF382-93D3-4DAC-AA80-2DE500A5F945");
            var approveResponsesPrivilageID = new Guid("A58791B5-E8AF-48D0-B9CD-ED0B54E564E6");
            var skipResponseApprovalPrivilageID = new Guid("A0F5B621-277A-417C-A862-801D7B9030A2");
            //var groupResponsesPrivilageID = new Guid("F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE");//currently not used in sync

            var adminWbdID = new Guid("F9870001-7C06-4B4B-8F76-A2A701102FF0");
            

            //To-do Reenable with new security

            //results.RequestResponseSecurityACLs = await (from t in SecurityTuple3.ReturnSecurityTupleWithChangedOn(DataContext)
            //                                      where
            //                                          t.ChangedOn > criteria.LastChecked
            //                                          && (t.PrivilegeID == seeQueuePrivilageID ||
            //                                          t.PrivilegeID == uploadResultsPrivilageID ||
            //                                          t.PrivilegeID == holdRequestPrivilageID ||
            //                                          t.PrivilegeID == rejectRequestPrivilageID ||
            //                                          t.PrivilegeID == approveResponsesPrivilageID ||
            //                                          t.PrivilegeID == skipResponseApprovalPrivilageID)

            //                                      select new SecurityTupleDTO
            //                                      {
            //                                          DeniedEntries = t.DeniedEntries,
            //                                          ExplicitAllowedEntries = t.ExplicitAllowedEntries,
            //                                          ExplicitDeniedEntries = t.ExplicitDeniedEntries,
            //                                          ID1 = t.ID1,
            //                                          ID2 = t.ID2,
            //                                          ID3 = t.ID3,
            //                                          PrivilegeID = t.PrivilegeID,
            //                                          SubjectID = t.SubjectID,
            //                                          ViaMembership = t.ViaMembership,
            //                                          ChangedOn = t.ChangedOn
            //                                      }).Distinct().ToArrayAsync();

            ////Check this one. Gotta be more to it, like the Organization
            //results.DataMartSecurityACLs = await (from t in SecurityTuple2.ReturnSecurityTupleWithChangedOn(DataContext)
            //                               where t.ChangedOn >= criteria.LastChecked
            //                               && (t.PrivilegeID == readPrivilageID
            //                               || t.PrivilegeID == editPrivilageID
            //                               || t.PrivilegeID == deletePrivilageID)
            //                               group t by new {t.DeniedEntries, t.ExplicitAllowedEntries, t.ExplicitDeniedEntries, t.ID1, t.ID2, t.ParentID1, t.ParentID2, t.PrivilegeID, t.SubjectID, t.ViaMembership} into g
            //                               select new SecurityTupleDTO
            //                               {
            //                                   DeniedEntries = g.Key.DeniedEntries,
            //                                   ExplicitAllowedEntries = g.Key.ExplicitAllowedEntries,
            //                                   ExplicitDeniedEntries = g.Key.ExplicitDeniedEntries,
            //                                   ID1 = g.Key.ID1,
            //                                   ID2 = g.Key.ID2,
            //                                   PrivilegeID = g.Key.PrivilegeID,
            //                                   SubjectID = g.Key.SubjectID,
            //                                   ViaMembership = g.Key.ViaMembership,
            //                                   ChangedOn =  g.Select(t => t.ChangedOn).Max()
            //                               }).Distinct().ToArrayAsync();

            //results.ManageWbdACLs = await (from t in SecurityTuple1.ReturnSecurityTupleWithChangedOn(DataContext)
            //                         where t.ChangedOn >= criteria.LastChecked && t.PrivilegeID == adminWbdID
            //                         select new SecurityTupleDTO
            //                         {
            //                             DeniedEntries = t.DeniedEntries,
            //                             ExplicitAllowedEntries = t.ExplicitAllowedEntries,
            //                             ExplicitDeniedEntries = t.ExplicitDeniedEntries,
            //                             ID1 = t.ID1,
            //                             PrivilegeID = t.PrivilegeID,
            //                             SubjectID = t.SubjectID,
            //                             ViaMembership = t.ViaMembership,
            //                             ChangedOn = t.ChangedOn
            //                         }).Distinct().ToArrayAsync();

            return results;
        }

        /// <summary>
        /// Downloads the specified document
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> DownloadDocument(Guid documentId)
        {
            var document = await (from f in DataContext.Documents where f.ID == documentId select f).SingleOrDefaultAsync();
            if (document == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Could not find the specified file."));



            var content = new StreamContent(new DocumentStream(DataContext, document.ID));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(document.MimeType);
            content.Headers.ContentLength = document.Length;
            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = document.FileName,
                Size = document.Length
            };


            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
        }

        /// <summary>
        /// Returns the viewable file for the request
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> DownloadRequestViewableFile(Guid requestId)
        {
            var document = await (from f in DataContext.Documents where f.ItemID == requestId && f.Viewable select f).SingleOrDefaultAsync();
            if (document == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Could not find the specified file."));


            var content = new StreamContent(new DocumentStream(DataContext, document.ID));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(document.MimeType);
            content.Headers.ContentLength = document.Length;
            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = document.FileName,
                Size = document.Length
            };


            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
        }

        /// <summary>
        /// Copies a request
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Guid> CopyRequest(Guid requestID)
        {
            var oldQuery = await (from q in DataContext.Requests.Include(x => x.DataMarts.Select(d => d.Responses)).Include(x => x.DataMarts) where q.ID == requestID select q).SingleOrDefaultAsync();
            if (oldQuery == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Could not find the request."));
            try
            {
                var newQuery = new Request();
                newQuery.Name = oldQuery.Name + " (Copy)";
                var properties = oldQuery.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    switch (prop.Name)
                    {
                        case "ID":
                        case "SID":
                        case "Name":
                        case "Activity":
                        case "Organization":
                        case "Project":
                        case "QueryType":
                        case "UpdatedOn":
                        case "SubmittedOn":
                        case "CreatedOn":
                            continue;
                    }

                    if (prop.PropertyType.FullName.Contains("ICollection"))
                        continue;

                    typeof(Request).GetProperty(prop.Name).SetValue(newQuery, prop.GetValue(oldQuery));
                }

                DataContext.Requests.Add(newQuery);

                await DataContext.SaveChangesAsync();

                //Add the data marts
                foreach (var dm in oldQuery.DataMarts)
                {
                    DataContext.RequestDataMarts.Add(new RequestDataMart
                    {
                        DataMartID = dm.DataMartID,
                        Properties = dm.Properties,
                        RequestID = newQuery.ID,
                        Status = RoutingStatus.Draft
                    });
                }

                //Do we need to add the responses? I don't think so.

                await DataContext.SaveChangesAsync();

                //copy the documents for the request
                //This should be done as a single sql query.
                var documents = await (from d in DataContext.Documents where d.ItemID == oldQuery.ID select d).ToArrayAsync();
                foreach (var doc in documents)
                {
                    var docNew = new Document
                    {
                        Name = doc.Name,
                        ItemID = newQuery.ID,
                        Viewable = doc.Viewable,
                        MimeType = doc.MimeType,
                        FileName = doc.FileName
                    };
                    //Create new document
                    DataContext.Documents.Add(docNew);

                    await DataContext.SaveChangesAsync();

                    docNew.SetData(DataContext, doc.GetData(DataContext));
                }

                await DataContext.SaveChangesAsync();

                return newQuery.ID;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the status of a response.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task UpdateResponseStatus(UpdateResponseStatusRequestDTO details)
        {
            //See: DatamartClientService.SetRequestStatus for current implementation.
            var user = await DataContext.Users.SingleAsync(u => u.ID == details.UserID);
            var routingInstance = await DataContext.Responses.SingleAsync(r => r.ID == details.ResponseID);
            var queryDatamart = await DataContext.RequestDataMarts.FirstAsync(q => q.Request.ID == details.RequestID && q.DataMart.ID == details.DataMartID);
            var queryName = await DataContext.Requests.Where(q => q.ID == details.RequestID).Select(q => q.Name).FirstAsync();
            

            routingInstance.ResponseMessage = !string.IsNullOrWhiteSpace(details.HoldReason) ? details.HoldReason : !string.IsNullOrWhiteSpace(details.RejectReason) ? details.RejectReason : details.Message;
            if(details.StatusID == RoutingStatus.AwaitingResponseApproval ||details.StatusID == RoutingStatus.Completed)
            { 
                routingInstance.RespondedByID = user.ID;
                routingInstance.ResponseTime = DateTime.Now;
            }

            var currentStatus = queryDatamart.Status;
            queryDatamart.Status = details.StatusID;

            await DataContext.SaveChangesAsync();

            if (queryDatamart.Status != currentStatus)
            {
                //log event for request status changed
                AuditEvent requestStatusChangedEvent = new AuditEvent
                {
                    KindID = new Guid("0A850001-FC8A-4DE2-9AA5-A22200E82398"),
                    Time = DateTime.Now,
                    TargetID1 = details.ProjectID,//project Sid
                    TargetID2 = details.OrganizationID,// organization Sid
                    TargetID3 = details.UserID// user Sid
                };

                DataContext.AuditEvents.Add(requestStatusChangedEvent);

                AuditEvent routingStatusChangedEvent = new AuditEvent
                {
                    KindID = new Guid("5AB90001-8072-42CD-940F-A22200CC24A2"),
                    Time = DateTime.Now,
                    TargetID1 = details.ProjectID,//project Sid
                    TargetID2 = details.OrganizationID,// organization Sid
                    TargetID3 = details.DataMartID// datamart Sid                
                };
                DataContext.AuditEvents.Add(routingStatusChangedEvent);

                await DataContext.SaveChangesAsync();

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = requestStatusChangedEvent.ID,
                    PropertyID = new Guid("B110851D-949A-4C5B-B4A0-732D673A967E"),//CommonProperties.ActingUser
                    GuidValue = user.ID, //the id of the user acting on the response
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = requestStatusChangedEvent.ID,
                    PropertyID = new Guid("2EE4F7CE-8BA4-4040-8597-DE0A0EA0900F"),//CommonProperties.Project
                    GuidValue = details.ProjectID //project SID
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = requestStatusChangedEvent.ID,
                    PropertyID = new Guid("4B6530BA-8205-4FB2-833C-A6989FC0E7BA"),//CommonProperties.Request
                    GuidValue = queryDatamart.RequestID //request ID
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = requestStatusChangedEvent.ID,
                    PropertyID = new Guid("3F11F85F-80B6-4544-9E4E-D512B52B10A4"),//CommonProperties.RequestType
                    GuidValue = details.RequestTypeID //request type ID (not defined in dns db, easier to get from wbd)
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = requestStatusChangedEvent.ID,
                    PropertyID = new Guid("D297A905-0578-4BDB-913B-D583AE63394F"),//CommonProperties.Name
                    StringValue = queryName //request display name
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = requestStatusChangedEvent.ID,
                    PropertyID = new Guid("7BE6FDE5-3F2A-47C6-AC4C-CF171BD951FB"),//Lpp.Dns.Portal.Events.RequestStatusChangeBase.OldStatus AudProp value
                    StringValue = currentStatus.ToString()
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = requestStatusChangedEvent.ID,
                    PropertyID = new Guid("658E6DFD-957A-4745-BA65-48D862951BA6"),//Lpp.Dns.Portal.Events.RequestStatusChangeBase.NewStatus AudProp value
                    StringValue = queryDatamart.Status.ToString()
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = requestStatusChangedEvent.ID,
                    PropertyID = new Guid("02595F77-AC76-433E-8892-A178F9E4F5B3"),//CommonProperties.DataMart
                    StringValue = queryDatamart.DataMartID.ToString()
                });


                //log event for routing status changed.                

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = routingStatusChangedEvent.ID,
                    PropertyID = new Guid("B110851D-949A-4C5B-B4A0-732D673A967E"),//CommonProperties.ActingUser
                    GuidValue = user.ID, //the id of the user acting on the response
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = routingStatusChangedEvent.ID,
                    PropertyID = new Guid("2EE4F7CE-8BA4-4040-8597-DE0A0EA0900F"),//CommonProperties.Project
                    GuidValue = details.ProjectID //project SID
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = routingStatusChangedEvent.ID,
                    PropertyID = new Guid("4B6530BA-8205-4FB2-833C-A6989FC0E7BA"),//CommonProperties.Request
                    GuidValue = queryDatamart.RequestID //request ID
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = routingStatusChangedEvent.ID,
                    PropertyID = new Guid("3F11F85F-80B6-4544-9E4E-D512B52B10A4"),//CommonProperties.RequestType
                    GuidValue = details.RequestTypeID //request type ID (not defined in dns db, easier to get from wbd)
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = routingStatusChangedEvent.ID,
                    PropertyID = new Guid("D297A905-0578-4BDB-913B-D583AE63394F"),//CommonProperties.Name
                    StringValue = queryName //request display name
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = routingStatusChangedEvent.ID,
                    PropertyID = new Guid("7BE6FDE5-3F2A-47C6-AC4C-CF171BD951FB"),//Lpp.Dns.Portal.Events.RequestStatusChangeBase.OldStatus AudProp value
                    StringValue = currentStatus.ToString()
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = routingStatusChangedEvent.ID,
                    PropertyID = new Guid("658E6DFD-957A-4745-BA65-48D862951BA6"),//Lpp.Dns.Portal.Events.RequestStatusChangeBase.NewStatus AudProp value
                    StringValue = queryDatamart.Status.ToString()
                });

                DataContext.AuditEventProperties.Add(
                new AuditPropertyValue
                {
                    AuditEventID = routingStatusChangedEvent.ID,
                    PropertyID = new Guid("02595F77-AC76-433E-8892-A178F9E4F5B3"),//CommonProperties.DataMart
                    StringValue = queryDatamart.DataMartID.ToString()
                });

                try
                {
                    await DataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                }

                
            }
        }

        /// <summary>
        /// Saves a response document to the database.
        /// </summary>
        /// <param name="responseID">The ID of the response</param>
        /// <param name="viewable">Is the document viewable.</param>
        /// <returns></returns>
        [HttpPut, ClientEntityIgnore]
        public async Task PostResponseDocument([FromUri] Guid responseID, [FromUri] bool viewable )
        {
            string filename = Request.Content.Headers.ContentDisposition.FileName.Trim('\"');
            string mimetype = Request.Content.Headers.ContentType.MediaType;
            byte[] buffer = await Request.Content.ReadAsByteArrayAsync();

            var dnsResponseID = await DataContext.Responses.Where(r => r.ID == responseID).Select(r => r.ID).SingleAsync();


            var doc = new Document
            {
                Name = filename,
                FileName = filename,
                Viewable = viewable,
                ItemID = dnsResponseID,
                MimeType = mimetype
            };

            //create entry in Documents table
            DataContext.Documents.Add(doc);

            try
            {
                await DataContext.SaveChangesAsync();

                doc.SetData(DataContext, buffer);
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }
        }
    }


}
