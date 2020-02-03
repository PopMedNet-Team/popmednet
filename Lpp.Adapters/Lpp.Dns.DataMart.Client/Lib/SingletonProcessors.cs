using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Model;

namespace Lpp.Dns.DataMart.Lib.RequestQueue
{
    public class SingletonProcessors
    {
        private static SingletonProcessors instance;

        // Model Id > DataMart Id > Model Processor
        private IDictionary<Guid, IDictionary<Guid, IModelProcessor>> modelDataMartProcessors = new Dictionary<Guid, IDictionary<Guid, IModelProcessor>>();

        public static SingletonProcessors Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                instance = new SingletonProcessors();
                return instance;
            }
        }

        public IDictionary<Guid, IDictionary<Guid, IModelProcessor>> ModelDataMartProcessors
        {
            get
            {
                return modelDataMartProcessors;
            }
        }

        private SingletonProcessors()
        {
        }

        public void Add(Guid modelId, Guid dataMartId, IModelProcessor processor)
        {
            IDictionary<Guid, IModelProcessor> dataMartProcessors;

            if (modelDataMartProcessors.ContainsKey(modelId))
                dataMartProcessors = modelDataMartProcessors[modelId];
            else
            {
                dataMartProcessors = new Dictionary<Guid, IModelProcessor>();
                modelDataMartProcessors.Add(modelId, dataMartProcessors);
            }

            dataMartProcessors.Add(dataMartId, processor);
        }

        public IModelProcessor GetProcessor(Guid modelId, Guid dataMartId)
        {
            if (modelDataMartProcessors.ContainsKey(modelId) &&
                modelDataMartProcessors[modelId].ContainsKey(dataMartId))
                return modelDataMartProcessors[modelId][dataMartId];

            return null;
        }
    }
}
