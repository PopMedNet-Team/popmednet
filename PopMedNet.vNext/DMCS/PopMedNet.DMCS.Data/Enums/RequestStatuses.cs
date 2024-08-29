using System;
using System.Collections.Generic;
using System.Text;

namespace PopMedNet.DMCS.Data.Enums
{
    public enum RequestStatuses
    {
        ThirdPartySubmittedDraft = 100,
        Draft = 200,
        DraftReview = 250,
        AwaitingRequestApproval = 300,
        PendingWorkingSpecification = 310,
        WorkingSpecificationPendingReview = 320,
        SpecificationsPendingReview = 330,
        PendingSpecifications = 340,
        PendingPreDistributionTesting = 350,
        PreDistributionTestingPendingReview = 360,
        RequestPendingDistribution = 370,
        TerminatedPriorToDistribution = 390,
        RequestRejected = 400,
        Submitted = 500,
        Resubmitted = 600,
        PendingUpload = 700,
        ResponseRejectedBeforeUpload = 800,
        ResponseRejectedAfterUpload = 900,
        ExaminedByInvestigator = 1000,
        AwaitingResponseApproval = 1100,
        PartiallyComplete = 9000,
        ConductingAnalysis = 9050,
        PendingDraftReport = 9110,
        DraftReportPendingApproval = 9120,
        PendingFinalReport = 9130,
        FinalReportPendingReview = 9140,
        Hold = 9997,
        Failed = 9998,
        Cancelled = 9999,
        Complete = 10000,
        CompleteWithReport = 10100
    }
}
