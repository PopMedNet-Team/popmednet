using System;
using System.Collections.Generic;

namespace Lpp.Dns.RedirectBridge
{
    public partial class RequestType
    {
        public int Id { get; set; }
        public virtual Model Model { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsMetadataRequest { get; set; }
        public string CreateRequestUrl { get; set; }
        public string RetrieveResponseUrl { get; set; }
        public System.Guid LocalId { get; set; }
    }
}
