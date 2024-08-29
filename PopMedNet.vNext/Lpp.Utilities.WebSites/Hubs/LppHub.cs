using Lpp.Objects;
using Lpp.Security;
using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Lpp.Utilities.WebSites.Hubs
{
    public class LppHub<TCrudOperationDTO, TEntity> : Hub, IHub
        where TCrudOperationDTO : class, new()
        where TEntity : EntityWithID
    {
        [BroadcastEndPoint]
        private TCrudOperationDTO NotifyCrud() {
            throw new NotImplementedException();
        }
    }
}
