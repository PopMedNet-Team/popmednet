using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Data
{
    public interface IPersistenceMagic<TDomain>
    {
        T AttachEntity<T>( T entity ) where T : class;
        void SetModified<T>( T entity ) where T : class;
    }
}