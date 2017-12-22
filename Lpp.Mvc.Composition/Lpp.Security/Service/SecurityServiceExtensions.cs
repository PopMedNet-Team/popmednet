using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Diagnostics;
using Lpp.Utilities.Legacy;

namespace Lpp.Security
{
    public static class SecurityServiceExtensions
    {
        public static Func<SecurityPrivilege, bool> Can<TDomain>( this ISecurityService<TDomain> service, SecurityTarget t, ISecuritySubject subject )
        {

            var acl = service.GetAcl( t ).Where( e => e.SubjectId == subject.ID && e.Allow ).ToLookup( e => e.PrivilegeId );
            return p => acl[p.SID].Any();
        }

        public static void Demand( this Func<SecurityPrivilege, bool> can, SecurityPrivilege p )
        {
            if ( !can( p ) ) throw new UnauthorizedAccessException();
        }

        public static IQueryable<UnresolvedAclEntry> GetAcl<TDomain>( this ISecurityService<TDomain> service, SecurityTarget target )
        {
            var equalsTarget = 
                Predicate.And(
                    BigTuple<Guid>.MemberAccess.Zip( target.Elements,
                    ( ma, e ) => Expr.Create( ( BigTuple<Guid> o ) => ma.Invoke( o ) == e.ID ) )
                );
            var filter = Expr.Create( ( SecurityTargetAcl t ) => equalsTarget.Invoke( t.TargetId ) ).Expand();

            var acls = service.GetAllAcls( target.Elements.Count() );
            return acls.Where( filter ).SelectMany( t => t.Entries );
        }

        public static IEnumerable<UnresolvedAclEntry> SkipMembershipImplied( this IQueryable<UnresolvedAclEntry> source )
        {
            return source.Where( e => !e.ViaMembership ).AsEnumerable().Select( e => new UnresolvedAclEntry( e ) { Allow = e.ExplicitAllow } );
        }

        public static IEnumerable<AnnotatedAclEntry> ResolveAcl<TDomain>( this IEnumerable<UnresolvedAclEntry> source, 
            ISecurityService<TDomain> sec, SecurityTargetKind targetKind )
        {
            return source.Select( e => sec.ResolveAclEntry( e, targetKind ) );
        }

        public static void Demand<TDomain>( this ISecurityService<TDomain> service, ISecurityObject o, ISecuritySubject subject, params SecurityPrivilege[] ps )
        {
            service.Demand( Sec.Target( o ), subject, ps );
        }

        public static void Demand<TDomain>( this ISecurityService<TDomain> service, SecurityTarget t, ISecuritySubject subject, params SecurityPrivilege[] ps )
        {
            if ( !ps.All( service.Can( t, subject ) ) ) throw new UnauthorizedAccessException();
        }

        public static bool HasPrivilege<TDomain>(
            this ISecurityService<TDomain> service, ISecurityObject obj, ISecuritySubject subject, SecurityPrivilege privilege )
        {
            return service.HasPrivilege( Sec.Target( obj ), subject, privilege );
        }

        /// <summary>
        /// Determins if a subject has rights to a target.
        /// </summary>
        /// <typeparam name="TDomain"></typeparam>
        /// <param name="service"></param>
        /// <param name="target"></param>
        /// <param name="subject"></param>
        /// <param name="privilege">Rights of subject to specified target to determine</param>
        /// <returns></returns>
        public static bool HasPrivilege<TDomain>(
            this ISecurityService<TDomain> service, SecurityTarget target, ISecuritySubject subject, SecurityPrivilege privilege )
        {
            return service.Can( target, subject )( privilege );
        }

        public static IEnumerable<bool> HasPrivileges<TDomain>(
            this ISecurityService<TDomain> service, SecurityTarget target, ISecuritySubject subject, 
            params SecurityPrivilege[] privileges )
        {
            if ( privileges.NullOrEmpty() ) return Enumerable.Empty<bool>();
            return privileges.Select( service.Can( target, subject ) );
        }

