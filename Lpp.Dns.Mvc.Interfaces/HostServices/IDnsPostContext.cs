using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;

namespace Lpp.Dns
{
    [ContractClass( typeof( Contracts.IDnsPostContextContract ) )]
    public interface IDnsPostContext
    {
        IValueProvider Values { get; }
        TModel GetModel<TModel>() where TModel : class, new();
    }

    namespace Contracts
    {
        [ContractClassFor( typeof( IDnsPostContext ) )]
        abstract class IDnsPostContextContract : IDnsPostContext
        {
            public IValueProvider Values
            {
                get { //Contract.Ensures( //Contract.Result<IValueProvider>() != null ); 
                    return null; 
                }
            }

            public abstract TModel GetModel<TModel>() where TModel : class, new();
        }
    }
}