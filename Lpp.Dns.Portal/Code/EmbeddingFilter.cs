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
using System.Text.RegularExpressions;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IMvcFilter))]
    class EmbeddingFilter : IActionFilter, IMvcFilter
    {
        public const string EmbeddedParam = "__embedded";

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

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            FixUpRedirect(filterContext, values =>
            {
                if (filterContext.HttpContext.IsEmbeddedRequest())
                    values[EmbeddedParam] = "1";
                else
                    values.Remove(EmbeddedParam);
            });
        }

        void FixUpRedirect(ActionExecutedContext filterContext, Action<IDictionary<string, object>> a)
        {
            var rd = filterContext.Result as RedirectResult;
            var rt = filterContext.Result as RedirectToRouteResult;
            if (rd == null && rt == null) return;

            if (rd != null)
            {
                var parts = rd.Url.Split(new[] { '?' }, 2);
                var values = parts.Skip(1)
                    .SelectMany(s => s.Split('&'))
                    .Select(kv => kv.Split(new[] { '=' }, 2))
                    .Where(kv => kv != null && kv.Length == 2 && !kv[0].NullOrEmpty())
                    .ToDictionary(kv => kv[0], kv => (object)HttpUtility.UrlDecode(kv[1]));
                a(values);

                var qs = string.Join("&", values.Select(kv => kv.Key + "=" + HttpUtility.UrlEncode(kv.Value as string ?? "")));
                filterContext.Result = new RedirectResult(parts.FirstOrDefault() + (qs.NullOrEmpty() ? "" : "?" + qs), rd.Permanent);
            }
            else if (rt != null && (rt.RouteValues.ValueOrDefault(EmbeddedParam) as string).NullOrEmpty())
            {
                a(rt.RouteValues);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetAttributes<NoAjaxNavigationAttribute>().Any())
            {
                filterContext.HttpContext.NoAjaxNavigation(true);
            }
        }
    }

    public static class EmbeddingExtensions
    {
        public static bool IsEmbeddedRequest(this HttpContextBase ctx)
        {
            return !ctx.Request.QueryString[EmbeddingFilter.EmbeddedParam].NullOrEmpty();
        }

        static readonly object _noAjaxNavigationKey = new object();
        public static bool NoAjaxNavigation(this HttpContextBase ctx)
        {
            return (ctx.Items[_noAjaxNavigationKey] as bool?) ?? false;
        }
        public static void NoAjaxNavigation(this HttpContextBase ctx, bool v)
        {
            ctx.Items[_noAjaxNavigationKey] = v;
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class NoAjaxNavigationAttribute : Attribute
    {
    }
}