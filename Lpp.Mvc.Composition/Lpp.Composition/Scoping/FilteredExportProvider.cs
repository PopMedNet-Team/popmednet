using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace Lpp.Composition
{
    public class FilteredExportProvider : ExportProvider
    {
        private readonly ExportProvider _inner;
        private readonly Func<ExportDefinition, bool> _filter;

        public FilteredExportProvider( ExportProvider inner, Func<ExportDefinition,bool> filter )
        {
            //Contract.Requires( inner != null );
            //Contract.Requires( filter != null );
            _inner = inner;
            _filter = filter;
        }

        protected override IEnumerable<Export> GetExportsCore( ImportDefinition definition, AtomicComposition atomicComposition )
        {
            return _inner.GetExports( definition, atomicComposition ).Where( e => _filter( e.Definition ) );
        }
    }
}