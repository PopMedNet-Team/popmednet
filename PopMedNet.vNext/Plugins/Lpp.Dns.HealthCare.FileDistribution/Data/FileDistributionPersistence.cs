using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Data.Composition;
using System.Data.Entity;
using System.ComponentModel.Composition;
using Lpp.Dns.HealthCare.FileDistribution.Data.Entities;
using Lpp.Data;

namespace Lpp.Dns.HealthCare.FileDistribution.Data
{
    [Export(typeof(IPersistenceDefinition<FileDistributionDomain>))]
    class FileUploadDocumentPersistence : IPersistenceDefinition<FileDistributionDomain>
    {
        public void BuildModel(DbModelBuilder builder)
        {
            var docs = builder.Entity<FileDistributionDocument>();
            var segs = builder.Entity<FileDistributionDocumentSegment>();
        }

    }
}
