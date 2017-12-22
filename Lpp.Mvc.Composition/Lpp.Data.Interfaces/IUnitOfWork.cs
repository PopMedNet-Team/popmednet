using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Data
{
    public interface IUnitOfWork<TDomain> : IDisposable
    {
        void Commit();
        void Rollback();
    }
}