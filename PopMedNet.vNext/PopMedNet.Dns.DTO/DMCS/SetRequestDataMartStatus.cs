using PopMedNet.Dns.DTO.Enums;
using System;

namespace PopMedNet.Dns.DTO.DMCS
{
    public class SetRequestDataMartStatus
    {
        public Guid RequestDataMartID { get; set; }
        public RoutingStatus Status { get; set; }
        public string Message { get; set; }
    }
}
