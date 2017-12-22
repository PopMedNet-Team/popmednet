using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Lpp.Dns.Data;
using Lpp.Dns.Portal.Models;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    public interface IRequestContext : IDnsRequestContext
    {
        Request Request { get; }
        IDnsModelPlugin Plugin { get; }
        new IQueryable<DataMart> DataMarts { get; }
        void Reset();
        void Close();
    }
}