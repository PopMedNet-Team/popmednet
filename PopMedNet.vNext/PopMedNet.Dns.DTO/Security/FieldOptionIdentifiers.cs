namespace PopMedNet.Dns.DTO.Security
{
    public static class FieldOptionIdentifiers
    {
        /// <summary>
        /// Permission for Request Name Field
        /// </summary>
        public const string RequestName = "Request-Name";
        /// <summary>
        /// Permission for Activity Field
        /// </summary>
        public const string Activity = "Request-Activity";
        /// <summary>
        /// Permission for Activity Originating Group Field
        /// </summary>
        public const string ActivityOriginatingGroup = "Request-Activity-Originating-Group";
        /// <summary>
        /// Permission for Activity Project Field
        /// </summary>
        public const string ActivityProject = "Request-Activity-Project";
        /// <summary>
        /// Permission for Activity Project Originating Group Field
        /// </summary>
        public const string ActivityProjectOriginatingGroup = "Request-Activity-Project-Originating-Group";
        /// <summary>
        /// Permission for Additional Instructions Field
        /// </summary>
        public const string AdditionalInstructions = "Request-Additional-Instructions";
        /// <summary>
        /// Permission for Request ID Field
        /// </summary>
        public const string RequestID = "Request-RequestID";
        /// <summary>
        /// Permission for Budget and Source Checkbox 
        /// </summary>
        public const string BudgetSourceCheckBox = "Budget-Source-CheckBox"; 
        /// <summary>
        /// Permission for Description Field
        /// </summary>
        public const string Description = "Request-Description";
        /// <summary>
        /// Permission for Due Date Field
        /// </summary>
        public const string DueDate = "Request-Due-Date"; 
        /// <summary>
        /// Permission for Level of PHI Disclosure Field
        /// </summary>
        public const string LevelOfPHIDisclosure = "Request-Level-Of-PHI-Disclosure";
        /// <summary>
        /// Permission for Priority Field
        /// </summary>
        public const string Priority = "Request-Priority";
        /// <summary>
        /// Permission for Purpose of Use Field
        /// </summary>
        public const string PurposeOfUse = "Request-Purpose-Of-Use";
        /// <summary>
        /// Permission for Requester Center Field
        /// </summary>
        public const string RequesterCenter = "Request-Requester-Center";
        /// <summary>
        /// Permission for Task Order Field
        /// </summary>
        public const string TaskOrder = "Request-Task-Order";
        /// <summary>
        /// Permission for Task Order Originating Group Field
        /// </summary>
        public const string TaskOrderOriginatingGroup = "Request-Task-Order-Originating-Group";
        /// <summary>
        /// Permission for Workplan Type Field
        /// </summary>
        public const string WorkplanType = "Request-Workplan-Type";
        /// <summary>
        /// Permission for Report Aggregation Level Field
        /// </summary>
        public const string ReportAggregationLevel = "Request-Report-Aggregation-Level";
         
        /// <summary>
        /// A collection of all the field option keys.
        /// </summary>
        public static readonly IEnumerable<string> AllFieldOptionKeys = new[] {
                Activity,
                ActivityOriginatingGroup,
                ActivityProject,
                ActivityProjectOriginatingGroup,
                AdditionalInstructions,
                RequestID,
                BudgetSourceCheckBox,
                Description,
                DueDate,
                LevelOfPHIDisclosure,
                Priority,
                PurposeOfUse,
                RequestName,
                RequesterCenter,
                TaskOrder,
                TaskOrderOriginatingGroup,
                WorkplanType,
                ReportAggregationLevel
            };
    }
}
