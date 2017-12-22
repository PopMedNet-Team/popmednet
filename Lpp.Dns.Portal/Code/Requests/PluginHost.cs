using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Linq.Expressions;
using Lpp.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.ComponentModel;
using Lpp.Dns.Portal.Models;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IDnsModelPluginHost)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    internal class PluginHost : IDnsModelPluginHost
    {
        [Import]
        public IRequestService RequestService { get; set; }

        [Import]
        public Lazy<IResponseService> ResponseService { get; set; }

        public IDnsRequestContext GetRequestContext(Guid requestId)
        {
            return RequestService.GetRequestContext(requestId);
        }

        public IDnsResponseContext GetResponseContext(Guid requestId, string contextToken)
        {
            return ResponseService.Value.GetResponseContext(RequestService.GetRequestContext(requestId), contextToken);
        }
    }
}