using PopMedNet.DMCS.Data.Enums;
using System;

namespace PopMedNet.DMCS.Models
{
    public class RoutingDTO
    {
        /// <summary>
        /// Gets or sets the ID of the route (RequestDataMart ID).
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Gets or sets the name of the Request.
        /// </summary>
        public string RequestName { get; set; }
        /// <summary>
        /// Gets or sets the RequestType name.
        /// </summary>
        public string RequestType { get; set; }
        /// <summary>
        /// Gets or sets the name of the Data Model.
        /// </summary>
        public string DataModel { get; set; }
        /// <summary>
        /// Gets or sets the MSRequestID.
        /// </summary>
        public string MSRequestID { get; set; }
        /// <summary>
        /// Gets or sets the name of the project the request belongs to.
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// Gets or sets the priority of the route.
        /// </summary>
        public Priorities Priority { get; set; }
        /// <summary>
        /// Gets or sets the due date of the route.
        /// </summary>
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// Gets or sets the status of the route.
        /// </summary>
        public RoutingStatus Status { get; set; }
        /// <summary>
        /// Gets or sets the name of the user that submitted the route.
        /// </summary>
        public string SubmittedBy { get; set; }
        /// <summary>
        /// Gets or set s the date the request was submitted.
        /// </summary>
        public DateTime RequestDate { get; set; }
        /// <summary>
        /// Gets or sets the name of the DataMart for the route.
        /// </summary>
        public string DataMartName { get; set; }
        /// <summary>
        /// Gets or sets the name of the user that responded to the route.
        /// </summary>
        public string RespondedBy { get; set; }
        /// <summary>
        /// Gets or sets the date the route was last responded on.
        /// </summary>
        public DateTime? RespondedDate { get; set; }
        /// <summary>
        /// Gets or sets the Request Identifier number.
        /// </summary>
        public long RequestIdentifier { get; set; }
    }
}
