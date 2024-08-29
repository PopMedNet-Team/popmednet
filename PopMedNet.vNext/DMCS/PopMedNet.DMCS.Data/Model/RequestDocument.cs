using PopMedNet.DMCS.Data.Enums;
using System;
using System.Collections.Generic;

namespace PopMedNet.DMCS.Data.Model
{
    public class RequestDocument
    {
        public Guid ResponseID { get; set; }
        public Response Response { get; set; }
        public Guid RevisionSetID { get; set; }

        public DocumentType DocumentType { get; set; }
    }
}
