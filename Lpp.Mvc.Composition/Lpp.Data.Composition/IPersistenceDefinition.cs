using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Lpp.Data.Composition
{
    public interface IPersistenceDefinition<TDomain>
    {
        void BuildModel( DbModelBuilder builder );
    }
}