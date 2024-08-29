using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Lpp.Dns
{
    public interface IDnsResponseContext
    {
        IDnsRequestContext Request { get; }
        IEnumerable<IDnsDataMartResponse> DataMartResponses { get; }

        /// <summary>
        /// This is a unique identifier of this context that may be persisted and later used to retrieve an identical context.
        /// <see cref="IDnsModelPluginHost.GetResponseContext"/>
        /// </summary>
        string Token { get; }

        /// <summary>
        /// Indicates if the view is outside of the normal site layout.
        /// </summary>
        bool IsExternalView { get; set; }
    }
}