using Lpp.Dns.Data;
using Lpp.Utilities;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Lpp.Utilities.Security;
using Lpp.Utilities.Logging;
using System.Text;
using System.Reflection;
using Lpp.Utilities.WebSites;
using System.Collections.Concurrent;
using System.Web.Configuration;
using System.Diagnostics;

namespace Lpp.Dns.Api.Controllers
{
    /// <summary>
    /// Controller that services notification requests.
    /// </summary>
    public class NotificationsController : LppApiController<DataContext>
    {
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(NotificationsController));

        


        /// <summary>
        /// Runs the scheduled notifications based on the permissions of the user used to login.
        /// Will fail if the user is not of type Background Task.
        /// </summary>
        /// <returns>Http OK or Forbidden</returns>
        [HttpGet, AllowAnonymous]
        public async Task<HttpResponseMessage> ExecuteScheduledNotifications(string userName, string password)
        {
            var bguser = await (from u in DataContext.Users where u.UserName == userName && u.Active && !u.Deleted select u).FirstOrDefaultAsync();

            if (bguser == null || bguser.UserType != DTO.Enums.UserTypes.BackgroundTask || bguser.PasswordHash != password.ComputeHash())
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to execute this notification. Either the login, password or User Type is wrong for the specified credentials");

            //Handle cleaning of documents
            //This is done here because we're executing notifications on a daily basis. This can be moved if necessary.
            var days = WebConfigurationManager.AppSettings["KeepResponseDocumentsDays"].ToInt32();
            if (days > 0)
            {
                try
                {
                    var dt = DateTime.UtcNow.AddDays(days * -1);

                    var docIds = await (from d in DataContext.Documents where d.CreatedOn < dt select d.ID).ToArrayAsync();

                    Logger.Debug($"{docIds.Count()} Documents pending deletion.");

                    await DataContext.Database.ExecuteSqlCommandAsync("DELETE FROM Documents where CreatedOn < {0}", dt);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error happened when deleting documents: ", ex);
                }
            }

            var runTime = DateTime.UtcNow;

            var Logging = new ConcurrentBag<object>();
            //Get all of the loggers
            var assemblies = ObjectEx.GetNonSystemAssemblies();
            foreach (var assembly in assemblies.AsParallel())
            {
                var logTypesToRegister = assembly.GetTypes().Where(
                                                    type => type.BaseType != null && !type.IsAbstract &&
                                                    type.BaseType.IsGenericType &&
                                                    (type.BaseType.GetGenericTypeDefinition() == typeof(EntityLoggingConfiguration<,>)));

                foreach (object configurationInstance in logTypesToRegister.Select(Activator.CreateInstance))
                    Logging.Add(configurationInstance);
            }


            var notifications = new ConcurrentBag<Notification>();

            //Loop through all of them generating notifications
            foreach(var log in Logging.AsParallel()) {
                try
                {
                    var method = log.GetType().GetMethod("GenerateNotificationsFromLogs");
                    var task = (Task<IEnumerable<Notification>>)method.Invoke(log, new object[] { DataContext });
                    foreach (var n in await task)
                        notifications.Add(n);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error Occured while executing notifications", ex);
                }
            }

            //Flip the results so that we get a list of users, and a sub list of their notifications.
            var users = (from n in notifications from r in n.Recipients select r).DistinctBy(n => n.Email);

            var sendNotifications = new ConcurrentBag<Notification>();
            foreach (var user in users.AsParallel())
            {
                try
                {
                    //We are multi-threading this so that we can send as many as possible at one time.
                    //Create a single notification for the user

                    var notification = new Notification
                    {
                        Recipients = new Recipient[] { user },
                    };

                    var uNotes = notifications.Where(n => n.Recipients.Any(r => r.Email == user.Email)).Select(n => new { n.Body, n.Subject });

                    if (uNotes.Count() > 1)
                    {//If there is more than one notification being aggrigated then set the subject to a generic title.
                        notification.Subject = "Notifications and Reminders";
                    }
                    else //If there is only one, use the specific subject created by the notification.
                    {
                        notification.Subject = uNotes.First().Subject;
                    }

                    notification.Body = string.Join("<hr/>", uNotes.Select(n => n.Body).ToArray());

                    //This adds it the aggrigate notification to the list of notifications that will be sent.
                    //Each of these notifications only have one recipient.
                    sendNotifications.Add(notification);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error Occured while sending notifications", ex);
                }
            }

            //Create the notifier
            var notifier = new Notifier();

            //Asynchronously send all of the notifications
           
            await Task.Run(() => notifier.Notify(sendNotifications.AsEnumerable()));

            //Upon completion update all of the notifications that should have run with the new time that they should be run again.
            var subscriptions = await (from s in DataContext.UserEventSubscriptions where (s.NextDueTime == null || s.NextDueTime.Value < runTime) && s.User.Active && s.Frequency != DTO.Enums.Frequencies.Immediately select s).ToArrayAsync();
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

            var mySubscriptions = await (from s in DataContext.UserEventSubscriptions where (s.NextDueTime == null || s.NextDueTime.Value < runTime) && s.User.Active && s.FrequencyForMy != DTO.Enums.Frequencies.Immediately select s).ToArrayAsync();
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

            await DataContext.SaveChangesAsync();

            if (Request != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                //request does not exist when run in test harness.
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

    }
}
