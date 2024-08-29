using System.ServiceModel;
namespace Lpp.Dns.Portal
{
    [ServiceContract( Namespace="http://lincolnpeak.com/schemas/DNS4/GarbageCollection" )]
    public interface IGarbageCollectionWcfService
    {
        [OperationContract]
        void CollectGarbage();
    }
}