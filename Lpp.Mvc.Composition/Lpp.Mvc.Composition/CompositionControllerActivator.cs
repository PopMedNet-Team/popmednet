using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Lpp.Composition;

namespace Lpp.Mvc
{
    [Export(typeof(IControllerActivator))]
    public class CompositionControllerActivator : IControllerActivator
    {
        public IController Create( System.Web.Routing.RequestContext requestContext, Type controllerType )
        { 
            return requestContext.HttpContext.Composition().Get( controllerType ) as IController;
        }
    }
}