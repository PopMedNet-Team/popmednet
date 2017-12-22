using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient
{
    [DataContract]
    public class Response
    {
        [DataMember]
        public Guid DataMartID { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [IgnoreDataMember]
        public DateTime CreatedOnLocal
        {
            get { return CreatedOn.ToLocalTime();  }
        }
        [DataMember]
        public Profile Author { get; set; }
    }
}
