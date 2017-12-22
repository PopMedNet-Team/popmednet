using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Data.Entity;
using Lpp.Dns.DTO.Security;
using System.Web;

namespace Lpp.Dns.Api.Requests
{
    /// <summary>
    /// Controller that services Request Observers related actions.
    /// </summary>
    public class RequestObserversController : LppApiDataController<RequestObserver, RequestObserverDTO, DataContext, PermissionDefinition>
    {
        readonly List<ObserverEventDTO> EventNames;
        public RequestObserversController()
        {
            EventNames = new List<ObserverEventDTO>();
            EventNames.Add(new ObserverEventDTO() { ID = Lpp.Dns.DTO.Events.EventIdentifiers.Request.NewRequestSubmitted.ID, Name = "New Request Submitted" });
            EventNames.Add(new ObserverEventDTO() { ID = Lpp.Dns.DTO.Events.EventIdentifiers.Request.RequestAssignmentChange.ID, Name = "Request Assigned" });
            EventNames.Add(new ObserverEventDTO() { ID = Lpp.Dns.DTO.Events.EventIdentifiers.Request.RequestCommentChange.ID, Name = "New Comment on Request" });
            EventNames.Add(new ObserverEventDTO() { ID = Lpp.Dns.DTO.Events.EventIdentifiers.Document.Change.ID, Name = "New Document Attached to Request" });
            EventNames.Add(new ObserverEventDTO() { ID = Lpp.Dns.DTO.Events.EventIdentifiers.Request.RequestStatusChanged.ID, Name = "Request Status Changed" });
            EventNames.Add(new ObserverEventDTO() { ID = Lpp.Dns.DTO.Events.EventIdentifiers.Request.RoutingStatusChanged.ID, Name = "Request DataMart Routing Status Changed" });
        }
        /// <summary>
        /// Inserts request observers.
        /// </summary>
        /// <param name="values">Enumerable RequestTypeDTOs</param>
        /// <returns>Inserted RequestTypeDTOs</returns>
        [HttpPost]
        public override async Task<IEnumerable<RequestObserverDTO>> Insert(IEnumerable<RequestObserverDTO> values)
        {
            var result = await base.Insert(values);
            if (result.Any(p => p.EventSubscriptions.Any(es => es.RequestObserverID == Guid.Empty)))
            {
                var toInsertCollection = result.Where(p => p.EventSubscriptions.Any(es => es.RequestObserverID == Guid.Empty));

                foreach (var observer in toInsertCollection)
                {
                    foreach (var observerEventSubscription in observer.EventSubscriptions.Where(p => p.RequestObserverID == Guid.Empty))
                    {
                        var obj = DataContext.RequestObserverEventSubscriptions.Create();
                        obj.EventID = observerEventSubscription.EventID;
                        obj.Frequency = observerEventSubscription.Frequency.HasValue ? observerEventSubscription.Frequency.Value : DTO.Enums.Frequencies.Immediately;
                        obj.RequestObserverID = observer.ID.Value;
                        DataContext.RequestObserverEventSubscriptions.Add(obj);
                    }
                }
                await DataContext.SaveChangesAsync();
            }

            return result;
        }

        /// <summary>
        /// Inserts or Updates if already exists request types.
        /// </summary>
        /// <param name="values">Enumerable RequestTypeDTOs</param>
        /// <returns>Inserted or updated RequestTypeDTOs</returns>
        [HttpPost]
        public override async System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestObserverDTO>> InsertOrUpdate(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestObserverDTO> values)
        {
            //This no longer works as we are setting the ID on the new observers at the portal side.
            //The update base code doesn't inser those entities in the database, thus we need to manually update/insert.
            //var result = await base.InsertOrUpdate(values);

            Guid requestID = values.First().RequestID;
            List<Guid> observerIDs = values.Select(p => p.ID.Value).ToList();

            //PMNDEV-4548 - We are only inserting new Observers via the Observer Add dialog and will not be handling deletions/removals.
            //Check if we need to delete any entities
            //if (DataContext.RequestObservers.Any(p => p.RequestID == requestID && observerIDs.Contains(p.ID) == false))
            //{
            //    var toDelete = DataContext.RequestObservers.Where(p => p.RequestID == requestID && observerIDs.Contains(p.ID) == false);
            //    DataContext.RequestObservers.RemoveRange(toDelete);
            //}

            foreach (var observer in values)
            {
                if (DataContext.RequestObservers.Any(p => p.ID == observer.ID))
                {
                    //Existing Observer
                    await base.Update(values.Where(p => p.ID == observer.ID));
                }
                else
                {
                    //New Observer
                    RequestObserver newObserver = DataContext.RequestObservers.Create();
                    newObserver.ID = observer.ID.Value;
                    newObserver.DisplayName = observer.DisplayName;
                    newObserver.Email = observer.Email;
                    newObserver.RequestID = observer.RequestID;
                    newObserver.SecurityGroupID = observer.SecurityGroupID;
                    newObserver.UserID = observer.UserID;
                    DataContext.RequestObservers.Add(newObserver);
                }
            }
            await DataContext.SaveChangesAsync();

            if (values.Any(p => p.EventSubscriptions.Any(es => es.RequestObserverID == Guid.Empty)))
            {
                var toInsertCollection = values.Where(p => p.EventSubscriptions.Any(es => es.RequestObserverID == Guid.Empty));
                foreach (var observer in toInsertCollection)
                {
                    foreach (var observerEventSubscription in observer.EventSubscriptions.Where(p => p.RequestObserverID == Guid.Empty))
                    {
                        var obj = DataContext.RequestObserverEventSubscriptions.Create();
                        obj.EventID = observerEventSubscription.EventID;
                        obj.Frequency = observerEventSubscription.Frequency.HasValue ? observerEventSubscription.Frequency.Value : DTO.Enums.Frequencies.Immediately;
                        obj.RequestObserverID = observer.ID.Value;
                        DataContext.RequestObserverEventSubscriptions.Add(obj);
                    }
                }

                await DataContext.SaveChangesAsync();
            }
            return values;
        }

