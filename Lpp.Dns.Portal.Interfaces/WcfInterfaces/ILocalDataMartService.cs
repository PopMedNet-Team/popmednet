using System.ServiceModel;
namespace Lpp.Dns.Portal
{
    [ServiceContract( Namespace="http://lincolnpeak.com/schemas/DNS4/LocalDataMart" )]
    public interface ILocalDataMartWcfService
    {
        [OperationContract]
        void ProcessRequests();
    }
}