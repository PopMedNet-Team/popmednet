using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;

namespace Lpp.Audit
{
    public interface IAuditProperty : IComparable<IAuditProperty>
    {
        Guid ID { get; }
        string Name { get; }
        Type Type { get; }
        object GetValue( Data.AuditPropertyValue v );
    }

    public interface IAuditProperty<T> : IAuditProperty
    {
    }
}