        /// <summary>
        /// Updates request types.
        /// </summary>
        /// <param name="values">Enumerable RequestTypeDTOs</param>
        /// <returns>Updated RequestTypeDTOs</returns>
        [HttpPost]
        public override async Task<IEnumerable<RequestObserverDTO>> Update(IEnumerable<RequestObserverDTO> values)
        {
            var result = await base.Update(values);
            return result;
        }

        /// <summary>
        /// Get request observers including default observers
        /// </summary>
        /// <param name="RequestID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<RequestObserverDTO>> ListRequestObservers(Guid? RequestID)
        {
            List<Guid> eventIDs = new List<Guid>();
            eventIDs.Add(Lpp.Dns.DTO.Events.EventIdentifiers.Request.NewRequestSubmitted.ID);
            eventIDs.Add(Lpp.Dns.DTO.Events.EventIdentifiers.Request.RequestAssignmentChange.ID);
            eventIDs.Add(Lpp.Dns.DTO.Events.EventIdentifiers.Request.RequestCommentChange.ID);
            eventIDs.Add(Lpp.Dns.DTO.Events.EventIdentifiers.Document.Change.ID);
            eventIDs.Add(Lpp.Dns.DTO.Events.EventIdentifiers.Request.RequestStatusChanged.ID);
            eventIDs.Add(Lpp.Dns.DTO.Events.EventIdentifiers.Request.RoutingStatusChanged.ID);

            var request = await DataContext.Requests.FindAsync(RequestID);

            var defaultProjectSecurityGroups = await (from pe in DataContext.ProjectEvents
                                                      where
                                                         eventIDs.Contains(pe.EventID)
                                                         && pe.ProjectID == request.ProjectID
                                                         && pe.Allowed
                                                      group pe by pe.SecurityGroupID into grp
                                                      select new RequestObserverDTO()
                                                      {
                                                          RequestID = RequestID.Value,
                                                          UserID = null,
                                                          SecurityGroupID = grp.FirstOrDefault().SecurityGroupID,
                                                          DisplayName = grp.FirstOrDefault().SecurityGroup.Path,
                                                          Email = "",
                                                          ID = null,
                                                          EventSubscriptions = (from evnt in grp
                                                                                select new RequestObserverEventSubscriptionDTO()
                                                                                {
                                                                                    LastRunTime = null,
                                                                                    NextDueTime = null,
                                                                                    Frequency = Dns.DTO.Enums.Frequencies.Immediately,
                                                                                    RequestObserverID = Guid.Empty,
                                                                                    EventID = evnt.EventID,
                                                                                })
                                                      }).ToListAsync();

            List<Guid> skipSecurityGroupIDs = new List<Guid>();
            skipSecurityGroupIDs.AddRange(defaultProjectSecurityGroups.Select(p => p.SecurityGroupID.Value));

            var defaultSecurityGroups = await (from ge in DataContext.GlobalEvents
                                               where
                                                  eventIDs.Contains(ge.EventID)
                                                  && skipSecurityGroupIDs.Contains(ge.SecurityGroupID) == false
                                                  && ge.Allowed
                                               group ge by ge.SecurityGroupID into grp
                                               select new RequestObserverDTO()
                                               {
                                                   RequestID = RequestID.Value,
                                                   UserID = null,
                                                   SecurityGroupID = grp.FirstOrDefault().SecurityGroupID,
                                                   DisplayName = grp.FirstOrDefault().SecurityGroup.Path,
                                                   Email = "",
                                                   ID = null,
                                                   EventSubscriptions = (from evnt in grp
                                                                         select new RequestObserverEventSubscriptionDTO()
                                                                         {
                                                                             LastRunTime = null,
                                                                             NextDueTime = null,
                                                                             Frequency = Dns.DTO.Enums.Frequencies.Immediately,
                                                                             RequestObserverID = Guid.Empty,
                                                                             EventID = evnt.EventID,
                                                                         })
                                               }).ToListAsync();

            var requestObservers = await DataContext.RequestObservers.Where(r => r.RequestID == RequestID).Select(r => new RequestObserverDTO()
            {
                RequestID = r.RequestID,
                UserID = r.UserID,
                SecurityGroupID = r.SecurityGroupID,
                DisplayName = r.DisplayName,
                Email = r.Email,
                ID = r.ID,
                EventSubscriptions = (from es in r.EventSubscriptions
                                      select new RequestObserverEventSubscriptionDTO()
                                      {
                                          LastRunTime = es.LastRunTime,
                                          NextDueTime = es.NextDueTime,
                                          Frequency = es.Frequency,
                                          RequestObserverID = es.RequestObserverID,
                                          EventID = es.EventID,
                                      })
            }).ToListAsync();

            return requestObservers.Concat(defaultProjectSecurityGroups).Concat(defaultSecurityGroups).OrderBy(p => p.DisplayName);
        }

