using PopMedNet.DMCS.Data.Enums;
using System;

namespace PopMedNet.DMCS.PMNApi.PMNDto
{
    public class DMCSRequestDocument
    {
        public Guid ResponseID { get; set; }
        public Guid RevisionSetID { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}
