using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.Scheduler.Models;
using Lpp.Dns.Scheduler.Controllers;

namespace Lpp.Dns.Scheduler.Tests
{
    [TestClass]
    public class ScheduleTests
    {
        [TestMethod]
        public void Daily()
        {
            RequestSchedulerModel model = new RequestSchedulerModel()
            {
                StartDate = DateTime.Now,
                RunTime = DateTime.Now.AddMinutes(1.0),
                RecurrenceType = "Daily",
                DailyType = "EveryNDays",
                NDays = 1
            };

            try
            {
                new SchedulerController().ScheduleJob(6, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), model);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
