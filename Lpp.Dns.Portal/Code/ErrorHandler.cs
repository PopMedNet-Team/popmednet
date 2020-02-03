using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Dns.Data;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Linq.Expressions;
using System.Xml.Linq;
using log4net;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IMvcFilter))]
    class ErrorHandler : IExceptionFilter, IMvcFilter
    {
        [Import]
        public ILog Log { get; set; }

        public bool AllowMultiple 
        { 
            get 
            {
                return false; 
            }
        }
        
        public int Order 
        {
            get
            {
                return 0; 
            }
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is UnauthorizedAccessException)
            {
                if (filterContext.IsChildAction) 
                    filterContext.Result = View.Result<Views.Errors.AccessDeniedEmbedded>().WithModel(filterContext.Exception.Message);
                else 
                    filterContext.Result = View.Result<Views.Errors.AccessDenied>().WithModel(filterContext.Exception.Message);

                filterContext.ExceptionHandled = true;
            }
            else
            {
                var req = filterContext.RequestContext.HttpContext.Request;
                Log.Error(string.Format("{0}URL: {1}{0}Method:{2}{0}Exception:{3}{0}",
                    Environment.NewLine, req.Url, req.HttpMethod, filterContext.Exception), filterContext.Exception);
            }
        }
    }
}