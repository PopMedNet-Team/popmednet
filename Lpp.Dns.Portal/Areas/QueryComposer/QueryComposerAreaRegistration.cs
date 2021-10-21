using Lpp.Dns.Portal.Views.Request;
using Lpp.QueryComposer;
using System.Linq;
using System;
using System.Web.Mvc;
using System.Reflection;
using System.Collections.Generic;

namespace Lpp.Dns.Portal.Areas.QueryComposer
{
    public class QueryComposerAreaRegistration : AreaRegistration 
    {
        public static List<IVisualTerm> Terms = new List<IVisualTerm>();

        public override string AreaName 
        {
            get 
            {
                return "QueryComposer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //Register all term types here
            RegisterTerms();

            context.MapRoute(
                "QueryComposer_default",
                "QueryComposer/{action}",
                new { controller = "QueryComposer", action = "Edit" }
            );
        }

        private void RegisterTerms()
        {
            var assemblies = Lpp.Utilities.ObjectEx.GetNonSystemAssemblies();

            Terms.AddRange(assemblies.SelectMany(a => a.GetTypes().Where(type => typeof(IVisualTerm).IsAssignableFrom(type) && type != typeof(IVisualTerm)).Select(t => (IVisualTerm)Activator.CreateInstance(t))));
        }
    }
}