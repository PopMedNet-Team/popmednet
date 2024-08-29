using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.Data;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.DTO.Events;
using Lpp.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lpp.Dns.Api.Tests.Requests
{
    [TestClass]
    public class RequestNotificationTests
    {
        static readonly log4net.ILog Logger;

        static RequestNotificationTests()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(RequestNotificationTests));
        }
        
        [TestMethod]
        public void RequestDataMartStatusChange()
        {
            using (var db = new Data.DataContext())
            {
                var eventLogger = new Data.RequestDataMartLogConfiguration();

                var routingStatusChangeLogItem = db.LogsRoutingStatusChange.OrderByDescending(l => l.TimeStamp).First();

                var notifications = eventLogger.CreateNotifications(routingStatusChangeLogItem, db, true);

                foreach(var note in notifications)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Subject: " + note.Subject);
                    sb.AppendLine("Recipients: " + string.Join(", ", note.Recipients.Select(r => r.Email)));
                    sb.AppendLine("Content:");
                    sb.AppendLine(note.Body);

                    Logger.Debug(sb.ToString());
                }


            }

            
        }

        [TestMethod]
        public void NotificationTest()
        {
            using (var db  = new Data.DataContext())
            {
                var logs = new RequestLogConfiguration();
                var nots = logs.GenerateNotificationsFromLogs(db).GetAwaiter().GetResult();
                //var immediate = false;
                //var userID = Guid.Parse("02CAFFEA-9402-46F8-B470-A640AD4A645E");
                //var requestID = Guid.Parse("55f2a07e-68a8-4b7d-b2f7-a4e200cf5323");
                //var log = db.LogsRequestStatusChanged.Include("Request").Where(x => x.RequestID == requestID && x.UserID == userID).FirstOrDefault();

                //var recipients = (from s in db.UserEventSubscriptions
                //                  join u in db.Users on s.UserID equals u.ID
                //                  where (s.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && !s.User.Deleted && s.User.Active && s.Frequency != null &&
                //                      (
                //                            //Additional Check: if the user does not have the Request Status Changed event enabled at Organization, Project, or Project Organization level, then notification is only sent if the user is the one who submitted the request
                //                            (
                //                                (db.OrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                //                                    || db.ProjectEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                //                                    || db.ProjectOrganizationEvents.Any(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active))
                //                                )
                //                              &&
                //                                (db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                //                                    && db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                //                                    && db.ProjectOrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.RequestStatusChanged.ID && a.ProjectID == log.Request.ProjectID && a.OrganizationID == log.Request.OrganizationID && a.SecurityGroup.Users.Any(us => us.UserID == s.UserID && !us.User.Deleted && us.User.Active)).All(a => a.Allowed)
                //                                )
                //                            )
                //                           || (log.Request.SubmittedByID == s.UserID && s.FrequencyForMy == null)
                //                        )
                //                     //user is not a request user OR user is a requestUser but has not subscribed to the "My" Notification
                //                     && (!db.RequestUsers.Any(ru => ru.RequestID == log.RequestID && ru.UserID == s.UserID)
                //                            ||
                //                            db.RequestUsers.Any(ru => ru.RequestID == log.RequestID && ru.UserID == s.UserID && s.FrequencyForMy == null && s.Frequency != null)
                //                         )
                //                         && ((!immediate && s.NextDueTime <= DateTime.UtcNow && (Frequencies)s.Frequency != Frequencies.Immediately)
                //                                || (immediate && (Frequencies)s.Frequency == Frequencies.Immediately))

                //                      )
                //                  select new Recipient
                //                  {
                //                      Email = s.User.Email,
                //                      Phone = s.User.Phone,
                //                      Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                //                      UserID = s.UserID
                //                  }).ToArray();
            }
        }

    }
}
