using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Web.Mvc;

namespace Lpp.Mvc
{
    public class FullTypeNameControllerFactory : DefaultControllerFactory
    {
        protected override Type GetControllerType( System.Web.Routing.RequestContext requestContext, string controllerName )
        {
            var type = Type.GetType( controllerName, false, true );

            if ( type != null && typeof( IController ).IsAssignableFrom( type ) ) 
                return type;
            
            return base.GetControllerType( requestContext, controllerName );
        }
    }
}