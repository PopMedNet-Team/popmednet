using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Lpp.Composition;

namespace Lpp.Security
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class SecurityObjectHierarchyService<TDomain> : ISecurityObjectHierarchyService<TDomain>
    {
        [Import] public DagService<TDomain, Guid, Data.InheritanceEdge, Data.InheritanceClosureEdge> Dag { get; set; }

        public void SetObjectInheritanceParent( ISecurityObject obj, ISecurityObject parent )
        {
            if ( obj.Kind != parent.Kind ) throw new InvalidOperationException( "Trying to set an object of kind " + parent.Kind + " as a parent for object of kind " + obj.Kind + ". Parent and child in access control inheritance tree must be of same kind." );
            Dag.SetAdjacency( obj.ID, new[] { parent.ID } );
        }

        public IQueryable<Guid> GetObjectChildren( ISecurityObject obj, bool includeSelf = false )
        {
            return Dag.GetAdjacentStarts( obj.ID, true, includeSelf );
        }

        public IQueryable<Guid> GetObjectTransitiveChildren( ISecurityObject obj, bool includeSelf = false )
        {
            return Dag.GetAdjacentStarts( obj.ID, false, includeSelf );
        }

        public Expression<Func<Guid, IQueryable<Guid>>> GetObjectChildren( bool includeSelf )
        {
            return Dag.GetAdjacentStarts( true, includeSelf );
        }

        public Expression<Func<Guid, IQueryable<Guid>>> GetObjectTransitiveChildren( bool includeSelf )
        {
            return Dag.GetAdjacentStarts( false, includeSelf );
        }
    }
}