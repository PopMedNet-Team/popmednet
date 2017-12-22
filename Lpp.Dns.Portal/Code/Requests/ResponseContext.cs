using Lpp.Dns.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal
{
    internal class ResponseContext : IDnsResponseContext
    {
        private readonly IEnumerable<IDnsDataMartResponse> _dataMartResponses;
        private readonly string _token;

        public ResponseContext(IDnsRequestContext reqCtx, IEnumerable<VirtualResponse> virtualResponses, IDocumentService documentService)
        {
            Request = reqCtx;
            _token = string.Join(",", virtualResponses.Select(r => r.ID));
            IsExternalView = false;

            List<Document> documents;
            using (var db = new DataContext())
            {
                var respIDs = virtualResponses.Where(r => r.SingleResponse != null).Select(r => r.SingleResponse.ID);
                var responseIDs = respIDs.Union(virtualResponses.Where(r => r.Group != null).SelectMany(r => r.Group.Responses.Select(rr => rr.ID))).ToArray();
                documents = db.Documents.Where(d => responseIDs.Contains(d.ItemID)).ToList();

                var datamartDetails = (from rsp in db.Responses
                                      where responseIDs.Contains(rsp.ID)
                                      select new {
                                          DataMartID = rsp.RequestDataMart.DataMartID,
                                          DataMartName = rsp.RequestDataMart.DataMart.Name,
                                          OrganizationName = rsp.RequestDataMart.DataMart.Organization.Name
                                      }).Distinct().ToArray();

            _dataMartResponses = (
                    from vr in virtualResponses
                    from r in (vr.Group == null ? new[] { vr.SingleResponse } : vr.Group.Responses)
                    select new DataMartResponse(() =>
                        reqCtx.DataMarts.FirstOrDefault(d => d.ID == r.RequestDataMart.DataMartID) ??
                        datamartDetails.Where(dd => dd.DataMartID == r.RequestDataMart.DataMartID).Select( dd =>
                            new RequestContext.DnsDataMart
                            {
                                ID = dd.DataMartID,
                                Name = dd.DataMartName,
                                Organization = dd.OrganizationName
                            }                            
                        ).FirstOrDefault()
                    )
                    {
                        Documents = documents.Where(d => (vr.SingleResponse != null && d.ItemID == vr.SingleResponse.ID) || (vr.Group != null && d.ItemID == r.ID)).ToList()
                    }
                ).ToList();
            }
        }

        public IDnsRequestContext Request { get; private set; }

        public IEnumerable<IDnsDataMartResponse> DataMartResponses
        {
            get
            {
                return _dataMartResponses;
            }
        }

        public string Token
        {
            get
            {
                return _token;
            }
        }

        /// <summary>
        /// Indicates if the view is outside of the normal site layout.
        /// </summary>
        /// <remarks>
        /// This property is mutable because by default if will be false, but could be set to true prior to reaching the view.
        /// </remarks>
        public bool IsExternalView { get; set; }
    }
}