namespace PopMedNet.DMCS.Data.Enums
{
    public enum RoutingStatus
    {
        Draft = 1,
        Submitted = 2,
        Completed = 3,
        AwaitingRequestApproval = 4,
        RequestRejected = 5,
        Canceled = 6,
        Resubmitted = 7,
        PendingUpload = 8,
        AwaitingResponseApproval = 10,
        Hold = 11,
        ResponseRejectedBeforeUpload = 12,
        ResponseRejectedAfterUpload = 13,
        ExaminedByInvestigator = 14,
        ResultsModified = 16,
        Failed = 99
    }
}
