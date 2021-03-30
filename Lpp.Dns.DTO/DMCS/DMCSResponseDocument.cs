using System;

namespace Lpp.Dns.DTO.DMCS
{
    public class DMCSResponseDocument
    {
        public Guid DocumentID { get; set; }
        public Guid RevisionSetID { get; set; }
        public Guid ResponseID { get; set; }
        public Guid RequestID { get; set; }
        public Guid DataMartID { get; set; }
        public string FileName { get; set; }
        public long Length { get; set; }
        public string MimeType { get; set; }
        public int CurrentChunk { get; set; }
        public int TotalChunks { get; set; }
    }
}
