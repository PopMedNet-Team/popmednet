using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request details for a specific datamart routing.
    /// </summary>
    [DataContract]
    public class HomepageRouteDetailDTO
    {
        /// <summary>
        /// Gets or sets the RequestDataMartID.
        /// </summary>
        [DataMember]
        public Guid RequestDataMartID { get; set; }
        /// <summary>
        /// Gets or sets the DataMartID.
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
        /// Gets or sets the RequestID.
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
        /// The status of the routing.
        /// </summary>
        [DataMember]
        public DTO.Enums.RoutingStatus RoutingStatus { get; set; }
        /// <summary>
        /// The display text for the status of the routing.
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
