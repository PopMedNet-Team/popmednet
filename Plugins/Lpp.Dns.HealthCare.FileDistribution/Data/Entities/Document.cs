using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.FileDistribution.Data.Entities
{
    public partial class FileDistributionDocument
    {
        public FileDistributionDocument()
        {
            this.Segments = new HashSet<FileDistributionDocumentSegment>();
            this.Created = DateTime.Now;
            this.MimeType = "application/octet-stream";
       }

        public int Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public System.DateTime Created { get; set; }
        public int RequestId { get; set; }
        public virtual ICollection<FileDistributionDocumentSegment> Segments { get; set; }
    }
}
