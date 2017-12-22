using System;
using System.ComponentModel.Composition;
using System.Linq;
using Lpp.Composition;
using Lpp.Utilities;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IDnsRequestValidator)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    internal class TwoDataMartsRuleValidator : IDnsRequestValidator
    {
        [Import]
        public IAuthenticationService Auth { get; set; }

        public DnsResult Validate(IDnsRequestContext context)
        {
            if (context.RequestType.IsMetadataRequest)
                return DnsResult.Success;

            var datacontext = System.Web.HttpContext.Current.DataContext();

            if(AsyncHelpers.RunSync<bool>(() => datacontext.HasPermission(Auth.ApiIdentity, Lpp.Dns.DTO.Security.PermissionIdentifiers.Portal.SkipTwoDataMartRule)))
                return DnsResult.Success;                

            var success = (from r in datacontext.Requests
                           let totalCount = r.DataMarts.Where(d => d.Status != DTO.Enums.RoutingStatus.Canceled).Count()
                           let ownCount = r.DataMarts.Where(d => d.Status != DTO.Enums.RoutingStatus.Canceled).Count(d => d.DataMart.OrganizationID == r.UpdatedBy.OrganizationID)
                           let otherCount = r.DataMarts.Where(d => d.Status != DTO.Enums.RoutingStatus.Canceled).Count(d => d.DataMart.OrganizationID != r.UpdatedBy.OrganizationID)
                           where r.ID == context.RequestID
                           select totalCount >= 2 && otherCount >= 1).First();

            if (success)
                return DnsResult.Success;

            return DnsResult.Failed("The system requires that you submit your query to 2 or more DataMarts from different organizations. Please select at least 1 DataMart from 2 separate organizations.");
        }
    }
}