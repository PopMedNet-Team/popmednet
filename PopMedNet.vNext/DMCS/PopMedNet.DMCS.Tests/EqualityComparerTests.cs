using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PopMedNet.DMCS.Tests
{
    [TestClass]
    public class EqualityComparerTests
    {
        [TestMethod]
        public void CompareRequests()
        {
            var ID = Guid.Parse("{C478A83D-A874-4E41-A2C2-13F80BE77274}");
            var submittedOn = DateTime.UtcNow.Date;

            var pmnRequest = new PopMedNet.DMCS.PMNApi.PMNDto.DMCSRequest
            {
                ID = ID,
                Identifier = 220568,
                MSRequestID = "Request 220568",
                Name = "Test Request",
                Description = "Description",
                AdditionalInstructions = "Additional Instructions",
                Activity = "Activity",
                ActivityDescription = "Activity Description",
                ActivityProject = "Activity Project",
                RequestType = "Request Type",
                SubmittedOn = submittedOn,
                SubmittedBy = "Test Admin",
                Project = "Project",
                PurposeOfUse = "Purpose of Use",
                PhiDisclosureLevel = "Phi Disclosure Level",
                TaskOrder = "Task Order",
                RequestorCenter = "Requestor Center",
                WorkPlanType = "Workplan Type",
                ReportAggregationLevel = "Report Aggregation Level",
                SourceActivity = "Source Activity",
                SourceActivityProject = "Source Activity Project",
                SourceTaskOrder = "Source Task Order"
            };
        }
    }
}
