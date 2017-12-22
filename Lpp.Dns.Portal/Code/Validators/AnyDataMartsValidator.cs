using System.ComponentModel.Composition;
using System.Linq;
using Lpp.Composition;
//using Lpp.Data;
//using Lpp.Dns.Model;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    [Export( typeof( IDnsRequestValidator ) ), PartMetadata( ExportScope.Key, TransactionScope.Id )]
    internal class AnyDataMartsValidator : IDnsRequestValidator
    {
        public DnsResult Validate( IDnsRequestContext context )
        {
            if (System.Web.HttpContext.Current.DataContext().RequestDataMarts.Any(dm => dm.RequestID == context.RequestID && dm.Status != DTO.Enums.RoutingStatus.Canceled))
                return DnsResult.Success;

            return DnsResult.Failed( "Please select at least one DataMart" );
        }
    }
}