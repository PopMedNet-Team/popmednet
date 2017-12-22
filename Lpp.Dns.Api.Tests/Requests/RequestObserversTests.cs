using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.DTO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Configuration;
using System.Data.Common;

namespace Lpp.Dns.Api.Tests.Requests
{
    /// <summary>
    /// Summary description for RequestObserversTests
    /// </summary>
    [TestClass]
    public class RequestObserversTests
    { 
        

        [TestMethod]
        public void TestMethod()
        {

            //var values = GetJson();
            using (var db = new Data.DataContext())
            {
                db.Database.Log = (s) => { Console.WriteLine(s); };

                //var tuple = (from v in values
                //             from ev in v.EventSubscriptions
                //             where v.SecurityGroupID.HasValue
                //             select new
                //             {
                //                 ev.EventID,
                //                 SecurityGroupID = v.SecurityGroupID.Value
                //             }).GroupBy(k => new { k.EventID, k.SecurityGroupID }).Select(k => new { k.Key.EventID, k.Key.SecurityGroupID });

                //a security group that will be used to have the explicit event denies
                Guid securityGroupID = new Guid("12C0443F-D8A5-43C0-988F-A62E010CD891");
                var tuple = new[] {
                    new { EventID = DTO.Events.EventIdentifiers.Request.NewRequestSubmitted.ID, SecurityGroupID = securityGroupID  },
                    new { EventID = DTO.Events.EventIdentifiers.Request.RequestAssignmentChange.ID, SecurityGroupID = securityGroupID  },
                    new { EventID = DTO.Events.EventIdentifiers.Request.RequestCommentChange.ID, SecurityGroupID = securityGroupID  },
                    new { EventID = DTO.Events.EventIdentifiers.Document.Change.ID, SecurityGroupID = securityGroupID  },
                    new { EventID = DTO.Events.EventIdentifiers.Request.RequestStatusChanged.ID, SecurityGroupID = securityGroupID  },
                    new { EventID = DTO.Events.EventIdentifiers.Request.RoutingStatusChanged.ID, SecurityGroupID = securityGroupID  }
                };

                //get the first submitted request belonging to the security groups owning project.
                var requestID = db.Requests.Where(r => db.SecurityGroups.Any(sg => sg.OwnerID == r.ProjectID && sg.ID == securityGroupID)
                                                            && (int)r.Status >= (int)DTO.Enums.RequestStatuses.Submitted).OrderByDescending(r => r.Identifier).Select(r => r.ID).First();

                Console.WriteLine(requestID);

                Guid[] securityGroups = tuple.Select(t => t.SecurityGroupID).Distinct().ToArray();
                Guid[] events = tuple.Select(t => t.EventID).Distinct().ToArray();

                var qq =
                    (from rqst in db.Requests
                     from deniedAcls in (db.GlobalEvents.Where(a => a.Allowed == false && securityGroups.Contains(a.SecurityGroupID) && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID })
                     .Concat(db.ProjectEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && securityGroups.Contains(a.SecurityGroupID) && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                     .Concat(db.OrganizationEvents.Where(a => a.Organization.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && securityGroups.Contains(a.SecurityGroupID) && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                     .Concat(db.ProjectOrganizationEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Organization.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && securityGroups.Contains(a.SecurityGroupID) && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                     .Concat(db.ProjectDataMartEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.DataMart.Requests.Any(r => r.RequestID == rqst.ID) && a.Allowed == false && securityGroups.Contains(a.SecurityGroupID) && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                     )
                     join sg in db.SecurityGroups on deniedAcls.SecurityGroupID equals sg.ID
                     join evt in db.Events on deniedAcls.EventID equals evt.ID
                     where rqst.ID == requestID
                     select new
                     {
                         EventName = evt.Name,
                         SecurityGroupName = sg.Name
                     }).ToArray();

                foreach (var x in qq.Distinct())
                {
                    //Console.WriteLine("{0}: {1} => {2}", x.SecurityGroupName, x.EventName, x.Denied ? "Denied" : "Not Denied");
                    Console.WriteLine("{0}: {1}", x.SecurityGroupName, x.EventName);
                }

                Console.WriteLine("");
                Console.WriteLine("Checking for a specific user.");
                //the user must be part of the security group that has the explict denies
                Guid userID = new Guid("9a26ebd2-a4f8-4f77-b487-a62c00b85126");

                var userEvents = new[] {
                    new { EventID = DTO.Events.EventIdentifiers.Request.NewRequestSubmitted.ID, UserID = userID  },
                    new { EventID = DTO.Events.EventIdentifiers.Request.RequestAssignmentChange.ID, UserID = userID  },
                    new { EventID = DTO.Events.EventIdentifiers.Request.RequestCommentChange.ID, UserID = userID  },
                    new { EventID = DTO.Events.EventIdentifiers.Document.Change.ID, UserID = userID  },
                    new { EventID = DTO.Events.EventIdentifiers.Request.RequestStatusChanged.ID, UserID = userID  },
                    new { EventID = DTO.Events.EventIdentifiers.Request.RoutingStatusChanged.ID, UserID = userID  }
                };

                Guid[] userIDs = userEvents.Select(ue => ue.UserID).Distinct().ToArray();
                events = userEvents.Select(ue => ue.EventID).Distinct().ToArray();

                var invalidUsers = (from rqst in db.Requests
                                   from u in db.Users
                                   join sgu in db.SecurityGroupUsers on u.ID equals sgu.UserID
                                   from deniedAcls in (db.GlobalEvents.Where(a => a.Allowed == false && a.SecurityGroupID == sgu.SecurityGroupID && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID })
                                        .Concat(db.ProjectEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && a.SecurityGroupID == sgu.SecurityGroupID && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                                        .Concat(db.OrganizationEvents.Where(a => a.Organization.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && a.SecurityGroupID == sgu.SecurityGroupID && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                                        .Concat(db.ProjectOrganizationEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.Organization.Requests.Any(r => r.ID == rqst.ID) && a.Allowed == false && a.SecurityGroupID == sgu.SecurityGroupID && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                                        .Concat(db.ProjectDataMartEvents.Where(a => a.Project.Requests.Any(r => r.ID == rqst.ID) && a.DataMart.Requests.Any(r => r.RequestID == rqst.ID) && a.Allowed == false && a.SecurityGroupID == sgu.SecurityGroupID && events.Contains(a.EventID)).Select(a => new { a.SecurityGroupID, a.EventID }))
                                        )
                                    join evt in db.Events on deniedAcls.EventID equals evt.ID
                                    join sg in db.SecurityGroups on deniedAcls.SecurityGroupID equals sg.ID
                                   where userIDs.Contains(u.ID) && rqst.ID == requestID
                                   select new {
                                       EventID = evt.ID,
                                       EventName = evt.Name,
                                       SecurityGroup = sg.Name,
                                       UserID = u.ID,
                                       FirstName = u.FirstName,
                                       LastName = u.LastName,
                                       Username = u.UserName
                                   }).ToArray();

                foreach (var u in invalidUsers.Distinct().GroupBy(k => new { k.EventID, k.EventName, k.UserID, k.FirstName, k.LastName, k.Username }))
                {
                    string name = (string.IsNullOrEmpty(u.Key.FirstName) == false || string.IsNullOrEmpty(u.Key.LastName) == false) ? (u.Key.FirstName + " " + u.Key.LastName).Trim() : u.Key.Username;
                    string securityGroupNames = string.Join(", ", u.Select(k => k.SecurityGroup));
                    Console.WriteLine("{0}: {1} ({2})", name, u.Key.EventName, securityGroupNames);
                }

            }
        }


        public IEnumerable<RequestObserverDTO> GetJson()
        {
            IList<RequestObserverDTO> requestObserver = new List<RequestObserverDTO>();
            //This will return Invalid Events for Security Groups
            var json = System.IO.File.ReadAllText("../Requests/Samples/BadRequestObserver.json");
            //This will not return Invalid Events for Security Groups
            //var json = System.IO.File.ReadAllText("../Requests/Samples/GoodRequestObserver.json");
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
            jsonSettings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            requestObserver.Add(JsonConvert.DeserializeObject<RequestObserverDTO>(json, jsonSettings));
            return requestObserver;
        }
    }
}
