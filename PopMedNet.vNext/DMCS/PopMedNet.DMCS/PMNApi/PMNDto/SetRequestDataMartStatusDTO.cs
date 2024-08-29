using PopMedNet.DMCS.Data.Enums;
using System;

namespace PopMedNet.DMCS.PMNApi.PMNDto
{
    public class SetRequestDataMartStatusDTO
    {
        public Guid RequestDataMartID { get; set; }
        public RoutingStatus Status { get; set; }
        public string Message { get; set; }
    }
}
