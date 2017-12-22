using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Lpp.Composition;
using System.Web.Routing;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ComponentModel.Composition.Hosting;
using System.Web;
using System.ServiceModel.Channels;
using System.ComponentModel;

namespace Lpp.Mvc
{
    public static class WcfExtensions
    {
        public static FaultException<T> AsWcfFault<T>( this T detail )
        {
            return new FaultException<T>( detail, new FaultReason( Convert.ToString( detail ) ) );
        }
    }
}