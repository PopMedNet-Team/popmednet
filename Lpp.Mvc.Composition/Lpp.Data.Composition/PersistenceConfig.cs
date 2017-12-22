using System;
using System.Data.Common;

namespace Lpp.Data.Composition
{
    public class PersistenceConfig<TDomain>
    {
        public string ConnectionString { get; set; }
        public Func<DbConnection> CreateConnection { get; set; }

        public int? CommandTimeoutSeconds { get; set; }
    }
}