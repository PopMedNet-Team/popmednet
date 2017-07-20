using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Lpp.Utilities.Legacy
{
    public class OpResult
    {
        public IEnumerable<string> ErrorMessages { get; set; }
        public bool IsSuccess { get { return ErrorMessages == null || !ErrorMessages.Any(); } }

        public static OpResult Failed( params string[] errorMsgs ) { return new OpResult { ErrorMessages = errorMsgs }; }
        public static readonly OpResult Success = new OpResult();

        public static OpResult FromException( Exception ex )
        {
            var errors = new List<string>();
            while (ex != null)
            {
                errors.Add(ex.Message);
                ex = ex.InnerException;
            }

            return new OpResult { ErrorMessages =  errors };
        }

        public static OpResult Catch( Func<OpResult> body )
        {
            try { return body(); }
            catch ( Exception ex ) { return FromException( ex ); }
        }

        public static OpResult Catch( Action body ) { return Catch( () => { body(); return OpResult.Success; } ); }
    }

    public static class OpResultExtensions
    {
        public static OpResult Merge( this OpResult a, OpResult b )
        {
            //Contract.Ensures( //Contract.Result<OpResult>() != null );
            return a == null ?
                b == null ? OpResult.Success : b :
                new OpResult
                {
                    ErrorMessages = 
                        a.ErrorMessages == null ? b.ErrorMessages :
                        b.ErrorMessages == null ? a.ErrorMessages :
                        a.ErrorMessages.Concat( b.ErrorMessages )
                };
        }

        public static OpResult Merge( this IEnumerable<OpResult> ress )
        {
            //Contract.Requires( ress != null );
            //Contract.Ensures( //Contract.Result<OpResult>() != null );
            return ress.Aggregate( OpResult.Success, ( a, b ) => a.Merge( b ) );
        }

        public static OpResult Lift( this MaybeNotNull<OpResult> maybe, OpResult nullResult = null )
        {
            //Contract.Requires( maybe != null );
            //Contract.Ensures( //Contract.Result<OpResult>() != null );

            return
                maybe.Kind == MaybeKind.Error ? OpResult.FromException( maybe.Exception ) :
                ( maybe.ValueOrNull() ?? nullResult ?? OpResult.Failed( "Unknown error has occurred" ) );
        }
    }
}