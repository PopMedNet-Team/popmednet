using System;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Lpp.Audit.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lpp.Security;
using Lpp.Utilities.Legacy;
namespace Lpp.Audit
{
    public static class AuditExtensions
    {
        public static Expression<Func<AuditEvent, bool>> AsExpression( this ILookup<AuditEventKind, IAuditEventFilter> filters )
        {
            return (from k in filters
                    from f in k
                    select f == null ?
                        Expr.Create( ( Data.AuditEvent e ) => e.KindId == k.Key.ID ) :
                        Expr.Create( ( Data.AuditEvent e ) => e.KindId == k.Key.ID && f.Filter.Invoke( e ) )
                   )
                   .StartWith( e => false )
                   .Fold( Expression.Or )
                   .Expand();
        }

        public static ILookup<AuditEventKind, IAuditEventFilter> DeserializeFilters<TDomain>( this IAuditService<TDomain> service, string filtersXml )
        {
            return service.DeserializeFilters( XElement.Parse( filtersXml ) );
        }

        public static IQueryable<AuditEventView> GetEvents<TDomain>( this IAuditService<TDomain> service, DateTime? from, DateTime? to, string filtersXml )
        {
            return service.GetEvents( from, to, service.DeserializeFilters( filtersXml ) );
        }

        public static IAuditEventBuilder With<T>( this IAuditEventBuilder builder, IAuditProperty<T> prop, T value )
        {
            var pv = new AuditPropertyValue { PropertyId = prop.ID };
            pv.SetValue( value );
            return builder.With( pv );
        }

        public static IAuditEventBuilder CreateEvent<TDomain,TEvent>( this IAuditService<TDomain> service, SecurityTarget target, TEvent e ) where TEvent : class
        {
            return Aud.AsPropertyValues( e ).Aggregate( service.CreateEvent( Aud.Event<TEvent>(), target ), ( b, pv ) => b.With( pv ) );
        }

        public static IQueryable<AuditEventView> RequirePermission<TDomain>( this IQueryable<AuditEventView> events,
            IEnumerable<AuditEventKind> allKinds, ISecurityService<TDomain> sec, SecurityPrivilege privilege, ISecuritySubject subject )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var evts = events.HomomorphismRestrictionWorkaround();
            //return allKinds
            //    .ToLookup( k => k.AppliesTo.ObjectKindsInOrder.Count() )
            //    .Aggregate(
            //        Enumerable.Empty<AuditEventView>(), // TODO: EF4 doesn't support UNION on complex types or UNION with subsequent operations.
            //                                            // Make this into IQueryable as soon as we move to EF4.5
            //        ( es, a ) =>
            //        {
            //            var takeNminusOne = BigTupleExpression.Take<Guid>( a.Key );
            //            var initKindField = BigTuple<Guid>.InitializerExpression( a.Key+1, a.Key-1 );
            //            var takeN = BigTupleExpression.Take<Guid>( a.Key+1 );
            //            var kinds = a.Select( e => e.Id );
            //            var res = from e in evts
            //                      where kinds.Contains( e.KindId )
            //                      join g in sec.AllGrantedTargets( subject, privilege, a.Key+1 )
            //                      on initKindField.Invoke( takeNminusOne.Invoke( e.TargetId ), e.KindId ) equals takeN.Invoke( g )
            //                      select e;

            //            return es.Concat( res.Expand() );
            //        }
            //    )
            //    .AsQueryable();
        }
    }
}