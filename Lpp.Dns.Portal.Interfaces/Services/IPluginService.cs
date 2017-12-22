using System;
using System.Collections.Generic;

namespace Lpp.Dns.Portal
{
    public interface IPluginService
    {
        IEnumerable<IDnsModelPlugin> GetAllPlugins();
        IEnumerable<IDnsModel> GetAllModels();

        IDictionary<Guid, PluginRequestType> GetPluginRequestTypes();
        
        PluginRequestType MissingPluginStub { get; }
        PluginRequestType GetPluginRequestType( Guid id );
    }

    public class PluginRequestType
    {
        public IDnsRequestType RequestType { get; set; }
        public IDnsModel Model { get; set; }
        public IDnsModelPlugin Plugin { get; set; }
    }
}