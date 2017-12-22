using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Permissions;
using System.Text;

namespace Lpp.Dns.DataMart.Client.DomainManger
{
    [Serializable]
    [SecurityPermission(SecurityAction.Demand, Infrastructure = true)]
    public sealed class LifetimeSponsor : MarshalByRefObject, ISponsor, IDisposable
    {
        ILease _proxy;
        bool _registered = false;

        public LifetimeSponsor(ILease proxy)
        {
            _proxy = proxy;
            _proxy.Register(this);
            _registered = true;
        }

        public TimeSpan Renewal(ILease lease)
        {
            if (_registered == false)
                return TimeSpan.Zero;

            return TimeSpan.FromDays(1);
        }

        public void Dispose()
        {
            if (_registered)
            {
                _proxy.Unregister(this);
                _registered = false;
            }
        }
    }
}
