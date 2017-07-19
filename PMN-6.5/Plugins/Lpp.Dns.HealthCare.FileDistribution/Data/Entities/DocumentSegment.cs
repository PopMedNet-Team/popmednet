using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.FileDistribution.Data.Entities
{
    public partial class FileDistributionDocumentSegment
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public FileDistributionDocument Document { get; set; }
    }
}
