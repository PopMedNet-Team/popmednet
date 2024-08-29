using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Reponse History
    /// </summary>
    [DataContract]
    public class ResponseHistoryDTO
    {
        /// <summary>
        /// Response History
        /// </summary>
        public ResponseHistoryDTO()
        {
            HistoryItems = new List<ResponseHistoryItemDTO>();
        }

        /// <summary>
        /// Gets or Sets Name of the DataMart
        /// </summary>
        [DataMember]
        public string DataMartName { get; set; }


        /// <summary>
        /// Gets or Sets Response History Items
        /// </summary>
        [DataMember]
        public IEnumerable<ResponseHistoryItemDTO> HistoryItems { get; set; }

        /// <summary>
        /// Gets or sets an error message for the response history retrieval attempt.
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }

    }
}
