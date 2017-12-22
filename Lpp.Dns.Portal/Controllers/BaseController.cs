using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Controllers
{
    /// <summary>
    /// This is a common base for all our controllers
    /// </summary>
    public abstract class BaseController : Lpp.Mvc.BaseController
    {
        //This is an example of using MEF to get a shared datacontext, not currently used, may move to this model in future.
        //[Import( RequiredCreationPolicy = CreationPolicy.Shared, AllowRecomposition = false, AllowDefault = true)]
        //public DataContext DataContext21
        //{
        //    get;
        //    set;
        //} 

        protected DataContext DataContext
        {
            get
            {
                return HttpContext.Items["DataContext"] as DataContext;
            }
        }
    }
}