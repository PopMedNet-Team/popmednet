using System;
using System.Data.Entity;
using System.Linq;
//using Lpp.Data.Composition;
using Lpp.Security;
using Lpp.Utilities.Legacy;

namespace Lpp.Audit.Data
{
    //TO_REMOVE: Not used in 4.04 and will be removed in future

    //public class PersistenceDefinition<TDomain> : IPersistenceDefinition<TDomain>
    //{
    //    public void BuildModel( DbModelBuilder builder )
    //    {
    //        builder.Entity<AuditPropertyValue>()
    //            .HasRequired( p => p.Event ).WithMany( e => e.PropertyValues )
    //            .Map( m => m.MapKey( "EventId" ) );

    //        var ev = builder.Entity<AuditEvent>();
    //        BigTuple<Guid>.MemberAccess
    //            .Select( ( m, i ) => new { m, i } )
    //            .ForEach( x => ev
    //                .Property( Expr.Create( ( AuditEvent t ) => x.m.Invoke( t.TargetId ) ).Expand() )
    //                .HasColumnName( "TargetId" + (x.i+1) ) );
    //    }
    //}

    //public static class Bootstrap
    //{
    //    public static void Execute( DbContext ctx )
    //    {
    //        ctx.RunStatements( ctx.SplitStatements( Properties.Resources.DDL ) );
    //    }
    //}
}