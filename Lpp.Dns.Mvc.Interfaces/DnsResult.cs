using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reactive;
using System.Diagnostics.Contracts;
using System.IO;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns
{
    public class DnsResult
    {
        public IEnumerable<string> ErrorMessages { get; set; }
        public bool IsSuccess 
        {
            get 
            {
                return ErrorMessages == null || !ErrorMessages.Any();
            }
        }

        public static DnsResult Failed( params string[] errorMsgs ) 
        {
            return new DnsResult { ErrorMessages = errorMsgs }; 
        }

        public static readonly DnsResult Success = new DnsResult();

        public static DnsResult FromException( Exception ex )
        {
            return new DnsResult { ErrorMessages = new string[] { ex.UnwindException()} };
        }

        public static DnsResult Catch( Func<DnsResult> body )
        {
            try { 
                return body(); 
            }
            catch ( Exception ex ) 
            {
                return FromException( ex ); 
            }
        }

        public static DnsResult Catch( Action body ) 
        {
            return Catch( () => { 
                    body(); 
                    return DnsResult.Success; 
                }
            ); 
        }
    }

    public static class DnsResultExtensions
    {
        public static DnsResult Merge( this DnsResult a, DnsResult b )
        {
            return a == null ?
                b == null ? DnsResult.Success : b :
                new DnsResult
                {
                    ErrorMessages = 
                        a.ErrorMessages == null ? b.ErrorMessages :
                        b.ErrorMessages == null ? a.ErrorMessages :
                        a.ErrorMessages.Concat( b.ErrorMessages )
                };
        }

        public static DnsResult Merge( this IEnumerable<DnsResult> ress )
        {
            return ress.Aggregate( DnsResult.Success, ( a, b ) => a.Merge( b ) );
        }

        public static DnsResult Lift( this MaybeNotNull<DnsResult> maybe, DnsResult nullResult = null )
        {
            return maybe.Kind == MaybeKind.Error ? DnsResult.FromException( maybe.Exception ) : ( maybe.ValueOrNull() ?? nullResult ?? DnsResult.Failed( "Unknown error has occurred" ) );
        }
    }
}