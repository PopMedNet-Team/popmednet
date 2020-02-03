using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
//using Lpp.Data.Composition;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Lpp.Utilities.Legacy;

namespace Lpp.Security.Data
{
    //TO_REMOVE: Not used in 4.04 and will be removed in future

    //class SecurityPersistenceDefinition<TDomain> : IPersistenceDefinition<TDomain>
    //{
    //    public void BuildModel( System.Data.Entity.DbModelBuilder builder )
    //    {
    //        builder.Entity<SecurityTarget>();

    //        builder.Entity<AclEntry>().HasRequired( e => e.Target ).WithMany( t => t.AclEntries )
    //            .HasForeignKey( e => e.TargetId ).WillCascadeOnDelete();

    //        builder.ComplexType<SecurityTargetId>();
    //        var st = builder.Entity<SecurityTarget>();
    //        BigTuple<Guid>.MemberAccess
    //            .Select( ( m, i ) => new { m, i } )
    //            .ForEach( x => st
    //                .Property( Expr.Create( (SecurityTarget t) => x.m.Invoke( t.ObjectIds ) ).Expand() )
    //                .HasColumnName( "ObjectId" + (x.i+1) ) );

    //        builder.Entity<MembershipEdge>().HasKey( m => new { m.Start, m.End } ).ToTable( "SecurityMembership" );
    //        builder.Entity<MembershipClosureEdge>().HasKey( m => new { m.Start, m.End } ).ToTable( "SecurityMembershipClosure" );
    //        builder.Entity<InheritanceEdge>().HasKey( m => new { m.Start, m.End } ).ToTable( "SecurityInheritance" );
    //        builder.Entity<InheritanceClosureEdge>().HasKey( m => new { m.Start, m.End } ).ToTable( "SecurityInheritanceClosure" );
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