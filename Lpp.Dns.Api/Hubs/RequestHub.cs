using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.WebSites.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Api.Hubs
{
    /// <summary>
    /// SignalR notifications for Requests
    /// </summary>
    public class RequestsHub : LppHub<NotificationCrudDTO, Request>
    {
        [BroadcastEndPoint]
        private string ResultsReceived()
        {
            throw new NotImplementedException();
        }
    }
}