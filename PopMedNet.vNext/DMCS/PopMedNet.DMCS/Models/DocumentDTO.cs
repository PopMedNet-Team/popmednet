using PopMedNet.DMCS.Data.Enums;
using System;

namespace PopMedNet.DMCS.Models
{
    public class DocumentDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public long Length { get; set; }
        public byte[] Timestamp { get; set; }
        public DocumentType DocumentType { get; set; }
        public DocumentStates DocumentState { get; set; }
    }
}
