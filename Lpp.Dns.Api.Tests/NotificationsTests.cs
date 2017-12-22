using Lpp.Dns.Data;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.DTO.Events;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Api.Tests
{
    [TestClass]
    public class NotificationsTests
    {
        [TestMethod]
        public async Task GenerateNotificationsForTaskReminder()
        {
            var loggingConfiguration = new Lpp.Dns.Data.ActionLogConfiguration();
            using (var db = new DataContext())
            {
                var notifications = await loggingConfiguration.GenerateNotificationsFromLogs(db);

                foreach (var notification in notifications)
                {
                    Console.WriteLine("Subject: " + notification.Subject);
                    Console.WriteLine("Recipients: " + string.Join(", ", notification.Recipients.Select(r => r.Name).ToArray()));
                    Console.WriteLine(notification.Body);
                    Console.WriteLine();
                }
            }
        }

        [TestMethod]
        public async Task UpdateDelayedNotificationsSentTime()
        {
            DateTime runTime = DateTime.UtcNow;
            using(var db = new DataContext())
            {
                //Upon completion update all of the notifications that should have run with the new time that they should be run again.
                var subscriptions = await(from s in db.UserEventSubscriptions where (s.NextDueTime == null || s.NextDueTime.Value < runTime) && s.User.Active && s.Frequency != DTO.Enums.Frequencies.Immediately select s).ToArrayAsync();
                foreach (var sub in subscriptions)
                {
                    switch (sub.Frequency)
                    {
                        case DTO.Enums.Frequencies.Daily:
                            sub.NextDueTime = runTime.AddDays(1);
                            sub.LastRunTime = runTime;
                            break;
                        case DTO.Enums.Frequencies.Monthly:
                            sub.NextDueTime = runTime.AddMonths(1);
                            sub.LastRunTime = runTime;
                            break;
                        case DTO.Enums.Frequencies.Weekly:
                            sub.NextDueTime = runTime.AddDays(7);
                            sub.LastRunTime = runTime;
                            break;
                    }
                }

                var mySubscriptions = await(from s in db.UserEventSubscriptions where (s.NextDueTime == null || s.NextDueTime.Value < runTime) && s.User.Active && s.FrequencyForMy != DTO.Enums.Frequencies.Immediately select s).ToArrayAsync();
                foreach (var mySub in subscriptions)
                {
                    switch (mySub.FrequencyForMy)
                    {
                        case DTO.Enums.Frequencies.Daily:
                            mySub.NextDueTimeForMy = runTime.AddDays(1);
                            mySub.LastRunTime = runTime;
                            break;
                        case DTO.Enums.Frequencies.Monthly:
                            mySub.NextDueTimeForMy = runTime.AddMonths(1);
                            mySub.LastRunTime = runTime;
                            break;
                        case DTO.Enums.Frequencies.Weekly:
                            mySub.NextDueTimeForMy = runTime.AddDays(7);
                            mySub.LastRunTime = runTime;
                            break;
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        [TestMethod]
        public async Task CallExecuteScheduledNotifications()
        {
            var controller = new Lpp.Dns.Api.Controllers.NotificationsController();
            //Specify the username and password of a user that has type of 2 (DTO.Enums.UserTypes.BackgroundTask)
            var result = await controller.ExecuteScheduledNotifications("", "");
        }

        [TestMethod]
        public void GetAwaitingRequestNotifications()
        {
            using (var db = new DataContext())
            {
                //Awaiting response nag
                var results = (from r in db.Requests
                               where (r.Status == RequestStatuses.Submitted || r.Status == RequestStatuses.Resubmitted) &&
                                   (
                                   (db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && a.ProjectID == r.ProjectID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).Any() &&
                                   db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && a.ProjectID == r.ProjectID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).All(a => a.Allowed))
                                   ||
                                   (db.ProjectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && a.ProjectID == r.ProjectID && a.DataMart.Requests.Any(req => req.RequestID == r.ID) && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).Any() &&
                                   db.ProjectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && a.ProjectID == r.ProjectID && a.DataMart.Requests.Any(requests => requests.RequestID == r.ID) && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).All(a => a.Allowed)
                                   )
                                   ||
                                   (db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).Any() &&
                                   db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.UploadResults && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID select s).Any()).All(a => a.Allowed)
                                   )
                                   )
                               select r).Include("DataMarts").ToArray();

                Console.WriteLine(results.Count());
                foreach (var req in results)
                {
                    Console.WriteLine("ID:{0:D}\t'{1}'", req.ID, req.Name);
                }
            }
        }

        [TestMethod]
        public void GetAssignedUserNotifications()
        {
            using (var db = new DataContext())
            {
                Guid userID = new Guid("2CBF97E0-FF50-496A-8F77-A57DA62DAC05");
                var results = db.GetAssignedUserNotifications(userID);

                foreach (var u in results)
                {
                    Console.WriteLine("{0:D}\t{1}\t{2}", u.Event, u.Description, u.Level);
                }
            }
        }

        [TestMethod]
        public void GetRecipientsForAwaitingRequestNotification()
        {
            //Request ID should be set based on a value from GetAwaitingRequestNotifications()
            var log = new { RequestID = new Guid("D8310001-21FE-487F-BE35-A2C8011CE4C0") };
            bool immediate = false;

            using (var db = new DataContext())
            {
                var results = (from s in db.ReturnUserEventSubscriptionsByRequest(log.RequestID, EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID, immediate)
                               join u in db.Users on s.UserID equals u.ID
                               where ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || (Frequencies)s.Frequency == Frequencies.Immediately)
                               select
                                     new
                                     {
                                         Email = s.Email,
                                         Phone = s.Phone,
                                         Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                         Username = u.UserName,
                                         UserID = s.UserID
                                     }).ToArray();

                Console.WriteLine("Results for users that have pending Submitted Request Awaits Response notifications - no security:");
                foreach (var u in results)
                {
                    Console.WriteLine("{0:D}\t{1}\t{2}", u.UserID, u.Username, u.Name);
                }


                Console.WriteLine("Results for users that have pending Submitted Request Awaits Response notifications - after security:");
                var r2 = (from s in db.ReturnUserEventSubscriptionsByRequest(log.RequestID, EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID, immediate)
                          join u in db.Users on s.UserID equals u.ID
                          let projectEventAcls = db.ProjectEvents.Where(e => e.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID && e.Project.Requests.Any(r => r.ID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                          let organizationEventAcls = db.OrganizationEvents.Where(e => e.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID && e.Organization.Requests.Any(r => r.ID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                          let projectDataMartEventAcls = db.ProjectDataMartEvents.Where(e => e.EventID == EventIdentifiers.Request.SubmittedRequestAwaitsResponse.ID && e.Project.Requests.Any(r => r.ID == log.RequestID) && e.DataMart.Requests.Any(r => r.ID == log.RequestID) && e.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                          where ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || (Frequencies)s.Frequency == Frequencies.Immediately)
                               && (
                                    (projectEventAcls.Any() || organizationEventAcls.Any() || projectDataMartEventAcls.Any())
                                    &&
                                    (projectEventAcls.All(a => a.Allowed) && organizationEventAcls.All(a => a.Allowed) && projectDataMartEventAcls.All(a => a.Allowed))
                               )
                          select
                                new
                                {
                                    Email = s.Email,
                                    Phone = s.Phone,
                                    Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                    Username = u.UserName,
                                    UserID = s.UserID
                                }).ToArray();

                foreach (var u in r2)
                {
                    Console.WriteLine("{0:D}\t{1}\t{2}", u.UserID, u.Username, u.Name);
                }
            }

        }

        [TestMethod]
        public void GetAwaitingNotificationsforResultsReminder()
        {
            using (var db = new DataContext())
            {
                var results = (from r in db.Requests
                               where (r.Status == RequestStatuses.Complete || r.Status == RequestStatuses.ExaminedByInvestigator || r.Status == RequestStatuses.PartiallyComplete || r.Status == RequestStatuses.Submitted) && r.DataMarts.Any(dm => dm.Responses.Any()) &&
                                   (
                                   (db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && a.ProjectID == r.ProjectID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).Any() &&
                                   db.ProjectAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && a.ProjectID == r.ProjectID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).All(a => a.Allowed))
                                   ||
                                   (db.ProjectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses && a.ProjectID == r.ProjectID && a.DataMart.Requests.Any(req => req.RequestID == r.ID) && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).Any() &&
                                   db.ProjectDataMartAcls.Where(a => a.PermissionID == PermissionIdentifiers.DataMartInProject.ApproveResponses && a.ProjectID == r.ProjectID && a.DataMart.Requests.Any(requests => requests.RequestID == r.ID) && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).All(a => a.Allowed)
                                   )
                                   ||
                                   (db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).Any() &&
                                   db.GlobalAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).All(a => a.Allowed)
                                   )
                                   ||
                                   (db.OrganizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && a.OrganizationID == r.OrganizationID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).Any() &&
                                   db.OrganizationAcls.Where(a => a.PermissionID == PermissionIdentifiers.Request.ViewResults && a.OrganizationID == r.OrganizationID && (from sgu in a.SecurityGroup.Users join s in db.UserEventSubscriptions on sgu.UserID equals s.UserID where s.EventID == EventIdentifiers.Request.ResultsReminder.ID select s).Any()).All(a => a.Allowed))
                                   )
                               orderby r.Identifier
                               select r).Include("DataMarts").ToArray();

                foreach (var req in results)
                {
                    Console.WriteLine("ID: {0:D}\t\tIdentifier: {1}\t{2}", req.ID, req.Identifier, req.Name);
                }
            }
        }

        [TestMethod]
        public void GetRecipientsForResultReminder()
        {
            //Request ID should be set based on a value from GetAwaitingNotificationsforResultsReminder()
            var log = new { RequestID = new Guid("767f0001-9117-41e2-8f36-a3f701000390"), UserID = Guid.Empty };
            bool immediate = false;
            using (var db = new DataContext())
            {
                var dataMartIDs = (from rdm in db.RequestDataMarts where rdm.RequestID == log.RequestID select rdm.DataMartID).ToArray();

                var results = (from s in db.UserEventSubscriptions
                               where s.EventID == EventIdentifiers.Request.ResultsReminder.ID && !s.User.Deleted && s.User.Active &&
                                   !dataMartIDs.All(dm => db.LogsResponseViewed.Any(rv => rv.UserID == s.UserID && rv.Response.RequestDataMart.RequestID == log.RequestID && rv.Response.RequestDataMart.DataMartID == dm))
                                   && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                               from r in db.FilteredRequestListForEvent(s.UserID, null)
                               where r.ID == log.RequestID
                               select new { s, r }).Where(sub => sub.r != null).Select(sub =>
                                           new
                                           {
                                               Email = sub.s.User.Email,
                                               Phone = sub.s.User.Phone,
                                               Name = ((sub.s.User.FirstName + " " + sub.s.User.MiddleName).Trim() + " " + sub.s.User.LastName).Trim(),
                                               UserID = sub.s.UserID,
                                               Username = sub.s.User.UserName
                                           }).Union(
                               from s in db.UserEventSubscriptions
                               join u in db.Users on s.UserID equals u.ID
                               where s.UserID == log.UserID && s.EventID == EventIdentifiers.Response.ResultsViewed.ID && !s.User.Deleted && s.User.Active
                                           && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                               select new
                               {
                                   Email = u.Email,
                                   Phone = u.Phone,
                                   Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                   UserID = u.ID,
                                   Username = u.UserName
                               }
                           ).ToArray();

                Console.WriteLine("Results without security check applied:");
                foreach (var res in results)
                {
                    Console.WriteLine("UserID: {0:D}\t Username:{1}\t Name:{2}", res.UserID, res.Username, res.Name);
                }

                if(!results.Any())
                    Console.WriteLine("\tNo results.");

                Console.WriteLine("");

                var r2 = (from s in db.UserEventSubscriptions
                           let orgAcls = db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Request.ResultsReminder.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                           let projectAcls = db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Request.ResultsReminder.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == s.UserID))
                           where s.EventID == EventIdentifiers.Request.ResultsReminder.ID && !s.User.Deleted && s.User.Active 
                               && !dataMartIDs.All(dm => db.LogsResponseViewed.Any(rv => rv.UserID == s.UserID && rv.Response.RequestDataMart.RequestID == log.RequestID && rv.Response.RequestDataMart.DataMartID == dm))
                               && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                               && (
                                    (orgAcls.Any() || projectAcls.Any())
                                    &&
                                    (orgAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed))
                               )
                           from r in db.FilteredRequestListForEvent(s.UserID, null)
                           where r.ID == log.RequestID
                           select new { s, r }).Where(sub => sub.r != null).Select(sub =>
                                           new
                                           {
                                               Email = sub.s.User.Email,
                                               Phone = sub.s.User.Phone,
                                               Name = ((sub.s.User.FirstName + " " + sub.s.User.MiddleName).Trim() + " " + sub.s.User.LastName).Trim(),
                                               UserID = sub.s.UserID,
                                               Username = sub.s.User.UserName
                                           }
                           )
                           .Union(
                               from s in db.UserEventSubscriptions
                               join u in db.Users on s.UserID equals u.ID
                               let orgAcls = db.OrganizationEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == log.UserID))
                               let projectAcls = db.ProjectEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == log.UserID))
                               let projectOrgAcls = db.ProjectOrganizationEvents.Where(a => a.EventID == EventIdentifiers.Response.ResultsViewed.ID && a.Organization.Requests.Any(r => r.ID == log.RequestID) && a.Project.Requests.Any(r => r.ID == log.RequestID) && a.SecurityGroup.Users.Any(sgu => sgu.UserID == log.UserID))
                               where s.UserID == log.UserID && s.EventID == EventIdentifiers.Response.ResultsViewed.ID && !s.User.Deleted && s.User.Active
                                           && ((!immediate && (s.NextDueTime == null || s.NextDueTime <= DateTime.UtcNow)) || s.Frequency == Frequencies.Immediately)
                                           && (
                                                (orgAcls.Any() || projectAcls.Any() || projectOrgAcls.Any())
                                                &&
                                                (orgAcls.All(a => a.Allowed) && projectAcls.All(a => a.Allowed) && projectOrgAcls.All(a => a.Allowed))
                                           )
                               select new
                               {
                                   Email = u.Email,
                                   Phone = u.Phone,
                                   Name = ((u.FirstName + " " + u.MiddleName).Trim() + " " + u.LastName).Trim(),
                                   UserID = u.ID,
                                   Username = u.UserName
                               }
                           )
                           .ToArray();

                Console.WriteLine("Results WITH security check applied:");
                foreach (var res in r2)
                {
                    Console.WriteLine("UserID: {0:D}\t Username:{1}\t Name:{2}", res.UserID, res.Username, res.Name);
                }

                if (!results.Any())
                    Console.WriteLine("\tNo results.");

            }

        }

        [TestMethod]
        public void LogMetadataChangedForRequest()
        {
            Dictionary<string, object> originalValues = new Dictionary<string, object>() {
                {"Name", null },
                {"MSRequestID", null },
                {"Priority", null },
                {"DueDate", null },
                {"RequesterCenterID", null },
                {"PurposeOfUse", null },
                {"PhiDisclosureLevel", null },
                {"WorkplanTypeID", null },
                {"ActivityID", null },
                {"Description", null } 
            };

            using (var db = new DataContext())
            {
                //get the first project that has any activities
                var request = db.Requests.Where(r => r.CreatedBy.UserName == "SystemAdministrator" && r.Project.Activities.Any()).OrderByDescending(r => r.Identifier).First();

                originalValues["Name"] = request.Name;
                originalValues["MSRequestID"] = request.MSRequestID;
                originalValues["Priority"] = request.Priority;
                originalValues["DueDate"] = request.DueDate;
                originalValues["RequesterCenterID"] = request.RequesterCenterID;
                originalValues["PurposeOfUse"] = request.PurposeOfUse;
                originalValues["PhiDisclosureLevel"] = request.PhiDisclosureLevel;
                originalValues["WorkplanTypeID"] = request.WorkplanTypeID;
                originalValues["ActivityID"] = request.ActivityID;
                originalValues["Description"] = request.Description;


                request.Name += " - Changed";
                request.MSRequestID = "Changed";
                request.Priority = Priorities.Urgent;
                request.DueDate = DateTime.UtcNow;
                request.RequesterCenterID = new Guid("78B5AD5B-1E2E-E411-8185-32A6CB894C9D");
                request.PurposeOfUse = "Changed";
                request.PhiDisclosureLevel = "Changed";
                request.WorkplanTypeID = new Guid("A1E33A76-1E2E-E411-8185-32A6CB894C9D");
                request.ActivityID = new Guid("B7880001-CDC3-4A06-8A74-A2C00137B578");
                request.Description = "<p>changed</p>";

                db.SaveChanges();


                request.Name = originalValues["Name"].ToStringEx();
                request.MSRequestID = originalValues["MSRequestID"].IsNull() ? null : originalValues["MSRequestID"].ToStringEx();
                request.Priority = (Priorities)originalValues["Priority"];
                request.DueDate = originalValues["DueDate"] as DateTime?;
                request.RequesterCenterID = originalValues["RequesterCenterID"] as Guid?;
                request.PurposeOfUse = originalValues["PurposeOfUse"].IsNull() ? null : originalValues["PurposeOfUse"].ToStringEx();
                request.PhiDisclosureLevel = originalValues["PhiDisclosureLevel"].IsNull() ? null : originalValues["PhiDisclosureLevel"].ToStringEx();
                request.WorkplanTypeID = originalValues["WorkplanTypeID"] as Guid?;
                request.ActivityID = originalValues["ActivityID"] as Guid?;
                request.Description = originalValues["Description"].IsNull() ? null : originalValues["Description"].ToStringEx();

                db.SaveChanges();

            }


        }

    }
}
