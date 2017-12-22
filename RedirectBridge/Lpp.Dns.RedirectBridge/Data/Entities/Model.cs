using System;
using System.Collections.Generic;

namespace Lpp.Dns.RedirectBridge
{
    public partial class Model
    {
        public Model()
        {
            this.Created = DateTime.UtcNow;
            this.RequestTypes = new HashSet<RequestType>();
        }
    
        public Guid ID { get; set; }
        public Guid ModelProcessorID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public byte[] RSA_Modulus { get; set; }
        public byte[] RSA_Exponent { get; set; }
        public DateTime Created { get; set; }
    
        public virtual ICollection<RequestType> RequestTypes { get; set; }
    }
    
}
