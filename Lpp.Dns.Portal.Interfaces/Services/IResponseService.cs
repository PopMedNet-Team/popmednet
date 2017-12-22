using System;
using System.Collections.Generic;
using System.Linq;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    public interface IResponseService
    {
        IDnsResponseContext GetResponseContext( IDnsRequestContext reqCtx, string token );
        IDnsResponseContext GetResponseContext( IDnsRequestContext reqCtx, IEnumerable<VirtualResponse> virtualResponses );
        IDnsResponseContext GetResponseHistoryContext(IRequestContext reqCtx, IEnumerable<Response> instances);
        DnsResult GroupResponses(string groupName, IEnumerable<Response> responses);
        DnsResult UngroupResponses( ResponseGroup group );
        DnsResult ApproveResponses(IEnumerable<Response> responses);
        DnsResult RejectResponses(IEnumerable<Response> responses, string message);
        DnsResult ResubmitResponses(IRequestContext ctx, IEnumerable<Response> responses, string message);
        IQueryable<VirtualResponse> GetVirtualResponses( Guid requestId, bool allowMetadataRequest = false );
        IEnumerable<VirtualResponse> GetVirtualResponses( Guid requestId, string commaSeparatedVirtualResponseIds );
        IQueryable<VirtualResponse> GetMostRecentMetadataResponses(Guid dataMartId, Guid requestTypeId);
    }

    public class VirtualResponse
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime? ResponseTime { get; set; }
        public IEnumerable<string> Messages { get; set; }

        public Response SingleResponse { get; set; }
        public ResponseGroup Group { get; set; }

        public bool NeedsApproval { get; set; }
        public bool IsRejectedBeforeUpload { get; set; }
        public bool IsRejectedAfterUpload { get; set; }

        public bool IsResultsModified { get; set; }
        public bool CanView { get; set; }
        public bool CanGroup { get; set; }
        public bool CanApprove { get; set; }
    }
}