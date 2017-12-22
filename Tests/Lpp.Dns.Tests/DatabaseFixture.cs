using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

namespace Lpp.Dns.Tests
{
    //public sealed class DatabaseFixture : Lpp.Tests.DatabaseFixture
    //{
    //    protected override IEnumerable<string> DatabaseCreateScript()
    //    {
    //        return
    //            new[]
    //            {
    //                Statements( Properties.Resources.DNS3_a_BaseStructure ),
    //                Statements( Properties.Resources.DNS4_a_Diff ),
    //                Statements( Properties.Resources.AuditDDL ),
    //                Statements( Properties.Resources.SecurityDAGs ),
    //                Statements( Properties.Resources.SecurityDDL ),
    //                Statements( Properties.Resources.SecurityTupleViews ),
    //                GetMigrations()
    //            }
    //            .Concat()
    //            .Where( s => s.IndexOf( "use [DNS3]", StringComparison.OrdinalIgnoreCase ) < 0 )
    //            .Concat( Statements( Properties.Resources.TestData ) );
    //    }

    //    private IEnumerable<string> GetMigrations()
    //    {
    //        return from resName in typeof( Lpp.Dns.Model.Request ).Assembly.GetManifestResourceNames()
    //               where resName.Contains( ".Migrations." )
    //               orderby resName
    //               from s in Statements( ReadResource( resName ) )
    //               select s;
    //    }

    //    private string ReadResource( string name )
    //    {
    //        using ( var s = typeof( Lpp.Dns.Model.Request ).Assembly.GetManifestResourceStream( name ) )
    //        {
    //            return new StreamReader( s ).ReadToEnd();
    //        }
    //    }
    //}
}