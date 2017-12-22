using RequestCriteria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Lpp.Dns.HealthCare.DataChecker.Models
{
    [DataContract]
    public class ResponseModel
    {
        [DataMember]
        public System.Data.DataSet RawData { get; set; }

        [DataMember]
        public Guid RequestID { get; set; }

        [DataMember]
        public string ResponseToken { get; set; }

        [DataMember]
        public bool IsExternalView { get; set; }

        [DataMember]
        public IEnumerable<Guid> ResponseDocumentIDs { get; set; }

        [DataMember]
        public RxAmountTypes[] RxAmounts { get; set; }

        [DataMember]
        public RxSupTypes[] RxSups { get; set; }

        [DataMember]
        public string CodeType { get; set; }

        [DataMember]
        public MetaDataTableTypes[] MetadataTables { get; set; }
    }
}