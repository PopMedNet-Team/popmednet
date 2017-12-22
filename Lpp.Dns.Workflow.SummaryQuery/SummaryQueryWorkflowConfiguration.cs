using Lpp.Dns.Workflow.SummaryQuery.Activities;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.SummaryQuery
{
    public class SummaryQueryWorkflowConfiguration : IWorkflowConfiguration
    {
        public static readonly Guid WorkflowID = new Guid("7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8");
        public static readonly Guid DraftRequestID = new Guid("197AF4BA-F079-48DD-9E7C-C7BE7F8DC896");
        public static readonly Guid ReviewRequestID = new Guid("CC1BCADD-4487-47C7-BDCA-1010F2C68FE0");
        public static readonly Guid DistributeRequestID = new Guid("752B83D7-2190-49DF-9BAE-983A7880A899");
        //list the routings and their status, handle grouping, and resubmitting
        public static readonly Guid ReviewResponsesID = new Guid("6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B");
        
        public static readonly Guid RespondToRequestID = new Guid("303C1C1B-A330-41DB-B3B6-4D7C02D02C8C");
        //respond to request goes back to review responses with either Upload Response or Reject Response. What is this step?
        //linked to review response through add/remove datamart and resubmit? TODO: clarify
        
        public static readonly Guid SubmitDraftReportID = new Guid("9173A8E7-27C4-469D-853D-69A78501A522");
        public static readonly Guid ReviewDraftReportID = new Guid("2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81");
        public static readonly Guid SubmitFinalReportID = new Guid("F888C5D6-B8EB-417C-9DE2-4A96D75F3208");
        public static readonly Guid ReviewFinalReportID = new Guid("2E7A3263-C87E-47BA-AC35-A78ABF8FE606");
        public static readonly Guid CompletedID = new Guid("9392ACEF-1AF3-407C-B19C-BAE88C389BFC");
        public static readonly Guid TerminateActivityID = new Guid("CC2E0001-9B99-4C67-8DED-A3B600E1C696");

        public Dictionary<Guid, Type> Activities
        {
            get {
                var dict = new Dictionary<Guid, Type>();
                dict.Add(SummaryQueryWorkflowConfiguration.DraftRequestID, typeof(DraftRequest));
                dict.Add(SummaryQueryWorkflowConfiguration.ReviewRequestID, typeof(RequestReview));
                dict.Add(SummaryQueryWorkflowConfiguration.DistributeRequestID, typeof(DistributeRequest));
                dict.Add(SummaryQueryWorkflowConfiguration.ReviewResponsesID, typeof(ReviewResponses));
                dict.Add(SummaryQueryWorkflowConfiguration.RespondToRequestID, typeof(RespondToRequest));
                dict.Add(SummaryQueryWorkflowConfiguration.SubmitDraftReportID, typeof(SubmitDraftReport));
                dict.Add(SummaryQueryWorkflowConfiguration.ReviewDraftReportID, typeof(ReviewDraftReport));
                dict.Add(SummaryQueryWorkflowConfiguration.SubmitFinalReportID, typeof(SubmitFinalReport));
                dict.Add(SummaryQueryWorkflowConfiguration.ReviewFinalReportID, typeof(ReviewFinalReport));
                dict.Add(SummaryQueryWorkflowConfiguration.CompletedID, typeof(Completed));
                
                return dict;
            }
        }

        public Guid ID
        {
            get { return WorkflowID; }
        }
    }
}
