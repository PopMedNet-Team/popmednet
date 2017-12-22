using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.Scheduler.Jobs;
using Quartz;
using System.ServiceModel;
using Lpp.Dns.Scheduler.Rest;

namespace Lpp.Dns.Scheduler.Tests
{
    [TestClass]
    public class SchedulerJobTests
    {
        [TestMethod]
        public void TestExecute()
        {
            string url = "http://localhost:3872/api/soap/scheduler";
            using (var factory = new ChannelFactory<ISchedulerRestService>(new BasicHttpBinding(), url))
            {
                var channel = factory.CreateChannel();
                try
                {
                    channel.SubmitSchedulerRequest("1", "21");
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
