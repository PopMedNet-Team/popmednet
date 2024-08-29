using System;

namespace PopMedNet.DMCS.Models
{
    public class RequestMetadataDTO : RoutingDTO
    {
        public Guid RequestID { get; set; }
        public Guid ResponseID { get; set; }
        public string ResponseMessage { get; set; }
        public string PurposeOfUse { get; set; }
        public string LevelOfReportAggregation { get; set; }
        public string SourceTaskOrder { get; set; }
        public string SourceActivity { get; set; }
        public string SourceActivityProject { get; set; }
        public string TaskOrder { get; set; }
        public string Activity { get; set; }
        public string ActivityProject { get; set; }
        public string RequestorCenter { get; set; }
        public string AdditionalInstructions { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the user permissions for this routing.
        /// </summary>
        public RoutePermissionsComponent Permissions { get; set; }
    }
}
