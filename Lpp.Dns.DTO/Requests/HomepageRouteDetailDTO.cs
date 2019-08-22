using Lpp.Dns.DTO.Enums;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request details for a specific datamart routing.
    /// </summary>
    [DataContract]
    public class HomepageRouteDetailDTO
    {
        /// <summary>
        /// Gets or Sets the Identifier of the RequestDatamart.
        /// </summary>
        [DataMember]
        public Guid RequestDataMartID { get; set; }
        /// <summary>
        /// Gets or Sets the Name of the Datamart.
        /// </summary>
        [DataMember]
        public string DataMart { get; set; }
        /// <summary>
        /// Gets or Sets the Identifier of the Datamart.
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
        /// Gets or Sets the Type of Routing this Route is
        /// </summary>
        [DataMember]
        public RoutingType? RoutingType { get; set; }
        /// <summary>
        /// Gets or sets the Identifier of the Request.
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
        /// <summary>
        /// Gets or sets the name of the Request.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the Identifier (System Number) of the Request.
        /// </summary>
        [DataMember, ReadOnly(true)]
        public long Identifier { get; set; }
        /// <summary>
        /// Gets or sets the date the Request was submitted on.
        /// </summary>
        [DataMember]
        public DateTimeOffset? SubmittedOn { get; set; }
        /// <summary>
        /// Gets or sets the username of the user that submitted the Request.
        /// </summary>
        [DataMember]
        public string SubmittedByName { get; set; }
        /// <summary>
        /// Gets or Sets the Identifier of the Current Response.
        /// </summary>
        [DataMember]
        public Guid ResponseID { get; set; }
        /// <summary>
        /// Gets or Sets the Date when the Current Response was submitted on.
        /// </summary>
        [DataMember]
        public DateTimeOffset? ResponseSubmittedOn { get; set; }
        /// <summary>
        /// Gets or Sets the Identifier if the User who Submitted the Current Response.
        /// </summary>
        [DataMember]
        public Guid? ResponseSubmittedByID { get; set; }
        /// <summary>
        /// Gets or Sets the UserName of the user who submitted the Current Response.
        /// </summary>
        [DataMember]
        public string ResponseSubmittedBy { get; set; }
        /// <summary>
        /// Gets or Sets the Time the Current Response was Responded To.
        /// </summary>
        [DataMember]
        public DateTime? ResponseTime { get; set; }
        /// <summary>
        /// Gets or Sets the Identifier of the User who Responded to the Route.
        /// </summary>
        [DataMember]
        public Guid? RespondedByID { get; set; }
        /// <summary>
        /// Gets or Sets the Username of the User who Responded to the Route.
        /// </summary>
        [DataMember]
        public string RespondedBy { get; set; }
        /// <summary>
        /// Gets or Sets the Identifier of the Response Group of the Current Response.
        /// </summary>
        [DataMember]
        public Guid? ResponseGroupID { get; set; }
        /// <summary>
        /// Gets or Sets the name of the Response Group of the Current Response.
        /// </summary>
        [DataMember]
        public string ResponseGroup { get; set; }
        /// <summary>
        /// Gets or Sets the Message for the Current Reponse.
        /// </summary>
        [DataMember]
        public string ResponseMessage { get; set; }
        /// <summary>
        /// Gets or sets the display text for the status of the Request.
        /// </summary>
        [DataMember]
        public string StatusText { get; set; }
        /// <summary>
        /// Gets or sets the status of the Request.
        /// </summary>
        [DataMember]
        public DTO.Enums.RequestStatuses RequestStatus { get; set; }
        /// <summary>
        /// Gets or Sets The status of the routing.
        /// </summary>
        [DataMember]
        public DTO.Enums.RoutingStatus RoutingStatus { get; set; }
        /// <summary>
        /// Gets or Sets The display text for the status of the routing.
        /// </summary>
        [DataMember]
        public string RoutingStatusText { get; set; }
        /// <summary>
        /// Gets or sets the name of the RequestType.
        /// </summary>
        [DataMember]
        public string RequestType { get; set; }
        /// <summary>
        /// Gets or sets the name of the Project.
        /// </summary>
        [DataMember]
        public string Project { get; set; }
        /// <summary>
        /// Gets or sets the Priority of the Request.
        /// </summary>
        [DataMember]
        public DTO.Enums.Priorities Priority { get; set; }
        /// <summary>
        /// Gets or sets the Due Date of the Request.
        /// </summary>
        [DataMember]
        public DateTimeOffset? DueDate { get; set; }
        /// <summary>
        /// Gets or sets the Mini-Sentiel Request ID for the Request.
        /// </summary>
        [DataMember]
        public string MSRequestID { get; set; }
        /// <summary>
        /// Gets or sets if the Request uses workflow.
        /// </summary>
        [DataMember]
        public bool IsWorkflowRequest { get; set; }
        /// <summary>
        /// Gets or sets if the user can edit the request's metadata.
        /// </summary>
        [DataMember]
        public bool CanEditMetadata { get; set; }
        /// <summary>
        /// Executes a comparision of the specified object using the RequestDataMartID to determine if they are the same entity.
        /// Does not compare any other properties.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is HomepageRouteDetailDTO)
            {
                var DTO = obj as HomepageRouteDetailDTO;
                return DTO.RequestDataMartID == this.RequestDataMartID;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Gets the hashcode of the RequestDataMartID.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return RequestDataMartID.GetHashCode();
        }

        /// <summary>
        /// Compares the RequestDataMartID of the specified object to determine if it is the same entity.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is HomepageRouteDetailDTO))
                return -1;

            var ob = obj as HomepageRouteDetailDTO;

            return this.RequestDataMartID.CompareTo(ob.RequestDataMartID);
        }

        /// <summary>
        /// Returns the RequestDataMartID of the route.
        /// </summary>
        /// <param name="o">The HomepageRouteDetail representing a specific route.</param>
        /// <returns></returns>
        public static implicit operator Guid(HomepageRouteDetailDTO o)
        {
            return o.RequestDataMartID;
        }

    }
}
