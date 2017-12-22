using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reactive;
using System.Diagnostics.Contracts;
using System.IO;

namespace Lpp.Dns
{
    public interface IDnsModelPluginHost
    {
        IDnsRequestContext GetRequestContext( Guid requestID );
        IDnsResponseContext GetResponseContext( Guid requestID, string contextToken );
    }
}