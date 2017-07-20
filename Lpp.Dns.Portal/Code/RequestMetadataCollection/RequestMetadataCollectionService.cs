using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using Lpp.Audit;
using System.ServiceModel.Activation;
using log4net;
using System.Collections.Generic;
using System;

namespace Lpp.Dns.Portal
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    class RequestMetadataCollectionService : IRequestMetadataCollectionWcfService
    {
        [ImportMany]
        public IEnumerable<IRequestMetadataCollectionProvider> Providers { get; set; }
        [Import]
        public ILog Log { get; set; }

        public void RequestMetadata()
        {
            foreach (var p in Providers)
            {
                try
                {
                    Log.Info("Executing RequestMetadata Job");
                    if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["RunRequestMetadataJob"]))
                        p.Export();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
    }
}