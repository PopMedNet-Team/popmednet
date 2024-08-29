using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Models
{
    /// <summary>
    /// Contains the user permissions for a routing.
    /// </summary>
    public class RoutePermissionsComponent
    {
        /// <summary>
        /// Gets or sets the ID of the user the permissions apply to.
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the routing the permissions are applicable for.
        /// </summary>
        public Guid RequestDataMartID { get; set; }
        /// <summary>
        /// Gets or sets if the user can see the request.
        /// </summary>
        public bool SeeRequest { get; set; }
        /// <summary>
        /// Gets or sets if the user can upload results.
        /// </summary>
        public bool UploadResults { get; set; }
        /// <summary>
        /// Gets or sets if the user can hold the routing.
        /// </summary>
        public bool HoldRequest { get; set; }
        /// <summary>
        /// Gets or sets if the user can reject a request.
        /// </summary>
        public bool RejectRequest { get; set; }
        /// <summary>
        /// Gets or sets if the user can modify results for the routing after submitting the initial results.
        /// </summary>
        public bool ModifyResults { get; set; }
        /// <summary>
        /// Gets or sets if the user can view attachments for the routing.
        /// </summary>
        public bool ViewAttachments { get; set; }
        /// <summary>
        /// Gets or sets if the user can modify attachments for the routing.
        /// </summary>
        public bool ModifyAttachments { get; set; }
    }
}
