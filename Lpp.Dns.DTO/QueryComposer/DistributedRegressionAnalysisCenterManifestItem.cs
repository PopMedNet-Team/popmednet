using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
    /// <summary>
    /// A manifest item specifing information about an input document for the Analysis Center.
    /// </summary>
    [DataContract]
    public class DistributedRegressionAnalysisCenterManifestItem
    {
        /// <summary>
        /// Gets or sets the DocumentID.
        /// </summary>
        [DataMember]
        public Guid DocumentID { get; set; }
        /// <summary>
        /// Gets or sets the document revisionset ID.
        /// </summary>
        [DataMember]
        public Guid RevisionSetID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the response associated with the document.
        /// </summary>
        [DataMember]
        public Guid ResponseID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the datamart.
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
        /// <summary>
        /// Gets or sets the folder identifier for the datamart.
        /// </summary>
        [DataMember]
        public string DataPartnerIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the name of the datamart.
        /// </summary>
        [DataMember]
        public string DataMart { get; set; }
        /// <summary>
        /// Gets or sets the routing ID.
        /// </summary>
        [DataMember]
        public Guid RequestDataMartID { get; set; }        
    }
}
