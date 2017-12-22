using Lpp.Composition;
using Lpp.Composition.Modules;
using Lpp.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc.Composition.Application
{
    public static class MefConfig
    {
        public static CompositionScopingService RegisterMef()
        {
            CompositionContainer container;
            CompositionScopingService service;
            ConfigureContainer(out container, out service);
            ControllerBuilder.Current.SetControllerFactory(new MefControllerFactory(container));

            //var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;

            DependencyResolver.SetResolver(
                type =>
                {
                    try { return (HttpContext.Current.Composition() ?? service.RootScope).Get(type); }
                    catch (CompositionException) { return null; }
                },
                type =>
                {
                    try { return (HttpContext.Current.Composition() ?? service.RootScope).GetMany(type); }
                    catch (CompositionException) { return null; }
                });

            return service;
        }

        private static void ConfigureContainer(out CompositionContainer container, out CompositionScopingService service)
        {
            var componentsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            
            var reg = new RegistrationBuilder();
            reg

                .ForTypesDerivedFrom<IController>()
                .AddMetadata(ExportScope.Key, TransactionScope.Id)
                .Export<IController>()
                .Export();

            var rootCatalog = new DirectoryCatalog(componentsFolder, reg);
            try
            {
                var mms = rootCatalog.DiscoverModules();
                service = new CompositionScopingService(mms.Catalog, mms.ExplicitExports, new String[] { TransactionScope.Id});
                container = new CompositionContainer(mms.Catalog);
            }
            catch (ReflectionTypeLoadException rtle)
            {

                var sb = new StringBuilder();
                sb.Append(rtle.UnwindException() + "<br/><br/>");

                foreach (var e in rtle.LoaderExceptions)
                {
                    sb.Append(e.UnwindException() + "<br/><br/>");
                }

                throw new ReflectionTypeLoadException(rtle.Types, rtle.LoaderExceptions, sb.ToString());
            }
        }
    }
}
