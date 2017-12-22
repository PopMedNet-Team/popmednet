using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lpp.Composition.Modules;
using System.Linq.Expressions;
using System.Linq;
using System.Xml.Linq;
using Lpp.Audit.Data;

namespace Lpp.Audit
{
    public interface IFactory<TDefinition, TObject>
    {
        Guid Id { get; }
        TObject Deserialize( TDefinition definition );
        TDefinition Serialize( TObject obj );
    }
}