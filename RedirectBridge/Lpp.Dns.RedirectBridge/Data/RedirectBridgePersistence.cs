using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Lpp.Dns.RedirectBridge.PersistenceDefinitions
{
    //[Export(typeof(IPersistenceDefinition<RedirectDomain>))]
    //[Export(typeof(IDatabaseBootstrapSegment))]
    //public class RedirectBridgePersistence : IPersistenceDefinition<RedirectDomain>, IDatabaseBootstrapSegment
    //{
    //    public void BuildModel( System.Data.Entity.DbModelBuilder builder )
    //    {
    //        builder.Entity<RequestType>().HasRequired( r => r.Model ).WithMany( m => m.RequestTypes ).Map( m => m.MapKey( "ModelId" ) );

    //        builder.Entity<Model>();
    //        builder.Entity<PluginSession>();

    //        builder.Entity<PluginSessionDocument>()
    //            .HasRequired( d => d.Session )
    //            .WithMany( s => s.Documents )
    //            .Map( m => m.MapKey( "SessionId" ) )
    //            .WillCascadeOnDelete();
    //    }

    //    public void Execute( System.Data.Entity.DbContext ctx )
    //    {
    //        ctx.RunStatements( ctx.SplitStatements( Properties.Resources.DDL ) );
    //    }

    //    public Type Domain { get { return typeof( RedirectDomain ); } }
    //}
}