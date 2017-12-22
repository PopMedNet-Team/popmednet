using System;
using System.ComponentModel.Composition;
using System.Linq;
using Lpp.Data;
using Lpp.Composition;
using Lpp.Dns.Portal;
using Lpp.Dns.HealthCare.FileDistribution.Data;
using Lpp.Dns.HealthCare.FileDistribution.Data.Entities;
using System.Collections.Generic;

namespace Lpp.Dns.HealthCare.FileDistribution
{
    /// <summary>
    ///
    /// </summary>
    [Export( typeof( IGarbageCollectionProvider ) )]
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    public class GarbageCollectionProvider : IGarbageCollectionProvider
    {
        [Import] IRepository<FileDistributionDomain, FileDistributionDocument> Documents { get; set; }
        [Import] IRepository<FileDistributionDomain, FileDistributionDocumentSegment> DocumentSegments { get; set; }
        [Import] IUnitOfWork<FileDistributionDomain> UnitOfWork { get; set; }

        public void GarbageCollection()
        {
            DateTime yesterday = DateTime.Today.Subtract(TimeSpan.FromDays(1));
            var docs = Documents.All.Where(d => d.Created < yesterday);

            docs.ForEach(b =>
            {
                b.Segments.ToList().ForEach(s => DocumentSegments.Remove(s));
                Documents.Remove(b);
            });

            UnitOfWork.Commit();
        }
    }
}