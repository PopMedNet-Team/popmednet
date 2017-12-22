using System.Linq;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Lpp.Data.Composition
{
    public static class DataExtensions
    {
        public static int ExecuteSqlCommand( this DbContext ctx, string cmd, params object[] parameters )
        {
            //Contract.Requires( ctx != null );
            //Contract.Requires( !string.IsNullOrEmpty( cmd ) );
            return ctx.Database.ExecuteSqlCommand( cmd, parameters );
        }

        static readonly Regex _goRegex = new Regex( @"^\s*GO\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline );
        public static IEnumerable<string> SplitStatements( this DbContext ctx, string statements )
        {
            //Contract.Ensures( //Contract.Result<IEnumerable<string>>() != null );
            return _goRegex.Split( statements ?? "" ).Select( s => s.Trim() );
        }

        public static void RunStatements( this DbContext ctx, IEnumerable<string> statements )
        {
            //Contract.Requires( ctx != null );
            //Contract.Requires( statements != null );
            foreach ( var s in statements ) ctx.ExecuteSqlCommand( s );
        }
    }
}