using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reactive;
using System.ComponentModel.Composition;
using System.Web.Mvc;

namespace Lpp.Dns
{
    /// <summary>
    /// This interface has the same purpose as the <see cref="IDnsModelPlugin.ValidateRequest"/> method, the difference
    /// being that these standalone validators get called for requests of all types. In other words, these validators are supposed
    /// to inplement some validation logic that transcends particular request type requirements and is common for all
    /// requests.
    /// </summary>
    public interface IDnsRequestValidator
    {
        /// <summary>
        /// Verifies that the given request is formed well enough to be submitted.
        /// </summary>
        DnsResult Validate( IDnsRequestContext context );
    }
}