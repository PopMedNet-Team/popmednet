using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Xunit;
using System.Web;
using Lpp.Data;
using System.Data.Entity;

namespace Lpp.Security.Tests
{
    public class DatabaseFixture : Lpp.Tests.DatabaseFixture
    {
        protected override IEnumerable<string> DatabaseCreateScript()
        {
            return 
                Statements( Properties.Resources.DAG ).Concat(
                Statements( Properties.Resources.CreateDatabase ) ).Concat( 
                Statements( Properties.Resources.TupleViews ) )
                .Where( s => 0 > s.IndexOf( "USE [sec]", StringComparison.InvariantCultureIgnoreCase ) );
        }
    }
}