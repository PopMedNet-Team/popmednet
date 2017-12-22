using System;
using System.ComponentModel.Composition;
using System.Linq;
using Lpp.Data;
using Lpp.Composition;
using Lpp.Dns.Portal;
using Lpp.Dns.HealthCare.ModularProgram.Data;
using Lpp.Dns.HealthCare.ModularProgram.Data.Entities;
using System.Collections.Generic;

namespace Lpp.Dns.HealthCare.ModularProgram
{
    /// <summary>
    ///
    /// </summary>
    [Export( typeof( IGarbageCollectionProvider ) )]
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    public class GarbageCollectionProvider : IGarbageCollectionProvider
    {
        [Import] IRepository<ModularProgramDomain, ModularProgramDocument> Documents { get; set; }
        [Import] IRepository<ModularProgramDomain, ModularProgramDocumentSegment> DocumentSegments { get; set; }
        [Import] IUnitOfWork<ModularProgramDomain> UnitOfWork { get; set; }

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