using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ServiceModel.Activation;
using System.Linq;
using Lpp.Composition;
//using Lpp.Data;
//using Lpp.Dns.Model;
using Lpp.Dns.Portal;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.LocalDataMart
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LocalDataMartService : ILocalDataMartWcfService
    {
        //[Import] public IRepository<DnsDomain, RequestRouting> Routings { get; set; }
        //[Import] public IUnitOfWork<DnsDomain> UnitOfWork { get; set; }
        [Import] public IRequestService RequestService { get; set; }
        [Import] public IPluginService PluginService { get; set; }
        [ImportMany] public IEnumerable<IModelProcessor> Processors { get; set; }

        public void ProcessRequests()
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();
            //var ids = 
            //    from r in Routings.All
            //    where r.DataMart.IsLocal && 
            //            (r.RequestStatus == RoutingStatuses.Submitted || r.RequestStatus == RoutingStatuses.Resubmitted)
            //    select r.RequestId;

            //var process =
            //    from id in ids.Take( 1000 ).ToList()
            //    let ctx = RequestService.GetRequestContext( id )
            //    join proc in Processors on ctx.Model.ModelProcessorId equals proc.Id

            //    from ri in
            //        from rt in ctx.Request.Routings
            //        where rt.DataMart.IsLocal
            //        from i in rt.Instances
            //        where i.IsCurrent
            //        select i

            //    let res = proc.Iterate( ctx )
            //    where res.IsFinished

            //    let docs = res.Documents.EmptyIfNull()
            //        .Select( doc => new
            //        {
            //            source = doc,
            //            doc = new Document
            //             {
            //                 Name = doc.Name,
            //                 IsViewable = doc.IsViewable,
            //                 MimeType = doc.MimeType,
            //                 FileName = doc.FileName
            //             }
            //        } )
            //        .ToList()

            //    let _ = ri.Documents = docs.Select( d => d.doc ).ToList()
            //    let __ = ri.ResponseTime = DateTime.Now
            //    let ___ = Maybe.Do( UnitOfWork.Commit )

            //    let writeDocs = docs
            //         .Do( d =>
            //         {
            //             d.doc.Data = d.source.Data;
            //             UnitOfWork.Commit();
            //         } )
            //         .LastOrDefault()

            //    select 0;

            //process.LastOrDefault();            
        }
    }
}