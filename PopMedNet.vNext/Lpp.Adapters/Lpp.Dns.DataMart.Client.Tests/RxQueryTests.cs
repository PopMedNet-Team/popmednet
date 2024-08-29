using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Client.Utils;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Model;
using System.Reactive.Linq;

namespace Lpp.Dns.DataMart.Client.Tests
{
    /// <summary>
    /// Summary description for RxQueryTests
    /// </summary>
    [TestClass]
    public class RxQueryTests
    {
        readonly log4net.ILog _logger;
        readonly Lpp.Dns.DataMart.Lib.NetWorkSetting _networkSetting;
        

        public RxQueryTests()
        {
            log4net.Config.XmlConfigurator.Configure();
            _logger = log4net.LogManager.GetLogger(typeof(RxQueryTests));
            _networkSetting = Configuration.Instance.GetNetworkSetting(1);
        }

        [TestMethod]
        public async Task OriginalGetRequestsForAutoProcessor()
        {
            //If there are more than 10 pending requests within the last 30 days that are possible for autoprocessing, only the first 10 will be retrieved

            var datamartIDs = _networkSetting.DataMartList
                               .Where(dm => dm.AllowUnattendedOperation && (dm.NotifyOfNewQueries || dm.ProcessQueriesAndNotUpload || dm.ProcessQueriesAndUploadAutomatically))
                               .Select(dm => dm.DataMartId).ToArray();

            if (datamartIDs.Length == 0)
            {
                throw new NotSupportedException("No datamarts enabled for autoprocessing.");
            }

            var requestFilter = new RequestFilter
            {
                Statuses = new[] { DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted, DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted },
                DataMartIds = datamartIDs
            };

            int batchSize = 10;
            var requests = from list in DnsServiceManager.GetRequestList("AutoProcessor", _networkSetting, 0, batchSize, requestFilter, null, null)
                           from rl in list.Segment.EmptyIfNull().ToObservable()
                           where rl.AllowUnattendedProcessing
                           from r in RequestCache.ForNetwork(_networkSetting).LoadRequest(rl.ID, rl.DataMartID)
                           select r;

            var rq = await requests.ToArray();

            foreach(var r in rq)
            {
                _logger.Debug($"{ r.DataMartName }: { r.Source.Name} [ { r.Source.Identifier } ]");
            }

        }

        [TestMethod]
        public async Task GetRequestsPaged()
        {
            var requests = await GetRequestsAsync();

            _logger.Debug($"{ requests.Count() } requests retrieved.");
            foreach(var request in requests)
            {
                _logger.Debug($"{ request.DataMartName }: {request.Source.Name } [{request.Source.Identifier} / {request.Source.RequestTypeName}]");
            }

        }

        async Task<HubRequest[]> GetRequestsAsync()
        {
            var datamartIDs = _networkSetting.DataMartList
                                .Where(dm => dm.AllowUnattendedOperation && (dm.NotifyOfNewQueries || dm.ProcessQueriesAndNotUpload || dm.ProcessQueriesAndUploadAutomatically))
                                .Select(dm => dm.DataMartId).ToArray();

            if (datamartIDs.Length == 0)
            {
                return Array.Empty<HubRequest>();
            }

            var requestFilter = new RequestFilter
            {
                Statuses = new[] { Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted, Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted },
                DataMartIds = datamartIDs,
                FromDate = DateTime.UtcNow.AddYears(-1),
                DateRange = DateRangeKind.Exact
            };

            var ob = Observable.Create<DTO.DataMartClient.RequestList>(async observer =>
            {

                int index = 0;
                int batchSize = 2;
                DTO.DataMartClient.RequestList rl = null;
                while (rl == null || (index < rl.TotalCount))
                {
                    _logger.Debug($"Observer loop: pageIndex={ index }.");

                    rl = await DnsServiceManager.GetRequestList("RxQueryTests-DefaultDSM", _networkSetting, index, batchSize, requestFilter, null, null);

                    _logger.Debug($"Observer loop: pageIndex={ index }, results returned={ rl.Segment.DefaultIfEmpty().Count() }, total results={ rl.TotalCount }");

                    if (rl.TotalCount == 0)
                    {
                        break;
                    }

                    observer.OnNext(rl);

                    index += batchSize;
                }

                _logger.Debug("Observer loop firing OnComplete");
                observer.OnCompleted();
            }).DefaultIfEmpty().Aggregate((requestList1, requestList2) => {
                if(requestList1 == null && requestList2 == null)
                {
                    return new DTO.DataMartClient.RequestList
                    {
                        Segment = Array.Empty<DTO.DataMartClient.RequestListRow>(),
                        SortedAscending = false,
                        SortedByColumn = DTO.DataMartClient.RequestSortColumn.RequestTime
                    };
                }
                else if(requestList1 != null && requestList2 == null)
                {
                    return requestList1;
                }else if(requestList1 == null && requestList2 != null)
                {
                    return requestList2;
                }
                else
                {
                    return new DTO.DataMartClient.RequestList
                    {
                        Segment = requestList1.Segment.EmptyIfNull().Concat(requestList2.Segment.EmptyIfNull()).ToArray(),
                        SortedAscending = requestList1.SortedAscending,
                        SortedByColumn = requestList1.SortedByColumn,
                        StartIndex = requestList1.StartIndex,
                        TotalCount = requestList1.TotalCount
                    };
                }                
            })
           .SelectMany(requestList => {
               if(requestList == null)
               {
                   return Array.Empty<DTO.DataMartClient.RequestListRow>();
               }

               return requestList.Segment.DefaultIfEmpty().Where(s => s.AllowUnattendedProcessing);
               
               })
           .SelectMany(rlr => RequestCache.ForNetwork(_networkSetting).LoadRequest(rlr.ID, rlr.DataMartID))
           .ToArray();

            return await ob;
        }

    }
}
