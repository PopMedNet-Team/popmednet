using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DataMart.Model;

namespace Lpp.Dns.DataMart.Client.DomainManger
{
    public class ProxyModelProcessorWithPatientIdentifierCapabilities : ProxyModelProcessorWithEarlyInitialize, IPatientIdentifierProcessor
    {
        IPatientIdentifierProcessor _patientProcessor;

        public ProxyModelProcessorWithPatientIdentifierCapabilities() { }

        public ProxyModelProcessorWithPatientIdentifierCapabilities(IPatientIdentifierProcessor processor) : base((IEarlyInitializeModelProcessor)processor)
        {
            _patientProcessor = processor;
        }

        bool IPatientIdentifierProcessor.CanGenerateLists => _patientProcessor.CanGenerateLists;

        void IPatientIdentifierProcessor.GenerateLists(Guid requestID, NetworkConnectionMetadata network, RequestMetadata md, IDictionary<Guid, string> outputPaths, string format)
        {
            _patientProcessor.GenerateLists(requestID, network, md, outputPaths, format);
        }

        IDictionary<Guid,string> IPatientIdentifierProcessor.GetQueryIdentifiers()
        {
            return _patientProcessor.GetQueryIdentifiers();
        }

        void IPatientIdentifierProcessor.SetPatientIdentifierSources(IDictionary<Guid, string> inputPaths)
        {
            _patientProcessor.SetPatientIdentifierSources(inputPaths);
        }
    }
}
