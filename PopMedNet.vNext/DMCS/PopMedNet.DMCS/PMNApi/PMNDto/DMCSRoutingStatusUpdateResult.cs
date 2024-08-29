using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.PMNApi.PMNDto
{
    public class DMCSRoutingStatusUpdateResult
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        public System.Net.HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Gets the status message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the ID of the RequestDataMart.
        /// </summary>
        public Guid RequestDataMartID { get; set; }
        /// <summary>
        /// Gets or sets the routing status.
        /// </summary>
        public Data.Enums.RoutingStatus RoutingStatus { get; set; }
        /// <summary>
        /// Gets or sets the timestamp for the RequestDataMart.
        /// </summary>
        public byte[] RequestDataMartTimestamp { get; set; }
    }
}
