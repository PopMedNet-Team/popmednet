using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reactive;
using System.Diagnostics.Contracts;
using System.IO;
using Lpp.Dns.Data;

namespace Lpp.Dns
{
    [ContractClass( typeof( Contracts.IDnsDataMartResponseContract ) )]
    public interface IDnsDataMartResponse
    {
        IDnsDataMart DataMart { get; }
        IEnumerable<Document> Documents { get; }
    }

    namespace Contracts
    {
        [ContractClassFor( typeof( IDnsDataMartResponse ) )]
        abstract class IDnsDataMartResponseContract : IDnsDataMartResponse
        {
            public IDnsDataMart DataMart
            {
                get { //Contract.Ensures( //Contract.Result<IDnsDataMart>() != null ); 
                    throw new NotImplementedException(); }
            }

            public IEnumerable<Document> Documents
            {
                get { //Contract.Ensures( //Contract.Result<IEnumerable<IDnsPersistentDocument>>() != null ); 
                    throw new NotImplementedException(); }
            }
        }
    }
}