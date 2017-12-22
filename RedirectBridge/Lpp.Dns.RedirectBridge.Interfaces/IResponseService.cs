using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ServiceModel.Web;

namespace Lpp.Dns
{
    [ServiceContract( Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    public interface IResponseService
    {
        /// <summary>
        /// Retrieve metadata associated with the current session
        /// </summary>
        /// <param name="sessionToken">Identifies the current communication session.</param>
        /// <returns>Metadata</returns>
        [OperationContract, WebGet( UriTemplate="{sessionToken}/Session" )]
        [FaultContract(typeof(InvalidSessionFault))]
        ResponseSessionMetadata GetSessionMetadata( string sessionToken );
    }

    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    public struct ResponseSessionMetadata
    {
        [DataMember] public SessionMetadata Session { get; set; }
        [DataMember] public DataMartResponse[] DataMartResponses { get; set; }
    }

    /// <summary>
    /// Describes response that came from a single datamart - which includes identification
    /// of the datamart in question and one or more documents.
    /// </summary>
    [DataContract( Namespace="http://lincolnpeak.com/schemas/DNS4/API" )]
    public struct DataMartResponse
    {
        [DataMember] public DataMart DataMart { get; set; }
        [DataMember] public Document[] Documents { get; set; }
    }
}