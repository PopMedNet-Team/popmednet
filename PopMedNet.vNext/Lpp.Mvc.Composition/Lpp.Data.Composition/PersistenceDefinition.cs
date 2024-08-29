using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.Composition;

namespace Lpp.Data.Composition
{
    public class PersistenceDefinition<TDomain,TEntity> : IPersistenceDefinition<TDomain>
        where TEntity : class
    {
        public virtual void BuildModel( DbModelBuilder builder )
        {
            builder.Entity<TEntity>();
        }
    }
}