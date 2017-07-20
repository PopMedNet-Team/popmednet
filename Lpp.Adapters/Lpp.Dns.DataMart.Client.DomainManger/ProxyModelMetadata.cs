using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DataMart.Model.Settings;

namespace Lpp.Dns.DataMart.Client.DomainManger
{
    [Serializable]
    public class ProxyModelMetadata : MarshalByRefObject, Lpp.Dns.DataMart.Model.IModelMetadata
    {
        readonly Lpp.Dns.DataMart.Model.IModelMetadata metadata;

        public ProxyModelMetadata() {
            metadata = new EmptyModelMetaData();
        }

        public ProxyModelMetadata(Model.IModelMetadata data)
        {
            metadata = data;
        }

        public string ModelName
        {
            get 
            { 
                return metadata.ModelName;  
            }
            set { }
        }

        public Guid ModelId
        {
            get
            {
                return metadata.ModelId;
            }
            set { }
        }

        public string Version
        {
            get
            {
                return metadata.Version;
            }
            set { }
        }

        public IDictionary<string, bool> Capabilities
        {
            get
            {
                return metadata.Capabilities;
            }
            set
            { }
        }

        public IDictionary<string, string> Properties
        {
            get
            {
                return metadata.Properties;
            }
            set { }
        }

        public ICollection<Model.Settings.ProcessorSetting> Settings
        {
            get
            {
                return metadata.Settings;
            }
            set { }
        }

        public IEnumerable<Model.Settings.SQLProvider> SQlProviders
        {
            get { return metadata.SQlProviders; }
            set { }
        }

        public sealed class EmptyModelMetaData : Lpp.Dns.DataMart.Model.IModelMetadata
        {
            public string ModelName
            {
                get { return "Empty Model"; }
            }

            public Guid ModelId
            {
                get { return Guid.Empty; }
            }

            public string Version
            {
                get { return "1.0.0.0"; }
            }

            public IDictionary<string, bool> Capabilities
            {
                get { return new Dictionary<string,bool>(); }
            }

            public IDictionary<string, string> Properties
            {
                get { return new Dictionary<string, string>(); }
            }

            public ICollection<Model.Settings.ProcessorSetting> Settings
            {
                get { return new List<Model.Settings.ProcessorSetting>(); }
            }

            public IEnumerable<Model.Settings.SQLProvider> SQlProviders
            {
                get { return new Model.Settings.SQLProvider[0]; }
            }
        }
        
    }
}
