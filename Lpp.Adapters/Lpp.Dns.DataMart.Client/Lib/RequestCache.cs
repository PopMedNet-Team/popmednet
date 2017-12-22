using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Linq;
using Lpp.Dns.DataMart.Lib.Classes;

namespace Lpp.Dns.DataMart.Lib
{
    public sealed class RequestCache
    {
        private static readonly ConcurrentDictionary<int, RequestCache> _perNetwork = new ConcurrentDictionary<int, RequestCache>();
        
        public static RequestCache ForNetwork( NetWorkSetting ns )
        {
            return _perNetwork.GetOrAdd( ns.NetworkId, _ => new RequestCache( ns ) );
        }

        private readonly Hashtable _requestCache = new Hashtable();
        private readonly object _cacheLock = new object();

        private readonly List<HubRequest> _keepers = new List<HubRequest>();
        private readonly object _keepersLock = new object();

        public NetWorkSetting Network { get; private set; }

        public RequestCache( NetWorkSetting ns )
        {
            Network = ns;
        }

        public void Invalidate()
        {
            lock (_cacheLock)
            {
                RequestCache requestCache;
                _perNetwork.TryRemove(Network.NetworkId, out requestCache);
            }
        }
        public IObservable<HubRequest> LoadRequest( Guid id, Guid dataMartId )
        {
            return LoadRequestImpl( id, dataMartId )
                .Select( req =>
                {
                    lock ( _cacheLock )
                    {
                        var existing = CacheHit( id, dataMartId );
                        if ( existing == null ) _requestCache[HashKey( id, dataMartId )] = new WeakReference( existing = req );
                        else existing.Assign( req );

                        return existing;
                    }
                } );
        }

        private int HashKey( Guid id, Guid dataMartId )
        {
            return (id.GetHashCode() << 8) | dataMartId.GetHashCode();
        }

        public HubRequest CacheHit( Guid id, Guid dataMartId )
        {
            lock ( _cacheLock )
            {
                var existing = from rf in Maybe.Value( _requestCache[HashKey( id, dataMartId )] as WeakReference )
                               where rf.IsAlive
                               from req in rf.Target as HubRequest
                               select req;
                return existing.ValueOrNull();
            }
        }

        public void Lock( HubRequest req )
        {
            lock ( _keepersLock )
            {
                _keepers.Add( req );
            }
        }

        public void Release( HubRequest req )
        {
            lock ( _keepersLock )
            {
                _keepers.RemoveAll( r => r.DataMartId == req.DataMartId && r.Source.ID == req.Source.ID );
            }
        }

        IObservable<HubRequest> LoadRequestImpl(Guid id, Guid dataMartId)
        {
            var dm = Network.DataMartList.FirstOrDefault(d => d.DataMartId == dataMartId);
            return from r in DnsServiceManager.GetRequests(Network, new[] { id }, dataMartId).SkipWhile(r => r.ID != id).Take(1)

                   let rt = r.Routings.EmptyIfNull().FirstOrDefault(x => x.DataMartID == dataMartId)
                   where rt != null

                   select new HubRequest
                   {
                       Source = r,
                       DataMartId = dataMartId,
                       NetworkId = Network.NetworkId,
                       NetworkName = Network.NetworkName,
                       DataMartName = dm == null ? null : dm.DataMartName,
                       DataMartOrgId = dm == null ? null : dm.OrganizationId,
                       DataMartOrgName = dm == null ? null : dm.OrganizationName,
                       ProjectName = r != null && r.Project != null ? r.Project.Name : null,

                       Properties = rt.Properties.EmptyIfNull().GroupBy(p => p.Name).ToDictionary(pp => pp.Key, pp => pp.First().Value),
                       Rights = r.Routings.Where(rn => rn.DataMartID == dataMartId).Select(rn => (HubRequestRights)rn.Rights).FirstOrDefault(),
                       RoutingStatus = rt.Status,
                       SubmittedDataMarts = string.Join(", ", from rtng in r.Routings
                                                              join dmt in Network.DataMartList on rtng.DataMartID equals dmt.DataMartId
                                                              select dmt.DataMartName),
                       Documents = r.Documents.EmptyIfNull().ToArray()
                   };
        }
    }
}