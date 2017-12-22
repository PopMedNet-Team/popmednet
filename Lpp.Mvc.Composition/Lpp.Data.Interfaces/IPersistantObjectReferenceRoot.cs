using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Data
{
    public interface IPersistentObjectReferenceRoot<TDomain, TRefTarget>
    {
        Expression<Func<TRefTarget, bool>> IsObjectReferenced { get; }
    }
}