        /// <summary>
        /// Retrieves all events that an observer can be registered to recieve notifications for
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ObserverEventDTO>> LookupObserverEvents()
        {
            return EventNames.OrderBy(n => n.Name);
        }

        /// <summary>
        /// Retrieve observers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ObserverDTO>> LookupObservers()
        {
            System.Collections.Specialized.NameValueCollection nvc = HttpUtility.ParseQueryString(base.Request.RequestUri.Query);
            var filter = nvc["criteria"];

            var observers = (from u in DataContext.Users
                             where (
                                     u.UserName.StartsWith(filter) ||
                                     u.FirstName.StartsWith(filter) || u.LastName.StartsWith(filter) ||
                                     u.Email.StartsWith(filter)
                                   )
                             &&
                             u.Deleted == false
                             select new ObserverDTO { ID = u.ID, DisplayName = u.UserName, DisplayNameWithType = u.UserName, ObserverType = DTO.Enums.ObserverTypes.User });

            var observerGroups = (from g in DataContext.SecurityGroups
                                  where g.Path.Contains(filter)
                                  &&
                                  (DataContext.Projects.Any(a => a.ID == g.OwnerID && a.Deleted == false) || DataContext.Organizations.Any(a => a.ID == g.OwnerID && a.Deleted == false))
                                  select new ObserverDTO { ID = g.ID, DisplayName = g.Path, DisplayNameWithType = g.Path, ObserverType = DTO.Enums.ObserverTypes.SecurityGroup });


            return await observers.Concat(observerGroups).OrderBy(p => p.DisplayName).ToListAsync();
        }

        /// <summary>
        /// Confirms that all the users and security groups being added as request observers have no explict denies for the events being subscribed.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ValidateInsertOrUpdate(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestObserverDTO> values)
        {

            Guid? requestID = values.Select(v => (Guid?)v.RequestID).FirstOrDefault();
            if (requestID.HasValue == false)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "The specified request to add the observers to was not found.");
            }

            List<string> errors = new List<string>();

