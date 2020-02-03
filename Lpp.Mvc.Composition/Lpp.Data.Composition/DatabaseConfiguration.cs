using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;

namespace Lpp.Data.Composition
{
    public class DatabaseConfiguration : DbConfiguration
    {
        public DatabaseConfiguration()
        {
            //SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => new SqlAzureExecutionStrategy()); //This gives us connection resiliancy
            SetTransactionHandler(SqlProviderServices.ProviderInvariantName, () => new CommitFailureHandler()); //This gives us commit resiliancy
        }
    }
}
