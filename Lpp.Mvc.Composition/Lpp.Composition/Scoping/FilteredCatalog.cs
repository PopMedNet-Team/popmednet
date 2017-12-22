using System;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace Lpp.Composition
{
    public class FilteredCatalog : ComposablePartCatalog 
    {
        private readonly IQueryable<ComposablePartDefinition> _parts;

        public FilteredCatalog( ComposablePartCatalog parentCatalog, Expression<Func<ComposablePartDefinition,bool>> predicate )
        {
            //Contract.Requires( parentCatalog != null );
            //Contract.Requires( predicate != null );

            _parts = parentCatalog.Parts.Where( predicate );
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return _parts; }
        }
    }
}