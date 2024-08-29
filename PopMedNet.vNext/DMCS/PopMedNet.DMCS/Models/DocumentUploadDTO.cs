using Microsoft.AspNetCore.Http;
using System;

namespace PopMedNet.DMCS.Models
{
    public class DocumentUploadDTO
    {
        public Guid UploadID { get; set; }
        public Guid RequestDataMartID { get; set; }
        public Guid RequestResponseID { get; set; }
        public int TotalChunks { get; set; }
        public int CurrentChunk { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public IFormFile File { get; set; }
    }
}
