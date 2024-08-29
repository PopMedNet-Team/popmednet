using PopMedNet.DMCS.Data.Model;
using System;

namespace PopMedNet.DMCS.PMNApi.PMNDto
{
    public class DMCSResponse : Data.Model.IResponseMetadata
    {
        public Guid ID { get; set; }
        public Guid RequestDataMartID { get; set; }
        public string RespondedBy { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime? ResponseTime { get; set; }
        public int Count { get; set; }
        public byte[] Timestamp { get; set; }
        byte[] IResponseMetadata.PmnTimestamp => Timestamp;
    }
}