            if (values.Where(x => x.SecurityGroupID.HasValue).Any())
            {
                //get the distinct list of security group/event combinations
                var securityGroupEvents = (from v in values
                                           from ev in v.EventSubscriptions
                                           where v.SecurityGroupID.HasValue
                                           select new
                                           {
                                               ev.EventID,
                                               SecurityGroupID = v.SecurityGroupID.Value
                                           }).GroupBy(k => new { k.EventID, k.SecurityGroupID }).Select(k => new { k.Key.EventID, k.Key.SecurityGroupID });

                Guid[] securityGroups = securityGroupEvents.Select(t => t.SecurityGroupID).Distinct().ToArray();
                Guid[] events = securityGroupEvents.Select(t => t.EventID).Distinct().ToArray();

                var invalidSecurityGroups =
                    (from rqst in DataContext.Requests
                     from deniedAcls in (DataContext.GlobalEvents.Where(a => a.Allowed == false && securityGroups.Contains(a.SecurityGroupID) && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID })
                     .Concat(DataContext.ProjectEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && securityGroups.Contains(a.SecurityGroupID) && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                     .Concat(DataContext.OrganizationEvents.Where(a => a.Organization.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && securityGroups.Contains(a.SecurityGroupID) && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                     .Concat(DataContext.ProjectOrganizationEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Organization.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && securityGroups.Contains(a.SecurityGroupID) && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                     .Concat(DataContext.ProjectDataMartEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.DataMart.Requests.Any(r => r.RequestID == rqst.ID) && a.Allowed == false && securityGroups.Contains(a.SecurityGroupID) && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                     )
                     join sg in DataContext.SecurityGroups on deniedAcls.SecurityGroupID equals sg.ID
                     join evt in DataContext.Events on deniedAcls.EventID equals evt.ID
                     where rqst.ID == requestID.Value
                     select new
                     {
                         EventID = evt.ID,
                         EventName = evt.Name,
                         SecurityGroupName = sg.Name,
                         SecurityGroupPath = sg.Path
                     }).ToArray();


                foreach (var invalid in invalidSecurityGroups.Distinct())
                {
                    var eventDetails = EventNames.Where(n => n.ID == invalid.EventID).FirstOrDefault();
                    errors.Add(string.Format("\"{0}\" does not have permission to subscribe to \"{1}\".", invalid.SecurityGroupPath, (eventDetails == null) ? invalid.EventName : eventDetails.Name));
                }
            }

            if (values.Where(x => x.UserID.HasValue).Any())
            {
                //get the distinct list of security group/event combinations
                var userEvents = (from v in values
                                  from ev in v.EventSubscriptions
                                  where v.UserID.HasValue
                                  select new
                                  {
                                      ev.EventID,
                                      UserID = v.UserID.Value
                                  }).GroupBy(k => new { k.EventID, k.UserID }).Select(k => new { k.Key.EventID, k.Key.UserID });

                Guid[] userIDs = userEvents.Select(ue => ue.UserID).Distinct().ToArray();
                Guid[] events = userEvents.Select(ue => ue.EventID).Distinct().ToArray();

                var invalidUsers = (from rqst in DataContext.Requests
                                    from u in DataContext.Users
                                    join sgu in DataContext.SecurityGroupUsers on u.ID equals sgu.UserID
                                    from deniedAcls in (DataContext.GlobalEvents.Where(a => a.Allowed == false && a.SecurityGroupID == sgu.SecurityGroupID && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID })
                                         .Concat(DataContext.ProjectEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && a.SecurityGroupID == sgu.SecurityGroupID && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                                         .Concat(DataContext.OrganizationEvents.Where(a => a.Organization.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && a.SecurityGroupID == sgu.SecurityGroupID && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                                         .Concat(DataContext.ProjectOrganizationEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Organization.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && a.SecurityGroupID == sgu.SecurityGroupID && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                                         .Concat(DataContext.ProjectDataMartEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.DataMart.Requests.Any(r => r.RequestID == rqst.ID) && a.Allowed == false && a.SecurityGroupID == sgu.SecurityGroupID && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                                         )
                                    join evt in DataContext.Events on deniedAcls.EventID equals evt.ID
                                    join sg in DataContext.SecurityGroups on deniedAcls.SecurityGroupID equals sg.ID
                                    where userIDs.Contains(u.ID) && rqst.ID == requestID.Value
                                    select new
                                    {
                                        EventID = evt.ID,
                                        EventName = evt.Name,
                                        SecurityGroup = sg.Name,
                                        UserID = u.ID,
                                        FirstName = u.FirstName,
                                        LastName = u.LastName,
                                        Username = u.UserName
                                    }).ToArray();


                string name, securityGroupNames;
                foreach (var u in invalidUsers.Distinct().GroupBy(k => new { k.EventID, k.EventName, k.UserID, k.FirstName, k.LastName, k.Username }))
                {
                    name = (string.IsNullOrEmpty(u.Key.FirstName) == false || string.IsNullOrEmpty(u.Key.LastName) == false) ? (u.Key.FirstName + " " + u.Key.LastName).Trim() : u.Key.Username;
                    securityGroupNames = string.Join(", ", u.Select(k => k.SecurityGroup));

                    var eventDetails = EventNames.Where(n => n.ID == u.Select(x => x.EventID).FirstOrDefault()).FirstOrDefault();
                    errors.Add(string.Format("\"{0}\" does not have permission to subscribe to \"{1}\".", name, (eventDetails == null) ? u.Key.EventName : eventDetails.Name));
                }
            }

            return Request.CreateResponse<IEnumerable<string>>(errors);
        }
    }
}