        public static IQueryable<SecurityTargetAcl> WhereNthIs( this IQueryable<SecurityTargetAcl> source, int idx, Guid id )
        {
            var expr = BigTuple<Guid>.MemberAccess[idx];
            return source.AsExpandable().Where( t => expr.Invoke( t.TargetId ) == id );
        }
        public static IQueryable<SecurityTargetAcl> WhereFirstIs( this IQueryable<SecurityTargetAcl> source, Guid id ) { return source.Where( t => t.TargetId.X0 == id ); }
        public static IQueryable<SecurityTargetAcl> WhereSecondIs( this IQueryable<SecurityTargetAcl> source, Guid id ) { return source.Where( t => t.TargetId.X1 == id ); }
        public static IQueryable<SecurityTargetAcl> WhereThirdIs( this IQueryable<SecurityTargetAcl> source, Guid id ) { return source.Where( t => t.TargetId.X2 == id ); }

        public static IQueryable<BigTuple<Guid>> AllGrantedTargets<TDomain>(
            this ISecurityService<TDomain> service, ISecuritySubject subject, SecurityPrivilege privilege, int arity )
        {
            return service.AllGrantedTargets( subject, pid => pid == privilege.SID, arity );
        }

        public static IQueryable<BigTuple<Guid>> AllGrantedTargets<TDomain>(
            this ISecurityService<TDomain> service, ISecuritySubject subject, Guid[] privilegeIds, int arity)
        {
            return service.AllGrantedTargets(subject, pid => privilegeIds.Contains(pid), arity);
        }

        public static IQueryable<BigTuple<Guid>> AllGrantedTargets<TDomain>(
            this ISecurityService<TDomain> service, ISecuritySubject subject, Expression<Func<Guid,bool>> privilegeFilter, SecurityTargetKind kind )
        {
            return service.AllGrantedTargets( subject, privilegeFilter, kind.ObjectKindsInOrder.Count() );
        }

        public static IQueryable<BigTuple<Guid>> AllGrantedTargets<TDomain>(
            this ISecurityService<TDomain> service, ISecuritySubject subject, SecurityPrivilege privilege, SecurityTargetKind kind )
        {
            return service.AllGrantedTargets( subject, privilege, kind.ObjectKindsInOrder.Count() );
        }

        public static IQueryable<TObject> AllGrantedObjects<TDomain,TObject>( 
            this ISecurityService<TDomain> service, IQueryable<TObject> source, ISecuritySubject subject, SecurityPrivilege privilege )
            where TObject: class, ISecurityObject
        {
            return (from t in service.AllGrantedTargets( subject, privilege, 1 )
                join o in source on t.X0 equals o.ID
                select o);
        }

        public static IQueryable<StaticQueryTuple<TObject1,TObject2>> AllGrantedPairs<TDomain, TObject1, TObject2>(
            this ISecurityService<TDomain> service, IQueryable<TObject1> source1, IQueryable<TObject2> source2, 
            ISecuritySubject subject, SecurityPrivilege privilege )
            where TObject1 : class, ISecurityObject
            where TObject2 : class, ISecurityObject
        {
            return
                (from t in service.AllGrantedTargets( subject, privilege, 2 )
                join o1 in source1 on t.X0 equals o1.ID
                join o2 in source2 on t.X1 equals o2.ID
                select new StaticQueryTuple<TObject1, TObject2> { Object1 = o1, Object2 = o2 });
        }

        public static IQueryable<StaticQueryTuple<TObject1, TObject2, TObject3>> AllGrantedTriples<TDomain, TObject1, TObject2, TObject3>(
            this ISecurityService<TDomain> service, IQueryable<TObject1> source1, IQueryable<TObject2> source2, IQueryable<TObject3> source3,
            ISecuritySubject subject, SecurityPrivilege privilege )
            where TObject1 : class, ISecurityObject
            where TObject2 : class, ISecurityObject
            where TObject3 : class, ISecurityObject
        {

            return
                (from t in service.AllGrantedTargets( subject, privilege, 3 )
                join o1 in source1 on t.X0 equals o1.ID
                join o2 in source2 on t.X1 equals o2.ID
                join o3 in source3 on t.X2 equals o3.ID
                select new StaticQueryTuple<TObject1, TObject2, TObject3> { Object1 = o1, Object2 = o2, Object3 = o3 });
        }
   }

    public class StaticQueryTuple<T1, T2>
    {
        public T1 Object1 { get; set; }
        public T2 Object2 { get; set; }
    }

    public class StaticQueryTuple<T1, T2, T3>
    {
        public T1 Object1 { get; set; }
        public T2 Object2 { get; set; }
        public T3 Object3 { get; set; }
    }
}