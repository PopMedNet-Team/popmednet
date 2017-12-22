using Lpp.Dns.Workflow.ModularProgram.Activities;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.ModularProgram
{
    public class ModularProgramWorkflowConfiguration : IWorkflowConfiguration
    {
        public static readonly Guid WorkflowID = new Guid("5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D");
        public static readonly Guid DraftRequestID = new Guid("0321E17F-AA1F-4B23-A145-85B159E74F0F");
        public static readonly Guid ReviewRequestID = new Guid("A96FBAD0-8FD8-4D10-8891-D749A71912F8");
        public static readonly Guid WorkingSpecificationID = new Guid("31C60BB1-2F6A-423B-A7B7-B52626FD9E97");
        public static readonly Guid WorkingSpecificationReviewID = new Guid("C8891CFD-80BF-4F71-90DE-6748BF71566C");
        public static readonly Guid SpecificationID = new Guid("C3B13067-3B9D-41E4-8D4A-7114A6E81930");
        public static readonly Guid SpecificationReviewID = new Guid("948B60F0-8CE5-4B14-9AD6-C50EC37DFC77");
        public static readonly Guid PreDistributionTestingID = new Guid("49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7");        
        public static readonly Guid PreDistributionTestingReviewID = new Guid("EA69E5ED-6029-47E8-9B45-F0F00B07FDC2");
        public static readonly Guid DistributeRequestID = new Guid("E6CCD61B-81C4-4217-A958-ADAFB5EE5554");
        public static readonly Guid ViewStatusAndResultsID = new Guid("D0E659B8-1155-4F44-9728-B4B6EA4D4D55");
        public static readonly Guid PrepareDraftReportID = new Guid("9173A8E7-27C4-469D-853D-69A78501A522");
        public static readonly Guid ReviewDraftReportID = new Guid("2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81");
        public static readonly Guid PrepareFinalReportID = new Guid("F888C5D6-B8EB-417C-9DE2-4A96D75F3208");
        public static readonly Guid ReviewFinalReportID = new Guid("2E7A3263-C87E-47BA-AC35-A78ABF8FE606");
        public static readonly Guid CompletedID = new Guid("9392ACEF-1AF3-407C-B19C-BAE88C389BFC");
        public static readonly Guid TerminateRequestID = new Guid("CC2E0001-9B99-4C67-8DED-A3B600E1C696");

        public Dictionary<Guid, Type> Activities
        {
            get
            {
                var dict = new Dictionary<Guid, Type>();
                dict.Add(DraftRequestID, typeof(DraftRequest));
                dict.Add(ReviewRequestID, typeof(RequestReview));
                dict.Add(WorkingSpecificationID, typeof(WorkingSpecification));
                dict.Add(WorkingSpecificationReviewID, typeof(WorkingSpecificationReview));
                dict.Add(SpecificationID, typeof(Specification));
                dict.Add(SpecificationReviewID, typeof(SpecificationReview));
                dict.Add(PreDistributionTestingID, typeof(PreDistributionTesting));                
                dict.Add(PreDistributionTestingReviewID, typeof(PreDistributionTestingReview));
                dict.Add(DistributeRequestID, typeof(DistributeRequest));
                dict.Add(ViewStatusAndResultsID, typeof(ViewStatusAndResults));
                dict.Add(PrepareDraftReportID, typeof(PrepareDraftReport));
                dict.Add(ReviewDraftReportID, typeof(ReviewDraftReport));
                dict.Add(PrepareFinalReportID, typeof(PrepareFinalReport));
                dict.Add(ReviewFinalReportID, typeof(ReviewFinalReport));
                dict.Add(CompletedID, typeof(Completed));
                dict.Add(TerminateRequestID, typeof(TerminateRequest));
                return dict;
            }
        }

        public Guid ID
        {
            get { return WorkflowID; }
        }
    }
}
