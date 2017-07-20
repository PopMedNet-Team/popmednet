using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Security.Principal;
using System.Web;
using System.Reactive.Linq;
using System.Threading;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq.Expressions;
using System.ComponentModel.Composition.Primitives;

namespace Lpp.Composition
{
    public interface IExportScopeMetadata
    {
        string Scope { get; }
    }
}