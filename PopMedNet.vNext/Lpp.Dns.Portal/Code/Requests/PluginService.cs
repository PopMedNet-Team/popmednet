using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Linq.Expressions;
using Lpp.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.ComponentModel;
using Lpp.Dns.Portal.Models;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IPluginService)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    internal class PluginService : IPluginService
    {
        [ImportMany]
        public IEnumerable<IDnsModelPlugin> Plugins { get; set; }
        readonly Lazy<IDictionary<Guid, PluginRequestType>> _reqTypes;

        public PluginService()
        {
            _reqTypes = new Lazy<IDictionary<Guid, PluginRequestType>>(() => GetPluginRequestTypesImpl().ToDictionary(r => r.RequestType.ID));
        }

        public IEnumerable<IDnsModelPlugin> GetAllPlugins() 
        { 
            return Plugins; 
        }

        public IEnumerable<IDnsModel> GetAllModels() 
        { 
            return Plugins.SelectMany(p => p.Models); 
        }

        public IDictionary<Guid, PluginRequestType> GetPluginRequestTypes() 
        { 
            return _reqTypes.Value; 
        }
        
        public PluginRequestType GetPluginRequestType(Guid id) 
        { 
            return _reqTypes.Value.ValueOrDefault(id); 
        }

        IEnumerable<PluginRequestType> GetPluginRequestTypesImpl()
        {
            return from p in Plugins
                   from m in p.Models
                   from r in m.Requests
                   select new PluginRequestType { RequestType = r, Model = m, Plugin = p };
        }

        static readonly PluginRequestType _missing = new PluginRequestType { Plugin = Portal.MissingPluginStub.Instance, Model = Portal.MissingPluginStub.Model, RequestType = Portal.MissingPluginStub.Request };
        
        public PluginRequestType MissingPluginStub 
        { 
            get 
            { 
                return _missing; 
            } 
        }
    }
}