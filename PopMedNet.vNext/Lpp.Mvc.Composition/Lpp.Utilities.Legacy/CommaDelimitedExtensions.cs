using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Lpp.Utilities.Legacy
{
    public static class CommaDelimitedExtensions
    {
        public static IEnumerable<int> CommaDelimitedInts( this string s )
        {
            return from si in (s??"").Split( ',' )
                   let sit = si.Trim()
                   where sit.Length > 0 && sit.All( char.IsDigit )
                   select int.Parse( sit );
        }

        public static IEnumerable<Guid> CommaDelimitedGuids( this string s )
        {
            return
                (s??"").Split( ',' )
                .Select( si =>
                {
                    Guid r;
                    return Guid.TryParse( si.Trim(), out r ) ? (Guid?)r : null;
                } )
                .Where( g => g != null )
                .Select( g => g.Value );
        }
    }
}