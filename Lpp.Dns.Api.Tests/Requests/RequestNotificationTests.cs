using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
