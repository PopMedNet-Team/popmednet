using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reactive;
using System.Diagnostics.Contracts;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;

namespace Lpp.Dns
{
    public interface IDnsDataMart
    {
        Guid ID { get; }
        string Name { get; }
        string Organization { get; }
        IEnumerable<Document> MetadataDocuments { get; }
        Lpp.Dns.DTO.Enums.Priorities Priority { get; set; }
        DateTime? DueDate { get; set; }
    }
}