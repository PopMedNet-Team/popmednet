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
        ILease _lease = null;

        public LifetimeSponsor(MarshalByRefObject lease)
        {
            _lease = (ILease)System.Runtime.Remoting.RemotingServices.GetLifetimeService(lease);
            if (_lease != null)
            {
                _lease.Register(this);
            }
        }

        public TimeSpan Renewal(ILease lease)
        {
            return TimeSpan.FromDays(1);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Dispose()
        {
            if (_lease != null)
            {
                _lease.Unregister(this);
            }
        }
    }
}
